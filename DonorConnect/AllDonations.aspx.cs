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
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AllDonations : System.Web.UI.Page
    {
        // a dictionary to store available category with its items for all opened donations
        private Dictionary<string, List<string>> categoryItemDict = new Dictionary<string, List<string>>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindGridView();
                Session["AvailableCategoryItem"] = categoryItemDict;
                string jsonCategoryItem = Newtonsoft.Json.JsonConvert.SerializeObject(categoryItemDict);
                ClientScript.RegisterStartupScript(this.GetType(), "ConsoleLogCategoryItem", $"console.log({jsonCategoryItem});", true);


                string donationPublishId = Request.QueryString["donationPublishId"];
                if (!string.IsNullOrEmpty(donationPublishId))
                {
                    // scroll to the specific donation
                    ClientScript.RegisterStartupScript(this.GetType(), "ScrollToDonation", $"scrollToSelectedDonation('{donationPublishId}');", true);
                    HttpContext.Current.Session.Remove("Distance");
                    HttpContext.Current.Session.Remove("PickupAddress");
                }
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
                    specificItemsForCategory, specificQtyForCategory, address, donationState, created_on, donationImage, donationAttch, orgId, timeRange, approved_on, newCountdownStart, recipientPhoneNumber, recipientName
                 FROM [donation_publish] WHERE status = '" + status + "' ORDER BY CASE WHEN urgentStatus = 'Yes' THEN 1 ELSE 2 END, donation_publish.created_on ASC";

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

            string username = Session["username"]?.ToString();
            HashSet<string> savedDonations = new HashSet<string>();

            if (!string.IsNullOrEmpty(username))
            {
                // fetch saved donations for the user
                DataTable savedDonationsTable = _Qry.GetData($"SELECT donationPublishId FROM saved_favourite_donation WHERE username = '{username}'");
                foreach (DataRow savedRow in savedDonationsTable.Rows)
                {
                    savedDonations.Add(savedRow["donationPublishId"].ToString());
                }
            }

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
                processedTable.Columns.Add("urgentLabel");
                processedTable.Columns.Add("cardBody");
                processedTable.Columns.Add("saveButton");
                processedTable.Columns.Add("countdownEndDate");
                processedTable.Columns.Add("newCountdownStart");
                processedTable.Columns.Add("recipientPhoneNumber");
                processedTable.Columns.Add("recipientName");

                foreach (DataRow row in _dt.Rows)
                {
                    string[] itemCategories = row["itemCategory"].ToString().Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] specificItems = SplitItems(row["specificItemsForCategory"].ToString().Trim('[', ']'));
                    string[] specificQty = SplitItems(row["specificQtyForCategory"].ToString().Trim('[', ']'));

                    StringBuilder itemDetailsBuilder = new StringBuilder();

                    bool isUrgent = row["urgentStatus"].ToString().ToLower() == "yes";

                    for (int i = 0; i < itemCategories.Length; i++)
                    {
                        string category = itemCategories[i].Trim();
                        string items = i < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[i]) ? specificItems[i].Trim('(', ')') : "Any";

                        // replace "null" with an empty string
                        items = items.Replace("null", "").Trim();

                        if (string.IsNullOrEmpty(items))
                        {
                            items = "Any";
                        }

                        // store in dictionary
                        if (!categoryItemDict.ContainsKey(category))
                        {
                            categoryItemDict[category] = new List<string>();
                        }

                        categoryItemDict[category].Add(items);
                   
                        itemDetailsBuilder.Append("Item Category " + (i + 1) + " (" + category + "):<br />");

                        if (i < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[i]))
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: " + items + "<br />");
                        }
                        else if (specificItems[i] == "null")
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: Any<br />");
                        }

                        if (isUrgent && i < specificQty.Length && !string.IsNullOrWhiteSpace(specificQty[i]))
                        {
                            itemDetailsBuilder.Append("Specific Quantity Needed: " + specificQty[i].Trim('(', ')') + "<br />");
                        }
                        else if (isUrgent)
                        {
                            itemDetailsBuilder.Append("Specific Quantity Needed: Not stated<br />");
                        }

                        itemDetailsBuilder.Append("<br />");
                    }                

                    string orgId = row["orgId"].ToString();
                    Organization org = new Organization("", orgId, "", "", "");
                    string profilePic = org.GetOrgProfilePic() ?? "/Image/default_picture.jpg";
                    string orgName = org.GetOrgName();

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
                    newRow["donationImages"] = ImageFileProcessing.ProcessImages(row["donationImage"].ToString());
                    newRow["donationFiles"] = ImageFileProcessing.ProcessFiles(row["donationAttch"].ToString());
                    newRow["orgProfilePic"] = profilePic;
                    newRow["orgName"] = orgName;
                    newRow["recipientPhoneNumber"] = row["recipientPhoneNumber"];
                    newRow["recipientName"] = row["recipientName"];

                    DateTime approvedOn = row["approved_on"] != DBNull.Value ? Convert.ToDateTime(row["approved_on"]) : DateTime.MinValue;

                    DateTime countdownStartTime = row["newCountdownStart"] != DBNull.Value
                        ? Convert.ToDateTime(row["newCountdownStart"])
                        : approvedOn;

                    if (isUrgent)
                    {
                        newRow["urgentLabel"] = @"
                        <div style='color: white; 
                                    background: red; 
                                    font-weight: bold; 
                                    text-align: left;
                                    padding: 5px 10px;
                                    height: 50px;
                                    display: flex;
                                    align-items: center;
                                    width: fit-content'>
                            <i class='fa fa-bullhorn' aria-hidden='true' style='margin-right: 8px;'></i>
                            URGENT!
                        </div>";

                        newRow["cardBody"] = "urgent-card";

                        int timeRangeDays = 0;
                        if (row["timeRange"] != DBNull.Value && int.TryParse(row["timeRange"].ToString(), out timeRangeDays) && timeRangeDays > 0)
                        {
                            // Add the timeRange value to the determined countdown start time
                            DateTime countdownEndDate = countdownStartTime.AddDays(timeRangeDays);
                            newRow["countdownEndDate"] = countdownEndDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else
                        {
                            newRow["countdownEndDate"] = "";
                        }

                    }
                    else
                    {
                        newRow["urgentLabel"] = "";
                    }

                    string donationId = row["donationPublishId"].ToString();
                    if (savedDonations.Contains(donationId))
                    {
                        newRow["saveButton"] = "black";
                    }
                    else
                    {
                        newRow["saveButton"] = "lightgray";
                    }

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

       
        public Dictionary<string, List<string>> GetCategoryItemDictionary()
        {
            return Session["AvailableCategoryItem"] as Dictionary<string, List<string>>;
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
                string icon = "fas " + GetIconForCategory(category);
                string colorClass = GetBorderColorForCategory(category); 

                sb.AppendFormat("<div class='category-box {0}'><i class='{1}'></i> {2}</div>", colorClass, icon, category);
            }

            return sb.ToString();
        }



        private string GetIconForCategory(string category)
        {
            string sql = "SELECT categoryIcon FROM itemCategory WHERE categoryName = @categoryName";
            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@categoryName", category }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["categoryIcon"].ToString();
            }
            else
            {
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

        protected void LoadCategories(object sender, EventArgs e)
        {
            if (rptCategories.Visible)
            {
                rptCategories.Visible = false;
                btnFilterDonations.Visible = false;
            }
            else
            {
                var categories = GetCategoriesFromDatabase();

                rptCategories.DataSource = categories;
                rptCategories.DataBind();

                rptCategories.Visible = true;
                btnFilterDonations.Visible = true;
            }
        }

        protected void CategorySelected(object sender, EventArgs e)
        {
            var categoryItemDictionary = Session["AvailableCategoryItem"] as Dictionary<string, List<string>>;

            if (categoryItemDictionary != null)
            {
                var itemsByCategory = GetItemsByCategories(categoryItemDictionary);

                foreach (RepeaterItem categoryItem in rptCategories.Items)
                {
                    var chkCategory = categoryItem.FindControl("chkCategory") as CheckBox;
                    if (chkCategory != null && chkCategory.Checked)
                    {
                        var category = chkCategory.Text;

                        var gvItems = categoryItem.FindControl("rptItems") as Repeater;
                        if (gvItems != null && itemsByCategory.ContainsKey(category))
                        {
                            var itemCategoryList = itemsByCategory[category].Select(item => new ItemCategory
                            {
                                Item = item,
                                Category = category
                            }).ToList();

                            gvItems.DataSource = itemCategoryList;
                            gvItems.DataBind();
                            gvItems.Visible = true;
                        }
                    }
                }
            }
        }


        private Dictionary<string, List<string>> GetItemsByCategories(Dictionary<string, List<string>> categoryItemDictionary)
        {
            var itemsByCategory = new Dictionary<string, List<string>>();

            if (categoryItemDictionary.Count == 0)
                return itemsByCategory;

            // filter the dictionary to only include categories that are already stored
            string categoryList = string.Join("','", categoryItemDictionary.Keys.Select(c => c.Replace("'", "''")));

            string query = $"SELECT categoryName, specificItems FROM itemCategory WHERE categoryName IN ('{categoryList}')";

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(query);

            foreach (DataRow row in dt.Rows)
            {
                string categoryName = row["categoryName"].ToString();
                string itemsString = row["specificItems"].ToString();

                var items = itemsString.Split(',').Select(i => i.Trim()).ToList();

                // store the matched items
                var matchedItems = new List<string>();

                if (categoryItemDictionary.ContainsKey(categoryName))
                {
                    foreach (var item in categoryItemDictionary[categoryName])
                    {
                        // compare each item
                        foreach (var dbItem in items)
                        {
                            if (item.IndexOf(dbItem, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                matchedItems.Add(dbItem);
                            }
                        }
                    }

                    // remove duplicates and merge the matched items with the existing ones in the dictionary
                    matchedItems = matchedItems.Distinct().ToList();

                    // add the matched items to the final dictionary
                    itemsByCategory[categoryName] = matchedItems;
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
                            itemStringList.Add("");
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
                        itemStringList.Add("");
                    }
                }
            }


            string specificItemsString = string.Join(",", itemStringList);
            string states = string.Join(",", selectedStates);

            // call the filtered donations method
            DataTable filteredDonations = GetFilteredDonations(categoryString, specificItemsString, states);

            string username = Session["username"]?.ToString();
            HashSet<string> savedDonations = new HashSet<string>();
            QRY _Qry = new QRY();

            if (!string.IsNullOrEmpty(username))
            {
                // fetch saved donations for the user
                DataTable savedDonationsTable = _Qry.GetData($"SELECT donationPublishId FROM saved_favourite_donation WHERE username = '{username}'");
                foreach (DataRow savedRow in savedDonationsTable.Rows)
                {
                    savedDonations.Add(savedRow["donationPublishId"].ToString());
                }
            }

            // add these columns, as they are from different table in database
            filteredDonations.Columns.Add("orgProfilePic", typeof(string));
            filteredDonations.Columns.Add("orgName", typeof(string));
            filteredDonations.Columns.Add("itemDetails", typeof(string));
            filteredDonations.Columns.Add("donationImages", typeof(string));
            filteredDonations.Columns.Add("donationFiles", typeof(string));
            filteredDonations.Columns.Add("urgentLabel", typeof(string));
            filteredDonations.Columns.Add("cardBody", typeof(string));
            filteredDonations.Columns.Add("saveButton");
            filteredDonations.Columns.Add("countdownEndDate");
           
            // populate the new columns
            foreach (DataRow row in filteredDonations.Rows)
            {
                string orgId = row["orgId"].ToString();
                Organization org = new Organization("", orgId, "", "", "");
                row["orgProfilePic"] = org.GetOrgProfilePic();
                row["orgName"] = org.GetOrgName();

                
                string[] itemCategories = row["itemCategory"].ToString().Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] specificItems = SplitItems(row["specificItemsForCategory"].ToString().Trim('[', ']'));
                string[] specificQty = SplitItems(row["specificQtyForCategory"].ToString().Trim('[', ']'));

                StringBuilder itemDetailsBuilder = new StringBuilder();

                for (int i = 0; i < itemCategories.Length; i++)
                {
                    // replace "null" with "Any"
                    string itemCategory = itemCategories[i].Trim();
                    if (itemCategory.Equals("null", StringComparison.OrdinalIgnoreCase))
                    {
                        itemCategory = "Any";
                    }

                    itemDetailsBuilder.Append("Item Category " + (i + 1) + " (" + itemCategory + "):<br />");

                    if (i < specificItems.Length)
                    {
                        string specificItem = specificItems[i].Trim('(', ')');
                        if (specificItem.Equals("null", StringComparison.OrdinalIgnoreCase))
                        {
                            specificItem = "Any";
                        }

                        if (!string.IsNullOrWhiteSpace(specificItem))
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: " + specificItem + "<br />");
                        }
                        else
                        {
                            itemDetailsBuilder.Append("Specific Items Needed: Any<br />");
                        }
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
                row["donationImages"] = ImageFileProcessing.ProcessImages(row["donationImage"].ToString());
                row["donationFiles"] = ImageFileProcessing.ProcessFiles(row["donationAttch"].ToString());
                DateTime approvedOn = row["approved_on"] != DBNull.Value ? Convert.ToDateTime(row["approved_on"]) : DateTime.MinValue;

                if (row["urgentStatus"].ToString().ToLower() == "yes")
                {
                    row["urgentLabel"] = @"
                        <div style='color: white; 
                                    background: red; 
                                    font-weight: bold; 
                                    text-align: left;
                                    padding: 5px 10px;
                                    height: 50px;
                                    display: flex;
                                    align-items: center;
                                    width: fit-content'>
                            <i class='fa fa-bullhorn' aria-hidden='true' style='margin-right: 8px;'></i>
                            URGENT!
                        </div>";
                    row["cardBody"] = "urgent-card";

                    int timeRangeDays = 0;
                    if (row["timeRange"] != DBNull.Value && int.TryParse(row["timeRange"].ToString(), out timeRangeDays) && timeRangeDays > 0)
                    {
                        // Add the timeRange value to the approved_on date
                        DateTime countdownEndDate = approvedOn.AddDays(timeRangeDays);
                        row["countdownEndDate"] = countdownEndDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        row["countdownEndDate"] = "";
                    }
                }
                else
                {
                    row["urgentLabel"] = "";
                   
                }

                string donationId = row["donationPublishId"].ToString();
                if (savedDonations.Contains(donationId))
                {
                    row["saveButton"] = "black";
                }
                else
                {
                    row["saveButton"] = "lightgray";
                }
            }

            
            gvAllDonations.DataSource = filteredDonations;
            gvAllDonations.DataBind();

            DisplayResultMessage(filteredDonations.Rows.Count);
        
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
            if (rptStates.Visible)
            {
                rptStates.Visible = false;
                btnFilterDonations.Visible = false;
            }
            else
            {
  
                rptStates.DataSource = new List<string>
                {
                    "Johor", "Kedah", "Kelantan", "Melaka", "Negeri Sembilan",
                    "Pahang", "Penang", "Perak", "Perlis", "Sabah", "Sarawak",
                    "Selangor", "Terengganu"
                };
                rptStates.DataBind();
                rptStates.Visible = true;
                btnFilterDonations.Visible = true;
            }
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

        private void DisplayResultMessage(int resultCount)
        {
            if (resultCount == 0)
            {
                lblNoResults.Visible = true;
                lblNoResults.Text = "No donations found matching your criteria.";
            }
            else
            {
                lblNoResults.Visible = true;
                lblNoResults.Text = $"{resultCount} result(s) found.";
            }
        }

        protected void SearchDonations(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();

            if (!string.IsNullOrEmpty(keyword))
            {
                // search organizations by keyword (org name)
                DataTable orgResults = GetOrganizationsByKeyword(keyword);

                // search donations by keyword (donation title)
                DataTable donationResults = GetDonationsByKeyword(keyword);

                // saved donations for the logged-in user
                string username = Session["username"]?.ToString();
                HashSet<string> savedDonations = new HashSet<string>();
                QRY _Qry = new QRY();

                if (!string.IsNullOrEmpty(username))
                {
                    // fetch saved donations for the user
                    DataTable savedDonationsTable = _Qry.GetData($"SELECT donationPublishId FROM saved_favourite_donation WHERE username = '{username}'");
                    foreach (DataRow savedRow in savedDonationsTable.Rows)
                    {
                        savedDonations.Add(savedRow["donationPublishId"].ToString());
                    }
                }

                // Organization Profiles 
                if (orgResults.Rows.Count > 0)
                {
                    ltOrgProfile.Text = "<h4>Organizations</h4>";

                    foreach (DataRow row in orgResults.Rows)
                    {
                        string orgId = row["orgId"].ToString();
                        string orgName = row["orgName"].ToString();
                        string orgProfilePicBase64 = row["orgProfilePicBase64"]?.ToString();
                        string orgProfilePic = !string.IsNullOrEmpty(orgProfilePicBase64)
                            ? "data:image/png;base64," + orgProfilePicBase64
                            : "/Image/default_picture.jpg";


                        // organization profile as a clickable box
                        ltOrgProfile.Text += $@"
                    <div style='border: 2px solid black; padding: 10px; display: flex; align-items: center; margin-bottom: 15px;'>
                        <a href='PreviewPublicInfo.aspx?role=organization&orgName={orgName}' style='text-decoration:none; color:black; display:flex; align-items:center;'>
                            <img src='{orgProfilePic}' alt='Org Profile' style='width:50px;height:50px;margin-right:10px;' />
                            <span>{orgName}</span>
                        </a>
                    </div>";
                    }
                }
                else
                {
                    ltOrgProfile.Text = "";
                }

                // Donations
                if (donationResults.Rows.Count > 0)
                {
                    ltDonation.Text = "<h4>Donations</h4>";
                   
                    donationResults.Columns.Add("orgProfilePic", typeof(string));
                    donationResults.Columns.Add("orgName", typeof(string));
                    donationResults.Columns.Add("itemDetails", typeof(string));
                    donationResults.Columns.Add("donationImages", typeof(string));
                    donationResults.Columns.Add("donationFiles", typeof(string));
                    donationResults.Columns.Add("urgentLabel", typeof(string));
                    donationResults.Columns.Add("cardBody", typeof(string));
                    donationResults.Columns.Add("saveButton");
                    donationResults.Columns.Add("countdownEndDate");
                    

                    foreach (DataRow row in donationResults.Rows)
                    {
                        string orgId = row["orgId"].ToString();
                        Organization org = new Organization("", orgId, "", "", "");
                        row["orgProfilePic"] = org.GetOrgProfilePic();
                        row["orgName"] = org.GetOrgName();

                        
                        string[] itemCategories = row["itemCategory"].ToString().Trim('[', ']').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] specificItems = SplitItems(row["specificItemsForCategory"].ToString().Trim('[', ']'));
                        string[] specificQty = SplitItems(row["specificQtyForCategory"].ToString().Trim('[', ']'));

                        StringBuilder itemDetailsBuilder = new StringBuilder();

                        for (int i = 0; i < itemCategories.Length; i++)
                        {
                            string itemCategory = itemCategories[i].Trim();
                            if (itemCategory.Equals("null", StringComparison.OrdinalIgnoreCase))
                            {
                                itemCategory = "Any";
                            }

                            itemDetailsBuilder.Append("Item Category " + (i + 1) + " (" + itemCategory + "):<br />");

                            if (i < specificItems.Length)
                            {
                                string specificItem = specificItems[i].Trim('(', ')');
                                if (specificItem.Equals("null", StringComparison.OrdinalIgnoreCase))
                                {
                                    specificItem = "Any";
                                }

                                if (!string.IsNullOrWhiteSpace(specificItem))
                                {
                                    itemDetailsBuilder.Append("Specific Items Needed: " + specificItem + "<br />");
                                }
                                else
                                {
                                    itemDetailsBuilder.Append("Specific Items Needed: Any<br />");
                                }
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
                        row["donationImages"] = ImageFileProcessing.ProcessImages(row["donationImage"].ToString());
                        row["donationFiles"] = ImageFileProcessing.ProcessFiles(row["donationAttch"].ToString());
                        DateTime approvedOn = row["approved_on"] != DBNull.Value ? Convert.ToDateTime(row["approved_on"]) : DateTime.MinValue;

                        if (row["urgentStatus"].ToString().ToLower() == "yes")
                        {
                            row["urgentLabel"] = @"
                        <div style='color: white; 
                                    background: red; 
                                    font-weight: bold; 
                                    text-align: left;
                                    padding: 5px 10px;
                                    height: 50px;
                                    display: flex;
                                    align-items: center;
                                    width: fit-content'>
                            <i class='fa fa-bullhorn' aria-hidden='true' style='margin-right: 8px;'></i>
                            URGENT!
                        </div>";
                            row["cardBody"] = "urgent-card";

                            int timeRangeDays = 0;
                            if (row["timeRange"] != DBNull.Value && int.TryParse(row["timeRange"].ToString(), out timeRangeDays) && timeRangeDays > 0)
                            {
                                // add the timeRange value to the approved_on date
                                DateTime countdownEndDate = approvedOn.AddDays(timeRangeDays);
                                row["countdownEndDate"] = countdownEndDate.ToString("yyyy-MM-ddTHH:mm:ss");
                            }
                            else
                            {
                                row["countdownEndDate"] = "";
                            }


                        }
                        else
                        {
                            row["urgentLabel"] = "";
                        }

                        string donationId = row["donationPublishId"].ToString();
                        if (savedDonations.Contains(donationId))
                        {
                            row["saveButton"] = "black";
                        }
                        else
                        {
                            row["saveButton"] = "lightgray";
                        }
                    }

                   
                    gvAllDonations.DataSource = donationResults;
                    gvAllDonations.DataBind();
                }
                else
                {
                    gvAllDonations.DataSource = null;
                    gvAllDonations.DataBind();
                }

                DisplayResultMessage(donationResults.Rows.Count);
            }
        }

        public DataTable GetOrganizationsByKeyword(string keyword)
        {
            // search for the keyword in the organization's name
            string sql = "SELECT orgId, orgName, orgProfilePicBase64 FROM organization WHERE LOWER(orgName) LIKE '%" + keyword.ToLower() + "%' AND orgStatus != 'Pending Approval'";

            QRY _Qry = new QRY();
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@keyword", keyword }
            };
            DataTable result = _Qry.GetData(sql, parameter);

            
            return result;
        }


        private DataTable GetDonationsByKeyword(string keyword)
        {
            QRY _Qry = new QRY();

            string lowerKeyword = keyword.ToLower();

            string sqlOrgId = "SELECT orgId FROM organization WHERE LOWER(orgName) LIKE '%" + lowerKeyword + "%'";

            // fetch orgIds from the database
            DataTable orgIdTable = _Qry.GetData(sqlOrgId);

            List<string> orgIds = new List<string>();
            foreach (DataRow row in orgIdTable.Rows)
            {
                orgIds.Add(row["orgId"].ToString());
            }
            string orgIdList = orgIds.Count > 0 ? string.Join(",", orgIds.Select(id => "'" + id + "'")) : "NULL";

            string status = "Opened";

            // find donations by title or matching orgIds
            string sqlDonations = "SELECT * FROM donation_publish " +
                          "WHERE (LOWER(title) LIKE '%" + lowerKeyword + "%' " +
                          "OR orgId IN (" + orgIdList + ")) " +
                          "AND status = '" + status + "'";

            DataTable dt = _Qry.GetData(sqlDonations);

            return dt;
        }

        [System.Web.Services.WebMethod]
        public static List<Dictionary<string, string>> GetActiveOrganizationAddresses()
        {
            List<Dictionary<string, string>> organizationInfo = new List<Dictionary<string, string>>();

            QRY _Qry = new QRY();

            string sql = "SELECT orgId, orgName, orgAddress FROM organization WHERE orgStatus = 'Active'";

            DataTable _dt = _Qry.GetData(sql);

            foreach (DataRow row in _dt.Rows)
            {
                var organization = new Dictionary<string, string>
            {
                { "orgId", row["orgId"].ToString() },
                { "orgName", row["orgName"].ToString() },
                { "orgAddress", row["orgAddress"].ToString() }
            };
                organizationInfo.Add(organization);
            }

            return organizationInfo;
        }

        [System.Web.Services.WebMethod]
        public static List<Dictionary<string, string>> GetDonationsByOrg(string orgName)
        {
            List<Dictionary<string, string>> donationList = new List<Dictionary<string, string>>();

            QRY _Qry = new QRY();
            Organization org = new Organization(orgName, "", "", "", "");
            string orgId = org.GetOrgId();

            if (!string.IsNullOrEmpty(orgId))
            {

                string sql = "SELECT title, itemCategory FROM donation_publish WHERE orgId = @orgId AND status = 'Opened'";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@orgId", orgId }
                };

                DataTable _dt = _Qry.GetData(sql, parameters);

                foreach (DataRow row in _dt.Rows)
                {
                    string itemCategory = row["itemCategory"].ToString().Trim('[', ']');
                    var donation = new Dictionary<string, string>
                    {
                        { "title", row["title"].ToString() },
                        { "itemCategory", itemCategory } 
                    };
                    donationList.Add(donation);
                }
            }

            return donationList;
        }

        protected void btnDonate_Click(object sender, EventArgs e)
        {
            
            if (Session["username"] == null)
            {
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showErrorMsg('Please login or sign up if you do not have an account yet to donate items to this organization.');", true);
            }
            else
            {
                // check if the user is a donor
                string role = Session["role"]?.ToString();
                if (role != "donor")
                {
                    // user is logged in but not a donor
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showInfo('Only donor accounts can donate items to this organization.');", true);
                }
                else
                {
                    // user is a donor, proceed with donation
                    Button btn = (Button)sender;
                    string donationPublishId = btn.CommandArgument;
                    string username = Session["username"].ToString();

                    Response.Redirect($"DonationRequest.aspx?donationPublishId={donationPublishId}");
                }
            }
        }

        protected void btnSaveFav_Click(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                // user is not logged in
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showErrorMsg('Please login or sign up if you " +
                    "do not have an account yet to save this donation to the cart.');", true);
            }
            else
            {
                // check if the user is a donor
                string role = Session["role"]?.ToString();
                if (role != "donor")
                {
                    // user is logged in but not a donor
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showInfo('Only donor accounts can save donations to favorites.');", true);
                }
                else
                {
                    // user is a donor, proceed with saving to favorites
                    string username = Session["username"].ToString();
                    LinkButton btn = (LinkButton)sender;
                    string donationPublishId = btn.CommandArgument;

                    // check if the donation is already saved
                    if (IsDonationAlreadySaved(username, donationPublishId))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showErrorMsg('This donation is already saved to your favorites.');", true);
                    }
                    else
                    {
                        // save donation to favorites
                        SaveToFavorite(username, donationPublishId);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation has been added to your favorites.');", true);
                    }
                }
            }
        }


        private bool IsDonationAlreadySaved(string username, string donationPublishId)
        {
           
            QRY _Qry = new QRY();
            string strSQL = "SELECT COUNT(*) FROM saved_favourite_donation WHERE username = @Username AND donationPublishId = @DonationPublishId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Username", username },
                { "@DonationPublishId", donationPublishId }
            };

            string count = _Qry.GetScalarValue(strSQL, parameters);

            int countInt = 0;

            if (!int.TryParse(count, out countInt))
            {               
                countInt = 0; 
            }

            return countInt > 0;
        }

        private void SaveToFavorite(string username, string donationPublishId)
        {
            QRY _Qry = new QRY();
            string id = GetCurrentSavedDonationId(_Qry);

            string strSQL = "INSERT INTO saved_favourite_donation(savedDonationId, username, donationPublishId, savedOn) VALUES (@SavedDonationId, @Username, @DonationPublishId, GETDATE())";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@SavedDonationId", id },
                { "@Username", username },
                { "@DonationPublishId", donationPublishId }
            };

           bool success= _Qry.ExecuteNonQuery(strSQL, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation saved to favorites successfully.');", true);
                
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showErrorMsg('There was an error saving the donation. Please try again!');", true);
            }
        }

        private string GetCurrentSavedDonationId(QRY qry)
        {
            string sql = "SELECT TOP 1 savedDonationId FROM saved_favourite_donation ORDER BY savedDonationId DESC";
            DataTable dt = qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                string latestId = dt.Rows[0]["savedDonationId"].ToString();
               
                int latestNum = int.Parse(latestId.Substring(1));
                int newNum = latestNum + 1;
                return "S" + newNum.ToString();
            }
            else
            {
                // when no IDs exist, start with 'S1'
                return "S1";
            }
        }

        [WebMethod]
        public static void UpdateCountdown(string donationId)
        {
            string sql = "UPDATE donation_publish SET countdownEnded = @currentTime, status = @status WHERE donationPublishId = @donationId";

            var parameters = new Dictionary<string, object>
            {
                { "@currentTime", DateTime.Now },
                { "@donationId", donationId },
                { "@status", "Ended" }
            };

            QRY _Qry = new QRY();

            string orgId = GetOrgId(donationId);

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                string sqlNtf = "EXEC [create_notifications] " +
                                "@method = 'INSERT', " +
                                "@id = NULL, " +
                                "@userId = @userId, " +
                                "@link = @link, " +
                                "@content = @content";

                string link = $"EditOrgDonations.aspx?donationPublishId={donationId}&orgId={orgId}&urgent=done";
                string encryptedLink = Encryption.Encrypt(link);

                string message = "Your urgent donation countdown timer is ended. Have you received enough needed items? If not, you can republish the donation again.";

                var notificationParameter = new Dictionary<string, object>
                {
                    { "@userId", orgId },
                    { "@link", encryptedLink },
                    { "@content", message }
                };

                _Qry.ExecuteNonQuery(sqlNtf, notificationParameter);

                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'URGENT DONATION', " +
                                  "@role = 'organization', " +
                                  "@orgId = @orgId, " +
                                  "@donationpublishid = @donationPublishId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@orgId", orgId },
                    { "@donationPublishId", donationId }
                };

                _Qry.ExecuteNonQuery(sqlemail, emailParameter);
            }
        }

        public static string GetOrgId(string donationPublishId)
        {
            QRY _Qry = new QRY();
            string orgId = "";

            try
            {
                string sql = "SELECT orgId FROM donation_publish WHERE donationPublishId = @donationPublishId";

                var parameters = new Dictionary<string, object>
                {
                    { "@donationPublishId", donationPublishId }               
                };

                DataTable result = _Qry.GetData(sql, parameters);

                if (result.Rows.Count > 0)
                {
                    orgId = result.Rows[0]["orgId"].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return orgId;
        }

    }
}
