<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AcceptDelivery.aspx.cs" Inherits="DonorConnect.AcceptDelivery" %>
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

    <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBHd69lOb31ywFRMu99sos-ysgl-uCtidY&map_ids=69dfb9f5c086e55e&callback=console.debug&libraries=maps,marker&v=beta"></script>

    
    <style>
        html, body {
            height: 100%;
            overflow: auto!important;
        }

         body{
            background: rgb(231,246,242);
            background: linear-gradient(180deg, rgba(231,246,242,1) 18%, rgba(165,201,202,1) 60%, rgba(57,91,100,1) 86%, rgba(44,51,51,1) 100%);
         }
      
        .centered-row {
            display: flex;
            justify-content: center;
            padding-left: 50px;
        }

        .centered-container {
            display: flex;
            justify-content: center;
            overflow: visible; 
        }

        .centered-grid {
            width: 100%;
            max-width: 1200px; 
        }

        .order-info-panel {
            border: 1px solid #ddd; 
            border-radius: 8px; 
            padding: 15px; 
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
            background-color: #fff; 
            transition: all 0.3s ease-in-out; 
        }

        .row.d-flex {
            display: flex;
        }

        .col-md-6.d-flex {
            display: flex;
            flex-direction: column;
        }

       
        .card {
            flex-grow: 1;
        }

      
        .order-info-panel .card {
            border: 1px solid #ddd; 
            border-radius: 8px; 
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); 
            flex-grow: 1;
        }


        .table-custom {
            margin-top: 20px;
            border: none;          
            border-collapse: separate;
            overflow: hidden;
            justify-content: center;
            display: grid;
        }

            .table-custom th, .table-custom td {
                border: 1px solid black;
                padding: 8px;
            }


            .table-custom th {
                background-color: #f8f9fa;
                text-align: center;
            }


    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-4" style="padding-top: 10px;">
        <!-- Recommended/All filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlRecommendedAll" runat="server" CssClass="form-control" style="border: solid 2px #425F57; background-color: #E9EFEC;">
                <asp:ListItem Text="Recommended*" Value="Recommended"></asp:ListItem>
                <asp:ListItem Text="All" Value="All"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Pickup State filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlPickupState" runat="server" CssClass="form-control" style="border: solid 2px #425F57; background-color: #E9EFEC;">
                       
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
            <asp:DropDownList ID="ddlDestinationState" runat="server" CssClass="form-control" style="border: solid 2px #425F57; background-color: #E9EFEC;">
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
            <asp:DropDownList ID="ddlEarnings" runat="server" CssClass="form-control" style="border: solid 2px #425F57; background-color: #E9EFEC;">
                <asp:ListItem Text="Earnings" Value=""></asp:ListItem>
                <asp:ListItem Text="Highest to Lowest" Value="DESC"></asp:ListItem>
                <asp:ListItem Text="Lowest to Highest" Value="ASC"></asp:ListItem>
            </asp:DropDownList>
        </div>

        <!-- Vehicle Type filter -->
        <div class="col-md-2" style="padding-top: 10px;">
            <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control" style="border: solid 2px #425F57; background-color: #E9EFEC;">
                <asp:ListItem Text="Vehicle Type (Any)" Value=""></asp:ListItem>
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
            <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnFilter_Click" style="background-color: #304D30; border: none;"/>
            <asp:Button ID="btnMap" runat="server" CssClass="btn btn-primary" Text="Map" OnClick="btnMap_Click" style="margin-left: 20px; background-color: #163020; border: none;"/>
        </div>
              
        <div id="map" style="height: 400px; width: 100%; display: none; margin-top: 10px; "></div>


    </div>
