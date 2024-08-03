using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                lblCategory.Visible = true;
                return;
            }


            lblCategory.Visible = false;

            List<string> categories = new List<string>();
            List<string> specificItems = new List<string>();
            List<string> quantities = new List<string>();

            if (chkFood.Checked)
            {
                categories.Add("Food");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificFood.Text) ? "" : txtSpecificFood.Text);
                quantities.Add(string.IsNullOrEmpty(qtyFood.Text) ? "" : qtyFood.Text);
            }
            if (chkClothing.Checked)
            {
                categories.Add("Clothing");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificClothing.Text) ? "" : txtSpecificClothing.Text);
                quantities.Add(string.IsNullOrEmpty(qtyClothing.Text) ? "" : qtyClothing.Text);
            }
            if (chkBooks.Checked)
            {
                categories.Add("Books");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificBooks.Text) ? "" : txtSpecificBooks.Text);
                quantities.Add(string.IsNullOrEmpty(qtyBooks.Text) ? "" : qtyBooks.Text);
            }
            if (chkElectronics.Checked)
            {
                categories.Add("Electronics");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificElectronics.Text) ? "" : txtSpecificElectronics.Text);
                quantities.Add(string.IsNullOrEmpty(qtyElectronics.Text) ? "" : qtyElectronics.Text);
            }
            if (chkFurniture.Checked)
            {
                categories.Add("Furniture");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificFurniture.Text) ? "" : txtSpecificFurniture.Text);
                quantities.Add(string.IsNullOrEmpty(qtyFurniture.Text) ? "" : qtyFurniture.Text);
            }
            if (chkHygiene.Checked)
            {
                categories.Add("Hygiene Products");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificHygiene.Text) ? "" : txtSpecificHygiene.Text);
                quantities.Add(string.IsNullOrEmpty(qtyHygiene.Text) ? "" : qtyHygiene.Text);
            }
            if (chkMedical.Checked)
            {
                categories.Add("Medical Supplies");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificMedical.Text) ? "" : txtSpecificMedical.Text);
                quantities.Add(string.IsNullOrEmpty(qtyMedical.Text) ? "" : qtyMedical.Text);
            }
            if (chkToys.Checked)
            {
                categories.Add("Toys");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificToys.Text) ? "" : txtSpecificToys.Text);
                quantities.Add(string.IsNullOrEmpty(qtyToys.Text) ? "" : qtyToys.Text);
            }
            if (chkOther.Checked)
            {
                categories.Add("Other");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificOther.Text) ? "" : txtSpecificOther.Text);
                quantities.Add(string.IsNullOrEmpty(qtyOther.Text) ? "" : qtyOther.Text);
            }

            string itemCategories = "[" + string.Join(", ", categories) + "]";
            string specificItemsString = "[" + string.Join(", ", specificItems.Select(item => string.IsNullOrEmpty(item) ? "" : $"({item})")) + "]";
            string quantitiesString = "[" + string.Join(", ", quantities.Select(qty => string.IsNullOrEmpty(qty) ? "" : $"({qty})")) + "]";

            string sql;
            QRY _Qry = new QRY();

            string status = "Pending Approval";
            string imgUpload = "";
            string fileUpload = "";

            string fileExtension = Path.GetExtension(donationFile.FileName).ToLower();

            if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
            {
                imgUpload = ConvertToBase64(donationFile.PostedFile);

            }
            if (fileExtension == ".pdf")
            {
                fileUpload = ConvertToBase64(donationFile.PostedFile);

            }
            string urgent = rbUrgentYes.Checked ? "Yes" : "No";

            string username = Session["username"].ToString();
            string orgId = GetOrgId(username);

            sql = "EXEC [create_org_item_donations] " +
                            "@method = 'INSERT', " +
                            "@donationPublishId = NULL, " +  //auto-generated in stored procedure
                            "@title = '" + txtTitle.Text + "', " +
                            "@peopleNeeded = '" + txtQuantity.Text + "', " +
                            "@address = '" + txtAddress.Text + "', " +
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
                            "@created_on= NULL ";

            DataTable dt_check = _Qry.GetData(sql);

            if (dt_check.Rows.Count > 0)
            {
                string message = dt_check.Rows[0]["MESSAGE"].ToString();
                if (message == "Successful! You have submitted a new item donation application. Your request is now pending for approval.")
                {
                    Session["message"] = message;
                        
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");

                }

                else
                {
                    // Show SweetAlert error dialog
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                }

            }
            
        }

        private string ConvertToBase64(HttpPostedFile postedFile)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                postedFile.InputStream.CopyTo(ms);
                byte[] bytes = ms.ToArray();
                return Convert.ToBase64String(bytes);
            }

        }

        private string GetOrgId(string username)
        {
            string sql;
            string id = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                id = _dt.Rows[0]["orgId"].ToString();
            }

            return id;
        }

        protected void chkFood_CheckedChanged(object sender, EventArgs e)
        {
            ToggleField();
        }

        protected void ToggleField()
        {
            if (chkFood.Checked)
            {
                txtSpecificFood.Attributes["style"] = "display: block;";
                qtyFood.Attributes["style"] = "display: block;";
            }
            else
            {
                txtSpecificFood.Attributes["style"] = "display: none;";
                qtyFood.Attributes["style"] = "display: none;";
            }
        }


    }
}