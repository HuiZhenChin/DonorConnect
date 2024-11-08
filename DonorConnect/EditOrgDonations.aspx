<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="EditOrgDonations.aspx.cs" Inherits="DonorConnect.EditOrgDonations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>Edit Donations</title>
     
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <style>
         body{
             background: rgb(249,247,247);
             background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
         }

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
        <div class="card" style=" background-color: rgba(255, 255, 255, 0.8);">
            <div class="card-header bg-primary text-white" style="background-color: #1F4E5F!important;">
                <h4 class="mb-0">Publish New Item Donations</h4>
            </div>
            <div class="card-body">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" HeaderText="Please correct the following errors:" />

                <div class="form-group" id="reason" style="display:none;" runat="server">
                    <label for="txtReason">Rejection Reason</label>
                    <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" ReadOnly="true"/>
                    
                </div>

                <div class="form-group">
                    <label for="urgentRadioGroup">Is your item donation urgent?</label>
                    <div>
                        <asp:RadioButton ID="rbUrgentYes" runat="server" CssClass="form-check-input" GroupName="urgent" Value="Yes" Enabled="false"/>
                        <label for="rbUrgentYes" class="form-check-label" style="margin-left: 10px;">Yes</label>
                    </div>
                    <div>
                        <asp:RadioButton ID="rbUrgentNo" runat="server" CssClass="form-check-input" GroupName="urgent" Value="No" Checked="True" Enabled="false"/>
                        <label for="rbUrgentNo" class="form-check-label" style="margin-left: 10px;">No</label>
                    </div>
                    <small class="form-text text-muted">
                        <em>This section cannot be modified.</em> 
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
                    <label for="txtName">Recipient Name</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter recipient name (enter your username if it is donated to your organization)" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Address is required." CssClass="text-danger" />
                </div>
                <div class="form-group">
                    <label for="txtPhone">Recipient Phone Number</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Enter recipient phone number" />
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone number is required." CssClass="text-danger" />
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
                        <asp:Repeater ID="rptCategories" runat="server">
                            <ItemTemplate>
                                <div class="category-checkbox">
                                    <asp:CheckBox ID="chkCategory" runat="server" Text='<%# Eval("categoryName") %>' />
                                    <asp:TextBox ID="txtSpecificItem" runat="server" CssClass="form-control specific-items-input" Placeholder="Specify items needed" Style="display: none;" />
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control specific-qty-input" Placeholder="Enter quantity" TextMode="Number" Style="display: none;" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                    </div>
                    
                     <label id="lblCategory" style="display:none; color: darkred;" runat="server">Please select at least one category needed by your organization, helping donors to understand your needs.</label>

                </div>
                
                <div class="form-group" id="timeRange" style="display: none;">
                    <label for="txtTimeRange">Time Range</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtTimeRange" runat="server" CssClass="form-control" TextMode="Number" placeholder="Enter time range (e.g., 7/ 14/ 21 days)" />
                        <div class="input-group-append">
                            <span class="input-group-text">Days</span>
                        </div>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTimeRange" runat="server" ControlToValidate="txtTimeRange" ErrorMessage="Time range of item donations is required." CssClass="text-danger" Enabled="false" />
                </div>

                <div class="form-group">
                    <label for="txtRestrictions">Restrictions of Donated Items</label>
                    <asp:TextBox ID="txtRestrictions" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter any restrictions for the donated items (e.g., no expired food, only new clothing)" />
                </div>

                <div class="form-group">
                    <label for="donationImg">Upload Donation Image/ Poster (if any)</label>
                    <asp:FileUpload ID="donationImg" runat="server" CssClass="form-control-file" AllowMultiple="true" OnChange="confirmImageUpload(this)" />
                    <small class="form-text text-muted">
                        <em>Accepted Formats: .jpg, .jpeg, .png. Maximum 5 images.</em>
                    </small>
                    <small class="form-text text-muted">
                        <em>Please upload the images again if you need them to display later. Or else, the system will not display any images.</em>
                    </small>
                    <span id="imageUploadError" class="text-danger" style="display: none;">You can upload a maximum of 5 images in .jpg, .jpeg, .png formats.</span>
                </div>
                <asp:Literal ID="imagesContainer" runat="server"></asp:Literal>

                <div class="form-group">
                    <label for="donationFile">Upload Donation File Attachment (if any)</label>
                    <asp:FileUpload ID="donationFile" runat="server" CssClass="form-control-file" AllowMultiple="true" OnChange="confirmFileUpload(this)" />
                    <small class="form-text text-muted">
                        <em>Accepted Formats: .pdf, .docx</em>
                    </small>
                    <small class="form-text text-muted">
                        <em>Please upload the files again if you need them to display later. Or else, the system will not display any files.</em>
                    </small>
                    <span id="fileUploadError" class="text-danger" style="display: none;">Only .pdf and .docx files are allowed.</span>
                </div>
                <asp:Literal ID="filesContainer" runat="server"></asp:Literal>

                <div class="text-right">
                    <asp:Button ID="btnUpdate" runat="server" type="submit" CssClass="btn btn-success" Text="Update Donation"  onClick="btnUpdateDonation_Click" Visible="false"/>
                     <asp:Button ID="btnCancel" runat="server" type="submit" CssClass="btn btn-danger" Text="Cancel Application"  onClick="btnCancelDonation_Click" Visible="false" style=" float: left;"/>
                     <asp:Button ID="btnResubmit" runat="server" type="submit" CssClass="btn btn-success" Text="Resubmit Application"  onClick="btnResubmitDonation_Click" Visible="false"/>
                    <asp:Button ID="btnDiscard" runat="server" CssClass="btn btn-secondary" Text="Discard" onClick="btnDiscardDonation_Click" style=" float: left;"/>
                     <asp:Button ID="btnRepublish" runat="server" type="submit" CssClass="btn btn-success" Text="Publish" Visible="false" onClick="btnRepublish_Click" style=" float: right;"/>
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

        function showSuccess2(message) {
            Swal.fire({
                title: 'Success!',
                text: message,
                icon: 'success',
                confirmButtonText: 'OK'
            }).then((result) => {
                if (result.isConfirmed) {              
                    window.location.href = 'OrgDonations.aspx'; 
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

        function validateFileUpload(fileInput) {
            var maxFiles = 5;
            var errorMsg = document.getElementById('fileUploadError');

            if (fileInput.files.length > maxFiles) {
                errorMsg.style.display = 'block';
                fileInput.value = ''; 
            } else {
                errorMsg.style.display = 'none';
            }
        }

        function validateImageUpload(fileInput) {
            var allowedExtensions = ['jpg', 'jpeg', 'png'];
            var errorMsg = document.getElementById('imageUploadError');
            var files = fileInput.files;
            var isValid = true;

            if (files.length > 5) {
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

        function validateFileAttachmentUpload(fileInput) {
            var allowedExtensions = ['pdf', 'docx'];
            var errorMsg = document.getElementById('fileUploadError');
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

        function confirmImageUpload(fileInput) {
            if (fileInput.files.length > 0) {
                Swal.fire({
                    title: 'Are you sure?',
                    text: "Uploading new images will replace your existing images.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#008000',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, replace them'
                }).then((result) => {
                    if (result.isConfirmed) {
                        validateImageUpload(fileInput);
                    } else {
                        fileInput.value = ''; 
                    }
                });
            }
        }

        function confirmFileUpload(fileInput) {
            if (fileInput.files.length > 0) {
                Swal.fire({
                    title: 'Are you sure?',
                    text: "Uploading new files will replace your existing files.",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#008000',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, replace them'
                }).then((result) => {
                    if (result.isConfirmed) {
                        validateFileAttachmentUpload(fileInput);
                    } else {
                        fileInput.value = ''; 
                    }
                });
            }
        }

        function confirmCancelDonation(donationId) {
            Swal.fire({
                title: 'Are you sure?',
                text: "Do you want to cancel the donation? By cancelling it, the record will be removed from the system.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, cancel it!',
                cancelButtonText: 'No, keep it'
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack('CancelDonationConfirmed', donationId);
                }
            });
        }

    </script>
</asp:Content>

