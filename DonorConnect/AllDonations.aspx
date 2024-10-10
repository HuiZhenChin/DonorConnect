<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllDonations.aspx.cs" Inherits="DonorConnect.AllDonations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Item Donations</title>
    
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.3/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBHd69lOb31ywFRMu99sos-ysgl-uCtidY&map_ids=69dfb9f5c086e55e&callback=console.debug&libraries=maps,marker&v=beta"></script>

   
    <style>
  
     .category-box {
        display: inline-block;
        padding: 10px;
        margin: 5px;
        border: 2px solid;
        border-radius: 5px;
        text-align: center;
        min-width: 100px;
     }

        
     .border-food {
        border-color: #f39c12;
     }

     .border-books {
        border-color: #3498db;
     }

     .border-toys {
        border-color: #e74c3c;
     }

     .border-medical {
        border-color: #2ecc71;
     }

     .border-clothing{
         border-color: #702963;
     }

     .border-electronics{
         border-color: #EADDCA;
     }

     .border-furniture{
         border-color: #C9CC3F;
     }

     .border-hygiene{
         border-color: #C9A9A6;
     }

     .border-default{
         border-color: dimgray;
     }

     .dropdown {
    position: relative;
    display: inline-block;
}

    .dropdown-toggle {
       
        color: white;
        border: none;
        padding: 10px 20px;
        font-size: 16px;
        cursor: pointer;
    }

    .dropdown-menu {
        display: none;
        position: absolute;
        background-color: #f9f9f9;
        min-width: 160px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        z-index: 1;
    }

    .dropdown-menu .dropdown-submenu {
        position: relative;
    }

    .dropdown-menu .dropdown-item {
        color: #000;
        padding: 12px 16px;
        text-decoration: none;
        display: block;
    }

    .dropdown-menu .dropdown-item:hover {
        background-color: #f1f1f1;
    }

    /* Submenu styles */
    .submenu {
        display: none;
        position: absolute;
        left: 100%;
        top: 0;
        background-color: #f9f9f9;
        min-width: 160px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
    }

    .dropdown-submenu:hover .submenu {
        display: block;
    }

    .submenu-item {
        padding: 12px 16px;
        text-decoration: none;
        display: block;
        color: #000;
    }

    .submenu-item:hover {
        background-color: #f1f1f1;
    }

    /* Show the dropdown menu on hover */
    .dropdown:hover .dropdown-menu {
        display: block;
    }

    /* Container for category columns */
    .category-container {
        display: flex; 
        flex-wrap: wrap; 
        gap: 10px; 
        padding: 10px; 
    }

    /* Style for each category column */
    .category-column {
        flex: 1 1 200px; 
        box-sizing: border-box;
        margin: 10px;
        border: 1px solid #ccc; 
        padding: 10px; 
    }

    /* Style for checkboxes in category columns */
    .category-checkbox {
        display: block;
        margin-bottom: 10px;
    }

    /* Style for items within each category */
    .item-column {
        margin-bottom: 5px;
    }

    /* Style for checkboxes in item columns */
    .item-checkbox {
        display: block;
    }

    /* Container for state cards */
    .state-container {
        display: flex; 
        flex-wrap: wrap; 
        gap: 10px; 
        padding: 10px; 
    }

    /* Style for each state card */
    .state-card {
        flex: 1 1 150px; 
        box-sizing: border-box;
        margin: 10px;
        border: 1px solid #ccc; 
        padding: 10px;
    }

    /* Style for checkboxes in state cards */
    .state-checkbox {
        display: block;
        margin: 0;
    }

    .urgent-card {
        background-color: #FFF5EE;
    }

    .card-body {
        position: relative;
        padding-top: 20px; 
    }

    .urgent-card::before {
        content: "";
        position: absolute;
        top: -8px;
        left: 0;
        width: 100%;
        height: 8px; 
        background: repeating-linear-gradient(
            45deg,
            #FAA0A0,
            #FAA0A0 10px,
            white 10px,
            white 20px
        );
        z-index: 1; 
        border-radius: 5px 5px 0 0; 

    }

    .image-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
        gap: 10px;
    }

    .image-item img {
        width: 200px;
        height: 200px;
        object-fit: cover;
    }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="filter-bar d-flex align-items-center mb-4" style="padding-top:20px;">

        <!-- Search Input -->
        <asp:TextBox ID="txtSearchKeyword" runat="server" CssClass="form-control" Placeholder="Search by keyword..." AutoPostBack="True" OnTextChanged="SearchDonations"></asp:TextBox>

        <!-- Categories Button -->
        <asp:Button ID="btnShowCategories" runat="server" Text="Categories" CssClass="btn btn-primary ml-3" OnClick="LoadCategories" />

         <!-- States Button -->
        <asp:Button ID="btnShowStates" runat="server" Text="States" CssClass="btn btn-primary ml-3" OnClick="LoadStates" />

       <asp:Button ID="btnShowMap" runat="server" Text="Show Map" CssClass="btn btn-primary ml-3" 
        OnClientClick="getUserCurrentLocation(); initMap(); return false;" />

        

    </div>

    <!-- Map Container (Initially hidden) -->
    <div id="map" style="height: 400px; width: 100%; display: none;"></div>


