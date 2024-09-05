using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace DonorConnect
{
    public partial class Favourites : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindFavoritesGridView();
            }
        }

        private void BindFavoritesGridView()
        {
            if (Session["username"] != null)
            {
                string username = Session["username"].ToString();
                DataTable favoriteDonations = GetFavoriteDonations(username);

                if (favoriteDonations.Rows.Count == 0)
                {
                    // no saved donations
                    noSavedFav.Visible = true;
                    gvFavourites.Visible = false;
                }
                else
                {
                    // saved donations found
                    noSavedFav.Visible = false;
                    gvFavourites.Visible = true;


                    favoriteDonations.Columns.Add("itemDetails");
                    favoriteDonations.Columns.Add("donationImages");
                    favoriteDonations.Columns.Add("donationFiles");
                    favoriteDonations.Columns.Add("orgProfilePic");
                    favoriteDonations.Columns.Add("orgName");
                    favoriteDonations.Columns.Add("urgentLabel");
                    favoriteDonations.Columns.Add("cardBody");

                    foreach (DataRow row in favoriteDonations.Rows)
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

                            itemDetailsBuilder.Append("Item Category " + (i + 1) + " (" + category + "):<br />");

                            if (i < specificItems.Length && !string.IsNullOrWhiteSpace(specificItems[i]))
                            {
                                itemDetailsBuilder.Append("Specific Items Needed: " + items + "<br />");
                            }
                            else
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

                        row["itemDetails"] = itemDetailsBuilder.ToString();
                        row["donationImages"] = ImageFileProcessing.ProcessImages(row["donationImage"].ToString());
                        row["donationFiles"] = ImageFileProcessing.ProcessFiles(row["donationAttch"].ToString());
                        row["orgProfilePic"] = profilePic;
                        row["orgName"] = orgName;

                        if (isUrgent)
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
                        }
                        else
                        {
                            row["urgentLabel"] = "";
                        }
                    }
                }
                gvFavourites.DataSource = favoriteDonations;
                gvFavourites.DataBind();
            }
            else
            {
                gvFavourites.DataSource = null;
                gvFavourites.DataBind();
            }
        }


        private DataTable GetFavoriteDonations(string username)
        {
            QRY _Qry = new QRY();
            string strSQL = @"SELECT dp.*
                      FROM donation_publish dp
                      INNER JOIN saved_favourite_donation fd ON dp.donationPublishId = fd.donationPublishId
                      WHERE fd.username = @Username";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {  
                { "@Username", username }
               
            };
            return _Qry.GetData(strSQL, parameters);
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
                string colorClass = GetBorderColorForCategory(category); 

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

        protected void btnDeleteFav_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
               
                string username = Session["username"].ToString();
                LinkButton btn = (LinkButton)sender;
                string donationPublishId = btn.CommandArgument;

                ScriptManager.RegisterStartupScript(this, GetType(), "confirmDelete",
                 $"confirmDelete('{donationPublishId}');", true);

            }
        }

        [System.Web.Services.WebMethod]
        public static string DeleteFavorite(string donationPublishId)
        {
            try
            {
                QRY _Qry = new QRY();
                string username = HttpContext.Current.Session["username"]?.ToString();
                string sql = "DELETE FROM saved_favourite_donation WHERE donationPublishId = @donationPublishId AND username = @username";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@donationPublishId", donationPublishId },
                    { "@username", username }
                };

                bool success = _Qry.ExecuteNonQuery(sql, parameters);

                if (success)
                {
                    
                    return "Success";
                    
                }
                else
                {
                    return "There was a problem deleting the item.";
                }


                
            }
            catch (Exception ex)
            {
              
                return "Error" + ex.Message;
            }
        }


    }
}