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

            string orgId = Session["SelectedOrgId"] as string;

            string riderId = Session["SelectedRiderId"] as string;

            if (orgId!= null)
            {
                BindOrgGridView(orgId);
            }

            else if (riderId!= null)
            {
                BindRiderGridView(riderId);
            }
        }

        protected void gvOrg_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
        }

        protected void gvRider_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

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
                    string encryptedImages = row["FieldValue"].ToString();
                    // decrypt and process the images
                    string processedImagesHtml = ProcessImages(encryptedImages);
              
                    row["FieldValue"] = processedImagesHtml;
                }
            }

           
            rptOrg.DataSource = _dt;
            rptOrg.DataBind();
        }

        private void BindRiderGridView(string riderId)
        {
            QRY _Qry = new QRY();
            DataTable _dt;


            string sql = @"
            SELECT 
                'ID' AS FieldName, CAST(riderId AS NVARCHAR(MAX)) AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Username' AS FieldName, riderUsername AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Full Name' AS FieldName, riderFullName AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Email Address' AS FieldName, riderEmail AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Contact Number' AS FieldName, riderContactNumber AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Vehicle Type' AS FieldName, vehicleType AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Vehicle Plate Number' AS FieldName, vehiclePlateNumber AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Driving License' AS FieldName, drivingLicenseImageBase64 AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'
            UNION ALL
            SELECT 
                'Face Photo' AS FieldName, riderFacePicBase64 AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'  
            UNION ALL
            SELECT 
                'Date Registered' AS FieldName, 
                CAST(registerDate AS VARCHAR) AS FieldValue 
            FROM 
                delivery_rider
            WHERE 
                riderId = '" + riderId.Replace("'", "''") + @"'   
        ";


            _dt = _Qry.GetData(sql);

            // check for 'Business License' field
            foreach (DataRow row in _dt.Rows)
            {
                if (row["FieldName"].ToString() == "Driving License")
                {
                    string encryptedImages = row["FieldValue"].ToString();
                    // decrypt and process the images
                    string processedImagesHtml = ProcessImages(encryptedImages);

                    row["FieldValue"] = processedImagesHtml;
                }

                if (row["FieldName"].ToString() == "Face Photo")
                {
                    string encryptedImages = row["FieldValue"].ToString();
                    // decrypt and process the images
                    string processedImagesHtml = ProcessImages(encryptedImages);

                    row["FieldValue"] = processedImagesHtml;
                }
            }


            rptOrg.DataSource = _dt;
            rptOrg.DataBind();
        }

        private string ProcessImages(string encryptedBase64Images)
        {
            if (string.IsNullOrEmpty(encryptedBase64Images))
            {
                return string.Empty;
            }

            // decrypt the base64 images string
            string decryptedBase64Images = DecryptAndSaveFiles(encryptedBase64Images);

            // split the decrypted string into individual base64 image strings
            string[] base64ImageArray = decryptedBase64Images.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder imagesBuilder = new StringBuilder();
            imagesBuilder.AppendLine("<div class='image-grid'>");

            foreach (string base64Image in base64ImageArray)
            {
                imagesBuilder.AppendLine($"<div class='image-item'><img src='data:image/png;base64,{base64Image}' alt='Image' class='img-fluid' /></div>");
            }

            imagesBuilder.AppendLine("</div>");
            return imagesBuilder.ToString();
        }

        private string DecryptAndSaveFiles(string encryptedImg)
        {
            // split into individual encrypted image
            var fileEntries = encryptedImg.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder decryptedBase64ImagesBuilder = new StringBuilder();

            foreach (var fileEntry in fileEntries)
            {
                // split into exactly 2 parts: filename and encrypted image string
                var fileParts = fileEntry.Split(new[] { ':' }, 2);
                if (fileParts.Length != 2)
                {
                    throw new ArgumentException("Invalid encrypted file data format.");
                }

                string fileName = fileParts[0];
                string encryptedBase64String = fileParts[1];

                // decrypt the base64 string
                string decryptedBase64String = DecryptImages(encryptedBase64String);

                // add the decrypted base64 string to the result
                decryptedBase64ImagesBuilder.AppendLine(decryptedBase64String);
            }

           
            return decryptedBase64ImagesBuilder.ToString();
        }



        private string DecryptImages(string encryptedBase64String)
        {
            // AES-256 decryption setup
            byte[] keyBytes = Encoding.UTF8.GetBytes("telleveryoneilovedonorconnectDc!");
            byte[] ivBytes = Encoding.UTF8.GetBytes("16ByteInitVector");
            byte[] cipherBytes = Convert.FromBase64String(encryptedBase64String);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        // write approve and reject here 
        // add ssm verify link from gov

    }
}