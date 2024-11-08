using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class OrgCreateSignature : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSignaturePIC();
            }
        }

        private void LoadSignaturePIC()
        {
            string username = Session["username"].ToString();

            QRY _Qry = new QRY();

            string sql = "SELECT signature, signaturePIC FROM organization WHERE orgName = @username";

            var parameters = new Dictionary<string, object>
            {
                { "@username", username }
            };

            DataTable result = _Qry.GetData(sql, parameters);

            if (result.Rows.Count > 0)
            {
                string signatureBase64 = result.Rows[0]["signature"].ToString();
                string personInChargeName = result.Rows[0]["signaturePIC"].ToString();

                signatureBase64= ImageFileProcessing.DecryptImages(signatureBase64);

                // if signature exists
                if (!string.IsNullOrEmpty(signatureBase64))
                {
                    signatureDB.Src = "data:image/png;base64," + signatureBase64;
                    signatureDB.Style["display"] = "block";
                }
                else
                {
                    signatureDB.Style["display"] = "none";
                }

                // fill the textbox for person-in-charge if value exists
                if (!string.IsNullOrEmpty(personInChargeName))
                {
                    txtPersonInCharge.Text = personInChargeName;
                }
            }
        }

        [WebMethod]
        public static void SaveSignature(string imageBase64)
        {
   
            string base64Data = Regex.Replace(imageBase64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            SaveSignatureToDatabase(base64Data);


        }

        private static void SaveSignatureToDatabase(string base64Data)
        {
            string username = HttpContext.Current.Session["username"].ToString();

            QRY _Qry = new QRY();

            string encryptedBase64 = ImageFileProcessing.EncryptStringAES(base64Data);

            string updateSql = $"UPDATE organization SET signature = @signature WHERE orgName = @username";

            var parameters = new Dictionary<string, object>
            {
                { "@signature", encryptedBase64 },
                { "@username", username }
            };

            _Qry.ExecuteNonQuery(updateSql, parameters);

            
        }

        protected void btnSavePIC_Click(object sender, EventArgs e)
        {
            string personInChargeName = txtPersonInCharge.Text;

           
            if (!string.IsNullOrEmpty(personInChargeName))
            {
                string username = Session["username"].ToString();

                QRY _Qry = new QRY();

                string updateSql = $"UPDATE organization SET signaturePIC = @personInChargeName WHERE orgName = @username";

                var parameters = new Dictionary<string, object>
                {
                    { "@personInChargeName", personInChargeName },
                    { "@username", username }
                };

                bool success = _Qry.ExecuteNonQuery(updateSql, parameters);

                if (success)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"showSuccess('Person-in-charge saved successfully');", true);
                }

                else 
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in saving person in-charge. Please try again.',);", true);

                }


            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in saving person in-charge. Please try again.',);", true);

            }
        }

      

    }
}