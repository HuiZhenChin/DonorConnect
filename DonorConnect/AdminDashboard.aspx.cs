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
    }
}