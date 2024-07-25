<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="DonorConnect.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="/Content/SignUp2.css" rel="stylesheet" type="text/css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
</head>
<body style="background-color: #bfdae2">
    <form id="form1" runat="server">
        <div class="container" style="width: 1300px">
            <div class="card" style="border-radius: 1rem;">
                <div class="row no-gutters flex-column flex-md-row">
                    <div class="col-12 col-md-6 d-flex align-items-center">
                        <div class="card-body p-4 p-lg-5 text-black">
                            <div class="d-flex align-items-center mb-3 pb-1"> 
                                <span class="h1 fw-bold mb-0">DonorConnect</span>
                            </div>
                            <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Create your account</h5>
                            <div class="role-selection mb-4">
                                <div class="role-box" id="donorBox" onclick="selectRole('donor')">
                                    <i class="fas fa-hand-holding-heart"></i>
                                    <div>Donor</div>
                                </div>
                                <div class="role-box" id="organizationBox" onclick="selectRole('organization')">
                                    <i class="fas fa-building"></i>
                                    <div>Organization</div>
                                </div>
                                <div class="role-box" id="riderBox" onclick="selectRole('rider')">
                                    <i class="fas fa-car"></i>
                                    <div>Delivery Rider</div>
                                </div>
                            </div>

                            <!-- Hidden input to store the selected role -->
                           <asp:HiddenField ID="selectedRole" runat="server" />
                            
                            <div id="donorDetails" class="role-details" runat="server">
                                <!-- Donor Fields -->
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="donorName" CssClass="form-control form-control-lg" runat="server" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="donorName" runat="server">Full Name*</asp:Label>
                                    <asp:label ID="lblDonorName" runat="server" CssClass="text-danger"/>
                                </div>
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="donorUsername" CssClass="form-control form-control-lg" runat="server" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="donorUsername" runat="server">Username*</asp:Label>
                                    <asp:label ID="lblDonorUsername" runat="server" CssClass="text-danger"/>
                                </div>
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="donorEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="donorEmail" runat="server">Email address*</asp:Label>
                                    <asp:label ID="lblDonorEmail" runat="server" CssClass="text-danger"/>
                                </div>
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="donorContactNumber" CssClass="form-control form-control-lg" runat="server" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="donorContactNumber" runat="server">Contact Number*</asp:Label>
                                    <asp:label ID="lblDonorContactNumber" runat="server" CssClass="text-danger"/>
                                </div>
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="donorPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="donorPassword" runat="server">Password*</asp:Label>
                                    <asp:label ID="lblDonorPassword" runat="server" CssClass="text-danger"/>
                                </div>
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="donorConfirmPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="donorConfirmPassword" runat="server">Confirm Password*</asp:Label>
                                    <asp:label ID="lblDonorConfirmPassword" runat="server" CssClass="text-danger"/>
                                </div>
                            </div>


                            <div id="organizationDetails" class="role-details" runat="server">
                                <!-- Organization Fields -->
                                <div class="section">
                                    <div class="section-label">Organization Information</div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="orgName" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="orgName" runat="server">Organization Name*</asp:Label>
                                        <asp:label ID="lblOrgName" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="orgEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="orgEmail" runat="server">Organization Email Address*</asp:Label>
                                        <asp:label ID="lblOrgEmail" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="orgContactNumber" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="orgContactNumber" runat="server">Organization Contact Number*</asp:Label>
                                        <asp:label ID="lblOrgContactNumber" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="orgAddress" CssClass="form-control form-control-lg" runat="server"/>
                                        <asp:Label CssClass="form-label" AssociatedControlID="orgAddress" runat="server">Organization Address*</asp:Label>
                                        <asp:label ID="lblOrgAddress" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:DropDownList ID="orgRegion" CssClass="form-control form-control-lg" runat="server" style="font-size: 1rem;">
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
                                        <asp:Label CssClass="form-label" AssociatedControlID="orgRegion" runat="server">Region in Malaysia*</asp:Label>
                                        <asp:label ID="lblOrgRegion" runat="server" CssClass="text-danger"/>
                                    </div>
                                </div>
                                <div class="section">
                                    <div class="section-label">Person-in-Charge Information</div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="picName" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="picName" runat="server">Person-in-Charge Name*</asp:Label>
                                        <asp:label ID="lblPicName" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="picEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="picEmail" runat="server">Person-in-Charge Email*</asp:Label>
                                        <asp:label ID="lblPicEmail" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="picNumber" CssClass="form-control form-control-lg" runat="server"/>
                                        <asp:Label CssClass="form-label" AssociatedControlID="picNumber" runat="server">Person-in-Charge Contact Number*</asp:Label>
                                        <asp:label ID="lblPicNumber" runat="server" CssClass="text-danger"/>
                                    </div>
                                </div>
                                <div class="section">
                                    <div class="section-label">Business License</div>
                                    <div class="form-outline mb-4">
                                        <div class="input-with-icon d-flex align-items-center">
                                            <asp:FileUpload ID="orgLicense" CssClass="form-control form-control-lg" runat="server" AllowMultiple="true" />
                                            <div id="showSampleBusinessLicense" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#sampleBusinessLicenseModal">
                                                <i class="fas fa-question-circle fa-lg" style="color: #6E747A;"></i>
                                            </div>
                                        </div>
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderCarLicense" runat="server">Upload Business License (SSM Borang E)*</asp:Label>
                                        
                                    </div>
                                        <div class="guidelines">
                                            <p>Accepted formats: .jpg, .jpeg, .png, .pdf</p>
                                        </div>
                                    <asp:label ID="lblOrgLicense" runat="server" CssClass="text-danger"/>
                                    <div></div>
                                    <asp:label ID="lblImgTypeOrgLicense" runat="server" CssClass="text-danger"/>
                                    </div>
                             
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="orgPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="orgPassword" runat="server">Password*</asp:Label>
                                    <asp:label ID="lblOrgPassword" runat="server" CssClass="text-danger"/>
                                </div>
                                <div class="form-outline mb-4">
                                    <asp:TextBox ID="orgConfirmPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                    <asp:Label CssClass="form-label" AssociatedControlID="orgConfirmPassword" runat="server">Confirm Password*</asp:Label>
                                    <asp:label ID="lblOrgConfirmPassword" runat="server" CssClass="text-danger"/>
                                </div>
                            </div>

                            <div id="riderDetails" class="role-details" runat="server">
                                <!-- Rider Fields -->
                                <div class="section">
                                    <div class="section-label">Personal Information</div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="riderName" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderName" runat="server">Full Name*</asp:Label>
                                        <asp:label ID="lblRiderName" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="riderUsername" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderUsername" runat="server">Username*</asp:Label>
                                        <asp:label ID="lblRiderUsername" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="riderEmail" CssClass="form-control form-control-lg" runat="server" TextMode="Email" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderEmail" runat="server">Email Address*</asp:Label>
                                        <asp:label ID="lblRiderEmail" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="riderContactNumber" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderContactNumber" runat="server">Contact Number*</asp:Label>
                                        <asp:label ID="lblRiderContactNumber" runat="server" CssClass="text-danger"/>
                                    </div>
                                </div>
                                <div class="section">
                                    <div class="section-label">Vehicle and License Information</div>
                                    <div class="form-outline mb-4">
                                        <asp:DropDownList ID="vehicleType" CssClass="form-control form-control-lg" runat="server" Style="font-size: 1rem;">
                                            <asp:ListItem Text="Select Vehicle Type" Value="" Disabled="true" Selected="true"></asp:ListItem>
                                            <asp:ListItem Text="Car" Value="Car"></asp:ListItem>
                                            <asp:ListItem Text="Truck" Value="Truck"></asp:ListItem>
                                            <asp:ListItem Text="Van" Value="Van"></asp:ListItem>
                                            <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label CssClass="form-label" AssociatedControlID="vehicleType" runat="server">Vehicle Type*</asp:Label>
                                        <div class="guidelines">
                                            <p>Motorcycles and bicycles are not allowed for item delivery.</p>
                                        </div>
                                        <asp:label ID="lblVehicleType" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4"> 
                                        <asp:TextBox ID="vehiclePlateNo" CssClass="form-control form-control-lg" runat="server" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="vehiclePlateNo" runat="server">Vehicle Plate Number*</asp:Label>
                                        <asp:label ID="lblVehiclePlateNo" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <div class="input-with-icon d-flex align-items-center">
                                            <asp:FileUpload ID="riderCarLicense" CssClass="form-control form-control-lg" runat="server" />
                                            <div id="showSampleImage" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#sampleImageModal">
                                                <i class="fas fa-question-circle fa-lg" style="color: #6E747A;"></i>
                                            </div>
                                        </div>
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderCarLicense" runat="server">Upload Driving License*</asp:Label>
                                        <div class="guidelines">
                                            <p>Accepted formats: .jpg, .jpeg, .png (only image file)</p> 
                                        </div>
                                        <asp:label ID="lblRiderCarLicense" runat="server" CssClass="text-danger"/>
                                        <asp:label ID="lblImgTypeCarLicense" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <div class="input-with-icon d-flex align-items-center">
                                            <asp:FileUpload ID="riderFacePhoto" CssClass="form-control form-control-lg" runat="server" />
                                            <div id="showSampleFacePhoto" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#sampleImageModal2">
                                                <i class="fas fa-question-circle fa-lg" style="color: #6E747A;"></i>
                                            </div>
                                        </div>
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderFacePhoto" runat="server">Upload Face Photo*</asp:Label>
                                        <div class="guidelines">
                                            <p>Accepted format: .jpg, .jpeg, .png (only image file)</p>
                                        </div>
                                        <asp:label ID="lblRiderFacePhoto" runat="server" CssClass="text-danger"/>
                                        <asp:label ID="lblImgTypeFacePhoto" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="riderPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderPassword" runat="server">Password*</asp:Label>
                                        <asp:label ID="lblRiderPassword" runat="server" CssClass="text-danger"/>
                                    </div>
                                    <div class="form-outline mb-4">
                                        <asp:TextBox ID="riderConfirmPassword" CssClass="form-control form-control-lg" runat="server" TextMode="Password" />
                                        <asp:Label CssClass="form-label" AssociatedControlID="riderConfirmPassword" runat="server">Confirm Password*</asp:Label>
                                        <asp:label ID="lblRiderConfirmPassword" runat="server" CssClass="text-danger"/>
                                    </div>
                                </div>
                            </div>

                            <div class="pt-1 mb-4">
                                <asp:Button ID="signUpButton" type="submit" name="submit" CssClass="btn btn-dark btn-lg btn-block" Text="Sign Up" runat="server" onClick="btnRegister_Click"/>
                            </div>
                            <p class="mb-5 pb-lg-2" style="color: #393f81;">Already have an account? <a href="Login.aspx" style="color: #393f81;">Login here</a></p>
                            <a href="#!" class="small text-muted">Terms of use.</a>
                            <a href="#!" class="small text-muted">Privacy policy</a>
                        </div>
                    </div>
                     <div class="col-12 col-md-6 image-container">
                        <img src="/Image/signup_pic.jpg" alt="sign up form" class="img-fluid image-side" style="height: 100%; width: 100%;" />
                    </div>
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
    </form>

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script>
        function selectRole(role) {
            // Get all role boxes
            var roleBoxes = document.getElementsByClassName('role-box');

            // Remove the 'selected' class from all role boxes
            for (var i = 0; i < roleBoxes.length; i++) {
                roleBoxes[i].classList.remove('selected');
            }

            // Add the 'selected' class to the clicked role box
            document.getElementById(role + 'Box').classList.add('selected');

            // Set the selected role value
            document.getElementById('<%= selectedRole.ClientID %>').value = role;

            document.getElementById('donorDetails').style.display = role === 'donor' ? 'block' : 'none';
            document.getElementById('organizationDetails').style.display = role === 'organization' ? 'block' : 'none';
            document.getElementById('riderDetails').style.display = role === 'rider' ? 'block' : 'none';
             
         }

  

        
    </script>

</body>
</html>



