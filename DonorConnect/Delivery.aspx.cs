using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Ajax.Utilities;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services.Description;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace DonorConnect
{
    public partial class Delivery : System.Web.UI.Page
    {
        
        public class DeliveryDetail
        {
            public string orgId { get; set; }
            public string donationId { get; set; }
        }

        public class OrganizationDetail
        {
            public string orgName { get; set; }
            public string orgAddress { get; set; }
            public string orgRegion { get; set; }
            public string PIC { get; set; }
            public string signature { get; set; }

        }

        public class DonationDetail
        {
            public string donorFullName { get; set; }
            public string pickupAddress { get; set; }
            public string state { get; set; }
            public string itemCategory { get; set; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    string username = Session["username"].ToString();

                    string userRole = Session["Role"] as string;

                    // set the role variable in JavaScript
                    ClientScript.RegisterStartupScript(this.GetType(), "SetUserRole", $"var userRole = '{userRole}';", true);

                    string donationId = Request.QueryString["donationId"];
                    string deliveryId = Request.QueryString["deliveryId"];
                    string token = Request.QueryString["token"];

                    // if donationId is provided 
                    if (!string.IsNullOrEmpty(donationId))
                    {
                        // load delivery details 
                        LoadDelivery(donationId);
                        LoadDetails(donationId);
                        LoadLiveLink(donationId);

                        shareLiveLocationLabel.Visible = true;
                        liveLinkContainer.Visible = true;
                        //btnTrack.Attributes["style"] = "display: block;";
                        gvDonations.Visible = false;

                        // progress bar
                        ClientScript.RegisterStartupScript(this.GetType(), "toggleProgressBar", "toggleProgressBar();", true);
                        return;  
                    }

                    // check if the role is 'organization'
                    if (Session["role"] != null && Session["role"].ToString() == "organization")
                    {
                        // check if both deliveryId and token are provided in the query string
                        if (!string.IsNullOrEmpty(deliveryId) && !string.IsNullOrEmpty(token))
                        {
                            QRY _Qry = new QRY();

                            string query = $"SELECT itemApproved, autoApproved FROM delivery WHERE deliveryId = '{deliveryId}'";
                            DataTable dt = _Qry.GetData(query);

                            if (dt.Rows.Count > 0)
                            {
                                // get itemApproved
                                var itemApprovedValue = dt.Rows[0]["itemApproved"];
                                var autoApproved= dt.Rows[0]["autoApproved"];

                                // check if itemApproved is 1
                                if (itemApprovedValue != DBNull.Value && Convert.ToInt32(itemApprovedValue) == 1)
                                {
                                    if (autoApproved != DBNull.Value && Convert.ToInt32(autoApproved) == 1)
                                    {
                                        // the system has auto-approved the item arrival
                                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire({ title: 'Auto-Approved', " +
                                            "text: 'The system has automatically approved the item arrival because it exceeds 7-day approval.', " +
                                            "icon: 'info', confirmButtonText: 'OK' });", true);
                                    }
                                    else
                                    {
                                        // the organization has already approved the item arrival
                                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire({ title: 'Already Verified', " +
                                            "text: 'The organization has already verified the item arrival.', icon: 'info', confirmButtonText: 'OK' });", true);
                                    }
                                }

                                else
                                {
                                    // fetch signature and signaturePIC from the organization table
                                    string signatureSql = "SELECT signature, signaturePIC FROM organization WHERE orgName = @username"; 

                                    var signatureParams = new Dictionary<string, object>
                                    {
                                        { "@username", username } 
                                    };

                                    DataTable signatureDt = _Qry.GetData(signatureSql, signatureParams);

                                    if (signatureDt.Rows.Count > 0)
                                    {
                                        var signature = signatureDt.Rows[0]["signature"];
                                        var signaturePIC = signatureDt.Rows[0]["signaturePIC"];

                                        // check if signature or signaturePIC is null or empty
                                        if (signature == DBNull.Value || signaturePIC == DBNull.Value || string.IsNullOrEmpty(signature.ToString()) || string.IsNullOrEmpty(signaturePIC.ToString()))
                                        {
                                            // ask organization to create a signature
                                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "Swal.fire({ title: 'Signature Required', text: 'Please create a signature for acknowledgment receipts.', icon: 'warning', confirmButtonText: 'Create Signature' }).then((result) => { if (result.isConfirmed) { window.location.href = 'OrgCreateSignature.aspx'; }});", true);
                                        }
                                        else
                                        {
                                            // if signature and signaturePIC are found
                                            approvalDiv.Attributes["style"] = "display: block;";
                                            lblDeliveryId.Text = deliveryId;

                                            string donationId2 = GetDonationId(deliveryId);
                                            string imgString = GetReachImageString(donationId2);
                                            string imagesHtml = ImageFileProcessing.ProcessImages(imgString);
                                            imageBox.InnerHtml = imagesHtml;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            // if no deliveryId and token
                            Organization org = new Organization(username, "", "", "", "");
                            string orgId = org.GetOrgId();

                            LoadDonationCount();
                            LoadOrder(orgId, "All");

                            navBar.Attributes["style"] = "display: block;";
                            gvDonations.Visible = true;
                        }
                    }
                }
                else
                {
                   
                }
            }
        }


        protected void LoadDelivery(string donationId)
        {
            // retrieve delivery details based on deliveryId
            QRY _Qry = new QRY();
            DataTable dtDelivery = new DataTable();
            string sqlQuery = $"SELECT deliveryStatus, riderId, pickupDate, pickupTime, pickupTimeByRider, acceptTimeByRider, reachTimeByRider, noteRider, noteOrg FROM delivery WHERE donationId = '{donationId}'";
            dtDelivery = _Qry.GetData(sqlQuery);
            string riderName = "";

            if (dtDelivery.Rows.Count > 0)
            {
                string deliveryStatus = dtDelivery.Rows[0]["deliveryStatus"].ToString();
                string riderId = dtDelivery.Rows[0]["riderId"] != DBNull.Value ? dtDelivery.Rows[0]["riderId"].ToString() : "No rider assigned yet.";
                string pickupDate = dtDelivery.Rows[0]["pickupDate"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["pickupDate"]).ToString("dd MMM yyyy") : "N/A";
                string scheduledTime = dtDelivery.Rows[0]["pickupTime"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["pickupTime"]).ToString("hh:mm") : "N/A";
                string acceptTime = dtDelivery.Rows[0]["acceptTimeByRider"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["acceptTimeByRider"]).ToString("dd MMM yyyy hh:mm") : "N/A";
                string pickupTime = dtDelivery.Rows[0]["pickupTimeByRider"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["pickupTimeByRider"]).ToString("dd MMM yyyy hh:mm") : "N/A";
                string reachTime = dtDelivery.Rows[0]["reachTimeByRider"] != DBNull.Value ? Convert.ToDateTime(dtDelivery.Rows[0]["reachTimeByRider"]).ToString("dd MMM yyyy hh:mm") : "N/A";

                string url = "";
                if (riderId != "No rider assigned yet.")
                {
                    riderName = GetRiderName(riderId);
                    url = $"PreviewPublicInfo.aspx?role=rider&username={riderName}";
                   
                }
                
                lblNote.Text= string.IsNullOrEmpty(dtDelivery.Rows[0]["noteRider"].ToString()) ? "-" : dtDelivery.Rows[0]["noteRider"].ToString();
                lblNote2.Text= string.IsNullOrEmpty(dtDelivery.Rows[0]["noteOrg"].ToString()) ? "-" : dtDelivery.Rows[0]["noteOrg"].ToString();

                donor.Attributes["style"] = "display: block;";
                org.Attributes["style"] = "display: block;";
                dateInfo.Attributes["style"] = "display: block;";
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
                        string sql = "SELECT pickupImg FROM delivery WHERE donationId = @donationId";
                        QRY _Qry2 = new QRY();
                        var parameters = new Dictionary<string, object>
                        {
                            { "@donationId", donationId }
                        };

                        DataTable dt = _Qry2.GetData(sql, parameters);

                        if (dt.Rows.Count > 0)
                        {
                            string pickupImg = dt.Rows[0]["pickupImg"].ToString();

                            if (!string.IsNullOrEmpty(pickupImg))
                            {
                                showPickupImg.Style["display"] = "block";
 
                            }
                            else
                            {

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
                        string sql2 = "SELECT pickupImg, reachImg, acknowledgmentReceiptPath FROM delivery WHERE donationId = @donationId";
                        QRY _Qry3 = new QRY();
                        var parameters2 = new Dictionary<string, object>
                        {
                            { "@donationId", donationId }
                        };

                        DataTable dt2 = _Qry3.GetData(sql2, parameters2);

                        if (dt2.Rows.Count > 0)
                        {
                            string reachImg = dt2.Rows[0]["reachImg"].ToString();
                            string receiptPath = dt2.Rows[0]["acknowledgmentReceiptPath"].ToString();
                            string pickupImg = dt2.Rows[0]["pickupImg"].ToString();

                            // decrypt the receipt file path
                            string decryptedReceiptFilePath = Encryption.Decrypt(receiptPath);

                            if (!string.IsNullOrEmpty(pickupImg))
                            {
                                showPickupImg.Style["display"] = "block";

                            }
                            else
                            {

                                showPickupImg.Style["display"] = "none";
                            }

                            if (!string.IsNullOrEmpty(reachImg))
                            {
                                showReachImg.Style["display"] = "block";
                            }
                            else
                            {
                                showReachImg.Style["display"] = "none";
                            }

                            if (!string.IsNullOrEmpty(receiptPath))
                            {
                                // link to download the acknowledgment receipt
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
                            // handle case where no delivery record is found 
                        }
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

        [WebMethod]
        public static string GetPickupImage(string donationId)
        {

            string encryptedBase64Images = GetPickupImageString(donationId);

            string imagesHtml = ImageFileProcessing.ProcessImages(encryptedBase64Images);

            return imagesHtml;
        }

        [WebMethod]
        public static string GetReachImage(string donationId)
        {

            string encryptedBase64Images = GetReachImageString(donationId);

            string imagesHtml = ImageFileProcessing.ProcessImages(encryptedBase64Images);

            return imagesHtml;
        }

        public static string GetPickupImageString(string donationId)
        {
            string img = "";

            QRY _Qry = new QRY();

            string sql = "SELECT pickupImg FROM delivery WHERE donationId = @donationId";
            var parameter = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                img = dt.Rows[0]["pickupImg"].ToString();
            }

            return img;
        }

        public static string GetReachImageString(string donationId)
        {
            string img = "";

            QRY _Qry = new QRY();

            string sql = "SELECT reachImg FROM delivery WHERE donationId = @donationId";
            var parameter = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt.Rows.Count > 0)
            {
                img = dt.Rows[0]["reachImg"].ToString();
            }

            return img;
        }

        protected static string GetDonationId(string deliveryId)
        {
            string donationId = "";

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

        protected void LoadOrder_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string status = btn.CommandArgument;
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            LoadOrder(orgId, status);

            lnkAll.CssClass = "nav-link";
            lnkWaiting.CssClass = "nav-link";
            lnkAccepted.CssClass = "nav-link";
            lnkDeliver.CssClass = "nav-link";
            lnkCompleted.CssClass = "nav-link";
            

            btn.CssClass = "nav-link active";
        }

        protected void LoadOrder(string orgId, string status)
        {
            string username = Session["username"].ToString();
            
            QRY _Qry = new QRY();
            DataTable dtDonations = new DataTable();
            string sqlQuery = "";

            if (status == "All")
            {
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickupAddress, 
                    d.destinationAddress, 
                    d.pickupDate,  
                    d.pickupTime,              
                    d.deliveryStatus, 
                    d.created_on,
                    dir.donorFullName, 
                    dir.donorPhone, 
                    dir.donorEmail
                FROM 
                    delivery d
                JOIN 
                    donation_item_request dir 
                    ON d.donationId = dir.donationId
                WHERE 
                    d.orgId = @orgId
                ORDER BY 
                    d.created_on DESC;


                ";
            }


            else if (status == "Waiting")
            {
                // Pending donations, excluding pickupDate and pickupTime
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickupAddress, 
                    d.destinationAddress, 
                    d.pickupDate,  
                    d.pickupTime,              
                    d.deliveryStatus, 
                    d.created_on,
                    dir.donorFullName, 
                    dir.donorPhone, 
                    dir.donorEmail
                FROM 
                    delivery d
                JOIN 
                    donation_item_request dir 
                    ON d.donationId = dir.donationId
                WHERE 
                    d.orgId = @orgId AND d.deliveryStatus= 'Waiting for delivery rider'
                ORDER BY 
                    d.created_on DESC;


                ";
            }

            else if (status == "Accepted")
            {
                // To Pay donations, with pickupDate and pickupTime included
                sqlQuery = @"
                SELECT 
                    d.donationId, 
                    d.pickupAddress, 
                    d.destinationAddress, 
                    d.pickupDate,  
                    d.pickupTime,              
                    d.deliveryStatus, 
                    d.created_on,
                    dir.donorFullName, 
                    dir.donorPhone, 
                    dir.donorEmail
                FROM 
                    delivery d
                JOIN 
                    donation_item_request dir 
                    ON d.donationId = dir.donationId
                WHERE 
                    d.orgId = @orgId AND d.deliveryStatus= 'Accepted'
                ORDER BY 
                    d.created_on DESC;


                ";
            }

            else if (status == "Delivering in progress")
            {
                // To Accept donations from the delivery table
                sqlQuery = @"
                 SELECT 
                    d.donationId, 
                    d.pickupAddress, 
                    d.destinationAddress, 
                    d.pickupDate,  
                    d.pickupTime,              
                    d.deliveryStatus, 
                    d.created_on,
                    dir.donorFullName, 
                    dir.donorPhone, 
                    dir.donorEmail
                FROM 
                    delivery d
                JOIN 
                    donation_item_request dir 
                    ON d.donationId = dir.donationId
                WHERE 
                    d.orgId = @orgId AND d.deliveryStatus= 'Delivering in progress'
                ORDER BY 
                    d.created_on DESC;

                ";
            }

            else if (status == "Reached Destination")
            {
                // To PickUp donations
                sqlQuery = @"
                 SELECT 
                    d.donationId, 
                    d.pickupAddress, 
                    d.destinationAddress, 
                    d.pickupDate,  
                    d.pickupTime,              
                    d.deliveryStatus, 
                    d.created_on,
                    dir.donorFullName, 
                    dir.donorPhone, 
                    dir.donorEmail
                FROM 
                    delivery d
                JOIN 
                    donation_item_request dir 
                    ON d.donationId = dir.donationId
                WHERE 
                    d.orgId = @orgId AND d.deliveryStatus= 'Reached Destination'
                ORDER BY 
                    d.created_on DESC;

                ";
            }
           

            var parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            dtDonations = _Qry.GetData(sqlQuery, parameters);

            if (dtDonations.Rows.Count == 0)
            {
                noDataLabel.Visible = true;
                gvDonations.Visible = false;
            }
            else
            {
                noDataLabel.Visible = false;
                gvDonations.Visible = true;
                gvDonations.DataSource = dtDonations;
                gvDonations.DataBind();
            }
        }

        protected string GetStatus(string status)
        {
            switch (status.Trim().ToLower())
            {
                case "waiting for delivery rider":
                    return "status-waiting";
                case "accepted":
                    return "status-accepted";
                case "delivering in progress":
                    return "status-delivering";
                case "reached destination":
                    return "status-completed";
                case "cancelled":
                    return "status-cancel";
                default:
                    return "";
            }
        }

        protected void LoadDonationCount()
        {
            // counts 
            int waitingCount = GetDonationCountByStatus("Waiting for delivery rider");
            int acceptCount = GetDonationCountByStatus("Accepted");
            int deliverCount = GetDonationCountByStatus("Delivering in progress");        
            int completedCount = GetDonationCountByStatus("Reached Destination");

            // update the text of the LinkButtons to include the count in bold
            lnkWaiting.Text = "Waiting" + (waitingCount > 0 ? $" (<strong>{waitingCount}</strong>)" : "");
            lnkAccepted.Text = "Accepted" + (acceptCount > 0 ? $" (<strong>{acceptCount}</strong>)" : "");
            lnkDeliver.Text = "Delivering" + (deliverCount > 0 ? $" (<strong>{deliverCount}</strong>)" : "");
            lnkCompleted.Text = "Completed" + (completedCount > 0 ? $" (<strong>{completedCount}</strong>)" : "");
            
        }




        private int GetDonationCountByStatus(string status)
        {
            string sqlQuery = "";
            Dictionary<string, object> parameter = new Dictionary<string, object>();

            string username = Session["username"].ToString();

            QRY _Qry = new QRY();

            Organization org = new Organization(username, "", "", "", "");
            string id = org.GetOrgId();           

            if (status == "Waiting for delivery rider")
            {
                // count 'To Pay' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d            
                WHERE d.deliveryStatus = 'Waiting for delivery rider' 
                AND d.orgId = @orgId";

                parameter.Add("@orgId", id);
            }
            else if (status == "Accepted")
            {
                // count 'To Accept' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d            
                WHERE d.deliveryStatus = 'Accepted' 
                AND d.orgId = @orgId";

                parameter.Add("@orgId", id);
            }
            else if (status == "Delivering in progress")
            {
                // count 'To PickUp' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d            
                WHERE d.deliveryStatus = 'Delivering in progress' 
                AND d.orgId = @orgId";

                parameter.Add("@orgId", id);
            }
            else if (status == "Reached Destination")
            {
                // count 'To Reach' donations
                sqlQuery = @"
                SELECT COUNT(*) 
                FROM delivery d            
                WHERE d.deliveryStatus = 'Reached Destination' 
                AND d.orgId = @orgId";

                parameter.Add("@orgId", id);
            }
         
            QRY _Qry2 = new QRY();
            DataTable dt2 = _Qry2.GetData(sqlQuery, parameter);

            if (dt2.Rows.Count > 0)
            {
                return Convert.ToInt32(dt2.Rows[0][0]);
            }
            else
            {
                return 0;
            }
        }

        private void LoadDonationItems(string donationId, PlaceHolder phDonationItems)
        {
            // fetch the donation items based on the donationId
            string query = @"
            WITH SplitItems AS (
                SELECT 
                    dir.donationId,
                    dir.itemCategory,
                    TRIM(value) AS item,
                    ROW_NUMBER() OVER (PARTITION BY dir.donationId, dir.itemCategory ORDER BY (SELECT NULL)) AS rowNum
                FROM 
                    donation_item_request dir
                CROSS APPLY 
                    STRING_SPLIT(dir.item, ',')
            ),
            SplitQuantities AS (
                SELECT 
                    dir.donationId,
                    dir.itemCategory,
                    TRIM(REPLACE(value, '(', '')) AS quantityDonated,
                    ROW_NUMBER() OVER (PARTITION BY dir.donationId, dir.itemCategory ORDER BY (SELECT NULL)) AS rowNum
                FROM 
                    donation_item_request dir
                CROSS APPLY 
                    STRING_SPLIT(REPLACE(dir.quantityDonated, ')', ''), ',')
            )
            SELECT 
                si.itemCategory,
                si.item,
                sq.quantityDonated,
                die.expiryDate,
                die.quantityWithSameExpiryDate
            FROM 
                SplitItems si
            JOIN 
                SplitQuantities sq
                ON si.donationId = sq.donationId 
                AND si.itemCategory = sq.itemCategory
                AND si.rowNum = sq.rowNum
            LEFT JOIN 
                donation_item_expiry_date die 
                ON si.donationId = die.donationId 
                AND si.itemCategory = die.itemCategory
                AND si.item = die.item2
            WHERE 
                si.donationId = @donationId
            ORDER BY 
                si.itemCategory, si.item, die.expiryDate";

            QRY qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };
            DataTable dt = qry.GetData(query, parameters);

            if (dt.Rows.Count > 0)
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='categoryDetailsTable' style='margin: auto;'>");
                sb.Append("<thead><tr><th>Category</th><th>Item</th><th>Quantity</th><th>Expiry Date (Quantity)</th></tr></thead>");
                sb.Append("<tbody>");

                string currentCategory = "";

                foreach (DataRow row in dt.Rows)
                {
                    string category = row["itemCategory"].ToString();
                    string item = row["item"].ToString();
                    string expiryDate = row["expiryDate"] == DBNull.Value ? "N/A" : Convert.ToDateTime(row["expiryDate"]).ToString("yyyy-MM-dd");
                    string expiryQuantity = row["quantityWithSameExpiryDate"] == DBNull.Value ? "N/A" : row["quantityWithSameExpiryDate"].ToString();

                    // if there is no expiry date, show the donated quantity from the original request
                    string quantityToDisplay = expiryDate == "N/A" ? row["quantityDonated"].ToString() : expiryQuantity;

                    // display category header once per category
                    if (currentCategory != category)
                    {
                        sb.AppendFormat("<tr><td colspan='5'><strong>{0}</strong></td></tr>", category);
                        currentCategory = category;
                    }

                    sb.Append("<tr>");
                    sb.AppendFormat("<td></td>");
                    sb.AppendFormat("<td>{0}</td>", item);
                    sb.AppendFormat("<td>{0}</td>", quantityToDisplay);
                    sb.AppendFormat("<td>{0}</td>", expiryDate);


                    sb.Append("</tr>");
                }

                sb.Append("</tbody></table>");


                phDonationItems.Controls.Add(new Literal { Text = sb.ToString() });
            }
            else
            {

                phDonationItems.Controls.Add(new Literal { Text = "<p>No items found for this donation.</p>" });
            }
        }

        protected void gvDonations_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // get the donationId for the current row
                string donationId = DataBinder.Eval(e.Row.DataItem, "donationId").ToString();

                // find the PlaceHolder control inside the current row
                PlaceHolder phDonationItems = (PlaceHolder)e.Row.FindControl("phDonationItems");

                // call the method to load the donation items for this donationId
                LoadDonationItems(donationId, phDonationItems);

            }
        }

        protected void btnViewDelivery_Click(object sender, EventArgs e)
        {
            string donationId = ((Button)sender).CommandArgument;

            Response.Redirect($"Delivery.aspx?donationId={donationId}");

        }

        [WebMethod]
        public static string GenerateReceipt(string deliveryId)
        {
            // fetch orgId and donationId 
            var deliveryDetail = GetDeliveryDetails(deliveryId);
            string orgId = deliveryDetail.orgId;
            string donationId = deliveryDetail.donationId;

            // fetch organization details 
            var orgDetail = GetOrganizationDetails(orgId);

            // fetch donor and donation details 
            var donationDetail = GetDonationDetails(donationId);

            // path where the receipt will be saved
            string folderPath = HttpContext.Current.Server.MapPath("~/AcknowledgementReceipt/");
            string filePath = folderPath + deliveryId + "_AcknowledgementReceipt.pdf";
            string fileUrl = "/AcknowledgementReceipt/" + deliveryId + "_AcknowledgementReceipt.pdf";

            // create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document document = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Fonts
                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Font footerFontSize = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.DARK_GRAY);

                // Title
                Paragraph title = new Paragraph("Item Donation Receipt Acknowledgement Letter", titleFont);
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                // Line break
                document.Add(new Paragraph("\n"));

                // Organization Details 
                Paragraph orgDetailsParagraph = new Paragraph();
                orgDetailsParagraph.Add(new Chunk(orgDetail.orgName + "\n", smallFont));
                orgDetailsParagraph.Add(new Chunk(orgDetail.orgAddress + "\n", smallFont));
                orgDetailsParagraph.Add(new Chunk(orgDetail.orgRegion + "\n", smallFont));
                document.Add(orgDetailsParagraph);

                document.Add(new Paragraph("\n"));

                // Date 
                Paragraph date = new Paragraph(DateTime.Now.ToString("MMMM dd, yyyy"), smallFont);
                document.Add(date);

                document.Add(new Paragraph("\n"));

                // Donor Details
                Paragraph donorDetails = new Paragraph();
                donorDetails.Add(new Chunk(donationDetail.donorFullName + "\n", smallFont));
                donorDetails.Add(new Chunk(donationDetail.pickupAddress + "\n", smallFont));
                donorDetails.Add(new Chunk(donationDetail.state + "\n", smallFont));
                document.Add(donorDetails);

                document.Add(new Paragraph("\n"));

                document.Add(new Paragraph("\n"));

                // Donation Details
                Paragraph body = new Paragraph();
                body.Add(new Chunk("Dear " + donationDetail.donorFullName + ",\n\n", boldFont));
                body.Add(new Chunk("Thank you for your contribution of ", normalFont));
                body.Add(new Chunk(donationDetail.itemCategory + ".\n\n", boldFont));
                body.Add(new Chunk("Please keep this written acknowledgment of your donated items for your records. This receipt serves as proof of your in-kind donation.\n\n", normalFont));
                body.Add(new Chunk("We appreciate your contribution and thank you for your support to ", normalFont));
                body.Add(new Chunk(orgDetail.orgName + ".\n\n", boldFont));
                document.Add(body);

                document.Add(new Paragraph("\n"));

                Paragraph signaturePart = new Paragraph();
                signaturePart.Add(new Chunk("Yours sincerely,\n\n", normalFont));
                document.Add(signaturePart);

                string base64Image = ImageFileProcessing.DecryptImages(orgDetail.signature); // decrypt and get the base64 string

                if (!string.IsNullOrEmpty(base64Image))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(base64Image);

                        using (var memoryStream = new MemoryStream(imageBytes))
                        {
                            iTextSharp.text.Image signatureImage = iTextSharp.text.Image.GetInstance(memoryStream);

                            signatureImage.ScaleToFit(100f, 80f);

                            document.Add(signatureImage);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error adding image: " + ex.Message);
                    }
                }


                // add PIC below signature
                Paragraph PIC = new Paragraph();
                PIC.Add(new Chunk("\n(" + orgDetail.PIC + ")\n", boldFont));
                document.Add(PIC);

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));

                Paragraph autoGeneratedFooter = new Paragraph("This receipt is auto-generated by the system.", footerFontSize);
                autoGeneratedFooter.Alignment = Element.ALIGN_CENTER;
                autoGeneratedFooter.SpacingAfter = 5f;
                document.Add(autoGeneratedFooter);

                int currentYear = DateTime.Now.Year;
                Paragraph footer = new Paragraph($"© {currentYear} DonorConnect", footerFontSize);
                footer.Alignment = Element.ALIGN_CENTER;
                document.Add(footer);

                // Close document
                document.Close();
                writer.Close();
                fs.Close();
            }

            // update itemApproved to 1 and save filepath after receipt is generated
            QRY _Qry = new QRY();

            string receiptFileUrl = Encryption.Encrypt(fileUrl);

            string updateSql = $"UPDATE delivery SET itemApproved = 1, acknowledgmentReceiptPath = '{receiptFileUrl}' WHERE deliveryId = '{deliveryId}'";
            _Qry.ExecuteNonQuery(updateSql);

            QRY _Qry2 = new QRY();

            string sql = $"SELECT donorId, donationId, paymentAmount, riderId, earningReceiptPath FROM delivery WHERE deliveryId = '{deliveryId}'";

            string donorId = "";
            string paymentAmount = "";
            string riderId = "";
            string earningPath = "";
            DataTable dt = _Qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                donorId = dt.Rows[0]["donorId"].ToString();
                donationId = dt.Rows[0]["donationId"].ToString();
                paymentAmount = dt.Rows[0]["paymentAmount"].ToString();
                riderId = dt.Rows[0]["riderId"].ToString();
                earningPath= dt.Rows[0]["earningReceiptPath"].ToString();
            }

            string token = Hash(donorId + DateTime.Now.ToString());
            token = token.Replace("+", "A");
            string fullLink = "https://localhost:44390/" + fileUrl;

            string encryptedLink = Encryption.Encrypt(fullLink);

            string message = $"Your acknowledgement receipt for {deliveryId} is here or you can view it at the Delivery Page";

            string sqlNtf = "EXEC [create_notifications] " +
            "@method = 'INSERT', " +
            "@id = NULL, " +
            "@userId = '" + donorId + "', " +
            "@link = '" + encryptedLink + "', " +
            "@supportingId = '" + deliveryId + "', " +
            "@content = '" + message + "' ";

            _Qry2.ExecuteNonQuery(sqlNtf);

            string fileUrl2= "/RiderPaymentReceipt/" + deliveryId + "_RiderPaymentReceipt.pdf";
            string fullLink2 = "https://localhost:44390/" + fileUrl2;

            string encryptedLink2 = Encryption.Encrypt(fullLink2);

            string message2 = $"Your earnings statement for {deliveryId} is here or you can view it at the Dashboard";

            string sqlNtf2 = "EXEC [create_notifications] " +
            "@method = 'INSERT', " +
            "@id = NULL, " +
            "@userId = '" + riderId + "', " +
            "@link = '" + encryptedLink2 + "', " +
            "@supportingId = '" + deliveryId + "', " +
            "@content = '" + message2 + "' ";

            _Qry2.ExecuteNonQuery(sqlNtf2);

            string originalPath= Encryption.Decrypt(earningPath);

            // send email to notify organization
            string sqlemail = "EXEC [application_email] " +
                              "@action = 'EARNINGS STATEMENT', " +
                              "@role = 'rider', " +
                              "@path = @path, " +
                              "@riderId = @riderId, " +
                              "@donationpublishid = @deliveryId";

            var emailParameter = new Dictionary<string, object>
                {
                    { "@path", originalPath },
                    { "@riderId", riderId },
                    { "@deliveryId", deliveryId }
                };

            _Qry.ExecuteNonQuery(sqlemail, emailParameter);

            string sqlGet = "SELECT walletAmount FROM delivery_rider WHERE riderId = @riderId";
            var parameterGet = new Dictionary<string, object>
            {
                { "@riderId", riderId }
            };

            QRY _QryGet = new QRY();
            DataTable dt2 = _QryGet.GetData(sqlGet, parameterGet);

            if (dt2.Rows.Count > 0)
            {
                // extract the "RM" prefix and convert the current wallet amount from varchar to decimal
                string currentWalletStr = dt2.Rows[0]["walletAmount"].ToString();
                decimal currentWalletAmount = 0;

                // remove "RM" and any whitespace
                currentWalletStr = currentWalletStr.Replace("RM", "").Trim();

                if (!decimal.TryParse(currentWalletStr, out currentWalletAmount))
                {
                    throw new Exception("Failed to convert current wallet amount to decimal.");
                }

                // extract the "RM" prefix and convert the payment amount from varchar to decimal
                string paymentAmountStr = paymentAmount.ToString();
                paymentAmountStr = paymentAmountStr.Replace("RM", "").Trim();
                decimal paymentAmountDecimal = 0;

                if (!decimal.TryParse(paymentAmountStr, out paymentAmountDecimal))
                {
                   
                    throw new Exception("Failed to convert payment amount to decimal.");
                }

                // add the payment amount to the current wallet amount
                decimal newWalletAmount = currentWalletAmount + paymentAmountDecimal;

                // update the wallet amount 
                string sqlUpdate = "UPDATE delivery_rider SET walletAmount = @newWalletAmount WHERE riderId = @riderId";
                var parameterUpdate = new Dictionary<string, object>
                {
                    { "@newWalletAmount", "RM " + newWalletAmount.ToString("F2") }, 
                    { "@riderId", riderId }
                };

                QRY _QryUpdate = new QRY();
                _QryUpdate.ExecuteNonQuery(sqlUpdate, parameterUpdate);

                string fullLink3 = $"https://localhost:44390/RiderWallet.aspx?riderId={riderId}";
                string encryptedLink3 = Encryption.Encrypt(fullLink3);

                string message3 = $"Your wallet has been credited with RM{paymentAmountDecimal:F2}.";
                
                string sqlNtf3 = "EXEC [create_notifications] " +
                "@method = 'INSERT', " +
                "@id = NULL, " +
                "@userId = '" + riderId + "', " +
                "@link = '" + encryptedLink3 + "', " +
                "@supportingId = '" + deliveryId + "', " +
                "@content = '" + message3 + "' ";

                _Qry2.ExecuteNonQuery(sqlNtf3);
            }
            else
            {
                throw new Exception("Rider not found.");
            }


            string sqlCheckDonationItems = $@"
            SELECT dir.donationId, dir.itemCategory, dir.item, dir.quantityDonated, dir.orgId 
            FROM donation_item_request dir
            LEFT JOIN itemCategory ic ON dir.itemCategory = ic.categoryName
            WHERE dir.donationId = '{donationId}'";

            DataTable dtDonationItems = _Qry.GetData(sqlCheckDonationItems);

            if (dtDonationItems.Rows.Count > 0)
            {
                foreach (DataRow row in dtDonationItems.Rows)
                {
                    string itemCategory = row["itemCategory"].ToString();

                    string itemCategoryTemp = "";

                    if (itemCategory.Contains("Food"))
                    {
                        itemCategoryTemp = "Food";
                    }
                    else
                    {
                        itemCategoryTemp = itemCategory;
                    }

                    string items = row["item"].ToString();
                    string quantities = row["quantityDonated"].ToString();
                    string id = row["orgId"].ToString();

                    // check if the itemCategory has expiry date
                    string sqlCheckHasExpiry = $@"
                    SELECT hasExpiryDate FROM itemCategory WHERE categoryName = '{itemCategoryTemp}'";

                    string hasExpiryDate = _Qry.GetScalarValue(sqlCheckHasExpiry)?.ToString();

                    // if the category does not have expiry date, insert into inventory
                    if (hasExpiryDate == "No")
                    {
                        string[] itemArray = items.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] quantityArray = quantities.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        if (itemArray.Length == quantityArray.Length)
                        {
                            for (int i = 0; i < itemArray.Length; i++)
                            {
                                // remove brackets and trim any whitespace
                                string cleanedItem = itemArray[i].Trim(new[] { '[', ']', '(', ')' }).Trim();
                                string cleanedQuantity = quantityArray[i].Trim(new[] { '[', ']', '(', ')' }).Trim();

                                // insert the item into the inventory
                                string createInventoryItemSql = @"
                                EXEC create_inventory_item 
                                    @id = @id,
                                    @method= @method,
                                    @donationId = @donationId, 
                                    @itemCategory = @itemCategory, 
                                    @item = @item, 
                                    @quantity = @quantity, 
                                    @orgId = @orgId";

                                Dictionary<string, object> parameters = new Dictionary<string, object>
                                {
                                    { "@id", DBNull.Value },
                                    { "@method", "INSERT" },
                                    { "@donationId", row["donationId"] },
                                    { "@itemCategory", itemCategory },
                                    { "@item", cleanedItem },  // with brackets removed
                                    { "@quantity", cleanedQuantity },  // with brackets removed
                                    { "@orgId", id }
                                };

                                _Qry.ExecuteNonQuery(createInventoryItemSql, parameters);
                            }
                        

                    }
                }
                    // if the category has expiry date
                    else if (hasExpiryDate == "Yes")
                    {
                        // split the items and quantities
                        string[] itemArray = items.Split(new[] { ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                        string[] quantityArray = quantities.Split(new[] { ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                        // fetch the number of rows table for this category and donationId
                        string sqlCheckExpiryCount = $@"
                        SELECT COUNT(*) FROM donation_item_expiry_date 
                        WHERE donationId = '{donationId}' AND itemCategory = '{itemCategory}'";

                        int expiryRowCount = Convert.ToInt32(_Qry.GetScalarValue(sqlCheckExpiryCount));

                        // compare the item count with the expiryRowCount
                        if (itemArray.Length >= expiryRowCount)
                        {
                            // if all items with expiry dates, insert them into inventory
                            DataTable dtExpiryItems = _Qry.GetData($@"
                            SELECT item2, quantityWithSameExpiryDate, expiryDate 
                            FROM donation_item_expiry_date 
                            WHERE donationId = '{donationId}' AND itemCategory = '{itemCategory}'");

                            List<string> expiryItems = new List<string>();
                            foreach (DataRow expiryRow in dtExpiryItems.Rows)
                            {
                                string createInventoryItemSql = @"
                                EXEC create_inventory_item 
                                    @id = @id,
                                    @method= @method,
                                    @donationId = @donationId, 
                                    @itemCategory = @itemCategory, 
                                    @item = @item, 
                                    @quantity = @quantityWithSameExpiryDate, 
                                    @expiryDate = @expiryDate, 
                                    @orgId = @orgId";

                                Dictionary<string, object> parameters = new Dictionary<string, object>
                                {
                                    { "@id", DBNull.Value },
                                    { "@method", "INSERT" },
                                    { "@donationId", row["donationId"] },
                                    { "@itemCategory", itemCategory },
                                    { "@item", expiryRow["item2"].ToString().Trim() },  
                                    { "@quantityWithSameExpiryDate", expiryRow["quantityWithSameExpiryDate"].ToString().Trim() },
                                    { "@expiryDate", expiryRow["expiryDate"] },
                                    { "@orgId", id }
                                };

                                _Qry.ExecuteNonQuery(createInventoryItemSql, parameters);
                                expiryItems.Add(expiryRow["item2"].ToString().Trim());  
                            }

                            // get items that are missing expiry dates and insert them
                            for (int i = 0; i < itemArray.Length; i++)
                            {
                                string cleanedItem = itemArray[i].Trim(new[] { '(', ')' }).Trim(); 
                                string cleanedQty= quantityArray[i].Trim(new[] { '(', ')' }).Trim();

                                // if the item is not in the list of items with expiry dates, insert without expiry date
                                if (!expiryItems.Contains(cleanedItem))
                                {
                                    // insert items without expiry dates
                                    string createInventoryItemSql = @"
                                    EXEC create_inventory_item 
                                        @id = @id,
                                        @method= @method,
                                        @donationId = @donationId, 
                                        @itemCategory = @itemCategory, 
                                        @item = @item, 
                                        @quantity = @quantity, 
                                        @orgId = @orgId";

                                    Dictionary<string, object> parameters = new Dictionary<string, object>
                                    {
                                        { "@id", DBNull.Value },
                                        { "@method", "INSERT" },
                                        { "@donationId", row["donationId"] },
                                        { "@itemCategory", itemCategory },
                                        { "@item", cleanedItem },  
                                        { "@quantity", cleanedQty },  
                                        { "@orgId", id }
                                    };

                                    _Qry.ExecuteNonQuery(createInventoryItemSql, parameters);
                                }
                            }
                        }
                    }

                }
            }


                    return fileUrl;
        }



        public static DeliveryDetail GetDeliveryDetails(string deliveryId)
        {
            DeliveryDetail deliveryDetail = new DeliveryDetail();

            QRY _Qry = new QRY();
    
            string sql = $"SELECT orgId, donationId FROM delivery WHERE deliveryId = '{deliveryId}'";

            DataTable dt = _Qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                deliveryDetail.orgId = dt.Rows[0]["orgId"].ToString();
                deliveryDetail.donationId = dt.Rows[0]["donationId"].ToString();
            }

            return deliveryDetail;
        }

        public static OrganizationDetail GetOrganizationDetails(string orgId)
        {
            OrganizationDetail orgDetail = new OrganizationDetail();

            QRY _Qry = new QRY();

            string sql = $"SELECT orgName, orgAddress, orgRegion, signature, signaturePIC FROM organization WHERE orgId = '{orgId}'";

            DataTable dt = _Qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                orgDetail.orgName = dt.Rows[0]["orgName"].ToString();
                orgDetail.orgAddress = dt.Rows[0]["orgAddress"].ToString();
                orgDetail.orgRegion = dt.Rows[0]["orgRegion"].ToString();
                orgDetail.signature = dt.Rows[0]["signature"].ToString();
                orgDetail.PIC = dt.Rows[0]["signaturePIC"].ToString();

            }

            return orgDetail;
        }

        public static DonationDetail GetDonationDetails(string donationId)
        {
            DonationDetail donationDetail = new DonationDetail();

            QRY _Qry = new QRY();
     
            string sql = $"SELECT donorFullName, pickUpAddress, state, itemCategory FROM donation_item_request WHERE donationId = '{donationId}'";

            DataTable dt = _Qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
                donationDetail.donorFullName = dt.Rows[0]["donorFullName"].ToString();
                donationDetail.pickupAddress = dt.Rows[0]["pickUpAddress"].ToString();
                donationDetail.state = dt.Rows[0]["state"].ToString();

                List<string> itemCategories = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    itemCategories.Add(row["itemCategory"].ToString());
                }

                // join the item categories with a comma 
                donationDetail.itemCategory = string.Join(", ", itemCategories);
            }


            return donationDetail;
        }

        [WebMethod]
        public static void UpdateItemApproved(string deliveryId)
        {
            // update the itemApproved column to 1
            QRY _Qry = new QRY();
            string updateSql = $"UPDATE delivery SET itemApproved = 1 WHERE deliveryId = '{deliveryId}'";

            _Qry.ExecuteNonQuery(updateSql);
        }
        static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        protected void LoadDetails(string donationId)
        {
            try
            {
                QRY _Qry = new QRY();

                string deliverySql = "SELECT pickupAddress, destinationAddress FROM delivery WHERE donationId = @donationId";
                var deliveryParams = new Dictionary<string, object> { { "@donationId", donationId } };
                DataTable deliveryData = _Qry.GetData(deliverySql, deliveryParams);

                if (deliveryData.Rows.Count > 0)
                {
                    lblDonorAddress.Text = deliveryData.Rows[0]["pickupAddress"].ToString();
                    lblOrgAddress.Text = deliveryData.Rows[0]["destinationAddress"].ToString();

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
                    Console.WriteLine("Delivery details not found for donationId: " + donationId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading details: " + ex.Message);
            }
        }

        [WebMethod]
        public static string GetRiderLocation(string donationId)
        {
            try
            {
                QRY _Qry = new QRY();

                // get deliveryId based on donationId
                string deliveryIdQuery = "SELECT deliveryId FROM delivery WHERE donationId = @donationId";
                var deliveryIdParam = new Dictionary<string, object> { { "@donationId", donationId } };

                string deliveryId = _Qry.ExecuteScalar(deliveryIdQuery, deliveryIdParam)?.ToString();

                if (string.IsNullOrEmpty(deliveryId))
                {
                    return JsonConvert.SerializeObject(new { success = false, message = "Delivery ID not found for this donation." });
                }

                // use deliveryId to fetch latitude, longitude, and lastUpdated
                string latitudeSql = "SELECT latitude FROM rider_location WHERE deliveryId = @deliveryId";
                string longitudeSql = "SELECT longitude FROM rider_location WHERE deliveryId = @deliveryId";
                string lastUpdatedSql = "SELECT lastUpdated FROM rider_location WHERE deliveryId = @deliveryId";
                var parameter = new Dictionary<string, object> { { "@deliveryId", deliveryId } };

                double latitude = Convert.ToDouble(_Qry.ExecuteScalar(latitudeSql, parameter) ?? 0.0);
                double longitude = Convert.ToDouble(_Qry.ExecuteScalar(longitudeSql, parameter) ?? 0.0);
                DateTime lastUpdated = Convert.ToDateTime(_Qry.ExecuteScalar(lastUpdatedSql, parameter) ?? DateTime.MinValue);

                // check if coordinates are valid
                if (latitude == 0.0 && longitude == 0.0)
                {
                    return JsonConvert.SerializeObject(new { success = false, message = "Location not found." });
                }

                // return the location data as JSON
                return JsonConvert.SerializeObject(new { success = true, lat = latitude, lng = longitude, lastUpdated = lastUpdated });
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { success = false, message = ex.Message });
            }
        }


        private void LoadLiveLink(string donationId)
        {
            try
            {
                QRY _Qry = new QRY();
                string sql = "SELECT liveLink FROM delivery WHERE donationId = @donationId";
                var parameter = new Dictionary<string, object> { { "@donationId", donationId } };

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

        [WebMethod]
        public static object GetDeliveryAddress(string donationId)
        {
            var deliveryDetails = new Dictionary<string, string>();
            DataTable dt = FetchDeliveryData(donationId);

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



        private static DataTable FetchDeliveryData(string donationId)
        {
            string sql = "SELECT pickupAddress, destinationAddress FROM delivery WHERE donationId = @donationId";
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            QRY _Qry = new QRY();
            return _Qry.GetData(sql, parameters);
        }
    }
}