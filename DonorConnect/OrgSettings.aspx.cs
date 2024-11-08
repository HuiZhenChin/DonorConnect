using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class OrgSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadReminderSetting();
            }
        }

        protected void SaveReminderSetting_Click(object sender, EventArgs e)
        {
            string reminderDay = reminderDaysTextBox.Text;

            if (int.TryParse(reminderDay, out int days))
            {
                if (days < 7)
                {
                    // if the value is less than 7
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire('Error', 'Please enter at least 7 days.', 'error');", true);
                }
                else
                {
                    // if the value is valid
                    SaveReminderSetting(days);
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire('Success', 'Reminder setting saved!', 'success');", true);
                }
            }
            else
            {
                // if the input is invalid number
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire('Error', 'Please enter a valid number of days.', 'error');", true);
            }
        }


        private void SaveReminderSetting(int days)
        {

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = "UPDATE organization SET expiryDateReminder = @reminderDay WHERE orgId = @orgId";

            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@reminderDay", days },
                { "@orgId", orgId }
            };

            _Qry.ExecuteNonQuery(sql, parameters);
        }

        private void LoadReminderSetting()
        {

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = "SELECT expiryDateReminder FROM organization WHERE orgId = @orgId";

            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                reminderDaysTextBox.Text = dt.Rows[0]["expiryDateReminder"].ToString();
            }
        }
    }
}