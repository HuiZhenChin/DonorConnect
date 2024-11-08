using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class OrgAnalytics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                GetCurrentTotalItem();
                GetLowStockItemCount();
                GetExpiredItem();
                GetMostUsedCategory();
                LoadRemainingQuantityCategory();
                
                LoadDistinctMonths();

                string currentMonthYear = DateTime.Now.ToString("MM-yyyy");

                // fetch the stock level data for the current month
                string stockLevelData = DisplayStockLevelOverTime(currentMonthYear);

                string itemData= DisplayItemStockOverTime(currentMonthYear);
                
                ClientScript.RegisterStartupScript(this.GetType(), "renderChartOnLoad",
                    $"<script type=\"text/javascript\">renderInventoryChart({stockLevelData});</script>");

                ClientScript.RegisterStartupScript(this.GetType(), "renderLineChartOnLoad",
                 $"<script type=\"text/javascript\">renderItemStockLevel({itemData});</script>");

            }
        }

        public class Item
        {
            public string item { get; set; }
            public string expiryDate { get; set; }
        }

        public class ItemData
        {
            public string Date { get; set; }
            public int qtyIn { get; set; }
            public int qtyOut { get; set; }
            public int cumulativeQty { get; set; }

            public ItemData(string date, int quantityIn, int quantityOut, int cumulativeQuantity)
            {
                Date = date;
                qtyIn = quantityIn;
                qtyOut = quantityOut;
                cumulativeQty = cumulativeQuantity;
            }
        }

        public class LowStockItem
        {
            public string item { get; set; }
            public int quantity { get; set; }
            public int threshold { get; set; }
        }

        protected void GetCurrentTotalItem()
        {
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = "SELECT ISNULL(SUM(CONVERT(INT, quantity)), 0) AS TotalItem FROM inventory WHERE orgId = @orgId";
            QRY _Qry = new QRY();

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                totalItemBox.Text = dt.Rows[0]["TotalItem"].ToString();
            }
            else
            {
                totalItemBox.Text = "0";
            }
        }

        protected void GetLowStockItemCount()
        {
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
            SELECT COUNT(*) AS lowStockCount
            FROM inventory
            WHERE ISNUMERIC(quantity) = 1
            AND quantity < ISNULL(threshold, 0)
            AND threshold IS NOT NULL
            AND orgId = @orgId"; 

            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                int lowStockCount = Convert.ToInt32(dt.Rows[0]["lowStockCount"]);
                lowStockBox.Text = lowStockCount.ToString();
            }
            else
            {
                lowStockBox.Text = "None";
            }
        }



        protected void GetExpiredItem()
        {
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string expiredQuery = "SELECT COUNT(*) AS ExpiredItem FROM inventory WHERE expiryDate < GETDATE() AND orgId= @orgId";
            string expiringSoonQuery = "SELECT COUNT(*) AS ExpiringItem FROM inventory WHERE expiryDate BETWEEN GETDATE() AND DATEADD(day, 7, GETDATE()) AND orgId= @orgId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            QRY _Qry = new QRY();

            DataTable dtExpired = _Qry.GetData(expiredQuery, parameters);
            DataTable dtExpiring = _Qry.GetData(expiringSoonQuery, parameters);

            if (dtExpired.Rows.Count > 0 && dtExpiring.Rows.Count > 0)
            {
                expiredBox.Text = dtExpired.Rows[0]["ExpiredItem"].ToString() + " Expired / " + dtExpiring.Rows[0]["ExpiringItem"].ToString() + " Expiring";
            }
        }

        private void GetMostUsedCategory()
        {
            string username = Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
             SELECT TOP 1 
                CASE 
                    WHEN itemCategory LIKE '%Food%' THEN 'Food'
                    ELSE itemCategory 
                END AS category,
                SUM(CONVERT(INT, quantityOut)) AS totalUsed
            FROM inventory_item_usage
            WHERE ISNUMERIC(quantityOut) = 1
            AND orgId= @orgId
            GROUP BY 
                CASE 
                    WHEN itemCategory LIKE '%Food%' THEN 'Food'
                    ELSE itemCategory 
                END
            ORDER BY totalUsed DESC";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql, parameters);

            if (dt.Rows.Count > 0)
            {
                mostUsedCategoryBox.Text = dt.Rows[0]["category"].ToString();
            }
            else
            {
                mostUsedCategoryBox.Text = "No Category";
            }
        }

        protected void LoadRemainingQuantityCategory()
        {
            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
           SELECT 
                CASE 
                    WHEN itemCategory LIKE '%Food%' THEN 'Food'
                    ELSE itemCategory
                END AS itemCategory,
                item, 
                SUM(CONVERT(INT, quantity)) AS remainingQty,
                ISNULL(threshold, 0) AS threshold -- Add threshold column to the query
            FROM inventory
            WHERE ISNUMERIC(quantity) = 1
            AND orgId = @orgId
            GROUP BY 
                CASE 
                    WHEN itemCategory LIKE '%Food%' THEN 'Food'
                    ELSE itemCategory
                END, 
                item, threshold;
            ";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql, parameters);

            // group data by category
            var groupedData = dt.AsEnumerable()
                .GroupBy(row => row["itemCategory"].ToString())
                .Select(group => new
                {
                    category = group.Key,
                    items = group.Select(item => new
                    {
                        label = item["item"].ToString(),
                        remainingQty = Convert.ToInt32(item["remainingQty"]),
                        threshold = Convert.ToInt32(item["threshold"])
                    }).ToList()
                }).ToList();

            // (Remaining Quantity and Threshold)
            var dataSource = groupedData.Select(categoryGroup => new
            {
                chart = new
                {
                    caption = $"{categoryGroup.category}",
                    xAxisName = "Items",
                    yAxisName = "Quantity",
                    theme = "fusion",
                    paletteColors = "#0075c2, #FF6961" // blue for remaining, red for threshold
                },
                categories = new[]
                {
            new
            {
                category = categoryGroup.items.Select(item => new { label = item.label }).ToList()
                }
            },
                dataset = new[]
                {
            new 
            {
                seriesname = "Remaining Quantity",
                data = categoryGroup.items.Select(item => new { value = item.remainingQty }).ToList()
            },
            new 
            {
                seriesname = "Threshold",
                data = categoryGroup.items.Select(item => new { value = item.threshold }).ToList()
                }
            }
            }).ToList();

            string jsonChartData = new JavaScriptSerializer().Serialize(dataSource);

            ClientScript.RegisterStartupScript(this.GetType(), "drawFusionChart", $"drawFusionChart({jsonChartData});", true);
        }


        [WebMethod]
        public static List<Item> GetExpiryDate()
        {
            string username = HttpContext.Current.Session["username"].ToString();

            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = "SELECT item, expiryDate FROM inventory WHERE expiryDate IS NOT NULL AND orgId = @orgId";
            QRY _Qry = new QRY();
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };
            DataTable dt = _Qry.GetData(sql, parameters);

            List<Item> items = new List<Item>();

            foreach (DataRow row in dt.Rows)
            {
                items.Add(new Item
                {
                    item = row["item"].ToString(),
                    expiryDate = Convert.ToDateTime(row["expiryDate"]).ToString("yyyy-MM-dd")
                });
            }

            return items;
        }

        [WebMethod]
        public static string MonthSelected(string monthYear)
        {
            return DisplayStockLevelOverTime(monthYear);
        }

        public static string DisplayStockLevelOverTime(string monthYear)
        {
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            QRY _Qry = new QRY();

            // filter by selected month (mm-yyyy)
            string sql = @"
            SELECT 
                CONVERT(VARCHAR(10), created_on, 120) AS created_on, 
                ISNULL(SUM(CAST(quantityIn AS INT)), 0) AS quantityIn, 
                ISNULL(SUM(CAST(quantityOut AS INT)), 0) AS quantityOut 
            FROM inventory_item_usage 
            WHERE FORMAT(created_on, 'MM-yyyy') = @monthYear
            AND orgId = @orgId
            GROUP BY CONVERT(VARCHAR(10), created_on, 120)
            ORDER BY created_on;
            ";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@monthYear", monthYear },
                { "@orgId", orgId } 
            };

            DataTable dt = _Qry.GetData(sql, parameters);

            var rows = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                rows.Add(new
                {
                    created_on = row["created_on"].ToString(),
                    quantityIn = row["quantityIn"].ToString(),
                    quantityOut = row["quantityOut"].ToString()
                });
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonData = serializer.Serialize(rows);

            return jsonData;
        }

        [WebMethod]
        public static string MonthSelectedItems(string monthYear)
        {
           return DisplayItemStockOverTime(monthYear);
        }

        public static string DisplayItemStockOverTime(string monthYear)
        {
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            // fetch the inventory usage data and filter by monthYear
            string sql = @"
            SELECT 
                itemCategory,
                item,
                ISNULL(quantityIn, 0) AS quantityIn,
                ISNULL(quantityOut, 0) AS quantityOut,
                created_on,
                inventoryId
            FROM inventory_item_usage
            WHERE orgId = @orgId
            AND FORMAT(created_on, 'MM-yyyy') = @MonthYear
            ORDER BY item, created_on;";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@MonthYear", monthYear }
            };

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql, parameters);

           
            // store data grouped by item
            var itemData = new Dictionary<string, List<ItemData>>();

            foreach (DataRow row in dt.Rows)
            {
                string item = row["item"].ToString();
                string date = Convert.ToDateTime(row["created_on"]).ToString("yyyy-MM-dd"); // Use only date
                int qtyIn = Convert.ToInt32(row["quantityIn"]);
                int qtyOut = Convert.ToInt32(row["quantityOut"]);

                if (!itemData.ContainsKey(item))
                {
                    itemData[item] = new List<ItemData>();
                }

                // get the last cumulative quantity if it exists
                int cumulativeQty = itemData[item].Count > 0 ? itemData[item].Last().cumulativeQty : 0;

                // update cumulative quantity: add quantityIn, subtract quantityOut
                cumulativeQty += qtyIn - qtyOut;

                itemData[item].Add(new ItemData(
                    date,  // (yyyy-MM-dd)
                    qtyIn,
                    qtyOut,
                    cumulativeQty
                ));
            }

            // accumulate usage for the same day
            var groupedItemData = itemData
                .GroupBy(item => item.Key)  
                .Select(itemGroup => new
                {
                    itemName = itemGroup.Key,

                    dates = itemGroup
                        .SelectMany(g => g.Value)  
                        .GroupBy(v => v.Date)  
                        .Select(g => new
                        {
                            label = g.Key,  // Date (yyyy-MM-dd)
                            finalQtyIn = g.Sum(x => x.qtyIn),  // sum of all quantityIn for this date
                            finalQtyOut = g.Sum(x => x.qtyOut),  // sum of all quantityOut for this date
                            cumulativeQty = g.Last().cumulativeQty  // last cumulative quantity of the day
                        })
                        .ToList()
                })
                .ToList();

            // sent to the frontend
            var chartData = groupedItemData.Select(item => new
            {
                itemName = item.itemName,
                dates = item.dates.Select(d => new { label = d.label }).ToList(), 
                cumulativeQty = item.dates.Select(d => new { value = d.cumulativeQty }).ToList(),  
                qtyInOut = item.dates.Select(d => new { qtyIn = d.finalQtyIn, qtyOut = d.finalQtyOut }).ToList()  // total in/out for each day
            }).ToList();

            string jsonChartData = new JavaScriptSerializer().Serialize(chartData);

            return jsonChartData;
        }



        [WebMethod]
        public static List<LowStockItem> GetLowStockItem()
        {
            // store low stock items
            List<LowStockItem> lowStockItems = new List<LowStockItem>();

            // fetch low stock items
            string sql = @"
            SELECT item, quantity, threshold 
            FROM inventory 
            WHERE ISNUMERIC(quantity) = 1 
            AND quantity < ISNULL(threshold, 0) 
            AND threshold IS NOT NULL 
            AND orgId = @orgId";

            QRY _Qry = new QRY();
            DataTable dt = _Qry.GetData(sql);

            foreach (DataRow row in dt.Rows)
            {
                lowStockItems.Add(new LowStockItem
                {
                    item = row["item"].ToString(),
                    quantity = Convert.ToInt32(row["quantity"]),
                    threshold = Convert.ToInt32(row["threshold"])
                });
            }

            return lowStockItems;
        }

        private void LoadDistinctMonths()
        {
            QRY _Qry = new QRY();

            // get months in 'MM-yyyy' format
            string sql = @"
                SELECT DISTINCT 
                    FORMAT(created_on, 'MM-yyyy') AS MonthYear 
                FROM inventory_item_usage 
                ORDER BY FORMAT(created_on, 'MM-yyyy') DESC;
            ";

            DataTable dt = _Qry.GetData(sql);

            ddlMonthFilter.Items.Clear();  
            ddlMonthFilter.Items.Add(new ListItem("Select Month", ""));  
            ddlMonthFilter2.Items.Clear();


            foreach (DataRow row in dt.Rows)
            {
                ddlMonthFilter.Items.Add(new ListItem(row["MonthYear"].ToString(), row["MonthYear"].ToString()));
                ddlMonthFilter2.Items.Add(new ListItem(row["MonthYear"].ToString(), row["MonthYear"].ToString()));
            }
        }


    }
}