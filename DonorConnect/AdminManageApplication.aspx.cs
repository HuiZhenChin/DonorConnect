using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminManageApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrgGridView();
                BindRiderGridView();

            }
        }

        protected void gvOrg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
            }
        }

        protected void gvRider_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }

        private void BindOrgGridView(string status = "Pending Approval")
        {

            string sql = "SELECT orgId, orgName, orgEmail, orgContactNumber, orgAddress, picName, picEmail, picContactNumber, " +
              "orgDescription, orgRegion, createdOn, orgStatus, adminId FROM organization " +
              "WHERE orgStatus = '" + status + "' ";

            if (status != "Pending Approval")
            {
                sql += " AND orgStatus = '" + status + "'";
            }

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvOrg.DataSource = _dt;
            gvOrg.DataBind();

            lblOrg.Visible = true;


        }

        protected void btnViewOrg_click(object sender, EventArgs e)
        {
           
            LinkButton btnViewOrg = (LinkButton)sender;
       
            string orgId = btnViewOrg.CommandArgument;
          
            Session["SelectedOrgId"] = orgId;

            string status = GetOrgStatus(orgId);

            if (status == "Pending Approval")
            {

                Response.Redirect($"AdminViewApplication.aspx?id={orgId}");

            }

            else if (status == "Rejected")
            {
                string reject = "Yes";
                Response.Redirect($"AdminViewApplication.aspx?id={orgId}&reject={reject}");
            }
        }

        private void BindRiderGridView(string status = "Pending Approval")
        {

            string sql = "SELECT riderId, riderUsername, riderFullName, riderEmail, riderContactNumber, vehicleType, vehiclePlateNumber, registerDate, riderStatus, adminId FROM delivery_rider WHERE riderStatus = '" + status + "'";

            if (status != "Pending Approval")
            {
                sql += " AND riderStatus = '" + status + "'";
            }

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvRider.DataSource = _dt;
            gvRider.DataBind();

            lblRider.Visible = true;
        }

        protected void btnViewRider_click(object sender, EventArgs e)
        {
           
            LinkButton btnViewRider = (LinkButton)sender;
         
            string riderId = btnViewRider.CommandArgument;
         
            Session["SelectedRiderId"] = riderId;

            string status = GetRiderStatus(riderId);

            if (status == "Pending Approval")
            {

                Response.Redirect($"AdminViewApplication.aspx?id={riderId}");

            }

            else if (status == "Rejected")
            {
                string reject = "Yes";
                Response.Redirect($"AdminViewApplication.aspx?id={riderId}&reject={reject}");
            }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = ddlStatus.SelectedValue;
            BindOrgGridView(selectedStatus);
            BindRiderGridView(selectedStatus);
        }

        private string GetOrgStatus(string orgId)
        {
            string sql;
            string status = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgId = '" + orgId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                status = _dt.Rows[0]["orgStatus"].ToString();
            }

            return status;
        }

        private string GetRiderStatus(string riderId)
        {
            string sql;
            string status = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [delivery_rider] WHERE riderId = '" + riderId + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                status = _dt.Rows[0]["riderStatus"].ToString();
            }

            return status;
        }

    }
}