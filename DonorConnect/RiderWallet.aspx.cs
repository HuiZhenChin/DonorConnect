using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DonorConnect
{
    public partial class RiderWallet2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["riderId"];

                if (!string.IsNullOrEmpty(id))
                {
                    LoadWalletAmount(id);
                }
                else
                {
                    LoadWalletAmount();
                }
            }
        }


        private void LoadWalletAmount()
        {
            string username = Session["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                walletAmount.InnerText = "0.00";
                return;
            }

            QRY _Qry = new QRY();
            string riderIdSql = "SELECT riderId FROM delivery_rider WHERE riderUsername = @username";
            string walletAmountSql = "SELECT walletAmount FROM delivery_rider WHERE riderId = @riderId";

            try
            {
        
                DataTable dt = _Qry.GetData(riderIdSql, new Dictionary<string, object>
                {
                    { "@username", username }
                });

                if (dt.Rows.Count > 0)
                {
                    string riderId = dt.Rows[0]["riderId"].ToString();

                    DataTable dt2 = _Qry.GetData(walletAmountSql, new Dictionary<string, object>
                    {
                        { "@riderId", riderId }
                    });

                    if (dt2.Rows.Count > 0)
                    {
                        string amountString = dt2.Rows[0]["walletAmount"].ToString();

                        // remove "RM" prefix and trim any spaces
                        amountString = amountString.Replace("RM", "").Trim();

                        if (decimal.TryParse(amountString, out decimal amount))
                        {
                            walletAmount.InnerText = amount.ToString("F2");
                        }
                        else
                        {
                            walletAmount.InnerText = "0.00";
                        }
                    }
                    else
                    {
                        walletAmount.InnerText = "0.00";
                    }
                }
                else
                {
                    walletAmount.InnerText = "0.00";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching wallet amount: " + ex.Message);
                walletAmount.InnerText = "0.00";
            }
        }

        private void LoadWalletAmount(string riderId)
        {
            if (string.IsNullOrEmpty(riderId))
            {
                walletAmount.InnerText = "0.00";
                return;
            }

            // retrieve the wallet amount for the specific rider
            QRY _Qry = new QRY();
            string walletAmountSql = "SELECT walletAmount FROM delivery_rider WHERE riderId = @riderId";
            var parameters = new Dictionary<string, object>
            {
                { "@riderId", riderId }
            };

            try
            {
              
                DataTable dt2 = _Qry.GetData(walletAmountSql, parameters);

                if (dt2.Rows.Count > 0)
                {
                    string amountString = dt2.Rows[0]["walletAmount"].ToString();

                    // remove "RM" prefix and any extra spaces
                    amountString = amountString.Replace("RM", "").Trim();

                    // decimal
                    if (decimal.TryParse(amountString, out decimal amount))
                    {
                        walletAmount.InnerText = amount.ToString("F2");
                    }
                    else
                    {
                        walletAmount.InnerText = "0.00"; 
                    }
                }
                else
                {
                    walletAmount.InnerText = "0.00";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching wallet amount: " + ex.Message);
                walletAmount.InnerText = "0.00";
            }
        }


        protected void btnCashOut_Click(object sender, EventArgs e)
        {
            decimal walletBalance = Convert.ToDecimal(walletAmount.InnerText);

            // show the cash out section 
            ClientScript.RegisterStartupScript(this.GetType(), "showCashOutSection", $@"
            document.getElementById('cashOutSection').style.display = 'block';
            document.getElementById('txtCashOutAmount').setAttribute('max', {walletBalance});
        ", true);
            }

        protected void btnConfirmCashOut_Click(object sender, EventArgs e)
        {
            decimal walletBalance = Convert.ToDecimal(walletAmount.InnerText);
            decimal cashOutAmount;

            if (decimal.TryParse(txtCashOutAmount.Text, out cashOutAmount))
            {
                if (cashOutAmount >= 10 && cashOutAmount <= walletBalance)
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "showCreditCardSection", @"
                document.getElementById('creditCardSection').style.display = 'block';
            ", true);
                    btnCashOut.Style["display"] = "none";
                }
                else
                {
                    cashOutError.InnerText = "Please enter an amount between RM 10.00 and your current wallet balance.";
                    cashOutError.Style["display"] = "block";
                    ClientScript.RegisterStartupScript(this.GetType(), "showCashOutSection", "document.getElementById('cashOutSection').style.display = 'block';", true);

                }
            }
            else
            {
                cashOutError.InnerText = "Invalid cash-out amount.";
                cashOutError.Style["display"] = "block";
            }
        }

        protected void btnSubmitCashOut_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            string username = Session["username"].ToString();
            QRY _Qry = new QRY();

            string sqlSendOtp = "EXEC [wallet] @action= @action, @riderUsername = @riderUsername, @otp = @otp, @riderEmail= @riderEmail";
            var otpParameters = new Dictionary<string, object>
            {
                { "@action", "OTP" },
                { "@riderUsername", username },
                { "@otp", otp },
                { "@riderEmail", txtEmailOTP.Text },
            };

            bool otpSent = _Qry.ExecuteNonQuery(sqlSendOtp, otpParameters);

            if (otpSent)
            {
                // store OTP in session and set expiration
                Session["Otp"] = otp;
                Session["OtpExpiration"] = DateTime.Now.AddMinutes(2);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "otpScript", @"
            Swal.fire({
                title: 'OTP Sent!',
                text: 'Please check your inbox. OTP has been sent to " + txtEmailOTP.Text + @"',
                icon: 'success',
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Ensure sections are displayed
                    document.getElementById('creditCardSection').style.display = 'block';
                    document.getElementById('otp').style.display = 'block';
                }
            });
        ", true);


                btnSubmitCashOut.Attributes["style"] = "display: none;";
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "errorScript", "showError('There was an error sending OTP. Please try again!');", true);
            }
        }


        protected void btnValidateOtp_Click(object sender, EventArgs e)
        {
            // check if OTP is still valid (not expired)
            if (Session["OtpExpiration"] != null && DateTime.Now > (DateTime)Session["OtpExpiration"])
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('The OTP has expired. Please request a new OTP.');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "showOtpDiv", "document.getElementById('otp').style.display = 'block';", true);
                txtEmailOTP.Text = "";
                return;
            }

            // retrieve OTP from session and entered OTP from input
            string sessionOtp = Session["Otp"]?.ToString();
            string enteredOtp = txtOTP.Text.Trim();

            if (sessionOtp == enteredOtp)
            {
                string username = Session["username"]?.ToString();

                if (string.IsNullOrEmpty(username))
                {
                    return;
                }

                decimal cashOutAmount;
                if (decimal.TryParse(txtCashOutAmount.Text, out cashOutAmount))
                {
                    QRY _Qry = new QRY();

                    // retrieve the current wallet amount
                    string sql = "SELECT walletAmount FROM delivery_rider WHERE riderUsername = @username";
                    DataTable dt = _Qry.GetData(sql, new Dictionary<string, object>
                    {
                        { "@username", username }
                    });

                    if (dt.Rows.Count > 0)
                    {
                        string walletAmount = dt.Rows[0]["walletAmount"].ToString().Replace("RM", "").Trim();
                        decimal originalAmount = Convert.ToDecimal(walletAmount);

                        // calculate the new wallet amount
                        decimal newWalletAmount = originalAmount - cashOutAmount;

                        // update the wallet amount in the database
                        string sql2 = "UPDATE delivery_rider SET walletAmount = @newWalletAmount WHERE riderUsername = @username";
                        _Qry.ExecuteNonQuery(sql2, new Dictionary<string, object>
                        {
                            { "@newWalletAmount", newWalletAmount },
                            { "@username", username }
                        });

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showSuccess('Cash-out successful! Your new wallet balance is RM" + newWalletAmount.ToString("F2") + "');", true);
                        LoadWalletAmount();
                        btnSubmitCashOut.Attributes["style"] = "display: block;";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Error in cashing out. Please try again!');", true);
                    }
                }


            }
            else
            {
                // OTP is invalid
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "showError('Invalid OTP. Please try again.');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "showOtpDiv", "document.getElementById('otp').style.display = 'block';", true);
                txtEmailOTP.Text = "";
                btnSubmitCashOut.Style["display"] = "block";
                return;
            }

        }

    }
}