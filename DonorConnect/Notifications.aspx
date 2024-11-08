<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="DonorConnect.Notifications" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>New Donation Request</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
    <style>
        body{
            background: rgb(243,238,234);
            background: linear-gradient(180deg, rgba(243,238,234,1) 17%, rgba(235,227,213,1) 55%, rgba(176,166,149,1) 80%, rgba(119,107,93,1) 100%);
        }

        .notification-link.unread {
            font-weight: bold;
            color: #543310; 
        }

        .notification-link.read {
            font-weight: normal;
            color: #61677A; 
        }

        .history-box {
            background-color: rgba(255, 255, 255, 0.7);
            border-radius: 8px;
            box-shadow: 0 4px 3px rgba(0, 0, 0, 0.1);
            margin-bottom: 15px;
            padding: 15px;
            position: relative;
            transition: transform 0.3s ease;
        }

        .history-box:hover {
            transform: scale(1.02); 
        }

        .history-box-header {
            display: flex;
            justify-content: flex-end;
            margin-bottom: 10px;
        }

        .created-on {
            font-size: 12px;
            color: #888;
        }

        .history-box-content p {
            font-size: 16px;
            margin: 0;
        }

        .notification-grid {
            border: none;
            width: 100%;

        }

        .notification-grid th,
        .notification-grid td {
            border: none; 
            padding: 0;
            margin: 0;
        }

        .notification-grid thead {
            display: none;
        }

        .noData {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 200px; 
            font-size: 18px;
            color: #555;
            text-align: center;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="w-100 text-center mb-4" style="font-weight: bold; font-size: 32px; padding: 10px;">Notifications</h2>
    
    <asp:Label ID="lblNoNotifications" runat="server" CssClass="noData" Visible="false"></asp:Label>

    <!-- Notification Grid -->
    <asp:GridView ID="gvNotifications" runat="server" AutoGenerateColumns="False"
    OnRowCommand="gvNotifications_RowCommand" CssClass="notification-grid">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <div class="history-box">
                    <div class="history-box-header">
                        <span class="created-on"><%# Convert.ToDateTime(Eval("created_on")).ToString("yyyy-MM-dd") %></span>
                    </div>
                    <div class="history-box-content">
                        <p>
                            <asp:LinkButton
                                runat="server"
                                Text='<%# Eval("Content") %>'
                                CommandName="Redirect"
                                CommandArgument='<%# Eval("Link") %>'
                                CssClass='<%# Convert.ToBoolean(Eval("isRead")) ? "notification-link read" : "notification-link unread" %>'>
                            </asp:LinkButton>
                        </p>
                        <asp:Label ID="lblNotificationId" runat="server" Text='<%# Eval("notificationId") %>' Visible="false"></asp:Label>
                    </div>
                </div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>



</asp:Content>
