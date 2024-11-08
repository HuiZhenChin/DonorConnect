using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // if the session 'username' is null, not logged-in user
                if (Session["username"] == null)
                {
                    // hide the username column and ask users to enter full name, email, and phone number
                    username.Visible = false; 
                    txtFullName.ReadOnly = false;
                    txtEmail.ReadOnly = false;
                    txtPhoneNumber.ReadOnly = false;
                }
                else
                {
                    // display username and make other fields read-only based on user role
                    string username = Session["username"].ToString();
                    txtUsername.Text = username;
                    txtUsername.ReadOnly = true;

                   
                    string role = Session["role"]?.ToString();
                    
                    if (role == "donor")
                    {
                        // for donors, display full name, email, and phone number
                        txtFullName.Text = GetFullName(username, role);  
                        txtFullName.ReadOnly = true;
                        txtEmail.Text = GetEmail(username, role);       
                        txtEmail.ReadOnly = true;
                        txtPhoneNumber.Text = GetPhone(username, role);      
                       
                    }
                    else if (role == "organization")
                    {
                        // for organizations, hide full name and display email and phone number 
                        fullName.Visible = false;
                        txtEmail.Text = GetEmail(username, role);        
                        txtEmail.ReadOnly = true;
                        txtPhoneNumber.Text = GetPhone(username, role);     
                       
                    }

                    else if (role == "rider")
                    {
                        // for rider, display full name, email, and phone number
                        txtFullName.Text = GetFullName(username, role);
                        txtFullName.ReadOnly = true;
                        txtEmail.Text = GetEmail(username, role);
                        txtEmail.ReadOnly = true;
                        txtPhoneNumber.Text = GetPhone(username, role);
                        
                    }
                }
            }
        }
  
        private string GetFullName(string username, string role)
        {

            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            string fullName = "";

            if (role == "donor")
            {
                sql = "SELECT * FROM [donor] WHERE donorUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];
                    
                    fullName = row["donorName"].ToString();
                   
                }
            }
          

            else if (role == "rider")
            {
                sql = "SELECT * FROM [delivery_rider] WHERE riderUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];

                    fullName = row["riderFullName"].ToString();
                  
                }
            }

            return fullName;
        }

        private string GetEmail(string username, string role)
        {
            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            string email = "";

            if (role == "donor")
            {
                sql = "SELECT * FROM [donor] WHERE donorUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];

                    email = row["donorEmail"].ToString();
                   
                }
            }

            else if (role == "organization")
            {
                sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];

                    email = row["orgEmail"].ToString();
                    
                }
            }

            else if (role == "rider")
            {
                sql = "SELECT * FROM [delivery_rider] WHERE riderUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];

                    email = row["riderEmail"].ToString();
                    
                }
            }

            return email;
        }

        private string GetPhone(string username, string role)
        {
            string sql;
            QRY _Qry = new QRY();
            DataTable _dt;
            string phoneNumber = "";

            if (role == "donor")
            {
                sql = "SELECT * FROM [donor] WHERE donorUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];

                    phoneNumber = row["donorContactNumber"].ToString();

                }
            }

            else if (role == "organization")
            {
                sql = "SELECT * FROM [organization] WHERE orgName = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];
                  
                    phoneNumber = row["orgContactNumber"].ToString() + " / " + row["picContactNumber"].ToString();


                }
            }

            else if (role == "rider")
            {
                sql = "SELECT * FROM [delivery_rider] WHERE riderUsername = '" + username + "' ";
                _dt = _Qry.GetData(sql);

                if (_dt.Rows.Count > 0)
                {
                    DataRow row = _dt.Rows[0];

                    phoneNumber = row["riderContactNumber"].ToString();

                }
            }

            return phoneNumber;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool isValid = true;

            // check required field input
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                lblErrorFullName.Visible = true;
                isValid = false;  
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                lblErrorEmail.Visible = true;
                isValid = false;  
            }
            if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                lblErrorPhoneNumber.Visible = true;
                isValid = false;  
            }
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                lblErrorMsg.Visible = true;
                isValid = false;  
            }

           
            if (!isValid)
            {
                return;
            }

            // if valid, proceed with form submission
            string username = Session["username"] != null ? Session["username"].ToString() : null;
            string base64File = "";
            string fileUrl = "";
            string filePath = "";
            string allFileUrl = "";
            string allFilePath = "";
            string messageId = GenerateMessageId();

            
            string senderEmail = txtEmail.Text;
            string senderPhoneNumber = txtPhoneNumber.Text;
            string senderFullName = txtFullName.Text;
            string senderOrgName = txtOrgName.Text;
            string messageSent = txtMessage.Text;

            if (fileAttachment.HasFiles)
            {
                string folderPath = HttpContext.Current.Server.MapPath("~/ContactUsAttachment/");
                List<string> fileUrls = new List<string>();  // To store all file URLs
                List<string> filePaths = new List<string>();  // To store all file paths

                // loop through all posted files
                foreach (HttpPostedFile postedFile in fileAttachment.PostedFiles)
                {
                    // get the file extension of the uploaded file
                    string fileExtension = Path.GetExtension(postedFile.FileName).ToLower();

                    string[] allowedExtensions = { ".pdf", ".png", ".jpg", ".jpeg" };

                    if (allowedExtensions.Contains(fileExtension))
                    {
                        string currentFilePath = Path.Combine(folderPath, messageId + "_" + Path.GetFileNameWithoutExtension(postedFile.FileName) + fileExtension);

                        string currentFileUrl = "/ContactUsAttachment/" + messageId + "_" + Path.GetFileNameWithoutExtension(postedFile.FileName) + fileExtension;

                        filePaths.Add(currentFilePath);
                        fileUrls.Add(currentFileUrl);

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // save the file
                        postedFile.SaveAs(currentFilePath);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Invalid file type. Only PDF, PNG, JPG, and JPEG are allowed.');", true);
                        return;
                    }
                }

                allFileUrl = string.Join(";", fileUrls);

                allFilePath = string.Join(";", filePaths);

            }
            else
            {
                allFilePath = "";
            }




            string sql = @"
          INSERT INTO [contact_us]
          ([messageId], [sentBy], [dateSent], [messageSent], [fileAttchSent], [senderEmail], 
           [senderPhoneNumber], [senderFullName], [senderOrgName])
          VALUES
          (@messageId, @sentBy, GETDATE(), @messageSent, @fileAttchSent, @senderEmail, 
           @senderPhoneNumber, @senderFullName, @senderOrgName)";

            string encryptedFileUrl= Encryption.Encrypt(allFileUrl);

            QRY _Qry = new QRY();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@messageId", messageId },
                { "@sentBy", username != null ? username : senderFullName },
                { "@messageSent", messageSent },
                { "@fileAttchSent", encryptedFileUrl },
                { "@senderEmail", senderEmail },
                { "@senderPhoneNumber", senderPhoneNumber },
                { "@senderFullName", senderFullName },
                { "@senderOrgName", senderOrgName }
            };

            string sql2 = "EXEC [contact_us_reminder] " +
             "@username = '" + (senderOrgName != null ? senderOrgName : (username != null ? username : senderFullName)) + "', " +
             "@SENDER_EMAIL = '" + senderEmail + "', " +
             "@fullName= '" + senderFullName + "'," +
             "@phoneNumber= '" + senderPhoneNumber + "'," +
             "@orgName= '" + (senderOrgName != null ? senderOrgName : "NULL") + "'," +
             "@message = '" + messageSent + "'," +
             "@attch = '" + allFilePath + "' ";


            bool success = _Qry.ExecuteNonQuery(sql, parameters);
            bool success2 = _Qry.ExecuteNonQuery(sql2);

            if (success || success2)
            {
                if (!string.IsNullOrEmpty(allFilePath))
                {
                    string[] filePaths = allFilePath.Split(';');

                    foreach (string indifilePath in filePaths)
                    {
                        if (File.Exists(indifilePath))
                        {
                            try
                            {
                                // Delete each file
                                File.Delete(indifilePath);
                            }
                            catch (Exception ex)
                            {
                                // Log the error if the file can't be deleted
                                Console.WriteLine("Error deleting file: " + ex.Message);
                            }
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Your message has been sent successfully!');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error submitting form. Please try again!');", true);
            }
        }


        private string GenerateMessageId()
        {
            string sql = "SELECT TOP 1 [messageId] FROM [contact_us] ORDER BY CAST(SUBSTRING([messageId], 2, LEN([messageId])-1) AS INT) DESC";
            QRY _Qry = new QRY();

            DataTable dt = _Qry.GetData(sql, new Dictionary<string, object>());

            if (dt.Rows.Count == 0)
            {
                return "M1";
            }

            string currentMaxId = dt.Rows[0]["messageId"].ToString();

            string number = currentMaxId.Substring(1);
            int newNumber = int.Parse(number) + 1;

            // generate the new messageId
            string newMessageId = "M" + newNumber;

            return newMessageId;
        }



    }
}