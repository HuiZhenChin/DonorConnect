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
            string sql = "SELECT notificationId, content, link, created_on, isRead FROM notifications WHERE userId = @userId ORDER BY created_on DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };

            QRY qry = new QRY();
            DataTable dt = qry.GetData(sql, parameters);

            if (dt.Rows.Count == 0)
            {
                lblNoNotifications.Text = "You have no notifications.";
                lblNoNotifications.Visible = true;
                gvNotifications.Visible = false;
            }
            else
            {
                gvNotifications.DataSource = dt;
                gvNotifications.DataBind();

                lblNoNotifications.Visible = false;
                gvNotifications.Visible = true;
            }
        }

        protected void gvNotifications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Redirect")
            {
             
                string encryptedLink = e.CommandArgument.ToString();
                string decodedEncryptedLink = HttpUtility.UrlDecode(encryptedLink);

                string decryptedLink;
                try
                {
                    decryptedLink = Encryption.Decrypt(decodedEncryptedLink);
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error decrypting link.');", true);
                    return;
                }

                // mark the notification as read using the link
                MarkNotificationAsRead(encryptedLink);

                // redirect to the decrypted link
                Response.Redirect(decryptedLink);
            }
        }

        private void MarkNotificationAsRead(string link)
        {
            string sql;
            var parameters = new Dictionary<string, object>
            {
                { "@link", link }
            };

            // check if the current role is admin
            if (Session["role"]?.ToString() == "admin")
            {
                string username = Session["username"]?.ToString();

                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("No username in session.");
                    return;
                }

                sql = "UPDATE notifications SET isRead = 1 WHERE link = @link AND userId = (SELECT adminId FROM admin WHERE adminUsername = @username)";
                parameters.Add("@username", username);
            }
            else
            {
                sql = "UPDATE notifications SET isRead = 1 WHERE link = @link";
            }

            QRY _Qry = new QRY();
            _Qry.ExecuteNonQuery(sql, parameters);
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