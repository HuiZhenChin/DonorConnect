using System;
using System.Collections.Generic;
using System.Data;
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



                }
            }
        }

        protected void btnSubmitNewDonation_Click(object sender, EventArgs e)
        {
            if (!chkFood.Checked && !chkClothing.Checked && !chkBooks.Checked && !chkElectronics.Checked
                && !chkFurniture.Checked && !chkHygiene.Checked && !chkMedical.Checked && !chkToys.Checked
                && !chkOther.Checked)
            {
                lblCategory.Style["display"] = "block";
                return;
            }


            lblCategory.Visible = false;

            List<string> categories = new List<string>();
            List<string> specificItems = new List<string>();
            List<string> quantities = new List<string>();

            if (chkFood.Checked)
            {
                categories.Add("Food");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificFood.Text) ? "null" : $"({txtSpecificFood.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyFood.Text) ? "" : $"({qtyFood.Text})");
            }
            if (chkClothing.Checked)
            {
                categories.Add("Clothing");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificClothing.Text) ? "null" : $"({txtSpecificClothing.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyClothing.Text) ? "" : $"({qtyClothing.Text})");
            }
            if (chkBooks.Checked)
            {
                categories.Add("Books");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificBooks.Text) ? "null" : $"({txtSpecificBooks.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyBooks.Text) ? "" : $"({qtyBooks.Text})");
            }
            if (chkElectronics.Checked)
            {
                categories.Add("Electronics");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificElectronics.Text) ? "null" : $"({txtSpecificElectronics.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyElectronics.Text) ? "" : $"({qtyElectronics.Text})");
            }
            if (chkFurniture.Checked)
            {
                categories.Add("Furniture");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificFurniture.Text) ? "null" : $"({txtSpecificFurniture.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyFurniture.Text) ? "" : $"({qtyFurniture.Text})");
            }
            if (chkHygiene.Checked)
            {
                categories.Add("Hygiene Products");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificHygiene.Text) ? "null" : $"({txtSpecificHygiene.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyHygiene.Text) ? "" : $"({qtyHygiene.Text})");
            }
            if (chkMedical.Checked)
            {
                categories.Add("Medical Supplies");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificMedical.Text) ? "null" : $"({txtSpecificMedical.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyMedical.Text) ? "" : $"({qtyMedical.Text})");
            }
            if (chkToys.Checked)
            {
                categories.Add("Toys");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificToys.Text) ? "null" : $"({txtSpecificToys.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyToys.Text) ? "" : $"({qtyToys.Text})");
            }
            if (chkOther.Checked)
            {
                categories.Add(newCategory.Text);

                // Split the specific items and quantities by comma for the "Other" category
                var otherSpecificItems = txtSpecificOther.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var otherQuantities = qtyOther.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // Add each specific item and quantity for the "Other" category without brackets
                specificItems.AddRange(otherSpecificItems.Select(item => item.Trim()));
                quantities.AddRange(otherQuantities.Select(qty => qty.Trim()));
            }

            // Construct the strings for saving to the database
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
            chkBooks.Checked = false;
            chkClothing.Checked = false;
            chkElectronics.Checked = false;
            chkFood.Checked = false;
            chkFurniture.Checked = false;
            chkHygiene.Checked = false;
            chkMedical.Checked = false;
            chkOther.Checked = false;
            chkToys.Checked = false;
            newCategory.Text = "";
        }
      


    }
}