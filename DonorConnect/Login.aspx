<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DonorConnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="/Content/Login.css" rel="stylesheet" type="text/css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        function showMessage(title) {
            const Toast = Swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.onmouseenter = Swal.stopTimer;
                    toast.onmouseleave = Swal.resumeTimer;
                }
            });
            Toast.fire({
                icon: "success",
                title: title
            });
        }

        function ErrorMsg(message, icon) {
            Swal.fire({
                icon: icon,
                title: message,
                timer: 5000,
            });
        }
    </script>
</head>
<body style="background-color: #bfdae2">
    <form id="form1" runat="server">
        <div class="container">
            <div class="card" style="border-radius: 1rem;">
                <div class="row no-gutters flex-column flex-md-row">
                    <div class="col-12 col-md-6 image-container">
                        <img src="/Image/login_pic.jpg" 
                             alt="login form" class="img-fluid image-side" style="height: 100%"/>
                    </div>
                    <div class="col-12 col-md-6 d-flex align-items-center">
                        <div class="card-body p-4 p-lg-5 text-black">
                            <div class="d-flex align-items-center mb-3 pb-1"> 
                                <span class="h1 fw-bold mb-0">DonorConnect</span>
                            </div>
                            <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Login to your account</h5>
                            <div class="form-outline mb-4">
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control form-control-lg"></asp:TextBox>
                                <asp:Label ID="lblUsername" runat="server" CssClass="form-label" AssociatedControlID="txtUsername">Username</asp:Label>
                            </div>
                            <div class="form-outline mb-4">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control form-control-lg" TextMode="Password"></asp:TextBox>
                                <asp:Label ID="lblPassword" runat="server" CssClass="form-label" AssociatedControlID="txtPassword">Password</asp:Label>
                            </div>
                            <div class="pt-1 mb-4">
                                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-dark btn-lg btn-block" Text="Login" onClick="btnLogin_Click"/>
                            </div>
                            <a class="small text-muted" href="#!">Forgot password?</a>
                            <p class="mb-5 pb-lg-2" style="color: #393f81; margin-top: 15px;">Doesn't have an account? <a href="SignUp.aspx" style="color: #393f81;">Register here</a></p>
                            <a href="#!" class="small text-muted">Terms of Use.</a>
                            <a href="#!" class="small text-muted">Privacy Policy</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://kit.fontawesome.com/a076d05399.js"></script>
</body>
</html>
