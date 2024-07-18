<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="DonorConnect.SignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="/Content/SignUp.css" rel="stylesheet" type="text/css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
</head>
<body style="background-color: #bfdae2">
   <form id="form1" runat="server">
    <div class="container" style="width: 1160px;">
        <div class="card" style="border-radius: 1rem;">
            <div class="row no-gutters flex-column flex-md-row">
                <div class="col-12 col-md-6 d-flex align-items-center">
                    <div class="card-body p-4 p-lg-5 text-black">
                        <div class="d-flex align-items-center mb-3 pb-1">
                            <i class="fas fa-cubes fa-2x me-3" style="color: #ff6219;"></i>
                            <span class="h1 fw-bold mb-0">DonorConnect</span>
                        </div>
                        <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Create your account</h5>
                        
                        <div class="role-selection mb-4">
                            <div class="role-box" id="donorBox" onclick="showDetails('donor')">
                                <i class="fas fa-hand-holding-heart"></i>
                                <div>Donor</div>
                            </div>
                            <div class="role-box" id="organizationBox" onclick="showDetails('organization')">
                                <i class="fas fa-building"></i>
                                <div>Organization</div>
                            </div>
                            <div class="role-box" id="riderBox" onclick="showDetails('rider')">
                                <i class="fas fa-car"></i>
                                <div>Delivery Rider</div>
                            </div>
                        </div>

                        <div id="donorDetails" class="role-details">
                            <!-- Donor Fields -->
                            <div class="form-outline mb-4">
                                <input type="text" id="donorName" class="form-control form-control-lg" />
                                <label class="form-label" for="donorName">Full Name</label>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="text" id="donorUsername" class="form-control form-control-lg" required="required"/>
                                <label class="form-label" for="donorUsername">Username</label>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="email" id="donorEmail" class="form-control form-control-lg" required="required" />
                                <label class="form-label" for="donorEmail">Email address</label>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="password" id="donorPassword" class="form-control form-control-lg" required="required"/>
                                <label class="form-label" for="donorPassword">Password</label>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="password" id="donorConfirmPassword" class="form-control form-control-lg" required="required" />
                                <label class="form-label" for="donorConfirmPassword">Confirm Password</label>
                            </div>
                        </div>

                        <div id="organizationDetails" class="role-details">
                            <!-- Organization Fields -->
                            <div class="section">
                                <div class="section-label">Organization Information</div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="orgName" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="orgName">Organization Name</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="email" id="orgEmail" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="orgEmail">Organization Email Address</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="orgContactNumber" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="orgContactNumber">Organization Contact Number</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="orgAddress" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="orgAddress">Organization Address</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="orgRegion" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="orgRegion">Region in Malaysia</label>
                                </div>
                            </div>
                            <div class="section">
                                <div class="section-label">Person-in-Charge Information</div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="picName" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="picName">Person-in-Charge Name</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="email" id="picEmail" class="form-control form-control-lg" />
                                    <label class="form-label" for="picEmail">Person-in-Charge Email</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="picNumber" class="form-control form-control-lg" />
                                    <label class="form-label" for="picNumber">Person-in-Charge Contact Number</label>
                                </div>
                            </div>
                            <div class="section">
                                <div class="section-label">Business License</div>
                                <div class="form-outline mb-4">
                                    <input type="file" id="orgLicense" class="form-control form-control-lg" required="required" multiple="multiple" />
                                    <label class="form-label" for="orgLicense">Upload Business License</label>
                                    <div class="guidelines">
                                        <p>Accepted formats: .jpg, .jpeg, .png, .pdf</p>
                                    </div>
                                </div>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="password" id="orgPassword" class="form-control form-control-lg" required="required" />
                                <label class="form-label" for="orgPassword">Password</label>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="password" id="orgConfirmPassword" class="form-control form-control-lg" required="required" />
                                <label class="form-label" for="orgConfirmPassword">Confirm Password</label>
                            </div>
                        </div>

                        <div id="riderDetails" class="role-details">
                            <!-- Rider Fields -->
                            <div class="section">
                                <div class="section-label">Personal Information</div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="riderName" class="form-control form-control-lg" required="required"/>
                                    <label class="form-label" for="riderName">Full Name</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="riderUsername" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="riderUsername">Username</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="email" id="riderEmail" class="form-control form-control-lg" required="required"/>
                                    <label class="form-label" for="riderEmail">Email Address</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="riderContactNumber" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="riderContactNumber">Contact Number</label>
                                </div>
                            </div>
                            <div class="section">
                                <div class="section-label">Vehicle and License Information</div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="vehicleType" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="vehicleType">Vehicle Type</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="text" id="vehiclePlateNo" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="vehiclePlateNo">Vehicle Plate Number</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="file" id="riderCarLicense" class="form-control form-control-lg" required="required" multiple="multiple" />
                                    <label class="form-label" for="riderCarLicense">Upload Driving License</label>
                                    <div class="guidelines">
                                        <p>Accepted formats: .jpg, .jpeg, .png, .pdf</p>
                                    </div>
                                </div>
                                <div id="showSampleIcon" style="cursor: pointer;">
                                    <i class="fas fa-image fa-lg"></i>
                                    
                                </div>
                                
                                <div id="sampleImageContainer" style="display: none;">
                                    <p>Sample Valid Drving License:</p>
                                    <img src="/Image/driving_license.jpg" alt="Sample Image" style="max-width: 100%; height: auto;"/>
                                    <p>Follow this format to upload your file. You may upload separate files for front and back of the license.</p>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="file" id="riderFacePhoto" class="form-control form-control-lg" required="required"/>
                                    <label class="form-label" for="riderFacePhoto">Upload Face Photo</label>
                                    <div class="guidelines">
                                        <p>Accepted formats: passport size</p>
                                    </div>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="password" id="riderPassword" class="form-control form-control-lg" required="required"/>
                                    <label class="form-label" for="riderPassword">Password</label>
                                </div>
                                <div class="form-outline mb-4">
                                    <input type="password" id="riderConfirmPassword" class="form-control form-control-lg" required="required" />
                                    <label class="form-label" for="riderConfirmPassword">Confirm Password</label>
                                </div>
                            </div>
                        </div>

                        <div class="pt-1 mb-4">
                            <button class="btn btn-dark btn-lg btn-block" type="button">Sign Up</button>
                        </div>
                        <p class="mb-5 pb-lg-2" style="color: #393f81; margin-top: 15px;">Already have an account? <a href="Login.aspx" style="color: #393f81;">Login here</a></p>
                        <a href="#" class="small text-muted">Terms of use.</a>
                        <a href="#" class="small text-muted">Privacy policy</a>
                    </div>
                </div>

                <div id="imageContainer" class="col-12 col-md-6 d-flex align-items-center" style="display: block;">
                    <img src="/Image/signup_pic.jpg" class="img-fluid" alt="Sign Up Image" style="height: 100%; width: 100%" />
                </div>
            </div>
        </div>
    </div>
</form>


    <script>
        function showDetails(role) {
            const roles = ['donor', 'organization', 'rider'];
            roles.forEach(r => {
                document.getElementById(r + 'Details').style.display = (r === role) ? 'block' : 'none';
            });
        }
        document.getElementById('showSampleIcon').addEventListener('click', function () {
           
            var sampleImageContainer = document.getElementById('sampleImageContainer');
            if (sampleImageContainer.style.display === 'none') {
                sampleImageContainer.style.display = 'block';
            } else {
                sampleImageContainer.style.display = 'none';
            }
        });
    </script>
</body>
</html>


