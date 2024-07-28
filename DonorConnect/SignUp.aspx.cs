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
using WebGrease.Activities;

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

                // validation based on role and set session variables to enter otp page
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Password and Confirm Password do not match!', 'warning');", true);
                            isValid = false;
                        }

                        string donorSQL;
                        QRY _Qry = new QRY();
                        DataTable _dt;
                        donorSQL = "SELECT * FROM [donor] WHERE donorUsername = '" + donorUsername.Text + "' ";

                        _dt = _Qry.GetData(donorSQL);

                        if (_dt.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Donor with the same username already exists', 'warning');", true);
                            isValid = false;
                        }

                        if (isValid)
                        {
                            Session["Role"] = "donor";
                            Session["DonorName"] = donorName.Text;
                            Session["DonorUsername"] = donorUsername.Text;
                            Session["DonorEmail"] = donorEmail.Text;
                            Session["DonorContactNumber"] = donorContactNumber.Text;
                            Session["DonorPassword"] = donorPassword.Text;
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
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Password and Confirm Password do not match!', 'warning');", true);
                            isValid = false;
                        }

                        string[] allowedExtensionsOrg = { ".jpg", ".jpeg", ".png", ".pdf" };
                        bool isValidBusinessLicense = allowedExtensionsOrg.Contains(Path.GetExtension(orgLicense.FileName).ToLower());
                        if (!isValidBusinessLicense)
                        {
                            lblImgTypeOrgLicense.Text = "Invalid file type. Accepted formats: .jpg, .jpeg, .png, .pdf";
                            isValid = false;
                        }
                        string orgSQL;
                        QRY _Qry1 = new QRY();
                        DataTable _dt1;
                        orgSQL = "SELECT * FROM [organization] WHERE orgName = '" + orgName.Text + "' ";

                        _dt1 = _Qry1.GetData(orgSQL);

                        if (_dt1.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Organization already exists', 'warning');", true);
                            isValid = false;
                        }
                        if (isValid)
                        {
                            Session["Role"] = "organization";
                            Session["OrgName"] = orgName.Text;
                            Session["OrgEmail"] = orgEmail.Text;
                            Session["OrgContactNumber"] = orgContactNumber.Text;
                            Session["OrgAddress"] = orgAddress.Text;
                            Session["PicName"] = picName.Text;
                            Session["PicEmail"] = picEmail.Text;
                            Session["PicNumber"] = picNumber.Text;
                            Session["OrgPassword"] = orgPassword.Text;
                            Session["OrgRegion"] = orgRegion.SelectedValue;

                            string base64BusinessLicense = ConvertToBase64(orgLicense.PostedFile);
                            Session["OrgLicense"] = base64BusinessLicense;

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
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Password and Confirm Password do not match!', 'warning');", true);
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
                        string riderSQL;
                        QRY _Qry2 = new QRY();
                        DataTable _dt2;
                        riderSQL = "SELECT * FROM [delivery_rider] WHERE riderFullName = '" + riderName.Text + "' AND riderEmail = '" + riderEmail.Text + "' ";

                        _dt2 = _Qry2.GetData(riderSQL);

                        if (_dt2.Rows.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Rider already exists', 'warning');", true);
                            isValid = false;
                        }
                        if (isValid)
                        {
                            Session["Role"] = "rider";
                            Session["RiderName"] = riderName.Text;
                            Session["RiderUsername"] = riderUsername.Text;
                            Session["RiderEmail"] = riderEmail.Text;
                            Session["RiderContactNumber"] = riderContactNumber.Text;
                            Session["VehiclePlateNo"] = vehiclePlateNo.Text;
                            Session["VehicleType"] = vehicleType.SelectedValue;
                            Session["RiderPassword"] = riderPassword.Text;

                            // Save the files in session (e.g., as byte arrays)
                            string base64DrvingLicense = ConvertToBase64(riderCarLicense.PostedFile);
                            Session["RiderCarLicense"] = base64DrvingLicense;

                            string base64FacePic = ConvertToBase64(riderFacePhoto.PostedFile);
                            Session["RiderFacePhoto"] = base64FacePic;

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
                            username = orgName.Text;

                            break;
                        case "rider":
                            email = riderEmail.Text;
                            username = riderUsername.Text;

                            break;
                    }

                    Response.Redirect($"SignUpOTP.aspx?email={email}&username={username}&selectedRole={role}");
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred: " + ex.Message);
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


        //public void clearText()
        //{
        //    donorName.Text = "";
        //    donorUsername.Text = "";
        //    donorEmail.Text = "";
        //    donorContactNumber.Text = "";
        //    donorPassword.Text = "";
        //    donorConfirmPassword.Text = "";
        //    orgName.Text = "";
        //    orgEmail.Text = "";
        //    orgContactNumber.Text = "";
        //    orgAddress.Text = "";
        //    orgRegion.Text = "";
        //    picName.Text = "";
        //    picEmail.Text = "";
        //    picNumber.Text = "";
        //    orgPassword.Text = "";
        //    orgConfirmPassword.Text = "";
        //    riderName.Text = "";
        //    riderUsername.Text = "";
        //    riderEmail.Text = "";
        //    riderContactNumber.Text = "";
        //    riderPassword.Text = "";
        //    riderConfirmPassword.Text = "";
        //    vehicleType.Text = "";
        //    vehiclePlateNo.Text = "";

        //}

    }

}
