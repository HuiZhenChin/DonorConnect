<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUpOTP.aspx.cs" Inherits="DonorConnect.SignUpOTP" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verify OTP</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" style="margin-top: 50px;">
            <div class="row justify-content-center">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-body">
                            <h3 class="card-title">Verify OTP</h3>
                            <p class="card-text">An OTP has been sent to your email address: <asp:Label ID="lblEmail" runat="server" CssClass="font-weight-bold"></asp:Label>. Please enter it below to verify your account.</p>
                            <div class="form-group">
                                <asp:Label ID="lblOTP" runat="server" CssClass="form-label">OTP</asp:Label>
                                <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" />
                               
                            </div>
                            <asp:Button ID="btnVerifyOTP" runat="server" CssClass="btn btn-primary" Text="Verify OTP" OnClick="btnVerifyOTP_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
 

</body>
</html>


