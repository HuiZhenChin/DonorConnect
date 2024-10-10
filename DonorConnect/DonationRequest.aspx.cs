using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.PeerToPeer;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static DonorConnect.AllDonations;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Emit;
using iTextSharp.text.pdf.qrcode;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace DonorConnect
{
    public partial class DonationRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["username"] == null || string.IsNullOrEmpty(Session["username"].ToString()))
            {
                // if username is not present, redirect to login page or show an alert
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showAlert('Please login to your account to access the page.');", true);
                hfCurrentSlide.Value = "0";
                UpdateSlideContent(0);
                return;
            }

            if (!IsPostBack)
            {
                // get donationPublishId and donationId from query string
                string donationPublishId = Request.QueryString["donationPublishId"];
                string encryptedDonationId = Request.QueryString["donationId"];
                string encryptedDonationPublishId = Request.QueryString["donationPublishId"];
                string username = Session["username"].ToString();

                if (!string.IsNullOrEmpty(donationPublishId) || !string.IsNullOrEmpty(encryptedDonationId))
                {
                    List<string> categoriesWithExpiryDate = GetCategoriesWithExpiryDate();
                    hfCategoriesWithExpiryDate.Value = new JavaScriptSerializer().Serialize(categoriesWithExpiryDate);

                    // check if both donationPublishId and encryptedDonationId are available
                    if (!string.IsNullOrEmpty(encryptedDonationId))
                    {
                        try
                        {
                            // check the status of the donation item request in the database
                            string sqlCheckStatus = "SELECT requestStatus, paymentStatus FROM donation_item_request WHERE donationId = @donationId";
                            var parameters = new Dictionary<string, object>
                            {
                                { "@donationId", encryptedDonationId }
                            };

                            QRY _Qry = new QRY();
                            DataTable dt = _Qry.GetData(sqlCheckStatus, parameters);

                            if (dt.Rows.Count > 0)
                            {
                                string requestStatus = dt.Rows[0]["requestStatus"].ToString();
                                string paymentStatus = dt.Rows[0]["paymentStatus"].ToString();

                                // if the status is "Cancelled", show SweetAlert and prompt for resubmission
                                if (requestStatus == "Cancelled")
                                {
                                    hfCurrentSlide.Value = "0";
                                    UpdateSlideContent(0);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                                    Swal.fire({
                                        icon: 'warning',
                                        title: 'Request Cancelled',
                                        text: 'This donation request is not available anymore as it has been cancelled by you previously. If you would like to donate again, please resubmit the request.',
                                        confirmButtonText: 'OK'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                           
                                            window.location.href = 'DonationRequest.aspx?donationPublishId=" + donationPublishId + @"';
                                        }
                                    });
                                ", true);
                                    return; 
                                }

                                if (paymentStatus == "Approved")
                                {
                                    hfCurrentSlide.Value = "0";
                                    UpdateSlideContent(0);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                                    Swal.fire({
                                        icon: 'warning',
                                        title: 'Payment Done',
                                        text: 'This donation request is alreay paid with delivery fee payment. It cannot be resubmitted again.',
                                        confirmButtonText: 'OK'
                                    }).then((result) => {
                                        if (result.isConfirmed) {
                                            
                                            window.location.href = 'Home.aspx';
                                        }
                                    });
                                ", true);
                                    return; 
                                }
                            }
                            // if both donationPublishId and donationId are present, display slide 2 (hfCurrentSlide = 2)
                            hfCurrentSlide.Value = "2";
                            formContainer1.Visible = false;
                            formContainer2.Visible = false;
                            UpdateSlideContent(2);
                           
                            return;

                        }
                        catch (Exception ex)
                        {
                            // handle decryption error
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Invalid or corrupted link.');", true);
                            return;
                        }
                    }
                    else
                    {
                        // if only donationPublishId is present, display the first slide (hfCurrentSlide = 0)
                        hfCurrentSlide.Value = "0";
                        UpdateSlideContent(0);
                    }
                }
               
            }
        }



        public class DonationItemRequest
        {
            public string category { get; set; }
            public string items { get; set; }
            public string quantities { get; set; }
            
        }

        public class DonationItemExpiryDate
        {
         
            public string category { get; set; }
            public string item { get; set; }
            public string quantity { get; set; }
            public string totalQuantity { get; set; }
            public string quantityWithSameExpiryDate { get; set; }
            public string expiryDate { get; set; }
            
        }
        public class PersonalDetails
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string State { get; set; }

        }


        protected void rptCategoryDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {


            }
        }
        private void LoadDonationDetails(string donationPublishId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            string sql = "SELECT title, itemCategory, orgId FROM donation_publish WHERE donationPublishId = @donationPublishId";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@donationPublishId", donationPublishId }
            };

      
            _dt = _Qry.GetData(sql, parameter);
            if (_dt.Rows.Count > 0)
            {

                lblDonationTitle.Text = _dt.Rows[0]["title"].ToString();

                string orgId = _dt.Rows[0]["orgId"].ToString();
                Organization org = new Organization("", orgId, "", "", "");            
                string orgName = org.GetOrgName();
                lblOrgName.Text = orgName;

                // split item categories 
                string itemCategories = _dt.Rows[0]["itemCategory"].ToString();

                DataTable itemCategoryTable = new DataTable();
                itemCategoryTable.Columns.Add("Category");

                // remove any unwanted characters from the category string and split it
                string[] categories = RemoveString(itemCategories).Split(',');

                foreach (string category in categories)
                {
                    // check if the category is "Food"
                    if (category.Trim().Equals("Food", StringComparison.OrdinalIgnoreCase))
                    {
                        // if it's "Food," add two separate rows for "Cooked Food" and "Canned/Packed Food"
                        itemCategoryTable.Rows.Add("Food (Cooked Food)");
                        itemCategoryTable.Rows.Add("Food (Canned/Packed Food)");
                    }
                    else
                    {
                        // for all other categories, add them as it is
                        itemCategoryTable.Rows.Add(category.Trim());
                    }
                }

                rptCategoryDetails.DataSource = itemCategoryTable;
                rptCategoryDetails.DataBind();
            }
        }

        private string RemoveString(string input)
        {
            return input.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Trim();
        }

        private void LoadDonorDetails(string username)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            string sql = "SELECT donorName, donorEmail, donorContactNumber, donorAddress1 FROM donor WHERE donorUsername = @donorUsername";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@donorUsername", username}
            };

           
            _dt = _Qry.GetData(sql, parameter);
            if (_dt.Rows.Count > 0)
            {

                txtFullName.Text = _dt.Rows[0]["donorName"].ToString();
                txtEmail.Text = _dt.Rows[0]["donorEmail"].ToString();
                txtPhone.Text = _dt.Rows[0]["donorContactNumber"].ToString();
                txtAddress.Text = _dt.Rows[0]["donorAddress1"].ToString();


            }

            
        }

   
        private void LoadPaymentDetails(string donationPublishId)
        {
            QRY _Qry = new QRY();
            DataTable _dt, _dt2;
            double delivery = 0.0;
            double distance = 0.0;
            double additionalCharge= 0.0;
            string feeBreakdown = "";
            string donationId = Request.QueryString["donationId"];           
            string distanceValue = GetDistance(donationId);
          
            // Step 1: Retrieve donation state from donation_publish
            string sql = "SELECT donationState FROM donation_publish WHERE donationPublishId = @donationPublishId";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@donationPublishId", donationPublishId }
            };
            _dt = _Qry.GetData(sql, parameter);

            // check if retrieved any records for donationPublishId
            if (_dt.Rows.Count > 0)
            {
                // Step 2: Extract the state from donation_publish
                string donationState = _dt.Rows[0]["donationState"].ToString();

                // Step 3: Retrieve the state from donation_item_request to compare
                string sql2 = "SELECT state FROM donation_item_request WHERE donationId = @donationId";
                Dictionary<string, object> parameter2 = new Dictionary<string, object>
                {
                    { "@donationId", donationId }
                };
                _dt2 = _Qry.GetData(sql2, parameter2);

                if (_dt2.Rows.Count > 0)
                {
                    string requestState = _dt2.Rows[0]["state"].ToString();

                    // Step 4: Compare the two states and apply the charge
                    if (requestState != donationState)
                    {
                        charge.Text = "RM 5.00";
                        additionalCharge = 5.00;
                    }
                    else
                    {
                        charge.Text = "RM 0.00";
                        additionalCharge = 0.00;
                    }
                }
            }

            if (distanceValue != null)
            {
                distance = double.Parse(distanceValue);
                distance = Math.Round(distance, 1);
            }

            //distance= double.Parse(distanceValue);

            if (ddlVehicleType.SelectedValue == "Car")
            {
                if (distance <= 4)
                {
                    delivery = 5.00;
                    feeBreakdown = $"4 km = RM 5.00";
                }
                else if (distance <= 100)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 1.50;
                    delivery = 5.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 5.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else
                {
                    double additionalDistance = distance - 4;
                    double firstSegment = 96 * 1.50;
                    double remainingDistance = additionalDistance - 96;
                    double remainingFee = remainingDistance * 1.10;
                    delivery = 5.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 5.00, 96 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            else if (ddlVehicleType.SelectedValue == "4x4 Pickup")
            {
                if (distance <= 4)
                {
                    delivery = 10.00;
                    feeBreakdown = $"4 km = RM 10.00";
                }
                else if (distance <= 100)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 1.76;
                    delivery = 10.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 10.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else
                {
                    double additionalDistance = distance - 4;
                    double firstSegment = 96 * 1.76;
                    double remainingDistance = additionalDistance - 96;
                    double remainingFee = remainingDistance * 1.32;
                    delivery = 10.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 10.00, 96 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            else if (ddlVehicleType.SelectedValue == "Van 7 Feet")
            {
                if (distance <= 4)
                {
                    delivery = 22.00;
                    feeBreakdown = $"4 km = RM 22.00";
                }
                else if (distance <= 60)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 2.40;
                    delivery = 22.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 22.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else if (distance <= 100)
                {
                    double firstSegment = 56 * 2.40;
                    double remainingDistance = distance - 60;
                    double remainingFee = remainingDistance * 2.00;
                    delivery = 22.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 22.00, 56 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else if (distance <= 250)
                {
                    double firstSegment = 56 * 2.40;
                    double secondSegment = 40 * 2.00;
                    double remainingDistance = distance - 100;
                    double remainingFee = remainingDistance * 1.60;
                    delivery = 22.00 + firstSegment + secondSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 22.00, 56 km = RM {firstSegment.ToString("F2")}, 40 km = RM {secondSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else
                {
                    double firstSegment = 56 * 2.40;
                    double secondSegment = 40 * 2.00;
                    double thirdSegment = 150 * 1.60;
                    double remainingDistance = distance - 250;
                    double remainingFee = remainingDistance * 1.20;
                    delivery = 22.00 + firstSegment + secondSegment + thirdSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 22.00, 56 km = RM {firstSegment.ToString("F2")}, 40 km = RM {secondSegment.ToString("F2")}, 150 km = RM {thirdSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            else if (ddlVehicleType.SelectedValue == "Van 9 Feet")
            {
                if (distance <= 4)
                {
                    delivery = 31.00;
                    feeBreakdown = $"4 km = RM 31.00";
                }
                else if (distance <= 60)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 2.40;
                    delivery = 31.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 31.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else if (distance <= 100)
                {
                    double firstSegment = 56 * 2.40;
                    double remainingDistance = distance - 60;
                    double remainingFee = remainingDistance * 2.00;
                    delivery = 31.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 31.00, 56 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else if (distance <= 250)
                {
                    double firstSegment = 56 * 2.40;
                    double secondSegment = 40 * 2.00;
                    double remainingDistance = distance - 100;
                    double remainingFee = remainingDistance * 1.60;
                    delivery = 31.00 + firstSegment + secondSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 31.00, 56 km = RM {firstSegment.ToString("F2")}, 40 km = RM {secondSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else
                {
                    double firstSegment = 56 * 2.40;
                    double secondSegment = 40 * 2.00;
                    double thirdSegment = 150 * 1.60;
                    double remainingDistance = distance - 250;
                    double remainingFee = remainingDistance * 1.20;
                    delivery = 31.00 + firstSegment + secondSegment + thirdSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 31.00, 56 km = RM {firstSegment.ToString("F2")}, 40 km = RM {secondSegment.ToString("F2")}, 150 km = RM {thirdSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            else if (ddlVehicleType.SelectedValue == "Lorry 10 Feet")
            {
                if (distance <= 4)
                {
                    delivery = 40.00;
                    feeBreakdown = $"4 km = RM 40.00";
                }
                else if (distance <= 60)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 2.40;
                    delivery = 40.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 40.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else if (distance <= 150)
                {
                    double firstSegment = 56 * 2.40;
                    double remainingDistance = distance - 60;
                    double remainingFee = remainingDistance * 2.00;
                    delivery = 40.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 40.00, 56 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else
                {
                    double firstSegment = 56 * 2.40;
                    double secondSegment = 90 * 2.00;
                    double remainingDistance = distance - 150;
                    double remainingFee = remainingDistance * 1.60;
                    delivery = 40.00 + firstSegment + secondSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 40.00, 56 km = RM {firstSegment.ToString("F2")}, 90 km = RM {secondSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            else if (ddlVehicleType.SelectedValue == "Lorry 14 Feet")
            {
                if (distance <= 4)
                {
                    delivery = 140.00;
                    feeBreakdown = $"4 km = RM 140.00";
                }
                else if (distance <= 60)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 3.20;
                    delivery = 140.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 140.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else if (distance <= 150)
                {
                    double firstSegment = 56 * 3.20;
                    double remainingDistance = distance - 60;
                    double remainingFee = remainingDistance * 2.00;
                    delivery = 140.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 140.00, 56 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else
                {
                    double firstSegment = 56 * 3.20;
                    double secondSegment = 90 * 2.00;
                    double remainingDistance = distance - 150;
                    double remainingFee = remainingDistance * 1.60;
                    delivery = 140.00 + firstSegment + secondSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 140.00, 56 km = RM {firstSegment.ToString("F2")}, 90 km = RM {secondSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            else if (ddlVehicleType.SelectedValue == "Lorry 17 Feet")
            {
                if (distance <= 4)
                {
                    delivery = 160.00;
                    feeBreakdown = $"4 km = RM 160.00";
                }
                else if (distance <= 60)
                {
                    double additionalDistance = distance - 4;
                    double additionalFee = additionalDistance * 3.20;
                    delivery = 160.00 + additionalFee;
                    feeBreakdown = $"4 km = RM 160.00, {additionalDistance.ToString("F1")} km = RM {additionalFee.ToString("F2")}";
                }
                else if (distance <= 150)
                {
                    double firstSegment = 56 * 3.20;
                    double remainingDistance = distance - 60;
                    double remainingFee = remainingDistance * 2.00;
                    delivery = 160.00 + firstSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 160.00, 56 km = RM {firstSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }
                else
                {
                    double firstSegment = 56 * 3.20;
                    double secondSegment = 90 * 2.00;
                    double remainingDistance = distance - 150;
                    double remainingFee = remainingDistance * 1.60;
                    delivery = 160.00 + firstSegment + secondSegment + remainingFee;
                    feeBreakdown = $"4 km = RM 160.00, 56 km = RM {firstSegment.ToString("F2")}, 90 km = RM {secondSegment.ToString("F2")}, {remainingDistance.ToString("F1")} km = RM {remainingFee.ToString("F2")}";
                }

                feeBreakdown = feeBreakdown.Replace(",", "<br/>").Replace(" ", "&nbsp;");

            }

            calculation.Text = feeBreakdown;

            deliveryFee.Text = "RM " + delivery.ToString("F2");

            // calculate total payment (delivery + additional charge)
            double totalPayment = delivery + additionalCharge;

            // set total payment in the UI
            payment.Text = "RM " + totalPayment.ToString("F2");
        }

        [WebMethod]
        public static string GetOrganizationAddress(string donationPublishId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;

            // Step 1: Fetch orgId based on donationPublishId from donation_publish table
            string sql = "SELECT address FROM donation_publish WHERE donationPublishId = @donationPublishId";
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@donationPublishId", donationPublishId }
            };
            _dt = _Qry.GetData(sql, parameter);

          
            if (_dt.Rows.Count > 0)
            {
                // Step 2: Extract orgId
                string address = _dt.Rows[0]["address"].ToString();
                HttpContext.Current.Session["DestinationAddress"] = address;

                return address;

            }

            // if no records found or any issues
            return null;
        }

        [WebMethod]
        public static double ReceiveDistance(string distance)
        {
          
            double distanceValue = double.Parse(distance);

            // set as session
            HttpContext.Current.Session["Distance"] = distance;

            return distanceValue;
        }

        protected void NextSlide_Click(object sender, EventArgs e)
        {
            int currentSlide = int.Parse(hfCurrentSlide.Value);

            if (currentSlide < 3) 
            {
                // move to the next slide
                int nextSlide = currentSlide + 1;
                hfCurrentSlide.Value = nextSlide.ToString();

                // update new slide number
                UpdateSlideContent(nextSlide);
            }
        }


        protected void PreviousSlide_Click(object sender, EventArgs e)
        {
            int currentSlide = int.Parse(hfCurrentSlide.Value);

            if (currentSlide > 0) 
            {
                // move to the previous slide
                int previousSlide = currentSlide - 1;
                hfCurrentSlide.Value = previousSlide.ToString();

                // update slide number
                UpdateSlideContent(previousSlide);
            }
        }



        private void UpdateSlideContent(int slideNumber)
        {
            // get the current slide value
            hfCurrentSlide.Value = slideNumber.ToString();

            int currentSlide = slideNumber; 
            string donationPublishId = Request.QueryString["donationPublishId"];
            string donationId = Request.QueryString["donationId"];
           
            // load data based on the current slide
            switch (currentSlide)
            {
                case 0:
                    LoadDonationDetails(donationPublishId);
                    formContainer1.Visible = (currentSlide == 0);
                    formContainer2.Visible = false;
                    formContainer3.Visible = false;
                    formContainer4.Visible = false;
                    break;

                case 1:
                    string username = Session["username"].ToString();
                    LoadDonorDetails(username);
                    formContainer2.Visible = (currentSlide == 1);
                    formContainer1.Visible = false;
                    formContainer3.Visible = false;
                    formContainer4.Visible = false;
                    break;

                case 2:
                    LoadDonationItems(donationId);
                    GetPickupAddress(donationId);
                    formContainer1.Visible = false;
                    formContainer2.Visible = false;
                    formContainer3.Visible = (currentSlide == 2);
                    formContainer4.Visible = false;
                    break;

                case 3:
                    
                    LoadPaymentDetails(donationPublishId);
                    distance.Text = Convert.ToDouble(GetDistance(donationId)).ToString("F1") + " km";
                    Session["distance"] = distance.Text;

                    formContainer4.Visible = (currentSlide == 3);
                    formContainer1.Visible = false;
                    formContainer2.Visible = false;
                    formContainer3.Visible = false;
                    break;
            }
        }

        public static string GetPickupAddress(string donationId)
        {
            if (string.IsNullOrEmpty(donationId))
            {
                throw new Exception("Donation Id is missing.");
            }

            string sql = "SELECT pickUpAddress FROM donation_item_request WHERE donationId = @donationId";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

          
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                string pickupAddress = dt.Rows[0]["pickUpAddress"].ToString();

                // set the pickup address in the session
                HttpContext.Current.Session["PickupAddress"] = pickupAddress;

                return pickupAddress;
            }
            else
            {
                throw new Exception($"No record found for donationId: {donationId}");
            }

            
        }


        [WebMethod]
        public static string GetPickupAddress()
        {
            // retrieve the address from the session
            return HttpContext.Current.Session["PickupAddress"]?.ToString();
        }


        [WebMethod]
        public static void SubmitAllData(string donatedItemsJson, string expiryDatesJson, string donationPublishId, PersonalDetails details, string desc, string image)
        {
            List<DonationItemRequest> donatedItems = JsonConvert.DeserializeObject<List<DonationItemRequest>>(donatedItemsJson);
            List<DonationItemExpiryDate> expiryDates = JsonConvert.DeserializeObject<List<DonationItemExpiryDate>>(expiryDatesJson);

            // generate the donationId once and use it for both SaveDonationItems and SaveDonationExpiryDates
            string donationId = GenerateDonationId();

            // save the donation items with the generated donationId
            SaveDonationItems(donatedItems, donationPublishId, "Pending", details, HttpContext.Current.Session, desc, image, donationId);

            // save the expiry dates with the same donationId
            SaveDonationExpiryDates(expiryDates, donationPublishId, HttpContext.Current.Session, donationId);
        }

        public static void SaveDonationItems(List<DonationItemRequest> donationItems, string donationPublishId, string status, PersonalDetails details, HttpSessionState session, string desc, string image, string donationId)
        {
            string username = session["username"].ToString();
            string id = GetDonorId(username);

            string orgId = GetOrgId(donationPublishId);

            foreach (var item in donationItems)
            {
                string sql = "EXEC [create_donation_item] " +
                             "@method = 'INSERT', " +
                             "@donationId = '" + donationId + "', " +  
                             "@itemImage = '" + image + "', " +
                             "@requestStatus = '" + status + "', " +
                             "@itemCategory = '" + item.category + "', " +
                             "@item = '" + item.items + "', " +
                             "@quantityDonated = '" + item.quantities + "', " +
                             "@reasonOfRejection = NULL, " +
                             "@description = '" + desc + "', " +
                             "@donorId = '" + id + "', " +
                             "@orgId = '" + orgId + "', " +
                             "@donationPublishId = '" + donationPublishId + "', " +
                             "@created_on = NULL, " +
                             "@donorFullName = '" + details.FullName + "', " +
                             "@donorEmail = '" + details.Email + "', " +
                             "@donorPhoneNumber = '" + details.Phone + "', " +
                             "@pickUpAddress = '" + details.Address + "', " +
                             "@state = '" + details.State + "' ";

                QRY _Qry = new QRY();
                bool success = _Qry.ExecuteNonQuery(sql);

                if (!success)
                {
                    Console.WriteLine($"Failed to insert donation items for category: {item.category}");
                }
            }
        }

        public static void SaveDonationExpiryDates(List<DonationItemExpiryDate> expiryDates, string donationPublishId, HttpSessionState session, string donationId)
        {
            string username = session["username"].ToString();
            string id = GetDonorId(username);

            string orgId = GetOrgId(donationPublishId);

            foreach (var expiry in expiryDates)
            {
                string sql = "EXEC [create_donation_item] " +
                             "@method = 'INSERT', " +
                             "@donationId = '" + donationId + "', " +  // use the same donationId
                             "@itemCategory = '" + expiry.category + "', " +
                             "@item = '" + expiry.item + "', " +
                             "@totalQuantity = '" + expiry.totalQuantity + "', " +
                             "@quantityWithSameExpiryDate = '" + expiry.quantity + "', " +
                             "@expiryDate = '" + expiry.expiryDate + "', " +
                             "@donorId = '" + id + "', " +
                             "@orgId = '" + orgId + "', " +
                             "@donationPublishId = '" + donationPublishId + "', " +
                             "@created_on = NULL ";

                QRY _Qry = new QRY();
                bool success = _Qry.ExecuteNonQuery(sql);

                if (!success)
                {
                    Console.WriteLine($"Failed to insert expiry date for item: {expiry.item}");
                }
            }
        }


        public static string GenerateDonationId()
        {
            string sql = @"
            SELECT 'DR' + CAST(ISNULL(MAX(CAST(SUBSTRING(donationId, 3, LEN(donationId) - 2) AS INT)), 0) + 1 AS VARCHAR)
            FROM donation_item_request;";

            QRY _Qry = new QRY();
         
            DataTable dt = _Qry.GetData(sql);

            if (dt.Rows.Count > 0)
            {
               
                return dt.Rows[0][0].ToString(); 
            }
            else
            {
                throw new Exception("Failed to generate donation ID.");
            }
        }


        public static string GetDonorId(string username)
        {
          

            if (string.IsNullOrEmpty(username))
            {
                throw new Exception("Username is missing.");
            }
        
            string sql = "SELECT donorId FROM donor WHERE donorUsername = @username";

         
            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@username", username }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["donorId"].ToString();
            }
            else
            {
                throw new Exception($"No record found for username: {username}");
            }
            
        }

        public static string GetOrgId(string donationPublishId)
        {


            if (string.IsNullOrEmpty(donationPublishId))
            {
                throw new Exception("Donation Publish Id is missing.");
            }

            string sql = "SELECT orgId FROM donation_publish WHERE donationPublishId = @donationPublishId";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationPublishId", donationPublishId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["orgId"].ToString();
            }
            else
            {
                throw new Exception($"No record found for donationPublishId: {donationPublishId}");
            }


        }

        private List<string> GetCategoriesWithExpiryDate()
        {
            List<string> categories = new List<string>();

            string sql = "SELECT categoryName FROM itemCategory WHERE hasExpiryDate = 'Yes'";

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // add category name to the list
                    categories.Add(row["categoryName"].ToString());
                }
            }

            return categories;
        }

        private void LoadDonationItems(string donationId)
        {
            string query1 = "SELECT content FROM deleted_item_donations WHERE donationId = @donationId";
            QRY qry1 = new QRY();
            var parameter = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };
            DataTable dt = qry1.GetData(query1, parameter);

            if (dt.Rows.Count > 0)
            {
  
                string deletedItem = dt.Rows[0]["content"].ToString();


                lblDeletedItem.Text = deletedItem;
            }

            else
            {
                lblDeletedItem.Text = "None";
            }

            string query2 = "SELECT itemCategory, item, quantityDonated FROM donation_item_request WHERE donationId = @donationId";

            QRY qry2 = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };
            DataTable dt2 = qry2.GetData(query2, parameters);

            bool hasFurniture = false; // check if category contains 'Furniture'

            if (dt2.Rows.Count > 0)
            {
                // start creating a table
                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='categoryDetailsTable' style='margin: auto;'>");
                sb.Append("<thead><tr><th>Category</th><th>Item</th><th>Quantity</th></tr></thead>");
                sb.Append("<tbody>");

                foreach (DataRow row in dt2.Rows)
                {
                    string category = row["itemCategory"].ToString();
                    string items = row["item"].ToString();
                    string quantities = row["quantityDonated"].ToString();

                    // check if the category is 'Furniture'
                    if (category.Equals("Furniture", StringComparison.OrdinalIgnoreCase))
                    {
                        hasFurniture = true;  // if Furniture is found
                    }

                    // split items and quantities by commas if needed
                    quantities = quantities.Replace("(", "").Replace(")", "");
                    string[] itemArray = items.Split(',');
                    string[] qtyArray = quantities.Split(',');

                    for (int i = 0; i < itemArray.Length; i++)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td>{0}</td>", category);  // display category in each row
                        sb.AppendFormat("<td>{0}</td>", itemArray[i].Trim());  // display each item
                        sb.AppendFormat("<td>{0}</td>", i < qtyArray.Length ? qtyArray[i].Trim() : "N/A");  // display each quantity or "N/A" if missing
                        sb.Append("</tr>");
                    }
                }

                sb.Append("</tbody></table>");

                phDonationItems.Controls.Add(new Literal { Text = sb.ToString() });

                // if Furniture category is found, disable the "Car" option in the vehicle type dropdown
                if (hasFurniture)
                {
                    System.Web.UI.WebControls.ListItem carOption = ddlVehicleType.Items.FindByValue("Car");

                    if (carOption != null)
                    {
                        carOption.Enabled = false;  
                    }
                }
            }
        }

        public static string GetDistance(string donationId)
        {

            if (string.IsNullOrEmpty(donationId))
            {
                throw new Exception("Donation Id is missing.");
            }

            string sql = "SELECT totalDistance FROM donation_item_request WHERE donationId = @donationId";


            QRY _Qry = new QRY();
            var parameters = new Dictionary<string, object>
            {
                { "@donationId", donationId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["totalDistance"].ToString();
            }
            else
            {
                throw new Exception($"No record found for donationId: {donationId}");
            }


        }

        protected void btnSubmitDelivery_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            string username = Session["username"].ToString();
            string donorId = GetDonorId(username);

            
            QRY _Qry = new QRY();

            string sqlSendOtp = "EXEC [send_payment] @action= @action, @donorId = @donorId, @otp = @otp, @donorEmail= @donorEmail";
            var otpParameters = new Dictionary<string, object>
            {
                { "@action", "OTP" },
                { "@donorId", donorId },
                { "@otp", otp },
                { "@donorEmail", txtEmailOTP.Text }
            };

            bool otpSent = _Qry.ExecuteNonQuery(sqlSendOtp, otpParameters);

            if (otpSent)
            {
                // store the OTP in the session to verify later
                Session["PaymentOtp"] = otp;
                Session["OtpExpiration"] = DateTime.Now.AddMinutes(2); // set the expiration time

                // notify the user and show the OTP field
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                Swal.fire({
                    title: 'OTP Sent!',
                    text: 'Please check your inbox. OTP has been sent to " + txtEmailOTP.Text + @"',
                    icon: 'success',
                    confirmButtonText: 'OK'
                }).then((result) => {
                    if (result.isConfirmed) {
                        document.getElementById('otp').style.display = 'block'; // Show OTP div
                    }
                });
            ", true);

                btnSubmit.Attributes["style"] = "display: none;";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error sending OTP. Please try again!');", true);
            }
        }

        protected void btnValidateOtp_Click(object sender, EventArgs e)
        {
            // check if OTP is still valid (not expired)
            if (Session["OtpExpiration"] != null && DateTime.Now > (DateTime)Session["OtpExpiration"])
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('The OTP has expired. Please request a new OTP.');", true);
                txtEmailOTP.Text = "";
                return;
            }

            // retrieve OTP from session and entered OTP from input
            string sessionOtp = Session["PaymentOtp"]?.ToString();
            string enteredOtp = txtOTP.Text.Trim();

            if (sessionOtp == enteredOtp)
            {
                
                string username = Session["username"].ToString();
                string donorId = GetDonorId(username);
                QRY _Qry = new QRY();

                string donationPublishId = Request.QueryString["donationPublishId"];
                string donationId = Request.QueryString["donationId"];
                string status = "Waiting for delivery rider";
                string paymentStatus = "Approved";
                string pickupAddress = Session["PickupAddress"].ToString();
                string destination = Session["DestinationAddress"].ToString();
                string orgId = GetOrgId(donationPublishId);

                // insert into the database
                string sql = "EXEC [create_delivery_payment] " +
                             "@method = @method, " +
                             "@deliveryId = @deliveryId, " +
                             "@date = @date, " +
                             "@time = @time, " +
                             "@status = @status, " +
                             "@vehicleType = @vehicleType, " +
                             "@pickupAddress = @pickupAddress, " +
                             "@destinationAddress = @destinationAddress, " +
                             "@donorId = @donorId, " +
                             "@orgId = @orgId, " +
                             "@donationId = @donationId, " +
                             "@noteRider = @noteRider, " +
                             "@noteOrg = @noteOrg, " +
                             "@fee = @fee, " +
                             "@paymentId = @paymentId, " +
                             "@paymentStatus = @paymentStatus";

              
                var parameters = new Dictionary<string, object>
                {
                    { "@method", "INSERT" },
                    { "@deliveryId", DBNull.Value }, 
                    { "@date", txtPickupDate.Text },
                    { "@time", txtPickupTime.Text },
                    { "@status", status },
                    { "@vehicleType", ddlVehicleType.SelectedValue },
                    { "@pickupAddress", pickupAddress },
                    { "@destinationAddress", destination },
                    { "@donorId", donorId },
                    { "@orgId", orgId },
                    { "@donationId", donationId },
                    { "@noteRider", noteRider.Text },
                    { "@noteOrg", noteOrg.Text },
                    { "@fee", deliveryFee.Text },
                    { "@paymentId", DBNull.Value }, 
                    { "@paymentStatus", paymentStatus } 
                };

              
                bool success = _Qry.ExecuteNonQuery(sql, parameters);

                if (success)
                {
                    // QR Code Generation 
                    Random random = new Random();
                    string code = random.Next(10000000, 99999999).ToString();

                    string token = Hash(code + username + DateTime.Now.ToString());
                    token = token.Replace("+", "A");
                    token = Encryption.Encrypt(token);

                    string loginUrl = $"https://localhost:44390/UpdateDelivery.aspx?token={token}";

                    //string qrContent = $"DonationId: {donationId}, PickUpAddress: {pickupAddress}, Code: {code}";
                    string qrBase64 = GenerateQrCode(loginUrl);

                    // store the QR code as base64 in the database
                    string qrSql = "UPDATE delivery SET qrCode = @qrCode, pageToken = @pageToken WHERE donationId = @donationId";
                    var qrParams = new Dictionary<string, object>
                    {
                        { "@donationId", donationId },
                        { "@qrCode", qrBase64 },
                        { "@pageToken", token }  
                    };

                    _Qry.ExecuteNonQuery(qrSql, qrParams);

                    Organization org = new Organization("", orgId, "", "", "");
                    string orgName = org.GetOrgName();

                    string donorSql= "SELECT donorName FROM donor WHERE donorId = @donorId";
                    var donorParameter = new Dictionary<string, object>
                    {
                        { "@donorId", donorId },
                    };
                    DataTable dt = _Qry.GetData(donorSql, donorParameter);

                    string donorName = "";
                    if (dt.Rows.Count > 0)
                    {
                        donorName = dt.Rows[0]["donorName"].ToString();
                    }

                    string vehicleSql = "SELECT vehicleType FROM delivery WHERE donationId = @donationId";
                    var vehicleParameter = new Dictionary<string, object>
                    {
                        { "@donationId", donationId },
                    };
                    DataTable vehicleData = _Qry.GetData(vehicleSql, vehicleParameter);
                    string vehicleType = "";
                    if (vehicleData.Rows.Count > 0)
                    {
                        vehicleType = vehicleData.Rows[0]["vehicleType"].ToString();  // Access by column name
                    }
                    // get Payment ID
                    string paymentSql = "SELECT paymentId FROM payment WHERE donationId = @donationId";
                    var paymentParameter = new Dictionary<string, object>
                    {
                        { "@donationId", donationId },
                    };
                    DataTable paymentData = _Qry.GetData(paymentSql, paymentParameter);
                    string paymentId = "";
                    if (paymentData.Rows.Count > 0)
                    {
                        paymentId = paymentData.Rows[0]["paymentId"].ToString();  // Access by column name
                    }

                    // generate PDF payment receipt
                    string pdfPath = GeneratePaymentReceipt(donationId, donorId, donorName, orgName, pickupAddress, destination, vehicleType, paymentId, "Approved", deliveryFee.Text );

                    // send the payment receipt via email
                    _Qry.ExecuteNonQuery("EXEC send_payment @action= @action, @donorId = @donorId, @pdfFilePath = @pdfFilePath, @donorEmail= @donorEmail", new Dictionary<string, object>
                    {
                        { "@action", "RECEIPT" },
                        { "@donorId", donorId },
                        { "@pdfFilePath", pdfPath },
                        { "@donorEmail", txtEmailOTP.Text }
                    });



                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('OTP is valid and payment successful! Your donation order is now in the queue of waiting for pickup from delivery rider. Thank you for donating!',);", true);
                   
                    Session.Remove("PaymentOtp");
                    Session.Remove("OtpExpiration");
                    Session.Remove("PickupAddress");
                    Session.Remove("distance");
                    Session.Remove("DestinationAddress");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error making payment. Please try again!');", true);
                }
            }
        
            else
            {
                // OTP is invalid
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Invalid OTP. Please try again.');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showOtpDiv", "document.getElementById('otp').style.display = 'block';", true);
                txtEmailOTP.Text = "";
                return;
            }

            
        }



        [System.Web.Services.WebMethod]
        public static string DeleteDonation(string donationId)
        {
            try
            {
                if (!string.IsNullOrEmpty(donationId))
                {

                    // Step 1: Update donation_item_request table to set status to 'Cancelled'
                    string status = "Cancelled";
                    string sqlUpdateRequest = "UPDATE donation_item_request SET requestStatus = @status WHERE donationId = @donationId";
                    var parameter = new Dictionary<string, object>
                    {
                        { "@status", status },
                        { "@donationId", donationId }
                    };

                    QRY _Qry = new QRY();
                    bool successItem = _Qry.ExecuteNonQuery(sqlUpdateRequest, parameter);


                    if (successItem)
                    {
                        // Step 2: Check if donationId exists in donation_item_expiry_date table
                        string sqlCheckExpiryDate = "SELECT COUNT(*) FROM donation_item_expiry_date WHERE donationId = @donationId";
                        int expiryCount = (int)_Qry.ExecuteScalar(sqlCheckExpiryDate, parameter);

                        if (expiryCount > 0)
                        {
                            // Step 3: If exists, delete from donation_item_expiry_date table
                            string sqlDeleteExpiry = "DELETE FROM donation_item_expiry_date WHERE donationId = @donationId";
                            bool successExpiry = _Qry.ExecuteNonQuery(sqlDeleteExpiry, parameter);

                            if (!successExpiry)
                            {
                                return "error"; // if failed to delete 
                            }
                        }

                        // Step 4: Return success if everything is deleted 
                        return "success";
                    }
                    else
                    {
                        return "error"; // if failed to delete from donation_item_request
                    }
                }
                return "error"; // if donationId is invalid
            }
            catch (Exception ex)
            {
              
                return "error"; 
            }
        }

        private string GeneratePaymentReceipt(string donationId, string donorId, string donorName, string orgName, string pickupAddress, string destination, string vehicleType, string paymentId, string paymentStatus, string fee)
        {
            // receipt saved file path
            string filePath = @"C:\DonorConnect_PaymentReceipt\" + donationId + "_receipt.pdf";

            // create directory if it does not exist
            if (!Directory.Exists(@"C:\DonorConnect_PaymentReceipt\"))
            {
                Directory.CreateDirectory(@"C:\DonorConnect_PaymentReceipt\");
            }

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
                Paragraph title = new Paragraph("Payment Receipt", titleFontSize);
                title.Alignment = Element.ALIGN_CENTER;
                title.SpacingAfter = 20;
                document.Add(title);

                // 3. Create Details Table 
                PdfPTable detailsTable = new PdfPTable(2);
                detailsTable.WidthPercentage = 100;
                detailsTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // add details 
                detailsTable.AddCell(new Phrase("Payment ID", bodyFontSize));
                detailsTable.AddCell(new Phrase(paymentId, bodyFontSize));

                detailsTable.AddCell(new Phrase("Payment Status", bodyFontSize));
                detailsTable.AddCell(new Phrase(paymentStatus, bodyFontSize));

                detailsTable.AddCell(new Phrase("Donor Name", bodyFontSize));
                detailsTable.AddCell(new Phrase(donorName, bodyFontSize));

                detailsTable.AddCell(new Phrase("Organization Name", bodyFontSize));
                detailsTable.AddCell(new Phrase(orgName, bodyFontSize));

                detailsTable.AddCell(new Phrase("Pickup Address", bodyFontSize));
                detailsTable.AddCell(new Phrase(pickupAddress, bodyFontSize));

                detailsTable.AddCell(new Phrase("Destination Address", bodyFontSize));
                detailsTable.AddCell(new Phrase(destination, bodyFontSize));

                detailsTable.AddCell(new Phrase("Vehicle Type", bodyFontSize));
                detailsTable.AddCell(new Phrase(vehicleType, bodyFontSize));

                document.Add(detailsTable);
                document.Add(new Paragraph("\n")); 

                // 4. Payment Type and Amount Table
                PdfPTable paymentTable = new PdfPTable(3);
                paymentTable.WidthPercentage = 100;
                paymentTable.SetWidths(new float[] { 33, 33, 34 }); 

                PdfPCell header1 = new PdfPCell(new Phrase("", bodyFontSize)); 
                header1.BorderColor = BaseColor.BLACK; 
                header1.BackgroundColor = BaseColor.LIGHT_GRAY; 
                header1.Border = PdfPCell.BOX; 
                paymentTable.AddCell(header1);

                PdfPCell header2 = new PdfPCell(new Phrase("Payment Type", bodyFontSize));
                header2.HorizontalAlignment = Element.ALIGN_CENTER;
                header2.BorderColor = BaseColor.BLACK; 
                header2.BackgroundColor = BaseColor.LIGHT_GRAY; 
                header2.Border = PdfPCell.BOX; 
                paymentTable.AddCell(header2);

                PdfPCell header3 = new PdfPCell(new Phrase("Payment Amount", bodyFontSize));
                header3.HorizontalAlignment = Element.ALIGN_CENTER;
                header3.BorderColor = BaseColor.BLACK; 
                header3.BackgroundColor = BaseColor.LIGHT_GRAY; 
                header3.Border = PdfPCell.BOX; 
                paymentTable.AddCell(header3);

                // Add Delivery Fee 
                PdfPCell feeCell1 = new PdfPCell(new Phrase("Delivery Fee", bodyFontSize));
                feeCell1.BorderColor = BaseColor.BLACK;
                feeCell1.Border = PdfPCell.BOX; 
                paymentTable.AddCell(feeCell1);

                PdfPCell feeCell2 = new PdfPCell(new Phrase("Credit / Debit Card", bodyFontSize));
                feeCell2.BorderColor = BaseColor.BLACK;
                feeCell2.Border = PdfPCell.BOX; 
                paymentTable.AddCell(feeCell2);

                PdfPCell feeCell3 = new PdfPCell(new Phrase(fee, bodyFontSize));
                feeCell3.BorderColor = BaseColor.BLACK;
                feeCell3.Border = PdfPCell.BOX; 
                paymentTable.AddCell(feeCell3);

                // add table
                document.Add(paymentTable);
                document.Add(new Paragraph("\n")); 

                // 5. Add Thank You Message
                Paragraph thankYou = new Paragraph("Thank you for donating! Before the rider accepts your order, you can request a refund from our team.", subTitleFontSize);
                thankYou.Alignment = Element.ALIGN_CENTER;
                thankYou.SpacingBefore = 30;
                thankYou.SpacingAfter = 20;
                document.Add(thankYou);

                // 6. Footer: Current Year and DonorConnect
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

            return filePath;
        }

        public string GenerateQrCode(string content)
        {
            using (QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator()) 
            {
                using (QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCoder.QRCodeGenerator.ECCLevel.Q))
                {
                    using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData)) 
                    {
                        using (Bitmap qrCodeImage = qrCode.GetGraphic(20))
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] byteImage = ms.ToArray();
                                return Convert.ToBase64String(byteImage);  // convert image to base64
                            }
                        }
                    }
                }
            }
        }

        static string Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

    }
}