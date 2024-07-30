<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword2.aspx.cs" Inherits="DonorConnect.ResetPassword2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet"/>
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

<script type="text/javascript">

    function showSuccess(message) {
        let timerInterval;
        Swal.fire({
            title: message,
            icon: 'info',
            html: 'Redirecting in <b></b> seconds.',
            timer: 5000,
            timerProgressBar: true,
            willOpen: () => {
                // Set up the countdown timer display
                const content = Swal.getHtmlContainer();
                const b = content.querySelector('b');
                timerInterval = setInterval(() => {
                    b.textContent = Math.ceil(Swal.getTimerLeft() / 1000);
                }, 100);
            },
            willClose: () => {
                // Clean up and redirect when the popup closes
                clearInterval(timerInterval);
                window.location.href = 'Login.aspx';
            }
        });
    }


    function ErrorMsg(message) {
        Swal.fire({
            icon: 'error',
            title: "The new password and confirm password fields do not match.",
            showCloseButton: true

        });
    }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" style="margin-top: 50px;">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-body">
                            <h3 class="card-title">Reset Password</h3>
                            <p class="card-text">Enter your new password below</p>
                            <div class="form-group">
                                <asp:Label ID="lblPassword" runat="server" CssClass="form-label">Password</asp:Label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" />
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lblConfirmPassword" runat="server" CssClass="form-label">Confirm Password</asp:Label>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" />
                            </div>
                           
                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" Text="Reset" OnClick="btnReset_Click" />
                            
                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>