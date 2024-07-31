<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="DonorConnect.MyAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .section-header {
            cursor: pointer;
            background-color: #f8f9fa;
            padding: 10px;
            border: 1px solid #dee2e6;
            border-radius: 5px;
            margin-bottom: 10px;
        }
        .section-content {
            display: none;
            padding: 15px;
            border: 1px solid #dee2e6;
            border-radius: 5px;
            background-color: #fff;
            margin-bottom: 15px;
        }
        .form-group {
            margin-bottom: 1rem;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- User Information Section -->
        <div class="section-header" id="userInfoHeader">
            <h5>User Information</h5>
        </div>
        <div class="section-content" id="donorContent" runat="server">
            <asp:Panel runat="server">
                <div class="form-group">
                    <asp:Label runat="server" ID="lblUsername" Text="Username" />
                    <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblFullName" Text="Full Name" />
                    <asp:TextBox runat="server" ID="txtFullName" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblEmail" Text="Email Address" />
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblPhone" Text="Phone Number" />
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="form-control"/>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblAddress" Text="Delivery Address" />
                    <asp:TextBox runat="server" ID="txtAddress" CssClass="form-control"/>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnSaveUserInfo" CssClass="btn btn-primary" Text="Save" />
                    <asp:Button runat="server" ID="btnCancelUserInfo" CssClass="btn btn-secondary" Text="Cancel" CausesValidation="False" />
                </div>
            </asp:Panel>
        </div>

        <!-- Notifications Section -->
        <div class="section-header" id="notificationsHeader">
            <h5>Notifications</h5>
        </div>
        <div class="section-content" id="notificationsContent" runat="server">
            <asp:Panel runat="server">
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="chkEmailNotifications" CssClass="form-check-input" />
                    <asp:Label runat="server" AssociatedControlID="chkEmailNotifications" Text="Email Notifications" CssClass="form-check-label" />
                </div>
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="chkSmsNotifications" CssClass="form-check-input" />
                    <asp:Label runat="server" AssociatedControlID="chkSmsNotifications" Text="SMS Notifications" CssClass="form-check-label" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnSaveNotifications" CssClass="btn btn-primary" Text="Save"/>
                    <asp:Button runat="server" ID="btnCancelNotifications" CssClass="btn btn-secondary" Text="Cancel"  CausesValidation="False" />
                </div>
            </asp:Panel>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('#userInfoHeader').click(function () {
                $('#<%= donorContent.ClientID %>').toggle(); // Use ClientID here
            });

            $('#notificationsHeader').click(function () {
                $('#<%= notificationsContent.ClientID %>').toggle(); // Use ClientID here
            });
        });
    </script>


</asp:Content>

