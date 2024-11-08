using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DonorConnect
{
    public partial class PublishDonations1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    string username = Session["username"].ToString();

                    LoadCategories();

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



        protected void btnSubmitNewDonation_Click(object sender, EventArgs e)
        {
          
            lblCategory.Visible = false;

            List<string> categories = new List<string>();
            List<string> specificItems = new List<string>();
            List<string> quantities = new List<string>();

           if (rbUrgentYes.Checked)
            {
                if (!int.TryParse(txtTimeRange.Text, out int timeRange) || timeRange <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire('Error', 'Please enter a valid time range in days.', 'error');", true);
                    return;
                }
            }

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
                lblCategory.Visible = true;
                lblCategory.Style["display"] = "block";
                lblCategory.InnerText = "Please select at least one category.";

                //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire('Error', 'Please select at least one category.', 'error');", true);
                return;
            }

            // construct the strings for saving to the database
            string itemCategories = "[" + string.Join(", ", categories) + "]";
            string specificItemsString = "[" + string.Join(", ", specificItems) + "]"; 
            string quantitiesString = "[" + string.Join(", ", quantities) + "]";

            string sql;
            QRY _Qry = new QRY();

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

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            if (txtAddress.Text == username)
            {
                address = org.GetOrgAddress();
            }
            else if (txtAddress.Text != username)
            {
                address = txtAddress.Text;
            }

            sql = "EXEC [create_org_item_donations] " +
                            "@method = 'INSERT', " +
                            "@donationPublishId = NULL, " +  //auto-generated in stored procedure
                            "@title = '" + txtTitle.Text + "', " +
                            "@peopleNeeded = '" + txtQuantity.Text + "', " +
                            "@name = '" + txtName.Text + "', " +
                            "@phone = '" + txtPhone.Text + "', " +
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
                            "@closureReason = NULL, " +
                            "@resubmit = NULL " ;

            DataTable dt_check = _Qry.GetData(sql);

            if (dt_check.Rows.Count > 0)
            {
                string message = dt_check.Rows[0]["MESSAGE"].ToString();
                if (message == "Successful! You have submitted a new item donation application. Your request is now pending for approval.")
                {
                    Session["message"] = message;

                    // send email notify admin
                    string sqlemail;
                    QRY _Qry2 = new QRY();

                    if (rbUrgentNo.Checked)
                    {
                        
                        sqlemail = "EXEC [admin_reminder_email] " +
                                 "@action = 'NEW NORMAL APPLICATION', " +
                                 "@orgName = '" + username + "' ";
                        _Qry2.ExecuteNonQuery(sqlemail);
                    }

                    if (rbUrgentYes.Checked)
                    {
                        
                        sqlemail = "EXEC [admin_reminder_email] " +
                                 "@action = 'NEW URGENT APPLICATION', " +
                                 "@orgName = '" + username + "' ";
                        _Qry2.ExecuteNonQuery(sqlemail);
                    }

                    // retrieve the list of active admin IDs
                    string getAdminIdsSql = "SELECT adminId FROM admin WHERE status = 'Active'";
                    DataTable adminIdsTable = _Qry2.GetData(getAdminIdsSql);

                    if (adminIdsTable.Rows.Count > 0)
                    {
                        // encrypt the link once, as it will be the same for all admins
                        string link = $"AdminManageDonationRequest.aspx";
                        string encryptedLink = Encryption.Encrypt(link);

                        // loop through each admin ID and create a notification
                        foreach (DataRow row in adminIdsTable.Rows)
                        {
                            string adminId = row["adminId"].ToString();
                            string message2 = "New donation request from " + username;

                            string sqlNtf = "EXEC [create_notifications] " +
                                            "@method = 'INSERT', " +
                                            "@id = NULL, " +
                                            "@userId = @userId, " +
                                            "@link = @link, " +
                                            "@content = @content";

                            // for each admin
                            var notificationParameter = new Dictionary<string, object>
                            {
                                { "@userId", adminId },
                                { "@link", encryptedLink },
                                { "@content", message2 }
                            };

                            _Qry2.ExecuteNonQuery(sqlNtf, notificationParameter);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No active admins found for notification.");
                    }

                  
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");
                    clearText();
                }

                else
                {
                    // show error dialog
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                }

            }

        }


        protected void clearText()
        {
            txtTitle.Text = "";
            txtQuantity.Text = "";
            txtAddress.Text = "";
            txtRegion.Text = "";
            txtDescription.Text = "";
            txtRestrictions.Text = "";
            rbUrgentNo.Checked = true;
            rbUrgentNo.Checked = false;
            txtTimeRange.Text = "";
            //chkBooks.Checked = false;
            //chkClothing.Checked = false;
            //chkElectronics.Checked = false;
            //chkFood.Checked = false;
            //chkFurniture.Checked = false;
            //chkHygiene.Checked = false;
            //chkMedical.Checked = false;
            //chkOther.Checked = false;
            //chkToys.Checked = false;
            //newCategory.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
        }
      


    }
}