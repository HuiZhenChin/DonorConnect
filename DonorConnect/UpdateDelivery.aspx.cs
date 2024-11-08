using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.PeerToPeer;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;
using Microsoft.AspNetCore.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace DonorConnect
{
    public partial class UpdateDelivery1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    if (Session["role"] != null && Session["role"].ToString() == "rider")
                    {
                     
                        string deliveryId = Request.QueryString["deliveryId"];
                        string token = Request.QueryString["token"];

                        // if only deliveryId is present (no token)
                        if (!string.IsNullOrEmpty(deliveryId) && string.IsNullOrEmpty(token))
                        {
                            // load the delivery based on deliveryId
                            LoadDelivery(deliveryId);
                            LoadDetails(deliveryId);
                            LoadLiveLink(deliveryId);
                        }
                        // both deliveryId and token are present
                        else if (!string.IsNullOrEmpty(deliveryId) && !string.IsNullOrEmpty(token))
                        {
                            // check if the QR code has already been used
                            bool isQrCodeUsed = CheckIfUsed(deliveryId);

                            if (isQrCodeUsed)
                            {
                                // if the QR code has already been used
                                ScriptManager.RegisterStartupScript(this, GetType(), "showalert",
                                "Swal.fire({ icon: 'error', title: 'QR Code already used', text: 'This QR code has already been used.' }).then(function() { window.location.href = 'ViewDelivery.aspx'; });", true);

                            }
                            else
                            {
                                // QR code is not used, update the status
                                UpdateStatus(deliveryId);

                                // load the delivery based on deliveryId
                                LoadDelivery(deliveryId);
                                LoadDetails(deliveryId);
                                LoadLiveLink(deliveryId);
                            }
                        }
                        else
                        {
                            // invalid QR code
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showError('Error in scanning QR Code! Please try again.');", true);
                        }
                    }
                    else
                    {
                       
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showError('Error in scanning QR Code! Please try again.');", true);
                    }
                }
                else
                {
               
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "showError('Error in scanning QR Code! Please try again.');", true);
                }
            }


        }

        protected void LoadDelivery(string deliveryId)
        {
            // retrieve delivery details based on deliveryId
            QRY _Qry = new QRY();
            DataTable dtDelivery = new DataTable();
            string sqlQuery = $"SELECT deliveryStatus, riderId, pickupDate, pickupTime, pickupTimeByRider, acceptTimeByRider, reachTimeByRider, noteRider FROM delivery WHERE deliveryId = '{deliveryId}'";
            dtDelivery = _Qry.GetData(sqlQuery);
            string riderName = "";

            if (dtDelivery.Rows.Count > 0)
            {
                string deliveryStatus = dtDelivery.Rows[0]["deliveryStatus"].ToString();
                string riderId = dtDelivery.Rows[0]["riderId"].ToString();
                string pickupDate = dtDelivery.Rows[0]["pickupDate"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["pickupDate"]).ToString("dd MMM yyyy") : "N/A";
                string scheduledTime = dtDelivery.Rows[0]["pickupTime"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["pickupTime"]).ToString("hh:mm") : "N/A";
                string acceptTime = dtDelivery.Rows[0]["acceptTimeByRider"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["acceptTimeByRider"]).ToString("dd MMM yyyy hh:mm") : "N/A";
                string pickupTime = dtDelivery.Rows[0]["pickupTimeByRider"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["pickupTimeByRider"]).ToString("dd MMM yyyy hh:mm") : "N/A";
                string reachTime = dtDelivery.Rows[0]["reachTimeByRider"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["reachTimeByRider"]).ToString("dd MMM yyyy hh:mm") : "N/A";

                lblNote.Text = string.IsNullOrEmpty(dtDelivery.Rows[0]["noteRider"].ToString()) ? "-" : dtDelivery.Rows[0]["noteRider"].ToString();

                riderName = GetRiderName(riderId);
                string url = $"PreviewPublicInfo.aspx?role=rider&username={riderName}";


                pickupDateLbl.Text = pickupDate;
                scheduledTimeLbl.Text = scheduledTime;
                // update the labels and progress bar based on the delivery status
                switch (deliveryStatus)
                {
                    case "Waiting for delivery rider":
                        lblStatus.Text = "Waiting for delivery rider to accept the order...";
                        step1.Attributes["class"] = "step-active";
                        break;

                    case "Accepted":
                        lblStatus.Text = "Waiting for delivery rider to pick up...";
                        step1.Attributes["class"] = "step-done";
                        step2.Attributes["class"] = "step-active";
                        date1.InnerText = acceptTime;
                        
                
                        lblRider.Text = riderName;
                        lblRider.NavigateUrl = url;
                        lblRider.Target = "_blank";
                        break;

                    case "Delivering in progress":
                        lblStatus.Text = "Delivering in Progress...";
                        step1.Attributes["class"] = "step-done";
                        step2.Attributes["class"] = "step-done";
                        step3.Attributes["class"] = "step-active";
                        date1.InnerText = pickupDate;
                        date2.InnerText = pickupTime;
                        lblRider.Text = riderName;
                        lblRider.NavigateUrl = url;
                        lblRider.Target = "_blank";

                        // check if the pickupImg has been uploaded
                        string sql = "SELECT pickupImg FROM delivery WHERE deliveryId = @deliveryId";
                        QRY _Qry2 = new QRY();
                        var parameters = new Dictionary<string, object>
                        {
                            { "@deliveryId", deliveryId }
                        };

                        DataTable dt = _Qry2.GetData(sql, parameters);

                        if (dt.Rows.Count > 0)
                        {
                            string pickupImg = dt.Rows[0]["pickupImg"].ToString();

                            if (!string.IsNullOrEmpty(pickupImg))
                            {
                                
                                showPickupImg.Style["display"] = "block";    
                                pickupBtn.Style["display"] = "none"; 
                            }
                            else
                            {
                                
                                pickupBtn.Style["display"] = "inline"; 
                                showPickupImg.Style["display"] = "none";    
                            }
                        }
                        else
                        {
                            // where no delivery record is found 
                        }
                        break;

                    case "Reached Destination":
                        lblStatus.Text = "Delivered!";
                        step1.Attributes["class"] = "step-done";
                        step2.Attributes["class"] = "step-done";
                        step3.Attributes["class"] = "step-done";
                        step4.Attributes["class"] = "step-done";
                        date1.InnerText = pickupDate;
                        date2.InnerText = pickupTime;
                        date3.InnerText = reachTime;
                        lblRider.Text = riderName;
                        lblRider.NavigateUrl = url;
                        lblRider.Target = "_blank";

                        // check if the reachImg has been uploaded
                        string sql2 = "SELECT reachImg, earningReceiptPath, itemApproved FROM delivery WHERE deliveryId = @deliveryId";
                        QRY _Qry3 = new QRY();
                        var parameters2 = new Dictionary<string, object>
                        {
                            { "@deliveryId", deliveryId }
                        };

                        DataTable dt2 = _Qry3.GetData(sql2, parameters2);

                        if (dt2.Rows.Count > 0)
                        {
                            string reachImg = dt2.Rows[0]["reachImg"].ToString();
                            string earningPath= dt2.Rows[0]["earningReceiptPath"].ToString();
                            int itemApproved = Convert.ToInt32(dt2.Rows[0]["itemApproved"]);

                            // decrypt the receipt file path
                            string decryptedReceiptFilePath = Encryption.Decrypt(earningPath);

                            if (!string.IsNullOrEmpty(reachImg))
                            {
                                
                                showReachImg.Style["display"] = "block";  
                                reachBtn.Style["display"] = "none";
                                
                               
                            }
                            else
                            {
                               
                                reachBtn.Style["display"] = "inline";      
                                showReachImg.Style["display"] = "none";   
                            }

                            if (itemApproved == 1 && !string.IsNullOrEmpty(decryptedReceiptFilePath))
                            {
                               receiptAttch.Attributes["href"] = decryptedReceiptFilePath;
                               receiptAttch.Style["display"] = "block";
                            }
                            else
                            {
                               receiptAttch.Style["display"] = "none";
                            }
                        }
                        else
                        {
                            
                        }

                        showPickupImg.Style["display"] = "block";
                        break;


                    default:
                        lblStatus.Text = "Delivery status unknown";
                        break;
                }
            }
        }

        public static string GetRiderName(string id)
        {


            if (string.IsNullOrEmpty(id))
            {
                throw new Exception("Id is missing.");
            }

            string sql = "SELECT riderFullName FROM delivery_rider WHERE riderId = @id";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@id", id }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["riderFullName"].ToString();
            }
            else
            {
                throw new Exception($"No record found for id: {id}");
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string deliveryId = Request.QueryString["deliveryId"];

            if (string.IsNullOrEmpty(deliveryId))
            {
               
                return;
            }

            // get the deliveryStatus from the database based on deliveryId
            string deliveryStatus = GetDeliveryStatus(deliveryId);

        
            if (deliveryStatus == "Accepted")
            {
                // trigger the frontend to enable the camera
                ScriptManager.RegisterStartupScript(this, GetType(), "startVideo()", "startVideo();", true);
            }

            else if (deliveryStatus == "Delivering in progress")
            {
                string sql = "SELECT pickupImg FROM delivery WHERE deliveryId = @deliveryId";
                QRY _Qry = new QRY();
                var parameters = new Dictionary<string, object>
                {
                    { "@deliveryId", deliveryId }
                };

                DataTable dt = _Qry.GetData(sql, parameters);

                if (dt.Rows.Count > 0)
                {
                    string pickupImg = dt.Rows[0]["pickupImg"].ToString();

                    if (!string.IsNullOrEmpty(pickupImg))
                    {
                     
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "showModal", "setTimeout(function() { showReachModal(); }, 500);", true);
                    }
                    else
                    {
                        // if no image is uploaded
                        
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showInfo('Please upload the pick-up image before updating the status.',);", true);

                    }
                }
                else
                {
                   
                }
            }

            else
            {
                
                lblStatus.Text = "Camera is not enabled for this status.";
                return;
            }

           
        }
     
        private string GetDeliveryStatus(string deliveryId)
        {
            string deliveryStatus = string.Empty;
         
            QRY _Qry = new QRY();

            string sql = "SELECT deliveryStatus FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                deliveryStatus = dt.Rows[0]["deliveryStatus"].ToString();
            }

            return deliveryStatus;
        }

        private string GetOrg(string deliveryId)
        {
            string orgId = string.Empty;

            QRY _Qry = new QRY();

            string sql = "SELECT orgId FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                orgId = dt.Rows[0]["orgId"].ToString();
            }

            return orgId;
        }

        private string GetDonationId(string deliveryId)
        {
            string donationId = string.Empty;

            QRY _Qry = new QRY();

            string sql = "SELECT donationId FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                donationId = dt.Rows[0]["donationId"].ToString();
            }

            return donationId;
        }
        private string GetDonorId(string deliveryId)
        {
            string donorId = string.Empty;

            QRY _Qry = new QRY();

            string sql = "SELECT donorId FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                donorId = dt.Rows[0]["donorId"].ToString();
            }

            return donorId;
        }

        [WebMethod]
        public static object DecryptUrl(string encryptedUrl)
        {
            string decryptedToken;
            string deliveryId;

            try
            {
                // parse the URL to extract query parameters
                Uri uri = new Uri(encryptedUrl);
                var query = HttpUtility.ParseQueryString(uri.Query);

                // extract deliveryId and token
                deliveryId = query["deliveryId"];
                string encryptedToken = query["token"];

                if (string.IsNullOrEmpty(deliveryId) || string.IsNullOrEmpty(encryptedToken))
                {
                    return new { isDecrypted = false, message = "Invalid URL or missing parameters." };
                }

                // decrypt the token
                decryptedToken = Decrypt(encryptedToken); 
            }
            catch (Exception ex)
            {
                return new { isDecrypted = false, message = "Decryption failed!" };
            }

            // store the decrypted URL 
            HttpContext.Current.Session["DecryptedUrl"] = encryptedUrl;

            // return the decrypted data and the deliveryId
            string newUrl = $"UpdateDelivery.aspx?deliveryId={deliveryId}&token={decryptedToken}";
            return new { isDecrypted = true, decryptedUrl = newUrl, deliveryId = deliveryId, token = decryptedToken };
        }


        private static string Decrypt(string encryptedToken)
        {
            encryptedToken= Encryption.Decrypt(encryptedToken);

            return encryptedToken;
        }

        protected void UpdateStatus(string deliveryId)
        {
            QRY _Qry = new QRY();
           
            string sql = "UPDATE delivery SET deliveryStatus = @status, pickupTimeByRider = @pickupTime, usedQrCode= @used WHERE deliveryId = @deliveryId";

            DateTime pickupTime = DateTime.Now;

            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId },
                { "@status", "Delivering in progress" },
                { "@pickupTime", pickupTime },
                { "@used", 1 },
            };

            bool success= _Qry.ExecuteNonQuery(sql, parameter);

            if (success)
            {
                QRY _Qry2 = new QRY();
                string message = "Your donation delivery status has been updated to Delivering in Progress" + ".";
                string donationId = GetDonationId(deliveryId);
                string donorId = GetDonorId(deliveryId);
                string link = $"Delivery.aspx?donationId={donationId}";

                string encryptedLink = Encryption.Encrypt(link);

                string sqlNtf = "EXEC [create_notifications] " +
                                "@method = 'INSERT', " +
                                "@id = NULL, " +
                                "@userId = @userId, " +
                                "@link = @link, " +
                                "@content = @content";

                var notificationParameter = new Dictionary<string, object>
                {
                    { "@userId", donorId },
                    { "@link", encryptedLink },
                    { "@content", message }
                };

                _Qry2.ExecuteNonQuery(sqlNtf, notificationParameter);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Status updated successful!',);", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in updating status. Please try again.',);", true);

            }


        }

        protected void btnUpdate2_Click(object sender, EventArgs e)
        {
            QRY _Qry = new QRY();

            string deliveryId = Request.QueryString["deliveryId"];

            string sql = "UPDATE delivery SET deliveryStatus = @status, reachTimeByRider = @reachTime, reachImg= @img WHERE deliveryId = @deliveryId";

            DateTime reachTime = DateTime.Now;

            string img = "";

            if (fuReach.HasFiles)
            {

                img = ImageFileProcessing.ConvertToBase64(fuReach.PostedFiles);
            }

            else
            {
                img = "";
            }

            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId },
                { "@status", "Reached Destination" },
                { "@reachTime", reachTime },
                { "@img", img}
            };

            bool success = _Qry.ExecuteNonQuery(sql, parameter);

            if (success)
            {
                string filepath= GeneratePaymentReceipt(deliveryId);
                string pathEncrypted = Encryption.Encrypt(filepath);
                string sqlPath = "UPDATE delivery SET earningReceiptPath= @path WHERE deliveryId = @deliveryId";

                var pathParam = new Dictionary<string, object>
                {
                    { "@path", pathEncrypted },
                    { "@deliveryId", deliveryId }
                };

                _Qry.ExecuteNonQuery(sqlPath, pathParam);

                string donationId = GetDonationId(deliveryId);
                string donorId = GetDonorId(deliveryId);
                string fullLink2 = "https://localhost:44390/Delivery.aspx?donationId=" + donationId;

                // send email to notify organization
                string sqlemail = "EXEC [application_email] " +
                                  "@action = 'DONATION DELIVERY UPDATE COMPLETED', " +
                                  "@role = 'donor', " +
                                  "@resubmitlink = @link, " +
                                  "@donorId = @donorId, " +
                                  "@donationpublishid = @donationPublishId";

                var emailParameter = new Dictionary<string, object>
                {
                    { "@link", fullLink2 },
                    { "@donorId", donorId },
                    { "@donationPublishId", donationId }
                };

                _Qry.ExecuteNonQuery(sqlemail, emailParameter);

                QRY _Qry2 = new QRY();
                string message2 = "Your donation delivery status has been updated to Completed" + ".";             
                string link = $"Delivery.aspx?donationId={donationId}";

                string encryptedLink2 = Encryption.Encrypt(link);

                string sqlNtf2 = "EXEC [create_notifications] " +
                                "@method = 'INSERT', " +
                                "@id = NULL, " +
                                "@userId = @userId, " +
                                "@link = @link, " +
                                "@content = @content";

                var notificationParameter = new Dictionary<string, object>
                {
                    { "@userId", donorId },
                    { "@link", encryptedLink2 },
                    { "@content", message2 }
                };

                _Qry2.ExecuteNonQuery(sqlNtf2, notificationParameter);

                // send notifications for org to get confirm whether the item is arrived

                string orgId = GetOrg(deliveryId);
                string message = "Delivery with ID [" + deliveryId + "] has successfully arrived at its destination. Please verify the status so the donor can receive the acknowledgment receipt, and the rider can claim their earnings. It only lasts for 7 days, the system will auto-approve it after that.";

                string token = Hash(orgId + DateTime.Now.ToString());
                token = token.Replace("+", "A");
                string fullLink = $"https://localhost:44390/Delivery.aspx?deliveryId={deliveryId}&token={token}";
                string encryptedLink = Encryption.Encrypt(fullLink);

                string sqlNtf = "EXEC [create_notifications] " +
                "@method = 'INSERT', " +
                "@id = NULL, " +
                "@userId = '" + orgId + "', " +
                "@link = '" + encryptedLink + "', " +
                "@supportingId = '" + deliveryId + "', " +
                "@content = '" + message + "' ";

                QRY _Qry4 = new QRY();
                bool success2 = _Qry4.ExecuteNonQuery(sqlNtf);

                QRY _Qry5 = new QRY();

                // send email notify organization
                string sqlEmail =
                   "EXEC [item_arrival_verification] " +
                   "@method = @method, " +
                   "@orgId = @orgId, " +
                   "@link = @link, " +
                   "@deliveryId = @deliveryId ";
                   

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@method", "APPROVAL" },
                    { "@orgId", orgId },
                    { "@link", fullLink },
                    { "@deliveryId", deliveryId }
                };

               
                bool success3 = _Qry5.ExecuteNonQuery(sqlEmail, parameters);

                if (success2 && success3)
                {
                   
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showSuccess('Status updated successfully');", true);
                    LoadDelivery(deliveryId);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in updating status. Please try again.',);", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in updating status. Please try again.',);", true);

            }
        }

        protected void btnUpdateImg_Click(object sender, EventArgs e)
        {
            QRY _Qry = new QRY();

            string deliveryId = Request.QueryString["deliveryId"];

            string sql = "UPDATE delivery SET pickupImg= @img WHERE deliveryId = @deliveryId";
        
            string img = "";

            if (fuPickUp.HasFiles)
            {

                img = ImageFileProcessing.ConvertToBase64(fuPickUp.PostedFiles);
            }

            else
            {
                img = "";
            }

            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId },              
                { "@img", img}
            };

            bool success = _Qry.ExecuteNonQuery(sql, parameter);

            if (success)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Picture uploaded successful!',);", true);
                LoadDelivery(deliveryId);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in uploaded pictures. Please try again.',);", true);

            }
        }


        private bool CheckIfUsed(string deliveryId)
        {
            bool isUsed = false;

            string sql = "SELECT usedQrCode FROM delivery WHERE deliveryId = @deliveryId";
            QRY _Qry = new QRY();

            var parameters = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                // check if usedQrCode is not 0
                if (dt.Rows[0]["usedQrCode"] != DBNull.Value && Convert.ToInt32(dt.Rows[0]["usedQrCode"]) != 0)
                {
                    isUsed = true;
                }
            }

            return isUsed;
        }


        [WebMethod]
        public static string GetPickupImage(string deliveryId)
        {
           
            string encryptedBase64Images = GetPickupImageString(deliveryId);
           
            string imagesHtml = ImageFileProcessing.ProcessImages(encryptedBase64Images);

            return imagesHtml;
        }

        [WebMethod]
        public static string GetReachImage(string deliveryId)
        {

            string encryptedBase64Images = GetReachImageString(deliveryId);

            string imagesHtml = ImageFileProcessing.ProcessImages(encryptedBase64Images);

            return imagesHtml;
        }

        public static string GetPickupImageString(string deliveryId)
        {
            string img = "";

            QRY _Qry = new QRY();

            string sql = "SELECT pickupImg FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                img = dt.Rows[0]["pickupImg"].ToString();
            }

            return img;
        }

        public static string GetReachImageString(string deliveryId)
        {
            string img = "";

            QRY _Qry = new QRY();

            string sql = "SELECT reachImg FROM delivery WHERE deliveryId = @deliveryId";
            var parameter = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                img = dt.Rows[0]["reachImg"].ToString();
            }

            return img;
        }

        static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

      
        private string GeneratePaymentReceipt(string deliveryId)
        {
                // receipt saved file path
                string folderPath = HttpContext.Current.Server.MapPath("~/RiderPaymentReceipt/");
                string filePath = folderPath + deliveryId + "_RiderPaymentReceipt.pdf";
                string fileUrl = "/RiderPaymentReceipt/" + deliveryId + "_RiderPaymentReceipt.pdf";

                // create the folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fullLink = "https://localhost:44390/" + fileUrl;

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // fonts

                iTextSharp.text.Font titleFontSize = FontFactory.GetFont("Arial", 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font subTitleFontSize = FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD, BaseColor.DARK_GRAY);
                iTextSharp.text.Font bodyFontSize = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font footerFontSize = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.DARK_GRAY);

                // 1. Add Logo and Website Name 
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 70, 30 });
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // add logo
                string logoFilePath = Server.MapPath("/Image/logo.png");

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoFilePath);
                logo.ScaleToFit(80f, 50f);
                PdfPCell logoCell = new PdfPCell(logo);
                logoCell.Border = PdfPCell.NO_BORDER;
                logoCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                logoCell.Padding = 10;
                headerTable.AddCell(logoCell);

                // add website name
                PdfPCell websiteName = new PdfPCell(new Phrase("DonorConnect", titleFontSize));
                websiteName.Border = PdfPCell.NO_BORDER;
                websiteName.HorizontalAlignment = Element.ALIGN_RIGHT;
                websiteName.VerticalAlignment = Element.ALIGN_MIDDLE;
                websiteName.BackgroundColor = BaseColor.LIGHT_GRAY;
                websiteName.Padding = 10;
                headerTable.AddCell(websiteName);

                document.Add(headerTable);
                document.Add(new Paragraph("\n"));

                // 2. Add Title
                Paragraph title = new Paragraph("Earnings Statement", titleFontSize);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                // 3. Create Details Table 
                PdfPTable detailsTable = new PdfPTable(2);
                detailsTable.WidthPercentage = 100;
                detailsTable.DefaultCell.Border = PdfPCell.NO_BORDER;

           
                var details = GetDeliveryDetails(deliveryId); 

                detailsTable.AddCell(new Phrase("Name", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["riderFullName"], bodyFontSize));

                detailsTable.AddCell(new Phrase("Email Address", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["riderEmail"], bodyFontSize));

                detailsTable.AddCell(new Phrase("Contact Number", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["riderContactNumber"], bodyFontSize));

                detailsTable.AddCell(new Phrase("Organization Name", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["orgName"], bodyFontSize));

                detailsTable.AddCell(new Phrase("Destination Address", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["destinationAddress"], bodyFontSize));

                detailsTable.AddCell(new Phrase("Vehicle Type", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["vehicleType"], bodyFontSize));

                detailsTable.AddCell(new Phrase("Plate Number", bodyFontSize));
                detailsTable.AddCell(new Phrase(details["plateNo"], bodyFontSize));


                document.Add(detailsTable);
                document.Add(new Paragraph("\n"));

                // 4. Payment Type and Amount Table
                PdfPTable paymentTable = new PdfPTable(2);
                paymentTable.WidthPercentage = 100;
                paymentTable.SetWidths(new float[] { 50, 50 });

            
                PdfPCell emptyHeader = new PdfPCell(new Phrase("", bodyFontSize));
                emptyHeader.BorderColor = BaseColor.BLACK;
                emptyHeader.BackgroundColor = BaseColor.LIGHT_GRAY;
                emptyHeader.Border = PdfPCell.BOX;
                paymentTable.AddCell(emptyHeader);

                PdfPCell amountEarnedHeader = new PdfPCell(new Phrase("Amount Earned", bodyFontSize));
                amountEarnedHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                amountEarnedHeader.BorderColor = BaseColor.BLACK;
                amountEarnedHeader.BackgroundColor = BaseColor.LIGHT_GRAY;
                amountEarnedHeader.Border = PdfPCell.BOX;
                paymentTable.AddCell(amountEarnedHeader);

             
                PdfPCell deliveryFeeLabel = new PdfPCell(new Phrase("Delivery Fee", bodyFontSize));
                deliveryFeeLabel.BorderColor = BaseColor.BLACK;
                deliveryFeeLabel.Border = PdfPCell.BOX;
                paymentTable.AddCell(deliveryFeeLabel);

                PdfPCell paymentAmountCell = new PdfPCell(new Phrase(details["paymentAmount"], bodyFontSize));
                paymentAmountCell.BorderColor = BaseColor.BLACK;
                paymentAmountCell.Border = PdfPCell.BOX;
                paymentAmountCell.HorizontalAlignment = Element.ALIGN_CENTER;  
                paymentAmountCell.VerticalAlignment = Element.ALIGN_MIDDLE;   
                paymentTable.AddCell(paymentAmountCell);

          
                document.Add(paymentTable);
                document.Add(new Paragraph("\n"));

                // 5. Footer: Current Year and DonorConnect
                Paragraph autoGeneratedFooter = new Paragraph("This receipt is auto-generated by the system.", footerFontSize);
                autoGeneratedFooter.Alignment = Element.ALIGN_CENTER;
                autoGeneratedFooter.SpacingAfter = 5f;
                document.Add(autoGeneratedFooter);

                int currentYear = DateTime.Now.Year;
                Paragraph footer = new Paragraph($"© {currentYear} DonorConnect", footerFontSize);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);


                // close the document
                document.Close();
                writer.Close();
            }
            return fullLink;
        }

        protected Dictionary<string, string> GetDeliveryDetails(string deliveryId)
        {
            Dictionary<string, string> deliveryDetails = new Dictionary<string, string>();
            string sql = @"
            SELECT 
                d.riderId,
                d.destinationAddress,
                d.paymentAmount,
                d.orgId,
                r.riderFullName,
                r.riderEmail,
                r.riderContactNumber,
                r.vehicleType,
                r.vehiclePlateNumber,
                o.orgName
            FROM 
                delivery d
            INNER JOIN 
                delivery_rider r ON d.riderId = r.riderId
            INNER JOIN
                organization o ON d.orgId = o.orgId
            WHERE 
                d.deliveryId = @deliveryId";

    
            var parameters = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql, parameters);

            // populate delivery details if data is retrieved
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                deliveryDetails["riderId"] = row["riderId"].ToString();
                deliveryDetails["destinationAddress"] = row["destinationAddress"].ToString();
                deliveryDetails["paymentAmount"] = row["paymentAmount"].ToString();
                deliveryDetails["orgId"] = row["orgId"].ToString();
                deliveryDetails["orgName"] = row["orgName"].ToString();
                deliveryDetails["riderFullName"] = row["riderFullName"].ToString();
                deliveryDetails["riderEmail"] = row["riderEmail"].ToString();
                deliveryDetails["riderContactNumber"] = row["riderContactNumber"].ToString();
                deliveryDetails["vehicleType"] = row["vehicleType"].ToString();
                deliveryDetails["plateNo"] = row["vehiclePlateNumber"].ToString();
            }
            else
            {
                throw new Exception("No delivery details found for the provided delivery ID.");
            }

            return deliveryDetails;
        }

        [WebMethod]
        public static object GetDeliveryAddress(string deliveryId)
        {
            var deliveryDetails = new Dictionary<string, string>();
            DataTable dt = FetchDeliveryData(deliveryId);

            if (dt.Rows.Count > 0)
            {
                deliveryDetails["pickupAddress"] = dt.Rows[0]["pickupAddress"].ToString();
                deliveryDetails["destinationAddress"] = dt.Rows[0]["destinationAddress"].ToString();
            }
            else
            {
                deliveryDetails["pickupAddress"] = "No pickup address found";
                deliveryDetails["destinationAddress"] = "No destination address found";
            }

            return new JavaScriptSerializer().Serialize(deliveryDetails);
        }



        private static DataTable FetchDeliveryData(string deliveryId)
        {
            string sql = "SELECT pickupAddress, destinationAddress FROM delivery WHERE deliveryId = @deliveryId";
            var parameters = new Dictionary<string, object>
            {
                { "@deliveryId", deliveryId }
            };

            QRY _Qry = new QRY();
            return _Qry.GetData(sql, parameters);
        }

        [WebMethod]
        public static void UpdateRiderLocation(string deliveryId, double latitude, double longitude)
        {
            // check if delivery status
            string checkStatusQuery = "SELECT deliveryStatus FROM delivery WHERE deliveryId = @deliveryId";
            var statusParams = new Dictionary<string, object> { { "@deliveryId", deliveryId } };

            QRY _Qry = new QRY();
            string deliveryStatus = Convert.ToString(_Qry.ExecuteScalar(checkStatusQuery, statusParams));

            // if the delivery status is "Reached Destination"
            if (deliveryStatus == "Reached Destination")
            {
                // do not update the location
                return;
            }

            string checkQuery = "SELECT COUNT(*) FROM rider_location WHERE deliveryId = @deliveryId";
            var checkParams = new Dictionary<string, object> { { "@deliveryId", deliveryId } };

            int count = Convert.ToInt32(_Qry.ExecuteScalar(checkQuery, checkParams));

            if (count > 0)
            {

                string updateQuery = "UPDATE rider_location SET latitude = @latitude, longitude = @longitude, lastUpdated = GETDATE() WHERE deliveryId = @deliveryId";
                var updateParams = new Dictionary<string, object>
                {
                    { "@latitude", latitude },
                    { "@longitude", longitude },
                    { "@deliveryId", deliveryId }
                };
                _Qry.ExecuteNonQuery(updateQuery, updateParams);
            }
            else
            {
                // if deliveryId does not exist, insert a new record
                string insertQuery = "INSERT INTO rider_location (deliveryId, latitude, longitude, lastUpdated) VALUES (@deliveryId, @latitude, @longitude, GETDATE())";
                var insertParams = new Dictionary<string, object>
                {
                    { "@deliveryId", deliveryId },
                    { "@latitude", latitude },
                    { "@longitude", longitude }
                };
                _Qry.ExecuteNonQuery(insertQuery, insertParams);
            }
        }


        protected void LoadDetails(string deliveryId)
        {
            try
            {
                QRY _Qry = new QRY();

                string deliverySql = "SELECT donationId, pickupAddress, destinationAddress FROM delivery WHERE deliveryId = @deliveryId";
                var deliveryParams = new Dictionary<string, object> { { "@deliveryId", deliveryId } };
                DataTable deliveryData = _Qry.GetData(deliverySql, deliveryParams);

                if (deliveryData.Rows.Count > 0)
                {
                    lblDonorAddress.Text = deliveryData.Rows[0]["pickupAddress"].ToString();
                    lblOrgAddress.Text = deliveryData.Rows[0]["destinationAddress"].ToString();
                    string donationId= deliveryData.Rows[0]["donationId"].ToString();

                    string donationRequestSql = "SELECT donorFullName, donorPhone, donationPublishId FROM donation_item_request WHERE donationId = @donationId";
                    var donationRequestParams = new Dictionary<string, object> { { "@donationId", donationId } };
                    DataTable donationRequestData = _Qry.GetData(donationRequestSql, donationRequestParams);

                    if (donationRequestData.Rows.Count > 0)
                    {
                       lblDonorName.Text = donationRequestData.Rows[0]["donorFullName"].ToString();
                       lblDonorContact.Text = donationRequestData.Rows[0]["donorPhone"].ToString();
                        string donationPublishId = donationRequestData.Rows[0]["donationPublishId"].ToString();

                        string donationPublishSql = "SELECT recipientName, recipientPhoneNumber FROM donation_publish WHERE donationPublishId = @donationPublishId";
                        var donationPublishParams = new Dictionary<string, object> { { "@donationPublishId", donationPublishId } };
                        DataTable donationPublishData = _Qry.GetData(donationPublishSql, donationPublishParams);

                        if (donationPublishData.Rows.Count > 0)
                        {
                           lblOrgName.Text = donationPublishData.Rows[0]["recipientName"].ToString();
                           lblOrgContact.Text = donationPublishData.Rows[0]["recipientPhoneNumber"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Recipient details not found for donationPublishId: " + donationPublishId);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Donation request details not found for donationId: " + donationId);
                    }
                }
                else
                {
                    Console.WriteLine("Delivery details not found for deliveryId: " + deliveryId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading details: " + ex.Message);
            }
        }

        [WebMethod]
        public static string saveLiveLink(string deliveryId, string liveLink)
        {
            if (string.IsNullOrWhiteSpace(liveLink))
            {
                return JsonConvert.SerializeObject(new { success = false, message = "Live link cannot be empty" });
            }

            try
            {
                QRY _Qry = new QRY();

                liveLink = Encryption.Encrypt(liveLink);

                string sql = "UPDATE delivery SET liveLink = @liveLink WHERE deliveryId = @deliveryId";
                var parameter = new Dictionary<string, object>
                {
                    { "@deliveryId", deliveryId },
                    { "@liveLink", liveLink }
                };

                bool success = _Qry.ExecuteNonQuery(sql, parameter);

                if (success)
                {
                    return JsonConvert.SerializeObject(new { success = true, message = "Live link saved successfully" });
                }
                else
                {
                    return JsonConvert.SerializeObject(new { success = false, message = "Failed to save live link. Delivery not found or no update occurred." });
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { success = false, message = ex.Message });
            }
        }

        private void LoadLiveLink(string deliveryId)
        {
            try
            {
                QRY _Qry = new QRY();
                string sql = "SELECT liveLink FROM delivery WHERE deliveryId = @deliveryId";
                var parameter = new Dictionary<string, object> { { "@deliveryId", deliveryId } };

                string encryptedLink = _Qry.ExecuteScalar(sql, parameter)?.ToString();

                if (!string.IsNullOrEmpty(encryptedLink))
                {
                    // decrypt the link 
                    string liveLink = Encryption.Decrypt(encryptedLink);
                    liveLocationLink.Text = liveLink;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading live link: " + ex.Message);
            }

        }
    }
}