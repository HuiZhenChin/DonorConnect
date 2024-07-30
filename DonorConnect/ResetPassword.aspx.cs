using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;

            // Search the database for the user
            string email = GetUserEmailFromDatabase(username);

            string role = GetUserRoleFromDatabase(username);

            if (!string.IsNullOrEmpty(email))
            {
                lblEmail.Text = email;
                confirmationSection.Visible = true;
            }
            if (!string.IsNullOrEmpty(role))
            {
                lblRole.Text = role;
                confirmationSection.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('User not found!');", true);
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string email = lblEmail.Text;

            string username = txtUsername.Text;

            string role = lblRole.Text;

            Session["Role"] = role;

            string strSQL, strSQL1;
            QRY _Qry = new QRY();
            

            string token = Hash(username + email + DateTime.Now.ToString());
            token = token.Replace("+", "A");

            string resetLink = "https://localhost:44390/ResetPassword2.aspx?token=" + token + "&role=" + HttpUtility.UrlEncode(role);

            strSQL = "EXEC [reset_password_email]  @email= '" + email + "' , @username= '" + username + "' , @resetLink='" + resetLink + "' ";
            _Qry.GetData(strSQL);

            strSQL1 = "EXEC [update_password] @method = 'INSERT' , @email= '" + email + "' , @username='" + username + "' , @token= '" + token + "' , @role= '" + role + "' ";
            _Qry.GetData(strSQL1);


          
        }

       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        private string GetUserEmailFromDatabase(string username)
        {
            string sql;
            string email="";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [user] WHERE username = '" + txtUsername.Text + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                email = _dt.Rows[0]["email"].ToString();
                lblEmail.Text = email;

                Session["Email"] = email;
            }

            return email;
        }

        private string GetUserRoleFromDatabase(string username)
        {
            string sql;
            string role = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [user] WHERE username = '" + txtUsername.Text + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                role = _dt.Rows[0]["role"].ToString();
                lblRole.Text = role;

                Session["Role"] = role;
            }

            return role;
        }

    }


}