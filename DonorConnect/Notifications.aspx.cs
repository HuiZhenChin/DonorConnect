using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class Notifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNotifications();
            }
        }

        private void LoadNotifications()
        {
            string username= Session["username"].ToString();
            string role= Session["role"].ToString();
            string userId = GetUserId(username, role);
            string sql = "SELECT content, link, created_on FROM notifications WHERE userId = @userId ORDER BY created_on DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };

            QRY qry = new QRY();
            DataTable dt = qry.GetData(sql, parameters);

            gvNotifications.DataSource = dt;
            gvNotifications.DataBind();
        }

        protected void gvNotifications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
                // get the encrypted link 
                string encryptedLink = e.CommandArgument.ToString();

                // decrypt the link 
                string decryptedLink;
                try
                {
                    decryptedLink = Encryption.Decrypt(encryptedLink);
                }
                catch (Exception ex)
                {
                    
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error decrypting link.');", true);
                    return;
                }

                // redirect to the decrypted link
                Response.Redirect(decryptedLink);
            }
        }

        private string GetUserId(string username, string role)
        {
            string sql = "";
            string userId = "";

            QRY qry = new QRY();
            DataTable dt;

            switch (role.ToLower())
            {
                case "donor":
                    sql = "SELECT donorId FROM donor WHERE donorUsername = @username";
                    break;
                case "organization":
                    sql = "SELECT orgId FROM organization WHERE orgName = @username";
                    break;
                case "rider":
                    sql = "SELECT riderId FROM delivery_rider WHERE riderUsername = @username";
                    break;
                case "admin":
                    sql = "SELECT adminId FROM admin WHERE adminUsername = @username";
                    break;
                default:
                    return null; 
            }

            var parameters = new Dictionary<string, object>
            {
                { "@username", username }
            };

            dt = qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                userId = dt.Rows[0][0].ToString();
            }

            return userId;
        }



    }
}