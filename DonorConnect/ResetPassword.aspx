<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="DonorConnect.ResetPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet"/>
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<script type="text/javascript">
        function ErrorMsg(message) {
            Swal.fire({
                icon: 'error',
                title: message,
                
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" style="margin-top: 50px;">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-body">
                            <h3 class="card-title">Reset Password</h3>
                            <p class="card-text">Enter your username to search for your account.</p>
                            <div class="form-group">
                                <asp:Label ID="lblUsername" runat="server" CssClass="form-label">Username</asp:Label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
                            </div>
                           
                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearch_Click" />
                            <div id="confirmationSection" runat="server" visible="false">
                                <p class="card-text">An account with the email address: <asp:Label ID="lblEmail" runat="server" CssClass="font-weight-bold"></asp:Label> and role as <asp:Label ID="lblRole" runat="server" CssClass="font-weight-bold"></asp:Label> was found. Is this you?</p>
                                <asp:Button ID="btnConfirm" runat="server" CssClass="btn btn-success" Text="Yes, send reset link" OnClick="btnConfirm_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="No, cancel" OnClick="btnCancel_Click" />
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

