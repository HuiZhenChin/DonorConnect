<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RiderWallet.aspx.cs" Inherits="DonorConnect.RiderWallet2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Wallet</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        body{
            background: rgb(226,226,182);
            background: linear-gradient(180deg, rgba(226,226,182,1) 19%, rgba(110,172,218,1) 69%, rgba(3,52,110,1) 90%, rgba(7,44,77,1) 100%);
        }
        .wallet-card {
            background-color: #4A90E2;
            color: white;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            border: solid 2px #304463;
        }

        .wallet-amount {
            font-weight: bold;
        }

        .box-container {
            background-color: #f8f9fa;
            border: 1px solid #ddd;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4" style="margin-top: 50px;">
        <div class="row">
            <!-- Wallet Amount -->
           <div class="col-md-6 d-flex align-items-center justify-content-center mx-auto">
                <div class="wallet-card" style="width: 100%; margin-top: 100px; padding: 20px;">
                    <div class="credit-card-logos position-absolute" style="right: 25px;">
                        <img class="img-responsive cc-img" src="https://upload.wikimedia.org/wikipedia/commons/0/04/Visa.svg" alt="Visa" style="height: 40px; margin-right: 10px;" />
                        <img class="img-responsive cc-img" src="https://upload.wikimedia.org/wikipedia/commons/a/a4/Mastercard_2019_logo.svg" alt="MasterCard" style="height: 40px;" />
                    </div>
                <h3>Wallet Balance</h3>
                <p class="wallet-amount display-4">
                    RM<span id="walletAmount" runat="server"></span>
                </p>
                <asp:Button ID="btnCashOut" runat="server" Text="Cash Out" CssClass="btn btn-secondary mt-3" OnClick="btnCashOut_Click" style="float: right; background-color: #0F1035;"/>
        
                <!-- Cash Out Amount Input (hidden initially) -->
                <div id="cashOutSection" style="display: none; margin-top: 15px; padding-top: 50px;">
                    <asp:Label runat="server" Text="Enter Cash Out Amount" style="font-weight: bold; padding-bottom: 10px;" />
                    <asp:TextBox ID="txtCashOutAmount" runat="server" CssClass="form-control" 
                    placeholder="0.00" oninput="formatCashOutAmount(this)" />
                    <span id="cashOutError" class="text-danger" style="display: none;" runat="server"></span>
                    <asp:Button ID="btnConfirmCashOut" runat="server" Text="Confirm Cash Out" CssClass="btn btn-primary mt-2" OnClick="btnConfirmCashOut_Click" style="float: right; background-color: #365486;"/>

                </div>

                <!-- Credit Card Details Section (hidden initially) -->
                <div id="creditCardSection" style="display: none; margin-top: 20px;">
                    <div class="form-group">
                        <asp:Label runat="server" Text="Card Number" style="font-weight: bold;" />
                        <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" placeholder="Valid Card Number" MaxLength="19" oninput="formatCreditCardNumber(this)" onkeypress="allowOnlyNumber(event)" />
                        <span id="cardNumberError" class="text-danger" style="display: none;"></span>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" Text="Cardholder Name" style="font-weight: bold;" />
                        <asp:TextBox ID="txtCardOwner" runat="server" CssClass="form-control text-uppercase" placeholder="Card Owner Name" />
                        <span id="cardholderError" class="text-danger" style="display: none;"></span>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" Text="Expiration Date (MM/YY)" style="font-weight: bold;" />
                        <asp:TextBox ID="txtExpirationDate" runat="server" CssClass="form-control" placeholder="MM / YY" MaxLength="5" oninput="formatExpiryDate(this)" />
                        <span id="expiryError" class="text-danger" style="display: none;"></span>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" Text="CVV/ CVC Code" style="font-weight: bold;" />
                        <asp:TextBox ID="txtCVVCode" runat="server" CssClass="form-control" placeholder="CVV" MaxLength="3" onkeypress="allowOnlyNumber(event)" />
                        <span id="cvvError" class="text-danger" style="display: none;"></span>
                    </div>

                    <div class="form-group">
                        <asp:Label runat="server" Text="Email for OTP" style="font-weight: bold;" />
                        <asp:TextBox ID="txtEmailOTP" runat="server" CssClass="form-control" placeholder="Email Address" TextMode="Email" />
                        <small class="form-text text-muted"><em style="color: black;">One-Time Password (OTP) will be sent to this email for verification.</em></small>
                    </div>

                    <asp:Button ID="btnSubmitCashOut" runat="server" Text="Submit" OnClick="btnSubmitCashOut_Click" CssClass="btn btn-success mt-2" style="float: right; background-color: #12486B;"/>
                 
                    <div class="form-group" id="otp" AutoPostBack="false" style="display: none;">
                        <asp:Label runat="server" Text="Enter OTP" style="font-weight: bold;" Id="otpLbl"/>
                        <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" placeholder="OTP" MaxLength="8" onkeypress="allowOnlyNumber(event)" />
                        <asp:Button ID="btnValidateOtp" runat="server" Text="Validate OTP" CssClass="btn btn-primary mt-2" OnClick="btnValidateOtp_Click" style="float: right; background-color: #06283D;"/>
                    </div>
                </div>
            </div>
        </div>
      </div>
   </div>
       
    <script>
      

        function formatCashOutAmount(input) {
            let value = input.value.replace(/\D/g, ''); 

            let formattedValue = (parseInt(value) / 100).toFixed(2); 

            input.value = formattedValue;
        }

        function validateCashOutAmount(input) {
            const maxAmount = parseFloat(input.getAttribute('max'));
            const minAmount = 10.00;
            const amount = parseFloat(input.value);

            if (amount < minAmount || amount > maxAmount) {
                document.getElementById("cashOutError").innerText = `Amount must be between RM${minAmount.toFixed(2)} and RM${maxAmount.toFixed(2)}`;
                document.getElementById("cashOutError").style.display = "block";
            } else {
                document.getElementById("cashOutError").style.display = "none";
            }
        }

        function formatCreditCardNumber(input) {
            // remove all non-digit characters
            let inputValue = input.value.replace(/\D/g, '');

            // limit to 16 digits
            inputValue = inputValue.substring(0, 16);

            // insert a space every 4 digits
            input.value = inputValue.replace(/(\d{4})(?=\d)/g, '$1 ');
        }

        // function to allow only numeric input
        function allowOnlyNumber(event) {
            // allow backspace, delete, arrow keys
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39) {
                return true;
            }

            // only allow numbers 
            if (event.key < '0' || event.key > '9') {
                event.preventDefault();
            }
        }

        function formatExpiryDate(input) {
            let value = input.value.replace(/\D/g, ''); // remove all non-digit characters

            if (value.length >= 2) {
                let month = value.substring(0, 2); // extract the month part

                // check if the month is valid (between 01 and 12)
                if (month < '01' || month > '12') {

                    document.getElementById("expiryError").style.display = "inline";
                    document.getElementById("expiryError").innerText = "Invalid month!";
                    input.value = "";
                    return;
                } else {

                    document.getElementById("expiryError").style.display = "none";
                }

                // add a slash after the valid month
                value = month + '/' + value.substring(2, 4);
            }

            input.value = value.substring(0, 5); // set to 5 characters (MM/YY)
        }

        function showError(message) {
            Swal.fire({
                text: message,
                icon: 'error',
                confirmButtonText: 'OK',

            });
        }

        function showSuccess(message) {
            Swal.fire({
                text: message,
                icon: 'success',
                confirmButtonText: 'OK',

            });
        }

        
    </script>
</asp:Content>




