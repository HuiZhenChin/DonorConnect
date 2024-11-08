using DonorConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class OrgInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string username = Session["username"].ToString();

                Organization org = new Organization(username, "", "", "", "");
                string orgId = org.GetOrgId();

                FetchDonationRequestCount();
                FetchOngoingDeliveryCount();
                FetchCompletedRequestCount();
                FetchAllRequestCount();

                LoadExpiryDate();

                GeneratePreviousMonthReport();

                var deliveryStatusData = GetDeliveryStatusData(orgId);
                string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(deliveryStatusData);

                ClientScript.RegisterStartupScript(this.GetType(), "deliveryData",
                    $"var deliveryData = {jsonData};", true);

                var itemCategoryStatusData = GetItemCategoryStatusData(orgId.ToString());
                string jsonData2 = Newtonsoft.Json.JsonConvert.SerializeObject(itemCategoryStatusData);

                ClientScript.RegisterStartupScript(this.GetType(), "itemCategoryData",
                    $"var itemCategoryData = {jsonData2};", true);

                if (Session["AlertMessage"] != null)
                {
                    string alert = Session["AlertMessage"].ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showInfo('{alert}');", true);

                    Session["AlertMessage"] = null;
                }

            }

            if (Request.Form["__EVENTTARGET"] == "MarkAsRead")
            {
                if (hfMarkAsRead.Value == "true")
                {
                    markRemindersAsRead();  
                    hfMarkAsRead.Value = "";  
                }
            }
        }

        public class DeliveryStatusData
        {
            public string status { get; set; }
            public int count { get; set; }
        }

        public class ItemCategoryStatusData
        {
            public string itemCategory { get; set; }
            public string requestStatus { get; set; }
            public int count { get; set; }
        }

        private void FetchDonationRequestCount()
        {
            QRY _Qry = new QRY();

            string username= Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT COUNT(DISTINCT donationId) 
                   FROM donation_item_request 
                   WHERE orgId = @orgId AND requestStatus = 'Pending'";

          
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }  
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
 
                newRequestBox.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                newRequestBox.Text = "0";  
            }

        }

     
        private void FetchOngoingDeliveryCount()
        {
       
            QRY _Qry = new QRY();

            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT COUNT(*) 
                        FROM delivery 
                        WHERE orgId = @orgId AND deliveryStatus NOT IN ('Cancelled', 'Reached Destination')";


            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {

                ongoingDeliveryBox.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                ongoingDeliveryBox.Text = "0";
            }
        }

        private void FetchCompletedRequestCount()
        {
            QRY _Qry = new QRY();

            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT COUNT(*) 
                            FROM delivery 
                            WHERE orgId = @orgId AND deliveryStatus = 'Reached Destination'";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {

                completedRequestBox.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                completedRequestBox.Text = "0";
            }

        }

        // fetch count for "Request Made" 
        private void FetchAllRequestCount()
        {
            QRY _Qry = new QRY();

            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT COUNT(DISTINCT donationId) 
                            FROM donation_item_request 
                            WHERE orgId = @orgId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {

                requestMadeBox.Text = dt.Rows[0][0].ToString();
            }
            else
            {
                requestMadeBox.Text = "0";
            }
        }

        private void LoadExpiryDate()
        {
            QRY _Qry = new QRY();

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();
     
            string sql = @"
            SELECT content 
            FROM temp_expiry_reminder 
            WHERE orgId = @orgId
            AND [read] = 0";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            // fetch unread reminders
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                List<string> reminders = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    reminders.Add(row["content"].ToString());
                }

                string reminderMessage = string.Join("<br>", reminders);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showSweetAlert",
                     $@"
                Swal.fire({{
                    title: 'Expiry Reminders',
                    html: '{reminderMessage}',
                    icon: 'info',
                    position: 'bottom-end',
                    showConfirmButton: true, 
                    confirmButtonText: 'Mark as Read'
                }}).then((result) => {{
                    if (result.isConfirmed) {{
                        document.getElementById('{hfMarkAsRead.ClientID}').value = 'true';  
                        __doPostBack('MarkAsRead', '');  // Trigger the postback
                    }}
                }});", true);


            }
            else
            {
                Console.WriteLine("No unread expiry reminders found.");
            }
        }

    
        private void markRemindersAsRead()
        {
            QRY _Qry = new QRY();

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string updateSql = @"
            UPDATE temp_expiry_reminder 
            SET [read] = 1 
            WHERE orgId = @orgId
            AND [read] = 0";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            _Qry.ExecuteNonQuery(updateSql, parameters);

            
        }

        public void GeneratePreviousMonthReport()
        {
            // get the previous month and year
            DateTime currentDate = DateTime.Now;
            int previousMonth = currentDate.AddMonths(-1).Month;
            int previousYear = currentDate.AddMonths(-1).Year;

            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string reportFileName = $"{orgId}_{previousMonth}_{previousYear}_MonthlyReport.pdf";

            string folderPath = HttpContext.Current.Server.MapPath("~/Reports/");
            string filePath = Path.Combine(folderPath, reportFileName);

            if (File.Exists(filePath))
            {
                // if the report already exists for the previous month, no need to generate it again
                Console.Write("The report for the previous month already exists.");
                return;
            }

            // check if there is data for the previous month in any of the tables
            bool hasData = CheckPreviousMonthData(orgId, previousMonth, previousYear);

            if (hasData)
            {
                // generate the report for the previous month and year
                OrgReport.GenerateMonthlyReport(previousMonth, previousYear);
            }
            else
            {
                Console.Write("No data available for the previous month; report will not be generated.");
            }
        }

        private bool CheckPreviousMonthData(string orgId, int month, int year)
        {
            QRY _Qry = new QRY();

            string deliveryQuery = @"
            SELECT COUNT(*) FROM delivery
            WHERE MONTH(created_on) = @month AND YEAR(created_on) = @year AND orgId = @orgId";

            string inventoryQuery = @"
            SELECT COUNT(*) FROM inventory
            WHERE orgId = @orgId";

            string inventoryHistoryQuery = @"
            SELECT COUNT(*) FROM inventory_history
            WHERE MONTH(created_on) = @month AND YEAR(created_on) = @year AND orgId = @orgId";

            string inventoryUsageQuery = @"
            SELECT COUNT(*) FROM inventory_item_usage
            WHERE MONTH(created_on) = @month AND YEAR(created_on) = @year AND orgId = @orgId";

            string donationItemRequestQuery = @"
            SELECT COUNT(*) FROM donation_item_request
            WHERE MONTH(created_on) = @month AND YEAR(created_on) = @year AND orgId = @orgId";

            var parameters = new Dictionary<string, object>
            {
                { "@month", month },
                { "@year", year },
                { "@orgId", orgId }
            };

            // check if any of the tables have data from the previous month
            if (int.TryParse(_Qry.GetScalarValue(deliveryQuery, parameters), out int deliveryCount) && deliveryCount > 0)
                return true;

            if (int.TryParse(_Qry.GetScalarValue(inventoryQuery, new Dictionary<string, object> { { "@orgId", orgId } }), out int inventoryCount) && inventoryCount > 0)
                return true;

            if (int.TryParse(_Qry.GetScalarValue(inventoryHistoryQuery, parameters), out int inventoryHistoryCount) && inventoryHistoryCount > 0)
                return true;

            if (int.TryParse(_Qry.GetScalarValue(inventoryUsageQuery, parameters), out int inventoryUsageCount) && inventoryUsageCount > 0)
                return true;

            if (int.TryParse(_Qry.GetScalarValue(donationItemRequestQuery, parameters), out int donationItemRequestCount) && donationItemRequestCount > 0)
                return true;

            // if no data found
            return false;
        }


        public List<DeliveryStatusData> GetDeliveryStatusData(string orgId)
        {
            List<DeliveryStatusData> deliveryStatusData = new List<DeliveryStatusData>();
            string sql = "SELECT deliveryStatus, COUNT(*) AS Count FROM delivery WHERE orgId = @orgId GROUP BY deliveryStatus";

            QRY _Qry = new QRY();

            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    deliveryStatusData.Add(new DeliveryStatusData
                    {
                        status = row["deliveryStatus"].ToString(),
                        count = Convert.ToInt32(row["Count"])
                    });
                }
            }

            return deliveryStatusData;
        }

        public List<ItemCategoryStatusData> GetItemCategoryStatusData(string orgId)
        {
            List<ItemCategoryStatusData> itemCategoryStatusData = new List<ItemCategoryStatusData>();
            string sql = @"
            SELECT itemCategory, requestStatus, COUNT(*) AS Count 
            FROM donation_item_request 
            WHERE orgId = @orgId 
            GROUP BY itemCategory, requestStatus";

            QRY _Qry = new QRY();
            Dictionary<string, object> parameter = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };
            DataTable dt = _Qry.GetData(sql, parameter);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    itemCategoryStatusData.Add(new ItemCategoryStatusData
                    {
                        itemCategory = row["itemCategory"].ToString(),
                        requestStatus = row["requestStatus"].ToString(),
                        count = Convert.ToInt32(row["Count"])
                    });
                }
            }

            return itemCategoryStatusData;
        }
    }
}