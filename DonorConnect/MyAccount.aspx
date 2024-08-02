<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="DonorConnect.MyAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>My Account</title>
    <link href="/Content/MyAccount.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="container">
         <!-- Profile Picture Section -->
        <div class="profile-pic" id="profilePic" runat="server" onclick="triggerFileUpload()" style="background-color: #f5f5f5; padding: 20px;">
            <label class="-label" for="fileUpload" style="cursor: pointer;">
                 <i class="fas fa-camera"></i>
                <span>Change Image</span>
            </label>
            <asp:FileUpload ID="fileUpload" runat="server" CssClass="d-none" OnChange="loadFile(event)"/>
            <asp:Image ID="output" runat="server" ImageUrl="/Image/default_picture.jpg" Width="165px" Height="165px" />
        </div>
        <!-- Display Username -->
       <div style="background-color: #f5f5f5; padding: 20px; text-align:center;">
        <asp:Label runat="server" ID="profileUsername" Font-Bold="true" style="font-size: 22px;"/>

       </div>
        <!-- Buttons for Profile Picture Updates -->
        <div id="buttons" runat="server" style="display:none; text-align: right; margin-top: 10px;">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
        </div>


        <!-- User Information Section -->
        <div class="section-header" id="userInfoHeader" style="margin-top: 20px;">
            <h5>User Information</h5>
        </div>
    <!-- Hidden input to store the selected role -->
    <asp:TextBox runat="server" ID="selectedRole" CssClass="form-control" style="display:none;"/>

        <div class="section-content" id="donorContent" runat="server">
            <asp:Panel runat="server">
                <div class="form-row">
                    <asp:Label runat="server" ID="lblUsername" Text="Username" />
                    <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" ReadOnly="true" ToolTip="Username cannot be modified"/>
                </div>
                <div class="form-row">
                    <asp:Label runat="server" ID="lblFullName" Text="Full Name" />
                    <asp:TextBox runat="server" ID="txtFullName" CssClass="form-control" />
                </div>
                <div class="form-row">
                    <asp:Label runat="server" ID="lblEmail" Text="Email Address" />
                    <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" TextMode="Email" />
                </div>
                <div class="form-row">
                    <asp:Label runat="server" ID="lblPhone" Text="Phone Number" />
                    <asp:TextBox runat="server" ID="txtPhone" CssClass="form-control"/>
                </div>
                <div class="form-row">
                    <asp:Label runat="server" ID="lblAddress" Text="Delivery Address" />
                    <asp:TextBox runat="server" ID="txtAddress" CssClass="form-control"/>
                </div>
                <div class="form-row">
                    <asp:Button runat="server" ID="btnSaveDonorInfo" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveDonorInfo_Click"/>
                    <asp:Button runat="server" ID="btnCancelDonorInfo" CssClass="btn btn-secondary" Text="Cancel" CausesValidation="False" style="margin-left: 10px;"/>
                </div>
            </asp:Panel>
        </div>

    <div class="section-content" id="orgContent" runat="server">
        <asp:Panel runat="server">
            <div class="form-row">
                <asp:Label runat="server" ID="lblOrgName" Text="Organization Name" />
                <asp:TextBox runat="server" ID="txtOrgName" CssClass="form-control" ReadOnly="true" ToolTip="Organization name cannot be modified"/>
            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="Label2" Text="Organization Email Address" />
                <asp:TextBox runat="server" ID="txtOrgEmail" CssClass="form-control" TextMode="Email" />
               
            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="Label3" Text="Organization Contact Number" />
                <asp:TextBox runat="server" ID="txtOrgNumber" CssClass="form-control" />
             
            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="lblPicName" Text="Person-In-Charge's Name" />
                <asp:TextBox runat="server" ID="txtPicName" CssClass="form-control" />
            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="lblPicEmail" Text="Person-In-Charge's Email Address" />
                <asp:TextBox runat="server" ID="txtPicEmail" CssClass="form-control" />

            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="lblPicNumber" Text="Person-In-Charge's Contact Number" />
                <asp:TextBox runat="server" ID="txtPicNumber" CssClass="form-control" />

            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="lblOrgAddress" Text="Organization Address" />
                <asp:TextBox runat="server" ID="txtOrgAddress" CssClass="form-control" /> 
            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="lblOrgRegion" Text="Region" />
                <asp:DropDownList ID="orgRegion" CssClass="form-control form-control-lg" runat="server" Style="font-size: 1rem;">
                    <asp:ListItem Text="Select Region in Malaysia" Value="" Disabled="true" />
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
            <div class="form-row">
                <asp:Label runat="server" ID="lblOrgDesc" Text="Description" />
                <asp:TextBox runat="server" ID="txtDesc" CssClass="form-control" />
            </div>
            <div class="form-row">
                <asp:Label runat="server" ID="lblCategory" Text="Most Needed Item Category" />
                <asp:TextBox runat="server" ID="txtCategory" CssClass="form-control" />
            </div>
            <div class="form-row">
                <asp:Button runat="server" ID="btnSaveOrgInfo" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveOrgInfo_Click"/>
                <asp:Button runat="server" ID="btnCancelOrgInfo" CssClass="btn btn-secondary" Text="Cancel" CausesValidation="False" style="margin-left: 10px;"/>
            </div>
        </asp:Panel>
    </div>
        <div class="section-content" id="riderContent" runat="server">
    <asp:Panel runat="server">
        <div class="form-row">
            <asp:Label runat="server" ID="lblRiderName" Text="Username" />
            <asp:TextBox runat="server" ID="txtRiderUsername" CssClass="form-control" ReadOnly="true" ToolTip="Username cannot be modified"/>
        </div>
        <div class="form-row">
            <asp:Label runat="server" ID="lblRiderFullName" Text="Full Name" />
            <asp:TextBox runat="server" ID="txtRiderFullName" CssClass="form-control" />
        </div>
        <div class="form-row">
            <asp:Label runat="server" ID="lblRiderEmail" Text="Email Address" />
            <asp:TextBox runat="server" ID="txtRiderEmail" CssClass="form-control" TextMode="email"/> 
        </div>
        <div class="form-row">
            <asp:Label runat="server" ID="lblRiderNumber" Text="Contact Number" />
            <asp:TextBox runat="server" ID="txtRiderNumber" CssClass="form-control" />
        </div>
        <div class="form-row">
            <asp:Label runat="server" ID="lblVehicle" Text="Vehicle Type" />
            <asp:DropDownList ID="vehicleType" CssClass="form-control form-control-lg" runat="server" Style="font-size: 1rem;">
                <asp:ListItem Text="Select Vehicle Type" Value="" Disabled="true" Selected="true"></asp:ListItem>
                <asp:ListItem Text="Car" Value="Car"></asp:ListItem>
                <asp:ListItem Text="Truck" Value="Truck"></asp:ListItem>
                <asp:ListItem Text="Van" Value="Van"></asp:ListItem>
                <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-row">
            <asp:Label runat="server" ID="lblPlateNo" Text="Vehicle Plate Number" />
            <asp:TextBox runat="server" ID="txtPlateNo" CssClass="form-control" />
        </div>
        <div class="form-row">
            <asp:Button runat="server" ID="btnSaveRiderInfo" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveRiderInfo_Click"/>
            <asp:Button runat="server" ID="btnCancelRiderInfo" CssClass="btn btn-secondary" Text="Cancel" CausesValidation="False" style="margin-left: 10px;"/>
        </div>
    </asp:Panel>
