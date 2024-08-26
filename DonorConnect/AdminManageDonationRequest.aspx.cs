using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminManageDonationRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDonationGridView();
               
            }
        }

        protected void gvDonation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                string urgentStatus = DataBinder.Eval(e.Row.DataItem, "urgentStatus").ToString();

                // if urgentStatus is 'Yes'
                if (urgentStatus == "Yes")
                {
                    // set the row color to pink
                    e.Row.BackColor = System.Drawing.Color.Pink;

                    HtmlGenericControl faFireIcon = (HtmlGenericControl)e.Row.FindControl("fireIcon");
                    if (faFireIcon != null)
                    {
                        faFireIcon.Visible = true;
                    }
                }

                string itemCategory = DataBinder.Eval(e.Row.DataItem, "itemCategory").ToString();

                // remove the brackets and any extra spaces
                itemCategory = itemCategory.Trim('[', ']');

                // split the categories and format them as an unordered list, split each category into a new line each
                string[] categories = itemCategory.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string formattedCategory = "<ul>";
                foreach (string category in categories)
                {
                    formattedCategory += $"<li>{category.Trim()}</li>";
                }
                formattedCategory += "</ul>";

                // set the formatted category to the Literal for ui display
                Literal litItemCategory = (Literal)e.Row.FindControl("litItemCategory");
                if (litItemCategory != null)
                {
                    litItemCategory.Text = formattedCategory;
                }
            }
        }


        private void BindDonationGridView(string status = "Pending Approval")
        {
            string sql = "SELECT dp.donationPublishId, dp.title, dp.address, dp.itemCategory, dp.orgId, org.orgName, dp.donationState, dp.restriction, dp.created_on, dp.adminId";

            if (status == "Rejected")
            {
                sql += ", dp.rejectedReason";
            }
            else if (status == "Closed")
            {
                sql += ", dp.closureReason";
            }

            sql += ", dp.urgentStatus FROM donation_publish dp";
            sql += " LEFT JOIN organization org ON dp.orgId = org.orgId";  //join table to get organization name
            sql += " WHERE dp.status = '" + status + "'";

            // order by urgentStatus (descending) and created_on (descending)
            sql += " ORDER BY dp.urgentStatus DESC, dp.created_on DESC";

            QRY _Qry = new QRY();
            DataTable _dt = _Qry.GetData(sql);

            gvDonation.DataSource = _dt;
            gvDonation.DataBind();
        }




        protected void btnViewDonation_click(object sender, EventArgs e)
        {

            LinkButton btnView = (LinkButton)sender;

            string donationPublishId = btnView.CommandArgument;

            Session["SelectedDonationPublishId"] = donationPublishId;

            string status = GetStatus(donationPublishId);

            string urgency = GetUrgency(donationPublishId);

            if (status == "Pending Approval")
            {

                Response.Redirect($"AdminViewDonationRequest.aspx?id={donationPublishId}&urgent={urgency}");

            }

            else if (status == "Rejected")
            {
                string reject = "Yes";
                Response.Redirect($"AdminViewDonationRequest.aspx?id={donationPublishId}&urgent={urgency}&reject={reject}");
            }
        }

        
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            BindDonationGridView(selectedStatus);
            
        }

        private string GetStatus(string donationPublishId)
        {
            string sql;
            string status = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                status = _dt.Rows[0]["status"].ToString();
            }

            return status;
        }

        private string GetUrgency(string donationPublishId)
        {
            string sql;
            string urgency = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donation_publish] WHERE donationPublishId = '" + donationPublishId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                urgency = _dt.Rows[0]["urgentStatus"].ToString();
            }

            return urgency;
        }


    }
}