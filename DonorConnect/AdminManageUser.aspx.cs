using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvDonors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("status");
                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    if (status == "Active")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (status == "Terminated")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }

        protected void gvAdmin_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("status");
                if (lblStatus != null)
                {
                    string status = lblStatus.Text;
                    if (status == "Active")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    else if (status == "Terminated")
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
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

        protected void btnShowDonor_click(object sender, EventArgs e)
        {
            BindDonorsGridView();
        }

        protected void btnShowOrg_click(object sender, EventArgs e)
        {
            BindOrgGridView();
        }

        protected void btnShowRider_click(object sender, EventArgs e)
        {
            BindRiderGridView();
        }

        protected void btnShowAdmin_click(object sender, EventArgs e)
        {
            BindAdminGridView();
        }

        private void BindDonorsGridView()
        {
          
            string sql = "SELECT donorId, donorUsername, donorName, donorEmail, donorContactNumber, donorAddress1, createdOn, status FROM donor";

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvDonors.DataSource = _dt;
            gvDonors.DataBind();
            donor.Style["display"] = "block";
            org.Style["display"] = "none";
            rider.Style["display"] = "none";
            admin.Style["display"] = "none";
        }

        private void BindOrgGridView()
        {

            string sql = "SELECT orgId, orgName, orgEmail, orgContactNumber, orgAddress, picName, picEmail, picContactNumber, " +
              "orgDescription, orgRegion, createdOn, orgStatus, adminId FROM organization " +
              "WHERE orgStatus NOT IN ('Pending Approval', 'Rejected')"; 

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvOrg.DataSource = _dt;
            gvOrg.DataBind();
            donor.Style["display"] = "none";
            org.Style["display"] = "block";
            rider.Style["display"] = "none";
            admin.Style["display"] = "none";


        }

        private void BindRiderGridView()
        {

            string sql = "SELECT riderId, riderUsername, riderFullName, riderEmail, riderContactNumber, vehicleType, vehiclePlateNumber, registerDate, riderStatus, adminId FROM delivery_rider WHERE riderStatus NOT IN('Pending Approval', 'Rejected')"; 


            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvRider.DataSource = _dt;
            gvRider.DataBind();
            donor.Style["display"] = "none";
            org.Style["display"] = "none";
            rider.Style["display"] = "block";
            admin.Style["display"] = "none";

        }

        private void BindAdminGridView()
        {

            string sql = "SELECT adminId, adminUsername, adminEmail, status, created_on, isMain FROM admin ";
           

            QRY _Qry = new QRY();
            DataTable _dt;
            _dt = _Qry.GetData(sql);

            gvAdmin.DataSource = _dt;
            gvAdmin.DataBind();
            donor.Style["display"] = "none";
            org.Style["display"] = "none";
            rider.Style["display"] = "none";
            admin.Style["display"] = "block";

        }

    }
}