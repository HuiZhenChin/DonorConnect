using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class Home : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAchievementCount();
            }
        }

        protected void btnViewDonation_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/AllDonations.aspx");
        }

        private void LoadAchievementCount()
        {
            QRY _Qry = new QRY();

            // total donations made
            DataTable donationsTable = _Qry.GetData("SELECT COUNT(*) AS TotalDonations FROM donation_item_request");
            int totalDonations = donationsTable.Rows.Count > 0 ? Convert.ToInt32(donationsTable.Rows[0]["TotalDonations"]) : 0;

            // total deliveries made
            DataTable deliveriesTable = _Qry.GetData("SELECT COUNT(*) AS TotalDeliveries FROM delivery");
            int totalDeliveries = deliveriesTable.Rows.Count > 0 ? Convert.ToInt32(deliveriesTable.Rows[0]["TotalDeliveries"]) : 0;

            // total registered users
            DataTable usersTable = _Qry.GetData("SELECT COUNT(*) AS RegisteredUsers FROM [user] WHERE role <> 'admin'");
            int registeredUsers = usersTable.Rows.Count > 0 ? Convert.ToInt32(usersTable.Rows[0]["RegisteredUsers"]) : 0;

            totalDonationsLabel.Text = totalDonations.ToString();
            totalDeliveriesLabel.Text = totalDeliveries.ToString();
            registeredUsersLabel.Text = registeredUsers.ToString();
        }

    }
}
