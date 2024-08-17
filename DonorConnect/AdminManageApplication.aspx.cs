using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminManageApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindOrgGridView();
            BindRiderGridView();
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

        private void BindOrgGridView()
        {

            string sql = "SELECT orgId, orgName, orgEmail, orgContactNumber, orgAddress, picName, picEmail, picContactNumber, " +
              "orgDescription, orgRegion, createdOn, orgStatus, adminId FROM organization " +
              "WHERE orgStatus = 'Pending Approval' ";

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvOrg.DataSource = _dt;
            gvOrg.DataBind();
         


        }

        protected void btnViewOrg_click(object sender, EventArgs e)
        {
           
            LinkButton btnView = (LinkButton)sender;
       
            string orgId = btnView.CommandArgument;
          
            Session["SelectedOrgId"] = orgId;

            Response.Redirect($"AdminViewApplication.aspx?id={orgId}");
        }

        private void BindRiderGridView()
        {

            string sql = "SELECT riderId, riderUsername, riderFullName, riderEmail, riderContactNumber, vehicleType, vehiclePlateNumber, registerDate, riderStatus, adminId FROM delivery_rider WHERE riderStatus = 'Pending Approval'";

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvRider.DataSource = _dt;
            gvRider.DataBind();



        }

        protected void btnViewRider_click(object sender, EventArgs e)
        {
           
            LinkButton btnView = (LinkButton)sender;
         
            string riderId = btnView.CommandArgument;
         
            Session["SelectedRiderId"] = riderId;

            Response.Redirect($"AdminViewApplication.aspx?id={riderId}");
        }

    }
}