using System;
using System.Collections.Generic;
using System.Data;
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
                    Session["donationPublishId"]= donationId;
                    Session["orgId"] = orgId;
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
                    // Check "Other" and add new category dynamically
                    chkOther.Checked = true;

                    // Create a TextBox for the new category name
                    TextBox newCategoryName = new TextBox
                    {
                        CssClass = "form-control specific-items-input",
                        Text = trimmedCategory
                    };
                    newCategoryName.Attributes.Add("placeholder", "Enter a new category");

                    // Create TextBox for specific items
                    TextBox newSpecificItems = new TextBox
                    {
                        CssClass = "form-control specific-items-input"
                    };
                    newSpecificItems.Attributes.Add("placeholder", "Specify items needed");

                    // Create TextBox for quantity
                    TextBox newQty = new TextBox
                    {
                        CssClass = "form-control specific-qty-input",
                        TextMode = TextBoxMode.Number
                    };
                    newQty.Attributes.Add("placeholder", "Enter quantity");

                    // Assign values if they exist
                    int index = Array.IndexOf(itemCategories, category);
                    newSpecificItems.Text = index < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[index])
                                          ? specificItems[index].Trim('(', ')')
                                          : string.Empty;

                    newQty.Text = index < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[index])
                                ? specificQty[index].Trim('(', ')')
                                : string.Empty;

                    // Add the new controls to the placeholder
                    PlaceholderNewCategory.Controls.Add(new LiteralControl("<br />"));  // Adding a line break
                    PlaceholderNewCategory.Controls.Add(newCategoryName);
                    PlaceholderNewCategory.Controls.Add(newSpecificItems);
                    PlaceholderNewCategory.Controls.Add(newQty);
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

        protected void btnUpdateDonation_Click(object sender, EventArgs e)
        {
            try {
                string donationId = Session["donationPublishId"].ToString();

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

                string status = GetOrgStatus(donationId);
                string createdOn= GetOrgCreatedOn(donationId);
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
                string orgId= Session["orgId"].ToString();

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

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('User information updated successfully!',);", true);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating user information. Please try again!');", true);
                }
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
    }
}