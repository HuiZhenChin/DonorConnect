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

                if (role == "organization")
                {
                    ShowOrgInfo(username);
                }

                else if (role == "rider")
                {
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
                lblUsername.Text = row["orgName"].ToString();
                profileUsername.Text = row["orgName"].ToString();
                lblEmail.Text = row["orgEmail"].ToString();
                lblPhoneNo.Text= row["orgContactNumber"].ToString();
                lblPicName.Text = row["picName"].ToString();
                lblPicEmail.Text = row["picEmail"].ToString();
                lblPicNo.Text = row["picContactNumber"].ToString();
                lblAddress.Text = row["orgAddress"].ToString();
                lblDescription.Text = row["orgDescription"].ToString();
                lblRegion.Text = row["orgRegion"].ToString();

                picName.Style["display"] = "block";
                picEmail.Style["display"]="block";
                picNo.Style["display"] = "block";
                address.Style["display"]= "block";
                desc.Style["display"] = "block";
                region.Style["display"] = "block";


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
                lblUsername.Text = row["donorUsername"].ToString();
                profileUsername.Text = row["donorUsername"].ToString();
                lblFullName.Text = row["donorName"].ToString();
                lblEmail.Text = row["donorEmail"].ToString();
                lblPhoneNo.Text = row["donorContactNumber"].ToString();

                fullName.Style["display"] = "block";


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
                lblUsername.Text = row["riderUsername"].ToString();
                profileUsername.Text = row["riderUsername"].ToString();
                lblFullName.Text = row["riderFullName"].ToString();
                lblEmail.Text = row["riderEmail"].ToString();
                lblPhoneNo.Text = row["riderContactNumber"].ToString();
                lblVehicleType.Text = row["vehicleType"].ToString();
                lblPlateNo.Text = row["vehiclePlateNumber"].ToString();

                fullName.Style["display"] = "block";
                vehicleType.Style["display"] = "block";
                plateNo.Style["display"] = "block";
            }

        
        }

        private void LoadProfilePicture()
        {
            string base64String = GetProfilePictureFromDb();

            if (!string.IsNullOrEmpty(base64String))
            {
                output.ImageUrl = "data:image/jpeg;base64," + base64String;
            }
            else
            {
                // default image if no profile picture is found
                output.ImageUrl = "/Image/default_picture.jpg";
            }
        }


        private string GetProfilePictureFromDb()
        {
            string username = Request.QueryString["username"];

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
                    return _dt3.Rows[0]["riderFacePicBase64"].ToString();
                }

            }

            else if (role == "admin")
            {

                return ConvertProfilePicToBase64("/Image/logo.jpg");
            }

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

    }
}