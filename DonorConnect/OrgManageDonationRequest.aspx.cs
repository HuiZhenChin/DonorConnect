using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static DonorConnect.DonationRequest;

namespace DonorConnect
{
    public partial class OrgManageDonationRequest2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string donationPublishId = Request.QueryString["donationPublishId"];
                if(donationPublishId != null)
                {
                    LoadDonationRequest(donationPublishId);

                }
                
            }
        }

        private void LoadDonationRequest(string donationPublishId)
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();

            string sql = @"
            SELECT d.donationPublishId, d.donationId, 
                   CASE 
                       WHEN d.itemCategory IN ('Food (Canned/Packed Food)', 'Food (Cooked Food)') THEN 'Food'
                       ELSE d.itemCategory 
                   END AS itemCategory, 
                   d.quantityDonated, d.item, d.created_on, 
                   d.donorFullName, d.donorEmail, d.pickUpAddress, d.state, 
                   ic.categoryIcon
            FROM [donation_item_request] d
            JOIN [itemCategory] ic ON 
                CASE 
                    WHEN d.itemCategory IN ('Food (Canned/Packed Food)', 'Food (Cooked Food)') THEN 'Food'
                    ELSE d.itemCategory 
                END = ic.categoryName
            WHERE d.donationPublishId = '" + donationPublishId + @"' 
              AND d.requestStatus = 'Pending'";

            _dt = _Qry.GetData(sql);
            if (_dt.Rows.Count > 0)
            {
                DataTable processedTable = new DataTable();
                processedTable.Columns.Add("donationPublishId");
                processedTable.Columns.Add("donationId");
                processedTable.Columns.Add("itemCategory");
                processedTable.Columns.Add("quantityDonated");
                processedTable.Columns.Add("item");
                processedTable.Columns.Add("created_on");
                processedTable.Columns.Add("donorFullName");
                processedTable.Columns.Add("donorEmail");
                processedTable.Columns.Add("pickUpAddress");
                processedTable.Columns.Add("state");
                processedTable.Columns.Add("categoryIcon");

                Dictionary<string, DataRow> groupedRows = new Dictionary<string, DataRow>();

                // update the grouping logic to include the icon information
                foreach (DataRow row in _dt.Rows)
                {
                    string donationId = row["donationId"].ToString();

                    // check if the category is Food (Canned/Packed Food) or Food (Cooked Food) and normalize it to "Food"
                    string itemCategory = row["itemCategory"].ToString();
                  
                    // check if this donationId is already in the groupedRows
                    if (groupedRows.ContainsKey(donationId))
                    {
                        // concatenate the itemCategory, item, quantityDonated, and categoryIcon for the same donationId
                        groupedRows[donationId]["itemCategory"] += ", " + itemCategory;
                        groupedRows[donationId]["item"] += ", " + row["item"];
                        groupedRows[donationId]["quantityDonated"] += ", " + row["quantityDonated"];
                        groupedRows[donationId]["categoryIcon"] += ", " + row["categoryIcon"];
                    }
                    else
                    {
                        // create a new row for this donationId
                        DataRow newRow = processedTable.NewRow();
                        newRow["donationPublishId"] = row["donationPublishId"];
                        newRow["donationId"] = row["donationId"];
                        newRow["itemCategory"] = itemCategory; 
                        newRow["quantityDonated"] = row["quantityDonated"];
                        newRow["item"] = row["item"];
                        newRow["created_on"] = row["created_on"];
                        newRow["donorFullName"] = row["donorFullName"];
                        newRow["donorEmail"] = row["donorEmail"];
                        newRow["pickUpAddress"] = row["pickUpAddress"];
                        newRow["state"] = row["state"];
                        newRow["categoryIcon"] = row["categoryIcon"]; // add the icon to the row

                        // add this row to the dictionary and processedTable
                        groupedRows[donationId] = newRow;
                        processedTable.Rows.Add(newRow);
                    }
                }

                // bind the processed table to the GridView
                gvDonations.DataSource = processedTable;
                gvDonations.DataBind();
            }
            else
            {
                gvDonations.DataSource = null;
                gvDonations.DataBind();
            }
        }

        protected void gvDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                
                string[] categories = rowView["itemCategory"].ToString().Split(',');
                string[] icons = rowView["categoryIcon"].ToString().Split(',');

                
                HtmlGenericControl ul = (HtmlGenericControl)e.Row.FindControl("itemList");

               
                for (int i = 0; i < categories.Length; i++)
                {
                    
                    HtmlGenericControl li = new HtmlGenericControl("li");

                    // get the specific border color class based on the category
                    string borderColorClass = GetBorderColorClass(categories[i].Trim());

                    // create the inner HTML with the category and icon 
                    li.InnerHtml = $"<div class='category-box {borderColorClass}'>" +
                                   $"<i class='fa {icons[i].Trim()}'></i> {categories[i].Trim()}" +
                                   $"</div>";

                    // add the <li> to the <ul>
                    ul.Controls.Add(li);
                }

         
            }
        }
        private string GetBorderColorClass(string category)
        {
            switch (category.Trim().ToLower())
            {
                case "food":
                case "food (canned/packed food)":
                case "food (cooked food)":
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

        protected void btnView_Click(object sender, EventArgs e)
        {

            if (Session["username"] != null)
            {
                Button btn = (Button)sender;
                string donationId = btn.CommandArgument;

                Response.Redirect($"OrgReviewDonationRequest.aspx?donationId={donationId}");


            }

        }



    }
}