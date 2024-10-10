<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AcceptDelivery.aspx.cs" Inherits="DonorConnect.AcceptDelivery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Delivery</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
    <style>
        .centered-row {
            display: flex;
            justify-content: center;
            padding-left: 50px;
        }

        .centered-container {
            display: flex;
            justify-content: center;
        }

        .centered-grid {
            width: 100%;
            max-width: 1200px; 
        }

        .order-info-panel {
          
            display: none;
        }

        #categoryDetailsTable {
            margin-top: 20px; 
            border: 1px solid black; 
            border-radius: 8px; 
            box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.2); 
            border-collapse: separate; 
            overflow: hidden; 
        }

            #categoryDetailsTable th, #categoryDetailsTable td {
                border: 1px solid black; 
                padding: 8px; 
            }

  
            #categoryDetailsTable th {
                background-color: #f8f9fa; 
                text-align: center;
            }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4" style="padding-top: 10px;">
        <!-- Recommended/All filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlRecommendedAll" runat="server" CssClass="form-control">
                <asp:ListItem Text="Recommended*" Value="Recommended"></asp:ListItem>
                <asp:ListItem Text="All" Value="All"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Pickup State filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlPickupState" runat="server" CssClass="form-control">
                       
                <asp:ListItem Text="From: (Any)" Value="" />
                <asp:ListItem Text="Johor" Value="Johor" />
                <asp:ListItem Text="Kedah" Value="Kedah" />
                <asp:ListItem Text="Kelantan" Value="Kelantan" />
                <asp:ListItem Text="Melaka" Value="Melaka" />
                <asp:ListItem Text="Negeri Sembilan" Value="Negeri Sembilan" />
                <asp:ListItem Text="Pahang" Value="Pahang" />
                <asp:ListItem Text="Penang" Value="Penang" />
                <asp:ListItem Text="Perak" Value="Perak" />
                <asp:ListItem Text="Perlis" Value="Perlis" />
                <asp:ListItem Text="Sabah" Value="Sabah" />
                <asp:ListItem Text="Sarawak" Value="Sarawak" />
                <asp:ListItem Text="Selangor" Value="Selangor" />
                <asp:ListItem Text="Terengganu" Value="Terengganu" />
              
            </asp:DropDownList>
        </div>

        <!-- Destination State filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlDestinationState" runat="server" CssClass="form-control">
                <asp:ListItem Text="To: (Any)" Value="" />
                <asp:ListItem Text="Johor" Value="Johor" />
                <asp:ListItem Text="Kedah" Value="Kedah" />
                <asp:ListItem Text="Kelantan" Value="Kelantan" />
                <asp:ListItem Text="Melaka" Value="Melaka" />
                <asp:ListItem Text="Negeri Sembilan" Value="Negeri Sembilan" />
                <asp:ListItem Text="Pahang" Value="Pahang" />
                <asp:ListItem Text="Penang" Value="Penang" />
                <asp:ListItem Text="Perak" Value="Perak" />
                <asp:ListItem Text="Perlis" Value="Perlis" />
                <asp:ListItem Text="Sabah" Value="Sabah" />
                <asp:ListItem Text="Sarawak" Value="Sarawak" />
                <asp:ListItem Text="Selangor" Value="Selangor" />
                <asp:ListItem Text="Terengganu" Value="Terengganu" />
               
            </asp:DropDownList>
        </div>

        <!-- Earnings filter (Highest to Lowest) -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlEarnings" runat="server" CssClass="form-control">
                <asp:ListItem Text="Earnings" Value=""></asp:ListItem>
                <asp:ListItem Text="Highest to Lowest" Value="DESC"></asp:ListItem>
                <asp:ListItem Text="Lowest to Highest" Value="ASC"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Vehicle Type filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control">
                <asp:ListItem Text="Vehicle Type" Value=""></asp:ListItem>
                <asp:ListItem Text="Any" Value="" />
                <asp:ListItem Text="Car (Any car models)" Value="Car"></asp:ListItem>
                <asp:ListItem Text="4x4 Pickup" Value="4x4 Pickup"></asp:ListItem>
                <asp:ListItem Text="Van 7 Feet" Value="Van 7 Feet"></asp:ListItem>
                <asp:ListItem Text="Van 9 Feet" Value="Van 9 Feet"></asp:ListItem>
                <asp:ListItem Text="Lorry 10 Feet" Value="Lorry 10 Feet"></asp:ListItem>
                <asp:ListItem Text="Lorry 14 Feet" Value="Lorry 14 Feet"></asp:ListItem>
                <asp:ListItem Text="Lorry 17 Feet" Value="Lorry 17 Feet"></asp:ListItem>
               
            </asp:DropDownList>
        </div>

       
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnFilter_Click" />
        </div>
    </div>
<div class="centered-container">
    <asp:GridView ID="gvAllDonations" runat="server" AutoGenerateColumns="False" CssClass="centered-grid" DataKeyNames="donationPublishId" GridLines="None" BorderStyle="None" CellPadding="0" OnRowCommand="gvAllDonations_RowCommand">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="row d-flex justify-content-center" data-donation-id='<%# Eval("deliveryId") %>'>
                        <div class="col-md-6">
                            <div class="card mb-4 shadow-sm card-custom" style="width: 100%!important;">
                                <div class="card-body <%# Eval("urgentStatus").ToString().ToLower() == "yes" ? "urgent-card" : "" %>" style="width: 100%!important;">
                                    <div class="row">
                                        <div class="col-md-8">
                                            <%# Eval("urgentLabel") %>  
                                        </div>
                                        <div class="col-md-4 text-right">
                                            <strong><i class="fas fa-car"></i></strong> <%# Eval("vehicleType") %>
                                        </div>
                                    </div>
                                    <div class="row mt-3 mb-3 text-center">
                                        <div class="col-md-12">
                                            <h3 class="card-title"><%# Eval("state") %> (<%# Eval("totalDistance") %> km)</h3>
                                            <h4 class="card-title"><%# Eval("pickupDate") %></h4>
                                            <h4 class="card-title"><%# Eval("pickupTime") %></h4>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-md-6">
                                            <strong><i class="fas fa-home"></i> From:</strong> <%# Eval("pickupAddress") %>
                                        </div>
                                        <div class="col-md-6">
                                            <strong><i class="fas fa-building"></i> To:</strong> <%# Eval("destinationAddress") %>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <strong><i class="fas fa-coins"></i> Earnings:</strong> <%# Eval("paymentAmount") %>
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:Button ID="btnViewOrder" runat="server" CssClass="btn btn-success" Text="View Order ->" CommandArgument='<%# Eval("deliveryId") %>' CommandName="ViewOrder" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                     
                        <div class="col-md-6">
                            <asp:Panel ID="pnlOrderInfo" runat="server" CssClass="order-info-panel" Style="display: none;" EnableViewState="true">
                                <div class="order-info">
                                    <h3>Order Details</h3>
                                    <p><strong>Donor Name:</strong>
                                        <asp:Label ID="lblDonorName" runat="server" /></p>
                                    <p><strong>Donor Phone:</strong>
                                        <asp:Label ID="lblDonorPhone" runat="server" /></p>
                                    <p><strong>Organization Name:</strong>
                                        <asp:Label ID="lblOrgName" runat="server" /></p>
                                    <p><strong>Organization Phone:</strong>
                                        <asp:Label ID="lblOrgPhone" runat="server" /></p>

                             
                                    <asp:PlaceHolder ID="phDonationItems" runat="server"></asp:PlaceHolder>
                                </div>
                            </asp:Panel>

                        </div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>




</asp:Content>

