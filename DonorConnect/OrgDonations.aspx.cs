using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace DonorConnect
{
    public partial class PublishDonations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView(string status = "All", string urgency = "All")
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();
            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string id = org.GetOrgId();
            string strSQL = @"SELECT donationPublishId, urgentStatus, title, peopleNeeded, description, restriction, itemCategory, 
                             specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, status, donationImage, donationAttch
                      FROM [donation_publish] 
                      WHERE orgId = '" + id + "'";

            if (status != "All")
            {
                strSQL += " AND status = '" + status + "'";
            }

            if (urgency != "All")
            {
                strSQL += " AND urgentStatus = '" + urgency + "'";
            }

            _dt = _Qry.GetData(strSQL);
            if (_dt.Rows.Count > 0)
            {
                DataTable processedTable = new DataTable();
                processedTable.Columns.Add("donationPublishId");
                processedTable.Columns.Add("urgentStatus");
                processedTable.Columns.Add("title");
                processedTable.Columns.Add("peopleNeeded");
                processedTable.Columns.Add("description");
                processedTable.Columns.Add("restriction");
                processedTable.Columns.Add("address");
                processedTable.Columns.Add("donationState");
                processedTable.Columns.Add("created_on");
                processedTable.Columns.Add("status");
                processedTable.Columns.Add("itemDetails");
                processedTable.Columns.Add("donationImages");
                processedTable.Columns.Add("donationFiles");

                foreach (DataRow row in _dt.Rows)
                {
                    string[] itemCategories = row["itemCategory"].ToString().Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] specificItems = SplitItems(row["specificItemsForCategory"].ToString().Trim('[', ']'));
                    string[] specificQty = SplitItems(row["specificQtyForCategory"].ToString().Trim('[', ']'));

                    StringBuilder itemDetailsBuilder = new StringBuilder();

                    for (int i = 0; i < itemCategories.Length; i++)
                    {
                        itemDetailsBuilder.Append("Item Category " + (i + 1) + " (" + itemCategories[i].Trim() + "):<br />");

                        if (i < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[i]))
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: " + specificItems[i].Trim('(', ')') + "<br />");
                        }
                        else
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: Any<br />");
                        }

                        if (i < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[i]))
                        {
                            itemDetailsBuilder.Append("Specific Quantity Needed: " + specificQty[i].Trim('(', ')') + "<br />");
                        }
                        else
                        {
                            itemDetailsBuilder.Append("Specific Quantity Needed: Not stated<br />");
                        }

                        itemDetailsBuilder.Append("<br />");
                    }

                    DataRow newRow = processedTable.NewRow();
                    newRow["donationPublishId"] = row["donationPublishId"];
                    newRow["urgentStatus"] = row["urgentStatus"];
                    newRow["title"] = row["title"];
                    newRow["peopleNeeded"] = row["peopleNeeded"];
                    newRow["description"] = row["description"];
                    newRow["restriction"] = row["restriction"];
                    newRow["address"] = row["address"];
                    newRow["donationState"] = row["donationState"];
                    newRow["created_on"] = row["created_on"];
                    newRow["status"] = row["status"];
                    newRow["itemDetails"] = itemDetailsBuilder.ToString();
                    newRow["donationImages"] = ImageFileProcessing.ProcessImages(row["donationImage"].ToString());
                    newRow["donationFiles"] = ImageFileProcessing.ProcessFiles(row["donationAttch"].ToString()); 


                    processedTable.Rows.Add(newRow);
                }

                gvDonations.DataSource = processedTable;
                gvDonations.DataBind();
                lblNoResults.Visible = false;
            }
            else
            {
                gvDonations.DataSource = null;
                gvDonations.DataBind();
                lblNoResults.Visible = true;
            }
        }

        private string[] SplitItems(string input)
        {
            var items = new List<string>();
            var sb = new StringBuilder();
            int openBracketCount = 0;

            foreach (char c in input)
            {
                if (c == ',' && openBracketCount == 0)
                {
                    items.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else
                {
                    if (c == '(')
                        openBracketCount++;
                    if (c == ')')
                        openBracketCount--;

                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
                items.Add(sb.ToString().Trim());

            return items.ToArray();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus= ddlStatus.SelectedValue;
            string selectedUrgency = ddlUrgency.SelectedValue;
            BindGridView(selectedStatus, selectedUrgency);
        }

        protected void ddlUrgency_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            string selectedUrgency = ddlUrgency.SelectedValue;
            BindGridView(selectedStatus, selectedUrgency);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            string selectedUrgency = ddlUrgency.SelectedValue;
            BindGridView(selectedStatus, selectedUrgency);
        }


        protected void btnResubmit_Click(object sender, EventArgs e)
        {
            // check if the donation has already been resubmitted
            LinkButton btn = (LinkButton)sender;
            string donationPublishId = btn.CommandArgument;
            string checkSql = "SELECT resubmit FROM [donation_publish] WHERE donationPublishId = '" + donationPublishId + "' ";
            QRY _QryCheck = new QRY();
            DataTable dt = _QryCheck.GetData(checkSql);

            if (dt.Rows.Count > 0 && dt.Rows[0]["resubmit"].ToString() == "yes")
            {
                // show alert if already resubmitted
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError2('You have already resubmitted your application. You can only resubmit once before approval from admin.');", true);
                return;
            }

            else
            {
                string username = Session["username"].ToString();
                Organization org = new Organization(username, "", "", "", "");
                string orgId = org.GetOrgId();

                // redirect to edit page 
                Response.Redirect($"EditOrgDonations.aspx?donationPublishId={donationPublishId}&orgId={orgId}");

            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // check if the donation has already been resubmitted, cannot allow editing
            LinkButton btn = (LinkButton)sender;
            string donationPublishId = btn.CommandArgument;
            string checkSql = "SELECT resubmit FROM [donation_publish] WHERE donationPublishId = '" + donationPublishId + "' ";
            QRY _QryCheck = new QRY();
            DataTable dt = _QryCheck.GetData(checkSql);

            if (dt.Rows.Count > 0 && dt.Rows[0]["resubmit"].ToString() == "yes")
            {
                // show alert if already resubmitted
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('This is your resubmitted application. You can only edit after getting approval from admin.');", true);
                return;
            }
            else
            {
                string username = Session["username"].ToString();
                Organization org = new Organization(username, "", "", "", "");
                string orgId = org.GetOrgId();

                // redirect to edit page 
                Response.Redirect($"EditOrgDonations.aspx?donationPublishId={donationPublishId}&orgId={orgId}");
                // send notifications to admin dashboard
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            string donationPublishId = hiddenDonationPublishId.Value;
            string closureReason = ddlClosureReason.SelectedValue;
            string otherReason = txtOtherReason.Text;

            if (closureReason == "Other")
            {
                closureReason = otherReason;
            }

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            // directly close the donation and proceed with notifications
            CloseDonation(donationPublishId, closureReason, username, orgId);
        }


        // close the donation and notify donors if confirmed
        protected void CloseDonation(string donationPublishId, string closureReason, string username, string orgId)
        {
            string status = "Closed";
            string sql = "UPDATE [donation_publish] SET status = @status WHERE donationPublishId = @donationPublishId";

            QRY _Qry = new QRY();
            var parameter = new Dictionary<string, object>
            {
                { "@status", status },
                { "@donationPublishId", donationPublishId }
            };
            bool success = _Qry.ExecuteNonQuery(sql, parameter);

            if (success)
            {
                // send closure notification to admin
                string sqlemail = "EXEC [admin_reminder_email] @action = 'CLOSE', @reason = @closureReason, @orgName = @username";
                var parameter2 = new Dictionary<string, object>
                {
                    { "@reason", closureReason },
                    { "@orgName", username }
                };
                QRY _Qry2 = new QRY();
                _Qry2.ExecuteNonQuery(sqlemail, parameter2);

                // update closure reason 
                string sqlupdate = "UPDATE [donation_publish] SET closureReason = @closureReason WHERE donationPublishId = @donationPublishId";
                var parameter3 = new Dictionary<string, object>
                {
                    { "@closureReason", closureReason },
                    { "@donationPublishId", donationPublishId }
                };
                QRY _Qry3 = new QRY();
                _Qry3.ExecuteNonQuery(sqlupdate, parameter3);

                // auto-reject pending requests and notify each donor
                string rejectSql = "UPDATE donation_item_request SET status = 'Rejected' WHERE donationPublishId = @donationPublishId AND requestStatus = 'Pending'";
                QRY _QryReject = new QRY();
                _QryReject.ExecuteNonQuery(rejectSql, parameter);

                // get all pending donor IDs to notify them of rejection
                string donorIdQuery = "SELECT donorId FROM donation_item_request WHERE donationPublishId = @donationPublishId AND requestStatus = 'Pending'";
                List<Dictionary<string, object>> result = _Qry.ExecuteQuery(donorIdQuery, parameter);

                // notify each donor
                foreach (var row in result)
                {
                    string donorId = row["donorId"].ToString();
                    string sqlemail2 = "EXEC [application_email] @action = 'AUTO-REJECT DONATION REQUEST', @reason = @reason, @role = 'donor', @donorId = @donorId";
                    var parameter6 = new Dictionary<string, object>
                    {
                        { "@reason", closureReason },
                        { "@donorId", donorId }
                    };
                    QRY _QryEmail = new QRY();
                    _QryEmail.ExecuteNonQuery(sqlemail2, parameter6);
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation closed successfully and pending requests have been rejected.');", true);
                BindGridView(); 
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error closing the donation. Please try again!');", true);
            }
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string donationPublishId = btn.CommandArgument;
            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql;
            QRY _Qry = new QRY();
            
            sql = "DELETE FROM [donation_publish] WHERE donationPublishId = '" + donationPublishId + "' ";

            bool isDeleted = _Qry.ExecuteNonQuery(sql);

            if (isDeleted)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation application cancelled successfully!',);", true);
                //send email notify admin
                string sqlemail;

                QRY _Qry2 = new QRY();

                sqlemail = "EXEC [admin_reminder_email] " +
                             "@action = 'CANCEL', " +
                             "@orgName = '" + username + "' ";
                _Qry2.ExecuteNonQuery(sqlemail);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error cancelling donation application. Please try again!');", true);
            }

            BindGridView();
        }


        protected void gvDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
          
                var statusLabel = (HtmlGenericControl)e.Row.FindControl("statusLabel");
                var status = DataBinder.Eval(e.Row.DataItem, "status").ToString();

                // assign icon based on the status
                string iconClass = GetStatusIcon(status);
                string iconHtml = $"<i class=\"{iconClass}\"></i>";

                if (statusLabel != null)
                {
                    statusLabel.InnerHtml =  status.ToUpper() + " " + iconHtml;
                   
                }
            }
        }

        private string GetStatusIcon(string status)
        {
            switch (status.ToLower())
            {
                case "pending approval":
                    return "fas fa-hourglass-half"; 
                case "rejected":
                    return "fas fa-times";
                case "closed":
                    return "fas fa-check"; 
                case "opened":
                    return "fas fa-handshake"; 
                default:
                    return "fas fa-question"; 
            }
        }


        protected void btnCreateDonation_Click(object sender, EventArgs e)
        {
            // redirect to a page to create a new donation
            Response.Redirect("~/PublishDonations.aspx");
        }

        [WebMethod]
        public static int GetPendingCount(string donationPublishId)
        {
            string pendingCountSql = "SELECT COUNT(*) FROM donation_item_request WHERE donationPublishId = @donationPublishId AND requestStatus = 'Pending'";
            QRY _QryCheck = new QRY();
            var parameter = new Dictionary<string, object> { { "@donationPublishId", donationPublishId } };
            int pendingCount = Convert.ToInt32(_QryCheck.ExecuteScalar(pendingCountSql, parameter));
            return pendingCount;
        }

    }
}