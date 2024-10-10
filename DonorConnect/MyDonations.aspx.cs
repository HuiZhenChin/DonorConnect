using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class MyDonations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                LoadDonations("All"); // Load Pending donations by default
                LoadDonationCount();
            }
        }

        protected void LoadDonations_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string status = btn.CommandArgument;  // Get the status from the CommandArgument

            // Call LoadDonations function with the selected status
            LoadDonations(status);

            lnkAll.CssClass = "nav-link";
            lnkPending.CssClass = "nav-link";
            lnkToPay.CssClass = "nav-link";
            lnkToAccept.CssClass = "nav-link";
            lnkToPickUp.CssClass = "nav-link";
            lnkToReach.CssClass = "nav-link";
            lnkCompleted.CssClass = "nav-link";

          
            btn.CssClass = "nav-link active";
        }

        protected void LoadDonations(string status)
        {
            string username = Session["username"].ToString();
            string donorId = GetDonorId(username);


            QRY _Qry = new QRY();
            DataTable dtDonations = new DataTable();
            string sqlQuery = "";

            if (status == "All")
            {
                // Query to fetch all donations from any status, ordered by created_on
                sqlQuery = @"
                SELECT 
                    dir.donationId, 
                    dir.pickUpAddress, 
                    dp.address AS destinationAddress, 
                    CASE 
                        WHEN dir.requestStatus IN ('Pending', 'To Pay') THEN NULL 
                        ELSE d.pickupDate 
                    END AS pickupDate, 
                    CASE 
                        WHEN dir.requestStatus IN ('Pending', 'To Pay') THEN NULL 
                        ELSE d.pickupTime 
                    END AS pickupTime, 
                    org.orgName, 
                    dir.requestStatus AS status,
                    dir.created_on
                FROM 
                    donation_item_request dir
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId
                INNER JOIN 
                    donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                LEFT JOIN 
                    delivery d ON dir.donationId = d.donationId 
                WHERE 
                    dir.donorId = @donorId
                ORDER BY 
                    dir.created_on DESC;

                ";
            }


            else if (status == "Pending")
            {
                // Pending donations, excluding pickupDate and pickupTime
                sqlQuery = @"
                SELECT dir.donationId, dir.pickUpAddress, dp.address AS destinationAddress, NULL AS pickupDate, NULL AS pickupTime, 
                org.orgName, 'Pending' AS status
                FROM donation_item_request dir
                INNER JOIN organization org ON dir.orgId = org.orgId
                INNER JOIN donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                WHERE dir.requestStatus = 'Pending' AND dir.donorId = @donorId

                ";
            }

            else if (status == "To Pay")
            {
                // To Pay donations, with pickupDate and pickupTime included
                sqlQuery = @"
                SELECT dir.donationId, dir.pickUpAddress, dp.address AS destinationAddress, NULL AS pickupDate, NULL AS pickupTime, 
                org.orgName, 'To Pay' AS status
                FROM donation_item_request dir
                INNER JOIN organization org ON dir.orgId = org.orgId
                INNER JOIN donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                WHERE dir.requestStatus = 'Approved' AND dir.paymentStatus IS NULL AND dir.donorId = @donorId

                ";
            }

            else if (status == "To Accept")
            {
                // To Accept donations from the delivery table
                sqlQuery = @"
                SELECT d.donationId, d.pickUpAddress, d.destinationAddress, d.pickupDate, d.pickupTime, org.orgName, 
                'To Accept' AS status FROM delivery d 
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId 
                INNER JOIN organization org ON dir.orgId = org.orgId 
                WHERE d.deliveryStatus = 'Waiting for delivery rider' AND d.donorId = @donorId";
            }
            else if (status == "To PickUp")
            {
                // To PickUp donations
                sqlQuery = @"
                SELECT d.donationId, d.pickUpAddress, d.destinationAddress, d.pickupDate, d.pickupTime, org.orgName, 
                'To PickUp' AS status FROM delivery d 
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId 
                INNER JOIN organization org ON dir.orgId = org.orgId    
                WHERE d.deliveryStatus = 'Accepted' AND d.donorId = @donorId";
            }
            else if (status == "To Reach")
            {
                // To Reach donations
                sqlQuery = @"
                SELECT d.donationId, d.pickUpAddress, d.destinationAddress, d.pickupDate, d.pickupTime, org.orgName, 
                'To Reach' AS status FROM delivery d
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN organization org ON dir.orgId = org.orgId
                WHERE d.deliveryStatus = 'Delivering in Progress' AND d.donorId = @donorId";
            }
            else if (status == "Completed")
            {
                // Completed donations
                sqlQuery = @"
                SELECT d.donationId, d.pickUpAddress, d.destinationAddress, d.pickupDate, d.pickupTime, org.orgName, 
                'Completed' AS status FROM delivery d 
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN organization org ON dir.orgId = org.orgId
                WHERE d.deliveryStatus = 'Reached Destination' AND d.donorId = @donorId";
            }

            

            // Replace with your data access method (e.g., ADO.NET, EF, etc.)
            var parameters = new Dictionary<string, object>
            {
                { "@donorId", donorId }
            };

            // Assuming you have a method to fetch data based on the query and parameters
            dtDonations = _Qry.GetData(sqlQuery, parameters);

            // Bind the fetched data to the GridView
            gvDonations.DataSource = dtDonations;
            gvDonations.DataBind();
        }

        public static string GetDonorId(string username)
        {


            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username is missing.");
            }

            string sql = "SELECT donorId FROM donor WHERE donorUsername = @username";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@username", username }
            };

            // Execute the SQL query to fetch the ID
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["donorId"].ToString();
            }
            else
            {
                throw new Exception($"No record found for username: {username}");
            }

        }

        protected void gvDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Check if the current row is a data row (not a header or footer)
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the donationId for the current row
                string donationId = DataBinder.Eval(e.Row.DataItem, "donationId").ToString();

                // Find the PlaceHolder control inside the current row
                PlaceHolder phDonationItems = (PlaceHolder)e.Row.FindControl("phDonationItems");

                // Call the method to load the donation items for this donationId
                LoadDonationItems(donationId, phDonationItems);

                //QRY _Qry = new QRY();
                //string qrSql = "SELECT qrCode FROM delivery WHERE donationId = @donationId";
                //var qrParams = new Dictionary<string, object>
                //{
                //    { "@donationId", donationId }
                //};

                //DataTable qrData = _Qry.GetData(qrSql, qrParams);

                //if (qrData.Rows.Count > 0)
                //{
                //    string qrBase64 = qrData.Rows[0]["qrCode"].ToString();

                //    // Find the QR code Panel (if required) and image control (or bind it directly in the template)
                //    Panel pnlQRCode = (Panel)e.Row.FindControl("pnlQRCode");
                //    if (pnlQRCode != null)
                //    {
                //        // Ensure the panel is visible if not already set by the Visible property
                //        pnlQRCode.Visible = true;

                //        // Add QR code as image base64
                //        System.Web.UI.WebControls.Image imgQRCode = new System.Web.UI.WebControls.Image();
                //        imgQRCode.ImageUrl = "data:image/png;base64," + qrBase64;
                //        imgQRCode.AlternateText = "QR Code";
                //        imgQRCode.Width = Unit.Pixel(150);
                //        imgQRCode.Height = Unit.Pixel(150);

                //        // Add the image to the QR code panel
                //        pnlQRCode.Controls.Add(imgQRCode);
                //    }
                //}
            }
        }


        private void LoadDonationItems(string donationId, PlaceHolder phDonationItems)
        {
            // Updated query to fetch the donation items based on the donationId
            string query = @"
            WITH SplitItems AS (
                SELECT 
                    dir.donationId,
                    dir.itemCategory,
                    TRIM(value) AS item,
                    ROW_NUMBER() OVER (PARTITION BY dir.donationId, dir.itemCategory ORDER BY (SELECT NULL)) AS rowNum
                FROM 
                    donation_item_request dir
                CROSS APPLY 
                    STRING_SPLIT(dir.item, ',')
            ),
            SplitQuantities AS (
                SELECT 
                    dir.donationId,
                    dir.itemCategory,
                    TRIM(REPLACE(value, '(', '')) AS quantityDonated,
                    ROW_NUMBER() OVER (PARTITION BY dir.donationId, dir.itemCategory ORDER BY (SELECT NULL)) AS rowNum
                FROM 
                    donation_item_request dir
                CROSS APPLY 
                    STRING_SPLIT(REPLACE(dir.quantityDonated, ')', ''), ',')
            )
            SELECT 
                si.itemCategory,
                si.item,
                sq.quantityDonated,
                die.expiryDate,
                die.quantityWithSameExpiryDate
            FROM 
                SplitItems si
            JOIN 
                SplitQuantities sq
                ON si.donationId = sq.donationId 
                AND si.itemCategory = sq.itemCategory
                AND si.rowNum = sq.rowNum
            LEFT JOIN 
                donation_item_expiry_date die 
                ON si.donationId = die.donationId 
                AND si.itemCategory = die.itemCategory
                AND si.item = die.item2
            WHERE 
                si.donationId = @donationId
            ORDER BY 
                si.itemCategory, si.item, die.expiryDate";

            // Assuming QRY is a database utility class that executes the query
            QRY qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };
            DataTable dt = qry.GetData(query, parameters);

            if (dt.Rows.Count > 0)
            {
                // Start creating a table
                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='categoryDetailsTable' style='margin: auto;'>");
                sb.Append("<thead><tr><th>Category</th><th>Item</th><th>Quantity</th><th>Expiry Date (Quantity)</th></tr></thead>");
                sb.Append("<tbody>");

                string currentCategory = "";

                foreach (DataRow row in dt.Rows)
                {
                    string category = row["itemCategory"].ToString();
                    string item = row["item"].ToString();
                    string expiryDate = row["expiryDate"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["expiryDate"]).ToString("yyyy-MM-dd");
                    string expiryQuantity = row["quantityWithSameExpiryDate"] == DBNull.Value ? "N/A" : row["quantityWithSameExpiryDate"].ToString();

                    // If there is no expiry date, show the donated quantity from the original request
                    string quantityToDisplay = expiryDate == "N/A" ? row["quantityDonated"].ToString() : expiryQuantity;

                    // Display category header once per category
                    if (currentCategory != category)
                    {
                        sb.AppendFormat("<tr><td colspan='5'><strong>{0}</strong></td></tr>", category);
                        currentCategory = category;
                    }

                    sb.Append("<tr>");
                    sb.AppendFormat("<td></td>");  // Empty for category
                    sb.AppendFormat("<td>{0}</td>", item);  // Display item
                    sb.AppendFormat("<td>{0}</td>", quantityToDisplay);  // Display the appropriate quantity
                    sb.AppendFormat("<td>{0}</td>", expiryDate);  // Display expiry date with quantity


                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");

                // Add the table HTML to the PlaceHolder control
                phDonationItems.Controls.Add(new Literal { Text = sb.ToString() });
            }
            else
            {
                // Optionally display a message if no items are found
                phDonationItems.Controls.Add(new Literal { Text = "<p>No items found for this donation.</p>" });
            }
        }

        [WebMethod]
        public static string GetQRCodeImage(string donationId)
        {
            QRY _Qry = new QRY();
            string qrSql = "SELECT qrCode FROM delivery WHERE donationId = @donationId";

            var qrParams = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            // Fetch the QR code from the database
            DataTable qrData = _Qry.GetData(qrSql, qrParams);

            if (qrData.Rows.Count > 0)
            {
                // Assuming the QR code is stored in base64 format
                string qrBase64 = qrData.Rows[0]["qrCode"].ToString();
                string qrCodeUrl = "data:image/png;base64," + qrBase64;
                return qrCodeUrl;  // Return the base64 image URL
            }
            else
            {
                return "";  // No QR code found
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string donationId = ((Button)sender).CommandArgument;

            QRY _Qry = new QRY();

            // Fetch the link from the database
            string paySql = "SELECT link FROM notifications WHERE supportingId = @donationId";
            var payParams = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            DataTable dt = _Qry.GetData(paySql, payParams);

            if (dt.Rows.Count > 0)
            {
                string link = dt.Rows[0]["link"].ToString();

                // Dynamically set the OnClientClick attribute to redirect to the link
                Button btnPay = (Button)sender;
                link = Encryption.Decrypt(link);
                btnPay.OnClientClick = "window.open('" + link + "', '_blank'); return false;";
            }
            else
            {
                // Handle case if the link is not found
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error: Link not found!');", true);
            }
        }

        protected string GetStatus(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return "status-pending";
                case "to pay":
                    return "status-to-pay";
                case "to accept":
                    return "status-to-accept";
                case "to pickup":
                    return "status-to-pickup";
                case "to reach":
                    return "status-to-reach";
                case "completed":
                    return "status-completed";
                case "rejected":
                    return "status-rejected";
                default:
                    return "";
            }
        }

        protected void LoadDonationCount()
        {
            // Example counts (replace with actual count logic from the database)
            int toPayCount = GetDonationCountByStatus("To Pay");
            int toAcceptCount = GetDonationCountByStatus("To Accept");
            int toPickUpCount = GetDonationCountByStatus("To PickUp");
            int toReachCount = GetDonationCountByStatus("To Reach");
            int completedCount = GetDonationCountByStatus("Completed");

            // Dynamically update the text of the LinkButtons to include the count in bold
            lnkToPay.Text = "To Pay" + (toPayCount > 0 ? $" (<strong>{toPayCount}</strong>)" : "");
            lnkToAccept.Text = "To Accept" + (toAcceptCount > 0 ? $" (<strong>{toAcceptCount}</strong>)" : "");
            lnkToPickUp.Text = "To PickUp" + (toPickUpCount > 0 ? $" (<strong>{toPickUpCount}</strong>)" : "");
            lnkToReach.Text = "To Reach" + (toReachCount > 0 ? $" (<strong>{toReachCount}</strong>)" : "");
            lnkCompleted.Text = "Completed" + (completedCount > 0 ? $" (<strong>{completedCount}</strong>)" : "");
        }




        private int GetDonationCountByStatus(string status)
        {
            string sqlQuery = "";
            Dictionary<string, object> parameter = new Dictionary<string, object>();

            string username = Session["username"].ToString(); 

            QRY _Qry = new QRY(); 

            
            string sql = "SELECT donorId FROM donor WHERE donorUsername = @username";         
            var donorParameter = new Dictionary<string, object>
            {
                { "@username", username }  
            };

          
            DataTable dt = _Qry.GetData(sql, donorParameter);

            string id = ""; 
            if (dt.Rows.Count > 0)
            {
               
                id = dt.Rows[0]["donorId"].ToString();
            }
            else
            {
                id = "";
            }


            if (status == "To Pay")
            {
                // SQL query to count 'To Pay' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM donation_item_request dir
                INNER JOIN organization org ON dir.orgId = org.orgId
                INNER JOIN donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                WHERE dir.requestStatus = 'Approved' 
                AND dir.paymentStatus IS NULL 
                AND dir.donorId = @donorId";

                parameter.Add("@donorId", id);  // Assuming you have donorId stored in the session
            }
            else if (status == "To Accept")
            {
                // SQL query to count 'To Accept' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN organization org ON dir.orgId = org.orgId
                WHERE d.deliveryStatus = 'Waiting for delivery rider' 
                AND d.donorId = @donorId";

                parameter.Add("@donorId", id);
            }
            else if (status == "To PickUp")
            {
                // SQL query to count 'To PickUp' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN organization org ON dir.orgId = org.orgId
                WHERE d.deliveryStatus = 'Accepted' 
                AND d.donorId = @donorId";

                parameter.Add("@donorId", id);
            }
            else if (status == "To Reach")
            {
                // SQL query to count 'To Reach' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN organization org ON dir.orgId = org.orgId
                WHERE d.deliveryStatus = 'Delivering in Progress' 
                AND d.donorId = @donorId";

                parameter.Add("@donorId", id);
            }
            else if (status == "Completed")
            {
                // SQL query to count 'Completed' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d
                INNER JOIN donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN organization org ON dir.orgId = org.orgId
                WHERE d.deliveryStatus = 'Reached Destination' 
                AND d.donorId = @donorId";

                parameter.Add("@donorId", id);
            }

            
            QRY _Qry2 = new QRY();  
            DataTable dt2 = _Qry2.GetData(sqlQuery, parameter);

            if (dt2.Rows.Count > 0)
            {
                return Convert.ToInt32(dt2.Rows[0][0]);  // Return the count
            }
            else
            {
                return 0;  // If no rows found, return 0
            }
        }

        protected void btnViewDelivery_Click(object sender, EventArgs e)
        {
            string donationId = ((Button)sender).CommandArgument;
          
            Response.Redirect($"Delivery.aspx?donationId={donationId}");

        }

    }

}