using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DonorConnect
{
    public partial class SignUp : System.Web.UI.Page
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

                string role = selectedRole.Value;

                bool isValid = true;

                switch (role)
                {
                    case "donor":
                        if (donorName.Text == "")
                        {
                            lblDonorName.Text = "Name is required";
                            isValid = false;
                        }
                        if (donorUsername.Text == "")
                        {
                            lblDonorUsername.Text = "Username is required";
                            isValid = false;
                        }
                        if (donorEmail.Text == "")
                        {
                            lblDonorEmail.Text = "Email is required";
                            isValid = false;
                        }
                        if (donorContactNumber.Text == "")
                        {
                            lblDonorContactNumber.Text = "Contact Number is required";
                            isValid = false;
                        }
                        if (donorPassword.Text == "")
                        {
                            lblDonorPassword.Text = "Password is required";
                            isValid = false;
                        }
                        if (donorConfirmPassword.Text == "")
                        {
                            lblDonorConfirmPassword.Text = "Confirm Password is required";
                            isValid = false;
                        }
                        if (donorPassword.Text != donorConfirmPassword.Text)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Password and Confirm Password do not match!');", true);
                            isValid = false;
                        }
                        break;

                    case "organization":
                        if (orgName.Text == "")
                        {
                            lblOrgName.Text = "Organization Name is required";
                            isValid = false;
                        }
                        if (orgEmail.Text == "")
                        {
                            lblOrgEmail.Text = "Organization Email is required";
                            isValid = false;
                        }
                        if (orgContactNumber.Text == "")
                        {
                            lblOrgContactNumber.Text = "Organization Contact Number is required";
                            isValid = false;
                        }
                        if (orgAddress.Text == "")
                        {
                            lblOrgAddress.Text = "Organization Address is required";
                            isValid = false;
                        }
                        if (picName.Text == "")
                        {
                            lblPicName.Text = "PIC Name is required";
                            isValid = false;
                        }
                        if (picEmail.Text == "")
                        {
                            lblPicEmail.Text = "PIC Email is required";
                            isValid = false;
                        }
                        if (picNumber.Text == "")
                        {
                            lblPicNumber.Text = "PIC Contact Number is required";
                            isValid = false;
                        }
                        if (orgPassword.Text == "")
                        {
                            lblOrgPassword.Text = "Password is required";
                            isValid = false;
                        }
                        if (orgConfirmPassword.Text == "")
                        {
                            lblOrgConfirmPassword.Text = "Confirm Password is required";
                            isValid = false;
                        }
                        if (orgRegion.SelectedValue == "")
                        {
                            lblOrgRegion.Text = "Organization Region is required";
                            isValid = false;
                        }
                        if (!orgLicense.HasFile)
                        {
                            lblOrgLicense.Text = "Organization License is required";
                            isValid = false;
                        }
                        if (orgPassword.Text != orgConfirmPassword.Text)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Password and Confirm Password do not match!');", true);
                            isValid = false;
                        }

                        string[] allowedExtensionsOrg = { ".jpg", ".jpeg", ".png", ".pdf" };
                        bool isValidBusinessLicense = allowedExtensionsOrg.Contains(Path.GetExtension(orgLicense.FileName).ToLower());
                        if (!isValidBusinessLicense)
                        {
                            lblImgTypeOrgLicense.Text = "Invalid file type. Accepted formats: .jpg, .jpeg, .png, .pdf";
                            isValid = false;
                        }
                        break;

                    case "rider":
                        if (riderName.Text == "")
                        {
                            lblRiderName.Text = "Name is required";
                            isValid = false;
                        }
                        if (riderUsername.Text == "")
                        {
                            lblRiderUsername.Text = "Username is required";
                            isValid = false;
                        }
                        if (riderEmail.Text == "")
                        {
                            lblRiderEmail.Text = "Email is required";
                            isValid = false;
                        }
                        if (riderContactNumber.Text == "")
                        {
                            lblRiderContactNumber.Text = "Contact Number is required";
                            isValid = false;
                        }
                        if (vehiclePlateNo.Text == "")
                        {
                            lblVehiclePlateNo.Text = "Vehicle Plate Number is required";
                            isValid = false;
                        }
                        if (vehicleType.SelectedValue == "")
                        {
                            lblVehicleType.Text = "Vehicle Type is required";
                            isValid = false;
                        }
                        if (riderPassword.Text == "")
                        {
                            lblRiderPassword.Text = "Password is required";
                            isValid = false;
                        }
                        if (riderConfirmPassword.Text == "")
                        {
                            lblRiderConfirmPassword.Text = "Confirm Password is required";
                            isValid = false;
                        }
                        if (!riderCarLicense.HasFile)
                        {
                            lblRiderCarLicense.Text = "Driving License is required";
                            isValid = false;
                        }
                        if (!riderFacePhoto.HasFile)
                        {
                            lblRiderFacePhoto.Text = "Face Photo is required";
                            isValid = false;
                        }
                        if (riderPassword.Text != riderConfirmPassword.Text)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Password and Confirm Password do not match!');", true);
                            isValid = false;
                        }

                        string[] allowedExtensionsRider = { ".jpg", ".jpeg", ".png" };
                        bool isValidCarLicense = allowedExtensionsRider.Contains(Path.GetExtension(riderCarLicense.FileName).ToLower());
                        bool isValidFacePhoto = allowedExtensionsRider.Contains(Path.GetExtension(riderFacePhoto.FileName).ToLower());

                        if (!isValidCarLicense || !isValidFacePhoto)
                        {
                            lblImgTypeCarLicense.Text = "Invalid file type. Accepted formats: .jpg, .jpeg, .png, .pdf";
                            lblImgTypeFacePhoto.Text = "Invalid file type. Accepted formats: .jpg, .jpeg, .png, .pdf";
                            isValid = false;
                        }
                        break;
                }

                if (isValid)
                {
                    // Redirect to OTP verification page
                    string email = "";
                    string username = "";

                    switch (role)
                    {
                        case "donor":
                            email = donorEmail.Text;
                            username = donorUsername.Text;
                            break;
                        case "organization":
                            email = orgEmail.Text;
                            username = picName.Text; // or orgUsername.Text if available
                            break;
                        case "rider":
                            email = riderEmail.Text;
                            username = riderUsername.Text;
                            break;
                    }

                    Response.Redirect($"SignUpOTP.aspx?email={email}&username={username}");

                    RegisterUser(role);

                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Registered Successful!');", true);
                }
            }
            catch (Exception ex)
            {
                // log the exception and display a user-friendly error message
                Response.Write("An error occurred: " + ex.Message);
            }
        }
    
            


        private void RegisterUser(string role)
        {
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
                        // Handle unknown role
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please select a role!');", true);
                        break;
                }
            }
        }


        private void RegisterDonor(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                // Hash the password before including it in the query
                string hashedPassword = HashPassword(donorPassword.Text);

                string donor_sql = "EXEC [create_donor] " +
                                   "@method = 'INSERT', " +
                                   "@donorId = NULL, " +  // donorId will be generated in the stored procedure
                                   "@donorName = '" + donorName.Text + "', " +
                                   "@donorUsername = '" + donorUsername.Text + "', " +
                                   "@donorEmail = '" + donorEmail.Text + "', " +
                                   "@donorContactNumber = '" + donorContactNumber.Text + "', " +
                                   "@donorHashPassword = '" + hashedPassword + "', " +
                                   "@donorNewHashPassword = NULL, " +
                                   "@donorAddress1 = NULL," +
                                   "@donorAddress2 = NULL, " +
                                   "@donorProfilePicBase64 = NULL ";

                
                DataTable dt_check = NewQry.GetData(donor_sql);
                string message = "";

                if (dt_check.Rows.Count > 0)
                {
                    message = dt_check.Rows[0]["MESSAGE"].ToString();
                    if (message != "" && message != "SUCCESSFUL! You have registered as a donor!")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>ErrorMsg('" + message + "');</script>");
                    }
                    else if (message != "" && message == "SUCCESSFUL! You have registered as a donor!")
                    {
                        Session["message"] = message;
                        
                    }
                }
                
                clearText();
                
            }
            catch (Exception ex)
            {
                // Log other exceptions or display specific error messages
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('An error occurred: " + ex.Message + "');", true);
            }
        }

        private void RegisterOrganization(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                // hash the password before including it in the query
                string hashedPassword = HashPassword(orgPassword.Text);

                // encrypt the image as base64

                string businessLicense = ConvertToBase64(orgLicense.PostedFile);
                
                // initial status after registration
                string status = "Pending Approval";

                string org_sql = "EXEC [create_organization] " +
                                   "@method = 'INSERT', " +
                                   "@orgId = NULL, " +  // orgId will be generated in the stored procedure
                                   "@orgName = '" + orgName.Text + "', " +
                                   "@orgEmail = '" + orgEmail.Text + "', " +
                                   "@orgContactNumber = '" + orgContactNumber.Text + "', " +
                                   "@orgHashPassword = '" + hashedPassword + "', " +
                                   "@orgNewHashPassword = NULL, " +
                                   "@orgAddress = '" + orgAddress.Text + "', " +
                                   "@businessLicenseImageBase64 ='" + businessLicense + "', " +
                                   "@picName = '" + picName.Text + "', " +
                                   "@picEmail = '" + picEmail.Text + "', " +
                                   "@picContactNumber = '" + picNumber.Text + "', " +
                                   "@orgProfilePicBase64 = NULL, " +
                                   "@orgDescription= NULL, " +
                                   "@mostNeededItemCategory = NULL, " +
                                   "@orgRegion = '" + orgRegion.SelectedValue + "', " +
                                   "@orgStatus = '" + status + "', " +
                                   "@adminId = NULL ";


                DataTable dt_check = NewQry.GetData(org_sql);
                string message = "";

                if (dt_check.Rows.Count > 0)
                {
                    message = dt_check.Rows[0]["MESSAGE"].ToString();
                    if (message != "" && message != "SUCCESSFUL! You have registered as an organization!")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>ErrorMsg('" + message + "');</script>");
                    }
                    else if (message != "" && message == "SUCCESSFUL! You have registered as an organization!")
                    {
                        Session["message"] = message;
                        
                    }
                }
               
                clearText();
            }
            catch (Exception ex)
            {
                // Log other exceptions or display specific error messages
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('An error occurred: " + ex.Message + "');", true);
            }
        }

        private void RegisterRider(SqlConnection con)
        {
            try
            {
                QRY NewQry = new QRY();

                // Hash the password before including it in the query
                string hashedPassword = HashPassword(riderPassword.Text);

                // encrypt the image as base64

                string drivingLicense = ConvertToBase64(riderCarLicense.PostedFile);

                string facePic = ConvertToBase64(riderFacePhoto.PostedFile);

                // initial status after registration
                string status = "Pending Approval";

                string rider_sql = "EXEC [create_rider] " +
                                   "@method = 'INSERT', " +
                                   "@riderId = NULL, " +  // riderId will be generated in the stored procedure
                                   "@riderUsername = '" + riderName.Text + "', " +
                                   "@riderFullName = '" + riderUsername.Text + "', " +
                                   "@riderEmail = '" + riderEmail.Text + "', " +
                                   "@riderContactNumber = '" + riderContactNumber.Text + "', " +
                                   "@riderHashPassword = '" + hashedPassword + "', " +
                                   "@riderNewHashPassword = NULL, " +
                                   "@drivingLicenseImageBase64 = '" + drivingLicense + "', " +
                                   "@vehicleType = '" + vehicleType.SelectedValue + "', " +
                                   "@riderFacePicBase64 = '" + facePic + "', " +
                                   "@noOfDeliveryMade = NULL," +
                                   "@walletAmount = NULL," +
                                   "@vehiclePlateNumber = '" + vehiclePlateNo.Text + "', " +
                                   "@registerDate = NULL," +
                                   "@riderStatus = '" + status + "', " +
                                   "@adminId = NULL";


                DataTable dt_check = NewQry.GetData(rider_sql);
                string message = "";

                if (dt_check.Rows.Count > 0)
                {
                    message = dt_check.Rows[0]["MESSAGE"].ToString();
                    if (message != "" && message != "SUCCESSFUL! You have registered as a delivery rider!")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PageUp", @"<script type='text/javascript'>ErrorMsg('" + message + "');</script>");
                    }
                    else if (message != "" && message == "SUCCESSFUL! You have registered as a delivery rider!")
                    {
                        Session["message"] = message;
                        
                    }
                }
               
                clearText();
            }
            catch (Exception ex)
            {
                // Log other exceptions or display specific error messages
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

        private string ConvertToBase64(HttpPostedFile postedFile)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                postedFile.InputStream.CopyTo(ms);
                byte[] bytes = ms.ToArray();
                return Convert.ToBase64String(bytes);
            }
        }

        public void clearText()
        {
            donorName.Text = "";
            donorUsername.Text = "";
            donorEmail.Text = "";
            donorContactNumber.Text = "";
            donorPassword.Text = "";
            donorConfirmPassword.Text = "";
            orgName.Text = "";
            orgEmail.Text = "";
            orgContactNumber.Text = "";
            orgAddress.Text = "";
            orgRegion.Text = "";
            picName.Text = "";
            picEmail.Text = "";
            picNumber.Text = "";
            orgPassword.Text = "";
            orgConfirmPassword.Text = "";
            riderName.Text = "";
            riderUsername.Text = "";
            riderEmail.Text = "";
            riderContactNumber.Text = "";
            riderPassword.Text = "";
            riderConfirmPassword.Text = "";
            vehicleType.Text = "";
            vehiclePlateNo.Text = "";

        }

    }

}
