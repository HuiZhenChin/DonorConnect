using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
        }


        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Username and Password cannot be empty!', 'warning');", true);
                return;
            }

            try
            {
                string encryptedpass = HashPassword(txtPassword.Text);
                string valid_sql = "EXEC login @username = '" + txtUsername.Text + "' , @password = '" + encryptedpass + "' ";
                QRY checkQry = new QRY();
                DataTable dt_check = checkQry.GetData(valid_sql);

                if (dt_check.Rows.Count > 0)
                {
                    string message = dt_check.Rows[0]["MESSAGE"].ToString();

                    if (message == "Login Successful!")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('Login Successfully!');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "redirect", "setTimeout(function(){ window.location.href='/Home.aspx'; }, 3000);", true);

                        Session["username"] = txtUsername.Text;

                        // get role
                        string sql = "SELECT * FROM [user] WHERE username = '" + txtUsername.Text + "' ";
                        QRY _Qry = new QRY();
                        DataTable _dt = _Qry.GetData(sql);

                        if (_dt.Rows.Count > 0)
                        {
                            string role = _dt.Rows[0]["role"].ToString();
                            Session["role"] = role;
                        }
                    }
                    else if (message == "Admin Registration")
                    {
                        Response.Redirect("~/AdminRegister.aspx");
                    }
                    else if (message == "Incorrect Password!")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Incorrect Password!', 'error');", true);
                    }
                    else if (message == "Account does not exist")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Account does not exist', 'warning');", true);
                    }
                    else if (message == "Your application is still pending for approval")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Your application is still pending for approval', 'warning');", true);
                    }
                    else if (message == "Your account has been terminated")
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Your account has been terminated', 'warning');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('An unexpected error occurred.', 'error');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('An error occurred: " + ex.Message + "', 'error');", true);
            }
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}