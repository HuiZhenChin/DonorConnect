<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrgManageDonationRequest.aspx.cs" Inherits="DonorConnect.OrgManageDonationRequest2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donation</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
    .category-box {
       display: inline-block;
       padding: 10px;
       margin: 5px;
       border: 2px solid;
       border-radius: 5px;
       text-align: center;
       min-width: 100px;
    }

    .border-food {
       border-color: #f39c12;
    }

    .border-books {
       border-color: #3498db;
    }

    .border-toys {
       border-color: #e74c3c;
    }

    .border-medical {
       border-color: #2ecc71;
    }

    .border-clothing{
        border-color: #702963;
    }

    .border-electronics{
        border-color: #EADDCA;
    }

    .border-furniture{
        border-color: #C9CC3F;
    }

    .border-hygiene{
        border-color: #C9A9A6;
    }

    .border-default{
        border-color: dimgray;
    }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="mt-3">
    <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Donation Details">
            <ItemTemplate>
                <!-- Donor Information -->
                <strong>Donor Name:</strong> <%# Eval("donorFullName") %><br />
                <strong>Donor Email:</strong> <%# Eval("donorEmail") %><br />
                <strong>Pick Up Address:</strong> <%# Eval("pickUpAddress") %><br />
                <strong>State:</strong> <%# Eval("state") %><br />
                <strong>Submitted On:</strong> <%# Eval("created_on") %><br />

                <!-- Table for Category, Item, and Quantity -->
                <strong>Items Donated:</strong>
                <ul id="itemList" runat="server">
                    <%--populate from backend--%>
                </ul>

                <!-- View Button -->
                <div class="text-right mt-2">
                    <asp:Button ID="btnView" runat="server" CommandArgument='<%# Eval("donationId") %>' Text="View Details" CssClass="btn btn-info btn-sm" OnClick="btnView_Click"/>
                </div>
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
</asp:GridView>

</div>
</asp:Content>


