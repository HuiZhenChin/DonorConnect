using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminItemCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadItemCategories();
            }
        }

        private void LoadItemCategories()
        {
            string sql = "SELECT categoryName, specificItems, hasExpiryDate, categoryIcon FROM itemCategory";
            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql);

            gvItemCategories.DataSource = dt;
            gvItemCategories.DataBind();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkButton btnEdit = (LinkButton)sender;
            string categoryName = btnEdit.CommandArgument;

            Response.Redirect($"EditItemCategory.aspx?categoryName={categoryName}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton btnDelete = (LinkButton)sender;
            string categoryName = btnDelete.CommandArgument;

            // delete the category from the database
            string sql = "DELETE FROM itemCategory WHERE categoryName = @categoryName";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@categoryName", categoryName }
            };

            QRY _Qry = new QRY();
            _Qry.ExecuteQuery(sql, parameters);

            // reload the categories
            LoadItemCategories();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Category deleted successfully!');", true);
        }

        protected void gvItemCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // set the row to edit mode
            gvItemCategories.EditIndex = e.NewEditIndex;
            LoadItemCategories();
        }

        protected void gvItemCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // cancel the editing
            gvItemCategories.EditIndex = -1;
            LoadItemCategories();
        }

        protected void gvItemCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // get the row being edited
            GridViewRow row = gvItemCategories.Rows[e.RowIndex];

            // get the updated values
            string categoryName = ((Label)row.FindControl("lblCategoryName")).Text;
            string specificItems = ((TextBox)row.FindControl("txtSpecificItems")).Text;
            string hasExpiryDate = ((DropDownList)row.FindControl("ddlHasExpiryDate")).SelectedValue;
            string categoryIcon = ((DropDownList)row.FindControl("ddlCategoryIcon")).SelectedValue;

            // categoryIcon has "fa-" prefix
            if (!string.IsNullOrEmpty(categoryIcon) && !categoryIcon.StartsWith("fa-"))
            {
                categoryIcon = "fa-" + categoryIcon;
            }

            // update the database
            QRY _Qry = new QRY();
            string sql = "UPDATE itemCategory SET specificItems = @specificItems, hasExpiryDate = @hasExpiryDate, categoryIcon = @categoryIcon WHERE categoryName = @categoryName";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@categoryName", categoryName },
                { "@specificItems", specificItems },
                { "@hasExpiryDate", hasExpiryDate },
                { "@categoryIcon", categoryIcon }
            };
            _Qry.ExecuteQuery(sql, parameters);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Item updated successfully!');", true);

            // exit edit mode
            gvItemCategories.EditIndex = -1;
            LoadItemCategories();
        }

        protected void gvItemCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvItemCategories.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlCategoryIcon = (DropDownList)e.Row.FindControl("ddlCategoryIcon");

                if (ddlCategoryIcon != null)
                {
                    ddlCategoryIcon.Items.Add(new ListItem("Select Icon", ""));

                    // get the list of Font Awesome icons
                    var iconNames = GetFontAwesomeIcons();

                    // add the icons to the dropdown with "fa-" prefix
                    foreach (var icon in iconNames)
                    {
                        ddlCategoryIcon.Items.Add(new ListItem(icon, "fa-" + icon));
                    }

                    // selected value
                    string currentIcon = DataBinder.Eval(e.Row.DataItem, "categoryIcon").ToString();
                    if (!string.IsNullOrEmpty(currentIcon))
                    {
                        ddlCategoryIcon.SelectedValue = currentIcon;
                    }
                }
            }
        }

        private List<string> GetFontAwesomeIcons()
        {
  

            string url = "https://raw.githubusercontent.com/FortAwesome/Font-Awesome/master/metadata/icons.json";
            using (var webClient = new System.Net.WebClient())
            {
                string json = webClient.DownloadString(url);
                var iconsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                return iconsData.Keys.ToList();
            }
        }

        protected void btnSaveNewCategory_Click(object sender, EventArgs e)
        {
            string newCategoryName = txtNewCategoryName.Text.Trim();

            if (!string.IsNullOrEmpty(newCategoryName))
            {
                try
                {
                    string sql = "INSERT INTO itemCategory (categoryName) VALUES (@CategoryName)";
                    QRY _Qry = new QRY();
                    var parameters = new Dictionary<string, object>
                    {
                        { "@CategoryName", newCategoryName }
                    };

                    bool success = _Qry.ExecuteNonQuery(sql, parameters);

                    if (success)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Category added successfully!');", true);

                        txtNewCategoryName.Text = string.Empty;

                        LoadItemCategories();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Failed to add category. Please try again.');", true);
                    }

                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('An error occurred while adding the category: " + ex.Message + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Category name cannot be empty.');", true);
            }
        }


    }
}
