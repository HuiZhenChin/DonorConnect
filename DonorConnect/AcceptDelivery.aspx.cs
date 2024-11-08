using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DonorConnect
{
    public partial class AcceptDelivery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"].ToString() != null)
                {
                    if (Session["role"].ToString() == "rider")
                    {
                       LoadDonationOrder("Recommended", "", "", "DESC", "");

                    }
                }
            }
        }


        private void LoadDonationOrder(string recommendedFilter = "", string pickupStateFilter = "", string destinationStateFilter = "", string earningsFilter = "", string vehicleTypeFilter = "")
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();

            string strSQL = @"
            SELECT d.deliveryId, d.pickupDate, d.pickupTime, d.pickupAddress, d.destinationAddress, d.paymentAmount, d.vehicleType, 
                   dir.donationPublishId, dir.totalDistance,  
                   dir.[state] AS PickupState, dp.donationState AS DestinationState, dp.urgentStatus,
                   CONCAT(dir.[state], ' -> ', dp.donationState) AS state
            FROM [delivery] d
            INNER JOIN [donation_item_request] dir ON d.donationId = dir.donationId  
            INNER JOIN [donation_publish] dp ON dir.donationPublishId = dp.donationPublishId
            WHERE d.deliveryStatus = 'Waiting for delivery rider'";

            var parameters = new Dictionary<string, object>();

            // apply the "Recommended" filter
            if (!string.IsNullOrEmpty(recommendedFilter) && recommendedFilter == "Recommended")
            {
                string username = Session["username"]?.ToString(); // get rider username
                if (!string.IsNullOrEmpty(username))
                {
                    // get the rider's region and vehicle type from the delivery_rider table
                    string riderQuery = "SELECT riderRegion, vehicleType FROM delivery_rider WHERE riderUsername = @username";
                    var riderParam = new Dictionary<string, object> { { "@username", username } };
                    DataTable riderPreferences = _Qry.GetData(riderQuery, riderParam);

                    if (riderPreferences.Rows.Count > 0)
                    {
                        string riderRegion = riderPreferences.Rows[0]["riderRegion"].ToString();
                        string riderVehicleType = riderPreferences.Rows[0]["vehicleType"].ToString();

                        // filter by matching pickup state and destination state with rider's region, and vehicle type
                        strSQL += " AND dir.[state] = @riderRegion AND dp.donationState = @riderRegion";
                        strSQL += " AND d.vehicleType = @riderVehicleType";


                        parameters.Add("@riderRegion", riderRegion);
                        parameters.Add("@riderVehicleType", riderVehicleType);
                    }
                }
            }

            // apply Pickup State filter
            if (!string.IsNullOrEmpty(pickupStateFilter))
            {
                strSQL += " AND dir.[state] = @pickupState";
                parameters.Add("@pickupState", pickupStateFilter);
            }

            // apply Destination State filter
            if (!string.IsNullOrEmpty(destinationStateFilter))
            {
                strSQL += " AND dp.donationState = @destinationState";
                parameters.Add("@destinationState", destinationStateFilter);
            }

            // apply Vehicle Type filter
            if (!string.IsNullOrEmpty(vehicleTypeFilter))
            {
                strSQL += " AND d.vehicleType = @vehicleType";
                parameters.Add("@vehicleType", vehicleTypeFilter);
            }

            // apply Earnings filter (Highest to Lowest or Lowest to Highest)
            if (!string.IsNullOrEmpty(earningsFilter))
            {
                // remove 'RM' and sort numerically
                strSQL += " ORDER BY CAST(REPLACE(d.paymentAmount, 'RM ', '') AS DECIMAL(18,2)) " + earningsFilter;
            }
            else
            {
                // default ordering: Urgent donations first, then by created date
                strSQL += " ORDER BY CASE WHEN dp.urgentStatus = 'Yes' THEN 1 ELSE 2 END, dp.created_on ASC";
            }

            // fetch filtered data based on the query and parameters
            _dt = _Qry.GetData(strSQL, parameters);

            if (_dt.Rows.Count > 0)
            {
                DataTable processedTable = new DataTable();
                processedTable.Columns.Add("deliveryId");
                processedTable.Columns.Add("donationPublishId");
                processedTable.Columns.Add("urgentStatus");
                processedTable.Columns.Add("state");
                processedTable.Columns.Add("pickupDate");
                processedTable.Columns.Add("pickupTime");
                processedTable.Columns.Add("pickupAddress");
                processedTable.Columns.Add("destinationAddress");
                processedTable.Columns.Add("paymentAmount");
                processedTable.Columns.Add("vehicleType");
                processedTable.Columns.Add("urgentLabel");
                processedTable.Columns.Add("cardBody");
                processedTable.Columns.Add("totalDistance");

                foreach (DataRow row in _dt.Rows)
                {
                    DataRow newRow = processedTable.NewRow();
                    newRow["deliveryId"] = row["deliveryId"];
                    newRow["donationPublishId"] = row["donationPublishId"];
                    newRow["urgentStatus"] = row["urgentStatus"];
                    newRow["state"] = row["state"];
                    newRow["pickupDate"] = row["pickupDate"];
                    newRow["pickupTime"] = row["pickupTime"];
                    newRow["pickupAddress"] = row["pickupAddress"];
                    newRow["destinationAddress"] = row["destinationAddress"];
                    newRow["paymentAmount"] = row["paymentAmount"];
                    newRow["vehicleType"] = row["vehicleType"];
                    newRow["totalDistance"] = row["totalDistance"];

                    bool isUrgent = row["urgentStatus"].ToString().ToLower() == "yes";
                    newRow["urgentLabel"] = isUrgent
                        ? "<div style='color: white; background: red; font-weight: bold; text-align: left; padding: 5px 10px; height: 50px; display: flex; align-items: center; width: fit-content'><i class='fa fa-bullhorn' aria-hidden='true' style='margin-right: 8px;'></i>URGENT!</div>"
                        : "";

                    processedTable.Rows.Add(newRow);
                }

                gvAllDonations.DataSource = processedTable;
                gvAllDonations.DataBind();
            }
            else
            {
                gvAllDonations.DataSource = null;
                gvAllDonations.DataBind();
            }
        }


        protected void btnFilter_Click(object sender, EventArgs e)
        {
            // get filter values from dropdowns
            string recommendedFilter = ddlRecommendedAll.SelectedValue;
            string pickupStateFilter = ddlPickupState.SelectedValue;
            string destinationStateFilter = ddlDestinationState.SelectedValue;
            string earningsFilter = ddlEarnings.SelectedValue;
            string vehicleTypeFilter = ddlVehicleType.SelectedValue;

            // selected filters
            LoadDonationOrder(recommendedFilter, pickupStateFilter, destinationStateFilter, earningsFilter, vehicleTypeFilter);
        }

        protected void gvAllDonations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrder")
            {
                // get the deliveryId from frontend
                string deliveryId = e.CommandArgument.ToString();
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                Panel pnlOrderInfo = (Panel)row.FindControl("pnlOrderInfo");
                Button btnViewOrder = (Button)row.FindControl("btnViewOrder");
                GridView gvDonationItems = (GridView)row.FindControl("gvDonationItems");

                string currentlyOpenPanelId = ViewState["OpenPanel"] != null ? ViewState["OpenPanel"].ToString() : null;

                if (currentlyOpenPanelId == pnlOrderInfo.ClientID)
                {
                    pnlOrderInfo.Attributes.CssStyle.Add("display", "none");
                    ViewState["OpenPanel"] = null;

                    btnViewOrder.Text = "View Order ->";
                    btnViewOrder.CssClass = "btn btn-success";
                }
                else
                {
                    // show the clicked panel
                    pnlOrderInfo.Attributes.CssStyle.Add("display", "block");

                    ViewState["OpenPanel"] = pnlOrderInfo.ClientID;

                    btnViewOrder.Text = "<- Close Order";
                    btnViewOrder.CssClass = "btn btn-danger";
                    string donationPublishId = "";

                    // populate order details
                    QRY _Qry = new QRY();
                    string donationQuery = "SELECT donationId, orgId, noteRider FROM delivery WHERE deliveryId = @deliveryId";
                    var donationParams = new Dictionary<string, object> { { "@deliveryId", deliveryId } };
                    DataTable deliveryData = _Qry.GetData(donationQuery, donationParams);

                    if (deliveryData.Rows.Count > 0)
                    {
                        string donationId = deliveryData.Rows[0]["donationId"].ToString();
                        string orgId = deliveryData.Rows[0]["orgId"].ToString();
                        Label lblNote = (Label)row.FindControl("lblNote");
                        lblNote.Text = string.IsNullOrEmpty(deliveryData.Rows[0]["noteRider"].ToString()) ? "-" : deliveryData.Rows[0]["noteRider"].ToString();

                        // fetch donor information
                        string donorQuery = "SELECT distinct donorFullName, donorPhone, donationPublishId FROM donation_item_request WHERE donationId = @donationId";
                        var donorParams = new Dictionary<string, object> { { "@donationId", donationId } };
                        DataTable donorData = _Qry.GetData(donorQuery, donorParams);

                        if (donorData.Rows.Count > 0)
                        {
                            Label lblDonorName = (Label)row.FindControl("lblDonorName");
                            Label lblDonorPhone = (Label)row.FindControl("lblDonorPhone");
                           
                            lblDonorName.Text = donorData.Rows[0]["donorFullName"].ToString();
                            lblDonorPhone.Text = donorData.Rows[0]["donorPhone"].ToString();
                            donationPublishId = donorData.Rows[0]["donationPublishId"].ToString();
                          

                        }

                        // fetch organization information
                        string orgQuery = "SELECT recipientName, recipientPhoneNumber FROM donation_publish WHERE donationPublishId = @donationPublishId";
                        var orgParams = new Dictionary<string, object> { { "@donationPublishId", donationPublishId } };
                        DataTable orgData = _Qry.GetData(orgQuery, orgParams);

                        if (orgData.Rows.Count > 0)
                        {
                            Label lblOrgName = (Label)row.FindControl("lblOrgName");
                            Label lblOrgPhone = (Label)row.FindControl("lblOrgPhone");
                            lblOrgName.Text = orgData.Rows[0]["recipientName"].ToString();
                            lblOrgPhone.Text = orgData.Rows[0]["recipientPhoneNumber"].ToString();
                        }

                        // fetch the donation items based on donationId 
                        string query2 = "SELECT itemCategory, item, quantityDonated FROM donation_item_request WHERE donationId = @donationId";
                        var parameters = new Dictionary<string, object> { { "@donationId", donationId } };
                        DataTable dt2 = _Qry.GetData(query2, parameters);

                        if (dt2.Rows.Count > 0)
                        {
                            gvDonationItems.DataSource = dt2;
                            gvDonationItems.DataBind();
                        }
                    }
                }
            }
        }

       
        protected void btnAcceptOrder_Click(object sender, EventArgs e)
        {
            Button btnAcceptOrder = (Button)sender;

            string deliveryId = btnAcceptOrder.CommandArgument;

            QRY _Qry = new QRY();

            string username = Session["username"].ToString();
            string riderId = GetRiderId(username);

            // fetch rider's vehicle type from the delivery_rider table
            string riderVehicleTypeQuery = "SELECT vehicleType FROM delivery_rider WHERE riderId = @riderId";
            var riderParams = new Dictionary<string, object> { { "@riderId", riderId } };
            DataTable riderData = _Qry.GetData(riderVehicleTypeQuery, riderParams);
            string riderVehicleType = null;

            if (riderData.Rows.Count > 0)
            {
                riderVehicleType = riderData.Rows[0]["vehicleType"].ToString();
            }
            else
            {

                riderVehicleType = null;
            }

            // fetch delivery's vehicle type from the delivery table
            string deliveryVehicleTypeQuery = "SELECT vehicleType FROM delivery WHERE deliveryId = @deliveryId";
            var deliveryParams = new Dictionary<string, object> { { "@deliveryId", deliveryId } };
            DataTable deliveryData = _Qry.GetData(deliveryVehicleTypeQuery, deliveryParams);
            string deliveryVehicleType = null;

            if (deliveryData.Rows.Count > 0)
            {
                deliveryVehicleType = deliveryData.Rows[0]["vehicleType"].ToString();
            }
            else
            {

                deliveryVehicleType = null;
            }

            // compare vehicle type
            if (riderVehicleType != null && deliveryVehicleType != null)
            {
                if (riderVehicleType == deliveryVehicleType)
                {
                    // if vehicle types are the same, proceed to accept the order
                    updateDb(deliveryId);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmAccept",
                    $"confirmAccept('{deliveryId}');", true);
                }
            }
        }

        [WebMethod]
        public static string AcceptOrder(string deliveryId)
        {
            return updateDb(deliveryId);
        }

        protected static string updateDb(string deliveryId)
        {
            try
            {
                QRY _Qry = new QRY();

                string username = HttpContext.Current.Session["username"].ToString();
                string riderId = GetRiderId(username);

                DateTime acceptTime = DateTime.Now;

                // update the delivery table to accept the order
                string updateQuery = "UPDATE delivery " +
                                     "SET riderId = @riderId, " +
                                     "acceptTimeByRider = @acceptTimeByRider, " +
                                     "deliveryStatus = @deliveryStatus " +
                                     "WHERE deliveryId = @deliveryId";

                var updateParams = new Dictionary<string, object>
                {
                    { "@riderId", riderId },
                    { "@acceptTimeByRider", acceptTime },
                    { "@deliveryStatus", "Accepted" },
                    { "@deliveryId", deliveryId }
                };

                bool updateSuccess = _Qry.ExecuteNonQuery(updateQuery, updateParams);

                // if successful
                if (updateSuccess)
                {
                    QRY _Qry2 = new QRY();
                    string message = "Your donation delivery status has been updated to " + "Accepted" + ".";
                    string donationId = GetDonationId(deliveryId);
                    string donorId = GetDonorId(deliveryId);
                    string link = $"Delivery.aspx?donationId={donationId}";

                    string encryptedLink = Encryption.Encrypt(link);

                    string sqlNtf = "EXEC [create_notifications] " +
                                    "@method = 'INSERT', " +
                                    "@id = NULL, " +
                                    "@userId = @userId, " +
                                    "@link = @link, " +
                                    "@content = @content";

                    var notificationParameter = new Dictionary<string, object>
                    {
                        { "@userId", donorId },
                        { "@link", encryptedLink },
                        { "@content", message }
                    };

                    _Qry2.ExecuteNonQuery(sqlNtf, notificationParameter);

                    string fullLink = "https://localhost:44390/Delivery.aspx?donationId=" + donationId;

                    // send email to notify organization
                    string sqlemail = "EXEC [application_email] " +
                                      "@action = 'DONATION DELIVERY UPDATE ACCEPTED', " +
                                      "@role = 'donor', " +
                                      "@resubmitlink = @link, " +
                                      "@donorId = @donorId, " +
                                      "@donationpublishid = @donationPublishId";

                    var emailParameter = new Dictionary<string, object>
                    {
                        { "@link", fullLink },
                        { "@donorId", donorId },
                        { "@donationPublishId", donationId }
                    };

                    _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "alert",
                    "Swal.fire({ " +
                    "title: 'Accepted!', " +
                    "text: 'Successful! Please visit your Dashboard to view the location navigation and status update of the order. Note that you will only receive the money earned in the DonorConnect wallet when you have delivered the item to the destination (status is Completed).', " +
                    "icon: 'success', " +
                    "confirmButtonText: 'OK' " +
                    "}).then(function() { " +
                    "window.location.href = 'Home.aspx'; " +
                    "});", true);


                    return "success";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "alert",
                    "Swal.fire('Error!', 'There was an error accepting the order. Please try again!', 'error');", true);

                    return "error";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "alert",
                    $"Swal.fire('Error!', 'Error: {ex.Message}', 'error');", true);

                return "error: " + ex.Message;
            }
        }



        public static string GetRiderId(string username)
        {


            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username is missing.");
            }

            string sql = "SELECT riderId FROM delivery_rider WHERE riderUsername = @username";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@username", username }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["riderId"].ToString();
            }
            else
            {
                throw new Exception($"No record found for username: {username}");
            }

        }

        public static string GetDonationId(string deliveryId)
        {
            string donationId = string.Empty;

            QRY _Qry = new QRY();

            string sql = "SELECT donationId FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                donationId = dt.Rows[0]["donationId"].ToString();
            }

            return donationId;
        }
        public static string GetDonorId(string deliveryId)
        {
            string donorId = string.Empty;

            QRY _Qry = new QRY();

            string sql = "SELECT donorId FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                donorId = dt.Rows[0]["donorId"].ToString();
            }

            return donorId;
        }

        protected void btnMap_Click(object sender, EventArgs e)
        {
            //LoadDonationOrder("All", "", "", "", "");
            // set dropdown list to "All"
            ddlRecommendedAll.SelectedIndex = 1;

            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showInfo('By opening the map, the search results will be refreshed from \"Recommended\" to \"All\".', 'error');", true);

        }

        [WebMethod]
        public static string GetDestinationAddress(string deliveryId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            string sql = "SELECT destinationAddress FROM delivery WHERE deliveryId = @deliveryId";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };
            _dt = _Qry.GetData(sql, parameter);


            if (_dt.Rows.Count > 0)
            {
               
                string address = _dt.Rows[0]["destinationAddress"].ToString();

                return address;

            }

            // if no records found or any issues
            return null;
        }

        [WebMethod]
        public static string GetPickupAddress2(string deliveryId)
        {
            if (string.IsNullOrEmpty(deliveryId))
            {
                throw new Exception("Delivery Id is missing.");
            }

            string sql = "SELECT pickupAddress FROM delivery WHERE deliveryId = @deliveryId";

            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                string pickupAddress = dt.Rows[0]["pickupAddress"].ToString();


                return pickupAddress;
            }
            else
            {
                throw new Exception($"No record found for deliveryId: {deliveryId}");
            }
        }

        [WebMethod]
        public static List<Dictionary<string, string>> GetPickupAddress()
        {
            List<Dictionary<string, string>> address = new List<Dictionary<string, string>>();

            QRY _Qry = new QRY();

            string sql = @"
           SELECT d.deliveryID, d.pickupAddress, d.destinationAddress, dir.donorFullName, dir.donorPhone, d.donationId 
           FROM delivery d
           INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
           WHERE d.deliveryStatus = 'Waiting for delivery rider'";

            DataTable _dt = _Qry.GetData(sql);

            foreach (DataRow row in _dt.Rows)
            {
                var donor = new Dictionary<string, string>
            {
                { "deliveryId", row["deliveryId"].ToString() },
                { "donorName", row["donorFullName"].ToString() },
                { "pickupAddress", row["pickupAddress"].ToString() },
                { "destinationAddress", row["destinationAddress"].ToString() },
                { "donorPhone", row["donorPhone"].ToString() },
            };
                address.Add(donor);
            }

            return address;
        }

    }
}