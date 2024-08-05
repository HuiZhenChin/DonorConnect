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
<%--<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-3">
        <div class="d-flex justify-content-between align-items-center">
            <h2>Manage Item Donations</h2>
            <asp:Button ID="btnCreateDonation" runat="server" Text="Create New Donation" CssClass="btn btn-primary" OnClick="btnCreateDonation_Click" />
        </div>--%>

        <%--<div class="mt-3">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search Donations..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged"></asp:TextBox>
        </div>--%>

    <%-- <div class="mt-3">
            <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="donationPublishId" HeaderText="ID" SortExpression="DonationID" />
                    <asp:BoundField DataField="title" HeaderText="Title" SortExpression="Title" />
                    <asp:BoundField DataField="peopleNeeded" HeaderText="Title" SortExpression="Title" />
                    <asp:BoundField DataField="description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="itemCategory" HeaderText="Item Category" SortExpression="ItemCategory" />
                    <asp:BoundField DataField="address" HeaderText="Address" SortExpression="Address" />
                    <asp:BoundField DataField="created_on" HeaderText="Date Published" SortExpression="DatePublished" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Edit"  CssClass="btn btn-info btn-sm" />
                            <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Delete"  CssClass="btn btn-danger btn-sm" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>--%>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-3">
        <div class="d-flex justify-content-between align-items-center">
            <h2>Manage Item Donations</h2>
            <asp:Button ID="btnCreateDonation" runat="server" Text="Create New Donation" CssClass="btn btn-primary" OnClick="btnCreateDonation_Click" />
        </div>

        <div class="mt-3">
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                <asp:ListItem Value="All" Text="All" />
                <asp:ListItem Value="Pending Approval" Text="Pending Approval" />
                <asp:ListItem Value="Opened" Text="Opened" />
                <asp:ListItem Value="Closed" Text="Closed" />
                <asp:ListItem Value="Rejected" Text="Rejected" />
            </asp:DropDownList>
            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-secondary" OnClick="btnFilter_Click" />
        </div>

        <div class="mt-3">
            <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Donation Details">
                        <ItemTemplate>
                            <div>
                                <strong>Title:</strong> <%# Eval("title") %><br />
                                <strong>People Needed:</strong> <%# Eval("peopleNeeded") %><br />
                                <strong>Description:</strong> <%# Eval("description") %><br />
                                <strong>Item Category:</strong> <%# Eval("itemCategory") %><br />
                                <strong>Address:</strong> <%# Eval("address") %><br />
                                <strong>Date Published:</strong> <%# Eval("created_on") %><br />
                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Edit" CssClass="btn btn-info btn-sm" />
                                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Delete" CssClass="btn btn-danger btn-sm" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>