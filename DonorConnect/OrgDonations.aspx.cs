using System;
using System.Collections.Generic;
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

        }

        protected void btnCreateDonation_Click(object sender, EventArgs e)
        {
            // Redirect to a page or open a modal to create a new donation
            Response.Redirect("~/PublishDonations.aspx");
        }
    }
}