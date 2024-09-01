using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminViewDonationRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string reject = Request.QueryString["reject"];

            if (!string.IsNullOrEmpty(id))
            {   
                string donationPublishId = id;


                BindDonationGridView(donationPublishId);

                
            }

        }

        private void BindDonationGridView(string donationPublishId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            string sql = @"
            SELECT 
                'ID' AS FieldName, CAST(donationPublishId AS NVARCHAR(MAX)) AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Organization' AS FieldName, orgId AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Title' AS FieldName, title AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'People Needed' AS FieldName, peopleNeeded AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Address' AS FieldName, address AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Description' AS FieldName, description AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Item Categories' AS FieldName, itemCategory AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Specific Items Needed' AS FieldName, specificItemsForCategory AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Specific Quantities Needed' AS FieldName, specificQtyForCategory AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Time Range' AS FieldName, timeRange AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Image(s)' AS FieldName, donationImage AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Attachment(s)' AS FieldName, donationAttch AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Restriction' AS FieldName, restriction AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Date Registered' AS FieldName, CAST(created_on AS NVARCHAR(MAX)) AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Status' AS FieldName, status AS FieldValue 
            FROM 
                donation_publish
            WHERE 
                donationPublishId = '" + donationPublishId.Replace("'", "''") + @"'
            ";

            _dt = _Qry.GetData(sql);

            
          
            foreach (DataRow row in _dt.Rows)
            {
                
                if (row["FieldName"].ToString() == "Organization")
                {
                    string id = row["FieldValue"].ToString();
                    Organization org = new Organization("", id, "", "", "");
                    string orgName = org.GetOrgName();
                    row["FieldValue"] = orgName;
                }

                // decrypt and process images and attachments
                if (row["FieldName"].ToString() == "Image(s)")
                {
                    string decryptedImages = row["FieldValue"].ToString();
                    string processedImagesHtml = ImageFileProcessing.ProcessImages(decryptedImages);
                    row["FieldValue"] = processedImagesHtml;
                }

                if (row["FieldName"].ToString() == "Attachment(s)")
                {
                    string decryptedFile = row["FieldValue"].ToString();
                    string processedFileHtml = ImageFileProcessing.ProcessImages(decryptedFile);
                    row["FieldValue"] = processedFileHtml;
                }
            }

            rptDonation.DataSource = _dt;
            rptDonation.DataBind();
        }

        private bool CheckCategoryExist(string categoryName)
        {
           
            string sql = "SELECT COUNT(*) AS CategoryCount FROM itemCategory WHERE categoryName = '" + categoryName.Replace("'", "''") + "'";
            QRY _Qry = new QRY();

            DataTable dt = _Qry.GetData(sql);

            
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0]["CategoryCount"]);
                return count > 0;
            }

            // category does not exist
            return false;
        }


        private bool CheckItemExist(string categoryName, string item)
        {
            QRY _Qry = new QRY();

            // check if the item exists in the specificItems in database 
            string sql = @"
            SELECT COUNT(*) 
            FROM itemCategory 
            WHERE categoryName = '" + categoryName.Replace("'", "''") + @"' 
            AND specificItems LIKE '%" + item.Replace("'", "''") + @"%'";

            // Execute the query and retrieve the count
            DataTable dt = _Qry.GetData(sql);

            // check if the item exists for the particular category based on the count
            if (dt.Rows.Count > 0)
            {
                int count = Convert.ToInt32(dt.Rows[0][0]);
                return count > 0;
            }

            return false; // item not found
        }


        protected void rptDonation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptCategoryDetails = (Repeater)e.Item.FindControl("rptCategoryDetails");
                string fieldName = DataBinder.Eval(e.Item.DataItem, "FieldName").ToString();
                string fieldValue = DataBinder.Eval(e.Item.DataItem, "FieldValue").ToString();

                // process categories
                if (fieldName == "Item Categories")
                {
                    string specificItems = GetFieldValue(e.Item, "Specific Items Needed");
                    string specificQuantities = GetFieldValue(e.Item, "Specific Quantities Needed");

                    string[] categories = SplitItems(fieldValue);
                    string[] items = SplitItems(specificItems);
                    string[] quantities = SplitItems(specificQuantities);

                    DataTable categoryDetailsTable = new DataTable();
                    categoryDetailsTable.Columns.Add("Category");
                    categoryDetailsTable.Columns.Add("SpecificItems");
                    categoryDetailsTable.Columns.Add("Quantity");
                    categoryDetailsTable.Columns.Add("CategoryExists");
                    categoryDetailsTable.Columns.Add("AddCategoryIcon");
                    categoryDetailsTable.Columns.Add("AddItemIcon");
                    categoryDetailsTable.Columns.Add("InfoIcon");

                    for (int i = 0; i < categories.Length; i++)
                    {
                        DataRow row = categoryDetailsTable.NewRow();
                        string category = (i < categories.Length) ? RemoveString(categories[i]).Trim() : "";
                        bool categoryExists = CheckCategoryExist(category);

                        row["Category"] = category;
                        row["SpecificItems"] = (i < items.Length) ? RemoveString(items[i]).Trim() : "";
                        row["Quantity"] = (i < quantities.Length) ? RemoveString(quantities[i]).Trim() : "";
                        row["CategoryExists"] = categoryExists ? "Yes" : "No";
                        row["AddCategoryIcon"] = categoryExists ? "" : "Yes"; 

                        bool newItemExists = false;  

                        if (!string.IsNullOrEmpty(row["SpecificItems"].ToString()))
                        {
                            string[] specificItemList = row["SpecificItems"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            // Loop through items and check existence
                            foreach (string item in specificItemList)
                            {
                                if (!CheckItemExist(category, item.Trim()))
                                {
                                    newItemExists = true;  // if any item is not found
                                    break;  
                                }
                            }
                        }

                        // icons only display if any item was new to the category
                        row["AddItemIcon"] = newItemExists ? "Yes" : ""; 
                        row["InfoIcon"] = newItemExists ? "Yes" : ""; 


                        categoryDetailsTable.Rows.Add(row);
                    }

                    rptCategoryDetails.DataSource = categoryDetailsTable;
                    rptCategoryDetails.DataBind();
                }

              
                // hide or show category based on data presence
                var categoryRow = (HtmlGenericControl)e.Item.FindControl("categoryRow");
                categoryRow.Visible = fieldName == "Item Categories";

                LinkButton btnApprove = (LinkButton)e.Item.FindControl("btnApprove");
                LinkButton btnReject = (LinkButton)e.Item.FindControl("btnReject");

                string reject = Request.QueryString["reject"];


                if (reject == "Yes")
                {
                    if (btnApprove != null)
                    {
                        btnApprove.Visible = false;
                    }

                    if (btnReject != null)
                    {
                        btnReject.Visible = false;
                    }
                    else
                    {
                        if (btnApprove != null)
                        {
                            btnApprove.Visible = true;

                            if (btnReject != null)
                            {
                                btnReject.Visible = true;
                            }
                        }

                    }
                }

                string urgency = Request.QueryString["urgent"];

             
                // check urgency and hide relevant fields
                if (urgency == "No" && (fieldName == "Time Range" || fieldName == "Specific Quantities Needed"))
                {
                    e.Item.Visible = false;
                }

                var litFieldName = (Literal)e.Item.FindControl("litLabel");
                var litFieldValue = (Literal)e.Item.FindControl("litValue");
                
              
                if (litFieldName != null && litFieldValue != null)
                {
                    if (fieldName == "Item Categories" || fieldName == "Specific Items Needed" || fieldName == "Specific Quantities Needed")
                    {
                        litFieldName.Visible = false;
                        litFieldValue.Visible = false;
                    }
                    
                }

                var phQuantityHeader = (PlaceHolder)e.Item.FindControl("phQuantityHeader");

                if (phQuantityHeader != null && urgency == "Yes")
                {
                    phQuantityHeader.Visible = true;
                }

                foreach (RepeaterItem item in rptCategoryDetails.Items)
                {
                    var txtQuantity = (TextBox)item.FindControl("txtQuantity");
                    if (txtQuantity != null && urgency == "Yes")
                    {
                        txtQuantity.Visible = true;
                    }
                }



            }

        }

        protected void AddNewCategory(object sender, CommandEventArgs e)
        {
            string category = e.CommandArgument.ToString();
            string id = Request.QueryString["id"];

            // insert new category
            string sql = "INSERT INTO [itemCategory] (categoryName) VALUES (@CategoryName)";

            
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", category }
            };

           
            QRY qry = new QRY();
            bool success = qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('New category added successfully!');", true);
                BindDonationGridView(id);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error adding new category. Please try again!');", true);
            }
        }



        protected void AddNewItem(object sender, CommandEventArgs e)
        {
            string[] args = e.CommandArgument.ToString().Split(';');
            string category = args[0];
            string items = args.Length > 1 ? args[1] : "";
            string id = Request.QueryString["id"];

            // get existing items for the category
            string sqlSelect = "SELECT specificItems FROM itemCategory WHERE categoryName = @CategoryName";
            var parametersSelect = new Dictionary<string, object>
            {
                { "@CategoryName", category }
            };

            QRY qry = new QRY();
            DataTable dt = qry.GetData(sqlSelect, parametersSelect);

            string existingItems = string.Empty;
            if (dt.Rows.Count > 0)
            {
                existingItems = dt.Rows[0]["specificItems"].ToString();
            }

            // store as newitems, added to exisingitems, if there is any existing items found
            string[] itemList = items.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string newItems = string.Join(",", itemList);
            if (!string.IsNullOrEmpty(existingItems))
            {
                newItems = $"{newItems}";
            }

            
            string sql;
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", category },
                { "@SpecificItems", newItems }
            };

            if (dt.Rows.Count > 0)
            {
                // update existing category with new items
                sql = "UPDATE [itemCategory] SET specificItems = @SpecificItems WHERE categoryName = @CategoryName";
            }
            else
            {
                // insert new category together with items if category not yet exist
                sql = "INSERT INTO [itemCategory] (categoryName, specificItems) VALUES (@CategoryName, @SpecificItems)";
            }

            bool success = qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Items updated successfully!');", true);
                BindDonationGridView(id);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating items. Please try again!');", true);
            }
        }




        private string GetFieldValue(RepeaterItem item, string fieldName)
        {
            var data = ((DataRowView)item.DataItem).Row;
            return data.Table.Select($"FieldName = '{fieldName}'").FirstOrDefault()?["FieldValue"].ToString() ?? "";
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
                    if (c == '(') openBracketCount++;
                    if (c == ')') openBracketCount--;

                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
                items.Add(sb.ToString().Trim());

            return items.ToArray();
        }

        private string RemoveString(string input)
        {
            return input.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Trim();
        }


        protected void btnApprove_click(object sender, EventArgs e)
        {
            string donationPublishId = Session["SelectedDonationPublishId"] as string;
            string adminId = Session["username"].ToString();
            string status = "Opened";
            DonationPublish dp = new DonationPublish(donationPublishId, "", "", "", "", "", "");
            string id = dp.GetId();

            string sql = "UPDATE [donation_publish] SET " +
                                "status = '" + status + "', " +
                                "adminId = '" + adminId + "' " +
                                 "WHERE donationPublishId = '" + donationPublishId + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);

            if (success)
            {
                // send email notify organization
                string sqlemail;

                QRY _Qry2 = new QRY();


                sqlemail = "EXEC [application_email] " +
                             "@action = 'DONATION APPROVED', " +
                             "@reason = NULL, " +
                             "@role = 'organization', " +
                             "@resubmitlink = NULL, " +
                             "@orgId = '" + id + "', " +
                             "@donationpublishid = '" + donationPublishId + "' ";

                _Qry2.ExecuteNonQuery(sqlemail);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation request approved successfully!',);", true);

                BindDonationGridView(donationPublishId);


            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error approving donation request. Please try again!');", true);
            }
        }

      
        protected void btnReject_Click(object sender, EventArgs e)
        {
            string donationPublishId = Session["SelectedDonationPublishId"] as string;
            string adminId = Session["username"].ToString();
            string rejectionReason = txtReason.Text;
            string status = "Rejected";
            DonationPublish dp = new DonationPublish(donationPublishId, "", "", "", "", "", "");
            string id= dp.GetId();
            bool isRejected = false;


            if (!string.IsNullOrEmpty(donationPublishId) && !string.IsNullOrEmpty(rejectionReason))
            {

                string sql2 = "UPDATE [donation_publish] SET " +
                              "status = '" + status + "', " +
                              "adminId = '" + adminId + "', " +
                              "rejectedReason = '" + rejectionReason + "' " +    
                              "WHERE donationPublishId = '" + donationPublishId + "'";

                QRY _Qry2 = new QRY();
                bool success2 = _Qry2.ExecuteNonQuery(sql2);

                if (success2)
                {


                    string sqlemail = "EXEC [application_email] " +
                                      "@action = 'DONATION REJECTED', " +
                                      "@reason = '" + rejectionReason + "', " +
                                      "@role = 'organization', " +
                                      "@orgId = '" + id + "', " +
                                      "@donationPublishId = '" + donationPublishId + "', " +
                                      "@resubmitlink = NULL ";

                    _Qry2.ExecuteNonQuery(sqlemail);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Application is rejected! An email is sent to inform them about this.',);", true);

                    BindDonationGridView(donationPublishId);
                    isRejected = true;

                }
            }

            if (isRejected)
            {
                Session["SelectedDonationPublishId"] = null;
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error rejecting the application. Please try again!');", true);
            }
        }


       

    }
}