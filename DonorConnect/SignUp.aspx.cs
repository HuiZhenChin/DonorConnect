﻿using System;
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
using System.Text.RegularExpressions;
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
                
            }
           
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string role = selectedRole.Value;

                if (string.IsNullOrEmpty(role))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire({ title: 'Error', text: 'Please select a role!', icon: 'warning', confirmButtonText: 'OK' });", true);
                    return;
                }

                bool isValid = true;

                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

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
                        else if (!Regex.IsMatch(donorPassword.Text, passwordPattern))
                        {
                            lblDonorPassword.Text = "Password must be at least 8 characters, include uppercase, lowercase, number, and special character.";
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
                        else if (!Regex.IsMatch(orgPassword.Text, passwordPattern))
                        {
                            lblOrgPassword.Text = "Password must be at least 8 characters, include uppercase, lowercase, number, and special character.";
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

                            if (orgLicense.HasFiles)
                            {
                                string base64BusinessLicense = ImageFileProcessing.ConvertToBase64(orgLicense.PostedFiles);
                                Session["OrgLicense"] = base64BusinessLicense;
                            }


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
                        if (riderRegion.SelectedValue == "")
                        {
                            lblRiderRegion.Text = "Preferred Region is required";
                            isValid = false;
                        }
                        if (riderPassword.Text == "")
                        {
                            lblRiderPassword.Text = "Password is required";
                            isValid = false;
                        }
                        else if (!Regex.IsMatch(riderPassword.Text, passwordPattern))
                        {
                            lblRiderPassword.Text = "Password must be at least 8 characters, include uppercase, lowercase, number, and special character.";
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
                            Session["RiderRegion"] = riderRegion.SelectedValue;  
                            Session["VehicleType"] = vehicleType.SelectedValue;
                            Session["RiderPassword"] = riderPassword.Text;

                            if (riderCarLicense.HasFiles)
                            {
                                string base64DrvingLicense = ImageFileProcessing.ConvertToBase64(riderCarLicense.PostedFiles);
                                Session["RiderCarLicense"] = base64DrvingLicense;

                            } 
                            
                            if (riderFacePhoto.HasFiles)
                            {
                                string base64FacePic = ImageFileProcessing.ConvertToBase64(riderFacePhoto.PostedFiles);
                                Session["RiderFacePhoto"] = base64FacePic;
                            }
                            

                        }
                        break;

                }

                if (isValid)
                {
                    // redirect to OTP verification page
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

                    ScriptManager.RegisterStartupScript(this, GetType(), "showConfirmationModal",
                    $"showConfirmationModal('{email}', '{username}', '{role}');", true);
                    //Response.Redirect($"SignUpOTP.aspx?email={email}&username={username}&selectedRole={role}");
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occurred: " + ex.Message);
            }
        }


        

    }

}
