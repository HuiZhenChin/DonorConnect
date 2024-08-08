﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
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
                categories.Add(newCategory.Text);
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
            string address = "";

            if (donationImg.HasFiles)
            {

                imgUpload = ConvertImgToBase64(donationImg.PostedFiles);
            }

            else
            {
                imgUpload = "";
            }

            if (donationFile.HasFiles)
            {
                fileUpload = ConvertFileToBase64(donationFile.PostedFiles);
            }
            
            else
            {
                fileUpload = ""; 
            }

            string urgent = rbUrgentYes.Checked ? "Yes" : "No";

            string username = Session["username"].ToString();
            string orgId = GetOrgId(username);

            if (txtAddress.Text == username)
            {
                address = GetOrgAddress(username);
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
                            "@state = '" + txtRegion.SelectedValue + "' ";

            DataTable dt_check = _Qry.GetData(sql);

            if (dt_check.Rows.Count > 0)
            {
                string message = dt_check.Rows[0]["MESSAGE"].ToString();
                if (message == "Successful! You have submitted a new item donation application. Your request is now pending for approval.")
                {
                    Session["message"] = message;

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");
                    clearText();
                }

                else
                {
                    // Show SweetAlert error dialog
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                }

            }

        }

        private string ConvertImgToBase64(IList<HttpPostedFile> postedFiles)
        {
            if (postedFiles == null || postedFiles.Count == 0)
            {
                return string.Empty;
            }

            List<string> base64Files = new List<string>();

            foreach (HttpPostedFile uploadedFile in postedFiles)
            {
                using (BinaryReader reader = new BinaryReader(uploadedFile.InputStream))
                {
                    byte[] fileBytes = reader.ReadBytes((int)uploadedFile.InputStream.Length);
                    string base64String = Convert.ToBase64String(fileBytes);
                    base64Files.Add(base64String);
                }
            }

            return string.Join(",", base64Files);
        }


        private string ConvertFileToBase64(IList<HttpPostedFile> postedFiles)
        {
            if (postedFiles == null || postedFiles.Count == 0)
            {
                return string.Empty;
            }

            List<string> base64Files = new List<string>();

            foreach (HttpPostedFile uploadedFile in postedFiles)
            {
                using (BinaryReader reader = new BinaryReader(uploadedFile.InputStream))
                {
                    byte[] fileBytes = reader.ReadBytes((int)uploadedFile.InputStream.Length);
                    string base64String = Convert.ToBase64String(fileBytes);
                    string fileName = uploadedFile.FileName;
                    base64Files.Add($"{fileName}:{base64String}");
                }
            }

            return string.Join(",", base64Files);
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

        private string GetOrgAddress(string username)
        {
            string sql;
            string address = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                address = _dt.Rows[0]["orgAddress"].ToString();
            }

            return address;
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