<asp:UpdatePanel ID="upAllDonations" runat="server">
    <ContentTemplate>
        <div class="centered-container">
            <asp:GridView ID="gvAllDonations" runat="server" AutoGenerateColumns="False" CssClass="centered-grid" DataKeyNames="deliveryId" GridLines="None" BorderStyle="None" CellPadding="0" OnRowCommand="gvAllDonations_RowCommand">
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
                                                    <asp:Button ID="btnViewOrder" runat="server" CssClass="btn btn-success" Text="View Order ->" CommandArgument='<%# Eval("deliveryId") %>' CommandName="ViewOrder" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 d-flex align-items-stretch">
                                    <asp:Panel ID="pnlOrderInfo" runat="server" CssClass="order-info-panel" Style="display: none; padding-bottom: 20px;">
                                        <div class="order-info" style="position: relative; height: 100%;">
                                            <div class="map-icon" style="position: absolute; top: 10px; right: 10px;">
                                                <i class="fas fa-map-marker-alt"
                                                    style="font-size: 24px; color: black; cursor: pointer;"
                                                    data-toggle="modal" data-target="#mapModal"
                                                    onclick="openMap('<%# Eval("deliveryId") %>')"></i>
                                            </div>

                                            <h3>Order Details</h3>
                                            <p><strong>Donor Name:</strong>
                                                <asp:Label ID="lblDonorName" runat="server" /></p>
                                            <p><strong>Donor Phone:</strong>
                                                <asp:Label ID="lblDonorPhone" runat="server" /></p>
                                            <p><strong>Organization Name:</strong>
                                                <asp:Label ID="lblOrgName" runat="server" /></p>
                                            <p><strong>Organization Phone:</strong>
                                                <asp:Label ID="lblOrgPhone" runat="server" /></p>
                                            <p>
                                                <strong>Note to Rider:</strong>
                                                <asp:Label ID="lblNote" runat="server" />
                                            </p>

                                            <asp:GridView ID="gvDonationItems" runat="server" AutoGenerateColumns="False" CssClass="table-custom">
                                                <Columns>
                                                    <asp:BoundField DataField="itemCategory" HeaderText="Category" />
                                                    <asp:BoundField DataField="item" HeaderText="Item" />
                                                    <asp:BoundField DataField="quantityDonated" HeaderText="Quantity" />
                                                </Columns>
                                            </asp:GridView>

                                            <div style="display: flex; justify-content: end; padding-top: 10px;">
                                                <asp:Button ID="btnAccept" runat="server" OnClick="btnAcceptOrder_Click" CssClass="btn btn-success" Text="Accept Order" CommandArgument='<%# Eval("deliveryId") %>' />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

    <!-- pop-up map -->
    <div class="modal fade" id="mapModal" tabindex="-1" role="dialog" aria-labelledby="mapModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="mapModalLabel">Delivery Location View</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="mapDiv" style="height: 500px; width: 100%;"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



    <script type="text/javascript">
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

        function showInfo(message) {
            Swal.fire({
                text: message,
                icon: 'info',
                confirmButtonText: 'OK',
            }).then((result) => {
                if (result.isConfirmed) {
                   
                    getUserCurrentLocation();
                    initMap();
                }
            });
        }

       
        function confirmAccept(deliveryId) {
            Swal.fire({
                title: 'Vehicle Mismatch (X)',
                text: 'Your vehicle type does not match the required delivery vehicle type. Please contact the donor before you accept the order. Do you still want to proceed?',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#28a745',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Yes, Proceed',
                cancelButtonText: 'No, Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                    // make an AJAX call to accept the order
                    $.ajax({
                        type: "POST",
                        url: "AcceptDelivery.aspx/AcceptOrder",
                        data: JSON.stringify({ deliveryId: deliveryId }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d === "success") {
                                Swal.fire({
                                    title: 'Accepted!',
                                    text: 'Successful! Please visit your Dashboard to view the location navigation and status update of the order. Note that you will only receive the money earned in the DonorConnect wallet when you have delivered the item to the destination (status is Completed).',
                                    icon: 'success',
                                    confirmButtonText: 'OK'
                                    }).then(() => {
                                        
                                        window.location.href = 'Home.aspx';
                                });
                            } else {
                                Swal.fire('Error!', 'There was an error accepting the order.', 'error');
                            }
                        },
                        error: function () {
                            Swal.fire('Error!', 'There was an error processing your request.', 'error');
                        }
                    });
                }
            });

            return false;  
        }

        function getUserCurrentLocation() {
            if (navigator.geolocation) {

                navigator.geolocation.getCurrentPosition(showPosition, showError, {
                    enableHighAccuracy: true,
                    timeout: 10000, // wait up to 10 seconds
                    maximumAge: 0    // don't use a cached position
                });
            } else {
                alert("Geolocation is not supported by the browser.");
            }
        }
   
        // show, plot the map and center it at the user's location
        function showPosition(position) {
            var userLocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            // display the map 
            document.getElementById('map').style.display = 'block';

            // display the map centered at the user's location
            var map = new google.maps.Map(document.getElementById('map'), {
                center: userLocation,
                zoom: 12
            });

            // mark user's location
            var marker = new google.maps.Marker({
                map: map,
                position: userLocation,
                title: "You are here!"
            });

            // mark organization addresses location
            fetchAddress(map);
        }

        // handle errors in getting the user's location
        function showError(error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    alert("User denied the request for geolocation.");
                    break;
                case error.POSITION_UNAVAILABLE:
                    alert("Location information is not available.");
                    break;
                case error.TIMEOUT:
                    alert("The request to get user location timed out.");
                    break;
                case error.UNKNOWN_ERROR:
                    alert("Unknown error occurred.");
                    break;
            }
        }

        // initialise map
        function initMap() {
            var map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 40.1215, lng: -100.4504 },
                zoom: 4,
                mapId: '69dfb9f5c086e55e' // valid map ID get from Google Cloud Console
            });

            // fetch active organization addresses from database
            fetchAddress(map);
        }

        // fetch addresses from server side and display them on the map

        function fetchAddress(map) {
            $.ajax({
                async: false,
                type: 'POST',
                url: 'AcceptDelivery.aspx/GetPickupAddress',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
       
                success: function (response) {
                    var address = response.d;
                    var geocoder = new google.maps.Geocoder();

                    address.forEach(function (donor) {
                        var donorName = donor.donorName;
                        var donorAddress = donor.pickupAddress.trim();
                        var orgAddress = donor.destinationAddress.trim();
                        var donorPhone = donor.donorPhone.trim();
                        var deliveryId = donor.deliveryId;

                        console.log(address);
                    geocodeAddress(deliveryId, donorAddress, donorName, orgAddress, donorPhone, geocoder, map);
                       // geocodeAddress(donorAddress, donorName, orgAddress, donorPhone, geocoder, map);
                    });
                },
                error: function (error) {
                    console.error('Error fetching addresses:', error);
                }
            });
        }


        function geocodeAddress(deliveryId, donorAddress, donorName, orgAddress, donorPhone, geocoder, map) {
            geocoder.geocode({ 'address': donorAddress }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    var location = results[0].geometry.location;

                    var marker = new google.maps.Marker({
                        position: location,
                        map: map,
                        title: donorAddress,
                        icon: {
                            url: "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
                            scaledSize: new google.maps.Size(40, 40)
                        }
                    });

                    var window = new google.maps.InfoWindow({
                        content: `
                    <div>
                        <h4>Donor Name: ${donorName}</h4>
                        <p><strong>From:</strong> ${donorAddress}</p>
                        <p><strong>To:</strong> ${orgAddress}</p>
                        <p><strong>Donor's Contact:</strong> ${donorPhone}</p>
                        <p><strong>DeliveryId:</strong> ${deliveryId}</p>

                        <button id="viewOrderBtn_${deliveryId}" type="button">View Order</button>
                    </div>`
                    });

                    google.maps.event.addListener(marker, 'click', function () {
                        window.open(map, marker);  // open the InfoWindow

                      
                        google.maps.event.addListenerOnce(window, 'domready', function () {
                            var button = document.getElementById(`viewOrderBtn_${deliveryId}`);
                            if (button) {
                                button.addEventListener('click', function () {
                                    event.preventDefault(); 
                                    scrollToOrder(deliveryId);  // call the scroll function when click the button
                                });
                            }
                        });
                    });

                    map.setCenter(location); 
                } else {
                    console.error('Geocode failed for address: ' + donorAddress + ' with status: ' + status);
                }
            });
        }


        function scrollToOrder(deliveryId) {
            var element = document.querySelector(`[data-donation-id='${deliveryId}']`);

            if (element) {
                
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
             
                element.style.transition = 'background-color 0.5s ease';  
                element.style.backgroundColor = '#40534C';  

                setTimeout(function () {
                    element.style.backgroundColor = '';  
                }, 3000); 
            } else {
                console.error('Could not find the order with deliveryId: ' + deliveryId);
            }
        }


        function openMap(deliveryId) {

            $('#mapModal').on('shown.bs.modal', function () {
              
                initSpecificLocation(deliveryId);
            });
        }

        function initSpecificLocation(deliveryId) {
            var map = new google.maps.Map(document.getElementById('mapDiv'), {
                center: { lat: 40.1215, lng: -100.4504 },  
                zoom: 7,
                mapId: '69dfb9f5c086e55e'
            });

            console.log(deliveryId);

            if (!deliveryId) {
                console.error('deliveryId not found');
                return;
            }

            // fetch and geocode pickup and organization addresses
            getPickUpAddress(deliveryId, function (pickupLocation, pickupAddress) {
                if (pickupLocation) {
                    fetchOrgAddress(deliveryId, function (orgLocation, orgAddress) {
                        if (orgLocation) {
                            plotRouteOnMap(pickupLocation, orgLocation, pickupAddress, orgAddress, map);
                        } else {
                            console.error('Organization address not found or geocode failed.');
                        }
                    });
                } else {
                    console.error('Pickup address not found or geocode failed.');
                }
            });
        }


        // function to plot the shortest route between pickup and organization addresses on the map
        function plotRouteOnMap(pickupLocation, orgLocation, pickupAddress, orgAddress, map) {
            var directionsService = new google.maps.DirectionsService();
            var directionsRenderer = new google.maps.DirectionsRenderer();

            // set the map for the directions renderer to display the route
            directionsRenderer.setMap(map);

            // request the route from the Directions API
            var request = {
                origin: pickupLocation,  // starting point
                destination: orgLocation,  // ending point
                travelMode: 'DRIVING'
            };

            directionsService.route(request, function (result, status) {
                if (status === 'OK') {
                    // display the route on the map
                    directionsRenderer.setDirections(result);
                 

                } else {
                    console.error('Directions request failed due to ' + status);
                }
            });
        }


        function getPickUpAddress(deliveryId, callback) {
            if (!deliveryId) {
                console.error('deliveryId is missing.');
                callback(null, null);
                return;
            }

            $.ajax({
                type: "POST",
                url: "AcceptDelivery.aspx/GetPickupAddress2",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ deliveryId: deliveryId }), 
                dataType: "json",
                success: function (response) {
                    var backendAddress = response.d;  
                    console.log("Address from backend: ", backendAddress);

                    if (!backendAddress) {
                        console.error('No address found in backend response');
                        callback(null, null); 
                        return;
                    }

                    var geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ 'address': backendAddress }, function (results, status) {
                        if (status === 'OK') {
                            var location = results[0].geometry.location;  // get geocoded location
                            callback(location, backendAddress);  
                        } else {
                            console.error('Geocode for pickup address failed: ' + status);
                            callback(null, null);  
                        }
                    });
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching address from backend:", error);
                    console.error("Status:", status);
                    console.error("Response Text:", xhr.responseText);  
                    callback(null, null);  
                }
            });
        }



        // function to fetch and geocode organization address based on deliveryId
        function fetchOrgAddress(deliveryId, callback) {
            if (!deliveryId) {
                console.error('deliveryId is missing.');
                return;
            }

            $.ajax({
                type: 'POST',
                url: 'AcceptDelivery.aspx/GetDestinationAddress',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ deliveryId: deliveryId }),  
                dataType: 'json',
                success: function (response) {
                    var address = response.d;

                    if (address) {
                        address = address.trim();

                        var geocoder = new google.maps.Geocoder();
                        geocoder.geocode({ 'address': address }, function (results, status) {
                            if (status === 'OK') {
                                var location = results[0].geometry.location;
                                callback(location, address);  // pass geocoded location and original address
                            } else {
                                console.error('Geocode for organization address failed: ' + status);
                                callback(null, null);
                            }
                        });
                    } else {
                        console.error('Organization address not found.');
                        callback(null, null);
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error fetching organization address:", error);
                    console.error("Response Text:", xhr.responseText);  
                    callback(null, null);
                }
            });
        }


     




       


    </script>



</asp:Content>

