using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class SignUpOTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the email and username from the query string
                string email = Request.QueryString["email"];
                string username = Request.QueryString["username"];
                if (!string.IsNullOrEmpty(email))
                {
                    // Store the email in the session
                    Session["EmailAddress"] = email;
                }

                if (!string.IsNullOrEmpty(username))
                {
                    // Store the username in the session
                    Session["Username"] = username;
                }

                if (Session["EmailAddress"] != null)
                {
                    lblEmail.Text = Session["EmailAddress"].ToString();
                }
                else
                {
                    lblEmail.Text = "Unknown";
                }

                // generate OTP and send it via email
                generateOTP();
            }
        }

        protected void generateOTP()
        {
            // Generate a 6-digit random OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // Store the OTP and generation time in the session
            Session["GeneratedOTP"] = otp;
            Session["OTPExpirationTime"] = DateTime.Now;

            // Retrieve email and username from the session
            string email = Session["EmailAddress"].ToString();
            string username = Session["Username"].ToString();

            // Call the stored procedure to send the OTP email
            QRY NewQry = new QRY();

            string sql = "EXEC [signup_otp] " +
                         "@email = '" + email + "', " +
                         "@username = '" + username + "', " +
                         "@otp = '" + otp + "'";

            DataTable dt_check = NewQry.GetData(sql);

            if (dt_check.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('The OTP is sent to the entered email address, please check your inbox.!');", true);
            }
        }

        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            string enteredOTP = txtOTP.Text.Trim();

            // Retrieve the generated OTP and generation time from the session
            string generatedOTP = Session["GeneratedOTP"] as string;
            DateTime? otpGenerationTime = Session["OTPExpirationTime"] as DateTime?;

            // Check if the OTP has expired (valid for 2 minutes)
            if (otpGenerationTime.HasValue && DateTime.Now.Subtract(otpGenerationTime.Value).TotalMinutes <= 2)
            {
                // Implement OTP verification logic
                if (enteredOTP == generatedOTP)
                {
                    // OTP verified successfully
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('OTP verified successfully!');", true);
                    //string email = Request.QueryString["email"];
                    //string username = Request.QueryString["username"];
                    //Response.Redirect($"SignUp.aspx?email={email}&username={username}");

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Invalid OTP. Please try again.');", true);
                    
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('OTP has expired. Please request a new OTP.');", true);

                
            }
        }
    }
}