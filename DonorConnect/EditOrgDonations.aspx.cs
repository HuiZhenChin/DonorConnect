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
            if (!IsPostBack)
            {
                string donationId = Request.QueryString["donationPublishId"];
                string orgId = Request.QueryString["orgId"];

                if (!string.IsNullOrEmpty(donationId))
                {
                    Session["donationPublishId"] = donationId;
                    string status = GetOrgStatus(donationId);
                    Session["orgId"] = orgId;

                    // check the status and set button visibility 
                    if (status == "Rejected")
                    {
                        btnResubmit.Visible = true;   
                        btnUpdate.Visible = false;   
                    }
                    else
                    {
                        btnUpdate.Visible = true;     
                        btnResubmit.Visible = false;  
                    }

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
                             specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, status, donationImage, donationAttch
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
                // auto checked and filled item category, specific items and quantity
                PopulateCategories(row["itemCategory"].ToString(),
                                    row["specificItemsForCategory"].ToString(),
                                    row["specificQtyForCategory"].ToString());

                string imagesHtml = ProcessImages(row["donationImage"].ToString());
                imagesContainer.Text = imagesHtml;

                // Display attachments
                string filesHtml = ProcessFiles(row["donationAttch"].ToString());
                filesContainer.Text = filesHtml;
            }
        }

        private void PopulateCategories(string itemCategoryStr, string specificItemsStr, string specificQtyStr)
        {
            // Split the category strings
            string[] itemCategories = itemCategoryStr.Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] specificItems = SplitItems(specificItemsStr.Trim('[', ']'));
            string[] specificQty = SplitItems(specificQtyStr.Trim('[', ']'));

            var categoryControls = new Dictionary<string, Tuple<CheckBox, TextBox, TextBox>>()
            {
                { "Food", new Tuple<CheckBox, TextBox, TextBox>(chkFood, txtSpecificFood, qtyFood) },
                { "Clothing", new Tuple<CheckBox, TextBox, TextBox>(chkClothing, txtSpecificClothing, qtyClothing) },
                { "Toys", new Tuple<CheckBox, TextBox, TextBox>(chkToys, txtSpecificToys, qtyToys) },
                { "Books", new Tuple<CheckBox, TextBox, TextBox>(chkBooks, txtSpecificBooks, qtyBooks) },
                { "Electronics", new Tuple<CheckBox, TextBox, TextBox>(chkElectronics, txtSpecificElectronics, qtyElectronics) },
                { "Furniture", new Tuple<CheckBox, TextBox, TextBox>(chkFurniture, txtSpecificFurniture, qtyFurniture) },
                { "Hygiene Products", new Tuple<CheckBox, TextBox, TextBox>(chkHygiene, txtSpecificHygiene, qtyHygiene) },
                { "Medical Supplies", new Tuple<CheckBox, TextBox, TextBox>(chkMedical, txtSpecificMedical, qtyMedical) },
                { "Other", new Tuple<CheckBox, TextBox, TextBox>(chkOther, txtSpecificOther, qtyOther) }
            };

            foreach (var category in itemCategories)
            {
                string trimmedCategory = category.Trim();

                if (categoryControls.TryGetValue(trimmedCategory, out var controls))
                {
                    controls.Item1.Checked = true; // Check the checkbox

                    int index = Array.IndexOf(itemCategories, category);
                    if (index < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[index]))
                    {
                        controls.Item2.Text = specificItems[index].Trim('(', ')');
                    }
                    else
                    {
                        controls.Item2.Text = string.Empty; // Set empty
                    }

                    if (index < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[index]))
                    {
                        controls.Item3.Text = specificQty[index].Trim('(', ')');
                    }
                    else
                    {
                        controls.Item3.Text = string.Empty; // Set empty
                    }
                }
                else
                {
                    // Handle the "Other" category
                    chkOther.Checked = true;

                    // Append the category to the "Other" text box
                    newCategory.Text = string.IsNullOrEmpty(newCategory.Text) ? trimmedCategory : $"{newCategory.Text}, {trimmedCategory}";

                    // Add comma between multiple specific items for "Other" category
                    int index = Array.IndexOf(itemCategories, category);
                    if (index < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[index]))
                    {
                        txtSpecificOther.Text = string.IsNullOrEmpty(txtSpecificOther.Text)
                            ? specificItems[index]
                            : $"{txtSpecificOther.Text}, {specificItems[index]}"; // Add comma separator
                    }

                    // Add comma between multiple quantities for "Other" category
                    if (index < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[index]))
                    {
                        qtyOther.Text = string.IsNullOrEmpty(qtyOther.Text)
                            ? specificQty[index]
                            : $"{qtyOther.Text}, {specificQty[index]}"; // Add comma separator
                    }
                }
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

        private string ProcessImages(string base64Images)
        {
            if (string.IsNullOrEmpty(base64Images))
            {
                return string.Empty;
            }

            string[] base64ImageArray = base64Images.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder imagesBuilder = new StringBuilder();
            imagesBuilder.AppendLine("<div class='image-grid'>");

            foreach (string base64Image in base64ImageArray)
            {
                imagesBuilder.AppendLine($"<div class='image-item'><img src='data:image/png;base64,{base64Image}' alt='Image' class='img-fluid' /></div>");
            }

            imagesBuilder.AppendLine("</div>");
            return imagesBuilder.ToString();
        }


        private string ProcessFiles(string base64Files)
        {
            if (string.IsNullOrEmpty(base64Files))
            {
                return string.Empty;
            }

            string[] base64FileArray = base64Files.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder filesBuilder = new StringBuilder();

            foreach (string base64File in base64FileArray)
            {
                string[] fileParts = base64File.Split(new char[] { ':' }, 2);
                if (fileParts.Length == 2)
                {
                    string fileName = fileParts[0]; // get original filename
                    string base64Content = fileParts[1];
                    string fileExtension = fileName.Split('.').Last().ToLower();  // get file extension
                    string fileDataUrl = $"data:application/{fileExtension};base64,{base64Content}"; // get url to download from browser
                    filesBuilder.AppendLine($"<a href='{fileDataUrl}' download='{fileName}'>Download {fileName}</a><br />");
                }
            }

            return filesBuilder.ToString();
        }


        protected void btnUpdateDonation_Click(object sender, EventArgs e)
        {
            try
            {
                string donationId = Session["donationPublishId"].ToString();

                List<string> categories = new List<string>();
                List<string> specificItems = new List<string>();
                List<string> quantities = new List<string>();

                if (chkFood.Checked)
                {
                    categories.Add("Food");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificFood.Text) ? "" : $"({txtSpecificFood.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyFood.Text) ? "" : $"({qtyFood.Text})");
                }
                if (chkClothing.Checked)
                {
                    categories.Add("Clothing");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificClothing.Text) ? "" : $"({txtSpecificClothing.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyClothing.Text) ? "" : $"({qtyClothing.Text})");
                }
                if (chkBooks.Checked)
                {
                    categories.Add("Books");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificBooks.Text) ? "" : $"({txtSpecificBooks.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyBooks.Text) ? "" : $"({qtyBooks.Text})");
                }
                if (chkElectronics.Checked)
                {
                    categories.Add("Electronics");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificElectronics.Text) ? "" : $"({txtSpecificElectronics.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyElectronics.Text) ? "" : $"({qtyElectronics.Text})");
                }
                if (chkFurniture.Checked)
                {
                    categories.Add("Furniture");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificFurniture.Text) ? "" : $"({txtSpecificFurniture.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyFurniture.Text) ? "" : $"({qtyFurniture.Text})");
                }
                if (chkHygiene.Checked)
                {
                    categories.Add("Hygiene Products");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificHygiene.Text) ? "" : $"({txtSpecificHygiene.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyHygiene.Text) ? "" : $"({qtyHygiene.Text})");
                }
                if (chkMedical.Checked)
                {
                    categories.Add("Medical Supplies");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificMedical.Text) ? "" : $"({txtSpecificMedical.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyMedical.Text) ? "" : $"({qtyMedical.Text})");
                }
                if (chkToys.Checked)
                {
                    categories.Add("Toys");
                    specificItems.Add(string.IsNullOrEmpty(txtSpecificToys.Text) ? "" : $"({txtSpecificToys.Text})");
                    quantities.Add(string.IsNullOrEmpty(qtyToys.Text) ? "" : $"({qtyToys.Text})");
                }
                if (chkOther.Checked)
                {
                    categories.Add(newCategory.Text);

                    // split the specific items and quantities by comma 
                    var otherSpecificItems = txtSpecificOther.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var otherQuantities = qtyOther.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // add each specific item and quantity without brackets
                    specificItems.AddRange(otherSpecificItems.Select(item => item.Trim()));
                    quantities.AddRange(otherQuantities.Select(qty => qty.Trim()));
                }

                // construct the strings for database storage
                string itemCategories = "[" + string.Join(", ", categories) + "]";
                string specificItemsString = "[" + string.Join(", ", specificItems) + "]";
                string quantitiesString = "[" + string.Join(", ", quantities) + "]";

                string sql;
                QRY _Qry = new QRY();

                string status = GetOrgStatus(donationId);
                string createdOn = GetOrgCreatedOn(donationId);
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

                string urgent = GetOrgUrgency(donationId);

                string username = Session["username"].ToString();
                string orgId = Session["orgId"].ToString();

                if (txtAddress.Text == username)
                {
                    address = GetOrgAddress(username);
                }
                else if (txtAddress.Text != username)
                {
                    address = txtAddress.Text;
                }


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
                                "created_on= '" + createdOn + "', " +
                                "restriction= '" + txtRestrictions.Text + "', " +
                                "donationState = '" + txtRegion.SelectedValue + "' " +
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

        private string GetOrgAddress(string donationId)
        {
            string sql;
            string address = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + donationId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                address = _dt.Rows[0]["orgAddress"].ToString();
            }

            return address;
        }

        private string GetOrgStatus(string donationId)
        {
            string sql;
            string status = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + donationId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                status = _dt.Rows[0]["status"].ToString();
            }

            return status;
        }

        private string GetOrgCreatedOn(string donationId)
        {
            string sql;
            string created_on = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + donationId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                created_on = _dt.Rows[0]["created_on"].ToString();
            }

            return created_on;
        }

        private string GetOrgUrgency(string donationId)
        {
            string sql;
            string urgency = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + donationId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                urgency = _dt.Rows[0]["urgentStatus"].ToString();
            }

            return urgency;
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

        protected void btnCancelDonation_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrgDonations.aspx");
        }

        protected void btnResubmitDonation_Click(object sender, EventArgs e)
        {
            
            // save another data in db, status as pending approval
            // in admin side let him compare the previous and current application
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
                specificItems.Add(string.IsNullOrEmpty(txtSpecificFood.Text) ? "" : $"({txtSpecificFood.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyFood.Text) ? "" : $"({qtyFood.Text})");
            }
            if (chkClothing.Checked)
            {
                categories.Add("Clothing");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificClothing.Text) ? "" : $"({txtSpecificClothing.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyClothing.Text) ? "" : $"({qtyClothing.Text})");
            }
            if (chkBooks.Checked)
            {
                categories.Add("Books");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificBooks.Text) ? "" : $"({txtSpecificBooks.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyBooks.Text) ? "" : $"({qtyBooks.Text})");
            }
            if (chkElectronics.Checked)
            {
                categories.Add("Electronics");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificElectronics.Text) ? "" : $"({txtSpecificElectronics.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyElectronics.Text) ? "" : $"({qtyElectronics.Text})");
            }
            if (chkFurniture.Checked)
            {
                categories.Add("Furniture");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificFurniture.Text) ? "" : $"({txtSpecificFurniture.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyFurniture.Text) ? "" : $"({qtyFurniture.Text})");
            }
            if (chkHygiene.Checked)
            {
                categories.Add("Hygiene Products");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificHygiene.Text) ? "" : $"({txtSpecificHygiene.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyHygiene.Text) ? "" : $"({qtyHygiene.Text})");
            }
            if (chkMedical.Checked)
            {
                categories.Add("Medical Supplies");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificMedical.Text) ? "" : $"({txtSpecificMedical.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyMedical.Text) ? "" : $"({qtyMedical.Text})");
            }
            if (chkToys.Checked)
            {
                categories.Add("Toys");
                specificItems.Add(string.IsNullOrEmpty(txtSpecificToys.Text) ? "" : $"({txtSpecificToys.Text})");
                quantities.Add(string.IsNullOrEmpty(qtyToys.Text) ? "" : $"({qtyToys.Text})");
            }
            if (chkOther.Checked)
            {
                categories.Add(newCategory.Text);

                // split the specific items and quantities by comma 
                var otherSpecificItems = txtSpecificOther.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var otherQuantities = qtyOther.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // add each specific item and quantity without brackets
                specificItems.AddRange(otherSpecificItems.Select(item => item.Trim()));
                quantities.AddRange(otherQuantities.Select(qty => qty.Trim()));
            }

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
            string donationId = Session["donationPublishId"].ToString();
            string username = Session["username"].ToString();
            string orgId = Session["orgId"].ToString();
            string resubmit = "yes";

            if (txtAddress.Text == username)
            {
                address = GetOrgAddress(username);
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation details resubmitted successfully! Your application is now pending for approval.',);", true);
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error resubmitting donation application. Please try again!');", true);
            }
        }
        
    }
}