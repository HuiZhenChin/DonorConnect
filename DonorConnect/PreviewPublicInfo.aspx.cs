using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class PreviewPublicInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string role = Request.QueryString["role"];

                string username = Request.QueryString["username"];

                string orgName = Request.QueryString["orgName"];

                if (role == "organization")
                {
                    if(username != null)
                    {
                        ShowOrgInfo(username);
                    }

                    else if (orgName != null)
                    {
                        ShowOrgInfo(orgName);
                    }
                    
                }

                else if (role == "rider")
                {
                   // string fullName = GetRiderName(username);
                    ShowRiderInfo(username);
                }

                else if (role == "donor")
                {
                    ShowDonorInfo(username);
                }

               LoadProfilePicture();
            }
        }

        private void ShowOrgInfo(string username)
        {
            string sql, sql2;
            QRY _Qry = new QRY();
            QRY _Qry2 = new QRY();
            DataTable _dt;
            sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";
            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                profileUsername.Text = row["orgName"].ToString();
                lblEmail.Text = row["orgEmail"].ToString();
                lblPhoneNo.Text= row["orgContactNumber"].ToString();
                lblPicName.Text = row["picName"].ToString();
                lblPicEmail.Text = row["picEmail"].ToString();
                lblPicNo.Text = row["picContactNumber"].ToString();
                lblAddress.Text = row["orgAddress"].ToString();
                lblDescription.Text = row["orgDescription"].ToString();
                lblRegion.Text = row["orgRegion"].ToString();
                lblStatus.Text = row["orgStatus"].ToString();
                if (lblStatus.Text == "Active")
                {
                    lblStatus.CssClass = "badgeNew badge-active";
                }
                else if (lblStatus.Text == "Terminated")
                {
                    lblStatus.CssClass = "badgeNew badge-terminated";
                }

                picName.Style["display"] = "block";
                picEmail.Style["display"]="block";
                picNo.Style["display"] = "block";
                address.Style["display"]= "block";
                desc.Style["display"] = "block";
                region.Style["display"] = "block";
                pic.Style["display"] = "block";
                other.Style["display"] = "block";


            }

            Organization org = new Organization(username, "", "", "", "");
            string id = org.GetOrgId();

            sql2 = "SELECT donationPublishId, title, itemCategory, urgentStatus, donationState FROM [donation_publish] WHERE orgId = @orgId AND status = 'Opened'";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", id }
            };

            DataTable donations = _Qry.GetData(sql2, parameters);
            donationHeader.Visible = true;
            rptDonations.Visible = true;
            
            rptDonations.DataSource = donations;
            rptDonations.DataBind();

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
                profileUsername.Text = row["donorUsername"].ToString();
                lblEmail.Text = row["donorEmail"].ToString();
                lblPhoneNo.Text = row["donorContactNumber"].ToString();
                lblStatus.Text = row["status"].ToString();
                if (lblStatus.Text == "Active")
                {
                    lblStatus.CssClass = "badgeNew badge-active";
                }
                else if (lblStatus.Text == "Terminated")
                {
                    lblStatus.CssClass = "badgeNew badge-terminated";
                }


                fullName.Style["display"] = "none";
                

            }

           
        }

        private void ShowRiderInfo(string fullname)
        {
            string sql;
            QRY _Qry = new QRY();
           
            DataTable _dt;
            sql = "SELECT * FROM [delivery_rider] WHERE riderFullName = '" + fullname + "' ";
            _dt = _Qry.GetData(sql);

            if (_dt.Rows.Count > 0)
            {
                DataRow row = _dt.Rows[0];
                profileUsername.Text = row["riderUsername"].ToString();
                lblFullName.Text = row["riderFullName"].ToString();
                lblEmail.Text = row["riderEmail"].ToString();
                lblPhoneNo.Text = row["riderContactNumber"].ToString();
                string vehicleTypeLbl = row["vehicleType"].ToString();
                string plateNo = row["vehiclePlateNumber"].ToString();
                lblVehicleType.Text = $"{vehicleTypeLbl} ({plateNo})";
                lblStatus.Text = row["riderStatus"].ToString();
                if (lblStatus.Text == "Active")
                {
                    lblStatus.CssClass = "badgeNew badge-active";
                }
                else if (lblStatus.Text == "Terminated")
                {
                    lblStatus.CssClass = "badgeNew badge-terminated";
                }

                fullName.Style["display"] = "block";
                vehicleType.Style["display"] = "block";

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
            string username = Request.QueryString["username"];

            string role = Request.QueryString["role"];

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
                sql3 = "SELECT riderFacePicBase64 FROM [delivery_rider] WHERE riderFullName = '" + username + "' ";
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

            //else if (role == "admin")
            //{

            //    return ConvertProfilePicToBase64("/Image/logo.jpg");
            //}

            return null;
        }

        private string ConvertProfilePicToBase64(string imagePath)
        {

            byte[] imageBytes = System.IO.File.ReadAllBytes(Server.MapPath(imagePath));
            return Convert.ToBase64String(imageBytes);
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

               
            }

            return role;
        }

        public static string GetRiderName(string username)
        {

            string sql = "SELECT riderFullName FROM delivery_rider WHERE riderUsername = @username";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@username", username }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["riderFullName"].ToString();
            }
            else
            {
                throw new Exception($"No record found");
            }

        }
    }
}