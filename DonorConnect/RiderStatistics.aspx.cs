using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class RiderStatistics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDeliveryChart();
            }
        }

        private DataTable GetMonthlyDeliveryStats(string riderId)
        {
            string query = @"
            SELECT 
            FORMAT(reachTimeByRider, 'yyyy-MM') AS Month,
            COUNT(*) AS DeliveryCount,
            SUM(CAST(paymentAmount AS decimal(18, 2))) AS TotalEarnings
        FROM delivery
        WHERE riderId = @riderId 
            AND deliveryStatus = 'Reached Destination' 
            AND itemApproved = 1 
            AND reachTimeByRider IS NOT NULL
        GROUP BY FORMAT(reachTimeByRider, 'yyyy-MM')
        ORDER BY Month;

            ";

            QRY _Qry = new QRY();
            return _Qry.GetData(query, new Dictionary<string, object>
            {
                { "@riderId", riderId }
            });
        }

        protected void LoadDeliveryChart()
        {
            string username = Session["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
                return;

            QRY _Qry = new QRY();
            string riderIdSql = "SELECT riderId FROM delivery_rider WHERE riderUsername = @username";
            DataTable dtRider = _Qry.GetData(riderIdSql, new Dictionary<string, object>
            {
                { "@username", username }
            });

            if (dtRider.Rows.Count > 0)
            {
                string riderId = dtRider.Rows[0]["riderId"].ToString();
                DataTable statsData = GetMonthlyDeliveryStats(riderId);

                List<string> labels = new List<string>();
                List<int> deliveryCounts = new List<int>();
                List<decimal> totalEarnings = new List<decimal>();

                foreach (DataRow row in statsData.Rows)
                {
                    labels.Add(row["Month"].ToString());
                    deliveryCounts.Add(Convert.ToInt32(row["DeliveryCount"]));
                    totalEarnings.Add(Convert.ToDecimal(row["TotalEarnings"]));
                }

                ClientScript.RegisterStartupScript(this.GetType(), "loadChart", $@"
           barChart({Newtonsoft.Json.JsonConvert.SerializeObject(labels)}, 
                    {Newtonsoft.Json.JsonConvert.SerializeObject(deliveryCounts)}, 
                    {Newtonsoft.Json.JsonConvert.SerializeObject(totalEarnings)});
                ", true);
            }
        }
    }
}