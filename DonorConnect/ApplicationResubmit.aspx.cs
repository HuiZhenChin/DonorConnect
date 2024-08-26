using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class ApplicationResubmit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string riderId = Request.QueryString["riderId"];
                string orgId = Request.QueryString["orgId"];

                
                if (!string.IsNullOrEmpty(riderId))
                {
                    riderDetails.Style["display"] = "block";
                    riderUsername.Text = GetValue("riderUsername", riderId);
                    riderName.Text = GetValue("riderFullName", riderId);
                    riderEmail.Text = GetValue("riderEmail", riderId);
                    riderContactNumber.Text = GetValue("riderContactNumber", riderId);
                    vehicleType.SelectedValue = GetValue("vehicleType", riderId);
                    vehiclePlateNo.Text = GetValue("vehiclePlateNumber", riderId);

                    string drivingLicense = ImageFileProcessing.ProcessImages(GetValue("drivingLicenseImageBase64", riderId));
                    string facePic = ImageFileProcessing.ProcessImages(GetValue("riderFacePicBase64", riderId));
                    imagesContainer.Text = drivingLicense;
                    imagesContainer2.Text = facePic;

                    string rejectedReason = GetValue("rejectedReason", riderId);

                    if (!string.IsNullOrEmpty(rejectedReason))
                    {

                        riderReason.Text = rejectedReason;
                    }
                    else
                    {
                        riderReason.Text = "No reason provided.";
                    }

                }

                if (!string.IsNullOrEmpty(orgId))
                {
                    orgDetails.Style["display"] = "block";
                    orgName.Text = GetValue("orgName", orgId);
                    orgEmail.Text = GetValue("orgEmail", orgId);
                    orgContactNumber.Text = GetValue("orgContactNumber", orgId);
                    orgAddress.Text = GetValue("orgAddress", orgId);
                    picName.Text = GetValue("picName", orgId);
                    picEmail.Text = GetValue("picEmail", orgId);
                    picNumber.Text = GetValue("picContactNumber", orgId);
                    orgRegion.SelectedValue = GetValue("orgRegion", orgId);

                    string businessLicense = ImageFileProcessing.ProcessImages(GetValue("businessLicenseImageBase64", orgId));
                    imagesContainer3.Text = businessLicense;

                    string rejectedReason = GetValue("rejectedReason", orgId);

                    if (!string.IsNullOrEmpty(rejectedReason))
                    {

                        orgReason.Text = rejectedReason;
                    }
                    else
                    {
                        orgReason.Text = "No reason provided.";
                    }
                }
            }
        }

        private string GetValue(string field, string id)
        {
            QRY _Qry = new QRY();
            string sql;


            if (id.StartsWith("r", StringComparison.OrdinalIgnoreCase))
            {
                sql = "SELECT " + field + " FROM delivery_rider WHERE riderId = '" + id.Replace("'", "''") + "'";
            }
            else if (id.StartsWith("o", StringComparison.OrdinalIgnoreCase))
            {
                sql = "SELECT " + field + " FROM organization WHERE orgId = '" + id.Replace("'", "''") + "'";
            }
            else
            {
                throw new ArgumentException("Invalid ID format.");
            }

            return _Qry.GetScalarValue(sql).ToString();
        }


        protected void btnResubmit_Click(object sender, EventArgs e)
        {
            string riderId = Request.QueryString["riderId"];
            
            string orgId = Request.QueryString["orgId"];

            bool isValid = true;

            if (!string.IsNullOrEmpty(riderId))
            {
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
               
                if (riderPassword.Text != riderConfirmPassword.Text)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "ErrorMsg('Password and Confirm Password do not match!', 'warning');", true);
                    isValid = false;
                }

                if (isValid)
                {
                    UpdateRiderData(riderId);
                }
            }

            if (!string.IsNullOrEmpty(orgId))
            {

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
                
                if (orgPassword.Text != orgConfirmPassword.Text)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showError('Password and Confirm Password do not match!', 'warning');", true);
                    isValid = false;
                }

                if (isValid)
                {
                    UpdateOrgData(orgId);
                }
            }


        }

        private void UpdateRiderData(string riderId)
        {
            QRY _Qry = new QRY();

            string status = "Pending Approval";
            string drivingLicenseImage, riderFacePicImage;
            string password = HashPassword(riderPassword.Text);

            // check if new images are uploaded, if not, keep the previous images
            if (riderCarLicense.HasFiles)
            {
                drivingLicenseImage = ImageFileProcessing.ConvertToBase64(riderCarLicense.PostedFiles);
            }
            else
            {
                drivingLicenseImage = GetValue("drivingLicenseImageBase64", riderId);
            }

            if (riderFacePhoto.HasFiles)
            {
                riderFacePicImage = ImageFileProcessing.ConvertToBase64(riderFacePhoto.PostedFiles);
            }
            else
            {
                riderFacePicImage = GetValue("riderFacePicBase64", riderId);
            }
       
            string sql = "UPDATE [delivery_rider] SET " +
                       "riderFullName = '" + riderName.Text + "', " +
                       "riderEmail = '" + riderEmail.Text + "', " +
                       "riderContactNumber = '" + riderContactNumber.Text + "', " +
                       "vehicleType = '" + vehicleType.SelectedValue + "', " +
                       "vehiclePlateNumber = '" + vehiclePlateNo.Text  + "', " +
                       "drivingLicenseImageBase64 = '" + drivingLicenseImage + "', " +
                       "riderFacePicBase64 = '" + riderFacePicImage + "', " +
                       "riderHashPassword = '" + password + "', " +
                       "riderStatus = '" + status + "' " +
                      "WHERE riderId = '" + riderId + "'";

            bool success = _Qry.ExecuteNonQuery(sql);

            if (success)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Form resubmitted successfully!',);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error submitting form. Please try again!');", true);
            }
        }

        private void UpdateOrgData(string orgId)
        {
            QRY _Qry = new QRY();

            string status = "Pending Approval";
            string businessLicenseImage;
            string password = HashPassword(orgPassword.Text);

            // check if new images are uploaded, if not, keep the previous images
            if (orgLicense.HasFiles)
            {
                businessLicenseImage = ImageFileProcessing.ConvertToBase64(orgLicense.PostedFiles);
            }
            else
            {
                businessLicenseImage = GetValue("businessLicenseImageBase64", orgId);
            }


            string sql = "UPDATE [organization] SET " +
                      "orgName = '" + orgName.Text + "', " +
                      "orgEmail = '" + orgEmail.Text + "', " +
                      "orgContactNumber = '" + orgContactNumber.Text + "', " +
                      "orgAddress = '" + orgAddress.Text + "', " +
                      "picName = '" + picName.Text + "', " +
                      "picEmail = '" + picEmail.Text + "', " +
                      "picContactNumber = '" + picNumber.Text + "', " +
                      "orgRegion = '" + orgRegion.SelectedValue + "', " +
                      "businessLicenseImageBase64 = '" + businessLicenseImage + "', " +
                      "orgHashPassword = '" + password + "', " +
                      "orgStatus = '" + status + "' " +
                     "WHERE orgId = '" + orgId + "'";

            bool success = _Qry.ExecuteNonQuery(sql);

            if (success)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Form resubmitted successfully!',);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error submitting form. Please try again!');", true);
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