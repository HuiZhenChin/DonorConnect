using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DonorConnect.Class;
using System.Web.Script.Services;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace DonorConnect
{
    public partial class OrgReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                PopulateExistingReports();

              
            }
        }

        private void PopulateExistingReports()
        {
            string folderPath = HttpContext.Current.Server.MapPath("~/Reports/");
            ddlExistingReports.Items.Clear();

            string username = Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            bool isReportFound = false;  

            if (Directory.Exists(folderPath))
            {
                string[] pdfFiles = Directory.GetFiles(folderPath, "*.pdf");

                // filter files based on orgId in the file name
                foreach (string filePath in pdfFiles)
                {
                    string fileName = Path.GetFileName(filePath);

                    // check if the file name contains the organization ID
                    if (fileName.Contains(orgId))
                    {
                        ddlExistingReports.Items.Add(new System.Web.UI.WebControls.ListItem(fileName, fileName));
                        isReportFound = true;  // a report is found
                    }
                }
            }

            if (!isReportFound)
            {
                ddlExistingReports.Items.Add(new System.Web.UI.WebControls.ListItem("No reports found", ""));
            }
            else
            {
                ddlExistingReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select an existing report", "") { Enabled = false, Selected = true });
            }
        }


        protected void btnDownloadReport_Click(object sender, EventArgs e)
        {
            string selectedFile = ddlExistingReports.SelectedValue;
            if (!string.IsNullOrEmpty(selectedFile))
            {
                string filePath = HttpContext.Current.Server.MapPath("~/Reports/") + selectedFile;
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + selectedFile);
                Response.TransmitFile(filePath);
                Response.End();
            }
        }

        public static void GenerateMonthlyReport(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();
            string reportMonth = $"{month:D2}-{year}"; 

            string folderPath = HttpContext.Current.Server.MapPath("~/Reports/");
            string filePath = folderPath + orgId + "_" + month + "_" + year + "_MonthlyReport.pdf";
            string fileUrl = "/Reports/" + orgId + "_" + month + "_" + year + "_MonthlyReport.pdf";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            DataTable itemUsage = FetchItemUsage(month, year);
            DataTable itemExpiry = FetchItemExpiryList();
            DataTable donorEngagement = FetchDonorEngagement(month, year);
            Dictionary<string, int> donationStatusBreakdown = FetchDonationStatusBreakdown(month, year);     
            DataTable lowStockItems = FetchLowStockItems();
            DataTable remainingStock = FetchRemainingStock();

            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

            writer.PageEvent = new PageEventHelper();
            doc.Open();
            // First Page: Cover Page
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD);
            Font headingFont = new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD);
            Font subtitleFont = new Font(Font.FontFamily.HELVETICA, 22, Font.NORMAL);

            PdfPTable coverTable = new PdfPTable(1) { WidthPercentage = 60, TotalWidth = doc.PageSize.Width - doc.LeftMargin - doc.RightMargin };
            coverTable.DefaultCell.Border = PdfPCell.NO_BORDER;

            PdfPCell coverCell = new PdfPCell();
            coverCell.Border = PdfPCell.NO_BORDER;
            coverCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            coverCell.HorizontalAlignment = Element.ALIGN_CENTER;
            coverCell.FixedHeight = doc.PageSize.Height - doc.TopMargin - doc.BottomMargin;

            coverCell.AddElement(new Paragraph(username, titleFont) { Alignment = Element.ALIGN_CENTER });

            Paragraph spacing = new Paragraph(" ") { SpacingBefore = 20 };  
            coverCell.AddElement(spacing);

            coverCell.AddElement(new Paragraph($"Monthly Report - {reportMonth}", subtitleFont) { Alignment = Element.ALIGN_CENTER });

            coverTable.AddCell(coverCell);

            doc.Add(coverTable);

            doc.NewPage();

            PdfPCell CreateHeaderCell(string text)
            {
                Font headerFont = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.WHITE);
                PdfPCell cell = new PdfPCell(new Phrase(text, headerFont))
                {
                    BackgroundColor = BaseColor.GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding = 13
                };
                return cell;
            }

            // Second Page: Donation Summary
            doc.Add(new Paragraph("Donation Summary", headingFont) { Alignment = Element.ALIGN_CENTER });
            PdfPTable summaryTable = new PdfPTable(2) { SpacingBefore = 20, SpacingAfter = 20, WidthPercentage = 80 };
            PdfPCell CreatePaddedCell(string text)
            {
                PdfPCell cell = new PdfPCell(new Phrase(text));
                cell.Padding = 8;  
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                return cell;
            }

            summaryTable.AddCell(CreateHeaderCell("Metric"));
            summaryTable.AddCell(CreateHeaderCell("Value"));

            summaryTable.AddCell(CreatePaddedCell("Total Donations Received"));
            summaryTable.AddCell(CreatePaddedCell(FetchDonationRequestCount(month, year).ToString()));
            summaryTable.AddCell(CreatePaddedCell("Total Items Donated"));
            summaryTable.AddCell(CreatePaddedCell(FetchItemCount(month, year).ToString()));
            summaryTable.AddCell(CreatePaddedCell("Top Donated Category"));
            summaryTable.AddCell(CreatePaddedCell(FetchTopDonatedCategory(month, year)));

            doc.Add(summaryTable);
            doc.NewPage();

            // Third Page: Item Usage and Inventory
            doc.Add(new Paragraph("Item Usage and Inventory", headingFont) { Alignment = Element.ALIGN_CENTER });
            PdfPTable itemUsageTable = new PdfPTable(5) { SpacingBefore = 20, SpacingAfter = 20, WidthPercentage = 100 };
            itemUsageTable.SetWidths(new float[] { 2f, 2f, 1f, 1f, 2f });
            itemUsageTable.AddCell(CreateHeaderCell("Item Category"));
            itemUsageTable.AddCell(CreateHeaderCell("Item"));
            itemUsageTable.AddCell(CreateHeaderCell("Quantity In"));
            itemUsageTable.AddCell(CreateHeaderCell("Quantity Out"));
            itemUsageTable.AddCell(CreateHeaderCell("Date"));
            foreach (DataRow row in itemUsage.Rows)
            {
                itemUsageTable.AddCell(CreatePaddedCell(row["itemCategory"].ToString()));
                itemUsageTable.AddCell(CreatePaddedCell(row["item"].ToString()));
                itemUsageTable.AddCell(CreatePaddedCell(row["quantityIn"].ToString()));
                itemUsageTable.AddCell(CreatePaddedCell(row["quantityOut"].ToString()));
                itemUsageTable.AddCell(CreatePaddedCell(row["created_on"].ToString()));
            }
            doc.Add(itemUsageTable);
            doc.NewPage();

            // Fourth Page: Low Stock Items
            doc.Add(new Paragraph("Current Low Stock Items", headingFont) { Alignment = Element.ALIGN_CENTER });
            PdfPTable lowStockTable = new PdfPTable(3) { SpacingBefore = 20, SpacingAfter = 20, WidthPercentage = 80 };
            lowStockTable.AddCell(CreateHeaderCell("Item Category"));
            lowStockTable.AddCell(CreateHeaderCell("Item"));
            lowStockTable.AddCell(CreateHeaderCell("Quantity"));
            foreach (DataRow row in lowStockItems.Rows)
            {
                lowStockTable.AddCell(CreatePaddedCell(row["itemCategory"].ToString()));
                lowStockTable.AddCell(CreatePaddedCell(row["item"].ToString()));
                lowStockTable.AddCell(CreatePaddedCell(row["quantity"].ToString()));
            }
            doc.Add(lowStockTable);
            doc.NewPage();

            // Fifth Page: Item Expiry List
            doc.Add(new Paragraph("Item Expiry List", headingFont) { Alignment = Element.ALIGN_CENTER });
            PdfPTable expiryTable = new PdfPTable(4) { SpacingBefore = 20, SpacingAfter = 20, WidthPercentage = 90 };
            expiryTable.AddCell(CreateHeaderCell("Item Category"));
            expiryTable.AddCell(CreateHeaderCell("Item"));
            expiryTable.AddCell(CreateHeaderCell("Quantity"));
            expiryTable.AddCell(CreateHeaderCell("Expiry Date"));
            foreach (DataRow row in itemExpiry.Rows)
            {
                expiryTable.AddCell(CreatePaddedCell(row["itemCategory"].ToString()));
                expiryTable.AddCell(CreatePaddedCell(row["item"].ToString()));
                expiryTable.AddCell(CreatePaddedCell(row["quantity"].ToString()));
                expiryTable.AddCell(CreatePaddedCell(Convert.ToDateTime(row["expiryDate"]).ToString("yyyy-MM-dd")));
            }
            doc.Add(expiryTable);
            doc.NewPage();

            // Sixth Page: Donor Engagement Analysis
            doc.Add(new Paragraph("Donor with Donations Analysis", headingFont) { Alignment = Element.ALIGN_CENTER });
            PdfPTable donorEngagementTable = new PdfPTable(5) { SpacingBefore = 20, SpacingAfter = 20, WidthPercentage = 90 };
            donorEngagementTable.AddCell(CreateHeaderCell("Donor Name"));
            donorEngagementTable.AddCell(CreateHeaderCell("Category"));
            donorEngagementTable.AddCell(CreateHeaderCell("Item"));
            donorEngagementTable.AddCell(CreateHeaderCell("Status"));
            donorEngagementTable.AddCell(CreateHeaderCell("Refund Reason"));

            foreach (DataRow row in donorEngagement.Rows)
            {
                string status = row["status"].ToString();
                PdfPCell statusCell = new PdfPCell(new Phrase(status))
                {
                    BackgroundColor = status == "Reached Destination" ? new BaseColor(144, 238, 144) :
                                      status == "Refund" ? new BaseColor(255, 182, 193) : BaseColor.WHITE
                };

                donorEngagementTable.AddCell(CreatePaddedCell(row["donorFullName"].ToString()));
                donorEngagementTable.AddCell(CreatePaddedCell(row["itemCategory"].ToString()));
                donorEngagementTable.AddCell(CreatePaddedCell(row["item"].ToString()));
                donorEngagementTable.AddCell(statusCell);
                donorEngagementTable.AddCell(CreatePaddedCell(row["refundReason"].ToString()));
            }
            doc.Add(donorEngagementTable);
            doc.NewPage();

            // Seventh Page: Donation Status Breakdown
            doc.Add(new Paragraph("Current Donation Status Breakdown", headingFont) { Alignment = Element.ALIGN_CENTER });
            PdfPTable statusTable = new PdfPTable(2) { SpacingBefore = 20, SpacingAfter = 20, WidthPercentage = 80 };
            statusTable.AddCell(CreateHeaderCell("Status"));
            statusTable.AddCell(CreateHeaderCell("Count"));
            foreach (var status in donationStatusBreakdown)
            {
                statusTable.AddCell(CreatePaddedCell(status.Key));
                statusTable.AddCell(CreatePaddedCell(status.Value.ToString()));
            }
            doc.Add(statusTable);
            doc.NewPage();

            // Eighth Page: Donor Region Pie Chart (Placeholder for actual chart image)
            doc.Add(new Paragraph("Geographical Distribution of Donors by State(s)", headingFont) { Alignment = Element.ALIGN_CENTER });
            AddDonorRegionChart(doc, month, year);
            doc.NewPage();

            doc.Close();

            string monthYear = new DateTime(year, month, 1).ToString("MMMM yyyy");
            HttpContext.Current.Session["AlertMessage"] = $"Your monthly report for {monthYear} has been successfully generated. You can download it from the Report Page.";

            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + orgId + "_" + month + "_" + year + "_MonthlyReport.pdf");
            HttpContext.Current.Response.TransmitFile(filePath);
            HttpContext.Current.Response.End();

        }


        public static int FetchDonationRequestCount(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT COUNT(DISTINCT donationId) 
                   FROM donation_item_request 
                   WHERE orgId = @orgId 
                     AND YEAR(created_on) = @year
                     AND MONTH(created_on) = @month";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@year", year },
                { "@month", month }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 0;
        }

        public static int FetchItemCount(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT quantityDonated 
                   FROM donation_item_request 
                   WHERE orgId = @orgId 
                     AND YEAR(created_on) = @year
                     AND MONTH(created_on) = @month";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@year", year },
                { "@month", month }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            int totalItemCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                string quantityStr = row["quantityDonated"].ToString();
                quantityStr = quantityStr.Replace("(", "").Replace(")", "");
                string[] quantities = quantityStr.Split(',');

                foreach (string qty in quantities)
                {
                    if (int.TryParse(qty.Trim(), out int parsedQty))
                    {
                        totalItemCount += parsedQty;
                    }
                }
            }

            return totalItemCount;
        }

        public static string FetchTopDonatedCategory(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT itemCategory 
                   FROM donation_item_request 
                   WHERE orgId = @orgId 
                     AND YEAR(created_on) = @year
                     AND MONTH(created_on) = @month";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@year", year },
                { "@month", month }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            Dictionary<string, int> categoryCounts = new Dictionary<string, int>();

            foreach (DataRow row in dt.Rows)
            {
                string category = row["itemCategory"].ToString();
                if (categoryCounts.ContainsKey(category))
                {
                    categoryCounts[category]++;
                }
                else
                {
                    categoryCounts[category] = 1;
                }
            }

            string topCategory = null;
            int maxCount = 0;
            foreach (var category in categoryCounts)
            {
                if (category.Value > maxCount)
                {
                    topCategory = category.Key;
                    maxCount = category.Value;
                }
            }

            return topCategory;
        }

        public static DataTable FetchItemUsage(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT itemCategory, item, quantityIn, quantityOut, 
                   FORMAT(created_on, 'yyyy-MM-dd') AS created_on 
                   FROM inventory_item_usage 
                   WHERE orgId = @orgId
                     AND YEAR(created_on) = @year
                     AND MONTH(created_on) = @month
                   ORDER BY created_on DESC";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@year", year },
                { "@month", month }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            return dt;
        }

        public static DataTable FetchRemainingStock()
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT itemCategory, item, quantity 
                   FROM inventory 
                   WHERE orgId = @orgId
                   ORDER BY itemCategory, item";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            return dt;
        }

        public static DataTable FetchLowStockItems()
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT itemCategory, item, quantity, threshold
                   FROM inventory 
                   WHERE orgId = @orgId";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            DataTable lowStockTable = new DataTable();
            lowStockTable.Columns.Add("itemCategory", typeof(string));
            lowStockTable.Columns.Add("item", typeof(string));
            lowStockTable.Columns.Add("quantity", typeof(int));

            foreach (DataRow row in dt.Rows)
            {
                if (int.TryParse(row["quantity"].ToString(), out int quantity) &&
                    int.TryParse(row["threshold"].ToString(), out int threshold))
                {
                    if (quantity < threshold)
                    {
                        DataRow lowStockRow = lowStockTable.NewRow();
                        lowStockRow["itemCategory"] = row["itemCategory"].ToString();
                        lowStockRow["item"] = row["item"].ToString();
                        lowStockRow["quantity"] = quantity;
                        lowStockTable.Rows.Add(lowStockRow);
                    }
                }
            }

            return lowStockTable;
        }

        public static DataTable FetchItemExpiryList()
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"SELECT itemCategory, item, quantity, expiryDate 
                   FROM inventory 
                   WHERE orgId = @orgId 
                   AND expiryDate IS NOT NULL
                   ORDER BY expiryDate ASC";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            return dt;
        }

        public static DataTable FetchDonorEngagement(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
            SELECT donorFullName, itemCategory, item, requestStatus, donorId, donationId, created_on 
            FROM donation_item_request 
            WHERE orgId = @orgId
              AND YEAR(created_on) = @year
              AND MONTH(created_on) = @month";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@year", year },
                { "@month", month }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            DataTable donorEngagementTable = new DataTable();
            donorEngagementTable.Columns.Add("donorFullName", typeof(string));
            donorEngagementTable.Columns.Add("itemCategory", typeof(string));
            donorEngagementTable.Columns.Add("item", typeof(string));
            donorEngagementTable.Columns.Add("status", typeof(string));
            donorEngagementTable.Columns.Add("refundReason", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                string status;
                string donorFullName = row["donorFullName"].ToString();
                string itemCategory = row["itemCategory"].ToString();
                string item = row["item"].ToString();
                string requestStatus = row["requestStatus"].ToString();
                string donationId = row["donationId"].ToString();
                string refundReason = string.Empty;

                if (requestStatus.Equals("rejected", StringComparison.OrdinalIgnoreCase))
                {
                    status = "Rejected";
                }
                else if (requestStatus.Equals("approved", StringComparison.OrdinalIgnoreCase))
                {
                    string deliverySql = @"SELECT deliveryStatus FROM delivery WHERE donationId = @donationId";
                    Dictionary<string, object> deliveryParams = new Dictionary<string, object>
                    {
                        { "@donationId", donationId }
                    };
                    DataTable deliveryDt = _Qry.GetData(deliverySql, deliveryParams);

                    if (deliveryDt.Rows.Count > 0)
                    {
                        status = deliveryDt.Rows[0]["deliveryStatus"].ToString();
                    }
                    else
                    {
                        status = "Approved";
                    }
                }
                else if (requestStatus.Equals("refund", StringComparison.OrdinalIgnoreCase))
                {
                    status = "Refund";
                    string refundSql = @"SELECT refundReason FROM delivery WHERE donationId = @donationId";
                    Dictionary<string, object> deliveryParams = new Dictionary<string, object>
                    {
                        { "@donationId", donationId }
                    };
                    DataTable refundDt = _Qry.GetData(refundSql, deliveryParams);

                    if (refundDt.Rows.Count > 0)
                    {
                        refundReason = refundDt.Rows[0]["refundReason"].ToString();
                    }
                }
                else
                {
                    status = requestStatus;
                }

                DataRow engagementRow = donorEngagementTable.NewRow();
                engagementRow["donorFullName"] = donorFullName;
                engagementRow["itemCategory"] = itemCategory;
                engagementRow["item"] = item;
                engagementRow["status"] = status;
                engagementRow["refundReason"] = refundReason;

                donorEngagementTable.Rows.Add(engagementRow);
            }

            return donorEngagementTable;
        }

        public static Dictionary<string, int> FetchDonationStatusBreakdown(int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
            SELECT donationId, deliveryStatus, acceptTimeByRider, pickupTimeByRider, reachTimeByRider 
            FROM delivery 
            WHERE orgId = @orgId
            AND (
                MONTH(acceptTimeByRider) = @month 
                OR MONTH(pickupTimeByRider) = @month 
                OR MONTH(reachTimeByRider) = @month
            )
            AND (
                YEAR(acceptTimeByRider) = @year
                OR YEAR(pickupTimeByRider) = @year
                OR YEAR(reachTimeByRider) = @year
            )
            AND (
                acceptTimeByRider IS NOT NULL 
                OR pickupTimeByRider IS NOT NULL 
                OR reachTimeByRider IS NOT NULL
            )";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@year", year },
                { "@month", month }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            Dictionary<string, int> statusCounts = new Dictionary<string, int>
            {
                { "Pending", 0 },
                { "Accepted", 0 },
                { "In Progress", 0 },
                { "Completed", 0 },
                { "Refund", 0 }
            };

            foreach (DataRow row in dt.Rows)
            {
                string deliveryStatus = row["deliveryStatus"].ToString();
                DateTime? acceptTime = row["acceptTimeByRider"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["acceptTimeByRider"]);
                DateTime? pickupTime = row["pickupTimeByRider"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["pickupTimeByRider"]);
                DateTime? reachTime = row["reachTimeByRider"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["reachTimeByRider"]);

                if (acceptTime != null && pickupTime == null && reachTime == null)
                {
                    statusCounts["Accepted"]++;
                }
                else if (pickupTime != null && reachTime == null)
                {
                    statusCounts["In Progress"]++;
                }
                else if (reachTime != null)
                {
                    statusCounts["Completed"]++;
                }
                else if (deliveryStatus == "Refund")
                {
                    statusCounts["Refund"]++;
                }
                else
                {
                    statusCounts["Pending"]++;
                }
            }

            return statusCounts;
        }

        public static void GenerateDonorRegionsPieChart(string chartFilePath, int month, int year)
        {
            QRY _Qry = new QRY();
            string username = HttpContext.Current.Session["username"].ToString();
            Organization org = new Organization(username, "", "", "", "");
            string orgId = org.GetOrgId();

            string sql = @"
            SELECT state, COUNT(DISTINCT donationId) AS donorCount
            FROM donation_item_request
            WHERE orgId = @orgId
              AND MONTH(created_on) = @month
              AND YEAR(created_on) = @year
            GROUP BY state
            ORDER BY donorCount DESC";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@orgId", orgId },
                { "@month", month },
                { "@year", year }
            };

            DataTable dt = _Qry.GetData(sql, parameters);
            Dictionary<string, int> donorRegionsData = new Dictionary<string, int>();
            foreach (DataRow row in dt.Rows)
            {
                string state = row["state"].ToString();
                int donorCount = Convert.ToInt32(row["donorCount"]);
                donorRegionsData[state] = donorCount;
            }

            using (var chart = new Chart())
            {
                chart.Width = 400;
                chart.Height = 400;

                ChartArea chartArea = new ChartArea();
                chart.ChartAreas.Add(chartArea);

                Series series = new Series
                {
                    Name = "DonorRegion",
                    IsValueShownAsLabel = true,
                    ChartType = SeriesChartType.Pie
                };

                chart.Series.Add(series);
                foreach (var entry in donorRegionsData)
                {
                    series.Points.AddXY(entry.Key, entry.Value);
                }

                series["PieLabelStyle"] = "Inside";
                series["DoughnutRadius"] = "60";
                chart.Legends.Add(new Legend("Legend"));

                chart.SaveImage(chartFilePath, ChartImageFormat.Png);
            }
        }

        public static void AddDonorRegionChart(Document doc, int month, int year)
        {
            string chartDirectory = HttpContext.Current.Server.MapPath("~/OrgReportChart/");
            string chartFilePath = Path.Combine(chartDirectory, "DonorRegionPieChart.png");

            if (!Directory.Exists(chartDirectory))
            {
                Directory.CreateDirectory(chartDirectory);
            }

            GenerateDonorRegionsPieChart(chartFilePath, month, year);

            if (File.Exists(chartFilePath))
            {
                iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(chartFilePath);
                chartImage.ScaleToFit(500f, 400f);
                chartImage.Alignment = Element.ALIGN_CENTER;
                doc.Add(chartImage);
            }
            else
            {
                doc.Add(new Paragraph("Donor Region Chart not available"));
            }
        }

        protected void ddlExistingReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFile = ddlExistingReports.SelectedValue;
            if (!string.IsNullOrEmpty(selectedFile))
            {
               
                pdfViewer.Attributes["src"] = ResolveUrl("~/Reports/" + selectedFile);
            }
            else
            {

                pdfViewer.Attributes["src"] = ResolveUrl("NoFileChosen.html");
            }
        }

    }
}
