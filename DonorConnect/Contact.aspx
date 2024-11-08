<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="DonorConnect.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Contact Us</title>
    <link href="/Content/MyAccount.css" rel="stylesheet" type="text/css" />
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Itim&family=Istok+Web:wght@400;700&display=swap" rel="stylesheet">

    <style>
        body {
            background: rgb(243,238,234);
            background: linear-gradient(180deg, rgba(243,238,234,1) 5%, rgba(243,238,234,1) 15%, rgba(235,227,213,1) 44%, rgba(235,227,213,1) 54%, rgba(176,166,149,1) 72%, rgba(119,107,93,1) 95%);
            background-size: cover;
        }
        
        .section-header{
            border: none;
        }

        .section-content{
            background-color: #eff5f7;
        }

        .form-control{
            border: solid 1px black!important;
            box-shadow: 2px 2px 8px rgba(0, 0, 0, 0.2);
            opacity: 0.8;
            color: black!important;
           
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">

        <div class="contact-us-header" style="background-image: url('/Image/contact.jpg'); background-size: cover; background-position: center; padding: 40px; border-radius: 10px;">
            <h2 style="text-align: center; font-family: 'Istok Web'; text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5); font-weight: 700; color: white;">Contact Us</h2>
        </div>

        <div class="row mb-3">
            <div class="col-md-6" id="fullName" runat="server">
                <asp:Label runat="server" Text="Full Name" AssociatedControlID="txtFullName" />
                <asp:TextBox runat="server" ID="txtFullName" CssClass="form-control" Tooltip="Full Name cannot be modified." Placeholder="Enter your name"/>
                <asp:Label ID="lblErrorFullName" runat="server" Text="Full Name is required." CssClass="text-danger" Visible="false"/>
            </div>
            <div class="col-md-6" id="username" runat="server">
                <asp:Label runat="server" Text="Username" AssociatedControlID="txtUsername" />
                <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" Tooltip="Username cannot be modified."/>
              
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-6">
                <asp:Label runat="server" Text="Email Address" AssociatedControlID="txtEmail" />
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" Tooltip="Email cannot be modified." TextMode="Email" Placeholder="Enter your active email address"/>
                <asp:Label ID="lblErrorEmail" runat="server" Text="Email Address is required." CssClass="text-danger" Visible="false"/>
                
            </div>
            <div class="col-md-6">
                <asp:Label runat="server" Text="Phone Number" AssociatedControlID="txtPhoneNumber" />
                <asp:TextBox runat="server" ID="txtPhoneNumber" CssClass="form-control" Placeholder="Enter your active phone number"/>
                <asp:Label ID="lblErrorPhoneNumber" runat="server" Text="Phone Number is required." CssClass="text-danger" Visible="false"/>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-12">
                <asp:Label runat="server" Text="Organization Name" AssociatedControlID="txtOrgName" />
                <small class="form-text text-muted"><em style="color: #543310;">(If you are representing your company, foundation, or organization)</em></small>
                <asp:TextBox runat="server" ID="txtOrgName" CssClass="form-control" Placeholder="Enter your company name"/>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-12">
                <asp:Label runat="server" Text="Message (Feedback/ Problem Faced)" AssociatedControlID="txtMessage" />
                <asp:TextBox runat="server" ID="txtMessage" TextMode="MultiLine" CssClass="form-control" Rows="5" Placeholder="Enter message"/>
                <asp:Label ID="lblErrorMsg" runat="server" Text="Message is required." CssClass="text-danger" Visible="false"/>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-12">
                <asp:Label runat="server" Text="Attach File" AssociatedControlID="fileAttachment" />
                <asp:FileUpload runat="server" ID="fileAttachment" CssClass="form-control" AllowMultiple="true" OnChange="validateAttachmentUpload(this)"/>
                <small class="form-text text-muted">
                    <em style="color: #543310;">Accepted Formats: .jpg, .jpeg, .png, .pdf Maximum 5 attachments.</em>
                </small>
                <span id="attchUploadError" class="text-danger" style="display: none;">You have uploaded files in not acceptable format or more than 5 attachments. Note that you can upload maximum 5 attachments in .jpg, .jpeg, .png or .pdf formats.</span>
            </div>
        </div>

        <div class="row">
        <div class="col-md-12 text-center">
            <div class="d-flex justify-content-between align-items-center">
               
                <!-- Clear Form -->
                <asp:LinkButton class="btn btn-link clear-form-link" runat="server" OnClientClick="resetForm(this)" Text="Clear Form" style="color: #1d2129;"></asp:LinkButton>
                 <!-- Submit Button -->
                <asp:Button runat="server" ID="btnSubmit" style="background-color: #1d2129; border: none!important;" Text="Submit" CssClass="btn btn-primary btn-lg btn-submit" OnClick="btnSubmit_Click" />
            
            </div>
        </div>
    </div>


    </div>


    <script>
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

        function resetForm() {
            // reset all form fields to their original states
            document.forms[0].reset();

        
        }


    </script>

</asp:Content>
