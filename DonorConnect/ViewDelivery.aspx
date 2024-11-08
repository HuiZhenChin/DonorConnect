<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDelivery.aspx.cs" Inherits="DonorConnect.UpdateDelivery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>Delivery</title>
     <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.3/umd/popper.min.js"></script>
     <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

     <style>
         body{
              background: rgb(231,246,242);
              background: linear-gradient(180deg, rgba(231,246,242,1) 18%, rgba(165,201,202,1) 60%, rgba(57,91,100,1) 86%, rgba(44,51,51,1) 100%);
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
    <div class="container mt-3">
   
    <ul class="nav nav-tabs">        
        <li class="nav-item">
            <asp:LinkButton ID="lnkToPickUp" runat="server" CssClass="nav-link" OnClick="LoadDelivery_Click" CommandArgument="Accepted" style="color: #1E201E;">To PickUp</asp:LinkButton>
        </li>
        <li class="nav-item">
            <asp:LinkButton ID="lnkToReach" runat="server" CssClass="nav-link" OnClick="LoadDelivery_Click" CommandArgument="Delivering in Progress" style="color: #1E201E;">To Reach</asp:LinkButton>
        </li>
        <li class="nav-item">
            <asp:LinkButton ID="lnkCompleted" runat="server" CssClass="nav-link" OnClick="LoadDelivery_Click" CommandArgument="Reached Destination" style="color: #1E201E;">Completed</asp:LinkButton>
        </li>
    </ul>

    </div>

    <asp:Label ID="noDataLabel" runat="server" Text="No delivery made yet" Visible="false" CssClass="noData"></asp:Label>

     <div class="centered-container">
     <asp:GridView ID="gvAllDonations" runat="server" AutoGenerateColumns="False" CssClass="centered-grid" DataKeyNames="deliveryId" GridLines="None" BorderStyle="None" CellPadding="0">
         <Columns>
             <asp:TemplateField>
                 <ItemTemplate>
                     <div class="row d-flex justify-content-center align-items-stretch" data-donation-id='<%# Eval("deliveryId") %>'>
                         <div class="col-md-6 d-flex align-items-stretch">
                             <div class="card mb-4 shadow-sm card-custom" style="width: 100%!important;">
                                 <div class="card-body <%# Eval("urgentStatus").ToString().ToLower() == "yes" ? "urgent-card" : "" %>" style="width: 100%!important; background-color: #F3FBF1;">
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
                                             <asp:Button ID="btnUpdateDelivery" runat="server" CssClass="btn btn-success" Text="View Delivery" OnClick="btnViewDelivery_Click" CommandArgument='<%# Eval("deliveryId") %>' CommandName="ViewDelivery" />
                                         </div>
                                     </div>
                                 </div>
                             </div>
                         </div>
                      </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
