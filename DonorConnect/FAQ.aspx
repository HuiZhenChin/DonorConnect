<%@ Page Title="FAQ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="DonorConnect.ContactUs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>FAQ</title>
    <link href="/Content/MyAccount.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Itim&family=Istok+Web:wght@400;700&display=swap" rel="stylesheet">

    <style>
        body{
            background: rgb(21,140,168);
            background: linear-gradient(180deg, rgba(21,140,168,1) 0%, rgba(196,227,235,1) 30%, rgba(195,227,190,1) 56%, rgba(44,103,78,1) 100%);
            background-size: cover; 
        }

        .section-header{
            border: none;
        }

        .section-content{
            background-color: #eff5f7;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-4">
        
        <!-- FAQ Section -->
        <div class="faq-header" style="background-image: url('/Image/mountain.jpg'); background-size: cover; background-position: center; padding: 40px; border-radius: 10px;">
            <h2 style="text-align: center; font-family: 'Istok Web'; text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.5); font-weight: 700; color: white;">Frequently Asked Questions (FAQ)</h2>
        </div>

        <div class="faq-section">
            <h4 style="font-size: 22px; padding-top: 10px;">General Questions</h4>
            <div class="section-header" id="q1" style="margin-top: 20px; background: #355d87" onclick="toggleAnswer('q1Content')">
                <h5 style="font-size: 18px; color: whitesmoke;">Why use DonorConnect?</h5>
            </div>
            <div class="section-content" id="q1Content" style="display: none;">
                <p>DonorConnect is the platform that connects donors with charity organizations. It makes the process of item donations and delivery easier by applying the concept of having delivery riders to deliver donated items to the destinations. This approach is time-saving and streamlines the process of requesting for donations and item delivery later.</p>
            </div>

            <div class="section-header" id="q17" style="margin-top: 20px; background: #355d87" onclick="toggleAnswer('q17Content')">
                <h5 style="font-size: 18px; color: whitesmoke;">Forgot Password</h5>
            </div>
            <div class="section-content" id="q17Content" style="display: none;">
                <p>You may reset your password at the Login Password by pressing the <b>"Forgot Password?" </b>link.</p>
            </div>
            </div>

        <div class="faq-section">
            <h4 style="font-size: 22px;">Donors</h4>

            <div class="section-header" id="q2" style="margin-top: 20px; background: #7aa3bf" onclick="toggleAnswer('q2Content')">
                <h5 style="font-size: 18px;">How do I register as a donor?</h5>
            </div>
            <div class="section-content" id="q2Content" style="display: none;">
                <p>You can register as a donor by visiting the Sign Up page and selecting the <b>"Donor" </b>option. Complete the required information and submit your details to get started of your donation journey.</p>
            </div>

            <div class="section-header" id="q3" style="margin-top: 20px; background: #7aa3bf" onclick="toggleAnswer('q3Content')">
                <h5 style="font-size: 18px;">How can I donate?</h5>
            </div>
            <div class="section-content" id="q3Content" style="display: none;">
                <p>You may visit the <b>"Donations" </b>page to look for the available donations. You may also search and filter the donations by <b>locations and item category</b>, specifically item type. After that, you can choose the donations you prefer and press the <b>"Donate" </b>button to fill up the donation request form, or save it in your favourite list if you would like to donate later.</p>
                <p>Once you confirmed to donate, you are required to fill up a <b>request form </b>and it will be sent to the organization for approval. It prevents wastage of items if they already have enough items. The donation approval will be informed through <b>email notifications.</b></p>
            </div>


            <div class="section-header" id="q4" style="margin-top: 20px; background: #7aa3bf" onclick="toggleAnswer('q4Content')">
                <h5 style="font-size: 18px;">How can I track my donations?</h5>
            </div>
            <div class="section-content" id="q4Content" style="display: none;">
                <p>Once you are logged in, you can track your donations by navigating to the <b>"My Donations" </b>page, where you will find detailed information about your donation status. Alternatively, you can also view the delivery status at the Dashboard.</p>
            </div>
        </div>

        <div class="faq-section">
            <h4 style="font-size: 22px;">Organizations</h4>
            <div class="section-header" id="q5" style="margin-top: 20px; background: #91bba9" onclick="toggleAnswer('q5Content')">
                <h5 style="font-size: 18px;">How do I register as an organization?</h5>
            </div>
            <div class="section-content" id="q5Content" style="display: none;">
                <p>You can register as an organization by visiting the Sign Up page and selecting the <b>"Organization" </b>option. Complete the required information and submit the needed documents and your application will be sent to our team for approval. You will be informed through email notifications about the status of your application.</p>
            </div>

            <div class="section-header" id="q6" style="margin-top: 20px; background: #91bba9" onclick="toggleAnswer('q6Content')">
                <h5 style="font-size: 18px;">What to do if my registration application is rejected?</h5>
            </div>
            <div class="section-content" id="q6Content" style="display: none;">
                <p>You may press the link sent to your email address and resubmit your application again based on the rejected reason provided.</p>
            </div>

            <div class="section-header" id="q7" style="margin-top: 20px; background: #91bba9" onclick="toggleAnswer('q7Content')">
                <h5 style="font-size: 18px;">How can I publish a new donation?</h5>
            </div>
            <div class="section-content" id="q7Content" style="display: none;">
                <p>You may visit the <b>"Manage Donations" </b>page to view the created donations or create a new donation. </p>
                <p>To create a new donation, you need to choose whether the donation is urgent or long-term based, as different settings will be made based on the selection. Complete all the required information and choose the <b>item category </b>followed by <b>specific items </b>needed for each category (if any) to provide a better understandings to the donors. Later, your donation will be sent to our team for approval.</p>
                <p>Noted that if it is an <b>urgent donation</b>, you need to choose a time range of when your organization needs the items and the system will set a countdown timer to inform donors about the donations. If the time is over and you still need more items, you can republish the particular donation again.</p>
            </div>

            <div class="section-header" id="q8" style="margin-top: 20px; background: #91bba9" onclick="toggleAnswer('q8Content')">
                <h5 style="font-size: 18px;">What to do if my donation is rejected?</h5>
            </div>
            <div class="section-content" id="q8Content" style="display: none;">
                <p>If your donation is rejected by our team, you may follow the guidelines sent to your email address to resubmit the new donation publish again.</p>
                <p>1. You may visit the <b>"Manage Donations" </b>page and filter the donations by status <b>"Rejected"</b> to choose the donation and modify the details by reading the rejected reason given to resubmit again.</p>
                <p>2. You may press the <b>"Notifications"</b> icon and select the particular notification of your donations. It will redirect you to the page to resubmit your donations. </p>
            </div>

            <div class="section-header" id="q9" style="margin-top: 20px; background: #569a7b" onclick="toggleAnswer('q9Content')">
                <h5 style="font-size: 18px;">Can I close the published donation?</h5>
            </div>
            <div class="section-content" id="q9Content" style="display: none;">
                <p>Yes, you can close the published donation by visiting the <b>"Manage Donations" </b>page and filter the donations by status <b>"Opened"</b> to select the donation that you would like to close and press the <b>"Close" </b>button. You are required to enter the reason of closure.</p>

            </div>

            <div class="section-header" id="q10" style="margin-top: 20px; background: #569a7b" onclick="toggleAnswer('q10Content')">
                <h5 style="font-size: 18px;">Can I edit the published donation?</h5>
            </div>
            <div class="section-content" id="q10Content" style="display: none;">
                <p>Yes, you can edit the published donation by visiting the <b>"Manage Donations" </b>page and select the donation that you would like to edit and press the <b>"Update" </b>button to confirm the changes.</p>

            </div>

            <div class="section-header" id="q11" style="margin-top: 20px; background: #569a7b" onclick="toggleAnswer('q11Content')">
                <h5 style="font-size: 18px;">Can I cancel or delete the donation that is pending for approval?</h5>
            </div>
            <div class="section-content" id="q11Content" style="display: none;">
                <p>Yes, you can still cancel the application of new donation by visiting the <b>"Manage Donations" </b>page and filter the donations by status <b>"Pending Approval"</b> to select the donation that you would like to cancel and press the <b>"Cancel" </b>button to delete the application.</p>

            </div>

            <div class="section-header" id="q12" style="margin-top: 20px; background: #569a7b" onclick="toggleAnswer('q12Content')">
                <h5 style="font-size: 18px;">Where can I receive the donation requests from the donors?</h5>
            </div>
            <div class="section-content" id="q12Content" style="display: none;">
                <p>You will receive email and system notifications for the new donation requests. Alternatively, you can check it at your dashboard.</p>

            </div>
        </div>

        <div class="faq-section">
            <h4 style="font-size: 22px;">Delivery Riders</h4>
            <div class="section-header" id="q13" style="margin-top: 20px; background: #2b5041" onclick="toggleAnswer('q13Content')">
                <h5 style="font-size: 18px; color: whitesmoke;">How do I register as a delivery rider?</h5>
            </div>
            <div class="section-content" id="q13Content" style="display: none;">
                <p>You can register as an organization by visiting the Sign Up page and selecting the <b>"Delivery Rider" </b>option. Complete the required information and submit the needed documents and your application will be sent to our team for approval. You will be informed through email notifications about the status of your application.</p>
            </div>

            <div class="section-header" id="q14" style="margin-top: 20px; background: #2b5041" onclick="toggleAnswer('q14Content')">
                <h5 style="font-size: 18px; color: whitesmoke;">What to do if my registration application is rejected?</h5>
            </div>
            <div class="section-content" id="q14Content" style="display: none;">
                <p>You may press the link sent to your email address and resubmit your application again based on the rejected reason provided.</p>
            </div>

            <div class="section-header" id="q15" style="margin-top: 20px; background: #2b5041" onclick="toggleAnswer('q15Content')">
                <h5 style="font-size: 18px; color: whitesmoke;">How can I look for delivery pick-ups?</h5>
            </div>
            <div class="section-content" id="q15Content" style="display: none;">
                <p>You may view it at your dashboard and select the donation pick-ups that your preferred. Please read the extra information provided by the donors (example: pick-up location, QR code scanning). Each pick-up orders will display the earning amount.</p>
            </div>

            <div class="section-header" id="q16" style="margin-top: 20px; background: #2b5041" onclick="toggleAnswer('q16Content')">
                <h5 style="font-size: 18px; color: whitesmoke;">What I need to do when I am picking up an order?</h5>
            </div>
            <div class="section-content" id="q16Content" style="display: none;">
                <p>You need to turn on the location permission (for location tracking), scan the QR code from the donors to confirm the pick-up and take a picture of the items collected to update in the system.</p>
            </div>
        </div>

    </div>

    <script>
    function toggleAnswer(contentId) {
        var content = document.getElementById(contentId);
        if (content.style.display === "none") {
            content.style.display = "block";
        } else {
            content.style.display = "none";
        }
    }
</script>
</asp:Content>
