<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgViewDonationRequest.aspx.cs" Inherits="DonorConnect.OrgManageDonationRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donation Requests</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

     <style>
           body{
             background: rgb(249,247,247);
             background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
           }

          .page-header {
    
            padding-bottom: 10px;
            border-bottom: 2px solid #ddd;
            margin-bottom: 20px;
          }

        .title{
            font-size: 1.8em;
            font-weight: bold;
            padding-bottom: 10px;
            color: #333;
            margin-bottom: 20px;
        }
     </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 class="title">Manage Donation Request</h1>
    </div>

    <asp:Repeater ID="rptDonations" runat="server" Visible="false">
        <ItemTemplate>
            <div class="ticket-card" style='<%# Convert.ToInt32(Eval("pendingRequestCount")) > 0 ? "border: 2px solid red;" : "border: 2px solid green;" %>; position: relative;'>
                <!-- Pending Request Count Badge -->
                <div class="pending-count-badge" style="position: absolute; top: 10px; right: 10px; background-color: #f39c12; color: white; padding: 5px 10px; border-radius: 12px;">
                    <%# Eval("pendingRequestCount") %> Pending
                </div>

                <!-- Urgent Status Section -->
                <div class="date">
                    <h1 style='<%# Eval("urgentStatus").ToString() == "Yes" ? "color: white;" : "color: lightgray;" %>'>
                        <%# Eval("urgentStatus").ToString() == "Yes" ? "<i class='fa fa-fire'></i>" : "<i class='fa fa-globe'></i>" %>
                    </h1>
                </div>

                <!-- Donation Information -->
                <div class="card-content">
                    <!-- Title -->
                    <h5 class="ticket-title"><%# Eval("title") %></h5>

                    <!-- Item Category -->
                    <div class="event-info">
                        <i class="fa fa-box"></i>
                        <%# Eval("itemCategory") %>
                    </div>

                    <!-- Donation Region -->
                    <div class="event-info">
                        <i class="fa fa-map-marker"></i>
                        <%# Eval("donationState") %>
                    </div>

                    <!-- View Button -->
                    <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="View Requests" CssClass="btn-tickets" OnClick="btnView_Click"/>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>

