<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgReviewDonationRequest.aspx.cs" Inherits="DonorConnect.OrgReviewDonationRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donation Request</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.10.2/dist/umd/popper.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.min.js"></script>
    <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBHd69lOb31ywFRMu99sos-ysgl-uCtidY&callback=initMap&libraries=places"></script>
 
    <style>
         body {
           background: rgb(249,247,247);
           background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
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
    <div class="card-deck">
        <div class="card mt-3 shadow-sm" style=" background-color: rgba(255, 255, 255, 0.8);">
            <div class="card-body">
                <div id="donationRequest" runat="server">
                    <asp:Label ID="lblDonation" runat="server" CssClass="d-block text-center mb-4" Text="New Donation Request" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                        <div class="col-md-12 mb-3">
                            <asp:PlaceHolder ID="phDonationItems" runat="server"></asp:PlaceHolder>
                        </div>

                    <asp:Repeater ID="rptDonation" runat="server" OnItemDataBound="rptDonation_ItemDataBound">
                        <ItemTemplate>
                          
                            <div class="row mb-2">
                                <div class="col-md-4 font-weight-bold">
                                    <asp:Literal ID="litLabel" runat="server" Text='<%# Eval("FieldName") %>'></asp:Literal>
                                </div>
                                <div class="col-md-8">
                                    <asp:Literal ID="litValue" runat="server" Text='<%# Eval("FieldValue") %>'></asp:Literal>
                                </div>
                            </div>

                         
                        </ItemTemplate>

                        <FooterTemplate>
                            <div class="row">
                                <div class="col-md-12 text-right">
                                    <asp:LinkButton ID="btnApprove" runat="server" CommandArgument='<%# Eval("donationId") %>' Text="Approve" CssClass="btn btn-info btn-lg" style="background-color: seagreen;" OnClick="btnApprove_Click"/>
                                    <asp:LinkButton ID="btnReject" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("donationId") %>' Text="Reject" CssClass="btn btn-danger btn-lg" OnClientClick='<%# "showRejectionModal(event, \"" + Eval("donationId") + "\"); return false;" %>' />
                                </div>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>                

                </div>
            </div>
        </div>
    </div>

    <!-- Rejection Reason Dialog Box -->
    <div class="modal fade" id="rejectionModal" tabindex="-1" role="dialog" aria-labelledby="rejectionModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="rejectionModalLabel">Rejection Reason</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- Dropdown for rejection reasons -->
                    <asp:DropDownList ID="ddlRejectionReason" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select reason" Value="" Disabled="True" />
                        <asp:ListItem Text="Item not needed" Value="ItemNotNeeded" />
                        <asp:ListItem Text="Donation exceeds our requirements" Value="ExceedsRequirements" />
                        <asp:ListItem Text="Duplicate donation request" Value="DuplicateRequest" />                       
                        <asp:ListItem Text="Others (Please Specify)" Value="Others" />
                    </asp:DropDownList>

                    <!-- Custom reason, hidden by default -->
                     <div class="form-group" id="otherReason" style="display: none;">
                         <label for="otherReason">Other Reason:</label>
                         <asp:TextBox ID="txtOtherReason" runat="server" CssClass="form-control" Placeholder="Enter your reason"></asp:TextBox>
                     </div>
                    <asp:HiddenField ID="hfDonationRequestId" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnSubmitRejection" runat="server" CssClass="btn btn-danger" OnClick="btnReject_Click"> Submit Rejection</asp:LinkButton>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


    <script>
        $(document).ready(function () {

            var rejectReason = $('#<%= ddlRejectionReason.ClientID %>');
            var otherReason = $('#otherReason');

            rejectReason.change(function () {
                if ($(this).val() === 'Others') {
                    otherReason.show();
                } else {
                    otherReason.hide();
                }
            });
        });

        function showSuccess(message) {
            Swal.fire({
                text: message,
                icon: 'success',
                confirmButtonText: 'OK',
                timer: 5000,
                timerProgressBar: true,
                willClose: () => {

                    window.location.href = 'OrgViewDonationRequest.aspx';
                }
            });
        }


        function showError(message) {
            Swal.fire({
                title: 'Error!',
                text: message,
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }

        function showRejectionModal(event, donationId) {
            event.preventDefault(); 
            $('#<%= hfDonationRequestId.ClientID %>').val(donationId);
            $('#rejectionModal').modal('show');
        }


        async function approveDonation(donationPublishId) {
            // show loading spinner or progress bar
            Swal.fire({
                title: 'Processing...',
                html: 'Saving data...',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            // fetch the pickup address
            getPickUpAddress(function (pickupLocation, pickupAddress) {
                if (!pickupLocation) {
                    Swal.fire('Error!', 'Failed to fetch pickup address.', 'error');
                    return;
                }

                // fetch the organization address
                fetchOrgAddress(donationPublishId, async function (orgLocation, orgAddress) {
                    if (!orgLocation) {
                        Swal.fire('Error!', 'Failed to fetch organization address.', 'error');
                        return;
                    }

                    // calculate distance between the two locations
                    calculateDistance(pickupLocation, orgLocation, async function (distance) {
                        if (distance !== null) {
                            
                            //console.log("Calculated Distance: " + distance);

                            try {
                                // send distance to backend 
                                await sendDistance(distance);

                                // close the loading spinner after the distance is successfully sent
                                Swal.close();

                                // show success message after processing
                                showSuccess('Donation request approved successfully! Pickup and organization addresses have been processed.');
                            } catch (error) {
                                Swal.fire('Error!', 'Failed to save distance.', 'error');
                            }
                        } else {
                            Swal.fire('Error!', 'Failed to calculate distance.', 'error');
                        }
                    });
                });
            });
        }


        function calculateDistance(pickupLocation, orgLocation, callback) {
            var service = new google.maps.DistanceMatrixService();
            service.getDistanceMatrix({
                origins: [pickupLocation],
                destinations: [orgLocation],
                travelMode: 'DRIVING',
                unitSystem: google.maps.UnitSystem.METRIC  // use metric units (meters)
            }, function (response, status) {
                if (status === 'OK') {
                    var distanceInMeters = response.rows[0].elements[0].distance.value;  // distance in meters
                    var distanceInKilometers = distanceInMeters / 1000;  // convert to kilometers

                    // 1 decimal place
                    var distanceRounded = distanceInKilometers.toFixed(1);  

                    console.log("Rounded Distance (in kilometers): " + distanceRounded);  

                    callback(parseFloat(distanceRounded));  
                } else {
                    console.error('Error calculating distance:', status);
                    callback(null);
                }
            });
        }


        function getPickUpAddress(callback) {
            $.ajax({
                type: "POST",
                url: "OrgReviewDonationRequest.aspx/GetPickupAddress",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var backendAddress = response.d;
                    console.log("Address from session: ", backendAddress);

                    if (!backendAddress) {
                        console.error('No address found in session');
                        callback(null, null);
                        return;
                    }

                    var geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ 'address': backendAddress }, function (results, status) {
                        if (status === 'OK') {
                            var location = results[0].geometry.location;
                            callback(location, backendAddress); 
                        } else {
                            console.error('Geocode for pickup address failed: ' + status);
                            callback(null, null);
                        }
                    });
                },
                error: function (error) {
                    console.error("Error fetching address from backend:", error);
                    callback(null, null);
                }
            });
        }


        // fetch and geocode organization address based on donationPublishId
        function fetchOrgAddress(donationPublishId, callback) {
            $.ajax({
                type: 'POST',
                url: 'OrgReviewDonationRequest.aspx/GetOrganizationAddress',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ donationPublishId: donationPublishId }),
                success: function (response) {
                    var address = response.d;
                    if (address) {
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
                error: function (error) {
                    console.error('Error fetching organization address:', error);
                    callback(null, null);
                }
            });
        }

        function sendDistance(distance) {
            fetch('/OrgReviewDonationRequest.aspx/ReceiveDistance', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ distance: distance })
            })
                .then(response => response.json())
                .then(data => {
                    console.log('Server response:', data);
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        }

        // function to remove the donation item from the table
        function confirmRemove(donationId, category, item, expiryDate, quantity) {
            Swal.fire({
                title: 'Are you sure to remove?',
                text: "You won't be able to undo this! Donor will be informed about this.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, remove it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    // perform the AJAX call to remove the item from the database
                    $.ajax({
                        type: "POST",
                        url: "OrgReviewDonationRequest.aspx/RemoveItem",  
                        data: JSON.stringify({ donationId: donationId, category: category, item: item, expiryDate: expiryDate, quantityWithSameExpiryDate: quantity }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",  
                        success: function (response) {
                            if (response.d) {  
                                // remove the row from the table
                                $("#" + item).remove();

                                Swal.fire(
                                    'Removed!',
                                    'The item has been removed.',
                                    'success'
                                ).then(() => {
                                    // page reload after successful deletion
                                    window.location.reload();  
                                });
                            } else {
                                Swal.fire(
                                    'Alert!',
                                    'This is the only item left for this donation. Instead of removing it, you may reject this donation request and the donor will be informed about that.',
                                    'warning'
                                );
                            }
                        },
                        error: function () {
                            Swal.fire(
                                'Error!',
                                'There was a problem communicating with the server.',
                                'error'
                            );
                        }
                    });
                }
            });
        }

        // load the list of donation items
        function loadDonationItems(donationId) {
            $.ajax({
                type: "POST",
                url: "OrgReviewDonationRequest.aspx/LoadDonationItems",
                data: JSON.stringify({ donationId: donationId }),  
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                    $("#categoryDetailsTable").html(response.d);  
                },
                error: function () {
                    Swal.fire(
                        'Error!',
                        'There was a problem updating the items.',
                        'error'
                    );
                }
            });
        }



    </script>

    
</asp:Content>

