using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class UpdateDelivery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["username"].ToString() != null)
                {
                    if (Session["role"].ToString() == "rider")
                    {

                        LoadDelivery("Accepted");
                    }
                }
            }
        }

        protected void LoadDelivery_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string status = btn.CommandArgument;  

            // selected status
            LoadDelivery(status);
          
            lnkToPickUp.CssClass = "nav-link";
            lnkToReach.CssClass = "nav-link";
            lnkCompleted.CssClass = "nav-link";


            btn.CssClass = "nav-link active";
        }

        private void LoadDelivery(string status)
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();

            string username= Session["username"].ToString();

            string riderId= GetRiderId(username);

            string strSQL = @"
             SELECT d.deliveryId, d.pickupDate, d.pickupTime, d.pickupAddress, d.destinationAddress, d.paymentAmount, d.vehicleType, 
                    dir.donationPublishId, dir.totalDistance,  
                    dir.[state] AS PickupState, dp.donationState AS DestinationState, dp.urgentStatus,
                    CONCAT(dir.[state], ' -> ', dp.donationState) AS state
             FROM [delivery] d
             INNER JOIN [donation_item_request] dir ON d.donationId = dir.donationId  
             INNER JOIN [donation_publish] dp ON dir.donationPublishId = dp.donationPublishId
             WHERE d.deliveryStatus = @status
             AND d.riderId = @riderId
             ORDER BY d.pickupDate";


            var parameters = new Dictionary<string, object>
            {
                { "@riderId", riderId },
                { "status", status }
            };

            // fetch filtered data 
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

                noDataLabel.Visible = false;
                gvAllDonations.DataSource = processedTable;
                gvAllDonations.DataBind();
            }
            else
            {
                noDataLabel.Visible = true;
                gvAllDonations.DataSource = null;
                gvAllDonations.DataBind();
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

        protected void btnViewDelivery_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string deliveryId = btn.CommandArgument;

            Response.Redirect($"UpdateDelivery.aspx?deliveryId={deliveryId}");

        }

    }
}