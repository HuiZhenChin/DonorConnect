<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="DonorConnect.AdminManageUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>User Management</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        .card-title{
            font-size: 18px;
        }

       

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <!-- Manage User Box -->
            <div class="col-md-3">
                <div class="card text-white bg-primary mb-3">
                    <div class="card-body text-center">
                        <h5 class="card-title">Manage User</h5>
                        <asp:Button ID="btnUser" style="opacity: 0%; width: 20%;" runat="server" CssClass="btn btn-light" OnClick="btnUser_click" />
                    </div>
                </div>
            </div>
            <!-- Donation Request Box -->
            <div class="col-md-3">
                <div class="card text-white bg-secondary mb-3">
                    <div class="card-body text-center">
                        <h5 class="card-title">Donation Request</h5>
                        <asp:Button ID="btnDonationRequest" style="opacity: 0%; width: 20%;" runat="server" CssClass="btn btn-light" OnClick="btnRequest_click" />
                    </div>
                </div>
            </div>
            <!-- Application Box -->
            <div class="col-md-3">
                <div class="card text-white bg-success mb-3">
                    <div class="card-body text-center">
                        <h5 class="card-title">Registration Application</h5>
                        <asp:Button ID="btnApplication" style="opacity: 0%; width: 20%;" runat="server" CssClass="btn btn-light" OnClick="btnApplication_click" />
                    </div>
                </div>
            </div>
            <!-- Delivery Box -->
            <div class="col-md-3">
                <div class="card text-white bg-warning mb-3">
                    <div class="card-body text-center">
                        <h5 class="card-title">Delivery</h5>
                        <asp:Button ID="btnDelivery" style="opacity: 0%; width: 20%;" runat="server" CssClass="btn btn-light" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
