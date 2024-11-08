<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="DonorConnect.AdminManageUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        body{
            background: rgb(238,233,218);
            background: linear-gradient(180deg, rgba(238,233,218,1) 17%, rgba(221,228,232,1) 56%, rgba(147,191,207,1) 80%, rgba(123,178,209,1) 100%);
        }

        .body-content::before {
            content: "";
            position: absolute;
            width: 100%;
            height: 100%;
            background: url(/Image/cloud.png) no-repeat bottom left;
            background-size: 100% auto;
            opacity: 0.5;
            z-index: -1;

        }

        .body-content::after {
            content: "";
            position: absolute;
            width: 100%;
            height: 100%;
            background: url(/Image/bird.png) no-repeat top right;
            background-size: 60% auto;
            opacity: 0.5;
            z-index: -1;
            top: -10%;
        }

        .card-title{
            font-size: 16px;
        }

        .badgeNew{
            font-size: 100%!important;
            padding: 0.2em 0.45em; 
            border-radius: 0.25rem; 
        }
       
        .border-box {
            border: 2px solid #ccc;
            border-radius: 15px; 
            padding: 20px; 
            background-color: #f8f9fa; 
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); 
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container d-flex flex-column justify-content-center align-items-center border-box" style="margin-top: 70px; background-color: rgba(238, 238, 238, 0.8);">
        <!-- Centered Dashboard Title -->
        <h2 class="w-100 text-center mb-4" style="font-weight: bold; font-size: 32px;">Dashboard</h2>

        <div class="row text-center">
            <!-- Manage User Box -->
            <div class="col-md-4 mb-4">
                <div class="card shadow-lg border-0" style="border-radius: 15px;">
                    <div class="card-body bg-primary text-white rounded-top" style="border-radius: 15px 15px 0 0;">
                        <h5 class="card-title">Manage User</h5>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnUser" runat="server" Text="Go to Manage User" CssClass="btn btn-primary" OnClick="btnUser_click" />
                    </div>
                </div>
            </div>
            <!-- Donation Request Box -->
            <div class="col-md-4 mb-4 position-relative">
                <div class="card shadow-lg border-0" style="border-radius: 15px; border: 2px solid #ffcc00;">
                    <div class="card-body bg-secondary text-white rounded-top" style="border-radius: 15px 15px 0 0;">
                        <h5 class="card-title">Donation Request</h5>
                        <!-- Display the pending approval count at the top right -->
                        <span class="badgeNew badge-danger position-absolute" style="top: 10px; right: 20px;">
                            <asp:Label ID="lblPendingApprovalCount" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnDonationRequest" runat="server" Text="Go to Donation Request" CssClass="btn btn-secondary" OnClick="btnRequest_click" />
                    </div>
                </div>
            </div>
            <!-- Registration Application Box -->
            <div class="col-md-4 mb-4 position-relative">
                <div class="card shadow-lg border-0" style="border-radius: 15px;">
                    <div class="card-body bg-success text-white rounded-top" style="border-radius: 15px 15px 0 0;">
                        <h5 class="card-title">Registration Application</h5>
                        <!-- Display the registration approval count at the top right -->
                        <span class="badgeNew badge-danger position-absolute" style="top: 10px; right: 20px;">
                            <asp:Label ID="lblRegistrationApprovalCount" runat="server" Text="0"></asp:Label>
                        </span>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnApplication" runat="server" Text="Go to Applications" CssClass="btn btn-success" OnClick="btnApplication_click" />
                    </div>
                </div>
            </div>
            <!-- Item Category Box -->
            <div class="col-md-4 mb-4">
                <div class="card shadow-lg border-0" style="border-radius: 15px;">
                    <div class="card-body bg-info text-white rounded-top" style="border-radius: 15px 15px 0 0;">
                        <h5 class="card-title">Item Category</h5>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnItemCategory" runat="server" Text="Manage Category" CssClass="btn btn-info" OnClick="btnItemCategory_click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
