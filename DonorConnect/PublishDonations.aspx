<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PublishDonations.aspx.cs" Inherits="DonorConnect.PublishDonations1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>Publish Donations</title>
     <%--<link href="/Content/MyAccount.css" rel="stylesheet" type="text/css" />--%>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h4 class="mb-0">Publish New Item Donations</h4>
            </div>
            <div class="card-body">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" HeaderText="Please correct the following errors:" />
                
                <div class="form-group">
                    <label for="urgentRadioGroup">Is your item donation urgent?</label>
                    <div>
                        <asp:RadioButton ID="rbUrgentYes" runat="server" CssClass="form-check-input" GroupName="urgent" Value="Yes"/>
                        <label for="rbUrgentYes" class="form-check-label" style="margin-left: 10px;">Yes</label>
                    </div>
                    <div>
                        <asp:RadioButton ID="rbUrgentNo" runat="server" CssClass="form-check-input" GroupName="urgent" Value="No" Checked="True" />
                        <label for="rbUrgentNo" class="form-check-label" style="margin-left: 10px;">No</label>
                    </div>
                    <small class="form-text text-muted">
                        <em>Urgent:</em> Select "Yes" if the donation is needed urgently and requires immediate attention. Otherwise, select "No".
                    </small>
                </div>
                
                <div class="form-group">
                    <label for="txtTitle">Item Donation Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Enter donation title" />
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="Title is required." CssClass="text-danger" />
                </div>
                
                <div class="form-group">
                    <label for="txtQuantity">Number of People in Need</label>
                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Enter number of people in need (e.g. a range of 30)" />
                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Number of people in need is required." CssClass="text-danger" />
                </div>
                
                <div class="form-group">
                    <label for="txtAddress">Recipient Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Enter recipient address (enter your username if it is donated to your organization, else enter the new address)" />
                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required." CssClass="text-danger" />
                </div>

                <div class="form-group">
                    <label for="txtRegion">Recipient State/ Region</label>
                    <asp:DropDownList ID="txtRegion" CssClass="form-control form-control-lg" runat="server" style="font-size: 1rem;">
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
                    <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ControlToValidate="txtRegion" ErrorMessage="Region is required." CssClass="text-danger" />
                </div>
                <div class="form-group">
                    <label for="txtDescription">Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Enter donation description (add more information for donors' understanding about the item donations needed by your organization)" />
                    <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Description is required." CssClass="text-danger" />
                </div>
                
                <div class="form-group">
                    <label>Item Categories</label>
                    <div class="category-row">
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkFood" runat="server" Text="Food"/>
                            <asp:TextBox ID="txtSpecificFood" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyFood" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity" TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkClothing" runat="server" Text="Clothing" />
                            <asp:TextBox ID="txtSpecificClothing" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyClothing" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity" TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkToys" runat="server" Text="Toys"  />
                            <asp:TextBox ID="txtSpecificToys" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyToys" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity"  TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkBooks" runat="server" Text="Books"  />
                            <asp:TextBox ID="txtSpecificBooks" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyBooks" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity"  TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkElectronics" runat="server" Text="Electronics"  />
                            <asp:TextBox ID="txtSpecificElectronics" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyElectronics" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity"  TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkFurniture" runat="server" Text="Furniture"  />
                            <asp:TextBox ID="txtSpecificFurniture" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyFurniture" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity" TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkHygiene" runat="server" Text="Hygiene Products"  />
                            <asp:TextBox ID="txtSpecificHygiene" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyHygiene" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity"  TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkMedical" runat="server" Text="Medical Supplies"  />
                            <asp:TextBox ID="txtSpecificMedical" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyMedical" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity" TextMode="Number" style="display:none;" />
                        </div>
                        <div class="category-checkbox">
                            <asp:CheckBox ID="chkOther" runat="server" Text="Other"  />
                            <asp:TextBox ID="newCategory" runat="server" CssClass="form-control specific-items-input" Placeholder="Enter new category" style="display:none;" />
                            <asp:TextBox ID="txtSpecificOther" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" style="display:none;" />
                            <asp:TextBox ID="qtyOther" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity"  TextMode="Number" style="display:none;" />
                        </div>
                    </div>
                    
                     <label id="lblCategory" style="display:none; color: darkred;" runat="server">Please select at least one category needed by your organization, helping donors to understand your needs.</label>

                </div>
                
                <div class="form-group" id="timeRange" style="display:none;">
                    <label for="txtTimeRange">Time Range</label>
                    <asp:TextBox ID="txtTimeRange" runat="server" CssClass="form-control" placeholder="Enter time range (e.g., 2 weeks or NONE for no time limitations)" />
                    <asp:RequiredFieldValidator ID="rfvTimeRange" runat="server" ControlToValidate="txtTimeRange" ErrorMessage="Time range of item donations is required." CssClass="text-danger" Enabled="false" />
                </div>

                <div class="form-group">
                    <label for="txtRestrictions">Restrictions of Donated Items</label>
                    <asp:TextBox ID="txtRestrictions" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter any restrictions for the donated items (e.g., no expired food, only new clothing)" />
                </div>
                
                <div class="form-group">
                    <label for="donationImg">Upload Donation Image/ Poster (if any)</label>
                    <asp:FileUpload ID="donationImg" runat="server" CssClass="form-control-file" AllowMultiple="true"/>
                    <small class="form-text text-muted">
                        <em>Accepted Formats: .jpg, .jpeg, .png</em>
                    </small>
                </div>

                <div class="form-group">
                    <label for="donationFile">Upload Donation File Attachment (if any)</label>
                    <asp:FileUpload ID="donationFile" runat="server" CssClass="form-control-file" AllowMultiple="true" />
                    <small class="form-text text-muted">
                        <em>Accepted Formats: .jpg, .jpeg, .png</em>
                    </small>
                </div>

 
                
                <div class="text-right">
                    <asp:Button ID="btnSubmit" runat="server" type="submit" CssClass="btn btn-success" Text="Submit Donation" OnClick="btnSubmitNewDonation_Click" />
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-secondary" Text="Cancel" />
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var urgentYes = document.getElementById('<%= rbUrgentYes.ClientID %>');
           var urgentNo = document.getElementById('<%= rbUrgentNo.ClientID %>');
    var timeRange = document.getElementById('timeRange');
           var rfvTimeRange = document.getElementById('<%= rfvTimeRange.ClientID %>');
           var categories = document.querySelectorAll('.category-checkbox');
           var lblCategory = document.getElementById('lblCategory');

           function toggleUrgentFields() {
               if (urgentYes.checked) {
                   timeRange.style.display = 'block';
                   rfvTimeRange.style.display = 'block';
                   rfvTimeRange.enabled = true; 
               } else {
                   timeRange.style.display = 'none';
                   rfvTimeRange.style.display = 'none';
                   rfvTimeRange.enabled = false; 
               }
           }

           function toggleSpecificFields() {
               categories.forEach(function (category) {
                   var checkbox = category.querySelector('input[type="checkbox"]');
                   var specificInputs = category.querySelectorAll('.specific-items-input');
                   var qtyInputs = category.querySelectorAll('.specific-qty-input');
                   if (checkbox.checked) {
                       specificInputs.forEach(function (input) {
                           input.style.display = 'block';
                       });
                       if (urgentYes.checked) {
                           qtyInputs.forEach(function (input) {
                               input.style.display = 'block';
                           });
                       }
                   } else {
                       specificInputs.forEach(function (input) {
                           input.style.display = 'none';
                       });
                       qtyInputs.forEach(function (input) {
                           input.style.display = 'none';
                       });
                   }
                   checkbox.addEventListener('change', function () {
                       if (checkbox.checked) {
                           specificInputs.forEach(function (input) {
                               input.style.display = 'block';
                           });
                           if (urgentYes.checked) {
                               qtyInputs.forEach(function (input) {
                                   input.style.display = 'block';
                               });
                           }
                       } else {
                           specificInputs.forEach(function (input) {
                               input.style.display = 'none';
                           });
                           qtyInputs.forEach(function (input) {
                               input.style.display = 'none';
                           });
                       }
                   });
               });
           }

           urgentYes.addEventListener('change', toggleUrgentFields);
           urgentNo.addEventListener('change', toggleUrgentFields);
           toggleUrgentFields();
           toggleSpecificFields();

          
       });

        function showSuccess(message) {
            Swal.fire({
                title: 'Success!',
                text: message,
                icon: 'success',
                confirmButtonText: 'OK'
            });
        }
    </script>
</asp:Content>
