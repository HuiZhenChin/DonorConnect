using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class OrgReviewDonationRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string donationId = Request.QueryString["donationId"];
               
                if (donationId != null)
                {
                    Session["donationId"] = donationId;
                    BindDonationGridView(donationId);
                    LoadDonationItems(donationId);
                }
            }
        }

        private void BindDonationGridView(string donationId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            string sql = @"
            SELECT 
                'ID' AS FieldName, MAX(donationId) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Donor' AS FieldName, MAX(donorFullName) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Email Address' AS FieldName, MAX(donorEmail) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Contact Number' AS FieldName, MAX(donorPhone) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Pick Up Address' AS FieldName, MAX(pickUpAddress) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'State' AS FieldName, MAX(state) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Description' AS FieldName, MAX(description) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Image(s)' AS FieldName, MAX(itemImage) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Submitted On' AS FieldName, CAST(MAX(created_on) AS NVARCHAR(MAX)) AS FieldValue 
            FROM 
                donation_item_request
            WHERE 
                donationId = '" + donationId.Replace("'", "''") + @"'          
            ";

            _dt = _Qry.GetData(sql);

            foreach (DataRow row in _dt.Rows)
            {
                // check if the field is an image
                if (row["FieldName"].ToString() == "Image(s)")
                {
                    string base64ImageData = row["FieldValue"].ToString();

                    // assume that multiple Base64 images are separated by commas                   
                    List<string> base64Images = new List<string>();

                    // check if the Base64 data is an array or a single string
                    if (base64ImageData.StartsWith("[") && base64ImageData.EndsWith("]"))
                    {
                        // parse JSON array of Base64 images 
                        base64Images = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(base64ImageData);
                    }
                    else
                    {
                        // split if multiple Base64 images are stored as comma-separated strings
                        base64Images = base64ImageData.Split(',').ToList();
                    }

                    // StringBuilder to hold the HTML for all images
                    var imgHtmlBuilder = new StringBuilder();

                    foreach (var base64Image in base64Images)
                    {
                        string processedBase64Image = base64Image.Trim();

                        // remove any Base64 image type prefix if present
                        if (processedBase64Image.StartsWith("data:image/jpeg;base64,"))
                        {
                            processedBase64Image = processedBase64Image.Replace("data:image/jpeg;base64,", string.Empty);
                        }
                        else if (processedBase64Image.StartsWith("data:image/png;base64,"))
                        {
                            processedBase64Image = processedBase64Image.Replace("data:image/png;base64,", string.Empty);
                        }

                      
                        string imageUrl = "data:image/jpeg;base64," + processedBase64Image;

                        // generate the <img> tag for this image
                        string imgHtml = $"<img src='{imageUrl}' alt='Image' style='max-width:300px; max-height:300px; margin:5px;' />";

                        // append the HTML to the StringBuilder
                        imgHtmlBuilder.Append(imgHtml);
                    }

                   
                    row["FieldValue"] = imgHtmlBuilder.ToString();
                }
            }


        rptDonation.DataSource = _dt;
        rptDonation.DataBind();
        }

    
        protected void rptDonation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {

            string donationId = Request.QueryString["donationId"];
            string username = Session["username"].ToString();
            string status = "Approved";

            string sql = "UPDATE [donation_item_request] SET " +
                                "requestStatus = '" + status + "' " +
                                 "WHERE donationId = '" + donationId + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);

            if (success)
            {
                

                string sqlNtf;

                QRY _Qry2 = new QRY();

                string sqlSelect = "SELECT donorId, donationPublishId, pickUpAddress, orgId FROM donation_item_request WHERE donationId = @donationId";
                var parametersSelect = new Dictionary<string, object>
                {
                    { "@donationId", donationId }

                };

                QRY qry = new QRY();
                DataTable dt = qry.GetData(sqlSelect, parametersSelect);

                string donorId = "";
                string donationPublishId = "";
                string pickupAddress = "";
                string orgId = "";

                if (dt.Rows.Count > 0)
                {
                    donorId = dt.Rows[0]["donorId"].ToString();
                    donationPublishId = dt.Rows[0]["donationPublishId"].ToString();
                    pickupAddress = dt.Rows[0]["pickUpAddress"].ToString();
                    orgId = dt.Rows[0]["orgId"].ToString();
                }

                string message = "Greetings, your donation request for " + username + " has been APPROVED. Please click here to proceed with delivery pick-up and payment. Thank you for donating!";
                string token = Hash(username + DateTime.Now.ToString());
                token = token.Replace("+", "A");
                string link = $"DonationRequest.aspx?donationPublishId={donationPublishId}&donationId={donationId}&status={status}&token={token}";
                string encryptedLink = Encryption.Encrypt(link);
                string fullLink = "https://localhost:44390/DonationRequest.aspx?donationPublishId=" + donationPublishId +
                 "&donationId=" + donationId +
                 "&status=" + status +
                 "&token=" + token;


                sqlNtf = "EXEC [create_notifications] " +
                             "@method = 'INSERT', " +
                             "@id = NULL, " +
                             "@userId = '" + donorId + "', " +
                             "@link = '" + encryptedLink + "', " +
                             "@supportingId = '" + donationId + "', " +
                             "@content = '" + message + "' ";


                _Qry2.ExecuteNonQuery(sqlNtf);

                QRY _Qry3 = new QRY();

                // send email notify organization
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'DONATION REQUEST APPROVED', " +
                                  "@role = 'donor', " +
                                  "@resubmitlink = @link, " +
                                  "@orgId = @orgId, " +
                                  "@donorId = @donorId, " +
                                  "@donationpublishid = @donationPublishId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@link", fullLink },
                    { "@orgId", orgId },
                    { "@donorId", donorId },
                    { "@donationPublishId", donationPublishId }
                };

                _Qry3.ExecuteNonQuery(sqlemail, emailParameter);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
             $"approveDonation('{donationPublishId}');", true);

                Session["PickupAddress"] = pickupAddress;


            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error approving donation request. Please try again!');", true);
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string donationId = Request.QueryString["donationId"];
            string username = Session["username"].ToString();

            string rejectionReason = ddlRejectionReason.SelectedValue == "Others" ? txtOtherReason.Text : ddlRejectionReason.SelectedItem.Text;
            string status = "Rejected";

            // update donation_item_request table
            string sql = "UPDATE [donation_item_request] SET " +
                         "requestStatus = @status, " +
                         "reasonOfRejection = @rejectionReason " +
                         "WHERE donationId = @donationId";

            var parameters = new Dictionary<string, object>
            {
                { "@status", status },
                { "@rejectionReason", rejectionReason },
                { "@donationId", donationId }
            };

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                // check and delete rows from donation_item_expiry_date table related to the donationId
                string checkExpiryDateSql = "SELECT COUNT(*) FROM [donation_item_expiry_date] WHERE donationId = @donationId";
                var expiryParameters = new Dictionary<string, object>
                {
                    { "@donationId", donationId }
                };

                int count = (int)_Qry.ExecuteScalar(checkExpiryDateSql, expiryParameters);

                if (count > 0)
                {
                    string deleteExpirySql = "DELETE FROM [donation_item_expiry_date] WHERE donationId = @donationId";
                    _Qry.ExecuteNonQuery(deleteExpirySql, expiryParameters);
                }

                // fetch donorId, donationPublishId, and orgId from donation_item_request table
                string sqlSelect = "SELECT donorId, donationPublishId, orgId FROM donation_item_request WHERE donationId = @donationId";
                DataTable dt = _Qry.GetData(sqlSelect, expiryParameters);

                string donorId = "";
                string donationPublishId = "";
                string orgId = "";

                if (dt.Rows.Count > 0)
                {
                    donorId = dt.Rows[0]["donorId"].ToString();
                    donationPublishId = dt.Rows[0]["donationPublishId"].ToString();
                    orgId = dt.Rows[0]["orgId"].ToString();
                }

                // send notifications and email
                string message = "Greetings, your donation request for " + username + " has been REJECTED. This is the reason provided: " + rejectionReason + ".";
                string link = $"DonationRequest.aspx?donationPublishId={donationPublishId}";
                string encryptedLink = Encryption.Encrypt(link);

                string sqlNtf = "EXEC [create_notifications] " +
                                "@method = 'INSERT', " +
                                "@id = NULL, " +
                                "@userId = @userId, " +
                                "@link = @link, " +
                                "@content = @content";

                var notificationParameter = new Dictionary<string, object>
                {
                    { "@userId", donorId },
                    { "@link", encryptedLink },
                    { "@content", message }
                };

                _Qry.ExecuteNonQuery(sqlNtf, notificationParameter);

                string fullLink = "https://localhost:44390/DonationRequest.aspx?donationPublishId=" + donationPublishId;

                // send email to notify organization
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'DONATION REQUEST REJECTED', " +
                                  "@reason = @reason, " +
                                  "@role = 'donor', " +
                                  "@resubmitlink = @link, " +
                                  "@orgId = @orgId, " +
                                  "@donorId = @donorId, " +
                                  "@donationpublishid = @donationPublishId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@reason", rejectionReason },
                    { "@link", fullLink },
                    { "@orgId", orgId },
                    { "@donorId", donorId },
                    { "@donationPublishId", donationPublishId }
                };

                _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Donation request is rejected. Notifications is sent to inform donors.');", true);

                Session.Remove("PickupAddress");
                Session.Remove("Distance");
                Session.Remove("donationId");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error rejecting donation request. Please try again!');", true);
            }
        }

    

    [WebMethod]
        public static string GetOrganizationAddress(string donationPublishId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            string sql = "SELECT address FROM donation_publish WHERE donationPublishId = @donationPublishId";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@donationPublishId", donationPublishId }
            };
            _dt = _Qry.GetData(sql, parameter);

            if (_dt.Rows.Count > 0)
            {
             
                string state = _dt.Rows[0]["address"].ToString();


                return state;

            }

            return null;
        }

        [WebMethod]
        public static double ReceiveDistance(string distance)
        {
       
            double distanceValue = double.Parse(distance);

            HttpContext.Current.Session["Distance"] = distance;

            string donationId = HttpContext.Current.Session["donationId"]?.ToString();


            if (!string.IsNullOrEmpty(donationId))
            {
                // save the distance to the database
                SaveDistanceToDatabase(donationId, distanceValue);
            }

            return distanceValue;
        }

        // save the distance in the database
        private static void SaveDistanceToDatabase(string donationId, double distance)
        {
           
            string sql = "UPDATE donation_item_request SET totalDistance = @totalDistance WHERE donationId = @donationId";
           
            var parameter = new Dictionary<string, object>
            {
                { "@totalDistance", distance },
                { "@donationId", donationId }
            };
          
            QRY qry = new QRY();
            
            qry.ExecuteNonQuery(sql, parameter);
        }


        [WebMethod]
        public static string GetPickupAddress()
        {
            // retrieve the address from the session
            return HttpContext.Current.Session["PickupAddress"]?.ToString();
        }

        static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        private void LoadDonationItems(string donationId)
        {
            // fetch the donation items based on the donationId
            string query = @"
            WITH SplitItems AS (
                SELECT 
                    dir.donationId,
                    dir.itemCategory,
                    TRIM(value) AS item,
                    ROW_NUMBER() OVER (PARTITION BY dir.donationId, dir.itemCategory ORDER BY (SELECT NULL)) AS rowNum
                FROM 
                    donation_item_request dir
                CROSS APPLY 
                    STRING_SPLIT(dir.item, ',')
            ),
            SplitQuantities AS (
                SELECT 
                    dir.donationId,
                    dir.itemCategory,
                    TRIM(REPLACE(value, '(', '')) AS quantityDonated,
                    ROW_NUMBER() OVER (PARTITION BY dir.donationId, dir.itemCategory ORDER BY (SELECT NULL)) AS rowNum
                FROM 
                    donation_item_request dir
                CROSS APPLY 
                    STRING_SPLIT(REPLACE(dir.quantityDonated, ')', ''), ',')
            )
            SELECT 
                si.itemCategory,
                si.item,
                sq.quantityDonated,
                die.expiryDate,
                die.quantityWithSameExpiryDate
            FROM 
                SplitItems si
            JOIN 
                SplitQuantities sq
                ON si.donationId = sq.donationId 
                AND si.itemCategory = sq.itemCategory
                AND si.rowNum = sq.rowNum
            LEFT JOIN 
                donation_item_expiry_date die 
                ON si.donationId = die.donationId 
                AND si.itemCategory = die.itemCategory
                AND si.item = die.item2
            WHERE 
                si.donationId = @donationId
            ORDER BY 
                si.itemCategory, si.item, die.expiryDate";

            QRY qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };
            DataTable dt = qry.GetData(query, parameters);

            if (dt.Rows.Count > 0)
            {
                // create a table
                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='categoryDetailsTable' style='margin: auto;'>");
                sb.Append("<thead><tr><th>Category</th><th>Item</th><th>Quantity</th><th>Expiry Date (Quantity)</th><th>Action</th></tr></thead>");
                sb.Append("<tbody>");

                string currentCategory = "";

                foreach (DataRow row in dt.Rows)
                {
                    string category = row["itemCategory"].ToString();
                    string item = row["item"].ToString();
                    string expiryDate = row["expiryDate"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["expiryDate"]).ToString("yyyy-MM-dd");
                    string expiryQuantity = row["quantityWithSameExpiryDate"] == DBNull.Value ? "N/A" : row["quantityWithSameExpiryDate"].ToString();

                    // if there is no expiry date, show the donated quantity from the original request
                    string quantityToDisplay = expiryDate == "N/A" ? row["quantityDonated"].ToString() : expiryQuantity;

                    // display category header once per category
                    if (currentCategory != category)
                    {
                        sb.AppendFormat("<tr><td colspan='5'><strong>{0}</strong></td></tr>", category);
                        currentCategory = category;
                    }

                    sb.Append("<tr>");
                    sb.AppendFormat("<td></td>"); 
                    sb.AppendFormat("<td>{0}</td>", item);  
                    sb.AppendFormat("<td>{0}</td>", quantityToDisplay);  
                    sb.AppendFormat("<td>{0}</td>", expiryDate); 

                    // add a delete icon that triggers the removal process
                    sb.AppendFormat("<td><a href='#' onclick=\"confirmRemove('{0}', '{1}', '{2}', '{3}', '{4}')\"><i class='fas fa-minus-circle'></i></a></td>", donationId, category, item, expiryDate, expiryQuantity);

                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");
         
                phDonationItems.Controls.Add(new Literal { Text = sb.ToString() });
            }
            else
            {
                // display a message if no items are found
                phDonationItems.Controls.Add(new Literal { Text = "<p>No items found for this donation.</p>" });
            }
        }

        [WebMethod]
        public static bool RemoveItem(string donationId, string category, string item, string expiryDate, string quantityWithSameExpiryDate)
        {
            try
            {
                QRY _Qry = new QRY();

                // Step 1: Get the count of categories (rows) for the same donationId
                string countQuery = "SELECT COUNT(*) FROM donation_item_request WHERE donationId = @donationId";
                var countParams = new Dictionary<string, object>
                {
                    { "@donationId", donationId }
                };

                int categoryCount = (int)_Qry.ExecuteScalar(countQuery, countParams);

                // Step 2: Fetch the current item and quantityDonated from donation_item_request
                string fetchQuery = "SELECT item, quantityDonated FROM donation_item_request WHERE donationId = @donationId AND itemCategory = @category";

                var fetchParams = new Dictionary<string, object>
                {
                    { "@donationId", donationId },
                    { "@category", category }
                };

                DataTable dt = _Qry.GetData(fetchQuery, fetchParams);
                if (dt.Rows.Count == 0) return false; 

                string items = dt.Rows[0]["item"].ToString();
                string quantities = dt.Rows[0]["quantityDonated"].ToString();

                // Step 3: Split the items and quantities to process them
                var itemList = items.Split(',').Select(i => i.Trim()).ToList();
                var quantityList = quantities.Replace("(", "").Replace(")", "").Split(',').Select(q => int.Parse(q.Trim())).ToList();

                // get the index of the item to be removed
                int index = itemList.IndexOf(item);
                if (index == -1) return false; // if the item is not found, return false

                string deletedContent;
                if (!string.IsNullOrEmpty(expiryDate) && expiryDate != "N/A")
                {
                    // if expiry date is available, include it in the deleted content
                    deletedContent = $"{item} ({quantityWithSameExpiryDate}) ({expiryDate})";
                }
                else
                {
                    // if no expiry date, retrieve the correct quantity from quantityList
                    deletedContent = $"{item} ({quantityList[index]})";
                }


                // insert or update the deleted_item_donations table
                string checkDeletedQuery = "SELECT content FROM deleted_item_donations WHERE donationId = @donationId";
                var checkDeletedParams = new Dictionary<string, object>
                {
                    { "@donationId", donationId }
                };

                DataTable deletedItemDt = _Qry.GetData(checkDeletedQuery, checkDeletedParams);
                if (deletedItemDt.Rows.Count > 0)
                {
                    // if an entry exists for this donationId, update the deleted content
                    string existingContent = deletedItemDt.Rows[0]["content"].ToString();
                    string updatedContent = existingContent + ", " + deletedContent;

                    string updateDeletedQuery = "UPDATE deleted_item_donations SET content = @updatedContent WHERE donationId = @donationId";
                    var updateDeletedParams = new Dictionary<string, object>
                    {
                        { "@updatedContent", updatedContent },
                        { "@donationId", donationId }
                    };

                    _Qry.ExecuteNonQuery(updateDeletedQuery, updateDeletedParams);
                }
                else
                {
                    // insert new row for deleted item
                    string insertDeletedQuery = "INSERT INTO deleted_item_donations (donationId, content) VALUES (@donationId, @content)";
                    var insertDeletedParams = new Dictionary<string, object>
                    {
                        { "@donationId", donationId },
                        { "@content", deletedContent }
                    };

                    _Qry.ExecuteNonQuery(insertDeletedQuery, insertDeletedParams);
                }

                // if there is more than one category, allow deletion when fetchQuery returns 1 item
                if (categoryCount > 1)
                {
                    // allow deletion when there are multiple categories
                    if (!string.IsNullOrEmpty(expiryDate) && expiryDate != "N/A")
                    {
                        // fetch the quantity for the specified item and expiry date
                        string fetchExpiryQuery = @"
                        SELECT quantityWithSameExpiryDate 
                        FROM donation_item_expiry_date 
                        WHERE donationId = @donationId 
                        AND itemCategory = @category 
                        AND item2 = @item 
                        AND expiryDate = @expiryDate";

                        var fetchExpiryParams = new Dictionary<string, object>
                        {
                            { "@donationId", donationId },
                            { "@category", category },
                            { "@item", item },
                            { "@expiryDate", expiryDate }
                        };

                        DataTable expiryDt = _Qry.GetData(fetchExpiryQuery, fetchExpiryParams);
                        if (expiryDt.Rows.Count == 0) return false; // no matching expiry found

                        int expiryQuantity = int.Parse(expiryDt.Rows[0]["quantityWithSameExpiryDate"].ToString());

                        // if expiryQuantity matches the current quantity in quantityList, prevent deletion
                        if (expiryQuantity == quantityList[index])
                        {
                            // check if the original item column contains a comma (multiple items)
                            string originalItemQuery = "SELECT item FROM donation_item_request WHERE donationId = @donationId AND itemCategory = @category";
                            var originalItemParams = new Dictionary<string, object>
                            {
                                { "@donationId", donationId },
                                { "@category", category }
                            };

                            DataTable originalItemDt = _Qry.GetData(originalItemQuery, originalItemParams);
                            if (originalItemDt.Rows.Count == 0) return false; // No matching item found

                            string originalItems = originalItemDt.Rows[0]["item"].ToString();

                            // if there is no comma, it means it's the only item in the category, so delete the entire row
                            if (!originalItems.Contains(","))
                            {
                                // delete the entire row from donation_item_request
                                string deleteRowQuery = "DELETE FROM donation_item_request WHERE donationId = @donationId AND itemCategory = @category";
                                var deleteRowParams = new Dictionary<string, object>
                                {
                                    { "@donationId", donationId },
                                    { "@category", category }
                                };

                                bool deleteRowSuccess = _Qry.ExecuteNonQuery(deleteRowQuery, deleteRowParams);
                                if (!deleteRowSuccess) return false;

                                // delete the corresponding row from donation_item_expiry_date
                                string deleteExpiryQuery = @"
                                DELETE FROM donation_item_expiry_date 
                                WHERE donationId = @donationId 
                                AND itemCategory = @category 
                                AND item2 = @item 
                                AND expiryDate = @expiryDate";

                                var deleteExpiryParams = new Dictionary<string, object>
                                {
                                    { "@donationId", donationId },
                                    { "@category", category },
                                    { "@item", item },
                                    { "@expiryDate", expiryDate }
                                };

                                return _Qry.ExecuteNonQuery(deleteExpiryQuery, deleteExpiryParams);
                            }
                            else
                            {
                                // if there are multiple items, proceed with updating the item and quantity lists
                                quantityList.RemoveAt(index);  // remove the corresponding quantity
                                itemList.RemoveAt(index);      // remove the corresponding item

                                // join the remaining items and quantities into comma-separated strings
                                string newItems = string.Join(", ", itemList);
                                string newQuantities = "(" + string.Join(", ", quantityList) + ")";

                                // update the donation_item_request table with the new items and quantities
                                string updateQuery = "UPDATE donation_item_request SET item = @newItems, quantityDonated = @newQuantities WHERE donationId = @donationId AND itemCategory = @category";
                                var updateParams = new Dictionary<string, object>
                                {
                                    { "@newItems", newItems },
                                    { "@newQuantities", newQuantities },
                                    { "@donationId", donationId },
                                    { "@category", category }
                                };

                                bool updateSuccess = _Qry.ExecuteNonQuery(updateQuery, updateParams);
                                if (!updateSuccess) return false;

                                // remove the specific entry from donation_item_expiry_date for this expiry date
                                string deleteExpiryQuery2 = @"
                                DELETE FROM donation_item_expiry_date 
                                WHERE donationId = @donationId 
                                AND itemCategory = @category 
                                AND item2 = @item 
                                AND expiryDate = @expiryDate";

                                var deleteExpiryParams2 = new Dictionary<string, object>
                                {
                                    { "@donationId", donationId },
                                    { "@category", category },
                                    { "@item", item },
                                    { "@expiryDate", expiryDate }
                                };

                                return _Qry.ExecuteNonQuery(deleteExpiryQuery2, deleteExpiryParams2);
                            }
                        }
                     
                        // if expiryQuantity is less, update quantity and remove the specific expiry entry
                        if (expiryQuantity < quantityList[index])
                        {
                            // subtract the expiryQuantity from the current quantity in the list
                            quantityList[index] -= expiryQuantity; // update the quantity for item2 in expiry date table

                            // join the remaining quantities into a comma-separated string, keeping the item string the same
                            string newQuantities = "(" + string.Join(", ", quantityList) + ")"; 

                            // update the donation_item_request with the new quantities but keep the items the same
                            string updateQuery = "UPDATE donation_item_request SET quantityDonated = @newQuantities WHERE donationId = @donationId AND itemCategory = @category AND item = @item";
                            var updateParams = new Dictionary<string, object>
                            {
                                { "@newQuantities", newQuantities }, 
                                { "@donationId", donationId },
                                { "@category", category },
                                { "@item", item }
                            };

                       
                            bool updateSuccess = _Qry.ExecuteNonQuery(updateQuery, updateParams);
                            if (!updateSuccess) return false;

                            // now remove the entry from the expiry date table
                            string deleteExpiryQuery = @"
                            DELETE FROM donation_item_expiry_date 
                            WHERE donationId = @donationId 
                            AND itemCategory = @category 
                            AND item2 = @item 
                            AND expiryDate = @expiryDate";

                            var deleteExpiryParams = new Dictionary<string, object>
                            {
                                { "@donationId", donationId },
                                { "@category", category },
                                { "@item", item },
                                { "@expiryDate", expiryDate }
                            };

                            // execute the delete query from the expiry date table
                            return _Qry.ExecuteNonQuery(deleteExpiryQuery, deleteExpiryParams);
                        }
                    }

                    else
                    {
                        if (itemList.Count == 1)
                        {
                            // delete the entire row from donation_item_request
                            string deleteRowQuery = "DELETE FROM donation_item_request WHERE donationId = @donationId AND itemCategory = @category";
                            var deleteRowParams = new Dictionary<string, object>
                            {
                                { "@donationId", donationId },
                                { "@category", category }
                            };

                            bool deleteRowSuccess = _Qry.ExecuteNonQuery(deleteRowQuery, deleteRowParams);
                            if (!deleteRowSuccess) return false;

                            return true; 
                        }
                        else
                        {
                            quantityList.RemoveAt(index);  // remove the corresponding quantity
                            itemList.RemoveAt(index);      // remove the corresponding item

                            // join the remaining items and quantities into comma-separated strings
                            string newItems = string.Join(", ", itemList);
                            string newQuantities = "(" + string.Join(", ", quantityList) + ")";

                            // update the donation_item_request table with the new items and quantities
                            string updateQuery = "UPDATE donation_item_request SET item = @newItems, quantityDonated = @newQuantities WHERE donationId = @donationId AND itemCategory = @category";
                            var updateParams = new Dictionary<string, object>
                            {
                                { "@newItems", newItems },
                                { "@newQuantities", newQuantities },
                                { "@donationId", donationId },
                                { "@category", category }
                            };

                            bool updateSuccess = _Qry.ExecuteNonQuery(updateQuery, updateParams);
                            if (!updateSuccess) return false;

                            return true; 
                        }
                    }
                }

                // if there is only one category
                else if (categoryCount == 1)
                {
                    // if it only have one item
                    if (itemList.Count == 1)
                    {
                        // if it has expiry date
                        if (!string.IsNullOrEmpty(expiryDate) && expiryDate != "N/A")
                        {
                            // fetch the quantity for the specified item and expiry date
                            string fetchExpiryQuery = @"
                            SELECT quantityWithSameExpiryDate 
                            FROM donation_item_expiry_date 
                            WHERE donationId = @donationId 
                            AND itemCategory = @category 
                            AND item2 = @item 
                            AND expiryDate = @expiryDate";

                            var fetchExpiryParams = new Dictionary<string, object>
                            {
                                { "@donationId", donationId },
                                { "@category", category },
                                { "@item", item },
                                { "@expiryDate", expiryDate }
                            };

                            DataTable expiryDt = _Qry.GetData(fetchExpiryQuery, fetchExpiryParams);
                            if (expiryDt.Rows.Count == 0) return false; 

                            int expiryQuantity = int.Parse(expiryDt.Rows[0]["quantityWithSameExpiryDate"].ToString());

                            // if expiryQuantity is the same as the current quantity in quantityList, prevent deletion or updates
                            if (expiryQuantity == quantityList[index])
                            {
                                return false; 
                            }

                            // if expiryQuantity less than current quantity
                            if (expiryQuantity < quantityList[index])
                            {
                                // remove the specific entry from donation_item_expiry_date (but keep the item in the itemList)
                                string deleteExpiryQuery = @"
                                DELETE FROM donation_item_expiry_date 
                                WHERE donationId = @donationId 
                                AND itemCategory = @category 
                                AND item2 = @item 
                                AND expiryDate = @expiryDate";

                                var deleteExpiryParams = new Dictionary<string, object>
                                {
                                    { "@donationId", donationId },
                                    { "@category", category },
                                    { "@item", item },
                                    { "@expiryDate", expiryDate }
                                };

                                // deletion query for the expiry date table
                                bool deleteExpirySuccess = _Qry.ExecuteNonQuery(deleteExpiryQuery, deleteExpiryParams);
                                if (!deleteExpirySuccess) return false;

                                // reduce the quantity but don't remove the item
                                quantityList[index] -= expiryQuantity;

                                // join the remaining quantities into a comma-separated string (keeping the items)
                                string newQuantities = "(" + string.Join(", ", quantityList) + ")";
                                string newItems = string.Join(", ", itemList); // keep the items the same

                                // update the donation_item_request with the new quantities and keep the items the same
                                string updateQuery = "UPDATE donation_item_request SET quantityDonated = @newQuantities, item = @newItems WHERE donationId = @donationId AND itemCategory = @category";
                                var updateParams = new Dictionary<string, object>
                                {
                                    { "@newQuantities", newQuantities }, // updated quantity
                                    { "@newItems", newItems }, // keep the items the same
                                    { "@donationId", donationId },
                                    { "@category", category }
                                };

                                // update query for donation_item_request
                                return _Qry.ExecuteNonQuery(updateQuery, updateParams);
                            }
                        }
                        else
                        {
                            // no expiry date but prevent deletion if quantity matches
                            if (quantityList[index] == int.Parse(quantityWithSameExpiryDate))
                            {
                                return false; 
                            }


                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(expiryDate) && expiryDate != "N/A")
                        {
                            // fetch the quantity for the specified item and expiry date
                            string fetchExpiryQuery = @"
                            SELECT quantityWithSameExpiryDate 
                            FROM donation_item_expiry_date 
                            WHERE donationId = @donationId 
                            AND itemCategory = @category 
                            AND item2 = @item 
                            AND expiryDate = @expiryDate";

                            var fetchExpiryParams = new Dictionary<string, object>
                            {
                                { "@donationId", donationId },
                                { "@category", category },
                                { "@item", item },
                                { "@expiryDate", expiryDate }
                            };

                            DataTable expiryDt = _Qry.GetData(fetchExpiryQuery, fetchExpiryParams);
                            if (expiryDt.Rows.Count == 0) return false; 

                            int expiryQuantity = int.Parse(expiryDt.Rows[0]["quantityWithSameExpiryDate"].ToString());

                            if (expiryQuantity == quantityList[index])
                            {
                                string deleteExpiryQuery = @"
                                DELETE FROM donation_item_expiry_date 
                                WHERE donationId = @donationId 
                                AND itemCategory = @category 
                                AND item2 = @item 
                                AND expiryDate = @expiryDate";

                                var deleteExpiryParams = new Dictionary<string, object>
                                {
                                    { "@donationId", donationId },
                                    { "@category", category },
                                    { "@item", item },
                                    { "@expiryDate", expiryDate }
                                };

                                // deletion query for the expiry date table
                                bool deleteExpirySuccess = _Qry.ExecuteNonQuery(deleteExpiryQuery, deleteExpiryParams);
                                if (!deleteExpirySuccess) return false;

                                quantityList.RemoveAt(index);  // remove the corresponding quantity
                                itemList.RemoveAt(index);      // remove the corresponding item

                                // join the remaining items and quantities into comma-separated strings
                                string newItems = string.Join(", ", itemList);
                                string newQuantities = "(" + string.Join(", ", quantityList) + ")";

                                // update the donation_item_request table with the new items and quantities
                                string updateQuery = "UPDATE donation_item_request SET item = @newItems, quantityDonated = @newQuantities WHERE donationId = @donationId AND itemCategory = @category";
                                var updateParams = new Dictionary<string, object>
                                {
                                    { "@newItems", newItems },
                                    { "@newQuantities", newQuantities },
                                    { "@donationId", donationId },
                                    { "@category", category }
                                };

                                bool updateSuccess = _Qry.ExecuteNonQuery(updateQuery, updateParams);
                                if (!updateSuccess) return false;

                                return true; 
                            
                            }
                            
                            if (expiryQuantity < quantityList[index])
                            {
                                // remove the specific entry from donation_item_expiry_date (but keep the item in the itemList)
                                string deleteExpiryQuery = @"
                                DELETE FROM donation_item_expiry_date 
                                WHERE donationId = @donationId 
                                AND itemCategory = @category 
                                AND item2 = @item 
                                AND expiryDate = @expiryDate";

                                var deleteExpiryParams = new Dictionary<string, object>
                                {
                                    { "@donationId", donationId },
                                    { "@category", category },
                                    { "@item", item },
                                    { "@expiryDate", expiryDate }
                                };

                                // deletion query for the expiry date table
                                bool deleteExpirySuccess = _Qry.ExecuteNonQuery(deleteExpiryQuery, deleteExpiryParams);
                                if (!deleteExpirySuccess) return false;

                                // reduce the quantity but don't remove the item
                                quantityList[index] -= expiryQuantity;

                                // join the remaining quantities into a comma-separated string (keeping the items)
                                string newQuantities = "(" + string.Join(", ", quantityList) + ")";
                                string newItems = string.Join(", ", itemList); // Keep the items the same

                                // update the donation_item_request with the new quantities and keep the items the same
                                string updateQuery = "UPDATE donation_item_request SET quantityDonated = @newQuantities, item = @newItems WHERE donationId = @donationId AND itemCategory = @category";
                                var updateParams = new Dictionary<string, object>
                                {
                                    { "@newQuantities", newQuantities }, // updated quantity
                                    { "@newItems", newItems }, // keep the items the same
                                    { "@donationId", donationId },
                                    { "@category", category }
                                };

                                // update query for donation_item_request
                                return _Qry.ExecuteNonQuery(updateQuery, updateParams);
                            }

                        }
                    }
                }
               
                // no condition is met
                return false;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }

        



    }
}