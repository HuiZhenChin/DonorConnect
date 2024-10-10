using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                        LoadDonationOrder("Recommended", "", "", "DESC", "Car");

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

            // LoadDonationOrder method with the selected filters
            LoadDonationOrder(recommendedFilter, pickupStateFilter, destinationStateFilter, earningsFilter, vehicleTypeFilter);
        }

        protected void gvAllDonations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrder")
            {
                // get the deliveryId from the CommandArgument
                string deliveryId = e.CommandArgument.ToString();

                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

                // find the panel in the current row and set it to be visible
                Panel pnlOrderInfo = (Panel)row.FindControl("pnlOrderInfo");
                pnlOrderInfo.Attributes.CssStyle.Add("display", "block");

                QRY _Qry = new QRY();
                string donationQuery = "SELECT donationId, orgId FROM delivery WHERE deliveryId = @deliveryId";
                var donationParams = new Dictionary<string, object> { { "@deliveryId", deliveryId } };
                DataTable deliveryData = _Qry.GetData(donationQuery, donationParams);

                if (deliveryData.Rows.Count > 0)
                {
                    string donationId = deliveryData.Rows[0]["donationId"].ToString();
                    string orgId = deliveryData.Rows[0]["orgId"].ToString();

                    // fetch donor information from donation_item_request table
                    string donorQuery = "SELECT donorFullName, donorPhone FROM donation_item_request WHERE donationId = @donationId";
                    var donorParams = new Dictionary<string, object> { { "@donationId", donationId } };
                    DataTable donorData = _Qry.GetData(donorQuery, donorParams);

                    if (donorData.Rows.Count > 0)
                    {
                        Label lblDonorName = (Label)row.FindControl("lblDonorName");
                        Label lblDonorPhone = (Label)row.FindControl("lblDonorPhone");
                        lblDonorName.Text = donorData.Rows[0]["donorFullName"].ToString();
                        lblDonorPhone.Text = donorData.Rows[0]["donorPhone"].ToString();
                    }

                    // fetch organization information from the organization table using orgId
                    string orgQuery = "SELECT orgName, orgContactNumber, picContactNumber FROM organization WHERE orgId = @orgId";
                    var orgParams = new Dictionary<string, object> { { "@orgId", orgId } };
                    DataTable orgData = _Qry.GetData(orgQuery, orgParams);

                    if (orgData.Rows.Count > 0)
                    {
                        Label lblOrgName = (Label)row.FindControl("lblOrgName");
                        Label lblOrgPhone = (Label)row.FindControl("lblOrgPhone");
                        lblOrgName.Text = orgData.Rows[0]["orgName"].ToString();
                        lblOrgPhone.Text = orgData.Rows[0]["orgContactNumber"].ToString() + " / " + orgData.Rows[0]["picContactNumber"].ToString();
                    }

                    // fetch the donation items based on the donationId
                    string query2 = "SELECT itemCategory, item, quantityDonated FROM donation_item_request WHERE donationId = @donationId";
                    var parameters = new Dictionary<string, object>
                    {
                        { "@donationId", donationId }
                    };
                    DataTable dt2 = _Qry.GetData(query2, parameters);
                  
                    if (dt2.Rows.Count > 0)
                    {
                        // creating a table
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<table id='categoryDetailsTable' style='margin: auto;'>");
                        sb.Append("<thead><tr><th>Category</th><th>Item</th><th>Quantity</th></tr></thead>");
                        sb.Append("<tbody>");

                        foreach (DataRow row2 in dt2.Rows)
                        {
                            string category = row2["itemCategory"].ToString();
                            string items = row2["item"].ToString();
                            string quantities = row2["quantityDonated"].ToString();

                           
                            // split items and quantities by commas if needed
                            quantities = quantities.Replace("(", "").Replace(")", "");
                            string[] itemArray = items.Split(',');
                            string[] qtyArray = quantities.Split(',');

                            for (int i = 0; i < itemArray.Length; i++)
                            {
                                sb.Append("<tr>");
                                sb.AppendFormat("<td>{0}</td>", category);  // display category in each row
                                sb.AppendFormat("<td>{0}</td>", itemArray[i].Trim());  // display each item
                                sb.AppendFormat("<td>{0}</td>", i < qtyArray.Length ? qtyArray[i].Trim() : "N/A");  // display each quantity or "N/A" if missing
                                sb.Append("</tr>");
                            }
                        }

                        sb.Append("</tbody></table>");

                  
                        PlaceHolder phDonationItems = (PlaceHolder)row.FindControl("phDonationItems");
                        phDonationItems.Controls.Add(new Literal { Text = sb.ToString() });
                    }
                }
            }
        }




    }
}