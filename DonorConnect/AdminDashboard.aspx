<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="DonorConnect.AdminManageUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>User Management</title>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <asp:Button ID="btnUser" runat="server" Text="Manage User" CssClass="btn btn-primary" OnClick="btnUser_click" />
     <asp:Button ID="btnDonationRequest" runat="server" Text="Donation Request" CssClass="btn btn-secondary" OnClick="btnRequest_click"/>
     <asp:Button ID="btnApplication" runat="server" Text="Application" CssClass="btn btn-success" OnClick="btnApplication_click"/>
     <asp:Button ID="btnDelivery" runat="server" Text="Delivery" CssClass="btn btn-warning" />

</asp:Content>
