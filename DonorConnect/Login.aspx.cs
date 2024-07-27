using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
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
            try { 
            
            string encryptedpass = null;
            encryptedpass = HashPassword(txtPassword.Text);
            string valid_sql = "EXEC login @username = '" + txtUsername.Text + "' , @password = '" + encryptedpass + "' ";
            QRY checkQry = new QRY();
            DataTable dt_check = new DataTable();
            dt_check = checkQry.GetData(valid_sql);
            if (dt_check.Rows.Count > 0)
            {
                string message = dt_check.Rows[0]["MESSAGE"].ToString();

                if (message == "Login Successful!")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('Login Successfully!', 'success');", true);
                    Session["username"] = txtUsername.Text;
                    Response.Redirect("/Home.aspx");
                }
                else if (message == "Incorrect Password!")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('Incorrect Password!', 'danger');", true);
                }
                else if (message == "Account does not exist")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('Account does not exist', 'warning');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('An unexpected error occurred.', 'danger');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('An error occurred: " + ex.Message + "', 'danger');", true);
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