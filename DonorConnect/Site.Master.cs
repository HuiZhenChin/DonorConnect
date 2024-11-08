using System;
using System.Collections.Generic;
using System.Data;
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
            if (!IsPostBack)
            {
                // check if the user is logged in
                if (Session["username"] != null && !string.IsNullOrEmpty(Session["username"].ToString()))
                {
                    // update any necessary notifications
                    UpdateUnreadNotificationCount();

                  
                }
            }
                        
            
        }


        protected void logout(object sender, EventArgs e)
        {
            // clear the session
            Session["username"] = null;
            HttpContext.Current.Session.Abandon();
            Session.Clear();
            Response.Redirect("~/Home");
        }

        private void UpdateUnreadNotificationCount()
        {
            string username = Session["username"].ToString();
            string role = Session["role"].ToString();
            string userId = GetUserId(username, role);
            string sql = "SELECT COUNT(*) FROM notifications WHERE userId = @userId AND isRead = 0";

            var parameters = new Dictionary<string, object>
            {
                { "@userId", userId }
            };

            QRY qry = new QRY();
            int unreadCount = Convert.ToInt32(qry.ExecuteScalar(sql, parameters));

            // update the unread notification count in the navbar
            Label notificationBadge = FindControlRecursive(this, "lblNotificationCount") as Label;
            if (notificationBadge != null)
            {
                notificationBadge.Text = unreadCount > 0 ? unreadCount.ToString() : string.Empty;
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

        private Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control child in root.Controls)
            {
                Control found = FindControlRecursive(child, id);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

    }
}