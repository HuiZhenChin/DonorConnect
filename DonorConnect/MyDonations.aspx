<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyDonations.aspx.cs" Inherits="DonorConnect.MyDonations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donations</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    
    
    <style>
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
    <div class="container mt-3">
        <!-- Tabs for different statuses -->
        <ul class="nav nav-tabs">
            <li class="nav-item">
                <asp:LinkButton ID="lnkAll" runat="server" CssClass="nav-link active" OnClick="LoadDonations_Click" CommandArgument="All">All</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkPending" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="Pending">Pending</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToPay" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To Pay">To Pay</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToAccept" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To Accept">To Accept</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToPickUp" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To PickUp">To PickUp</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkToReach" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="To Reach">To Reach</asp:LinkButton>
            </li>
            <li class="nav-item">
                <asp:LinkButton ID="lnkCompleted" runat="server" CssClass="nav-link" OnClick="LoadDonations_Click" CommandArgument="Completed">Completed</asp:LinkButton>
            </li>
        </ul>

        </div>

       <%-- <div class="mt-3">
            <h3>QR Code Scanner</h3>
            <video id="video" autoplay style="width:100%; max-width:400px;"></video>
            <canvas id="canvas" style="display:none;"></canvas>
            <p id="outputMessage">Scanning for QR code...</p>
            <p id="outputData"></p>
        </div>
    </div>--%>


        <!-- GridView for displaying donations -->
        <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Donation Details">
                    <ItemTemplate>

                        <div class="row-container">

                            <!-- Status Box -->
                            <div class="status-box <%# GetStatus(Eval("status").ToString()) %>">
                                <%# Eval("status") %>
                            </div>

                            <!-- Donor Information -->
                            <strong>Donation ID:</strong> <%# Eval("donationId") %><br />
                            <strong>Pick Up Address:</strong> <%# Eval("pickUpAddress") %><br />
                            <strong>Published By:</strong> <%# Eval("orgName") %><br />
                            <strong>Destination Address:</strong> <%# Eval("destinationAddress") %><br />

                            <!-- Pickup Date and Time are displayed only when status is not Pending or To Pay -->

                            <asp:Panel ID="pnlPickUpDate" runat="server" Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" %>'>
                                <strong>Pick Up Date:</strong>
                                <asp:Label ID="lblPickUpDate" runat="server"
                                    Text='<%# Eval("pickupDate", "{0:MM/dd/yyyy}") %>'>
                                </asp:Label><br />
                            </asp:Panel>

                            <!-- Pick Up Time Section -->
                            <asp:Panel ID="pnlPickUpTime" runat="server" Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" %>'>
                                <strong>Pick Up Time:</strong>
                                <asp:Label ID="lblPickUpTime" runat="server"
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
                                    Text="Make Payment" CssClass="btn btn-success btn-sm"
                                    Visible='<%# Eval("status").ToString() == "To Pay" %>' OnClick="btnPay_Click" />

                                <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("donationId") %>'
                                    Text="View Delivery" CssClass="btn btn-info btn-sm" OnClick="btnViewDelivery_Click"
                                    Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" && Eval("status").ToString() != "Rejected" %>' />

                                <asp:Button ID="btnQRCode" runat="server" CommandArgument='<%# Eval("donationId") %>'
                                    Text="QR Code" CssClass="btn btn-info btn-sm"
                                    Visible='<%# Eval("status").ToString() != "Pending" && Eval("status").ToString() != "To Pay" && Eval("status").ToString() != "Rejected"%>'
                                    OnClientClick='<%# "showQRCodeModal(\"" + Eval("donationId") + "\"); return false;" %>' />


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



        // Reference the video, canvas, and output elements
        //const video = document.getElementById('video');
        //const canvas = document.getElementById('canvas');
        //const outputMessage = document.getElementById('outputMessage');
        //const outputData = document.getElementById('outputData');
        //const context = canvas.getContext('2d');

        //// Function to start video feed from the camera
        //function startVideo() {
        //    navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } })
        //        .then(stream => {
        //            video.srcObject = stream;
        //            video.setAttribute("playsinline", true); // to prevent fullscreen in iOS
        //            video.play();
        //            requestAnimationFrame(scanQRCode);
        //        })
        //        .catch(err => {
        //            console.error("Error accessing the camera: ", err);
        //            outputMessage.innerText = "Unable to access camera.";
        //        });
        //}

        //// Function to capture a frame and scan for QR code
        //function scanQRCode() {
        //    if (video.readyState === video.HAVE_ENOUGH_DATA) {
        //        canvas.width = video.videoWidth;
        //        canvas.height = video.videoHeight;
        //        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        //        // Capture the image from the video feed
        //        const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
        //        const code = jsQR(imageData.data, imageData.width, imageData.height, { inversionAttempts: "dontInvert" });

        //        // If QR code is found, display the data and redirect
        //        if (code) {
        //            outputMessage.hidden = true;
        //            outputData.innerText = "QR Code Data: " + code.data;

        //            // If the QR code contains a URL, redirect to it
        //            if (isValidUrl(code.data)) {
        //                window.location.href = code.data; // Redirect to the scanned URL
        //            } else {
        //                console.log("QR Code found, but not a valid URL.");
        //            }
        //        } else {
        //            outputMessage.hidden = false;
        //            outputMessage.innerText = "Scanning for QR code...";
        //            outputData.innerText = "";
        //        }
        //    }
        //    // Continue scanning for the QR code
        //    requestAnimationFrame(scanQRCode);
        //}

        //// Helper function to validate if the scanned data is a valid URL
        //function isValidUrl(string) {
        //    try {
        //        new URL(string);
        //        return true;
        //    } catch (_) {
        //        return false;
        //    }
        //}

        // Start the video when the page loads
       // window.onload = startVideo;

        

    </script>

   
</asp:Content>
