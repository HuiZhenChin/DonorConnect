<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="DonorConnect.Notifications" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>New Donation Request</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Notifications</h2>
    
    <!-- Notification Grid -->
    <asp:GridView ID="gvNotifications" runat="server" AutoGenerateColumns="False" 
        CssClass="notification-grid" OnRowCommand="gvNotifications_RowCommand">
        <Columns>
          
            <asp:TemplateField HeaderText="Notification">
                <ItemTemplate>
                     <asp:LinkButton 
                    runat="server" 
                    Text='<%# Eval("Content") %>' 
                    CommandName="Redirect" 
                    CommandArgument='<%# Eval("Link") %>' 
                    CssClass="notification-link">
                </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>

            
            <asp:BoundField DataField="created_on" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
        </Columns>
    </asp:GridView>
</asp:Content>
