<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrgDonations.aspx.cs" Inherits="DonorConnect.PublishDonations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Publish Donations</title>
    <%--<link href="/Content/MyAccount.css" rel="stylesheet" type="text/css" />--%>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-3">
        <div class="d-flex justify-content-between align-items-center">
            <h2>Publish Donations</h2>
            <asp:Button ID="btnCreateDonation" runat="server" Text="Create New Donation" CssClass="btn btn-primary" OnClick="btnCreateDonation_Click" />
        </div>

        <%--<div class="mt-3">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Donations..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
        </div>--%>

        <%--<div class="mt-3">
            <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="DonationID" HeaderText="ID" SortExpression="DonationID" />
                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="DatePublished" HeaderText="Date Published" SortExpression="DatePublished" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("DonationID") %>' Text="Edit" OnClick="btnEdit_Click" CssClass="btn btn-info btn-sm" />
                            <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("DonationID") %>' Text="Delete" OnClick="btnDelete_Click" CssClass="btn btn-danger btn-sm" />--%>
                        <%--</ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>--%>
    </div>
</asp:Content>
