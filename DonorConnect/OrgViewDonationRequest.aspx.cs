using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class OrgManageDonationRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = Session["username"].ToString();
                LoadDonation(username);
            }
        }


        private void LoadDonation(string username)
        {
            string sql;
            QRY _Qry = new QRY();
           
            Organization org = new Organization(username, "", "", "", "");
            string id = org.GetOrgId();

            sql = "SELECT donationPublishId, title, itemCategory, urgentStatus, donationState FROM [donation_publish] WHERE orgId = @orgId AND status = 'Opened'";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", id }
            };

            DataTable donations = _Qry.GetData(sql, parameters);
            
            rptDonations.Visible = true;

            rptDonations.DataSource = donations;
            rptDonations.DataBind();

        }

        protected void btnView_Click(object sender, EventArgs e)
        {

            if (Session["username"] != null)
            {
                Button btn = (Button)sender;
                string donationPublishId = btn.CommandArgument;

                Response.Redirect($"OrgManageDonationRequest.aspx?donationPublishId={donationPublishId}");
                

            }

        }
    }
}