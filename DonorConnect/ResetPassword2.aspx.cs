﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class ResetPassword2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string token = Request.QueryString["token"];

            if (!IsPostBack && !string.IsNullOrEmpty(token))
            {
                QRY _qry = new QRY();
                string sql = "SELECT used FROM reset_password WHERE password_token = @token";
                var parameter = new Dictionary<string, object>
                {
                    { "@token", token }
                };

                // get data from reset_password table
                DataTable dt = _qry.GetData(sql, parameter);

                if (dt.Rows.Count > 0)
                {
                    bool usedStatus = Convert.ToBoolean(dt.Rows[0]["used"]);

                    // check if used is 1
                    if (usedStatus)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire({ icon: 'error', title: 'Link Used', text: 'This password reset link has already been used.' });", true);
                    }
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            string token = HttpUtility.UrlDecode(Request.QueryString["token"]);
            string role = HttpUtility.UrlDecode(Request.QueryString["role"]);

            try
            {
               
                if (!string.IsNullOrEmpty(token))
                {
                     if (string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtConfirmPassword.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Password fields cannot be empty!', 'warning');", true);
                        return;
                    }

                    if (txtPassword.Text != txtConfirmPassword.Text)
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('The new password and confirm password fields do not match.');", true);
                    }

                    if (txtPassword.Text == txtConfirmPassword.Text)
                    {
                        QRY _Qry = new QRY();
                        DataTable dt_check = new DataTable();
                        string strSQL, message;

                        string password = HashPassword(txtPassword.Text);

                        strSQL = "EXEC [update_password] @method = 'UPDATE' , @token= '" + token + "' , @password= '" + password + "', @role= '" + role + "' ";
                        dt_check = _Qry.GetData(strSQL);
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Your password has been reset successfully.'); window.location='" + ResolveUrl("~/Login.aspx") + "';", true);

                        if (dt_check.Rows.Count > 0)
                        {
                            message = dt_check.Rows[0]["MESSAGE"].ToString();


                            //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + message + "');window.location='" + ResolveUrl("~/Login.aspx") + "';", true);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");

                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Cannot update password, please try again!');", true);
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