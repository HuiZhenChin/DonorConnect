using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

        public class ItemCategory
        {
            public string Item { get; set; }
            public string Category { get; set; }
        }


        private void BindGridView(string keyword = "", string selectedCategory = "", string selectedLocation = "")
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();
            //string username = Session["username"].ToString();
            //string id = GetOrgId(username);
            string status = "Opened";
            string strSQL = @"SELECT *, donationPublishId, urgentStatus, title, peopleNeeded, description, restriction, itemCategory, 
                    specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, donationImage, donationAttch, orgId
                 FROM [donation_publish] WHERE status = '" + status + "'";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                strSQL += " AND (title LIKE '%" + keyword + "%' OR description LIKE '%" + keyword + "%')";
            }

            if (!string.IsNullOrWhiteSpace(selectedCategory))
            {
                strSQL += " AND itemCategory LIKE '%" + selectedCategory + "%'";
            }

            if (!string.IsNullOrWhiteSpace(selectedLocation))
            {
                strSQL += " AND donationState = '" + selectedLocation + "'";
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
                    string profilePic = GetOrgProfilePic(orgId) ?? "/Image/default_picture.jpg";
                    string orgName = GetOrgName(orgId);

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

        protected void LoadCategories(object sender, EventArgs e)
        {
            var categories = GetCategoriesFromDatabase();

            // bind categories
            rptCategories.DataSource = categories;
            rptCategories.DataBind();

           
            rptCategories.Visible = true;
        }


        protected void CategorySelected(object sender, EventArgs e)
        {
            foreach (RepeaterItem categoryItem in rptCategories.Items)
            {
                // find the checkbox in the current category 
                var chkCategory = categoryItem.FindControl("chkCategory") as CheckBox;
                if (chkCategory != null && chkCategory.Checked)
                {
                    // get the category name
                    var category = chkCategory.Text;

                    // find the nested rptItems 
                    var gvItems = categoryItem.FindControl("rptItems") as Repeater;
                    if (gvItems != null)
                    {
                        // retrieve items for the selected category
                        var itemsByCategory = GetItemsByCategories(new List<string> { category });

                        // create a list of ItemCategory objects for binding
                        var itemCategoryList = new List<ItemCategory>();
                        if (itemsByCategory.ContainsKey(category))
                        {
                            foreach (var item in itemsByCategory[category])
                            {
                                itemCategoryList.Add(new ItemCategory { Item = item, Category = category });
                            }
                        }

                        gvItems.DataSource = itemCategoryList;
                        gvItems.DataBind();

                        
                        gvItems.Visible = true;
                    }
                }
            }
        }



        private Dictionary<string, List<string>> GetItemsByCategories(List<string> categories)
        {
            var itemsByCategory = new Dictionary<string, List<string>>();

            if (categories.Count == 0)
                return itemsByCategory;

            // retrieve items for the selected categories
            string categoryList = string.Join("','", categories.Select(c => c.Replace("'", "''"))); 
            string query = $"SELECT categoryName, specificItems FROM itemCategory WHERE categoryName IN ('{categoryList}')";

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(query);

            
            foreach (DataRow row in dt.Rows)
            {
                string categoryName = row["categoryName"].ToString();
                string itemsString = row["specificItems"].ToString();

                
                var items = itemsString.Split(',').Select(i => i.Trim()).ToList();

                // category is added to the dictionary with its items
                if (!itemsByCategory.ContainsKey(categoryName))
                {
                    itemsByCategory[categoryName] = items;
                }
                else
                {
                    // if the category already exists, add new items to it (if not already present)
                    foreach (var item in items)
                    {
                        if (!itemsByCategory[categoryName].Contains(item))
                        {
                            itemsByCategory[categoryName].Add(item);
                        }
                    }
                }
            }

            return itemsByCategory;
        }


        private List<string> GetCategoriesFromDatabase()
        {
            // retrieve categories from the database
            string sql = "SELECT categoryName FROM itemCategory";

            var categories = new List<string>();
            QRY _Qry = new QRY();
            DataTable _dt;

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow row in _dt.Rows)
                {
                    categories.Add(row["categoryName"].ToString());
                }
            }

            return categories;
        }



        protected void FilterDonations(object sender, EventArgs e)
        {
            var selectedCategories = new List<string>();
            var selectedItemsDictionary = new Dictionary<string, List<string>>();
            var selectedStates = hfStateName.Value.Split(',');

            // collect selected categories and items
            foreach (RepeaterItem categoryItem in rptCategories.Items)
            {
                var chkCategory = categoryItem.FindControl("chkCategory") as CheckBox;
                var rptItems = categoryItem.FindControl("rptItems") as Repeater; 

                if (chkCategory != null && chkCategory.Checked)
                {
                    // add the selected category to the list
                    selectedCategories.Add(chkCategory.Text);

                  
                    if (rptItems != null)
                    {
                        foreach (RepeaterItem item in rptItems.Items)
                        {
                            var chkItem = item.FindControl("chkItem") as CheckBox;
                            if (chkItem != null && chkItem.Checked)
                            {
                                var hiddenField = item.FindControl("hfCategory") as HiddenField;
                                if (hiddenField != null)
                                {
                                    var category = hiddenField.Value;
                                    if (!selectedItemsDictionary.ContainsKey(category))
                                    {
                                        selectedItemsDictionary[category] = new List<string>();
                                    }
                                    selectedItemsDictionary[category].Add(chkItem.Text);
                                }
                            }
                        }
                    }
                }
            }


            // create category and items strings for filtering
               var categoryString = string.Join(",", selectedCategories);
               var itemStringList = new List<string>();
            foreach (var category in selectedCategories)
            {
                if (selectedItemsDictionary.ContainsKey(category))
                {
                    var items = selectedItemsDictionary[category];
                    if (items.Count > 0)
                    {
                        itemStringList.Add($"({string.Join(",", items)})");
                    }
                    else
                    {
                        // retrieve all items for the category from the database
                        var allItems = GetAllItemsForCategory(category); 

                        if (allItems.Count > 0)
                        {
                            itemStringList.Add($"({string.Join(",", allItems)}, null)");
                        }
                        else
                        {
                            itemStringList.Add("null");
                        }
                    }
                }
                else
                {
                    // retrieve all items for the category from the database
                    var allItems = GetAllItemsForCategory(category); 

                    if (allItems.Count > 0)
                    {
                        itemStringList.Add($"({string.Join(",", allItems)}, null)");
                    }
                    else
                    {
                        itemStringList.Add("null");
                    }
                }
            }


            string specificItemsString = string.Join(",", itemStringList);
            string states = string.Join(",", selectedStates);

            // call the filtered donations method
            DataTable filteredDonations = GetFilteredDonations(categoryString, specificItemsString, states);

            // add these columns, as they are from different table in database
            filteredDonations.Columns.Add("orgProfilePic", typeof(string));
            filteredDonations.Columns.Add("orgName", typeof(string));
            filteredDonations.Columns.Add("itemDetails", typeof(string));
            filteredDonations.Columns.Add("donationImages", typeof(string));
            filteredDonations.Columns.Add("donationFiles", typeof(string));

            // populate the new columns
            foreach (DataRow row in filteredDonations.Rows)
            {
                string orgId = row["orgId"].ToString();
                row["orgProfilePic"] = GetOrgProfilePic(orgId);
                row["orgName"] = GetOrgName(orgId);

                
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

                row["itemDetails"] = itemDetailsBuilder.ToString();
                row["donationImages"] = ProcessImages(row["donationImage"].ToString());
                row["donationFiles"] = ProcessFiles(row["donationAttch"].ToString());
            }

            
            gvAllDonations.DataSource = filteredDonations;
            gvAllDonations.DataBind();
        }


        private List<string> GetAllItemsForCategory(string category)
        {
            var allItems = new List<string>();
          
            category = category.Replace("'", "''"); 

           
            string sql = $"SELECT specificItems FROM itemCategory WHERE categoryName = '{category}'";

            
            QRY _Qry = new QRY();
            DataTable _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow row in _dt.Rows)
                {
                    
                    var items = row["specificItems"].ToString();
                    if (!string.IsNullOrEmpty(items))
                    {
                        var itemList = items.Split(',').Select(item => item.Trim()).ToList();
                        allItems.AddRange(itemList);
                    }
                }
            }

            return allItems;
        }



        private DataTable GetFilteredDonations(string categoryName, string specificItems, string state)
        {
            
            string sql = "EXEC [filter_donations] " +
                           "@categoryName = '" + categoryName + "', " +
                           "@specificItems = '" + specificItems + "', " +
                           "@state = '" + state + "' ";
           
            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql);


            return dt;
        }

        protected void LoadStates(object sender, EventArgs e)
        {
            // load the list of states
            rptStates.DataSource = new List<string>
            {
                "Johor", "Kedah", "Kelantan", "Melaka", "Negeri Sembilan",
                "Pahang", "Penang", "Perak", "Perlis", "Sabah", "Sarawak",
                "Selangor", "Terengganu"
            };
            rptStates.DataBind();
            rptStates.Visible = true;
        }

        protected void StateSelected(object sender, EventArgs e)
        {
            // retrieve selected states
            var selectedStates = new List<string>();
            foreach (RepeaterItem state in rptStates.Items)
            {
                var chkState = (CheckBox)state.FindControl("chkState");
                if (chkState != null && chkState.Checked)
                {
                    selectedStates.Add(chkState.Text);
                }
            }

            // store selected states in hidden field 
            hfStateName.Value = string.Join(",", selectedStates);
        }



    }
}
