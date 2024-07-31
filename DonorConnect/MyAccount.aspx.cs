using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                        //orgContent.Visible = false;
                    }
                    else if (role == "organization")
                    {
                        ShowOrgInfo(username);
                        // donorContent.Visible = false;
                        //orgContent.Visible = true;
                    }
                    else
                    {
                        //donorContent.Visible = false;
                        //orgContent.Visible = false;
                    }
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

                Session["Role"] = role;
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
            sql = "SELECT * FROM [organization] WHERE username = '" + username + "' ";
            _dt = _Qry.GetData(sql);

            //if (_dt.Rows.Count > 0)
            //{
            //    DataRow row = _dt.Rows[0];
            //    TextBox1.Text = row["username"].ToString();
            //    TextBox2.Text = row["full_name"].ToString();
            //    TextBox3.Text = row["email"].ToString();
            //    TextBox4.Text = row["phone"].ToString();
            //    TextBox5.Text = row["address"].ToString();
            //}
        }
    }
}