</div>

        <!-- Notifications Section -->
        <div class="section-header" id="notificationsHeader">
            <h5>Notifications</h5>
        </div>
        <div class="section-content" id="notificationsContent" runat="server">
            <asp:Panel runat="server">
                <div class="form-row">
                    <asp:CheckBox runat="server" ID="chkEmailNotifications" CssClass="form-check-input" />
                    <asp:Label runat="server" AssociatedControlID="chkEmailNotifications" Text="Email Notifications" CssClass="form-check-label" />
                </div>
                <div class="form-row">
                    <asp:CheckBox runat="server" ID="chkSmsNotifications" CssClass="form-check-input" />
                    <asp:Label runat="server" AssociatedControlID="chkSmsNotifications" Text="SMS Notifications" CssClass="form-check-label" />
                </div>
                <div class="form-row">
                    <asp:Button runat="server" ID="btnSaveNotifications" CssClass="btn btn-primary" Text="Save"/>
                    <asp:Button runat="server" ID="btnCancelNotifications" CssClass="btn btn-secondary" Text="Cancel" CausesValidation="False" style="margin-left: 10px;"/>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script>
        $(document).ready(function () {
            // toggle content sections based on role
            var selectedRole = $('#<%= selectedRole.ClientID %>').val();
            if (selectedRole === "donor") {
                $('#<%= donorContent.ClientID %>').show();
                $('#<%= orgContent.ClientID %>').hide();
                $('#<%= riderContent.ClientID %>').hide();
            } else if (selectedRole === "organization") {
                $('#<%= donorContent.ClientID %>').hide();
                $('#<%= orgContent.ClientID %>').show();
                $('#<%= riderContent.ClientID %>').hide();
            } else if (selectedRole === "rider") {
                $('#<%= donorContent.ClientID %>').hide();
                $('#<%= orgContent.ClientID %>').hide();
                $('#<%= riderContent.ClientID %>').show();
            }

            $('#userInfoHeader').click(function () {
                if (selectedRole === "donor") {
                    $('#<%= donorContent.ClientID %>').toggle();
                } else if (selectedRole === "organization") {
                    $('#<%= orgContent.ClientID %>').toggle();
                } else if (selectedRole === "rider") {
                    $('#<%= riderContent.ClientID %>').toggle();
                }
            });

            $('#notificationsHeader').click(function () {
                $('#<%= notificationsContent.ClientID %>').toggle();
            });
        });


    </script>

    <script>
        function showSuccess(message) {
            Swal.fire({
                title: 'Success!',
                text: message,
                icon: 'success',
                confirmButtonText: 'OK'
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

        // trigger file upload from device
        function triggerFileUpload() {
            document.getElementById('<%= fileUpload.ClientID %>').click();
       }

       // load uploaded profile picture
        function loadFile(event) {
            var output = document.getElementById('<%= output.ClientID %>');
            var reader = new FileReader();
            reader.onload = function () {
                output.src = reader.result;
                document.getElementById('<%= buttons.ClientID %>').style.display = 'block';
            };
            reader.readAsDataURL(event.target.files[0]);
        }

    </script>


</asp:Content>

