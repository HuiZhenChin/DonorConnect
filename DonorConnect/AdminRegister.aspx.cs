using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminRegister : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initial page load actions (if any)

            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = true;

                if (txtEmail.Text == "")
                {
                    lblEmail.Text = "Email is required";
                    isValid = false;
                }

                string sql;
                QRY _Qry = new QRY();
                DataTable _dt;
                sql = "SELECT * FROM [admin] WHERE adminEmail = '" + txtEmail.Text + "' ";

                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Email already exists', 'warning');", true);
                    isValid = false;
                }

                if (isValid)
                {
                    generateOTP();
                    otpSection.Style["display"] = "block";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showMessage('The OTP is sent to the entered email address, please check your inbox!');", true);
                }

            }

            catch (Exception ex)
            {
                Response.Write("An error occurred: " + ex.Message);
            }
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
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

        protected void generateOTP()
        {
            // generate a 6-digit random OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // store the OTP and generation time in the session
            Session["GeneratedOTP"] = otp;
            Session["OTPExpirationTime"] = DateTime.Now;

            string email = txtEmail.Text;

            // call the stored procedure to send the OTP email
            QRY NewQry = new QRY();

            string sql = "EXEC [signup_otp] " +
                        "@email = '" + email + "', " +
                        "@username = '" + email + "', " +
                        "@otp = '" + otp + "'";

            DataTable dt_check = NewQry.GetData(sql);

            if (dt_check.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('The OTP is sent to the entered email address, please check your inbox.!');", true);
            }
        }

        // verify otp function
        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            if (txtOtp.Text == "")
            {
                lblOtp.Text = "Please enter OTP";
            }

            string enteredOTP = txtOtp.Text.Trim();

            // retrieve the generated OTP and generation time 
            string generatedOTP = Session["GeneratedOTP"] as string;
            DateTime? otpGenerationTime = Session["OTPExpirationTime"] as DateTime?;

            // check if the OTP has expired (valid for 2 minutes)
            if (otpGenerationTime.HasValue && DateTime.Now.Subtract(otpGenerationTime.Value).TotalMinutes <= 2)
            {
                // otp verification
                if (enteredOTP == generatedOTP)
                {
                    // OTP verified successfully
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showSuccess('OTP verified successfully!');", true);

                    QRY _Qry2 = new QRY();
                    string status = "Active";
                    string generatedPassword = GenerateRandomPassword();
                    string hashedPassword = HashPassword(generatedPassword);
                    string register_sql = "EXEC [create_admin] " +
                                   "@method = 'INSERT', " +
                                   "@adminId = NULL, " +  // adminId will be generated in the stored procedure
                                   "@adminUsername = NULL, " +
                                   "@adminEmail = '" + txtEmail.Text + "', " +
                                   "@adminHashPassword = '" + hashedPassword + "', " +
                                   "@adminPassword = '" + generatedPassword + "', " +
                                   "@status = '" + status + "' ";


                    DataTable dt_check = _Qry2.GetData(register_sql);

                    if (dt_check.Rows.Count > 0)
                    {
                        string message = dt_check.Rows[0]["MESSAGE"].ToString();
                        if (message == "Successful! You have registered as an admin! The login details are sent in DonorConnect official email address. You may login and manage the application now.")
                        {

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");

                        }

                        else
                        {
                            // Show SweetAlert error dialog
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                        }
                    }


                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Invalid OTP. Please try again.');", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('OTP has expired. Please request a new OTP.');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "redirect", "setTimeout(function(){ window.location.href='/Login.aspx'; }, 3000);", true);



            }
        }
    }
}