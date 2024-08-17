<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminRegister.aspx.cs" Inherits="DonorConnect.AdminRegister" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrator Registration</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
  
    <script>

        function ErrorMsg(message) {
            Swal.fire({
                icon: 'error',
                title: message,

            });
        }

        function showMessage(message) {
            Swal.fire({
                title: 'Success!',
                text: message,
                icon: 'success',
                confirmButtonText: 'OK'
            });
        }

        function showSuccess(message) {
            Swal.fire({
                icon: 'success',
                title: message,
                showConfirmButton: true,
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = 'Login.aspx';
                }
            });
        }

   </script>
        
  <style>
        body {
            margin: 0;
            padding: 0;
            width: 100%;
            height: 100vh;
            background: url('/Image/main.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex;
            justify-content: center;
            align-items: center;
            overflow: visible;
        }
        .main-box {
            background: #f5f5f5;
            opacity: 0.9;
            border-radius: 12px;
            padding: 30px;
            max-width: 600px;
            width: 100%;
        }
        .title {
            font-weight: 800;
            color: #191970;
            text-align: center;
            font-family: sans-serif;
        }
        .inp {
            background-color: lightgray;
            color: black;
            font-size: 14px;
            border: 1px solid black;
        }
        .sub {
            background-color: black;
            color: #fff;
            border-radius: 20px;
            font-weight: 600;
            transition: background-color 0.3s ease;
        }
        .sub:hover {
            background-color: grey;
        }
       
           
        .otp-section {
            display: none;
            margin-top: 20px;
        }


    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-box">
            <h1 class="title">DONOR CONNECT</h1>
            <h2 class="title" style="font-size: 24px; font-weight: lighter;">ADMIN REGISTRATION</h2>

            <div class="login-form" runat="server">
                <div class="mb-3">
                    <label for="email" class="form-label">
                        <b>Your Personal Email</b>    
                    </label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter personal email" />
                    <asp:Label ID="lblEmail" runat="server" CssClass="text-danger" />
                    <small class="form-text text-muted">
                        <em> Note: Emergency donation requests or events will be only sent to your personal email, otherwise will be in DonorConnect official email.</em>
                    </small>
                </div>

                <asp:Button type="button" class="btn sub w-100" id="signupBtn" OnClick="btnRegister_Click" runat="server" Text="Sign Up"></asp:Button>

                <div class="otp-section" id="otpSection" runat="server" style="display:none;">
                    <div class="mb-3">
                        <label for="otp" class="form-label"><b>OTP Verification</b></label>
                        <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control" placeholder="Enter OTP" />
                        <asp:Label ID="lblOtp" runat="server" CssClass="text-danger" />
                    </div>
                    <asp:Button type="button" class="btn sub w-100" id="verifyOtpBtn" OnClick="btnVerifyOTP_Click" runat="server" Text="Verify OTP"></asp:Button>
                </div>
            </div>
        </div>
    </form>
 

</body>
</html>

