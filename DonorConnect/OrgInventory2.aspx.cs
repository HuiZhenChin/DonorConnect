using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static iTextSharp.text.pdf.AcroFields;
using System.Windows.Controls.Primitives;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Net.PeerToPeer;
using static DonorConnect.AllDonations;

namespace DonorConnect
{
    public partial class OrgInventory2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDonationItem();

                PopulateItemCategory();

                PopulateOtherItemCategory();

                sideViewContainer.Visible = false;
            }
        }

        public class InventoryHistory
        {
            public string content { get; set; }
            public string createdOn { get; set; }
        }

        public class ChartData
        {
            public string label { get; set; }
            public string value { get; set; }
        }


        private void LoadDonationItem()
        {

            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
            SELECT itemCategory, item, quantity, expiryDate, inventoryId, imageItem, threshold
            FROM inventory
            WHERE orgId = @orgId
            AND expiryDate IS NOT NULL
            ORDER BY expiryDate ASC ";


            QRY _Qry = new QRY();


            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
           
                gvDonationItems.DataSource = dt;
                gvDonationItems.DataBind();
                btnAddItem.Style["display"] = "masonry";
                btnAddOtherItem.Style["display"] = "none";
            }
            else
            {

                gvDonationItems.DataSource = null;
                gvDonationItems.DataBind();
                message.Text = "No record found.";
                btnAddItem.Style["display"] = "masonry";
                btnAddOtherItem.Style["display"] = "none";
            }

        }

        protected void gvDonationItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // get the expiry date for the current row
                DateTime expiryDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "expiryDate"));
                DateTime currentDate = DateTime.Now;
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");

                // check if the item is expired
                if (expiryDate < currentDate)
                {
                    // item is expired already
                    lblStatus.Text = "Expired";
                    lblStatus.CssClass = "expire expire-danger";
                    e.Row.BackColor = System.Drawing.Color.LightCoral; 
                }
                // check if the item will expire within the next 7 days
                else if ((expiryDate - currentDate).TotalDays <= 7)
                {
                    // item will expire soon
                    lblStatus.Text = "Expiring Soon";
                    lblStatus.CssClass = "expire expire-warning"; 
                    e.Row.BackColor = System.Drawing.Color.MistyRose; 
                }
                else
                {
                    // item is not expired yet
                    lblStatus.Text = "Not Expired";
                    lblStatus.CssClass = "expire expire-success"; 
                }

                string encryptedBase64Images = DataBinder.Eval(e.Row.DataItem, "imageItem").ToString();

                string imagesHtml = ImageFileProcessing.ProcessImagesForInventory(encryptedBase64Images);

                Literal ltImage = (Literal)e.Row.FindControl("ltImage");

                ltImage.Text = imagesHtml;

                int quantity = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "quantity"));
                object thresholdValue = DataBinder.Eval(e.Row.DataItem, "threshold");
                if (thresholdValue != DBNull.Value && int.TryParse(thresholdValue.ToString(), out int threshold))
                {

                    Label lblQuantity = (Label)e.Row.FindControl("lblQuantity");

                    if (quantity < threshold)
                    {
         
                        TableCell cell = lblQuantity.Parent as TableCell;
                        if (cell != null)
                        {
                           
                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8463D");

                            cell.ToolTip = $"Warning: Current quantity ({quantity}) is below the threshold ({threshold})!";
                        }
                    }
                }
            }
        }

        protected void toggleGridViewChange(object sender, EventArgs e)
        {
            if (gvDonationItems.Visible)
            {
               
                gvDonationItems.Style["display"] = "none";
                gvNoExpiryItem.Style["display"] = "table";
                gvDonationItems.Visible = false;
                gvNoExpiryItem.Visible = true;

                btnViewOther.Text = "View Expiry Dates";
                
                // load the items without expiry dates
                LoadOtherItem();
            }
            else
            {
               
                gvDonationItems.Style["display"] = "table";
                gvNoExpiryItem.Style["display"] = "none";
                gvDonationItems.Visible = true;
                gvNoExpiryItem.Visible = false;
               
                btnViewOther.Text = "View Other Items";

              
                // load the items with expiry dates
                LoadDonationItem();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "hideLoading", "hideLoadingBar();", true);
        }


        private void LoadOtherItem()
        {

            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
            SELECT itemCategory, item, quantity, inventoryId, imageItem, threshold
            FROM inventory
            WHERE orgId = @orgId
            AND expiryDate IS NULL
            ";


            QRY _Qry = new QRY();


            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {

                gvNoExpiryItem.DataSource = dt;
                gvNoExpiryItem.DataBind();
                btnAddOtherItem.Style["display"] = "masonry";
                btnAddItem.Style["display"] = "none";
                message.Style["display"] = "none";
            }
            else
            {
           
                gvNoExpiryItem.DataSource = null;
                gvNoExpiryItem.DataBind();
                message.Text = "No record found.";
                btnAddOtherItem.Style["display"] = "masonry";
                btnAddItem.Style["display"] = "none";

            }

        }

        protected void gvNoExpiryItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string encryptedBase64Images = DataBinder.Eval(e.Row.DataItem, "imageItem").ToString();

                string imagesHtml = ImageFileProcessing.ProcessImagesForInventory(encryptedBase64Images);

                Literal ltImage = (Literal)e.Row.FindControl("ltImage2");

                ltImage.Text = imagesHtml;

                int quantity = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "quantity"));
                object thresholdValue = DataBinder.Eval(e.Row.DataItem, "threshold");
                if (thresholdValue != DBNull.Value && int.TryParse(thresholdValue.ToString(), out int threshold))
                {

                    Label lblQuantity = (Label)e.Row.FindControl("lblQuantity2");

                    if (quantity < threshold)
                    {

                        TableCell cell = lblQuantity.Parent as TableCell;
                        if (cell != null)
                        {

                            cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#C8463D");

                            cell.ToolTip = $"Warning: Current quantity ({quantity}) is below the threshold ({threshold})!";
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            LinkButton btnSave = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btnSave.NamingContainer;

            TextBox txtItem = (TextBox)row.FindControl("txtItem");
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity");
            TextBox txtExpiryDate = (TextBox)row.FindControl("txtExpiryDate");
            FileUpload fuItemImage = (FileUpload)row.FindControl("fuItemImage");

            string newItem = txtItem.Text;
            int newQuantity = int.Parse(txtQuantity.Text); 
            string newExpiryDate = txtExpiryDate.Text;

            string inventoryId = btnSave.CommandArgument;

            string imageItem = "";

            // retrieve existing image 
            if (fuItemImage.HasFiles)
            {
                imageItem = ImageFileProcessing.ConvertToBase64(fuItemImage.PostedFiles);
            }
            else
            {
                string sql2 = "SELECT imageItem FROM inventory WHERE inventoryId = @inventoryId";
                Dictionary<string, object> selectParam = new Dictionary<string, object>
                {
                    { "@inventoryId", inventoryId }
                };

                QRY _Qry2 = new QRY();
                DataTable dt = _Qry2.GetData(sql2, selectParam);
                if (dt.Rows.Count > 0)
                {
                    imageItem = dt.Rows[0]["imageItem"].ToString();
                }
                else
                {
                    imageItem = "";
                }
            }

            // retrieve the original quantity 
            string selectOriginalQuantity = "SELECT quantity FROM inventory WHERE inventoryId = @inventoryId";
            Dictionary<string, object> selectParams = new Dictionary<string, object>
            {
                { "@inventoryId", inventoryId }
            };

            QRY _QryOriginal = new QRY();
            DataTable dtOriginal = _QryOriginal.GetData(selectOriginalQuantity, selectParams);
            int originalQuantity = 0;
            if (dtOriginal.Rows.Count > 0)
            {
                originalQuantity = Convert.ToInt32(dtOriginal.Rows[0]["quantity"]);
            }

            string update = "UPDATE inventory SET item = @item, quantity = @quantity, expiryDate = @expiryDate, imageItem = @imageItem WHERE inventoryId = @inventoryId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@item", newItem },
                { "@quantity", newQuantity },
                { "@expiryDate", newExpiryDate },
                { "@inventoryId", inventoryId },
                { "@imageItem", imageItem }
            };

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(update, parameters);

            if (success)
            {
                // compare original and new quantities to log item in/out
                int quantityDifference = newQuantity - originalQuantity;

                if (quantityDifference > 0)
                {
                    // Log items added (quantityIn)
                    string itemInSql = "INSERT INTO inventory_item_usage (inventoryId, itemCategory, item, quantityIn, created_on, orgId) " +
                                            "SELECT inventoryId, itemCategory, item, @quantityIn, GETDATE(), orgId FROM inventory WHERE inventoryId = @inventoryId";

                    Dictionary<string, object> itemInParam = new Dictionary<string, object>
                    {
                        { "@quantityIn", quantityDifference },
                        { "@inventoryId", inventoryId }
                    };

                    _Qry.ExecuteNonQuery(itemInSql, itemInParam);
                }
                else if (quantityDifference < 0)
                {
                    // Log items used (quantityOut)
                    string itemOutSql = "INSERT INTO inventory_item_usage (inventoryId, itemCategory, item, quantityOut, created_on, orgId) " +
                                             "SELECT inventoryId, itemCategory, item, @quantityOut, GETDATE(), orgId FROM inventory WHERE inventoryId = @inventoryId";

                    Dictionary<string, object> itemOutParam = new Dictionary<string, object>
                    {
                        { "@quantityOut", Math.Abs(quantityDifference) },
                        { "@inventoryId", inventoryId }
                    };

                    _Qry.ExecuteNonQuery(itemOutSql, itemOutParam);
                }

                // insert history log
                string insertHistory = @"
                INSERT INTO inventory_history (content, orgId, created_on) 
                               SELECT CONCAT(item, ' from ', itemCategory, 
                              ' was updated with item name ', @item, 
                              ' and quantity of ', @quantity, 
                              ' on ', CONVERT(VARCHAR, GETDATE(), 23),
                              ' with new expiry date ', 
                              @expiryDate
                             ),
                       orgId, 
                       GETDATE()
                FROM inventory 
                WHERE inventoryId = @inventoryId";

                Dictionary<string, object> historyParam = new Dictionary<string, object>
                {
                    { "@quantity", newQuantity },
                    { "@inventoryId", inventoryId },
                    { "@item", newItem },
                    { "@expiryDate", newExpiryDate },
                };

                bool success2 = _Qry.ExecuteNonQuery(insertHistory, historyParam);

                if (success2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showSuccess('Item updated successfully');", true);
                    LoadDonationItem();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in updating data. Please try again.');", true);
            }
        }


        protected void btnSaveOther_Click(object sender, EventArgs e)
        {
            LinkButton btnSave = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btnSave.NamingContainer;

            TextBox txtItem = (TextBox)row.FindControl("txtItem2");
            TextBox txtQuantity = (TextBox)row.FindControl("txtQuantity2");
            FileUpload fuItemImage = (FileUpload)row.FindControl("fuItemImage2");

            string newItem = txtItem.Text;
            int newQuantity = int.Parse(txtQuantity.Text); // Convert to integer
            string inventoryId = btnSave.CommandArgument;

            string imageItem = "";

            // retrieve existing image 
            if (fuItemImage.HasFiles)
            {
                imageItem = ImageFileProcessing.ConvertToBase64(fuItemImage.PostedFiles);
            }
            else
            {
                string sql2 = "SELECT imageItem FROM inventory WHERE inventoryId = @inventoryId";
                Dictionary<string, object> selectParam = new Dictionary<string, object>
                {
                    { "@inventoryId", inventoryId }
                };

                QRY _Qry2 = new QRY();
                DataTable dt = _Qry2.GetData(sql2, selectParam);
                if (dt.Rows.Count > 0)
                {
                    imageItem = dt.Rows[0]["imageItem"].ToString();
                }
                else
                {
                    imageItem = "";
                }
            }

            string selectOriginalQuantity = "SELECT quantity FROM inventory WHERE inventoryId = @inventoryId";
            Dictionary<string, object> selectParams = new Dictionary<string, object>
            {
                { "@inventoryId", inventoryId }
            };

            QRY _QryOriginal = new QRY();
            DataTable dtOriginal = _QryOriginal.GetData(selectOriginalQuantity, selectParams);
            int originalQuantity = 0;
            if (dtOriginal.Rows.Count > 0)
            {
                originalQuantity = Convert.ToInt32(dtOriginal.Rows[0]["quantity"]);
            }

            string updateOther = "UPDATE inventory SET item = @item, quantity = @quantity, imageItem = @imageItem WHERE inventoryId = @inventoryId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@item", newItem },
                { "@quantity", newQuantity },
                { "@inventoryId", inventoryId },
                { "@imageItem", imageItem }
            };

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(updateOther, parameters);

            if (success)
            {
                // compare original and new quantities to log item in/out
                int quantityDifference = newQuantity - originalQuantity;

                if (quantityDifference > 0)
                {
                    // Log items added (quantityIn)
                    string itemInSql = "INSERT INTO inventory_item_usage (inventoryId, itemCategory, item, quantityIn, created_on, orgId) " +
                                            "SELECT inventoryId, itemCategory, item, @quantityIn, GETDATE(), orgId FROM inventory WHERE inventoryId = @inventoryId";

                    Dictionary<string, object> itemInParam = new Dictionary<string, object>
                    {
                        { "@quantityIn", quantityDifference },
                        { "@inventoryId", inventoryId }
                    };

                    _Qry.ExecuteNonQuery(itemInSql, itemInParam);
                }
                else if (quantityDifference < 0)
                {
                    // Log items used (quantityOut)
                    string itemOutSql = "INSERT INTO inventory_item_usage (inventoryId, itemCategory, item, quantityOut, created_on, orgId) " +
                                             "SELECT inventoryId, itemCategory, item, @quantityOut, GETDATE(), orgId FROM inventory WHERE inventoryId = @inventoryId";

                    Dictionary<string, object> itemOutParam = new Dictionary<string, object>
                    {
                        { "@quantityOut", Math.Abs(quantityDifference) },
                        { "@inventoryId", inventoryId }
                    };

                    _Qry.ExecuteNonQuery(itemOutSql, itemOutParam);
                }

                // insert history log
                string insertHistory = @"
                INSERT INTO inventory_history (content, orgId, created_on) 
                SELECT CONCAT(item, ' from ', itemCategory, ' was updated with item name ', @item, 
                ' and quantity of ', @quantity, ' on ', CONVERT(VARCHAR, GETDATE(), 23)), orgId, GETDATE() 
                FROM inventory 
                WHERE inventoryId = @inventoryId";

                Dictionary<string, object> historyParam = new Dictionary<string, object>
                {
                    { "@quantity", newQuantity },
                    { "@inventoryId", inventoryId },
                    { "@item", newItem }
                };

                bool success2 = _Qry.ExecuteNonQuery(insertHistory, historyParam);

                if (success2)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showSuccess('Item updated successfully');", true);
                    LoadOtherItem();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in updating data. Please try again.');", true);
            }
        }


        [WebMethod]
        public static string DeleteItem(string inventoryId)
        {
            string selectInventory = @"
            SELECT item, itemCategory, orgId 
            FROM inventory 
            WHERE inventoryId = @inventoryId";

            Dictionary<string, object> selectParams = new Dictionary<string, object>
            {
                { "@inventoryId", inventoryId }
            };

            QRY _Qry = new QRY();
            DataTable inventoryData = _Qry.GetData(selectInventory, selectParams); 

            if (inventoryData.Rows.Count > 0)
            {
                string item = inventoryData.Rows[0]["item"].ToString();
                string itemCategory = inventoryData.Rows[0]["itemCategory"].ToString();
                string orgId = inventoryData.Rows[0]["orgId"].ToString();

                // delete the inventory item
                string delete = "DELETE FROM inventory WHERE inventoryId = @inventoryId";
                bool success = _Qry.ExecuteNonQuery(delete, selectParams);

                if (success)
                {

                    string insertHistory = @"
                    INSERT INTO inventory_history (content, orgId, created_on) 
                    VALUES (@content, @orgId, GETDATE())";

                    string content = $"{item} from {itemCategory} is deleted on {DateTime.Now:yyyy-MM-dd}";

                    Dictionary<string, object> historyParams = new Dictionary<string, object>
                    {
                        { "@content", content },
                        { "@orgId", orgId }
                    };

                    bool success2 = _Qry.ExecuteNonQuery(insertHistory, historyParams);

                    if (success2)
                    {
                        return new JavaScriptSerializer().Serialize(new { status = "success", message = "Item deleted successfully" });
                    }
                    else
                    {
                        return new JavaScriptSerializer().Serialize(new { status = "error", message = "Failed to insert into history" });
                    }
                }
                else
                {
                    return new JavaScriptSerializer().Serialize(new { status = "error", message = "Error in deleting data. Please try again." });
                }
            }
            else
            {
                return new JavaScriptSerializer().Serialize(new { status = "error", message = "Inventory item not found." });
            }
        }

        [WebMethod]
        public static string itemOut(string inventoryId)
        {
            string sql = "SELECT quantity, item, itemCategory, orgId FROM inventory WHERE inventoryId = @inventoryId";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@inventoryId", inventoryId }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                int currentQuantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
                string item = dt.Rows[0]["item"].ToString();
                string itemCategory = dt.Rows[0]["itemCategory"].ToString();
                string orgId = dt.Rows[0]["orgId"].ToString();

                string itemOutSql = "INSERT INTO inventory_item_usage (inventoryId, itemCategory, item, quantityOut, created_on, orgId) " +
                                    "SELECT inventoryId, itemCategory, item, @quantityOut, GETDATE(), orgId FROM inventory WHERE inventoryId = @inventoryId";

                Dictionary<string, object> itemOutParam = new Dictionary<string, object>
                {
                    { "@quantityOut", currentQuantity },
                    { "@inventoryId", inventoryId }
                };

                bool success1 = _Qry.ExecuteNonQuery(itemOutSql, itemOutParam);
                if (!success1)
                {
                    return new JavaScriptSerializer().Serialize(new { status = "error", message = "Failed to log item out." });
                }

                string insertHistory = @"
                INSERT INTO inventory_history (content, orgId, created_on) 
                VALUES (@content, @orgId, GETDATE())";

                string content = $"{item} from {itemCategory} was deleted on {DateTime.Now:yyyy-MM-dd}";

                Dictionary<string, object> historyParams = new Dictionary<string, object>
                {
                    { "@content", content },
                    { "@orgId", orgId }
                };

                bool success2 = _Qry.ExecuteNonQuery(insertHistory, historyParams);
                if (!success2)
                {
                    return new JavaScriptSerializer().Serialize(new { status = "error", message = "Failed to log history." });
                }

                string deleteSql = "DELETE FROM inventory WHERE inventoryId = @inventoryId";
                bool success3 = _Qry.ExecuteNonQuery(deleteSql, itemOutParam);

                if (success3)
                {
                    return new JavaScriptSerializer().Serialize(new { status = "success", message = "Item removed successfully" });
                }
            }

            return new JavaScriptSerializer().Serialize(new { status = "error", message = "Item not found or failed to delete." });
        }


        protected void btnAddItem_Click(object sender, EventArgs e)
        {
         
            fetchExistingItem();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#addItemModal').modal('show');", true);
        }

        protected void fetchExistingItem()
        {
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = "SELECT inventoryId, item, quantity FROM inventory WHERE expiryDate IS NULL AND orgId= @orgId";
            DataTable dt = new DataTable();

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            QRY _Qry = new QRY();

            dt = _Qry.GetData(sql, parameters); 

            gvExistingItems.DataSource = dt;
            gvExistingItems.DataBind();
        }

        protected void gvExistingItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string inventoryId = gvExistingItems.DataKeys[index].Value.ToString();  
   
                hfInventoryId.Value = inventoryId;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDatePickerModal", "$('#expiryDateModal').modal('show');", true);
            }
        }

        protected void PopulateItemCategory()
        {
            string sql = "SELECT categoryName FROM itemCategory WHERE hasExpiryDate = @expiryDate ";
            DataTable dt = new DataTable();

            QRY _Qry = new QRY();

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@expiryDate", "Yes" }
            };

            dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "categoryName";
                ddlCategory.DataValueField = "categoryName"; 
                ddlCategory.DataBind();
            }

            
        }

        protected void btnSaveNewItem_Click(object sender, EventArgs e)
        {            
            string category = ddlCategory.SelectedValue;
            string item = txtNewItemName.Text;
            string quantity = txtNewItemQuantity.Text;
            string expiryDate = txtExpiryDate.Text;

            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();


            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(item) || string.IsNullOrEmpty(quantity) || string.IsNullOrEmpty(expiryDate))
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('All fields are required.');", true);
                return;
            }

             string sql = @"
                        EXEC create_inventory_item 
                            @id = @id,
                            @method= @method,
                            @donationId = @donationId, 
                            @itemCategory = @itemCategory, 
                            @item = @item, 
                            @quantity = @quantityWithSameExpiryDate, 
                            @expiryDate = @expiryDate, 
                            @orgId = @orgId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", DBNull.Value },
                { "@method", "INSERT" },
                { "@donationId", DBNull.Value },
                { "@itemCategory", category },
                { "@item", item },  
                { "@quantityWithSameExpiryDate", quantity },
                { "@expiryDate", expiryDate },
                { "@orgId", orgId }
            };


            QRY _Qry = new QRY();

            bool success= _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('New item created successfully.');", true);
                clearForm();
                LoadDonationItem();
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There is an error creating new item. Please try again!');", true);

            }

            
        }

        protected void btnAddOtherItem_Click(object sender, EventArgs e)
        {
     
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#addOtherItemModal').modal('show');", true);
        }

           
        protected void PopulateOtherItemCategory()
        {
            string sql = "SELECT categoryName FROM itemCategory";
            DataTable dt = new DataTable();

            QRY _Qry = new QRY();
       
            dt = _Qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                ddlOtherCategory.DataSource = dt;
                ddlOtherCategory.DataTextField = "categoryName";
                ddlOtherCategory.DataValueField = "categoryName";
                ddlOtherCategory.DataBind();
            }


        }

        protected void btnSaveOtherNewItem_Click(object sender, EventArgs e)
        {
            string category = ddlOtherCategory.SelectedValue;
            string item = txtOtherNewItemName.Text;
            string quantity = txtOtherNewItemQuantity.Text;
            
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();


            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(item) || string.IsNullOrEmpty(quantity))
            {
             
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('All fields are required.');", true);
                return;
            }

            string sql = @"
                        EXEC create_inventory_item 
                            @id = @id,
                            @method= @method,
                            @donationId = @donationId, 
                            @itemCategory = @itemCategory, 
                            @item = @item, 
                            @quantity = @quantityWithSameExpiryDate, 
                            @expiryDate = @expiryDate, 
                            @orgId = @orgId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", DBNull.Value },
                { "@method", "INSERT" },
                { "@donationId", DBNull.Value },
                { "@itemCategory", category },
                { "@item", item },
                { "@quantityWithSameExpiryDate", quantity },
                { "@expiryDate", DBNull.Value },
                { "@orgId", orgId }
            };


            QRY _Qry = new QRY();

            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('New item created successfully.');", true);
                clearForm();
                LoadOtherItem();
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There is an error creating new item. Please try again!');", true);

            }


        }

        protected void btnSubmitExpiryDate_Click(object sender, EventArgs e)
        {
         
            string inventoryId = hfInventoryId.Value;
            string expiryDate = txtExpiryDateModal.Text;

            if (!string.IsNullOrEmpty(expiryDate) && !string.IsNullOrEmpty(inventoryId))
            { 
                string query = "UPDATE inventory SET expiryDate = @expiryDate WHERE inventoryId = @inventoryId";

                QRY _Qry = new QRY();
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@expiryDate", expiryDate },
                    { "@inventoryId", inventoryId }
                };

                bool success = _Qry.ExecuteNonQuery(query, parameters);

                if (success)
                {
                    LoadDonationItem();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Item added successfully.');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "HideDatePickerModal", "$('#expiryDateModal').modal('hide');", true);
                    fetchExistingItem();
                }

                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There is an error adding item. Please try again!');", true);
                }
                
            }
            else
            {
                // expiry date not selected
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select an expiry date.');", true);
            }
        }

        [WebMethod]
        public static bool SaveThreshold(string inventoryId, int threshold)
        {
            string sql = "UPDATE inventory SET threshold = @threshold WHERE inventoryId = @inventoryId";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@threshold", threshold },
                { "@inventoryId", inventoryId }
            };

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql, parameters);

            return success;
        }

        [WebMethod]
        public static string GetThreshold(string inventoryId)
        {
            string thresholdValue = string.Empty;
            string sql = "SELECT threshold FROM inventory WHERE inventoryId = @inventoryId";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@inventoryId", inventoryId }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0 && dt.Rows[0]["threshold"] != DBNull.Value)
            {
                thresholdValue = dt.Rows[0]["threshold"].ToString();
            }

            return thresholdValue;
        }

        protected void clearForm()
        {
            ddlCategory.SelectedIndex = 0;
            txtNewItemName.Text = "";
            txtNewItemQuantity.Text = "";
            txtExpiryDate.Text = "";
        }

        protected void showInventoryHistory(object sender, EventArgs e)
        {
            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();
          
            List<InventoryHistory> historyList = GetInventoryHistory(orgId);

            if (historyList != null && historyList.Count > 0)
            {
                inventoryHistoryRepeater.DataSource = historyList;
                inventoryHistoryRepeater.DataBind();

                sideViewContainer.Visible = true;
            }
        }

        public List<InventoryHistory> GetInventoryHistory(string orgId)
        {
            List<InventoryHistory> historyList = new List<InventoryHistory>();

            string sql = "SELECT content, created_on FROM inventory_history WHERE orgId = @orgId ORDER BY created_on DESC";

            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            foreach (DataRow row in dt.Rows)
            {
                historyList.Add(new InventoryHistory
                {
                    content = row["content"].ToString(),
                    createdOn = Convert.ToDateTime(row["created_on"]).ToString("yyyy-MM-dd")
                });
            }

            return historyList;
        }

        [WebMethod]
        public static List<ChartData> GetItemCategory()
        {
            List<ChartData> data = new List<ChartData>();

            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string query = "SELECT itemCategory, COUNT(*) AS count FROM inventory WHERE orgId = @orgId GROUP BY itemCategory";

            QRY qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = qry.GetData(query, parameters);

            foreach (DataRow row in dt.Rows)
            {
                string category = row["itemCategory"].ToString();

                if (category.ToLower().Contains("food"))
                {
                    category = "Food"; // consider it as "Food"
                }

                data.Add(new ChartData
                {
                    label = category,
                    value = row["count"].ToString()
                });
            }

            return data;
        }

        

    }
}