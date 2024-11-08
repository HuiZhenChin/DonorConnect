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
                LoadDonations("All"); 
                LoadDonationCount();
            }
        }

        protected void LoadDonations_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string status = btn.CommandArgument;  

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
                sqlQuery = @"
                SELECT 
                    dir.donationId, 
                    dir.pickUpAddress, 
                    dp.address AS destinationAddress, 
                    MAX(CASE 
                        WHEN dir.requestStatus IN ('Pending', 'To Pay') THEN NULL 
                        ELSE d.pickupDate 
                    END) AS pickupDate, 
                    MAX(CASE 
                        WHEN dir.requestStatus IN ('Pending', 'To Pay') THEN NULL 
                        ELSE d.pickupTime 
                    END) AS pickupTime, 
                    org.orgName, 
                    dir.requestStatus AS status,
                    STRING_AGG(dir.itemCategory, ', ') AS itemCategories,  
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
                GROUP BY 
                    dir.donationId, dir.pickUpAddress, dp.address, org.orgName, dir.requestStatus, dir.created_on
                ORDER BY 
                    dir.created_on DESC;
                ";
            }

            else if (status == "Pending")
            {
                sqlQuery = @"
                SELECT 
                    dir.donationId, 
                    dir.pickUpAddress, 
                    dp.address AS destinationAddress, 
                    NULL AS pickupDate, 
                    NULL AS pickupTime, 
                    org.orgName, 
                    'Pending' AS status
                FROM 
                    donation_item_request dir
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId
                INNER JOIN 
                    donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                WHERE 
                    dir.requestStatus = 'Pending' 
                    AND dir.donorId = @donorId
                GROUP BY 
                    dir.donationId, dir.pickUpAddress, dp.address, org.orgName;
                ";
            }
            else if (status == "To Pay")
            {
                sqlQuery = @"
                SELECT 
                    dir.donationId, 
                    dir.pickUpAddress, 
                    dp.address AS destinationAddress, 
                    NULL AS pickupDate, 
                    NULL AS pickupTime, 
                    org.orgName, 
                    'To Pay' AS status
                FROM 
                    donation_item_request dir
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId
                INNER JOIN 
                    donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                WHERE 
                    dir.requestStatus = 'Approved' 
                    AND dir.paymentStatus IS NULL 
                    AND dir.donorId = @donorId
                GROUP BY 
                    dir.donationId, dir.pickUpAddress, dp.address, org.orgName;
                ";
            }
            else if (status == "To Accept")
            {
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickUpAddress, 
                    d.destinationAddress, 
                    MAX(d.pickupDate) AS pickupDate, 
                    MAX(d.pickupTime) AS pickupTime, 
                    org.orgName, 
                    'To Accept' AS status
                FROM 
                    delivery d
                INNER JOIN 
                    donation_item_request dir ON d.donationId = dir.donationId 
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId 
                WHERE 
                    d.deliveryStatus = 'Waiting for delivery rider' 
                    AND d.donorId = @donorId
                GROUP BY 
                    d.donationId, d.pickUpAddress, d.destinationAddress, org.orgName;
                ";
            }
            else if (status == "To PickUp")
            {
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickUpAddress, 
                    d.destinationAddress, 
                    MAX(d.pickupDate) AS pickupDate, 
                    MAX(d.pickupTime) AS pickupTime, 
                    org.orgName, 
                    'To PickUp' AS status
                FROM 
                    delivery d
                INNER JOIN 
                    donation_item_request dir ON d.donationId = dir.donationId 
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId    
                WHERE 
                    d.deliveryStatus = 'Accepted' 
                    AND d.donorId = @donorId
                GROUP BY 
                    d.donationId, d.pickUpAddress, d.destinationAddress, org.orgName;
                ";
            }
            else if (status == "To Reach")
            {
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickUpAddress, 
                    d.destinationAddress, 
                    MAX(d.pickupDate) AS pickupDate, 
                    MAX(d.pickupTime) AS pickupTime, 
                    org.orgName, 
                    'To Reach' AS status
                FROM 
                    delivery d
                INNER JOIN 
                    donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId
                WHERE 
                    d.deliveryStatus = 'Delivering in Progress' 
                    AND d.donorId = @donorId
                GROUP BY 
                    d.donationId, d.pickUpAddress, d.destinationAddress, org.orgName;
                ";
            }
            else if (status == "Completed")
            {
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickUpAddress, 
                    d.destinationAddress, 
                    MAX(d.pickupDate) AS pickupDate, 
                    MAX(d.pickupTime) AS pickupTime, 
                    org.orgName, 
                    'Completed' AS status
                FROM 
                    delivery d 
                INNER JOIN 
                    donation_item_request dir ON d.donationId = dir.donationId
                INNER JOIN 
                    organization org ON dir.orgId = org.orgId
                WHERE 
                    d.deliveryStatus = 'Reached Destination' 
                    AND d.donorId = @donorId
                GROUP BY 
                    d.donationId, d.pickUpAddress, d.destinationAddress, org.orgName;
                ";
            }

            var parameters = new Dictionary<string, object>
            {
                { "@donorId", donorId }
            };

            dtDonations = _Qry.GetData(sqlQuery, parameters);

            if (dtDonations.Rows.Count == 0)
            {
                noDataLabel.Visible = true;
                gvDonations.Visible = false;
            }
            else
            {
                noDataLabel.Visible = false;
                gvDonations.Visible = true;
                gvDonations.DataSource = dtDonations;
                gvDonations.DataBind();
            }
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
     
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // get the donationId for the current row
                string donationId = DataBinder.Eval(e.Row.DataItem, "donationId").ToString();

                // find the PlaceHolder control inside the current row
                PlaceHolder phDonationItems = (PlaceHolder)e.Row.FindControl("phDonationItems");

                // call the method to load the donation items for this donationId
                LoadDonationItems(donationId, phDonationItems);

            }
        }


        private void LoadDonationItems(string donationId, PlaceHolder phDonationItems)
        {
            // fetch the donation items based on the donationId
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

            QRY qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };
            DataTable dt = qry.GetData(query, parameters);

            if (dt.Rows.Count > 0)
            {
             
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

                    // if there is no expiry date, show the donated quantity from the original request
                    string quantityToDisplay = expiryDate == "N/A" ? row["quantityDonated"].ToString() : expiryQuantity;

                    // display category header once per category
                    if (currentCategory != category)
                    {
                        sb.AppendFormat("<tr><td colspan='5'><strong>{0}</strong></td></tr>", category);
                        currentCategory = category;
                    }

                    sb.Append("<tr>");
                    sb.AppendFormat("<td></td>");  
                    sb.AppendFormat("<td>{0}</td>", item);  
                    sb.AppendFormat("<td>{0}</td>", quantityToDisplay); 
                    sb.AppendFormat("<td>{0}</td>", expiryDate); 


                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");

              
                phDonationItems.Controls.Add(new Literal { Text = sb.ToString() });
            }
            else
            {
               
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

            // fetch the QR code from the database
            DataTable qrData = _Qry.GetData(qrSql, qrParams);

            if (qrData.Rows.Count > 0)
            {
            
                string qrBase64 = qrData.Rows[0]["qrCode"].ToString();
                string qrCodeUrl = "data:image/png;base64," + qrBase64;
                return qrCodeUrl; 
            }
            else
            {
                return "";  // no QR code found
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            string donationId = ((Button)sender).CommandArgument;

            QRY _Qry = new QRY();

            // fetch the link from the database
            string paySql = "SELECT link FROM notifications WHERE supportingId = @donationId";
            var payParams = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            DataTable dt = _Qry.GetData(paySql, payParams);

            if (dt.Rows.Count > 0)
            {
                string link = dt.Rows[0]["link"].ToString();

                // redirect to the link
                Button btnPay = (Button)sender;
                link = Encryption.Decrypt(link);
                btnPay.OnClientClick = "window.open('" + link + "', '_blank'); return false;";
            }
            else
            {
                // if the link is not found
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error: Link not found!');", true);
            }
        }

        protected string GetStatus(string status)
        {
          
            switch (status.ToLower())
            {
                case "pending":
                    return "status-pending";
                case "approved":
                    return "status-to-pay";
                case "to pay":
                    return "status-to-pay";
                case "to accept":
                    return "status-to-accept";
                case "to pickup":
                    return "status-to-pickup";
                case "to reach":
                    return "status-to-reach";
                case "delivering in progress":
                    return "status-to-reach";
                case "completed":
                    return "status-completed";
                case "rejected":
                    return "status-rejected";
                case "refund":
                    return "status-refund";
                default:
                    return "";
            }
        }

        protected void LoadDonationCount()
        {
            // counts 
            int toPayCount = GetDonationCountByStatus("To Pay");
            int toAcceptCount = GetDonationCountByStatus("To Accept");
            int toPickUpCount = GetDonationCountByStatus("To PickUp");
            int toReachCount = GetDonationCountByStatus("To Reach");
            int completedCount = GetDonationCountByStatus("Completed");

            // update the text of the LinkButtons to include the count in bold
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
                // count 'To Pay' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM donation_item_request dir
                INNER JOIN organization org ON dir.orgId = org.orgId
                INNER JOIN donation_publish dp ON dir.donationPublishId = dp.donationPublishId
                WHERE dir.requestStatus = 'Approved' 
                AND dir.paymentStatus IS NULL 
                AND dir.donorId = @donorId";

                parameter.Add("@donorId", id);  
            }
            else if (status == "To Accept")
            {
                // count 'To Accept' donations
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
                // count 'To PickUp' donations
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
                // count 'To Reach' donations
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
                // count 'Completed' donations
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
                return Convert.ToInt32(dt2.Rows[0][0]); 
            }
            else
            {
                return 0;  
            }
        }

        protected void btnViewDelivery_Click(object sender, EventArgs e)
        {
            string donationId = ((Button)sender).CommandArgument;
          
            Response.Redirect($"Delivery.aspx?donationId={donationId}");

        }

        protected void btnRefund_Click(object sender, EventArgs e)
        {
            string donationId = hfDonationId.Value;

            string username= Session["username"].ToString();

            string reason = "";

            if (string.IsNullOrEmpty(ddlRefundReason.SelectedValue) || ddlRefundReason.SelectedValue == "")
            {
                lblError.Text = "Please select your reason for requesting a refund.";
                return;
            }

            else
            {
                reason= ddlRefundReason.Text;
            }
           

            QRY _Qry = new QRY();

            string sql = "UPDATE donation_item_request SET requestStatus = @status WHERE donationId = @donationId";
            var parametersDonation = new Dictionary<string, object>
            {
                { "@donationId", donationId },
                { "@status", "Refund" }
            };

            bool successDonation = _Qry.ExecuteNonQuery(sql, parametersDonation);

            // update the delivery table
            string sql2 = "UPDATE delivery SET deliveryStatus = @deliveryStatus, refundReason= @reason WHERE donationId = @donationId";
            var parametersDelivery = new Dictionary<string, object>
            {
                { "@donationId", donationId },
                { "@deliveryStatus", "Cancelled" },
                { "@reason", reason }
            };

            bool successDelivery = _Qry.ExecuteNonQuery(sql2, parametersDelivery);

            if (successDonation && successDelivery)
            {
                string sql3 = "EXEC [admin_reminder_email] " +
                 "@username = '" + username + "', " +
                 "@reason= '" + reason + "'," +
                 "@action = '" + "REFUND" + "' ";

                bool successEmail = _Qry.ExecuteNonQuery(sql3);

                if (successEmail)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Refund requested successfully! Please be patient while our team handles your request.');", true);
                    LoadDonations("To Accept");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('TheError in requesting refund. Please try again.');", true);
                }

                
            }
            else
            {
             
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in requesting refund. Please try again.');", true);
            }
        }


    }

}