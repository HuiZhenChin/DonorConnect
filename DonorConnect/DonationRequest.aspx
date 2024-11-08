<%@ Page Title="Donations" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DonationRequest.aspx.cs" Inherits="DonorConnect.DonationRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donation</title>
    <link rel="stylesheet" href="/Content/DonationRequest.css">
    <script src="/Scripts/DonationRequest.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Itim&family=Istok+Web:wght@400;700&display=swap" rel="stylesheet">
   

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container2" id="container2">
        <header>MAKE YOUR ITEM DONATIONS</header>
      
        <ol id="progress_bar">
          <li id="step1" class="step-todo">Step 1</li>
          <li id="step2" class="step-todo">Step 2</li>
          <li id="step3" class="step-todo">Step 3</li>
          <li id="step4" class="step-todo">Step 4</li>
         
        </ol>

        <asp:HiddenField ID="hfCurrentSlide" runat="server" Value="0" />
       
        <!-- Form Pages -->
        <div class="form-container" id="formContainer1" runat="server">
            <div id="slidePage1" class="slide-page" runat="server">
                <h2>Donation Details</h2>
                <asp:Label runat="server" Text="Donation Title: " Style="font-weight: bold;"></asp:Label>
                <asp:Label ID="lblDonationTitle" runat="server" Text="Donation Title"></asp:Label>
                <br />
                <asp:Label runat="server" Text="Organization: " Style="font-weight: bold;"></asp:Label>
                <asp:Label ID="lblOrgName" runat="server" Text="Organization Name"></asp:Label>

                <%--Donation Items Selection--%>
                <table id="categoryDetailsTable" class="table table-bordered" style="margin-top: 20px;">
                    <thead>
                        <tr>
                            <th>Category</th>
                            <th>Specific Items</th>
                            <th>Quantity</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptCategoryDetails" runat="server" OnItemDataBound="rptCategoryDetails_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control category-box" Text='<%# Eval("Category") %>' ReadOnly="true"></asp:TextBox>
                                    </td>

                                    <td>
                                        <div class="specific-items-container" style="position: relative; padding-right: 25px;">

                                            <i class="fa fa-plus add-icon" onclick="addTextBox(this);"
                                                style="cursor: pointer; position: absolute; right: 5px; transform: translateY(-50%);"></i>
                                        </div>
                                    </td>

                                    <td>
                                        <div class="quantity-container"></div>
                                    </td>
                                    <td>
                                        <div style="display: flex; justify-content: center; align-items: center;">
                                            <asp:Button ID="btnDonate" runat="server" style="background-color: #2f6aa1; border-color: #0a365f;" Text="I Want Donate" CssClass="btn btn-primary" OnClientClick="changeButtonColor(this); return false;" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <asp:HiddenField ID="hfCategoriesWithExpiryDate" runat="server" />

                <div style="display: flex; justify-content:end;">
                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm Submission"
                    OnClientClick="saveAndDisplay(); return false;"
                    CssClass="btn btn-success" />
                </div>
                <asp:HiddenField ID="hiddenFieldData" runat="server" />
                <asp:HiddenField ID="hiddenFieldDonatedItems" runat="server" />
                <asp:HiddenField ID="hiddenFieldExpiryDate" runat="server" />

                <div id="hiddenSection" style="display: none; padding-top: 35px;">
                    <div>
                        <asp:Label runat="server" Text="Item Descriptions: " style="font-weight: bold; padding-top: 10px;"></asp:Label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Placeholder="Brief description of items (eg: explain item condition, item usage, item type whether it's new or second-hand etc.)" TextMode="MultiLine" Rows="5" Style="margin-top: 10px;"></asp:TextBox>
                        
                        <span id="descEmpty" class="text-danger" style="display: none;">Please enter item description.</span>

                    </div>
                    <div class="mb-3" style="padding-top: 15px;">
                        <asp:Label runat="server" for="fileUploadImages" CssClass="form-label" Text="Upload the images of the donated items here (for organization reference):" Style="font-weight: bold;"></asp:Label>
                        <asp:FileUpload ID="fileUploadImages" runat="server" CssClass="form-control" AllowMultiple="true" Style="height: 45px; margin-top: 10px;" OnChange="validateAttachmentUpload(this)" />
                        <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                            <em>Please make sure that the images uploaded are valid. Organizations have the right to make a complaint to our team if the items received is different with the images uploaded.</em>
                        </small>
                        <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                            <em>Accepted Formats: .jpg, .jpeg, .png, Maximum 10 attachments.</em>
                        </small>
                        <span id="attchUploadError" class="text-danger" style="display: none;"></span>
                        <span id="attchEmpty" class="text-danger" style="display: none;">At least ONE image must be uploaded.</span>

                    </div>
                    <div class="mb-3" style="padding-top: 15px;" id="expiryDate">
                        <asp:Label runat="server" CssClass="form-label" Text="Select the expiry date:" Style="font-weight: bold;"> </asp:Label>
                       
                        <div id="displayTableContainer"></div>
                        <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                            <em>Please ensure the expiry date input are correct. Categorize the items in one group if they have the same expiry date.</em>
                            <em>You are only allowed to donate items with an expiry date that is at least one month from today.</em>
                        </small>
                    </div>
                    <div style="display: flex; justify-content: end;">
                        <asp:Button ID="btnConfirm2" runat="server" Text="Confirm Submission"
                            OnClientClick="showMessage('Are you sure?', event);"
                            CssClass="btn btn-success" />

                        <asp:HiddenField ID="hfUserConfirmed" runat="server" />

                        <asp:Button class="firstNext" ID="firstNext" CssClass="btn btn-success" OnClick="NextSlide_Click" runat="server" Text="Next" style="display: none;"></asp:Button>
                    </div>

                </div>

            </div>
        </div>
         
       <%-- Donor Information--%>
        <div class="form-container" id="formContainer2" runat="server">
            <div id= "slidePage2" class="slide-page" runat="server">
              
                <h2>Donor Information</h2>
                <span class="text-info" style="font-style:italic; color: #0047AB!important;">Note that the information are retrieved automatically based on current logged-in user information. Please modify here if there is any changes regarding the donor information.</span>
                <div class="form-group">
                    <label style="padding-top: 10px; font-weight: bold;">Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control text-uppercase"></asp:TextBox>
                    <span id="nameError" class="text-danger" style="display: none;"></span>
                </div>
                <div class="form-group">
                    <label style="font-weight: bold;">Email Address</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    <span id="emailError" class="text-danger" style="display: none;"></span>
                </div>
                <div class="form-group">
                    <label style="font-weight: bold;">Phone Number</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Example: 60125678954"></asp:TextBox>
                    <span id="phoneError" class="text-danger" style="display: none;"></span>
                </div>
                <div class="form-group">
                    <label style="font-weight: bold;">Pick-up Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    <span id="addressError" class="text-danger" style="display: none;"></span>
                </div>

                <div class="form-group">
                    <label style="font-weight: bold;">State in Malaysia</label>
                    <asp:DropDownList ID="pickupRegion" CssClass="form-control form-control-lg" runat="server" style="font-size: 1rem;">
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
                    <span id="stateError" class="text-danger" style="display: none;"></span>
                
                </div>

               <div style="display: flex; justify-content: end;">
                    <%--<asp:Button class="prev-1" runat="server" OnClientClick="finalSubmit(event); return false;" OnClick="PreviousSlide_Click" Text="Previous"></asp:Button>--%>
                    <asp:Button class="next-1" runat="server" OnClientClick="finalSubmit(event); return false;" Text="Submit" CssClass="btn btn-success"></asp:Button>
                </div>
                </div>
            </div>

        <%--Pickup and Payment Details--%>
        <div class="form-container" id="formContainer3" runat="server">
            <div id= "slidePage3" class="slide-page" runat="server">
                <h2>Pickup and Delivery Details</h2>
                <span class="text-info" style="font-style:italic; color: #0047AB!important;">Your donation request have been approved by the organization. Here are the summary. Please note that the page will inform you if there is any unwanted items by the organization.</span>
                <span class="text-info" style="font-style:italic; color: #0047AB!important;">You may submit the pick up details and make your payment now.</span>
                <div class="form-group">
                   <label style="font-weight: bold; padding-top: 10px;">Summary of Items </label>
                    <asp:PlaceHolder ID="phDonationItems" runat="server"></asp:PlaceHolder>
                </div>
                <div class="form-group" style="text-align: center;">
                    <label style="font-style:italic; padding-top: 10px; color: darkred; font-weight: 500;">Item(s) Removed by Organization: </label>
                    <asp:Label ID="lblDeletedItem" style="font-style:italic; padding-top: 10px; color: darkred;" Text="" runat="server"></asp:Label>
                </div>

                <div class="form-group">
                    <label style="font-weight: bold;">Pickup Date</label>
                    <asp:TextBox ID="txtPickupDate" runat="server" CssClass="form-control" TextMode="Date" OnFocus="restrictPastDate()"></asp:TextBox>
                    <span id="dateError" class="text-danger" style="display: none;"></span>
                     <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                         <em>The delivery will be available starting one day after your payment is made.</em>
                     </small>
                </div>
                <div class="form-group">
                    <label style="font-weight: bold;">Pickup Time</label>
                    <asp:TextBox ID="txtPickupTime" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                    <span id="timeError" class="text-danger" style="display: none;"></span>
                </div>
                <div class="form-group">
                    <label style="font-weight: bold;">Vehicle Type</label>
                    <asp:LinkButton ID="infoIcon" runat="server" CssClass="info-icon" style="color: #085226;" PostBackUrl="/DeliveryVehicle.aspx" ToolTip="View furniture dimensions">
                         <i class="fa fa-info-circle"></i>
                     </asp:LinkButton>
                    <asp:DropDownList ID="ddlVehicleType" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select Preferred Vehicle Type" Value="" Disabled="true" Selected="true" />
                        <asp:ListItem Text="Car (Any car models)" Value="Car"></asp:ListItem>
                        <asp:ListItem Text="4x4 Pickup" Value="4x4 Pickup"></asp:ListItem>
                        <asp:ListItem Text="Van 7 Feet" Value="Van 7 Feet"></asp:ListItem>
                        <asp:ListItem Text="Van 9 Feet" Value="Van 9 Feet"></asp:ListItem>
                        <asp:ListItem Text="Lorry 10 Feet" Value="Lorry 10 Feet"></asp:ListItem>
                        <asp:ListItem Text="Lorry 14 Feet" Value="Lorry 14 Feet"></asp:ListItem>
                        <asp:ListItem Text="Lorry 17 Feet" Value="Lorry 17 Feet"></asp:ListItem>
                    </asp:DropDownList>
                    <span id="vehicleError" class="text-danger" style="display: none;"></span>
                </div>

               <%-- <div class="form-group">
                    <label style="font-weight: bold;">Pickup Method</label>
                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select Pick Up Method" Value="" Disabled="true" Selected="true" />
                        <asp:ListItem Text="Pick up from me" Value="Pick up from me"></asp:ListItem>
                        <asp:ListItem Text="Outside" Value="Outside"></asp:ListItem>
                        <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
                    </asp:DropDownList>
                    <span id="methodError" class="text-danger" style="display: none;"></span>
                </div>--%>

                <div class="form-group">
                    <asp:Label runat="server" style="font-weight: bold;" Text="Note to Delivery Rider (if any)" />
                    <asp:TextBox ID="noteRider" runat="server" CssClass="form-control" placeholder="Any note to rider" TextMode="MultiLine" Rows="2"/>
                </div>

                <div class="form-group">
                    <asp:Label runat="server" style="font-weight: bold;" Text="Note to Organization (if any)" />
                    <asp:TextBox ID="noteOrg" runat="server" CssClass="form-control" placeholder="Any note to organization" TextMode="MultiLine" Rows="2"/>
                </div>
         
                <div style="display: flex; justify-content:end;">
                    <asp:Button class="prev-3" ID="btnCancel" runat="server" Text="Cancel Donations" OnClientClick="return confirmCancel();" CssClass="btn btn-danger" style="margin-right: 20px; float: left;"></asp:Button>
                    <asp:Button class="next-2" runat="server" OnClientClick="return validateSlide3();" OnClick="NextSlide_Click" Text="Next" CssClass="btn btn-success"></asp:Button>
                </div>

            </div>
            </div>
        <div class="form-container" id="formContainer4" runat="server">
         <div id= "slidePage4" class="slide-page" runat="server">
             <div class="container" style="padding-top: 20px;">
                 <div class="row">
                     <!-- First Column: Distance and Fee Summary -->
                     <div class="col-xs-12 col-md-6">
                         <div class="panel panel-default">
                             <div class="panel-heading">
                                 <h3>Payment Summary</h3>
                             </div>
                             <div class="panel-body">
                                 <div class="form-group">
                                     <asp:Label runat="server" Text="Delivery Fee: " Style="font-weight: bold;"></asp:Label>
                                     <asp:Label ID="lblDeliveryFee" runat="server" Text="Calculated Fee"></asp:Label>

                                 </div>
                                 <div class="form-group">
                                     <label>Calculate Delivery Fee</label>
                                     <div id="mapDiv" style="width: 100%; height: 400px;"></div>

                                     <div class="row">
                                         <div class="col-md-8">
                                             <light style="font-weight: 500;">Delivery Fee Calculation</light>
                                         </div>
                                         <div class="col-md-4 text-right">
                                             <asp:Label ID="calculation" runat="server" Text=""> </asp:Label>
                                         </div>
                                     </div>

                                     <div class="row">
                                         <div class="col-md-8">
                                             <light style="font-weight: 500;">Total Distance Travelled:</light>
                                         </div>
                                         <div class="col-md-4 text-right">
                                             <asp:Label ID="distance" runat="server" Text="" style="display: inline-block;"></asp:Label>
                                         </div>
                                     </div>

                                     <div class="row">
                                         <div class="col-md-8">
                                             <light style="font-weight: 500;">Delivery Fee:</light>
                                         </div>
                                         <div class="col-md-4 text-right">
                                             <asp:Label ID="deliveryFee" runat="server" Text="RM 0.00"></asp:Label>
                                         </div>
                                     </div>

                                     <div class="row">
                                         <div class="col-md-8">
                                             <light style="font-weight: 500;">Additional Charges (Cross-State):</light>
                                         </div>
                                         <div class="col-md-4 text-right">
                                             <asp:Label ID="charge" runat="server" Text="RM 0.00"></asp:Label>
                                         </div>
                                     </div>

                                     <div class="row">
                                         <div class="col-md-8">
                                             <strong style="font-size: 20px;">Total Payment</strong>
                                         </div>
                                         <div class="col-md-4 text-right">
                                             <asp:Label ID="payment" runat="server" style="font-size: 20px; font-weight: bold;" Text="RM 0.00"></asp:Label>
                                         </div>
                                     </div>
                                 </div>
                             </div>
                         </div>
                     </div>

                     <!-- Second Column: Payment Details Form -->
                     <div class="col-xs-12 col-md-6">
                         <div class="panel panel-default">
                             <div class="panel-heading">
                                 <div class="row">
                                     <div class="col-md-8">
                                         <h3>Payment Details</h3>
                                     </div>
                                     <div class="col-md-4 text-right" style="display: flex;">
                                         <img class="img-responsive cc-img" src="https://upload.wikimedia.org/wikipedia/commons/0/04/Visa.svg" alt="Visa" style="height: 40px; margin-right: 10px;" />
                                         <img class="img-responsive cc-img" src="https://upload.wikimedia.org/wikipedia/commons/a/a4/Mastercard_2019_logo.svg" alt="MasterCard" style="height: 40px;" />
                                     </div>
                                 </div>
                             </div>
                             <div class="panel-body">
                                 <!-- Card Number -->
                                 <div class="form-group">
                                     <asp:Label runat="server" Text="Card Number" style="font-weight: bold;"/>
                                     <div class="input-group">
                                         <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" placeholder="Valid Card Number" MaxLength="19" oninput="formatCreditCardNumber(this)" onkeypress="allowOnlyNumber(event)"></asp:TextBox>
                                         <span class="input-group-addon" style="cursor: pointer; margin-left: 10px;" data-toggle="modal" data-target="#creditCardGuidelinesModal">
                                             <i class="fa fa-credit-card"></i>
                                         </span>
                                         <span id="cardNumberError" class="text-danger" style="display: none;"></span>
                                     </div>
                                 </div>

                                 <!-- Card Owner -->
                                 <div class="form-group">
                                     <asp:Label runat="server" Text="Cardholder Name" style="font-weight: bold;"/>
                                     <asp:TextBox ID="txtCardOwner" runat="server" CssClass="form-control text-uppercase" placeholder="Card Owner Names" />
                                     <span id="cardholderError" class="text-danger" style="display: none;"></span>
                                 </div>

                                 <!-- Expiration Date and CV Code -->

                                 <div class="form-group">
                                    <asp:Label runat="server" Text="Expiration Date (MM/YY)" style="font-weight: bold;" />
                                    <asp:TextBox ID="txtExpirationDate" runat="server" CssClass="form-control" placeholder="MM / YY (08/26)" MaxLength="5" oninput="formatExpiryDate(this);"></asp:TextBox>
                                    <span id="expiryError" class="text-danger" style="display: none;"></span>
                                </div>

                                 <div class="form-group">
                                     <asp:Label runat="server" Text="CVV/ CVC Code" style="font-weight: bold;"/>
                                     <asp:TextBox ID="txtCVVCode" runat="server" CssClass="form-control" placeholder="CVV/ CVC" MaxLength="3" onkeypress="allowOnlyNumber(event)" />
                                     <span id="cvvError" class="text-danger" style="display: none;"></span>
                                 </div>

                                 <div class="form-group">
                                    <asp:Label runat="server" Text="Cardholder Email Address" style="font-weight: bold;"/>
                                    <asp:TextBox ID="txtEmailOTP" runat="server" CssClass="form-control" placeholder="Valid Email Address" TextMode="Email"/>
                                    <span id="emailOTPError" class="text-danger" style="display: none;"></span>
                                     <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                                        <em>One-Time Password (OTP) will be sent to this email address for payment authentication later.</em>
                                    </small>
                                 </div>

                                 <div class="form-group" id="otp" style="display: none;">
                                    <asp:Label runat="server" Text="Enter OTP" style="font-weight: bold;"/>
                                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" placeholder="OTP" MaxLength="8" onkeypress="allowOnlyNumber(event)" />
                                    <asp:Button ID="btnValidateOtp" runat="server" Text="Validate OTP" CssClass="btn btn-primary" OnClick="btnValidateOtp_Click" style="margin-top: 10px; display: flex; justify-content:end;"/>
                                </div>

                             </div>


                         </div>
                     </div>
                 </div>
             </div>
         </div>

      
                 <div class="button-container">
                     <asp:Button class="prev-3" runat="server" OnClick="PreviousSlide_Click" Text="Previous" CssClass="btn btn-info"></asp:Button>
                     <asp:Button class="submit-2" id="btnSubmit" Text="Submit" runat="server" CssClass="btn btn-success" OnClientClick="return validateSlide4();" OnClick="btnSubmitDelivery_Click"></asp:Button>
                 </div>
            </div>
                  
        </div>

    <div class="modal fade" id="creditCardGuidelinesModal" tabindex="-1" role="dialog" aria-labelledby="creditCardGuidelinesModalLabel" aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="creditCardGuidelinesModalLabel">Credit Card Guidelines</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- Text instructions -->
                    <ul>
                        <li>Ensure your card number is 16 digits long.</li>
                        <li>Card number must be numeric.</li>
                        <li>Use the expiration date in the format MM/YY.</li>
                        <li>Provide the 3-digit CVV code from the back of your card.</li>
                        <li>Ensure your card is valid before submitting.</li>
                    </ul>

                    <!-- Images -->
                    <div class="text-center">
                        <h6>Sample Credit Card Information</h6>
                        <!-- Example credit card image -->
                        <img src="/Image/credit_card.png" alt="Sample Credit Card Front" class="img-fluid" style="max-width: 100%; height: auto;">
                        <p class="text-muted">Front of the card: Ensure the number is entered correctly.</p>                       
                        <p class="text-muted">Back of the card: Use the CVV code.</p>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        var mapInitialized = false;

        // update ui progress bar
        updateProgressBar();
      
        function changeButtonColor(button) {
            var row = button.closest('tr'); 
            if (button.style.backgroundColor === 'rgb(93, 181, 124)') {
                // if the button was in "Selected!" state, revert to "I Want Donate"
                button.style.backgroundColor = '#2f6aa1';
                button.style.borderColor = '#0a365f';
                button.value = 'I Want Donate';

                // remove all added textboxes for this category
                removeAllTextBoxesForCategory(row);

            } else {
                // change to 'Selected!' state
                button.style.backgroundColor = '#5db57c';
                button.style.borderColor = '#11632d';
                button.value = 'Selected!';
                addTextBox(button);
            }
        }


        function showElements() {
            // find the hidden section by its ID and change its display to block
            var hiddenSection = document.getElementById('hiddenSection');
            hiddenSection.style.display = 'block';
        }

        function showInfo(message) {
            Swal.fire({
                text: message,
                icon: 'info',
                confirmButtonText: 'OK',
               
            });
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
                timer: 5000,
                timerProgressBar: true,
                willClose: () => {

                    window.location.href = 'Home.aspx';
                }
            });
        }

        function showAlert(message) {
            Swal.fire({
                text: message,
                icon: 'warning',
                confirmButtonText: 'OK',
                timer: 5000,
                timerProgressBar: true,
                willClose: () => {

                    window.location.href = 'Login.aspx';
                }
            });
        }

        function confirmCancel() {
            Swal.fire({
                title: 'Are you sure?',
                text: 'Do you want to cancel and delete this donation? This action cannot be undone.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#3085d6',
                confirmButtonText: 'Yes, cancel it!',
                cancelButtonText: 'No, keep it'
            }).then((result) => {           
                if (result.isConfirmed) {
                    // make an AJAX call to delete the donation
                    $.ajax({
                        type: "POST",
                        url: "DonationRequest.aspx/DeleteDonation",  
                        data: JSON.stringify({ donationId: '<%= Request.QueryString["donationId"] %>' }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d === "success") {
                                Swal.fire({
                                    title: 'Deleted!',
                                    text: 'Your donation has been deleted.',
                                    icon: 'success',
                                    confirmButtonText: 'OK'
                                }).then(() => {
                                    
                                    window.location.href = 'Home.aspx';
                                });
                            } else {
                                Swal.fire('Error!', 'There was an error deleting the donation.', 'error');
                            }
                        },
                        error: function () {
                            Swal.fire('Error!', 'There was an error processing your request.', 'error');
                        }
                    });
                   }
            });

            return false;  // prevent the default form submission when no action performed yet
        }

        // show pop up message for submission 
        function showMessage(title, event) {
            event.preventDefault(); 

            Swal.fire({
                title: title,
                text: "Once you proceed, you won't be able to make any changes!",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Yes, proceed!",
                cancelButtonText: "No, cancel",
                reverseButtons: true
            }).then((result) => {
                if (result.isConfirmed) {
                    // save values and validate the form
                    const isValid = saveValues(event);

                    if (isValid) {
                        // if all values are valid, hide confirm button and show the next button
                        document.getElementById('<%= btnConfirm2.ClientID %>').style.display = 'none';
                        document.getElementById('<%= firstNext.ClientID %>').style.display = 'block';
                       
                    } else {
                        // if there are validation errors, do not change button states
                        Swal.fire({
                            icon: 'error',
                            title: 'Submission Error',
                            text: 'Please fill in all the required fields before proceeding.'
                        });
                    }
                } else if (result.dismiss === Swal.DismissReason.cancel) {

                }
            });
        }

    
        // function to remove all textboxes for the current category (row)
        function removeAllTextBoxesForCategory(row) {
            // find all specific item textboxes and quantity textboxes in this row
            var specificItemsContainer = row.querySelector('.specific-items-container');
            var quantityContainer = row.querySelector('.quantity-container');

            // remove all textboxes (including the first one) in both containers
            if (specificItemsContainer) {
                var specificItemTextboxes = specificItemsContainer.querySelectorAll('input[type="text"]');
                specificItemTextboxes.forEach(function (textbox) {
                    var minusIcon = textbox.nextElementSibling;
                    if (minusIcon && minusIcon.classList.contains('fa-minus')) {
                        minusIcon.remove(); // remove associated minus icon
                    }
                    textbox.remove(); // remove the textbox
                });
            }

            if (quantityContainer) {
                var quantityTextboxes = quantityContainer.querySelectorAll('input[type="number"]');
                quantityTextboxes.forEach(function (textbox) {
                    textbox.remove(); // remove the quantity textbox
                });
            }

            // after removing, update the placeholders
            updatePlaceholders();
        }


        function updatePlaceholders() {
            // get all specific items textboxes
            var specificItems = document.querySelectorAll('.specific-items-container input[type="text"]');
            var quantities = document.querySelectorAll('.quantity-container input[type="number"]');

            // loop through and update the placeholder for specific items and quantities
            specificItems.forEach(function (textbox, index) {
                if (textbox) {
                    textbox.setAttribute('placeholder', 'Enter item ' + (index + 1));
                }
            });

            quantities.forEach(function (textbox, index) {
                if (textbox) {
                    textbox.setAttribute('placeholder', 'Enter quantity for item ' + (index + 1));
                }
            });
        }

        function addTextBox(icon) {
            // get the table by its ID
            var table = document.getElementById('categoryDetailsTable');

            // get the parent row of the clicked icon
            var row = icon.closest('tr');

            // get the "I Want Donate" button inside this row by ID 
            var donateButton = row.querySelector('input[id*="btnDonate"], button[id*="btnDonate"]'); 

            // check if the donate button exists and is in the "Selected!" state 
            if (donateButton && donateButton.style.backgroundColor === 'rgb(93, 181, 124)') {
                // if button is selected, proceed to add textboxes

                // find the specific items container and create a new input element
                var specificItemsContainer = row.querySelector('.specific-items-container');
                var newSpecificItem = document.createElement('input');
                newSpecificItem.setAttribute('type', 'text');
                newSpecificItem.setAttribute('class', 'form-control');
                newSpecificItem.style.marginTop = '15px';

                // find the quantity container and create a new input element
                var quantityContainer = row.querySelector('.quantity-container');
                var newQuantity = document.createElement('input');
                newQuantity.setAttribute('type', 'number');
                newQuantity.setAttribute('class', 'form-control');
                newQuantity.style.marginTop = '15px';

                // create minus icon for removing the textboxes
                var minusIcon = document.createElement('i');
                minusIcon.setAttribute('class', 'fa fa-minus');
                minusIcon.style.cursor = 'pointer';
                minusIcon.style.marginLeft = '10px';
                minusIcon.onclick = function () {
                    removeTextBox(newSpecificItem, newQuantity);
                };

                // append the new input to the specific items container
                specificItemsContainer.appendChild(newSpecificItem);
                specificItemsContainer.appendChild(minusIcon);

                // append the new input to the quantity container
                quantityContainer.appendChild(newQuantity);
                updatePlaceholders();


            } else {
                // notify the user that they need to press "I Want Donate" first
                showInfo('Please press "I Want Donate" to select this donation before adding items.');
            }
        }

        // function to remove unwanted textboxes
        function removeTextBox(specificItemTextbox, quantityTextbox) {
            if (specificItemTextbox && specificItemTextbox.parentNode) {
                // remove the minus icon associated with this textbox
                var minusIcon = specificItemTextbox.nextElementSibling;
                if (minusIcon && minusIcon.classList.contains('fa-minus')) {
                    minusIcon.parentNode.removeChild(minusIcon);
                }
                specificItemTextbox.parentNode.removeChild(specificItemTextbox);
            }
            if (quantityTextbox && quantityTextbox.parentNode) {
                quantityTextbox.parentNode.removeChild(quantityTextbox);
            }

            // after removing, update placeholders to change correct numbering
            updatePlaceholders();
        }


        // display map
        function initMap() {
            var map = new google.maps.Map(document.getElementById('mapDiv'), {
                center: { lat: 40.1215, lng: -100.4504 },
                zoom: 7,
                mapId: '69dfb9f5c086e55e' // valid map ID get from Google Cloud Console
            });

            
            // fetch the donationPublishId from query string
            var donationPublishId = getQueryStringParameter('donationPublishId');
            if (!donationPublishId) {
                console.error('donationPublishId not found in query string');
                return;
            }

            // geocode the pickup address and plot it
            getPickUpAddress(function (pickupLocation, pickupAddress) {
                if (pickupLocation) {
                    // fetch and geocode the organization address and plot it
                    fetchOrgAddress(donationPublishId, function (orgLocation, orgAddress) {
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

        
        // function to check if slide 4 is active and initialize the map if it isn't already initialized
        function onslidechange(slidenumber) {

            if (slidenumber === 4 && !mapinitialized) {
                initmap(); // call the map initialization function when slide 4 is visible
                mapinitialized = true; // set this to true so the map isn't initialized again
               
            }
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

                    // calculate the distance and display it
                    var distance = result.routes[0].legs[0].distance.text;

                    console.log('Distance: ' + distance);

                    var distanceInKm = parseFloat(distance.replace(/ km/, '')).toFixed(1);
                    console.log('Distance: ' + distanceInKm);
                    sendDistance(distanceInKm);

                  
                    // optionally, display the distance in an info window or another element
                    var infoWindow = new google.maps.InfoWindow({
                        content: `<strong>Distance:</strong> ${distance}`
                    });

                    map.addListener('click', function () {
                        infoWindow.open(map);
                    });

                } else {
                    console.error('Directions request failed due to ' + status);
                }
            });
        }

       
        // function to get the donationPublishId from the query string
        function getQueryStringParameter(param) {
            var urlParams = new URLSearchParams(window.location.search);
            return urlParams.get(param);
        }

        function getPickUpAddress(callback) {
            $.ajax({
                type: "POST",
                url: "DonationRequest.aspx/GetPickupAddress",
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
                            callback(location, backendAddress);  // pass geocoded location and original address
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


        // function to fetch and geocode organization address based on donationPublishId
        function fetchOrgAddress(donationPublishId, callback) {
            $.ajax({
                type: 'POST',
                url: 'DonationRequest.aspx/GetOrganizationAddress',
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

        // send distance calculated to backend
        function sendDistance(distance) {
            fetch('/DonationRequest.aspx/ReceiveDistance', {
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

        function saveAndDisplay() {
            // find the hidden section element
            var hiddenSection = document.getElementById('hiddenSection');
            hiddenSection.style.display = 'none'; // initially hide the hidden section

            // validate textboxes and "I Want Donate" buttons
            var categoryRows = document.querySelectorAll('#categoryDetailsTable tbody tr');
            var hasEmptyTextboxes = false; // track if any textbox is empty
            var hasDonateSelection = false; // track if any "I Want Donate" button is selected
            var hasInvalidQuantity = false; // track if any quantity is invalid

            // check each row for empty textboxes and donation button selection
            categoryRows.forEach(function (row) {
                var specificItems = row.querySelectorAll('.specific-items-container input[type="text"]');
                var quantities = row.querySelectorAll('.quantity-container input[type="number"]');
                var donateButton = row.querySelector('input[id*="btnDonate"], button[id*="btnDonate"]');

                var isDonateButtonSelected = donateButton && donateButton.style.backgroundColor === 'rgb(93, 181, 124)'; // check if button is green
                var isTextboxEmpty = false; // track if textboxes in this row are empty

                // check for empty specific items and quantity textboxes if donate button is selected
                if (isDonateButtonSelected) {
                    hasDonateSelection = true; // at least one donate button is selected

                    specificItems.forEach(function (textbox) {
                        if (!textbox.value.trim()) {
                            isTextboxEmpty = true; // mark as true if a textbox is empty
                        }
                    });

                    quantities.forEach(function (textbox) {
                        var quantityValue = textbox.value.trim();
                        var quantity = parseInt(quantityValue, 10);

                        // check if quantity is a whole number greater than zero
                        if (!quantityValue || isNaN(quantity) || quantity <= 0 || !Number.isInteger(quantity)) {
                            hasInvalidQuantity = true; // invalid if not a whole number or is less than 1
                            textbox.style.border = "1px solid red"; // highlight the invalid input
                        } else {
                            textbox.style.border = ""; // reset border if valid
                        }

                        if (!textbox.value.trim()) {
                            isTextboxEmpty = true; // mark as true if quantity is empty
                        }
                    });

                    // if the donate button is selected but textboxes are empty, set as false
                    if (isTextboxEmpty) {
                        hasEmptyTextboxes = true;
                    }
                }
            });

            if (hasInvalidQuantity) {
                showError("Please enter valid whole numbers greater than zero for quantity.");
                return; 
            }

            // if no donate button is selected or if there are empty textboxes for selected donations, show pop up dialog message
            if (!hasDonateSelection || hasEmptyTextboxes) {
                Swal.fire({
                    icon: 'error',
                    title: 'Missing Details',
                    text: 'Donor must select "I Want Donate" and fill in all donated item details!',
                });
                hiddenSection.style.display = 'none'; // ensure hidden section stays hidden
                return false; // prevent further actions if validation fails
            }

            // if all textboxes are filled and at least one donate button is selected, show hidden section
            hiddenSection.style.display = 'block';
            saveTextBoxValues();
            displaySavedData();
        }


        // save textboxes values in the table
        function saveTextBoxValues() {
            var categoryRows = document.querySelectorAll('#categoryDetailsTable tbody tr');
            var sessionData = [];
            var categoriesWithExpiryDate = JSON.parse(document.getElementById('<%= hfCategoriesWithExpiryDate.ClientID %>').value);


            categoryRows.forEach(function (row) {
                var category = row.querySelector('.category-box').value;
                console.log('Retrieved Category:', category);
                var specificItems = row.querySelectorAll('.specific-items-container input[type="text"]');
                var quantities = row.querySelectorAll('.quantity-container input[type="number"]');

                // chg here to get expiry date
                specificItems.forEach(function (item, index) {
                    if (item.value.trim() !== '' && (category === 'Food (Canned/Packed Food)' || categoriesWithExpiryDate.includes(category))) {
                        sessionData.push({
                            category: category,
                            item: item.value.trim(),
                            quantity: quantities[index].value.trim()
                        });
                    }
                });
            });

            console.log('Session Data:', sessionData);

            // save to a hidden field to send to the server
            document.getElementById('<%= hiddenFieldData.ClientID %>').value = JSON.stringify(sessionData);
        }

        // display expiry date table
        function displaySavedData() {
            // get the session data from the hidden field
            var sessionData = JSON.parse(document.getElementById('<%= hiddenFieldData.ClientID %>').value);
            var expiryDateSection = document.getElementById('expiryDate');

            // get the container where the table will be displayed
            var displayContainer = document.getElementById('displayTableContainer');

            // clear any previous content
            displayContainer.innerHTML = '';

            // if no data, show a message
            if (sessionData.length === 0) {
                expiryDateSection.style.display = 'none';
                return;
            }

            expiryDateSection.style.display = 'block';
            // create a table element
            var table = document.createElement('table');
            table.className = 'table table-bordered';
         
            var headerRow = document.createElement('tr');
            headerRow.innerHTML = `
                <th>Category</th>
                <th>Item</th>
                <th>Quantity</th>
                <th>Expiry Date</th>
            `;
            table.appendChild(headerRow);

            // loop through the sessionData and populate the rows
            sessionData.forEach(function (data) {
                var row = document.createElement('tr');

                // category
                var categoryCell = document.createElement('td');
                categoryCell.textContent = data.category;
                row.appendChild(categoryCell);

                // item
                var itemCell = document.createElement('td');
                itemCell.textContent = data.item;
                row.appendChild(itemCell);

                // quantity
                var quantityCell = document.createElement('td');
                var quantityWrapper = document.createElement('div');

                // add the first input and set it to read-only by default
                var quantityInput = createQuantityInput(data.quantity, quantityWrapper, true);
                quantityWrapper.appendChild(quantityInput);

                quantityCell.appendChild(quantityWrapper);
                row.appendChild(quantityCell);

                // expiry date
                var dateCell = document.createElement('td');
                var dateWrapper = document.createElement('div');
                dateWrapper.style.display = 'flex'; 

                // add the dropdown and date picker next to each other for the first textbox
                var expiryDateDropdown = createExpiryDateDropdown(dateWrapper, quantityWrapper, data.quantity, dateCell);
                dateWrapper.appendChild(expiryDateDropdown);
                dateCell.appendChild(dateWrapper);
                row.appendChild(dateCell);

                table.appendChild(row);

                // function to create a new input textbox for remaining quantity
                function createQuantityInput(value, wrapper, isReadOnly) {
                    var input = document.createElement('input');
                    input.setAttribute('type', 'text');
                    input.className = 'form-control';
                    input.style.marginBottom = '10px';
                    input.value = value || '1';

                    if (isReadOnly) {
                        input.setAttribute('readonly', true);
                    }

                    // store the original value before any changes
                    input.setAttribute('data-prev-value', input.value);

                    // listen for 'focus' to capture the original value before user types
                    input.addEventListener('focus', function () {
                        input.setAttribute('data-prev-value', input.value);
                    });

                    // listen for 'blur' event (when the user finishes typing)
                    input.addEventListener('blur', function () {
                        validateQuantity(input);
                        updateRemainingQuantities(input);
                    });

                    // allow user to press enter to generate a new textbox
                    input.addEventListener('keydown', function (e) {
                        if (e.key === 'Enter') {
                        
                            validateQuantity(input);
                            updateRemainingQuantities(input);

                          
                        }
                    });

                    return input;
                }

                // function to validate input (no empty and number must be >= 1)
                function validateQuantity(input) {
                    var value = input.value.trim();

                    if (!value || isNaN(value) || parseInt(value) < 1) {
                        input.value = '1';
                    }
                }

                // function to create a dropdown for expiry date selection
                function createExpiryDateDropdown(wrapper, quantityWrapper, totalQuantity, dateCell) {
                    var dropdown = document.createElement('select');
                    dropdown.className = 'form-control';
                    dropdown.style.marginLeft = '10px';
                    dropdown.style.marginRight = '10px'; 

                    // create "None" option
                    var noneOption = document.createElement('option');
                    noneOption.value = 'none';
                    noneOption.textContent = 'None';
                    dropdown.appendChild(noneOption);

                    // create "Choose Date" option
                    var chooseDateOption = document.createElement('option');
                    chooseDateOption.value = 'choose-date';
                    chooseDateOption.textContent = 'Choose Date';
                    chooseDateOption.selected = true;
                    dropdown.appendChild(chooseDateOption);

                    
                    dropdown.addEventListener('change', function () {
                        if (dropdown.value === 'choose-date') {
                        
                            // enable textboxes and add date picker 
                            applyFirstDatePicker(dateWrapper, quantityWrapper);
                            enableTextboxes(quantityWrapper);
                        } else {
                            // remove all additional textboxes and date pickers, leaving only the first textbox and resetting it
                            removeAllTextboxesExceptFirst(quantityWrapper);
                            disableTextboxes(quantityWrapper, totalQuantity);
                            removeDatePickerFromAll(dateCell);
                        }
                    });
                    applyFirstDatePicker(wrapper, quantityWrapper);
                    enableTextboxes(quantityWrapper);


                    return dropdown;
                }

                // function to apply the first date picker inline with the dropdown
                function applyFirstDatePicker(dateWrapper, quantityWrapper) {
                    // clear any existing date pickers
                    removeDatePickerFromAll(dateWrapper);

                    var datePicker = createDatePicker(); // create date picker for the first input
                    dateWrapper.appendChild(datePicker); // append to the right of the dropdown

                    // enable textboxes if they are disabled
                    enableTextboxes(quantityWrapper);
                }

                // function to remove all extra textboxes and keep only the first one
                function removeAllTextboxesExceptFirst(quantityWrapper) {
                    var inputs = quantityWrapper.querySelectorAll('input[type="text"]');
                    inputs.forEach(function (input, index) {
                        if (index > 0) {
                            input.remove(); // remove all except the first textbox
                        }
                    });
                }

                // function to remove all date pickers from date cell
                function removeDatePickerFromAll(dateCell) {
                    var datePickers = dateCell.querySelectorAll('input[type="date"]');
                    datePickers.forEach(function (datePicker) {
                        datePicker.remove();
                    });
                }

                // function to create a new date picker
                function createDatePicker() {
                    var input = document.createElement('input');
                    input.setAttribute('type', 'date');
                    input.className = 'form-control';
                    input.style.marginBottom = '10px';

                    // get today's date
                    var today = new Date();

                    // add one month to today's date
                    var oneMonthLater = new Date(today);
                    oneMonthLater.setMonth(today.getMonth() + 1);

                    // handle case when adding a month rolls over to the next year
                    if (oneMonthLater.getMonth() !== (today.getMonth() + 1) % 12) {
                        oneMonthLater.setDate(0);  // set to last day of the previous month
                    }

                    // format the date to 'YYYY-MM-DD'
                    var formattedOneMonthLater = oneMonthLater.toISOString().split('T')[0];

                    // set the 'min' attribute to one month after today's date
                    input.setAttribute('min', formattedOneMonthLater);

                    return input;
                }



                // disable textboxes if "None" is selected
                function disableTextboxes(quantityWrapper, totalQuantity) {
                    var inputs = quantityWrapper.querySelectorAll('input[type="text"]');
                    inputs.forEach(function (input) {
                        input.setAttribute('readonly', true); 
                        input.value = totalQuantity; // keep the original quantity
                    });
                }

                // enable textboxes if "Choose Date" is selected
                function enableTextboxes(quantityWrapper) {
                    var inputs = quantityWrapper.querySelectorAll('input[type="text"]');
                    inputs.forEach(function (input) {
                        input.removeAttribute('readonly'); 
                    });
                }

                // function to update remaining quantities across inputs
                function updateRemainingQuantities(modifiedInput) {
                    var totalQuantity = parseInt(data.quantity);
                    var currentSum = 0;
                    var inputs = quantityWrapper.querySelectorAll('input[type="text"]');

                    inputs.forEach(function (input) {
                        currentSum += parseInt(input.value) || 0;
                    });

                    var remaining = totalQuantity - currentSum;

                    if (currentSum > totalQuantity) {
                        alert('Total quantity exceeds the original amount!');
                        var previousValue = modifiedInput.getAttribute('data-prev-value');
                        modifiedInput.value = previousValue;
                        return;
                    }

                    if (remaining < 0) {
                        remaining = 0;
                    }

                    if (remaining > 0 && !hasEmptyInput() && currentSum < totalQuantity) {
                        var newInput = createQuantityInput(remaining, quantityWrapper, false);
                        quantityWrapper.appendChild(newInput);

                        // add a new date picker on the next line for the newly added textbox
                        var newDatePickerWrapper = document.createElement('div');                      
                        var newDatePicker = createDatePicker();
                        newDatePickerWrapper.appendChild(newDatePicker);

                        dateCell.appendChild(newDatePickerWrapper); // add date picker below the new input
                    }
                }

                // check if there are any empty input fields left
                function hasEmptyInput() {
                    var inputs = quantityWrapper.querySelectorAll('input[type="text"]');
                    return Array.from(inputs).some(function (input) {
                        var value = input.value.trim();
                        return value === '' || isNaN(value) || parseInt(value) < 1;
                    });
                }

                // initialize the remaining quantities
                updateRemainingQuantities();
            });

            // append the table to the display container
            displayContainer.appendChild(table);
        }

        function saveValues(event) {
            event.preventDefault();  // always prevent the default form submission

            var categoryRows = document.querySelectorAll('#categoryDetailsTable tbody tr');
            var sessionDonatedItems = [];
            var sessionExpiryDate = [];
            var sessionImages = [];
            var allDataIsValid = true; 

            var description = document.getElementById('<%= txtDescription.ClientID %>').value.trim();
            var errorMsgDesc = document.getElementById('descEmpty'); 

            // validate description field
            if (!description) {
                console.error("Description cannot be empty");
                errorMsgDesc.style.display = 'block'; 
                allDataIsValid = false;  
            } else {
                errorMsgDesc.style.display = 'none';  
            }

            // process categoryDetailsTable for category, item, quantity
            categoryRows.forEach(function (row) {
                var category = row.querySelector('.category-box').value;
                var specificItems = row.querySelectorAll('.specific-items-container input[type="text"]');
                var quantities = row.querySelectorAll('.quantity-container input[type="number"]');
                var itemsArray = [];
                var quantitiesArray = [];
                var totalItemQuantity = 0;

                // validate category
                if (!category.trim()) {
                    console.error("Category cannot be empty");
                    allDataIsValid = false;  
                }

                specificItems.forEach(function (item, index) {
                    var itemValue = item.value.trim();
                    var quantityValue = quantities[index] ? quantities[index].value.trim() : '0';

                    // validate item and quantity
                    if (!itemValue || quantityValue === '0') {
                        console.error("Item names and quantities must be specified");
                        allDataIsValid = false;  
                    }

                    itemsArray.push(itemValue);
                    quantitiesArray.push(quantityValue);

                    // sum the total quantity for this item
                    totalItemQuantity += parseInt(quantityValue, 10) || 0;  // default to 0 if fails
                });

                if (itemsArray.length > 0) {
                    sessionDonatedItems.push({
                        category: category,
                        items: itemsArray.join(', '),
                        quantities: '(' + quantitiesArray.join(', ') + ')'
                    });

                    // add total quantity to each item for later expiry date processing
                    sessionDonatedItems[sessionDonatedItems.length - 1].totalQuantity = totalItemQuantity;
                }
            });

            // process displayTableContainer for expiry dates and total quantity
             var displayTable = document.querySelector('#displayTableContainer table');
            if (displayTable) {
                var displayRows = displayTable.querySelectorAll('tr:not(:first-child)');
                var itemQuantityMap = {};  // store total quantity for each item

                // loop through and sum the quantities for each item
                displayRows.forEach(function (row) {
                    var item = row.cells[1].textContent.trim();
                    var quantityInputs = row.cells[2].querySelectorAll('input[type="text"]');

                    quantityInputs.forEach(function (quantityInput) {
                        var quantity = parseInt(quantityInput.value.trim(), 10) || 0;  
                        if (!itemQuantityMap[item]) {
                            itemQuantityMap[item] = 0;  // initialize the total quantity for this item
                        }
                        itemQuantityMap[item] += quantity;  // add the current row's quantity to the total
                    });
                });

                // process and store the expiry date and total quantity for each item
                displayRows.forEach(function (row) {
                    var quantityInputs = row.cells[2].querySelectorAll('input[type="text"]');
                    var expiryDateInputs = row.cells[3].querySelectorAll('input[type="date"]');
                    var expiryDropdown = row.cells[3].querySelector('select');  
                    var category = row.cells[0].textContent.trim();
                    var item = row.cells[1].textContent.trim();

                    quantityInputs.forEach(function (quantityInput, index) {
                        var quantity = quantityInput.value.trim();
                        var expiryDate = expiryDateInputs[index] ? expiryDateInputs[index].value.trim() : '';
                        var expiryDropdownValue = expiryDropdown ? expiryDropdown.value : 'none';

                        // if dropdown is "none", skip saving expiry date
                        if (expiryDropdownValue !== 'none') {
                            
                            if (!quantity || !expiryDate) {
                                console.error("Quantity and expiry date must be specified");
                                allDataIsValid = false;
                            }

                            // store the sum of quantities for this item in totalQuantity
                            var totalQuantity = itemQuantityMap[item];  // get the total quantity for this item from the map

                            sessionExpiryDate.push({
                                category: category,
                                item: item,
                                quantity: quantity,  // entered quantity for expiry row
                                totalQuantity: totalQuantity,  // summed total quantity for the item
                                expiryDate: expiryDate
                            });
                        }
                    });
                });
            } else {
                console.warn('No table found in #displayTableContainer.');
            }

            // file upload
            var fileUploadControl = document.getElementById('<%= fileUploadImages.ClientID %>');
            var files = fileUploadControl.files;  
            var errorMsgImages = document.getElementById('attchEmpty'); 

            // validate file upload
            if (files.length === 0) {
                console.error("At least one file must be uploaded");
                errorMsgImages.style.display = 'block'; 
                allDataIsValid = false;  
            } else {
                errorMsgImages.style.display = 'none';  
            }

            if (files.length > 0) {
                var imagePromises = [];

                // function to convert file to Base64
                function getFileBase64(file) {
                    return new Promise((resolve, reject) => {
                        var reader = new FileReader();
                        reader.onload = function (event) {
                            resolve(event.target.result);  
                        };
                        reader.onerror = function (err) {
                            reject(err);
                        };
                        reader.readAsDataURL(file);  // convert file to Base64
                    });
                }

                // convert each image to Base64 and store in an array
                for (var i = 0; i < files.length; i++) {
                    var file = files[i];
                    imagePromises.push(getFileBase64(file));
                }

                // wait for all files to be converted to Base64
               
                Promise.all(imagePromises).then(function (base64Images) {
                    // store the Base64 string array in session 
                    sessionStorage.setItem("images", JSON.stringify(base64Images));
                   
                    //console.log('Base64 Images Array:', base64Images);

                    // saving other data
                    if (allDataIsValid) {
                        sessionStorage.setItem("donatedItems", JSON.stringify(sessionDonatedItems));
                        sessionStorage.setItem("expiryDates", JSON.stringify(sessionExpiryDate));
                        sessionStorage.setItem("desc", description);
                      
                        document.getElementById('<%= btnConfirm2.ClientID %>').style.display = 'none';
                        document.getElementById('<%= firstNext.ClientID %>').style.display = 'block';

                        allDataIsValid = true;
                    } else {
                        alert('Please choose the date for item expiration.');
                        allDataIsValid = false;
                    }
                }).catch(function (error) {
                    console.error("Error converting images to Base64:", error);
                    alert('There was an error processing the images. Please try again.');
                });

            } else {
                // no images to process, proceed with other data
                if (allDataIsValid) {
                    sessionStorage.setItem("donatedItems", JSON.stringify(sessionDonatedItems));
                    sessionStorage.setItem("expiryDates", JSON.stringify(sessionExpiryDate));
                    sessionStorage.setItem("desc", description);
                    alert("Data successfully saved!");
                } else {
                    alert('Please correct the errors before submitting.');
                }
            }

          
            console.log('Session Donated Items:', sessionDonatedItems);
            console.log('Session Expiry Date:', sessionExpiryDate);
            console.log(description);

            return allDataIsValid;
        }

        // submit slide 1 and 2 information to backend
        function finalSubmit(event) {
            event.preventDefault();

            // validate donor input
            if (!validateDonorInput()) {
              
                Swal.fire({
                    title: 'Error!',
                    text: 'Please fill in all required fields correctly.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
                return; 
            }

            Swal.fire({
                title: 'Are you sure to submit?',
                text: "By submitting, you acknowledge that the organization may remove some items from your donation request if any of them is unwanted.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes, submit it!',
                cancelButtonText: 'No, cancel!',
                reverseButtons: true
            }).then((result) => {
                if (result.isConfirmed) {
                    
                    var personalDetails = {
                        fullName: document.getElementById('<%= txtFullName.ClientID %>').value,
                        email: document.getElementById('<%= txtEmail.ClientID %>').value,
                        phone: document.getElementById('<%= txtPhone.ClientID %>').value,
                        address: document.getElementById('<%= txtAddress.ClientID %>').value,
                        state: document.getElementById('<%= pickupRegion.ClientID %>').value
                    };

                    // retrieve stored data from session 
                    var donatedItems = JSON.parse(sessionStorage.getItem("donatedItems"));
                    var expiryDates = JSON.parse(sessionStorage.getItem("expiryDates"));
                    var desc = sessionStorage.getItem("desc");
                    var image = sessionStorage.getItem("images");

                    // send data to the backend
                    $.ajax({
                        type: "POST",
                        url: "DonationRequest.aspx/SubmitAllData",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify({
                            donatedItemsJson: JSON.stringify(donatedItems),
                            expiryDatesJson: JSON.stringify(expiryDates),
                            donationPublishId: getQueryStringParameter('donationPublishId'),
                            details: personalDetails,
                            desc: desc,
                            image: image
                        }),
                        success: function (response) {
                            console.log("All data submitted successfully!");
                            Swal.fire({
                                title: 'Success!',
                                text: 'You have submitted your donation request successfully! Your request is now pending approval. Once it is approved, you may proceed to make payment.',
                                icon: 'success',
                                confirmButtonText: 'OK'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    // redirect to AllDonations.aspx after the user presses OK
                                    window.location.href = 'AllDonations.aspx';
                                }
                            });
                        },
                        error: function (xhr, status, error) {
                            console.error("Error submitting all data: " + xhr.responseText);
                            Swal.fire({
                                title: 'Error!',
                                text: 'Error in submitting, please try again!',
                                icon: 'error',
                                confirmButtonText: 'OK'
                            });
                        }
                    });
                } else if (result.dismiss === Swal.DismissReason.cancel) {
                    Swal.fire(
                        'Cancelled',
                        'Your submission has been cancelled.',
                        'info'
                    );
                }
            });
        }


        function validateAttachmentUpload(fileInput) {
            var allowedExtensions = ['png', 'jpg', 'jpeg'];
            var errorMsg = document.getElementById('attchUploadError');
            var files = fileInput.files;
            var isValid = true;

            // check if the number of files exceeds 10
            if (files.length > 10) {
                errorMsg.textContent = 'You can only upload a maximum of 10 files.';
                errorMsg.style.display = 'block';
                fileInput.value = ''; // clear the file input
                return;
            }

            for (var i = 0; i < files.length; i++) {
                var fileExtension = files[i].name.split('.').pop().toLowerCase();
                if (!allowedExtensions.includes(fileExtension)) {
                    isValid = false;
                    break;
                }
            }

            if (!isValid) {
                errorMsg.textContent = 'Only PNG, JPG, and JPEG files are allowed.';
                errorMsg.style.display = 'block';
                fileInput.value = ''; // clear the file input
            } else {
                errorMsg.style.display = 'none';
            }
        }

        function validateDonorInput() {
            var isValid = true; 

            var fullName = document.getElementById('<%= txtFullName.ClientID %>');
            var nameError = document.getElementById('nameError');

            var email = document.getElementById('<%= txtEmail.ClientID %>');
            var emailError = document.getElementById('emailError');

            var phone = document.getElementById('<%= txtPhone.ClientID %>');
            var phoneError = document.getElementById('phoneError');

            var address = document.getElementById('<%= txtAddress.ClientID %>');
            var addressError = document.getElementById('addressError');

            var region = document.getElementById('<%= pickupRegion.ClientID %>');
            var stateError = document.getElementById('stateError');

            // full name
            if (fullName.value.trim() === "") {
                nameError.textContent = "Please enter your full name.";
                nameError.style.display = "block";
                isValid = false;
            } else {
                nameError.style.display = "none";
            }

            // email
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            if (!emailPattern.test(email.value.trim())) {
                emailError.textContent = "Please enter a valid email address.";
                emailError.style.display = "block";
                isValid = false;
            } else {
                emailError.style.display = "none";
            }

            // phone number
            if (phone.value.trim() === "") {
                phoneError.textContent = "Please enter your phone number.";
                phoneError.style.display = "block";
                isValid = false;
            } else {
                phoneError.style.display = "none";
            }

            // address 
            if (address.value.trim() === "") {
                addressError.textContent = "Please enter your pick-up address.";
                addressError.style.display = "block";
                isValid = false;
            } else {
                addressError.style.display = "none";
            }

            // region 
            if (region.value === "") {
                stateError.textContent = "Please select a region in Malaysia.";
                stateError.style.display = "block";
                isValid = false;
            } else {
                stateError.style.display = "none";
            }

            return isValid; 
        }

        function updateProgressBar() {
            // get the current slide number from the hidden field
            var currentSlide = document.getElementById('<%= hfCurrentSlide.ClientID %>').value;

            currentSlide = parseInt(currentSlide);

            var steps = document.querySelectorAll('#progress_bar li');

            // loop through the steps and update their class
            steps.forEach(function (step, index) {
                if (index < currentSlide) {
                    // set previous steps to "step-done"
                    step.className = 'step-done';
                } else if (index == currentSlide) {
                    // set the current step to "step-active"
                    step.className = 'step-active';
                } else {
                    // set future steps to "step-todo"
                    step.className = 'step-todo';
                }
            });

        }

        function formatCreditCardNumber(input) {
            // remove all non-digit characters
            let inputValue = input.value.replace(/\D/g, '');

            // limit to 16 digits
            inputValue = inputValue.substring(0, 16);

            // insert a space every 4 digits
            input.value = inputValue.replace(/(\d{4})(?=\d)/g, '$1 ');
        }

        // function to allow only numeric input
        function allowOnlyNumber(event) {
            // allow backspace, delete, arrow keys
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39) {
                return true;
            }

            // only allow numbers 
            if (event.key < '0' || event.key > '9') {
                event.preventDefault();
            }
        }

        function formatExpiryDate(input) {
            let value = input.value.replace(/\D/g, ''); // remove all non-digit characters

            if (value.length >= 2) {
                let month = value.substring(0, 2); // extract the month part

                // check if the month is valid (between 01 and 12)
                if (month < '01' || month > '12') {
                   
                    document.getElementById("expiryError").style.display = "inline";
                    document.getElementById("expiryError").innerText = "Invalid month!";
                    input.value = ""; 
                    return;
                } else {
                    
                    document.getElementById("expiryError").style.display = "none";
                }

                // add a slash after the valid month
                value = month + '/' + value.substring(2, 4);
            }

            input.value = value.substring(0, 5); // set to 5 characters (MM/YY)
        }


        function validateSlide3() {
            var pickupDate = document.getElementById('<%= txtPickupDate.ClientID %>').value;
            var pickupTime = document.getElementById('<%= txtPickupTime.ClientID %>').value;
            var vehicleType = document.getElementById('<%= ddlVehicleType.ClientID %>').value;
           
            var isValid = true;

            // Pickup Date Validation
            if (pickupDate === "") {
                document.getElementById("dateError").innerHTML = "Pickup date is required.";
                document.getElementById("dateError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("dateError").style.display = "none";
            }

            // Pickup Time Validation
            if (pickupTime === "") {
                document.getElementById("timeError").innerHTML = "Pickup time is required.";
                document.getElementById("timeError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("timeError").style.display = "none";
            }

            // Vehicle Type Validation
            if (vehicleType === "") {
                document.getElementById("vehicleError").innerHTML = "Vehicle type is required.";
                document.getElementById("vehicleError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("vehicleError").style.display = "none";
            }

           

            return isValid; 
        }


        function validateSlide4() {
           
            var cardNumber = document.getElementById('<%= txtCardNumber.ClientID %>').value;
            var cardOwner = document.getElementById('<%= txtCardOwner.ClientID %>').value;
            var expirationDate = document.getElementById('<%= txtExpirationDate.ClientID %>').value;
            var cvvCode = document.getElementById('<%= txtCVVCode.ClientID %>').value;
            var emailOTP = document.getElementById('<%= txtEmailOTP.ClientID %>').value;

            var isValid = true;

            // Card Number Validation
            if (cardNumber === "" || cardNumber.length !== 19) {
                document.getElementById("cardNumberError").innerHTML = "Please enter a valid 16-digit card number.";
                document.getElementById("cardNumberError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("cardNumberError").style.display = "none";
            }

            // Card Owner Name Validation
            if (cardOwner === "") {
                document.getElementById("cardholderError").innerHTML = "Cardholder name is required.";
                document.getElementById("cardholderError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("cardholderError").style.display = "none";
            }

            // Expiration Date Validation
            if (expirationDate === "") {
                document.getElementById("expiryError").innerHTML = "Expiration date is required.";
                document.getElementById("expiryError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("expiryError").style.display = "none";
            }

            // CVV Code Validation
            if (cvvCode === "" || cvvCode.length !== 3) {
                document.getElementById("cvvError").innerHTML = "Please enter a valid 3-digit CVV code.";
                document.getElementById("cvvError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("cvvError").style.display = "none";
            }

            // Email OTP Validation
            if (emailOTP === "") {
                document.getElementById("emailOTPError").innerHTML = "Please enter an OTP.";
                document.getElementById("emailOTPError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("emailOTPError").style.display = "none";
            }

            // Email Format Validation
            var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
            if (!emailPattern.test(emailOTP.trim())) {
                document.getElementById("emailOTPError").innerHTML = "Please enter a valid email address.";
                document.getElementById("emailOTPError").style.display = "block";
                isValid = false;
            } else {
                document.getElementById("emailOTPError").style.display = "none";
            }

            return isValid;
        }


        function restrictPastDate() {
            var today = new Date();
            today.setDate(today.getDate() + 1); // move the date to tomorrow

            var day = today.getDate();
            var month = today.getMonth() + 1; // months are zero-based
            var year = today.getFullYear();

            if (day < 10) {
                day = '0' + day;
            }
            if (month < 10) {
                month = '0' + month;
            }

            // set the minimum date for the date picker to tomorrow
            var minDate = year + '-' + month + '-' + day;
            document.getElementById('<%= txtPickupDate.ClientID %>').setAttribute('min', minDate);
        }

        
        

    </script>

         <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBHd69lOb31ywFRMu99sos-ysgl-uCtidY&callback=initMap&libraries=places"></script>
 
      

</asp:Content>
