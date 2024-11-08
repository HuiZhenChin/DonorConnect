<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PreviewPublicInfo.aspx.cs" Inherits="DonorConnect.PreviewPublicInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Preview Public Profile</title>
    <link href="/Content/MyAccount.css" rel="stylesheet" type="text/css" />
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        body{
            background: rgb(231,246,242);
            background: linear-gradient(180deg, rgba(231,246,242,1) 0%, rgba(165,201,202,1) 59%, rgba(57,91,100,1) 89%, rgba(44,51,51,1) 100%);
        }

        .badgeNew {
            border-radius: 5px;
            font-weight: bold;
            color: #fff;
            font-size: 100%!important;
            line-height: 1.5!important;
            padding: 5px;
        }

        .badge-active {
            background-color: #28a745; 
        }

        .badge-terminated {
            background-color: #dc3545;
        }


    </style>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
    
        <!-- Profile Picture Section -->
        <div style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.5);">
        <div class="profile-pic" id="profilePic" runat="server" style="background-color: #EEEEEE; padding: 20px;">

            <asp:Image ID="output" runat="server" ImageUrl="/Image/default_picture.jpg" Width="165px" Height="165px" style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.5);"/>
        </div>
        <!-- Display Username -->
        <div style="background-color: #EEEEEE; padding-bottom: 20px; text-align: center;">
            <asp:Label runat="server" ID="profileUsername" Font-Bold="true" Style="font-size: 22px;" />
        </div>
        </div>

        <!-- User Information Section -->
        <div class="section-header" id="userInfoHeader" style="margin-top: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.5); background-color: #D8D9DA;">
            <h5>User Information</h5>
        </div>
        <div class="section-content" style="display: block!important;">
        <asp:Panel runat="server">
            <div class="form-row" style="justify-content: flex-end;">
                <asp:Label runat="server" ID="lblStatus" Text="Status" />

            </div>
     
            <span style="padding-bottom: 20px; font-size: 18px; font-weight: 500; text-decoration: underline;">Basic Information</span>
            <div class="form-row" id="fullName" style="display: none;" runat="server">
               
                    <i class="fa fa-address-card" aria-hidden="true" style="padding-right: 10px; font-size: 18px; margin-left: 2px;"></i>
                    <asp:Label runat="server" ID="lblFullName" Text="Full Name" style="font-size: 16px;"/>
                
            </div>

            <div class="form-row">
               
                    <strong style="padding-right: 10px; font-size: 18px;">✉️ </strong>
                    <asp:Label runat="server" ID="lblEmail" Text="Email" style="font-size: 16px;"/>
                
            </div>

            <div class="form-row">
                
                    <strong style="padding-right: 10px; font-size: 18px;">📞 </strong>
                    <asp:Label runat="server" ID="lblPhoneNo" Text="Contact Number" style="font-size: 16px;"/>
                
            </div>

            <div class="form-row" id="address" style="display: none;" runat="server">

                <strong style="padding-right: 10px; font-size: 18px;">🏡</strong>
                <asp:Label runat="server" ID="lblAddress" Text="Address" Style="font-size: 16px;" />

            </div>

            <div class="form-row" id="region" style="display: none;" runat="server">

                <strong style="padding-right: 15px; font-size: 18px;">📍</strong>
                <asp:Label runat="server" ID="lblRegion" Text="Region" Style="font-size: 16px;" />

            </div>

            <div class="form-row" id="vehicleType" style="display: none;" runat="server">
                <strong style="padding-right: 10px; font-size: 18px;">🚘</strong>
                <asp:Label runat="server" ID="lblVehicleType" Text="Vehicle Type" Style="font-size: 16px;" />
             
            </div>


            <span id="pic" style="padding-bottom: 20px; font-size: 18px; font-weight: 500; text-decoration: underline; display: none;" runat="server">Person In-Charge Information</span>
            <div class="form-row" id="picName" style="display: none;" runat="server">
               
                    <strong>Name:</strong>
                    <asp:Label runat="server" ID="lblPicName" Text="Username" style="font-size: 16px;"/>
                
            </div>

            <div class="form-row" id="picEmail" style="display: none;" runat="server">
                
                    <strong>Email:</strong>
                    <asp:Label runat="server" ID="lblPicEmail" Text="Email" style="font-size: 16px;"/>
                
            </div>

            <div class="form-row" id="picNo" style="display: none;" runat="server">
               
                    <strong>Contact Number:</strong>
                    <asp:Label runat="server" ID="lblPicNo" Text="Contact Number" style="font-size: 16px;"/>
                
            </div>

            <span id="other" style="padding-bottom: 20px; font-size: 18px; font-weight: 500; text-decoration: underline; display: none;" runat="server">Other Information</span>
            <div class="form-row" id="desc" style="display: none;" runat="server">

                <strong>Description:</strong>
                <asp:Label runat="server" ID="lblDescription" Text="Description" Style="font-size: 16px;" />

            </div>

  
        </asp:Panel>
       </div>
      
       <!-- Opened Donations Section -->
        <div class="section-header" id="donationHeader" style="margin-top: 20px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.5); background-color: #D8D9DA;" Visible="false" runat="server">
            <h5>Opened Donations</h5>
        </div>
         <asp:Repeater ID="rptDonations" runat="server" Visible="false">
        <ItemTemplate>
            <div class="ticket-card">
                <!-- Urgent Status Section with Conditional Color -->
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
                  <a href='AllDonations.aspx?donationPublishId=<%# Eval("donationPublishId") %>' class="btn-tickets">View</a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    </div>

</asp:Content>
