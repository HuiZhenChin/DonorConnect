using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class Delivery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string donationId = Request.QueryString["donationId"];

                if (donationId != null)
                {
                    LoadDelivery(donationId);

                }
            }
        }

        protected void LoadDelivery(string donationId)
        {
            // retrieve delivery details based on donationId
            QRY _Qry = new QRY();
            DataTable dtDelivery = new DataTable();
            string sqlQuery = $"SELECT deliveryStatus, riderId, pickupDate FROM delivery WHERE donationId = '{donationId}'";
            dtDelivery = _Qry.GetData(sqlQuery);

            if (dtDelivery.Rows.Count > 0)
            {
                string deliveryStatus = dtDelivery.Rows[0]["deliveryStatus"].ToString();
                string riderId = dtDelivery.Rows[0]["riderId"].ToString();
                string pickupDate = Convert.ToDateTime(dtDelivery.Rows[0]["pickupDate"]).ToString("dd MMM yyyy");

                // update the labels and progress bar based on the delivery status
                switch (deliveryStatus)
                {
                    case "Waiting for delivery rider":
                        lblStatus.Text = "Waiting for delivery rider to accept the order...";
                        lblDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                        step1.Attributes["class"] = "step-active";
                        break;

                    case "Accepted":
                        lblStatus.Text = "Waiting for delivery rider to pick up...";
                        lblDate.Text = pickupDate;
                        step1.Attributes["class"] = "step-done";
                        step2.Attributes["class"] = "step-active";
                        break;

                    case "Delivering in progress":
                        lblStatus.Text = "Delivering in Progress...";
                        lblDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                        step1.Attributes["class"] = "step-done";
                        step2.Attributes["class"] = "step-done";
                        step3.Attributes["class"] = "step-active";
                        break;

                    case "Reached Destination":
                        lblStatus.Text = "Delivered!";
                        lblDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                        step1.Attributes["class"] = "step-done";
                        step2.Attributes["class"] = "step-done";
                        step3.Attributes["class"] = "step-done";
                        step4.Attributes["class"] = "step-done";
                        break;

                    default:
                        lblStatus.Text = "Delivery status unknown";
                        break;
                }
            }
        }

    }
}