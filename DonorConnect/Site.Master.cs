using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void logout(object sender, EventArgs e)
        {
            // clear the session
            Session["username"] = null;
            HttpContext.Current.Session.Abandon();
            Session.Clear();
            Response.Redirect("~/Home");
        }
    }
}