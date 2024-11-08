<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyDonations.aspx.cs" Inherits="DonorConnect.MyDonations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donations</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    
    
    <style>
    body{
        background: rgb(248,236,209);
        background: linear-gradient(180deg, rgba(248,236,209,1) 18%, rgba(222,182,171,1) 56%, rgba(172,125,136,1) 79%, rgba(133,88,111,1) 100%);
    }

    .body-content::before {
        content: "";
        position: fixed; 
        bottom: 0; 
        left: 0;
        width: 100%;
        height: 100%;
        background: url(/Image/cloud2.png) no-repeat bottom left;
        background-size: 100% auto;
        opacity: 0.6;
        z-index: -1;
    }

    .row-container {
        position: relative;
        padding: 15px;
        border-bottom: 1px solid #ddd;
    }

   
    .status-box {
        position: absolute;
        top: 10px;
        right: 10px;
        padding: 5px 10px;
        font-weight: bold;
        font-size: 14px;
        border-radius: 5px;
        border: 1px solid;
    }


    .status-pending {
        color: #000;
        border-color: #fdfd96; 
        background-color: #fdfd96;
    }

    .status-to-pay {
        color: #000;
        border-color: #fdd835; 
        background-color: #fdd835;
    }

    .status-to-accept {
        color: #000;
        border-color: #ffcc80; 
        background-color: #ffcc80;
    }

    .status-to-pickup {
        color: #000;
        border-color: #ffa726; 
        background-color: #ffa726;
    }

    .status-to-reach {
        color: #000;
        border-color: #aed581; 
        background-color: #aed581;
    }

    .status-completed {
        color: #fff;
        border-color: #388e3c; 
        background-color: #388e3c;
    }

    .status-rejected {
        color: #fff;
        border-color: #e53935; 
        background-color: #e53935;
    }

    .status-refund {
        color: #fff;
        border-color: #e53935; 
        background-color: orange;
    }


    #categoryDetailsTable {
        margin-top: 20px; 
        border: 1px solid black; 
        border-radius: 8px; 
        box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.2); 
        border-collapse: separate; 
        overflow: hidden; 
        }

    #categoryDetailsTable thead tr th{
        color: black;
    }

    #categoryDetailsTable th, #categoryDetailsTable td {
        border: 1px solid black; 
        padding: 8px; 
    }

    
    #categoryDetailsTable th {
        background-color: #f8f9fa; 
        text-align: center;
    }

    .donation-details p {
        margin-bottom: 10px; 
    }

    .table {
        background-color: rgba(255, 255, 255, 0.7); 
        border-collapse: collapse;
        width: 100%;
    }

    .table tbody tr th {
        background-color: #838383;
        color: #ffffff;
        padding: 10px;
        font-weight: bold;
        text-align: left;
    }

    .table tbody tr {
        border-bottom: 1px solid #e0e0e0;
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
        <!-- Tabs for different statuses -->
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <asp:LinkButton ID="lnkAll" runat="server" CssClass="nav-link active" OnClick="LoadDonations_Click" CommandArgument="All" style="color: #493628;">All</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkPending" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="Pending" style="color: #493628;">Pending</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToPay" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To Pay" style="color: #493628;">To Pay</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToAccept" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To Accept" style="color: #493628;">To Accept</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToPickUp" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To PickUp" style="color: #493628;">To PickUp</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToReach" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To Reach" style="color: #493628;">To Reach</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkCompleted" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="Completed" style="color: #493628;">Completed</asp:LinkButton>
            </li>
            
        </ul>

        </div>

    <asp:Label ID="noDataLabel" runat="server" Text="No delivery made yet" Visible="false" CssClass="noData"></asp:Label>

        <!-- GridView for displaying donations -->
        <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound" style="margin-top: 20px;">
            <Columns>
                <asp:TemplateField HeaderText="Donation Details">
                    <ItemTemplate>

                        <div class="row-container">

                            <!-- Status Box -->
                            <div class="status-box <%# GetStatus(Eval("status").ToString()) %>">
                                <%# Eval("status") %>
                            </div>

                            <!-- Donor Information -->
                            <div class="donation-details">
                                <p><strong>Donation ID:</strong> <%# Eval("donationId") %></p>
                                <p><strong>Pick Up Address:</strong> <%# Eval("pickUpAddress") %></p>
                                <p><strong>Published By:</strong> <%# Eval("orgName") %></p>
                                <p><strong>Destination Address:</strong> <%# Eval("destinationAddress") %></p>
                            </div>

                            <!-- Pickup Date and Time are displayed only when status is not Pending or To Pay -->

                            <asp:Panel ID="pnlPickUpDate" runat="server" Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" %>'>
                                <strong>Pick Up Date:</strong>
                                <asp:Label ID="lblPickUpDate" runat="server"
                                    Text='<%# Eval("pickupDate", "{0:MM/dd/yyyy}") %>'>
                                </asp:Label><br />
                            </asp:Panel>

                            <!-- Pick Up Time Section -->
                            <asp:Panel ID="pnlPickUpTime" runat="server" Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" %>'>
                                <strong style="bottom: -10px; position: relative;">Pick Up Time:</strong>
                                <asp:Label ID="lblPickUpTime" runat="server" style="bottom: -10px; position: relative;"
                                    Text='<%# Eval("pickupTime", "{0:hh:mm tt}") %>'>
                                </asp:Label><br />
                            </asp:Panel>
                            <br />


                            <div class="col-md-12 mb-3">
                                <asp:PlaceHolder ID="phDonationItems" runat="server"></asp:PlaceHolder>
                            </div>

                            <asp:Panel ID="pnlQRCode" runat="server" Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" %>'>
                            </asp:Panel>


                            <!-- View and QR Code buttons are hidden when status is Pending or To Pay -->
                            <div class="text-right mt-2">
                                <asp:Button ID="btnMakePayment" runat="server" CommandArgument='<%# Eval("donationId") %>'
                                    Text="Make Payment" CssClass="btn btn-success btn-" style="background-color: #ED8D8D; border: #ED8D8D; padding: 8px 16px; font-size: 1em; "
                                    Visible='<%# Eval("status").ToString() == "To Pay" %>' OnClick="btnPay_Click" />

                                <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("donationId") %>'
                                    Text="View Delivery" CssClass="btn btn-info btn-sm" OnClick="btnViewDelivery_Click" style="background-color: #8D6262; border: #8D6262; padding: 8px 16px; font-size: 1em;"
                                    Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" && Eval("status").ToString() != "Rejected"  && Eval("status").ToString() != "Refund" && Eval("status").ToString() != "Approved" && Eval("status").ToString() != "Cancelled" %>' />

                                <asp:Button ID="btnQRCode" runat="server" CommandArgument='<%# Eval("donationId") %>'
                                    Text="QR Code" CssClass="btn btn-info btn-sm" style="background-color: #393232; border: #393232; padding: 8px 16px; font-size: 1em;"
                                    Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" && Eval("status").ToString() != "Approved" && Eval("status").ToString() != "Refund" && Eval("status").ToString() != "To Reach" && Eval("status").ToString() != "Completed" && Eval("status").ToString() != "Cancelled" && Eval("status").ToString() != "To Accept" && Eval("status").ToString() != "Rejected"%>'
                                    OnClientClick='<%# "showQRCodeModal(\"" + Eval("donationId") + "\"); return false;" %>' />

                                
                            </div>

                            <div class="text-left mt-2" style="position: relative;">
                                <asp:Button ID="btnRefund" runat="server" CommandArgument='<%# Eval("donationId") %>'
                                    Text="Refund?"
                                    style="background: none; border: none; text-decoration: underline;"
                                    OnClientClick='<%# "showRefundModal(\"" + Eval("donationId") + "\"); return false;" %>'
                                    Visible='<%# Eval("status").ToString() == "To Accept" %>' />
                            </div>



                        </div>
                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    <!-- Modal for displaying QR Code -->
    <div class="modal fade" id="qrCodeModal" tabindex="-1" role="dialog" aria-labelledby="qrCodeModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="qrCodeModalLabel">QR Code</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body text-center">
                    <!-- QR Code will be displayed here -->
                    <img id="qrCodeImage" alt="QR Code" width="200" height="200" />
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="refundModal" tabindex="-1" role="dialog" aria-labelledby="refundModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="refundModalLabel">Request Refund</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <p>Are you sure you want to request a refund for this donation? You can only request a refund before your pickup is accepted by the rider. </p>

                <!-- Hidden field for donationId -->
                <asp:HiddenField ID="hfDonationId" runat="server" />

                <!-- Dropdown for selecting refund reason -->
                <div class="form-group">
                    <label for="ddlRefundReason">Reason for Refund:</label>
                    <asp:DropDownList ID="ddlRefundReason" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- Select Reason --" Value="" />                      
                        <asp:ListItem Text="Change of Delivery Address" Value="Change of Delivery Address" />
                        <asp:ListItem Text="No Longer Need the Item" Value="No Longer Need the Item" />
                        <asp:ListItem Text="Donor Unable to Provide Item" Value="Donor Unable to Provide Item" />
                        <asp:ListItem Text="Cancel the Donation" Value="Cancel the Donation" />
                        <asp:ListItem Text="Other" Value="Other" />
                    </asp:DropDownList>
                </div>
                <asp:Label ID="lblError" runat="server" Text="">
                </asp:Label><br />
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                <asp:Button ID="btnConfirmRefund" runat="server" CssClass="btn btn-danger" Text="Confirm Refund" OnClick="btnRefund_Click" />
            </div>
        </div>
    </div>
</div>



    <script src="https://cdn.jsdelivr.net/npm/jsqr/dist/jsQR.js"></script>
    <script>
        function showQRCodeModal(donationId) {
           
            $.ajax({
                type: "POST",
                url: "MyDonations.aspx/GetQRCodeImage", 
                data: JSON.stringify({ donationId: donationId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    var qrCodeUrl = response.d;  

                    if (qrCodeUrl) {
                        // set the QR code image in the dialog
                        document.getElementById('qrCodeImage').src = qrCodeUrl;

                        $('#qrCodeModal').modal('show');
                    } else {
                        console.error("No QR code found for this donation.");
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching QR code: " + error);
                }
            });

            return false; 
        }

        function showRefundModal(donationId) {
            
            $('#<%= hfDonationId.ClientID %>').val(donationId);
           
            $('#refundModal').modal('show');
        }

        function showError(message) {
            Swal.fire({
                text: message,
                icon: 'error',
                confirmButtonText: 'OK',

            });
        }

        function showSuccess(message) {
            Swal.fire({
                text: message,
                icon: 'success',
                confirmButtonText: 'OK',

            });
        }

    </script>

   
</asp:Content>
