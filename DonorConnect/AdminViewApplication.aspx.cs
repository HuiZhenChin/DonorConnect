using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class AdminViewApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string reject= Request.QueryString["reject"];

            if (!string.IsNullOrEmpty(id))
            {
                if (id.StartsWith("r", StringComparison.OrdinalIgnoreCase))
                {
                    string riderId = id;

                    BindRiderGridView(riderId);
                    
                }
                else if (id.StartsWith("o", StringComparison.OrdinalIgnoreCase))
                {
                    string orgId = id;

                    
                    BindOrgGridView(orgId);
                    
                }
            }

        }

       
        private void BindOrgGridView(string orgId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;


            string sql = @"
            SELECT 
                'ID' AS FieldName, CAST(orgId AS NVARCHAR(MAX)) AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Name' AS FieldName, orgName AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Email Address' AS FieldName, orgEmail AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Contact Number' AS FieldName, orgContactNumber AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Address' AS FieldName, orgAddress AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Person In-Charge''s Name' AS FieldName, picName AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Person In-Charge''s Email Address' AS FieldName, picEmail AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Person In-Charge''s Contact Number' AS FieldName, picContactNumber AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Description' AS FieldName, orgDescription AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Region' AS FieldName, orgRegion AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Business License' AS FieldName, businessLicenseImageBase64 AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Date Registered' AS FieldName, CAST(createdOn AS NVARCHAR(MAX)) AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Status' AS FieldName, orgStatus AS FieldValue 
            FROM 
                organization
            WHERE 
                orgId = '" + orgId.Replace("'", "''") + @"'
        ";


            _dt = _Qry.GetData(sql);

            // check for 'Business License' field
            foreach (DataRow row in _dt.Rows)
            {
                if (row["FieldName"].ToString() == "Business License")
                {
                    string decryptedImages = row["FieldValue"].ToString();
                    // decrypt and process the images
                    string processedImagesHtml = ImageFileProcessing.ProcessImages(decryptedImages);

                    Session["DecryptedBusinessLicense"] = decryptedImages;

                    row["FieldValue"] = processedImagesHtml;
                }
            }


            rptOrg.DataSource = _dt;
            rptOrg.DataBind();

            rptRider.Visible = false;
            rptOrg.Visible = true;

        }

        protected void rptOrg_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal litLabel = (Literal)e.Item.FindControl("litLabel");
                HyperLink hyperLinkLicense = (HyperLink)e.Item.FindControl("hyperLinkLicense");

                if (litLabel != null && hyperLinkLicense != null)
                {
                    // check if the FieldName is "Business License"
                    if (litLabel.Text == "Business License")
                    {
                        // show the hyperlink
                        hyperLinkLicense.Visible = true;
                    }
                }
            }

            LinkButton btnApproveOrg = (LinkButton)e.Item.FindControl("btnApproveOrg");
            LinkButton btnRejectOrg = (LinkButton)e.Item.FindControl("btnRejectOrg");

            string reject = Request.QueryString["reject"];


            if (reject == "Yes")
            {
                if (btnApproveOrg != null)
                {
                    btnApproveOrg.Visible = false;
                }

                if (btnRejectOrg != null)
                {
                    btnRejectOrg.Visible = false;
                }
                else
                {
                    if (btnApproveOrg != null)
                    {
                        btnApproveOrg.Visible = true;

                        if (btnRejectOrg != null)
                        {
                            btnRejectOrg.Visible = true;
                        }
                    }

                }
            }
        }


        protected void rptRider_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

            }
            LinkButton btnApproveRider = (LinkButton)e.Item.FindControl("btnApproveRider");
            LinkButton btnRejectRider = (LinkButton)e.Item.FindControl("btnRejectRider");

            string reject = Request.QueryString["reject"];


            if (reject == "Yes")
            {
                if (btnApproveRider != null)
                {
                    btnApproveRider.Visible = false;
                }

                if (btnRejectRider != null)
                {
                    btnRejectRider.Visible = false;
                }
                else
                {
                    if (btnApproveRider != null)
                    {
                        btnApproveRider.Visible = true;

                        if (btnRejectRider != null)
                        {
                            btnRejectRider.Visible = true;
                        }
                    }

                }
            }
            
        }

        private void BindRiderGridView(string riderId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;


            string sql = @"
            SELECT 
                'ID' AS FieldName2, CAST(riderId AS NVARCHAR(MAX)) AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Username' AS FieldName2, riderUsername AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Full Name' AS FieldName2, riderFullName AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Email Address' AS FieldName2, riderEmail AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Contact Number' AS FieldName2, riderContactNumber AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Vehicle Type' AS FieldName2, vehicleType AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Vehicle Plate Number' AS FieldName2, vehiclePlateNumber AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Driving License' AS FieldName2, drivingLicenseImageBase64 AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Face Photo' AS FieldName2, riderFacePicBase64 AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'  
            UNION ALL
            SELECT 
                'Date Registered' AS FieldName2, 
                CAST(registerDate AS VARCHAR) AS FieldValue2 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'   
        ";


            _dt = _Qry.GetData(sql);

            // check for 'Business License' field
            foreach (DataRow row in _dt.Rows)
            {
                if (row["FieldName2"].ToString() == "Driving License")
                {
                    string decryptedImages = row["FieldValue2"].ToString();
                    // decrypt and process the images
                    string processedImagesHtml = ImageFileProcessing.ProcessImages(decryptedImages);

                    Session["DecryptedDrivingLicenseImage"] = decryptedImages;

                    row["FieldValue2"] = processedImagesHtml;
                }

                if (row["FieldName2"].ToString() == "Face Photo")
                {
                    string decryptedImages = row["FieldValue2"].ToString();
                    // decrypt and process the images
                    string processedImagesHtml = ImageFileProcessing.ProcessImages(decryptedImages);

                    Session["DecryptedFacePhoto"] = decryptedImages;

                    row["FieldValue2"] = processedImagesHtml;
                }
            }


            rptRider.DataSource = _dt;
            rptRider.DataBind();

            rptRider.Visible = true;
            rptOrg.Visible = false;
        }

        

        // write approve and reject here 
        // add ssm verify link from gov
        protected void btnApproveOrg_click(object sender, EventArgs e)
        {
            string orgId = Session["SelectedOrgId"] as string;
            string adminId = Session["username"].ToString();
            string status = "Active";

            string sql = "UPDATE [organization] SET " +
                                "orgStatus = '" + status + "', " +
                                "adminId = '" + adminId + "' " +
                                 "WHERE orgId = '" + orgId + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);

            if (success)
            {
                // send email notify organization
                string sqlemail;

                QRY _Qry2 = new QRY();


                sqlemail = "EXEC [application_email] " +
                             "@action = 'APPROVED', " +
                             "@reason = NULL, " +
                             "@role = 'organization', " +
                             "@resubmitLink = NULL, " +
                             "@orgId = '" + orgId + "' ";

                _Qry2.ExecuteNonQuery(sqlemail);
       
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Application approved successfully!',);", true);

                BindOrgGridView(orgId);

               
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error approving application. Please try again!');", true);
            }
        }

        protected void btnApproveRider_click(object sender, EventArgs e)
        {
            string riderId = Session["SelectedRiderId"] as string;
            string adminId = Session["username"].ToString();
            string status = "Active";

            string sql = "UPDATE [delivery_rider] SET " +
                                "riderStatus = '" + status + "', " +
                                "adminId = '" + adminId + "' " +
                                 "WHERE riderId = '" + riderId + "'";

            QRY _Qry = new QRY();
            bool success = _Qry.ExecuteNonQuery(sql);

            if (success)
            {
                // send email notify organization
                string sqlemail;

                QRY _Qry2 = new QRY();

                sqlemail = "EXEC [application_email] " +
                             "@action = 'APPROVED', " +
                             "@reason = NULL, " +
                             "@role = 'rider', " +
                             "@resubmitLink = NULL, " +
                             "@riderId = '" + riderId + "' ";

                _Qry2.ExecuteNonQuery(sqlemail);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Application approved successfully!',);", true);

                BindRiderGridView(riderId);

                
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error approving application. Please try again!');", true);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string orgId = Session["SelectedOrgId"] as string;
            string adminId = Session["username"].ToString();
            string rejectionReason = txtReason.Text;
            string riderId = Session["SelectedRiderId"] as string;
            string status = "Rejected";
            string orgToken = Hash(orgId + DateTime.Now.ToString());
            string riderToken = Hash(riderId + DateTime.Now.ToString());
            bool isRiderRejected = false;
            bool isOrgRejected = false;
            string resubmit = "No";

            if (!string.IsNullOrEmpty(riderId) && !string.IsNullOrEmpty(rejectionReason))
            {
 
                string sql = "UPDATE [delivery_rider] SET " +
                             "riderStatus = '" + status + "', " +
                             "adminId = '" + adminId + "', " +
                             "rejectedReason = '" + rejectionReason + "', " +
                             "resubmitApplication = '" + resubmit + "' " +
                             "WHERE riderId = '" + riderId + "'";

                QRY _Qry = new QRY();
                bool success = _Qry.ExecuteNonQuery(sql);

                if (success)
                {
                    // generate URL for rider resubmit
                    string resubmitLink = "https://localhost:44390/ApplicationResubmit.aspx?riderId=" + riderId + "&token=" + riderToken;

                    string sqlemail = "EXEC [application_email] " +
                                      "@action = 'REJECTED', " +
                                      "@reason = '" + rejectionReason + "', " +
                                      "@role = 'rider', " +
                                      "@riderId = '" + riderId + "', " +
                                      "@resubmitLink = '" + resubmitLink + "'";

                    _Qry.ExecuteNonQuery(sqlemail);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Application is rejected! An email is sent to inform them about this.',);", true);

                    BindRiderGridView(riderId);
                    isRiderRejected = true;

                }
            }

            if (!string.IsNullOrEmpty(orgId) && !string.IsNullOrEmpty(rejectionReason))
            {

                string sql2 = "UPDATE [organization] SET " +
                              "orgStatus = '" + status + "', " +
                              "adminId = '" + adminId + "', " +
                              "rejectedReason = '" + rejectionReason + "', " +
                              "resubmitApplication = '" + resubmit + "' " +
                              "WHERE orgId = '" + orgId + "'";

                QRY _Qry2 = new QRY();
                bool success2 = _Qry2.ExecuteNonQuery(sql2);

                if (success2)
                {
                    // generate URL for organization resubmit
                    string resubmitLink = "https://localhost:44390/ApplicationResubmit.aspx?orgId=" + orgId + "&token=" + orgToken;

                  
                    string sqlemail = "EXEC [application_email] " +
                                      "@action = 'REJECTED', " +
                                      "@reason = '" + rejectionReason + "', " +
                                      "@role = 'organization', " +
                                      "@orgId = '" + orgId + "', " +
                                      "@resubmitLink = '" + resubmitLink + "'";

                    _Qry2.ExecuteNonQuery(sqlemail);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Application is rejected! An email is sent to inform them about this.',);", true);

                    BindOrgGridView(orgId);
                    isOrgRejected = true;
                    
                }
            }

            if (isRiderRejected || isOrgRejected)
            {
                Session["SelectedOrgId"] = null;
                Session["SelectedRiderId"] = null;
            }

            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('There was an error rejecting the application. Please try again!');", true);
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