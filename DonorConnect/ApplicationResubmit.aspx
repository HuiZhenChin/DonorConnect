<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApplicationResubmit.aspx.cs" Inherits="DonorConnect.ApplicationResubmit" EnableViewState="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
     <meta name="viewport" content="width=device-width, initial-scale=1.0" />
     <title>Resubmit Application</title>
     <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script type="text/javascript">

            function validateImageUpload(fileInput) {
                var allowedExtensions = ['jpg', 'jpeg', 'png'];
                var errorMsg = document.getElementById('imageUploadError');
                var files = fileInput.files;
                var isValid = true;

                if (files.length > 1) {
                    isValid = false;
                } else {
                    for (var i = 0; i < files.length; i++) {
                        var fileExtension = files[i].name.split('.').pop().toLowerCase();
                        if (!allowedExtensions.includes(fileExtension)) {
                            isValid = false;
                            break;
                        }
                    }
                }

                if (!isValid) {
                    errorMsg.style.display = 'block';
                    fileInput.value = ''; // clear the file input
                } else {
                    errorMsg.style.display = 'none';

                }
            }

            function validateAttachmentUpload(fileInput) {
                var allowedExtensions = ['pdf', 'png', 'jpg', 'jpeg'];
                var errorMsg = document.getElementById('attchUploadError');
                var files = fileInput.files;
                var isValid = true;

                for (var i = 0; i < files.length; i++) {
                    var fileExtension = files[i].name.split('.').pop().toLowerCase();
                    if (!allowedExtensions.includes(fileExtension)) {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid) {
                    errorMsg.style.display = 'block';
                    fileInput.value = ''; // clear the file input
                } else {
                    errorMsg.style.display = 'none';
                }
            }


            function validateAttachmentUpload2(fileInput) {
                var allowedExtensions = ['pdf', 'png', 'jpg', 'jpeg'];
                var errorMsg = document.getElementById('fileUploadError2');
                var files = fileInput.files;
                var isValid = true;

                for (var i = 0; i < files.length; i++) {
                    var fileExtension = files[i].name.split('.').pop().toLowerCase();
                    if (!allowedExtensions.includes(fileExtension)) {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid) {
                    errorMsg.style.display = 'block';
                    fileInput.value = ''; // clear the file input
                } else {
                    errorMsg.style.display = 'none';
                }
            }

             function showSuccess(message) {
                 Swal.fire({
                     title: 'Success!',
                     text: message,
                     icon: 'success',
                     timer: 6000,
                     timerProgressBar: true,
                     showConfirmButton: false,
                     willClose: () => {
                         window.close();
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

             function showInfo(message) {
                 Swal.fire({
                     title: 'Already Submitted!',
                     text: message,
                     icon: 'info',
                     timer: 6000, 
                     timerProgressBar: true,
                     showConfirmButton: false, 
                     willClose: () => {
                         window.close(); 
                     }
                 });
             }


            function resetForm() {
                // reset all form fields to their original states
                document.forms[0].reset();
            }


     </script>
    <style>
        .category-row {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
        }
        .category-checkbox {
            display: flex;
            align-items: center;
        }
        .specific-items-input {
            margin-left: 10px;
            display: none;
        }
        input[type="checkbox"], input[type="radio"] {
           
            transform: scale(1.0);        
            margin: 5px;
            margin-left: 15px;
        }
    </style>
</head>

<body>

    <form id="form1" runat="server">
        <div class="container mt-4" style="padding-bottom: 20px;">
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">Resubmit Application</h4>
                </div>

                <asp:Label CssClass="text-danger" runat="server" Style="padding-bottom: 10px;" Text="Please make the changes according to the Rejected Reason provided." />
                <div class="card-body">

                    <div id="orgDetails" class="role-details" runat="server" style="display: none;">
                        <div class="section">
                            <div class="section-label">Organization Information</div>
                            <div class="form-outline mb-4">
                                <asp:Label CssClass="form-label" AssociatedControlID="orgName" runat="server">Organization Name*</asp:Label>
                                <asp:TextBox ID="orgName" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblOrgName" runat="server" CssClass="text-danger"/>

                            </div>
                            <div class="form-outline mb-4">
                                <asp:Label CssClass="form-label" AssociatedControlID="orgEmail" runat="server">Organization Email Address*</asp:Label>
                                <asp:TextBox ID="orgEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email"/>
                                <asp:label ID="lblOrgEmail" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="orgContactNumber" runat="server">Organization Contact Number*</asp:Label>
                                <asp:TextBox ID="orgContactNumber" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblOrgContactNumber" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="orgAddress" runat="server">Organization Address*</asp:Label>
                                <asp:TextBox ID="orgAddress" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblOrgAddress" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">
                                <asp:Label CssClass="form-label" AssociatedControlID="orgRegion" runat="server">Region in Malaysia*</asp:Label>
                                <asp:DropDownList ID="orgRegion" CssClass="form-control form-control-lg" runat="server" Style="font-size: 1rem;">
                                    <asp:ListItem Text="Select Region in Malaysia" Value="" Disabled="true" Selected="true" />
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

                               <asp:label ID="lblOrgRegion" runat="server" CssClass="text-danger"/>
                            </div>
                        </div>
                        <div class="section">
                            <div class="section-label">Person-in-Charge Information</div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="picName" runat="server">Person-in-Charge Name*</asp:Label>
                                <asp:TextBox ID="picName" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblPicName" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="picEmail" runat="server">Person-in-Charge Email*</asp:Label>
                                <asp:TextBox ID="picEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email" />
                                <asp:label ID="lblPicEmail" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="picNumber" runat="server">Person-in-Charge Contact Number*</asp:Label>
                                <asp:TextBox ID="picNumber" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblPicNumber" runat="server" CssClass="text-danger"/>
                            </div>
                        </div>
                        <div class="section">
                            <div class="section-label">Business License (SSM Borang E)*</div>
                            
                            <div class="form-outline mb-4">
                                <div class="input-with-icon d-flex align-items-center">
                                    <asp:FileUpload ID="orgLicense" CssClass="form-control form-control-lg" runat="server" AllowMultiple="true" OnChange="validateAttachmentUpload(this)" />
                                    <div id="showSampleBusinessLicense" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#sampleBusinessLicenseModal">
                                        <i class="fas fa-question-circle fa-lg" style="color: #6E747A;"></i>
                                    </div>
                                    
                                </div>
                                <small class="form-text text-muted">
                                    <em>Accepted Formats: .jpg, .jpeg, .png, .pdf Maximum 5 attachments.</em>
                                </small>
                                <span id="attchUploadError" class="text-danger" style="display: none;">You have uploaded files in not acceptable format or more than 5 attachments. Note that you can upload maximum 5 attachments in .jpg, .jpeg, .png or .pdf formats.</span>
                                <asp:Label CssClass="form-label" AssociatedControlID="orgLicense" runat="server" style="padding-top: 10px; font-style:italic;">Previous Submitted Attachment</asp:Label>
                                <asp:Literal ID="imagesContainer3" runat="server"></asp:Literal>


                            </div>


                        </div>

                        <div class="form-outline mb-4">

                            <asp:Label CssClass="form-label" AssociatedControlID="orgPassword" runat="server">Password*</asp:Label>
                            <asp:TextBox ID="orgPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                            <asp:label ID="lblOrgPassword" runat="server" CssClass="text-danger"/>
                        </div>
                        <div class="form-outline mb-4">

                            <asp:Label CssClass="form-label" AssociatedControlID="orgConfirmPassword" runat="server">Confirm Password*</asp:Label>
                            <asp:TextBox ID="orgConfirmPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                            <asp:label ID="lblOrgConfirmPassword" runat="server" CssClass="text-danger"/>
                        </div>
                        <div class="form-outline mb-4">

                            <asp:Label CssClass="form-label" AssociatedControlID="orgReason" runat="server">Rejected Reason</asp:Label>
                            <asp:TextBox ID="orgReason" CssClass="form-control form-control-lg" runat="server" ReadOnly="true" />

                        </div>
                    </div>

                    <div id="riderDetails" class="role-details" runat="server" style="display: none;">
                        <!-- Rider Fields -->
                        <div class="section">
                            <div class="section-label">Personal Information</div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderName" runat="server">Full Name*</asp:Label>
                                <asp:TextBox ID="riderName" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblRiderName" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderUsername" runat="server">Username*</asp:Label>
                                <asp:TextBox ID="riderUsername" CssClass="form-control form-control-lg" runat="server" ReadOnly="true" Alt="Username is not allowed to modify."/>
                                <asp:label ID="lblRiderUsername" runat="server" CssClass="text-danger"/>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderEmail" runat="server">Email Address*</asp:Label>
                                <asp:TextBox ID="riderEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email" />
                                <asp:label ID="lblRiderEmail" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderContactNumber" runat="server">Contact Number*</asp:Label>
                                <asp:TextBox ID="riderContactNumber" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblRiderContactNumber" runat="server" CssClass="text-danger"/>
                            </div>
                        </div>
                        <div class="section">
                            <div class="section-label">Vehicle and License Information</div>
                            <asp:Label CssClass="form-label" AssociatedControlID="vehicleType" runat="server">Vehicle Type*</asp:Label>
                            <div class="form-outline mb-4">
                                <asp:DropDownList ID="vehicleType" CssClass="form-control form-control-lg" runat="server" Style="font-size: 1rem;">
                                    <asp:ListItem Text="Select Vehicle Type" Value="" Disabled="true" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="Car" Value="Car"></asp:ListItem>
                                    <asp:ListItem Text="Truck" Value="Truck"></asp:ListItem>
                                    <asp:ListItem Text="Van" Value="Van"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                </asp:DropDownList>

                                <div class="guidelines">
                                    <p>Motorcycles and bicycles are not allowed for item delivery.</p>
                                </div>
                                 <asp:label ID="lblVehicleType" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="vehiclePlateNo" runat="server">Vehicle Plate Number*</asp:Label>
                                <asp:TextBox ID="vehiclePlateNo" CssClass="form-control form-control-lg" runat="server" />
                                <asp:label ID="lblVehiclePlateNo" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">
                                <asp:Label CssClass="form-label" AssociatedControlID="riderCarLicense" runat="server">Upload Driving License*</asp:Label>
                                
                                <div class="input-with-icon d-flex align-items-center">
                                    <asp:FileUpload ID="riderCarLicense" CssClass="form-control form-control-lg" runat="server" AllowMultiple="true" OnChange="validateAttachmentUpload2(this)" />
                                    <div id="showSampleImage" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#sampleImageModal">
                                        <i class="fas fa-question-circle fa-lg" style="color: #6E747A;"></i>
                                    </div>
                                </div>
                                <small class="form-text text-muted">
                                    <em>Accepted Formats: .jpg, .jpeg, .png, .pdf Maximum 5 attachments.</em>
                                </small>
                                <span id="fileUploadError2" class="text-danger" style="display: none;">You have uploaded files in not acceptable format or more than 5 attachments. Note that you can upload maximum 5 attachments in .jpg, .jpeg, .png or .pdf formats.</span>
                                <asp:Label CssClass="form-label" AssociatedControlID="riderCarLicense" runat="server" Style="padding-top: 10px; font-style: italic;">Previous Submitted Attachment</asp:Label>
                                <asp:Literal ID="imagesContainer" runat="server"></asp:Literal>


                            </div>
                            <div class="form-outline mb-4">
                                <asp:Label CssClass="form-label" AssociatedControlID="riderFacePhoto" runat="server">Upload Face Photo*</asp:Label>
                                
                                <div class="input-with-icon d-flex align-items-center">
                                    <asp:FileUpload ID="riderFacePhoto" CssClass="form-control form-control-lg" runat="server" OnChange="validateImageUpload(this)" />
                                    <div id="showSampleFacePhoto" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#sampleImageModal2">
                                        <i class="fas fa-question-circle fa-lg" style="color: #6E747A;"></i>
                                    </div>
                                </div>
                                <small class="form-text text-muted">
                                    <em>Accepted Formats: .jpg, .jpeg, .png. Maximum 1 image.</em>
                                </small>
                                <span id="imageUploadError" class="text-danger" style="display: none;">You have uploaded photo in not acceptable format or more than 1 photo. Note that you can upload one image in .jpg, .jpeg, or .png formats.</span>
                                <asp:Label CssClass="form-label" AssociatedControlID="riderCarLicense" runat="server" Style="padding-top: 10px; font-style: italic;">Previous Submitted Attachment</asp:Label>
                                <asp:Literal ID="imagesContainer2" runat="server"></asp:Literal>

                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderPassword" runat="server">Password*</asp:Label>
                                <asp:TextBox ID="riderPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                <asp:label ID="lblRiderPassword" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderConfirmPassword" runat="server">Confirm Password*</asp:Label>
                                <asp:TextBox ID="riderConfirmPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                <asp:label ID="lblRiderConfirmPassword" runat="server" CssClass="text-danger"/>
                            </div>
                            <div class="form-outline mb-4">

                                <asp:Label CssClass="form-label" AssociatedControlID="riderReason" runat="server">Rejected Reason</asp:Label>
                                <asp:TextBox ID="riderReason" CssClass="form-control form-control-lg" runat="server" ReadOnly="true" />

                            </div>
                        </div>

                    </div>
                </div>

                <div class="row" style="padding-bottom: 20px; padding-right: 10px;">
                    <div class="col-md-12 text-right">
                        <asp:Button ID="btnResubmit" type="submit" runat="server" Text="Resubmit" CssClass="btn btn-info btn-lg" Style="background-color: seagreen;" OnClick="btnResubmit_Click"/>
                        <asp:Button ID="btnCancel" runat="server" Style="margin-left: 10px;" Text="Cancel" CssClass="btn btn-danger btn-lg" OnClientClick="resetForm(this)"/>
                    </div>
                </div>
            </div>

                <!-- Modal/ Dialog Box -->
                <div class="modal fade" id="sampleImageModal" tabindex="-1" role="dialog" aria-labelledby="sampleImageModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="sampleImageModalLabel">Sample Valid Driving License</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <img src="/Image/driving_license.jpg" alt="Sample Image" style="max-width: 100%; height: auto;" />
                                <p>Follow this format to upload your file. You may upload separate files for front and back of the license.</p>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="modal fade" id="sampleImageModal2" tabindex="-1" role="dialog" aria-labelledby="sampleImageModalLabel2" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="sampleImageModalLabel2">Sample Valid and Invalid Face Photos</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <div class="valid-sample">
                                    <i class="fas fa-check-circle fa-lg" style="color: green;"></i>
                                    <img src="/Image/valid_face_photo.jpg" alt="Valid Sample Image" style="max-width: 80%; height: auto;" />
                                    <p>Sample Valid Face Photo</p>
                                </div>
                                <div class="invalid-sample">
                                    <i class="fas fa-times-circle fa-lg" style="color: red;"></i>
                                    <img src="/Image/invalid_face_photo.jpg" alt="Invalid Sample Image" style="max-width: 100%; height: auto;" />
                                    <p>Example Invalid Face Photo </p>
                                </div>

                                <p>Follow the valid example to upload one face photo of you. Make sure the photo is clear, with your face fully visible.</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal fade" id="sampleBusinessLicenseModal" tabindex="-1" role="dialog" aria-labelledby="sampleBusinessLicenseModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="sampleBusinessLicenseModalLabel">Sample Business License</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <img src="/Image/business_license.jpg" alt="Sample Image" style="max-width: 100%; height: auto;" />
                                <p>Follow this format to upload your file. Ensure the business name and registration number are clearly visible.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>


</body>
</html>
