using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AllDonations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();
            //string username = Session["username"].ToString();
            //string id = GetOrgId(username);
            string status = "Opened";
            string strSQL = @"SELECT *, donationPublishId, urgentStatus, title, peopleNeeded, description, restriction, itemCategory, 
                    specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, donationImage, donationAttch, orgId
                 FROM [donation_publish] WHERE status = '" + status + "'";

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
                processedTable.Columns.Add("itemDetails");
                processedTable.Columns.Add("donationImages");
                processedTable.Columns.Add("donationFiles");
                processedTable.Columns.Add("orgProfilePic");
                processedTable.Columns.Add("orgName");

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

                    string orgId = row["orgId"].ToString();
                    string profilePic = GetOrgProfilePic(orgId);
                    string orgName= GetOrgName(orgId);

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
                    newRow["itemDetails"] = itemDetailsBuilder.ToString();
                    newRow["donationImages"] = ProcessImages(row["donationImage"].ToString());
                    newRow["donationFiles"] = ProcessFiles(row["donationAttch"].ToString());
                    newRow["orgProfilePic"] = profilePic;
                    newRow["orgName"] = orgName;

                    processedTable.Rows.Add(newRow);
                }

                gvAllDonations.DataSource = processedTable;
                gvAllDonations.DataBind();
                    
            }
            else
            {
                gvAllDonations.DataSource = null;
                gvAllDonations.DataBind();
                   
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

        protected void gvAllDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
            }
        }

        protected string GetItemCategoryWithIcon(object itemDetails)
        {
            if (itemDetails == null) return string.Empty;

            string details = itemDetails.ToString();
            string[] categories = ExtractCategories(details);
            StringBuilder sb = new StringBuilder();

            foreach (string category in categories)
            {
                string icon = GetIconForCategory(category);
                string colorClass = GetBorderColorForCategory(category); // Get color class for each category

                sb.AppendFormat("<div class='category-box {0}'><i class='{1}'></i> {2}</div>", colorClass, icon, category);
            }

            return sb.ToString();
        }



        private string GetIconForCategory(string category)
        {
            
            switch (category.ToLower())
            {
                case "clothing":
                    return "fas fa-tshirt"; 
                case "food":
                    return "fas fa-seedling"; 
                case "furniture":
                    return "fas fa-couch";
                case "toys":
                    return "fas fa-football-ball";
                case "books":
                    return "fas fa-book";
                case "electronics":
                    return "fas fa-plug";
                case "hygiene products":
                    return "fas fa-pump-soap";
                case "medical supplies":
                    return "fas fa-briefcase-medical";
                default:
                    return "fas fa-box-open"; 
            }
        }

        private string[] ExtractCategories(string details)
        {
            List<string> categories = new List<string>();

            // using format from ItemDetails "Item Category 1 (CategoryName):"
            var matches = Regex.Matches(details, @"Item Category \d+ \((.*?)\):");
            foreach (Match match in matches)
            {
                categories.Add(match.Groups[1].Value.Trim());
            }

            return categories.ToArray();
        }

        private string GetBorderColorForCategory(string category)
        {
            switch (category.ToLower())
            {
                case "food":
                    return "border-food";
                case "books":
                    return "border-books";
                case "toys":
                    return "border-toys";
                case "medical supplies":
                    return "border-medical";
                case "clothing":
                    return "border-clothing";
                case "electronics":
                    return "border-electronics";
                case "furniture":
                    return "border-furniture";
                case "hygiene products":
                    return "border-hygiene";
                default:
                    return "border-default";
            }
        }

        private string GetOrgProfilePic(string orgId)
        {
            string sql;
            string profilepic = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgId = '" + orgId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                profilepic = _dt.Rows[0]["orgProfilePicBase64"].ToString();
            }

            return profilepic;
        }

        private string GetOrgName(string orgId)
        {
            string sql;
            string name = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgId = '" + orgId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                name = _dt.Rows[0]["orgName"].ToString();
            }

            return name;
        }

    }
}