<!-- Category and Its Specific Items -->
<div class="category-container">
    <asp:Repeater ID="rptCategories" runat="server">
        <ItemTemplate>
            <div class="category-column">
                <asp:CheckBox ID="chkCategory" runat="server" Text='<%# Container.DataItem %>'
                    OnCheckedChanged="CategorySelected" AutoPostBack="True" CssClass="category-checkbox" />
                <asp:Repeater ID="rptItems" runat="server">
                    <ItemTemplate>
                        <div class="item-column">
                            <asp:CheckBox ID="chkItem" runat="server" Text='<%# Eval("Item") %>' 
                                AutoPostBack="True" CssClass="item-checkbox" />
                            <asp:HiddenField ID="hfCategory" runat="server" Value='<%# Eval("Category") %>' />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

    <!-- States Repeater -->
<div class="state-container">
    <asp:Repeater ID="rptStates" runat="server">
        <ItemTemplate>
            <div class="state-card">
                <asp:CheckBox ID="chkState" runat="server" Text='<%# Container.DataItem %>' 
                    OnCheckedChanged="StateSelected" AutoPostBack="True" CssClass="state-checkbox" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

 
    <asp:HiddenField ID="hfStateName" runat="server" />

    <asp:HiddenField ID="hfCategoryName" runat="server" />


    <asp:Button ID="btnFilterDonations" runat="server" Text="Filter Donations" OnClick="FilterDonations" CssClass="btn btn-primary" />

     <div class="mt-3">
        <asp:Label ID="lblNoResults" runat="server" style="margin-top: 20px;" CssClass="alert alert-warning" Visible="false" />
    </div>

   <div style="padding-top: 10px;">
        <asp:Literal ID="ltOrgProfile" runat="server"></asp:Literal>
    </div>

    <div style="padding-top: 10px;">
        <asp:Literal ID="ltDonation" runat="server"></asp:Literal>
    </div>

    <asp:GridView ID="gvAllDonations" runat="server" AutoGenerateColumns="False" CssClass="centered-grid" DataKeyNames="donationPublishId" GridLines="None" BorderStyle="None" CellPadding="0">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="row" data-donation-id='<%# Eval("donationPublishId") %>'>                
                        <%-- Profile Picture and Organization Name --%>
                        <div class="col-md-3 d-flex">
                            <img src='<%# String.IsNullOrEmpty(Eval("orgProfilePic") as string) ? "/Image/default_picture.jpg" : "data:image/png;base64," + Eval("orgProfilePic") %>'
                                 alt="Org Profile Picture"
                                 style="width: 100px; height: 100px; border-radius: 50%; border: 1px solid black;" />
                            <a href='<%# "PreviewPublicInfo.aspx?role=organization&username=" + Eval("orgName") %>' style="text-decoration: none; color: black;">
                                <strong class="ml-2"><%# Eval("orgName") %></strong>
                            </a>


                        </div>

                        <%-- Card Body with Donation Details --%>
                      <div class="col-md-9">
                        <div class="card mb-4 shadow-sm card-custom" style="width: 100%!important;">
                            <div class="card-body <%# Eval("cardBody") %>" style="width: 100%!important;">
                                
                                <div class="row">
                                    <div class="col-md-8">
                                        <%-- URGENT Label --%>
                                        <%# Eval("urgentLabel") %>
                                            <%-- Title --%>
                                            <h3 class="card-title text-center"><%# Eval("title") %></h3>

                                            <%-- People Needed and State --%>
                                            <div class="row mb-3">
                                                <div class="col-md-6">
                                                    <strong>People In Need For Items:</strong>
                                                    <i class="icon icon-people-needed"></i><%# Eval("peopleNeeded") %>
                                                </div>
                                                <div class="col-md-6 text-right">
                                                    <strong>State:</strong>
                                                    <i class="icon icon-state"></i><%# Eval("donationState") %>
                                                </div>
                                            </div>

                                            <%-- Address and Description --%>
                                            <div class="mb-3">
                                                <strong>Address:</strong> <%# Eval("address") %><br />
                                                <strong>Description:</strong> <%# Eval("description") %>
                                            </div>

                                            <%-- Restrictions --%>
                                            <div class="mb-3">
                                                <strong>Restrictions:</strong>
                                                <asp:Label ID="lblRestrictions" runat="server" Text='<%# Eval("restriction") %>' Visible='<%# !String.IsNullOrEmpty(Eval("restriction") as string) %>'></asp:Label>
                                            </div>

                                            <%-- Item Details --%>
                                            <div class="mb-3">
                                                <strong>Item Details:</strong> <%# Eval("itemDetails") %>
                                            </div>

                                            <%-- Images and Files --%>
                                            <div class="mb-3">
                                                <%# String.IsNullOrEmpty(Eval("donationImages") as string) ? "" : "<strong>Images:</strong> " + Eval("donationImages") %>
                                            </div>
                                            <div>
                                                <%# String.IsNullOrEmpty(Eval("donationFiles") as string) ? "" : "<strong>Files:</strong> " + Eval("donationFiles") %>
                                            </div>
                                        </div>
                                        <div class="col-md-4 text-center">
                                            <%-- Item Category and Icons --%>
                                            <asp:Label ID="lblItemCategory" runat="server" Text='<%# GetItemCategoryWithIcon(Eval("itemDetails")) %>' />
                                            <div class="row mb-3">
                                                <div class="col-12 text-right position-absolute" style="bottom: 20px; right: 10px;">
                                                    <asp:LinkButton class="fas fa-heart " style='<%# "font-size: 24px; cursor: pointer; padding-right: 20px; color: " + Eval("saveButton") %>' title="Save to Favorites" runat="server" OnClick="btnSaveFav_Click" CommandArgument='<%# Eval("donationPublishId") %>'></asp:LinkButton>
                                                    <asp:Button ID="btnDonate" runat="server" type="submit" CssClass="btn btn-success" Text="Donate Now!" OnClick="btnDonate_Click" CommandArgument='<%# Eval("donationPublishId") %>'/>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <script>
       
        $(document).ready(function () {
            
            $('#filterButton').dropdown();
        });

        function showErrorMsg(message) {
            Swal.fire({
                title: 'Error!',
                text: message,
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }

        function showSuccess(message) {
            Swal.fire({
                text: message,
                icon: 'success',
                confirmButtonText: 'OK',
                timer: 5000,
                timerProgressBar: true,
                willClose: () => {

                    window.location.href = 'AllDonations.aspx';
                }
            });
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

        //function getUserCurrentLocation() {
        //    if (navigator.geolocation) {
        //        navigator.geolocation.getCurrentPosition(showPosition, showError);
        //    } else {
        //        alert("Geolocation is not supported by the browser.");
        //    }
        //}


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
            fetchAddressesAndDisplayMarkers(map);
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
            fetchAddressesAndDisplayMarkers(map);
        }

        // fetch addresses from server side and display them on the map
        function fetchAddressesAndDisplayMarkers(map) {
            $.ajax({
                async: false,
                type: 'POST',
                url: 'AllDonations.aspx/GetActiveOrganizationAddresses',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var organizationInfo = response.d;

                    var geocoder = new google.maps.Geocoder();
                    organizationInfo.forEach(function (org) {
                        var orgName = org.orgName;
                        var orgAddress = org.orgAddress;

                        geocodeAddress(orgAddress, orgName, geocoder, map);
                    });
                },
                error: function (error) {
                    console.error('Error fetching addresses:', error);
                }
            });
        }

        function fetchDonationsAndShowInfo(orgName, marker, map) {
            $.ajax({
                type: 'POST',
                url: 'AllDonations.aspx/GetDonationsByOrg',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ orgName: orgName }),
                dataType: 'json',
                success: function (response) {
                    var donations = response.d;
                    var contentString = '<div><h3>' + orgName + '</h3><ul>';

                    if (donations.length > 0) {
                        donations.forEach(function (donation) {
                            contentString += '<li><b>' + donation.title + '</b>: ' + donation.itemCategory + '</li>';
                        });
                    } else {
                        contentString += '<li>No donations available.</li>';
                    }

                    contentString += '</ul>';
                    var url = 'PreviewPublicInfo.aspx?role=organization&orgName=' + encodeURIComponent(orgName);
                    console.log('Redirect URL:', url); 
                    contentString += '<button onclick="window.open(\'' + url + '\', \'_blank\')">View Details</button>';

                    var infoBox = new google.maps.InfoWindow({
                        content: contentString
                    });

                    infoBox.open(map, marker);
                },
                error: function (error) {
                    console.error('Error fetching donations:', error);
                }
            });
        }

        function geocodeAddress(address, orgName, geocoder, map) {
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status === google.maps.GeocoderStatus.OK) {
                    var location = results[0].geometry.location;

                    var marker = new google.maps.Marker({
                        position: location,
                        map: map,
                        title: orgName,
                        icon: {
                            url: "http://maps.google.com/mapfiles/ms/icons/red-dot.png",
                            scaledSize: new google.maps.Size(40, 40)
                        }
                    });

                    // Add click event listener to marker
                    google.maps.event.addListener(marker, 'click', function () {
                        fetchDonationsAndShowInfo(orgName, marker, map);
                    });

                    map.setCenter(location);
                } else {
                    console.error('Geocode was not successful with this reason: ' + status);
                }
            });
        }

        function scrollToSelectedDonation(donationPublishId) {
            
            var donation = document.querySelector(`[data-donation-id='${donationPublishId}']`);
            if (donation) {
               
                donation.scrollIntoView({ behavior: 'smooth' });
            }
        }

     
    </script>
</asp:Content>


