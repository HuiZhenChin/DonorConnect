using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class EditOrgDonations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string eventTarget = Request["__EVENTTARGET"];
            string eventArgument = Request["__EVENTARGUMENT"];

            if (eventTarget == "CancelDonationConfirmed" && !string.IsNullOrEmpty(eventArgument))
            {
                string donationId = eventArgument;
                DeleteDonation(donationId);

               
            }

            if (!IsPostBack)
            {
                string donationId = Request.QueryString["donationPublishId"];
                string orgId = Request.QueryString["orgId"];
                string rejected = Request.QueryString["status"];
                string urgent = Request.QueryString["urgent"];

                if (!string.IsNullOrEmpty(donationId))
                {
                    Session["donationPublishId"] = donationId;
                    DonationPublish dp = new DonationPublish(donationId, "", "", "", "", "", "");
                    string status = dp.GetStatus();
                    Session["orgId"] = orgId;

                    if (!string.IsNullOrEmpty(urgent) && !string.IsNullOrEmpty(donationId) && !string.IsNullOrEmpty(orgId))
                    {
                        // display the Republish button if urgent is present
                        btnRepublish.Visible = true;
                    }
                    else if (status == "Rejected" || !string.IsNullOrEmpty(rejected))
                    {
                        // check the status and set button visibility
                        btnResubmit.Visible = true;
                        btnUpdate.Visible = false;
                        btnCancel.Visible = true;
                    }
                    else
                    {
                        btnUpdate.Visible = true;
                        btnResubmit.Visible = false;
                    }

                    // load donation details
                    LoadDonationDetails(donationId);
                }
            }

        }

        private void LoadDonationDetails(string donationId)
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();
            string username = Session["username"].ToString();
            //string id = GetOrgId(username);
            string strSQL = @"SELECT urgentStatus, title, peopleNeeded, description, restriction, itemCategory, 
                             specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, status, donationImage, donationAttch, rejectedReason, recipientName, recipientPhoneNumber, timeRange
                      FROM [donation_publish] 
                      WHERE donationPublishId  = '" + donationId + "' ";

            _dt = _Qry.GetData(strSQL);
            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                rbUrgentYes.Checked = row["urgentStatus"].ToString() == "Yes";
                rbUrgentNo.Checked = row["urgentStatus"].ToString() == "No";
                txtTitle.Text = row["title"].ToString();
                txtQuantity.Text = row["peopleNeeded"].ToString();
                txtAddress.Text = row["address"].ToString();
                txtRegion.SelectedValue = row["donationState"].ToString();
                txtDescription.Text = row["description"].ToString();
                txtRestrictions.Text = row["restriction"].ToString();
                txtName.Text = row["recipientName"].ToString();
                txtPhone.Text = row["recipientPhoneNumber"].ToString();
                txtTimeRange.Text = row["timeRange"].ToString();

                LoadCategories();

                // auto checked and filled item category, specific items and quantity
                PopulateCategories(row["itemCategory"].ToString(),
                                    row["specificItemsForCategory"].ToString(),
                                    row["specificQtyForCategory"].ToString());

                string imagesHtml = ImageFileProcessing.ProcessImages(row["donationImage"].ToString());
                imagesContainer.Text = imagesHtml;

                // display attachments
                string filesHtml = ImageFileProcessing.ProcessImages(row["donationAttch"].ToString());
                filesContainer.Text = filesHtml;

                if (row["rejectedReason"] != DBNull.Value && !string.IsNullOrEmpty(row["rejectedReason"].ToString()))
                {
                    txtReason.Text = row["rejectedReason"].ToString();
                    reason.Style["display"] = "block";
                }
                else
                {
                    reason.Style["display"] = "none";
                }

            }
        }

        private void PopulateCategories(string itemCategoryStr, string specificItemsStr, string specificQtyStr)
        {
            // split the category strings
            string[] itemCategories = itemCategoryStr.Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] specificItems = SplitItems(specificItemsStr.Trim('[', ']'));
            string[] specificQty = SplitItems(specificQtyStr.Trim('[', ']'));

            foreach (RepeaterItem item in rptCategories.Items)
            {
                CheckBox chkCategory = item.FindControl("chkCategory") as CheckBox;
                TextBox txtSpecificItem = item.FindControl("txtSpecificItem") as TextBox;
                TextBox txtQuantity = item.FindControl("txtQuantity") as TextBox;

                if (chkCategory != null && txtSpecificItem != null && txtQuantity != null)
                {
                    // check if the current checkbox's category text matches any category in the list
                    for (int j = 0; j < itemCategories.Length; j++)
                    {
                        string trimmedCategory = itemCategories[j].Trim();

                        if (chkCategory.Text.Equals(trimmedCategory, StringComparison.OrdinalIgnoreCase))
                        {
                            chkCategory.Checked = true; // check the checkbox

                            if (j < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[j]) && !specificItems[j].Equals("null", StringComparison.OrdinalIgnoreCase))
                            {
                                txtSpecificItem.Text = specificItems[j].Trim('(', ')');
                                txtSpecificItem.Style["display"] = "block"; 
                            }
                            else
                            {
                                txtSpecificItem.Text = string.Empty; 
                                txtSpecificItem.Style["display"] = "none"; 
                            }

                            // populate quantity if available
                            if (j < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[j]) && !specificQty[j].Equals("null", StringComparison.OrdinalIgnoreCase))
                            {
                                txtQuantity.Text = specificQty[j].Trim('(', ')');
                                txtQuantity.Style["display"] = "block"; 
                            }
                            else
                            {
                                txtQuantity.Text = string.Empty; 
                                txtQuantity.Style["display"] = "none"; 
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void LoadCategories()
        {
            string sql = "SELECT categoryName FROM itemCategory";
            QRY _Qry = new QRY();
            DataTable categoriesTable = _Qry.GetData(sql);

            DataTable reorderedTable = categoriesTable.Clone(); 

            foreach (DataRow row in categoriesTable.Rows)
            {
                if (!row["categoryName"].ToString().Equals("Others", StringComparison.OrdinalIgnoreCase))
                {
                    reorderedTable.ImportRow(row);
                }
            }

            // add the "Others" row at the end, if it exists
            foreach (DataRow row in categoriesTable.Rows)
            {
                if (row["categoryName"].ToString().Equals("Others", StringComparison.OrdinalIgnoreCase))
                {
                    reorderedTable.ImportRow(row);
                    break;
                }
            }

            rptCategories.DataSource = reorderedTable;
            rptCategories.DataBind();
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


        protected void btnUpdateDonation_Click(object sender, EventArgs e)
        {
            try
            {
                string donationId = Session["donationPublishId"].ToString();

                List<string> categories = new List<string>();
                List<string> specificItems = new List<string>();
                List<string> quantities = new List<string>();

                foreach (RepeaterItem item in rptCategories.Items)
                {
                    CheckBox chkCategory = (CheckBox)item.FindControl("chkCategory");
                    TextBox txtSpecificItem = (TextBox)item.FindControl("txtSpecificItem");
                    TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");

                    if (chkCategory.Checked)
                    {
                        categories.Add(chkCategory.Text);
                        // if specific item is empty, use "null"
                        specificItems.Add(string.IsNullOrEmpty(txtSpecificItem.Text) ? "null" : $"({txtSpecificItem.Text})");
                        quantities.Add(string.IsNullOrEmpty(txtQuantity.Text) ? "" : $"({txtQuantity.Text})");
                    }
                }

                if (categories.Count == 0)
                {
                    lblCategory.Style["display"] = "block";
                    return;
                }

                // construct the strings for saving to the database
                string itemCategories = "[" + string.Join(", ", categories) + "]";
                string specificItemsString = "[" + string.Join(", ", specificItems) + "]";
                string quantitiesString = "[" + string.Join(", ", quantities) + "]";

                string sql;
                QRY _Qry = new QRY();

                DonationPublish dp = new DonationPublish(donationId, "", "", "", "", "", "");
                string status = dp.GetStatus();
                DateTime? createdOn = dp.GetCreatedOn();
                string imgUpload = "";
                string fileUpload = "";
                string address = "";

                if (donationImg.HasFiles)
                {

                    imgUpload = ImageFileProcessing.ConvertToBase64(donationImg.PostedFiles);
                }

                else
                {
                    imgUpload = "";
                }

                if (donationFile.HasFiles)
                {
                    fileUpload = ImageFileProcessing.ConvertToBase64(donationFile.PostedFiles);
                }

                else
                {
                    fileUpload = "";
                }

                DonationPublish dp2 = new DonationPublish(donationId, "", "", "", "", "", "");
                string urgent = dp2.GetUrgency();

                string username = Session["username"].ToString();
                string orgId = Session["orgId"].ToString();
                Organization org = new Organization(username, "", "", "", "");
                if (txtAddress.Text == username)
                {
                    address = org.GetOrgAddress();
                }
                else if (txtAddress.Text != username)
                {
                    address = txtAddress.Text;
                }


                string createdOnValue = createdOn.HasValue ? $"'{createdOn.Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL";

                sql = "UPDATE [donation_publish] SET " +
                                "title = '" + txtTitle.Text + "', " +
                                "peopleNeeded = '" + txtQuantity.Text + "', " +
                                "address = '" + address + "', " +
                                "description = '" + txtDescription.Text + "', " +
                                "itemCategory = '" + itemCategories + "', " +
                                "specificItemsForCategory = '" + specificItemsString + "', " +
                                "specificQtyForCategory ='" + quantitiesString + "', " +
                                "timeRange = '" + txtTimeRange.Text + "', " +
                                "urgentStatus = '" + urgent + "', " +
                                "status = '" + status + "', " +
                                "donationImage = '" + imgUpload + "', " +
                                "donationAttch = '" + fileUpload + "', " +
                                "orgId = '" + orgId + "', " +
                                "adminId= NULL, " +
                                "created_on= " + createdOnValue + ", " +  // Handle null or valid date
                                "restriction= '" + txtRestrictions.Text + "', " +
                                "donationState = '" + txtRegion.SelectedValue + "', " +
                                "recipientName = '" + txtName.Text + "', " +
                                "recipientPhoneNumber = '" + txtPhone.Text + "' " +
                                "WHERE donationPublishId = '" + donationId + "'";

                bool success = _Qry.ExecuteNonQuery(sql);

                if (success)
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation details updated successfully!',);", true);
                    // send email notify admin
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating donation details. Please try again!');", true);
                }

                LoadDonationDetails(donationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showError('There was an error updating donation details: {ex.Message}');", true);
            }



        }

   
        protected void btnDiscardDonation_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrgDonations.aspx");
        }

        protected void btnResubmitDonation_Click(object sender, EventArgs e)
        {
           
            lblCategory.Visible = false;

            List<string> categories = new List<string>();
            List<string> specificItems = new List<string>();
            List<string> quantities = new List<string>();

            foreach (RepeaterItem item in rptCategories.Items)
            {
                CheckBox chkCategory = (CheckBox)item.FindControl("chkCategory");
                TextBox txtSpecificItem = (TextBox)item.FindControl("txtSpecificItem");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");

                if (chkCategory.Checked)
                {
                    categories.Add(chkCategory.Text);
                    // If specific item is empty, use "null", otherwise enclose the item in parentheses
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificItem.Text) ? "null" : $"({txtSpecificItem.Text})");
                    quantities.Add(string.IsNullOrEmpty(txtQuantity.Text) ? "" : $"({txtQuantity.Text})");
                }
            }

            if (categories.Count == 0)
            {
                lblCategory.Style["display"] = "block";
                return;
            }

            // construct the strings for saving to the database
            string itemCategories = "[" + string.Join(", ", categories) + "]";
            string specificItemsString = "[" + string.Join(", ", specificItems) + "]";
            string quantitiesString = "[" + string.Join(", ", quantities) + "]";

            string sql, sqlupdate;
            QRY _Qry = new QRY();
            QRY _QryUpdate = new QRY();

            string status = "Pending Approval";
            string imgUpload = "";
            string fileUpload = "";
            string address = "";

            if (donationImg.HasFiles)
            {

                imgUpload = ImageFileProcessing.ConvertToBase64(donationImg.PostedFiles);
            }

            else
            {
                imgUpload = "";
            }

            if (donationFile.HasFiles)
            {
                fileUpload = ImageFileProcessing.ConvertToBase64(donationFile.PostedFiles);
            }

            else
            {
                fileUpload = "";
            }

            string urgent = rbUrgentYes.Checked ? "Yes" : "No";
            string donationId = Session["donationPublishId"].ToString();
            string username = Session["username"].ToString();
            string orgId = Session["orgId"].ToString();
            string resubmit = "yes";
            Organization org = new Organization(username, "", "", "", "");

            if (txtAddress.Text == username)
            {
                address = org.GetOrgAddress();
            }
            else if (txtAddress.Text != username)
            {
                address = txtAddress.Text;
            }

            sqlupdate = "UPDATE [donation_publish] SET " +             
                              "resubmit = '" + resubmit + "' " +
                               "WHERE donationPublishId = '" + donationId + "'";

            _QryUpdate.ExecuteNonQuery(sqlupdate);

            sql = "EXEC [create_org_item_donations] " +
                            "@method = 'INSERT', " +
                            "@donationPublishId = NULL, " +  // auto-generated in stored procedure
                            "@title = '" + txtTitle.Text + "', " +
                            "@peopleNeeded = '" + txtQuantity.Text + "', " +
                            "@address = '" + address + "', " +
                            "@desc = '" + txtDescription.Text + "', " +
                            "@itemCategory = '" + itemCategories + "', " +
                            "@specificItems = '" + specificItemsString + "', " +
                            "@specificQty ='" + quantitiesString + "', " +
                            "@timeRange = '" + txtTimeRange.Text + "', " +
                            "@urgentStatus = '" + urgent + "', " +
                            "@status = '" + status + "', " +
                            "@donationImage = '" + imgUpload + "', " +
                            "@donationAttch = '" + fileUpload + "', " +
                            "@orgId = '" + orgId + "', " +
                            "@adminId= NULL, " +
                            "@created_on= NULL, " +
                            "@restriction= '" + txtRestrictions.Text + "', " +
                            "@state = '" + txtRegion.SelectedValue + "', " +
                            "@name = '" + txtName.Text + "', " +
                            "@phone = '" + txtPhone.Text + "', " +
                            "@closureReason = NULL, " +
                            "@resubmit = '" + resubmit + "' ";

            bool success = _Qry.ExecuteNonQuery(sql);
            string sqlemail;
            QRY _Qry2 = new QRY();

            if (success)
            {
                sqlemail = "EXEC [admin_reminder_email] " +
                            "@action = 'RESUBMIT', " +
                            "@orgName = '" + username + "' ";
                _Qry2.ExecuteNonQuery(sqlemail);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess2('Donation details resubmitted successfully! Your application is now pending for approval.',);", true);
               
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error resubmitting donation application. Please try again!');", true);
            }
        }

        protected void btnCancelDonation_Click(object sender, EventArgs e)
        {
            string donationId = Session["donationPublishId"].ToString();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmCancelDonation",
            "confirmCancelDonation('" + donationId + "');", true);
        }

        private void DeleteDonation(string donationId)
        {

            string sql = "DELETE FROM donation_publish WHERE donationPublishId = @donationId";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@donationId", donationId }
            };

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Your donation application has been cancelled successful!',);", true);
                Response.Redirect("OrgDonations.aspx");
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error cancelling donation application. Please try again!');", true);
            }
        }

        protected void btnRepublish_Click(object sender, EventArgs e)
        {
            string donationId = Request.QueryString["donationPublishId"];
            string username = Session["username"].ToString();
            string orgId = Session["orgId"].ToString();

            List<string> categories = new List<string>();
            List<string> specificItems = new List<string>();
            List<string> quantities = new List<string>();

            foreach (RepeaterItem item in rptCategories.Items)
            {
                CheckBox chkCategory = (CheckBox)item.FindControl("chkCategory");
                TextBox txtSpecificItem = (TextBox)item.FindControl("txtSpecificItem");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");

                if (chkCategory.Checked)
                {
                    categories.Add(chkCategory.Text);
                    // if specific item is empty, use "null"
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificItem.Text) ? "null" : $"({txtSpecificItem.Text})");
                    quantities.Add(string.IsNullOrEmpty(txtQuantity.Text) ? "" : $"({txtQuantity.Text})");
                }
            }

            if (categories.Count == 0)
            {
                lblCategory.Style["display"] = "block";
                return;
            }

            string itemCategories = "[" + string.Join(", ", categories) + "]";
            string specificItemsString = "[" + string.Join(", ", specificItems) + "]";
            string quantitiesString = "[" + string.Join(", ", quantities) + "]";

            string address = (txtAddress.Text == username) ? new Organization(username, "", "", "", "").GetOrgAddress() : txtAddress.Text;

            string imgUpload = donationImg.HasFiles ? ImageFileProcessing.ConvertToBase64(donationImg.PostedFiles) : "";
            string fileUpload = donationFile.HasFiles ? ImageFileProcessing.ConvertToBase64(donationFile.PostedFiles) : "";

            string urgent = rbUrgentYes.Checked ? "Yes" : "No";
            string status = "Opened";

            string sql = "UPDATE donation_publish SET " +
                         "timeRange = @timeRange, " +
                         "status = @status, " +
                         "newCountdownStart = @start, " +
                         "title = @title, " +
                         "peopleNeeded = @peopleNeeded, " +
                         "address = @address, " +
                         "description = @description, " +
                         "itemCategory = @itemCategory, " +
                         "specificItemsForCategory = @specificItems, " +
                         "specificQtyForCategory = @specificQty, " +
                         "urgentStatus = @urgentStatus, " +
                         "donationImage = @donationImage, " +
                         "donationAttch = @donationAttch, " +
                         "restriction = @restriction, " +
                         "donationState = @state, " +
                         "recipientName = @name, " +
                         "recipientPhoneNumber = @phone " +                       
                         "WHERE donationPublishId = @donationId";

            var parameters = new Dictionary<string, object>
            {
                { "@timeRange", txtTimeRange.Text },
                { "@donationId", donationId },
                { "@status", status },
                { "@start", DateTime.Now },
                { "@title", txtTitle.Text },
                { "@peopleNeeded", txtQuantity.Text },
                { "@address", address },
                { "@description", txtDescription.Text },
                { "@itemCategory", "[" + string.Join(", ", categories) + "]" }, 
                { "@specificItems", "[" + string.Join(", ", specificItems) + "]" },
                { "@specificQty", "[" + string.Join(", ", quantities) + "]" },
                { "@urgentStatus", urgent },
                { "@donationImage", imgUpload },
                { "@donationAttch", fileUpload },
                { "@restriction", txtRestrictions.Text },
                { "@state", txtRegion.SelectedValue },
                { "@name", txtName.Text },
                { "@phone", txtPhone.Text }

            };

            QRY _Qry = new QRY();

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess2('Your donation application has been published successfully!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error publishing the donation. Please try again!');", true);
            }
        }

    }
}