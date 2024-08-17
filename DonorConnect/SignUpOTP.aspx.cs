using Microsoft.AspNet.FriendlyUrls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace DonorConnect
{
    public partial class SignUpOTP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // retrieve the email and username passed from the Sign Up Page
                string email = Request.QueryString["email"];
                string username = Request.QueryString["username"];
                string role = Request.QueryString["selectedRole"];
                if (!string.IsNullOrEmpty(email))
                {
                    // store the email in the session
                    Session["EmailAddress"] = email;
                }

                if (!string.IsNullOrEmpty(username))
                {
                    // store the username in the session
                    Session["Username"] = username;
                }

                if (!string.IsNullOrEmpty(role))
                {
                    // store the role in the session
                    Session["Role"] = role;
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
            // generate a 6-digit random OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // store the OTP and generation time in the session
            Session["GeneratedOTP"] = otp;
            Session["OTPExpirationTime"] = DateTime.Now;

            // retrieve email and username from the session
            string email = Session["EmailAddress"].ToString();
            string username = Session["Username"].ToString();

            // call the stored procedure to send the OTP email
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

        // verify otp function
        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            string enteredOTP = txtOTP.Text.Trim();

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

                    // perform user registration based on role
                    RegisterUser();


                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Invalid OTP. Please try again.');", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('OTP has expired. Please request a new OTP.');", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "redirect", "setTimeout(function(){ window.location.href='/SignUp.aspx'; }, 3000);", true);



            }
        }

        private void RegisterUser()
        {
            string role = Session["Role"].ToString();
            string email = Session["EmailAddress"].ToString();
            string username = Session["Username"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["DCConnString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                switch (role)
                {
                    case "donor":

                        RegisterDonor(con);
                        
                        break;
                    case "organization":
                        RegisterOrganization(con);
                        
                        break;
                    case "rider":
                        RegisterRider(con);
                       
                        break;
                    default:
                        // handle unknown role
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Please select a role!');", true);
                        break;
                }
            }
        }


        private void RegisterDonor(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                string name = Session["DonorName"].ToString();
                string contactNumber = Session["DonorContactNumber"].ToString();
                string password = Session["DonorPassword"].ToString();
                string email = Session["EmailAddress"].ToString();
                string username = Session["Username"].ToString();
                string status = "Active";

                // hash the password 
                string hashedPassword = HashPassword(password);

                string donor_sql = "EXEC [create_donor] " +
                                   "@method = 'INSERT', " +
                                   "@donorId = NULL, " +  // donorId will be generated in the stored procedure
                                   "@donorName = '" + name + "', " +
                                   "@donorUsername = '" + username + "', " +
                                   "@donorEmail = '" + email + "', " +
                                   "@donorContactNumber = '" + contactNumber + "', " +
                                   "@donorHashPassword = '" + hashedPassword + "', " +
                                   "@donorNewHashPassword = NULL, " +
                                   "@donorAddress1 = NULL," +    
                                   "@donorProfilePicBase64 = NULL, " +
                                   "@status= '" + status + "' ";


                DataTable dt_check = NewQry.GetData(donor_sql);

                if (dt_check.Rows.Count > 0)
                {
                    string message = dt_check.Rows[0]["MESSAGE"].ToString();
                    if (message == "Successful! You have registered as a donor. You may start your item donation journey now!")
                    {
                        Session["message"] = message;
                        SendConfirmationEmail(con);

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");

                    }
                    
                    else
                    {
                        // Show SweetAlert error dialog
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                    }
                }

               
                Session.Clear();


            }
            catch (Exception ex)
            {
                // error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('An error occurred: " + ex.Message + "');", true);
            }
        }

        private void RegisterOrganization(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                string orgName = Session["OrgName"].ToString();
                string orgContactNumber = Session["OrgContactNumber"].ToString();
                string orgAddress = Session["OrgAddress"].ToString();
                string orgEmail = Session["OrgEmail"].ToString();
                string picName = Session["PicName"].ToString();
                string picEmail = Session["PicEmail"].ToString();
                string picNumber = Session["PicNumber"].ToString();
                string password = Session["OrgPassword"].ToString();
                string region = Session["OrgRegion"].ToString();
                string businessLicense = Session["OrgLicense"].ToString();

                // hash the password 
                string hashedPassword = HashPassword(password);

                // initial status after registration (waiting for admin approval)
                string status = "Pending Approval";

                string org_sql = "EXEC [create_organization] " +
                                   "@method = 'INSERT', " +
                                   "@orgId = NULL, " +  // orgId will be generated in the stored procedure
                                   "@orgName = '" + orgName + "', " +
                                   "@orgEmail = '" + orgEmail + "', " +
                                   "@orgContactNumber = '" + orgContactNumber + "', " +
                                   "@orgHashPassword = '" + hashedPassword + "', " +
                                   "@orgNewHashPassword = NULL, " +
                                   "@orgAddress = '" + orgAddress + "', " +
                                   "@businessLicenseImageBase64 ='" + businessLicense + "', " +
                                   "@picName = '" + picName + "', " +
                                   "@picEmail = '" + picEmail + "', " +
                                   "@picContactNumber = '" + picNumber + "', " +
                                   "@orgProfilePicBase64 = NULL, " +
                                   "@orgDescription= NULL, " +
                                   "@mostNeededItemCategory = NULL, " +
                                   "@orgRegion = '" + region + "', " +
                                   "@orgStatus = '" + status + "', " +
                                   "@adminId = NULL ";


                DataTable dt_check = NewQry.GetData(org_sql);

                if (dt_check.Rows.Count > 0)
                {
                    string message = dt_check.Rows[0]["MESSAGE"].ToString();
                    if (message == "Successful! You have registered as an organization! Your application is pending for approval now.")
                    {
                        Session["message"] = message;
                        SendConfirmationEmail(con);

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");

                    }
                    
                    else
                    {
                        // Show SweetAlert error dialog
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                    }
                }


                Session.Clear();


            }

            catch (Exception ex)
            {
                // error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('An error occurred: " + ex.Message + "');", true);
            }
        }

        private void RegisterRider(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                string name = Session["RiderName"].ToString();
                string contactNumber = Session["RiderContactNumber"].ToString();
                string vehiclePlateNo = Session["VehiclePlateNo"].ToString();
                string vehicleType = Session["VehicleType"].ToString();
                string password = Session["RiderPassword"].ToString();
                string drivingLicense = Session["RiderCarLicense"].ToString();
                string facePic = Session["RiderFacePhoto"].ToString();
                string username = Session["RiderUsername"].ToString();
                string email = Session["RiderEmail"].ToString();

                // hash the password 
                string hashedPassword = HashPassword(password);

                // initial status after registration (waiting for admin approval)
                string status = "Pending Approval";

                string rider_sql = "EXEC [create_rider] " +
                                   "@method = 'INSERT', " +
                                   "@riderId = NULL, " +  // riderId will be generated in the stored procedure
                                   "@riderFullName = '" + name + "', " +
                                   "@riderUsername = '" + username + "', " +
                                   "@riderEmail = '" + email + "', " +
                                   "@riderContactNumber = '" + contactNumber + "', " +
                                   "@riderHashPassword = '" + hashedPassword + "', " +
                                   "@riderNewHashPassword = NULL, " +
                                   "@drivingLicenseImageBase64 = '" + drivingLicense + "', " +
                                   "@vehicleType = '" + vehicleType + "', " +
                                   "@riderFacePicBase64 = '" + facePic + "', " +
                                   "@noOfDeliveryMade = NULL," +
                                   "@walletAmount = NULL," +
                                   "@vehiclePlateNumber = '" + vehiclePlateNo + "', " +
                                   "@registerDate = NULL," +
                                   "@riderStatus = '" + status + "', " +
                                   "@adminId = NULL";


                DataTable dt_check = NewQry.GetData(rider_sql);

                if (dt_check.Rows.Count > 0)
                {
                    string message = dt_check.Rows[0]["MESSAGE"].ToString();
                    if (message == "Successful! You have registered as a delivery rider! Your application is pending for approval now.")
                    {
                        Session["message"] = message;
                        SendConfirmationEmail(con);

                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>showSuccess('" + message + "');</script>");

                    }
                   
                    else
                    {
                        // Show SweetAlert error dialog
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Error in signing up!', 'error');", true);
                    }
                }


                Session.Clear();


            }
            catch (Exception ex)
            {
                // error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('An error occurred: " + ex.Message + "');", true);
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

        // do encryption for license




        private void SendConfirmationEmail(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                string username = Session["Username"].ToString();
                string email = Session["EmailAddress"].ToString();
                string role = Session["Role"].ToString();


                string email_sql = "EXEC [signup_confirm] " +
                                "@role = '" + role + "', " +
                                "@username = '" + username + "', " +
                                "@email = '" + email + "' ";


                DataTable dt_check = NewQry.GetData(email_sql);
                Session.Clear();



            }
            catch (Exception ex)
            {
                // error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('An error occurred: " + ex.Message + "');", true);
            }
        }
    }
}