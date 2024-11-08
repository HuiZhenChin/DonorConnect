<%@ Page Title="Delivery Tracking" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Delivery.aspx.cs" Inherits="DonorConnect.Delivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Delivery Tracking</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBHd69lOb31ywFRMu99sos-ysgl-uCtidY&callback=initMap&libraries=places"></script>

    
    <style>

    body{
        background: rgb(231,246,242);
        background: linear-gradient(180deg, rgba(231,246,242,1) 18%, rgba(165,201,202,1) 60%, rgba(57,91,100,1) 86%, rgba(44,51,51,1) 100%);
    }

    :root {
        --default-color: black;
        --grey-color: black;
        --main-color: #1B262C;
    }

    #progress_bar {
        display: table;
        width: 100%;
        padding: 15px 15px 0;
        table-layout: fixed;
        counter-reset: step;
        margin-top: 20px; 
        margin-bottom: 20px;
    }

    #progress_bar li {
        list-style-type: none;
        display: table-cell;
        width: 2%;
        font-size: 16px;
        position: relative;
        text-align: center;
    }

    #progress_bar li:before {
        width: 50px;
        height: 50px;
        color: lightgrey;
        content: counter(step);
        counter-increment: step;
        line-height: 50px;
        font-size: 18px;
        border: 1px solid var(--grey-color);
        display: block;
        text-align: center;
        margin: 0 auto 10px auto;
        border-radius: 50%;
        background-color: #fff;
    }

    #progress_bar li:after {
        width: 100%;
        height: 10px;
        content: '';
        position: absolute;
        background-color: #fff;
        top: 25px;
        left: -50%;
        z-index: -1;
    }

    #progress_bar li:first-child:after {
        content: none;
    }

    #progress_bar li.step-done {
        color: var(--main-color);
    }

        #progress_bar li.step-done:before {
            border-color: var(--main-color);
            background-color: var(--main-color);
            color: #fff;
            content: "\f00c"; 
            font-family: "FontAwesome";
        }

        #progress_bar li.step-done + li:after {
            background-color: var(--main-color);
        }

    #progress_bar li.step-active {
        color: var(--main-color);
    }

        #progress_bar li.step-active:before {
            border-color: var(--main-color);
            color: var(--main-color);
            font-weight: 700;
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


    .status-waiting {
        color: #000;
        border-color: #FFBF78; 
        background-color: #fdfd96;
    }

    .status-accepted {
        color: #000;
        border-color: #FF7D29; 
        background-color: #fdd835;
    }

    .status-delivering {
        color: #000;
        border-color: #4F200D; 
        background-color: #F2921D;
    }

    .status-completed {
        color: #000;
        border-color: #547C66; 
        background-color: #A2CA71;
    }
     
    .status-cancel {
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

     .table {
        background-color: rgba(238, 240, 229, 0.7);
        border-collapse: collapse;
        width: 100%;
     }

        .table tr:nth-child(odd) {
            background-color: rgba(238, 240, 229, 0.7); 
        }

        .table tr:nth-child(even) {
            background-color: #ffffff; 
        }

    .table tbody tr th {
        background-color: #163020;
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

   
    .map-link-box {
        font-family: Arial, sans-serif;
        color: #333;
        font-size: 16px;
        white-space: nowrap; 
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="row">
        <div class="col-12 text-center">
            <h3 style="padding-top: 20px; font-weight: bold; font-size: 34px;">
                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></h3>
           <div id="dateInfo" runat="server" style="display: none;">
            <span style="font-size: 18px;">📅</span><asp:Label ID="pickupDateLbl" runat="server" Text=" " style="font-size: 20px; font-weight: 500; padding-left: 10px;"></asp:Label><br />           
            <span style="font-size: 18px;">🕒</span><asp:Label ID="scheduledTimeLbl" runat="server" Text=" " style="font-size: 20px; font-weight: 500; padding-left: 10px;"></asp:Label><br />

            <span style="font-size: 20px; padding-right: 10px; font-style: italic;">Delivered by: </span><asp:HyperLink ID="lblRider" runat="server" Text="" style="font-size: 20px; font-style: italic; font-weight: 500; color: #1A3636;"></asp:HyperLink><br />
            <span style="font-size: 20px; padding-right: 10px; font-style: italic;">Note to Rider: </span><asp:Label ID="lblNote" runat="server" Text="" style="font-size: 20px; font-style: italic; font-weight: 500; color: #1A3636;"></asp:Label><br />
            <span style="font-size: 20px; padding-right: 10px; font-style: italic;">Note to Organization: </span><asp:Label ID="lblNote2" runat="server" Text="" style="font-size: 20px; font-style: italic; font-weight: 500; color: #1A3636;"></asp:Label><br />
        </div>
            <%--<asp:Button Id="btnTrack" class="btn btn-outline-primary" Text="Track Order Details" runat="server" style="display: none; align-items: center;"></asp:Button>--%>
        </div>
    </div>

    <div class="row" style="padding: 20px;">
    <!-- Donor Details Column -->
    <div class="col-md-6" style="display: none;" runat="server" id="donor">
        <div class="card">
            <div class="card-body" style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #FEF5ED; border-radius: 8px;">
                <h5 class="card-title" style="font-weight: bold;">Donor</h5>
                <p class="card-text">
                    <span style="font-size: 18px;">🏡</span> <asp:Label ID="lblDonorName" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📞</span> <asp:Label ID="lblDonorContact" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📍</span> <asp:Label ID="lblDonorAddress" runat="server" Text="" style="font-size: 16px;"></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <!-- Organization Details Column -->
    <div class="col-md-6" style="display: none;" runat="server" id="org">
        <div class="card">
            <div class="card-body" style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #EEF0E5; border-radius: 8px;">
                <h5 class="card-title" style="font-weight: bold;">Organization</h5>
                <p class="card-text">
                    <span style="font-size: 18px;">🏘️</span> <asp:Label ID="lblOrgName" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📞</span> <asp:Label ID="lblOrgContact" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📍</span> <asp:Label ID="lblOrgAddress" runat="server" Text="" style="font-size: 16px;"></asp:Label>
                </p>
            </div>
        </div>
    </div>
</div>

     <ol id="progress_bar" style="display: none;">
     <!-- Step 1: To Accept -->
     <li id="step1" runat="server" class="step-done">To Accept<br>
         <span id="date1" runat="server"></span>
     </li>

     <!-- Step 2: To Pick Up -->
     <li id="step2" runat="server" class="step-todo">To Pick Up<br>
         <span id="date2" runat="server"></span><br>
         <i id="showPickupImg" class="fas fa-file-image" style="font-size: 24px; display: none; color: black; cursor: pointer; padding-top: 10px;" runat="server" onclick="showPickupImage();"></i>
      
         
     </li>

     <!-- Step 3: To Reach -->
     <li id="step3" runat="server" class="step-todo">To Reach<br>
         <span id="date3" runat="server"></span>
         <br>        
         <i id="showReachImg" class="fas fa-file-image" style="font-size: 24px; color: black; cursor: pointer; display: none; padding-top: 10px;" runat="server" onclick="showReachImage();"></i>
     </li>

     <!-- Step 4: Completed -->
     <li id="step4" runat="server" class="step-todo">Completed<br>
         <span id="date4" runat="server"></span>
         <br>
         <a id="receiptAttch" runat="server" style="display: none;" target="_blank">
             <i class="fa fa-file-pdf" style="font-size: 24px; color: black; cursor: pointer; padding-top: 10px;"></i>
         </a>
     </li>
 </ol>

    <asp:Label ID="shareLiveLocationLabel" runat="server" Visible="false" Text="Share Live Location Link from Rider"
        Style="font-weight: bold; color: #304463; font-size: 16px; margin-left: 10px; margin-bottom: 5px;"></asp:Label>

    <div id="liveLinkContainer" runat="server" Visible="false" style="padding: 15px; background-color: #E3FEF7; border: solid 2px #304463; width: fit-content; border-radius: 8px; margin-left: 10px;">
        <asp:Label ID="liveLocationLink" runat="server" CssClass="map-link-box" Text="Link will be displayed here"></asp:Label>
    </div>


    <div id="map" style="height: 300px; width: 100%; position: relative; display: none;">
    
</div>


    <div id="navBar" class="container mt-3" style="display: none;" runat="server">
     <!-- Tabs for different statuses -->
     <ul class="nav nav-tabs">
         <li class="nav-item">
             <asp:LinkButton ID="lnkAll" runat="server" CssClass="nav-link active" OnClick="LoadOrder_Click" CommandArgument="All" style="color: #3B5B5D;">All</asp:LinkButton>
         </li>
         <li class="nav-item">
             <asp:LinkButton ID="lnkWaiting" runat="server" CssClass="nav-link" OnClick="LoadOrder_Click" CommandArgument="Waiting" style="color: #3B5B5D;">Waiting</asp:LinkButton>
         </li>
         <li class="nav-item">
             <asp:LinkButton ID="lnkAccepted" runat="server" CssClass="nav-link" OnClick="LoadOrder_Click" CommandArgument="Accepted" style="color: #3B5B5D;">Accepted</asp:LinkButton>
         </li>
         <li class="nav-item">
             <asp:LinkButton ID="lnkDeliver" runat="server" CssClass="nav-link" OnClick="LoadOrder_Click" CommandArgument="Delivering in progress" style="color: #3B5B5D;">Delivering</asp:LinkButton>
         </li>
         <li class="nav-item">
             <asp:LinkButton ID="lnkCompleted" runat="server" CssClass="nav-link" OnClick="LoadOrder_Click" CommandArgument="Reached Destination" style="color: #3B5B5D;">Completed</asp:LinkButton>
         </li>
         
         
     </ul>

     </div>

    <asp:Label ID="noDataLabel" runat="server" Text="No delivery made yet" Visible="false" CssClass="noData"></asp:Label>

     <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" Visible="false" OnRowDataBound="gvDonations_RowDataBound">
     <Columns>
         <asp:TemplateField HeaderText="Donation Details">
             <ItemTemplate>

                 <div class="row-container">

                     <!-- Status Box -->
                     <div class="status-box <%# GetStatus(Eval("deliveryStatus").ToString()) %>">
                         <%# Eval("deliveryStatus") %>
                     </div>

                     <!-- Donor Information -->
                     <div class="donation-details" style="margin-bottom: 10px;">
                         <strong>Donation ID:</strong> <%# Eval("donationId") %><br />
                         <strong>Donor Name:</strong> <%# Eval("donorFullName") %><br />        
                         <strong>Donor Contact Number:</strong> <%# Eval("donorPhone") %><br />        
                         <strong>Donor Email Address:</strong> <%# Eval("donorEmail") %><br />        
                         <strong>Destination Address:</strong> <%# Eval("destinationAddress") %><br />
                    </div>
                     <!-- Pickup Date and Time are displayed only when status is not Pending or To Pay -->

                     <asp:Panel ID="pnlPickUpDate" runat="server">
                         <strong style="bottom: -10px; position: relative;">Scheduled Pick Up Date:</strong>
                         <asp:Label ID="lblPickUpDate" runat="server" style="bottom: -10px; position: relative;"
                             Text='<%# Eval("pickupDate", "{0:MM/dd/yyyy}") %>'>
                         </asp:Label><br />
                     </asp:Panel>

                     <!-- Pick Up Time Section -->
                     <asp:Panel ID="pnlPickUpTime" runat="server">
                         <strong style="bottom: -10px; position: relative;">Scheduled Pick Up Time:</strong>
                         <asp:Label ID="lblPickUpTime" runat="server" style="bottom: -10px; position: relative;"
                             Text='<%# Eval("pickupTime", "{0:hh:mm tt}") %>'>
                         </asp:Label><br />
                     </asp:Panel>
                     <br />


                     <div class="col-md-12 mb-3">
                         <asp:PlaceHolder ID="phDonationItems" runat="server"></asp:PlaceHolder>
                     </div>
                     
                     <div class="text-right mt-2">
                         
                         <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("donationId") %>'
                             Text="View Delivery" CssClass="btn btn-info btn-sm" OnClick="btnViewDelivery_Click" style="background-color: #3B5B5D; border: solid 1px #3F4C48; padding: 8px 16px; font-size: 1em;"/>

                        
                     </div>


                 </div>
                 
             </ItemTemplate>
         </asp:TemplateField>
     </Columns>
 </asp:GridView>

    <div id="approvalDiv" class="container mt-5" style="display: none;" runat="server">
    <div class="card" style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #F4F7F7;">
        <div class="card-header text-white" style="background-color: #3B6978;">
            Item Arrival Verification
        </div>
        <div class="card-body">
            <h5 class="card-title">Did you receive the items?</h5>
            <p class="card-text">Please confirm whether you have received the items for Delivery ID: 
                <strong><asp:Label ID="lblDeliveryId" runat="server"></asp:Label></strong>.
            </p>
            
            <!-- Image from the delivery (reachImg) -->
            <div id="imageBox" runat="server">
                
            </div>
            
            <!-- Yes/No buttons-->
            <div class="text-center">
                <button id="btnYes" class="btn btn-success" onclick="confirmChoice(event, 'yes')">Yes, I received</button>
                <button id="btnNo" class="btn btn-danger" onclick="confirmChoice(event, 'no')">No, I did not receive</button>
            </div>
        </div>
    </div>
</div>



     <!-- Modal for displaying images -->
 <div class="modal fade" id="pickupImg" tabindex="-1" role="dialog" aria-labelledby="imagesModalLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="imagesModalLabel">Uploaded Files</h5>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">&times;</span>
                 </button>
             </div>
             <div class="modal-body">

                 <div id="pickupImageContent"></div>
             </div>
             <div class="modal-footer">
                 <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
             </div>
         </div>
     </div>
 </div>

 <!-- Modal for displaying images -->
 <div class="modal fade" id="reachImg" tabindex="-1" role="dialog" aria-labelledby="imagesModalLabel2" aria-hidden="true">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
                 <h5 class="modal-title" id="imagesModalLabe2">Uploaded Files</h5>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">&times;</span>
                 </button>
             </div>
             <div class="modal-body">

                 <div id="reachImageContent"></div>
             </div>
             <div class="modal-footer">
                 <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
             </div>
         </div>
     </div>
 </div>


    <script>
        function showPickupImage() {

            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);

            // deliveryId from the query string
            const donationId = url.get('donationId');

            if (donationId) {

                $.ajax({
                    type: "POST",
                    url: "Delivery.aspx/GetPickupImage",
                    data: JSON.stringify({ donationId: donationId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        var processedImagesHtml = response.d;

                        // inject the processed HTML into the modal body
                        document.getElementById('pickupImageContent').innerHTML = processedImagesHtml;

                        // show the modal
                        $('#pickupImg').modal('show');
                    },
                    error: function (xhr, status, error) {
                        console.log("Error fetching images: " + error);
                    }
                });
            } else {
                console.log("No donationId found in the query string.");
            }
        }

        function showReachImage() {

            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);

            // deliveryId from the query string
            const donationId = url.get('donationId');

            if (donationId) {

                $.ajax({
                    type: "POST",
                    url: "Delivery.aspx/GetReachImage",
                    data: JSON.stringify({ donationId: donationId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        var processedImagesHtml = response.d;

                        // inject the processed HTML into the modal body
                        document.getElementById('reachImageContent').innerHTML = processedImagesHtml;

                        // show the modal
                        $('#reachImg').modal('show');
                    },
                    error: function (xhr, status, error) {
                        console.log("Error fetching images: " + error);
                        console.log("Server response: " + xhr.responseText);
                    }
                });
            } else {
                console.log("No donationId found in the query string.");
            }
        }

        function toggleProgressBar() {
          
            const url = new URLSearchParams(window.location.search);        
            const progressBar = document.getElementById("progress_bar");
          
            if (url.has("donationId")) {
                // display the progress bar
                progressBar.style.display = "block";
            } else {
                // hide the progress bar
                progressBar.style.display = "none";
            }
        }

      //  window.onload = toggleProgressBar;

      // call backend
        function confirmChoice(event, choice) {
            event.preventDefault();

            const currentUrlParams = new URLSearchParams(window.location.search);
            const deliveryId = currentUrlParams.get("deliveryId");

            if (choice === 'yes') {
                Swal.fire({
                    title: 'Are you sure?',
                    text: "Once confirmed, you will acknowledge receipt of the items.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Yes, I\'m sure',
                    cancelButtonText: 'No, cancel',
                }).then((result) => {
                    if (result.isConfirmed) {
                        // send AJAX request to backend to generate receipt
                        $.ajax({
                            type: "POST",
                            url: '/Delivery.aspx/GenerateReceipt',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ deliveryId }), // send deliveryId from query string
                            success: function () {
                                Swal.fire({
                                    title: 'Thank You!',
                                    text: "A receipt has been generated and sent to the donor.",
                                    icon: 'success',
                                    confirmButtonText: 'OK'
                                }).then(() => {
                                    window.location.href = "Home.aspx";
                                });
                            },
                            error: function () {
                                Swal.fire({
                                    title: 'Error',
                                    text: 'There was an issue generating the receipt. Please try again later.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    }
                });
            } else if (choice === 'no') {
                Swal.fire({
                    title: 'Are you sure?',
                    text: "You are indicating that you did not receive the items. Action cannot be undone.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Yes, I\'m sure',
                    cancelButtonText: 'No, cancel',
                }).then((result) => {
                    if (result.isConfirmed) {
                        
                        $.ajax({
                            type: "POST",
                            url: '/Delivery.aspx/UpdateItemApproved',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ deliveryId }), 
                            success: function () {
                                Swal.fire({
                                    title: 'Please Attach Evidence',
                                    text: "You need to provide evidence, including the delivery tracking page and rider information. You will be redirected to the Contact Us page.",
                                    icon: 'info',
                                    confirmButtonText: 'Proceed'
                                }).then(() => {
                                    window.location.href = "Contact.aspx";
                                });
                            },
                            error: function () {
                                Swal.fire({
                                    title: 'Error',
                                    text: 'There was an issue updating the delivery status. Please try again later.',
                                    icon: 'error',
                                    confirmButtonText: 'OK'
                                });
                            }
                        });
                    }
                });
            }
        }

        let riderMarker; 
        let lastPosition = null;

        function initMap() {
            const centerLocation = { lat: 3.1390, lng: 101.6869 }; // Kuala Lumpur
            const map = new google.maps.Map(document.getElementById('map'), {
                center: centerLocation,
                zoom: 12,
                mapId: "1acb1d4b189a5d00",
            });

            const directionsService = new google.maps.DirectionsService();
            const directionsRenderer = new google.maps.DirectionsRenderer();
            directionsRenderer.setMap(map);

            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);
            const donationId = url.get('donationId');
            console.log("DonationId ID:", donationId);
           // addRiderMarker({ lat: 3.1390, lng: 101.6869 });

            // fetch pickup and destination addresses from the server
            $.ajax({
                url: 'Delivery.aspx/GetDeliveryAddress',
                method: 'POST',
                data: JSON.stringify({ donationId: donationId }),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (response) {
                    const data = JSON.parse(response.d);
                    const pickupAddress = data.pickupAddress;
                    const destinationAddress = data.destinationAddress;

                    console.log("Pickup Address:", pickupAddress);
                    console.log("Destination Address:", destinationAddress);

                    // display route from pickup to destination on the map
                    const geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ address: pickupAddress }, (pickupResults, status) => {
                        if (status === 'OK') {
                            geocoder.geocode({ address: destinationAddress }, (destinationResults, status) => {
                                if (status === 'OK') {
                                    const request = {
                                        origin: pickupResults[0].geometry.location,
                                        destination: destinationResults[0].geometry.location,
                                        travelMode: google.maps.TravelMode.DRIVING,
                                    };

                                    directionsService.route(request, (result, status) => {
                                        if (status === 'OK') {
                                            directionsRenderer.setDirections(result);
                                        } else {
                                            console.error("Could not display directions due to: " + status);
                                        }
                                    });
                                } else {
                                    console.error("Geocode failed for destination: " + status);
                                }
                            });
                        } else {
                            console.error("Geocode failed for pickup: " + status);
                        }
                    });

                    // start updating the rider's location on the map for the donor to track
                    updateRiderLocationOnMap(map, donationId);
                },
                error: function () {
                    console.error("Error fetching delivery details.");
                }
            });
        }

        function updateRiderLocationOnMap(map, donationId) {
            setInterval(() => {
                $.ajax({
                    url: 'Delivery.aspx/GetRiderLocation', 
                    method: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({ donationId: donationId }),
                    dataType: 'json',
                    success: function (response) {
                        const data = JSON.parse(response.d);

                        if (data.success) {
                            const newPosition = {
                                lat: parseFloat(data.lat),
                                lng: parseFloat(data.lng)
                            };

                            console.log("New Position:", newPosition);

                            // Check map object
                            console.log("Map Object:", map);

                            if (riderMarker) {
                                //animateMarker(riderMarker, lastPosition, newPosition);
                                riderMarker.setVisible(true);
                                riderMarker.setMap(null); // remove the marker from the map
                                riderMarker.setPosition(new google.maps.LatLng(newPosition.lat, newPosition.lng));
                                riderMarker.setMap(map);   // re-attach the marker to the map
                                riderMarker.setVisible(true);
                            } else {
                                riderMarker = new google.maps.Marker({
                                    position: new google.maps.LatLng(newPosition.lat, newPosition.lng),
                                    map: map,
                                    title: 'Rider Location', 
                                    visible: true
                                });
                            }

                            // center map on the new position and update lastPosition
                            map.setCenter(new google.maps.LatLng(newPosition.lat, newPosition.lng));
                            lastPosition = newPosition;
                        } else {
                            console.error("Location not found:", data.message);
                        }
                    },
                    error: function () {
                        console.error("Error fetching rider's location.");
                    }
                });
            }, 10000); // update every 10 seconds
        }

        // function to smoothly animate the AdvancedMarkerElement between two points
        function animateMarker(marker, fromPosition, toPosition) {
            if (!fromPosition) {
                // If no previous position, just set the marker to the new position
                marker.position = toPosition;
               
                return;
            }

            let deltaLat = (toPosition.lat - fromPosition.lat) / 20;
            let deltaLng = (toPosition.lng - fromPosition.lng) / 20;

            let step = 0;
            let interval = setInterval(() => {
                step += 1;
                let lat = fromPosition.lat + deltaLat * step;
                let lng = fromPosition.lng + deltaLng * step;

                marker.position = new google.maps.LatLng(lat, lng);
                
                if (step >= 20) {
                    clearInterval(interval);
                }
            }, 50); // Runs every 50ms for smooth animation
        }

        // initialize the map when the window loads
        window.onload = function () {
            if (typeof userRole !== "undefined" && userRole === "donor") {
                // if the user role is donor, display the map
                document.getElementById("map").style.display = "block";
                initMap();
            } else if (typeof userRole !== "undefined" && userRole === "organization") {
                // if the user role is organization, check for the 'token' query string
                const urlParams = new URLSearchParams(window.location.search);
                if (urlParams.has("token")) {
                    // hide the map
                    document.getElementById("map").style.display = "none";
                }
                if (urlParams.has("donationId")) {
                    // hide the map
                    document.getElementById("map").style.display = "block";
                    initMap();
                } else {
                    // display the map 
                    document.getElementById("map").style.display = "none";
                    
                }
            }
        };





    </script>



</asp:Content>

