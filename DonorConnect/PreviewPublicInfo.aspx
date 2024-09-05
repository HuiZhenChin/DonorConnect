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

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
    
        <!-- Profile Picture Section -->
        <div class="profile-pic" id="profilePic" runat="server" style="background-color: #f5f5f5; padding: 20px;">

            <asp:Image ID="output" runat="server" ImageUrl="/Image/default_picture.jpg" Width="165px" Height="165px" />
        </div>
        <!-- Display Username -->
        <div style="background-color: #f5f5f5; padding-bottom: 20px; text-align: center;">
            <asp:Label runat="server" ID="profileUsername" Font-Bold="true" Style="font-size: 22px;" />

        </div>

        <!-- User Information Section -->
        <div class="section-header" id="userInfoHeader" style="margin-top: 20px;">
            <h5>User Information</h5>
        </div>
        <asp:Panel runat="server">
            <div class="form-row">              
                    <strong>Username:</strong>
                    <asp:Label runat="server" ID="lblUsername" Text="Username" />
                
            </div>

            <div class="form-row" id="fullName" style="display: none;" runat="server">
               
                    <strong>Full Name:</strong>
                    <asp:Label runat="server" ID="lblFullName" Text="Full Name" />
                
            </div>

            <div class="form-row">
               
                    <strong>Email:</strong>
                    <asp:Label runat="server" ID="lblEmail" Text="Email" />
                
            </div>

            <div class="form-row">
                
                    <strong>Contact Number:</strong>
                    <asp:Label runat="server" ID="lblPhoneNo" Text="Contact Number" />
                
            </div>

            <div class="form-row" id="picName" style="display: none;" runat="server">
               
                    <strong>Person In-Charge's Name:</strong>
                    <asp:Label runat="server" ID="lblPicName" Text="Username" />
                
            </div>

            <div class="form-row" id="picEmail" style="display: none;" runat="server">
                
                    <strong>Person In-Charge's Email:</strong>
                    <asp:Label runat="server" ID="lblPicEmail" Text="Email" />
                
            </div>

            <div class="form-row" id="picNo" style="display: none;" runat="server">
               
                    <strong>Person In-Charge's Contact Number:</strong>
                    <asp:Label runat="server" ID="lblPicNo" Text="Contact Number" />
                
            </div>

            <div class="form-row" id="address" style="display: none;" runat="server">
              
                    <strong>Address:</strong>
                    <asp:Label runat="server" ID="lblAddress" Text="Address" />
                
            </div>

            <div class="form-row" id="desc" style="display: none;" runat="server">
              
                    <strong>Description:</strong>
                    <asp:Label runat="server" ID="lblDescription" Text="Description" />
                
            </div>

            <div class="form-row" id="region" style="display: none;" runat="server">
             
                    <strong>Region:</strong>
                    <asp:Label runat="server" ID="lblRegion" Text="Region" />
             
            </div>

            <div class="form-row" id="vehicleType" style="display: none;" runat="server">
              
                    <strong>Vehicle Type:</strong>
                    <asp:Label runat="server" ID="lblVehicleType" Text="Vehicle Type" />
               
            </div>

            <div class="form-row" id="plateNo" style="display: none;" runat="server">
               
                    <strong>Plate Number:</strong>
                    <asp:Label runat="server" ID="lblPlateNo" Text="Plate Number" />
                
            </div>
        </asp:Panel>

      
       <!-- Opened Donations Section -->
        <div class="section-header" id="donationHeader" style="margin-top: 20px;" Visible="false" runat="server">
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

                    <!-- Event Date -->
                    <%--<div class="event-date">
                        <i class="fa fa-calendar"></i>
                        <time><%# Eval("created_on", "{0:dddd, dd MMMM yyyy}") %></time>
                    </div>--%>

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
