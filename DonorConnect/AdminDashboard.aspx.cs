using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminManageUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPendingApprovalRequestCount();
                LoadRegistrationApprovalCounts();
            }
        }

        protected void btnUser_click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminManageUser.aspx");
        }

        protected void btnApplication_click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminManageApplication.aspx");
        }

        protected void btnRequest_click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminManageDonationRequest.aspx");
        }

        protected void btnItemCategory_click(object sender, EventArgs e)
        {
            Response.Redirect("~/AdminItemCategory.aspx");
        }

        private void LoadPendingApprovalRequestCount()
        {
            string sql = "SELECT COUNT(*) FROM donation_publish WHERE status = 'Pending Approval'";
            QRY qry = new QRY(); 
            int count = Convert.ToInt32(qry.GetScalarValue(sql));

            lblPendingApprovalCount.Text = count.ToString();
        }

        private void LoadRegistrationApprovalCounts()
        {
            // count from both organization and delivery rider tables
            string orgSql = "SELECT COUNT(*) FROM organization WHERE orgStatus = 'Pending Approval'";
            string riderSql = "SELECT COUNT(*) FROM delivery_rider WHERE riderStatus = 'Pending Approval'";

            QRY qry = new QRY();
            int orgCount = Convert.ToInt32(qry.GetScalarValue(orgSql));
            int riderCount = Convert.ToInt32(qry.GetScalarValue(riderSql));

            int totalCount = orgCount + riderCount;
            lblRegistrationApprovalCount.Text = totalCount.ToString();
        }
    }
}