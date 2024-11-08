<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgSettings.aspx.cs" Inherits="DonorConnect.OrgSettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Settings</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        body{
            background: rgb(249,247,247);
            background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
        }

         .page-header {
    
            padding-bottom: 10px;
            border-bottom: 2px solid #ddd;
            margin-bottom: 20px;
         }

        .title{
            font-size: 1.8em;
            font-weight: bold;
            padding-bottom: 10px;
            color: #333;
            margin-bottom: 20px;
        }

        
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class = "page-header">
            <h1 class="title">Settings</h1>
        </div>
        <div class="form-group">
            <label for="reminderDays">Set expiry reminder (in days):</label>
            <asp:TextBox ID="reminderDaysTextBox" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Enter number of days"></asp:TextBox>
        </div>
        <asp:Button ID="btnSaveReminder" runat="server" Text="Save Reminder" CssClass="btn btn-primary" OnClick="SaveReminderSetting_Click" style="float: right;"/>
  

    </div>
</asp:Content>
