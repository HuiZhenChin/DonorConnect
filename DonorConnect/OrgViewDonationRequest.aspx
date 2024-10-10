<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrgViewDonationRequest.aspx.cs" Inherits="DonorConnect.OrgManageDonationRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donation</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage Donation Request</h2>
    <asp:Repeater ID="rptDonations" runat="server" Visible="false">
        <ItemTemplate>
            <div class="ticket-card">
                <!-- Urgent Status Section with Conditional Color -->
                <div class="date">
                    <h1 style='<%# Eval("urgentStatus").ToString() == "Yes" ? "color: white;": "color: lightgray;" %>'>

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
                   
                    <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="View Requests" class="btn-tickets" OnClick="btnView_Click"/>

                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
