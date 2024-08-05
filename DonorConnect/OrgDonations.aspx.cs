using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class PublishDonations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView(string status = "All")
        {
            QRY _Qry = new QRY();
            DataTable _dt = new DataTable();
            string username = Session["username"].ToString();
            string id = GetOrgId(username);
            string strSQL = "SELECT donationPublishId, title, peopleNeeded, description, itemCategory, address, created_on, status FROM [donation_publish] WHERE orgId = '" + id + "'";

            if (status != "All")
            {
                strSQL += " AND status = '" + status + "'";
            }

            _dt = _Qry.GetData(strSQL);
            if (_dt.Rows.Count > 0)
            {
                gvDonations.DataSource = _dt;
                gvDonations.DataBind();
            }
            else
            {
                gvDonations.DataSource = null;
                gvDonations.DataBind();
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            BindGridView(selectedStatus);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            BindGridView(selectedStatus);
        }

        protected void gvDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Customize row data if needed
            }
        }

        protected void btnCreateDonation_Click(object sender, EventArgs e)
        {
            // Redirect to a page or open a modal to create a new donation
            Response.Redirect("~/PublishDonations.aspx");
        }

        private string GetOrgId(string username)
        {
            string sql;
            string id = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                id = _dt.Rows[0]["orgId"].ToString();
            }

            return id;
        }
    }
}