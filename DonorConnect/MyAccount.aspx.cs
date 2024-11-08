using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace DonorConnect
{
    public partial class MyAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    string username = Session["username"].ToString();

                    string role = GetUserRoleFromDatabase(username);

                    if (role == "donor")
                    {
                        ShowDonorInfo(username);
                        donorContent.Visible = true;
                        orgContent.Visible = false;
                        adminContent.Visible = false;
                        selectedRole.Text = "donor";
                        profileUsername.Text = username;
                    }
                    else if (role == "organization")
                    {
                        ShowOrgInfo(username);
                        donorContent.Visible = false;
                        orgContent.Visible = true;
                        adminContent.Visible = false;
                        selectedRole.Text = "organization";
                        profileUsername.Text = username;
                    }
                    else if (role == "rider")
                    {
                        ShowRiderInfo(username);
                        donorContent.Visible = false;
                        orgContent.Visible = false;
                        riderContent.Visible = true;
                        adminContent.Visible = false;
                        selectedRole.Text = "rider";
                        profileUsername.Text = username;
                        // not allow rider to chg profile pic, will display their uploaded face photo during registration
                        profilePic.Style.Add("pointer-events", "none");
                        profilePic.Style.Add("opacity", "1.0"); 
                        fileUpload.Enabled = false;
                    }
                    else if (role == "admin")
                    {
                        btnPreview.Visible = false;
                        ShowAdminInfo(username);
                        donorContent.Visible = false;
                        orgContent.Visible = false;
                        riderContent.Visible = false;
                        adminContent.Visible = true;
                        selectedRole.Text = "admin";
                        profileUsername.Text = username;
                        // not allow admin to chg profile pic, will display DonorConnect logo
                        profilePic.Style.Add("pointer-events", "none");
                        profilePic.Style.Add("opacity", "1.0");
                        fileUpload.Enabled = false;
                    }
                    else
                    {
                        donorContent.Visible = false;
                        orgContent.Visible = false;
                        riderContent.Visible = false;
                        selectedRole.Text = " ";
                        profileUsername.Text = " ";
                    }

                    // load current profile picture from database
                    LoadProfilePicture();
                }

              
            }
        }


        private string GetUserRoleFromDatabase(string username)
        {
            string sql;
            string role = "";
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [user] WHERE username = '" + username + "' ";

            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                role = _dt.Rows[0]["role"].ToString();

                Session["role"] = role;
            }

            return role;
        }

        private void ShowDonorInfo(string username)
        {
            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [donor] WHERE donorUsername = '" + username + "' ";
            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                txtUsername.Text = row["donorUsername"].ToString();
                txtFullName.Text = row["donorName"].ToString();
                txtEmail.Text = row["donorEmail"].ToString();
                txtPhone.Text = row["donorContactNumber"].ToString();
                txtAddress.Text = row["donorAddress1"].ToString();
            }
        }

        private void ShowOrgInfo(string username)
        {
            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";
            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                txtOrgName.Text = row["orgName"].ToString();
                txtOrgEmail.Text = row["orgEmail"].ToString();
                txtOrgNumber.Text = row["orgContactNumber"].ToString();
                txtOrgAddress.Text = row["orgAddress"].ToString();
                txtPicName.Text = row["picName"].ToString();
                txtPicEmail.Text = row["picEmail"].ToString();
                txtPicNumber.Text = row["picContactNumber"].ToString();
                orgRegion.SelectedValue = row["orgRegion"].ToString();
                txtDesc.Text = row["orgDescription"].ToString();
                txtCategory.Text = row["mostNeededItemCategory"].ToString();


            }
        }

        private void ShowRiderInfo(string username)
        {
            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [delivery_rider] WHERE riderUsername = '" + username + "' ";
            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                txtRiderUsername.Text = row["riderUsername"].ToString();
                txtRiderFullName.Text = row["riderFullName"].ToString();
                txtRiderEmail.Text = row["riderEmail"].ToString();
                txtRiderNumber.Text = row["riderContactNumber"].ToString();
                vehicleType.SelectedValue = row["vehicleType"].ToString();
                txtPlateNo.Text = row["vehiclePlateNumber"].ToString();


            }
        }

        private void ShowAdminInfo(string username)
        {
            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [admin] WHERE adminUsername = '" + username + "' ";
            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                txtAdminUsername.Text = row["adminUsername"].ToString();
                txtAdminEmail.Text = row["adminEmail"].ToString();
         
            }
        }

        protected void btnSaveDonorInfo_Click(object sender, EventArgs e)
        {
            UpdateDonorInfo();
        }

        protected void btnSaveOrgInfo_Click(object sender, EventArgs e)
        {
            UpdateOrgInfo();
        }

        protected void btnSaveRiderInfo_Click(object sender, EventArgs e)
        {
            UpdateRiderInfo();
        }

        protected void btnSaveAdminInfo_Click(object sender, EventArgs e)
        {
            UpdateAdminInfo();
        }

        private void UpdateDonorInfo()
        {
            string username = Session["username"].ToString();

            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
                
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('All fields must be filled out.');", true);
                return;
            }
            string sql = "UPDATE [donor] SET " +
                  "donorName = '" + txtFullName.Text + "', " +
                   "donorEmail = '" + txtEmail.Text + "', " +
                  "donorContactNumber = '" + txtPhone.Text + "', " +
                  "donorAddress1 = '" + txtAddress.Text + "' " +
                  "WHERE donorUsername = '" + username + "'";

            string sql2 = "UPDATE [user] SET " +
                   "email = '" + txtEmail.Text + "' " +
                  "WHERE username = '" + username + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);
            bool success2 = _Qry.ExecuteNonQuery(sql2);

            if (success || success2)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('User information updated successfully!',);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating user information. Please try again!');", true);
            }
        }

        private void UpdateOrgInfo()
        {
            string username = Session["username"].ToString();

            if (string.IsNullOrWhiteSpace(txtOrgEmail.Text) ||
                string.IsNullOrWhiteSpace(txtOrgNumber.Text) ||
                string.IsNullOrWhiteSpace(txtOrgAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPicName.Text) ||
                string.IsNullOrWhiteSpace(txtPicEmail.Text) || 
                string.IsNullOrWhiteSpace(txtPicNumber.Text) ||
                string.IsNullOrWhiteSpace(orgRegion.SelectedValue))
                //string.IsNullOrWhiteSpace(txtDesc.Text) ||
                //string.IsNullOrWhiteSpace(txtCategory.Text))
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('All fields must be filled out.');", true);
                return;
            }

            string sql = "UPDATE [organization] SET " +
                         "orgEmail = '" + txtOrgEmail.Text + "', " +
                         "orgContactNumber = '" + txtOrgNumber.Text + "', " +
                         "orgAddress = '" + txtOrgAddress.Text + "', " +
                         "picName = '" + txtPicName.Text + "', " +
                         "picEmail = '" + txtPicEmail.Text + "', " +
                         "picContactNumber = '" + txtPicNumber.Text + "', " +
                         "orgRegion = '" + orgRegion.SelectedValue + "', " +
                         "orgDescription = '" + txtDesc.Text + "', " +
                         "mostNeededItemCategory = '" + txtCategory.Text + "' " +
                         "WHERE orgName = '" + username + "'";

            string sql2 = "UPDATE [user] SET " +
                   "email = '" + txtOrgEmail.Text + "', " +
                  "WHERE username = '" + username + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);

            bool success2 = _Qry.ExecuteNonQuery(sql2);

            if (success || success2)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('User information updated successfully!',);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating user information. Please try again!');", true);
            }
        }

        private void UpdateRiderInfo()
        {
            string username = Session["username"].ToString();

            if (string.IsNullOrWhiteSpace(txtRiderFullName.Text) ||
                string.IsNullOrWhiteSpace(txtRiderEmail.Text) ||
                string.IsNullOrWhiteSpace(txtRiderNumber.Text) ||
                string.IsNullOrWhiteSpace(vehicleType.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtPlateNo.Text))
            {
               
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('All fields must be filled out.');", true);
                return; 
            }

            string sql = "UPDATE [delivery_rider] SET " +
                         "riderFullName = '" + txtRiderFullName.Text + "', " +
                         "riderEmail = '" + txtRiderEmail.Text + "', " +
                         "riderContactNumber = '" + txtRiderNumber.Text + "', " +
                         "vehicleType = '" + vehicleType.SelectedValue + "', " +
                         "vehiclePlateNumber = '" + txtPlateNo.Text + "' " +
                         "WHERE riderUsername = '" + username + "'";

            string sql2 = "UPDATE [user] SET " +
                   "email = '" + txtRiderEmail.Text + "', " +
                  "WHERE username = '" + username + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);
            bool success2 = _Qry.ExecuteNonQuery(sql2);

            if (success || success2)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('User information updated successfully!',);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating user information. Please try again!');", true);
            }
        }

        private void UpdateAdminInfo()
        {
            string username = Session["username"].ToString();

            if (string.IsNullOrWhiteSpace(txtAdminEmail.Text))
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('All fields must be filled out.');", true);
                return;
            }

            string sql = "UPDATE [admin] SET " +
                         "adminEmail = '" + txtAdminEmail.Text + "' " +
                         "WHERE adminUsername = '" + username + "'";

            string sql2 = "UPDATE [user] SET " +
                   "email = '" + txtAdminEmail.Text + "', " +
                  "WHERE username = '" + username + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);
            bool success2 = _Qry.ExecuteNonQuery(sql2);

            if (success || success2)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('User information updated successfully!',);", true);
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating user information. Please try again!');", true);
            }
        }

        private void LoadProfilePicture()
        {
            string base64String = GetProfilePictureFromDb();

            if (!string.IsNullOrEmpty(base64String))
            {
                string imageFormat = "jpeg"; 
                if (base64String.StartsWith("/9j/")) 
                {
                    imageFormat = "jpeg";
                }
                else if (base64String.StartsWith("iVBORw0KGgo")) 
                {
                    imageFormat = "png";
                }

                output.ImageUrl = $"data:image/{imageFormat};base64," + base64String;
            }
            else
            {

                output.ImageUrl = "/Image/default_picture.jpg";
            }
        }



        private string GetProfilePictureFromDb()
        {
            string username = Session["username"].ToString();
            string role = GetUserRoleFromDatabase(username);

            string sql, sql2, sql3;
            QRY _Qry = new QRY();
            DataTable _dt, _dt2, _dt3;
            
            if (role == "donor")
            {
                sql = "SELECT donorProfilePicBase64 FROM [donor] WHERE donorUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    return _dt.Rows[0]["donorProfilePicBase64"].ToString();
                }

            }
            else if (role == "organization")
            {
                sql2 = "SELECT orgProfilePicBase64 FROM [organization] WHERE orgName = '" + username + "' ";
                _dt2 = _Qry.GetData(sql2);

                if (_dt2.Rows.Count > 0)
                {
                    return _dt2.Rows[0]["orgProfilePicBase64"].ToString();
                }

            }
            else if (role == "rider")
            {
                sql3 = "SELECT riderFacePicBase64 FROM [delivery_rider] WHERE riderUsername = '" + username + "' ";
                _dt3 = _Qry.GetData(sql3);

                if (_dt3.Rows.Count > 0)
                {
                    string encryptedImage = _dt3.Rows[0]["riderFacePicBase64"].ToString();

                    string[] parts = encryptedImage.Split(new char[] { ':' }, 2);
                    string filename = parts[0]; 

                    string decryptedImage = ImageFileProcessing.DecryptImages(parts[1]);

                    return decryptedImage;
                }
            }

            else if (role == "admin")
            {

                return ConvertProfilePicToBase64("/Image/logo.jpg");
            }

            return null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (fileUpload.HasFile)
            {
                using (Stream fs = fileUpload.PostedFile.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytes = br.ReadBytes((int)fs.Length);
                        string base64String = Convert.ToBase64String(bytes);

                        // save the base64 string to the database
                        SaveProfilePictureToDb(base64String);

                        // update the ImageUrl to display the new profile picture
                        output.ImageUrl = "data:image/jpeg;base64," + base64String;
                    }
                }
            }
            // hide buttons after saving
            buttons.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // reload the profile picture from the database to cancel the changes
            LoadProfilePicture();
            // hide buttons after changes discarded
            buttons.Visible = false;
        }

        private void SaveProfilePictureToDb(string base64String)
        {
            string username = Session["username"].ToString();
            string role = GetUserRoleFromDatabase(username);
            string sql;
            QRY _Qry = new QRY();

            try
            {

                if (role == "donor")
                {
                    sql = "UPDATE [donor] SET " +
                     "donorProfilePicBase64 = '" + base64String + "' " +
                    "WHERE donorUsername = '" + username + "'";
                    _Qry.ExecuteNonQuery(sql);

                    bool success = _Qry.ExecuteNonQuery(sql);

                    if (success)
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Profile picture updated successfully!',);", true);
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating profile picture. Please try again!');", true);
                    }
                }
                else if (role == "organization")
                {
                    sql = "UPDATE [organization] SET " +
                     "orgProfilePicBase64 = '" + base64String + "' " +
                    "WHERE orgName = '" + username + "'";
                    _Qry.ExecuteNonQuery(sql);

                    bool success = _Qry.ExecuteNonQuery(sql);

                    if (success)
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Profile picture updated successfully!',);", true);
                    }
                    else
                    {

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error updating profile picture. Please try again!');", true);
                    }
                }
                
            }

            catch (Exception ex)
            {
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showError('Error updating profile picture: {ex.Message}');", true);
            }
        }

        private string ConvertProfilePicToBase64(string imagePath)
        {
            
            byte[] imageBytes = System.IO.File.ReadAllBytes(Server.MapPath(imagePath));
            return Convert.ToBase64String(imageBytes);
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                string username = Session["username"].ToString();

                string role = GetUserRoleFromDatabase(username);

                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

                // validate that the password meets the requirements
                if (!Regex.IsMatch(txtPassword.Text, passwordPattern))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                        "showError('Password must be at least 8 characters long and include uppercase, lowercase, a number, and a special character.');", true);
                    return;
                }


                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Password does not match!');", true);
                    return;
                }

                else if (txtPassword.Text == txtConfirmPassword.Text)
                {

                    string sql;
                    QRY _Qry = new QRY();

                    string hashedPassword = HashPassword(txtPassword.Text);

                    if (role == "donor")
                    {
                        sql = "UPDATE [donor] SET " +
                         "donorHashPassword = '" + hashedPassword + "' " +
                        "WHERE donorUsername = '" + username + "'";

                        _Qry.ExecuteNonQuery(sql);

                        bool success = _Qry.ExecuteNonQuery(sql);

                        if (success)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Password changed successfully!',);", true);
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error changing password. Please try again!');", true);
                        }

                    }
                    else if (role == "organization")
                    {
                        sql = "UPDATE [organization] SET " +
                          "orgHashPassword = '" + hashedPassword + "' " +
                         "WHERE orgName = '" + username + "'";

                        _Qry.ExecuteNonQuery(sql);

                        bool success = _Qry.ExecuteNonQuery(sql);

                        if (success)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Password changed successfully!',);", true);
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error changing password. Please try again!');", true);
                        }
                    }
                    else if (role == "rider")
                    {
                        sql = "UPDATE [delivery_rider] SET " +
                          "riderHashPassword = '" + hashedPassword + "' " +
                         "WHERE riderUsername = '" + username + "'";

                        _Qry.ExecuteNonQuery(sql);

                        bool success = _Qry.ExecuteNonQuery(sql);

                        if (success)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Password changed successfully!',);", true);
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error changing password. Please try again!');", true);
                        }

                    }
                    else if (role == "admin")
                    {
                        sql = "UPDATE [admin] SET " +
                           "adminHashPassword = '" + hashedPassword + "' " +
                          "WHERE adminUsername = '" + username + "'";

                        _Qry.ExecuteNonQuery(sql);

                        bool success = _Qry.ExecuteNonQuery(sql);

                        if (success)
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Password changed successfully!',);", true);
                        }
                        else
                        {

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error changing password. Please try again!');", true);
                        }
                    }
                }
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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            QRY _Qry= new QRY();

            string username = Session["username"].ToString();

            string role = Session["role"].ToString();

            if (username != null && role != null)
            {
                string query = username;

                if (role.Equals("rider", StringComparison.OrdinalIgnoreCase))
                {

                    string sql = "SELECT riderFullName FROM [delivery_rider] WHERE riderUsername = @username";
                    var parameters = new Dictionary<string, object>
                    {
                        { "@username", username}
                    };
                    var _dt = _Qry.GetData(sql, parameters);

                    if (_dt.Rows.Count > 0)
                    {
                        query = _dt.Rows[0]["riderFullName"].ToString();
                    }
                }

                Response.Redirect($"PreviewPublicInfo.aspx?role={role}&username={query}");
            }
        }


    }
}
