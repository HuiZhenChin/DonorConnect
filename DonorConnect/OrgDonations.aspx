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
    
    <style>
        .image-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 10px;
        }

        .image-item img {
            width: 200px;
            height: 200px;
            object-fit: cover;
        }

        .donation-row {
            position: relative;
            padding: 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            margin-bottom: 10px;
        }

            .donation-row.urgent::before {
                content: '';
                position: absolute;
                top: 10px;
                left: -10px;
                width: 0;
                height: 0;
                border-left: 20px solid red;
                border-top: 20px solid transparent;
                border-bottom: 20px solid transparent;
            }

            .donation-row.urgent::after {
                content: 'Urgent';
                position: absolute;
                top: 40px;
                left: -50px;
                background-color: red;
                color: white;
                padding: 5px;
                transform: rotate(-90deg);
                transform-origin: left top;
            }

        .mr-2{
            width: 430px!important;
        }
</style>

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

   <div class="d-flex justify-content-start mt-3">
            <div class="mr-2">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                    <asp:ListItem Value="All" Text="All" />
                    <asp:ListItem Value="Pending Approval" Text="Pending Approval" />
                    <asp:ListItem Value="Opened" Text="Opened" />
                    <asp:ListItem Value="Closed" Text="Closed" />
                    <asp:ListItem Value="Rejected" Text="Rejected" />
                </asp:DropDownList>
            </div>

            <div class="mr-2">
                <asp:DropDownList ID="ddlUrgency" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUrgency_SelectedIndexChanged">
                    <asp:ListItem Value="All" Text="All" />
                    <asp:ListItem Value="yes" Text="Urgent" />
                    <asp:ListItem Value="no" Text="Long Term" />
                </asp:DropDownList>
            </div>

            <div>
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-secondary" OnClick="btnFilter_Click" />
            </div>
        </div>

         <div class="mt-3">
            <asp:Label ID="lblNoResults" runat="server" style="margin-top: 20px;" CssClass="alert alert-warning" Visible="false" Text="No results found." />
        </div>

<div class="mt-3">
    <asp:GridView ID="gvDonations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" OnRowDataBound="gvDonations_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Donation Details">
                <ItemTemplate>
                    <div class='<%# Eval("urgentStatus").ToString().ToLower() == "yes" ? "donation-row urgent" : "donation-row" %>'>
                        <strong>Urgent?</strong> <%# Eval("urgentStatus") %><br />
                        <strong>ID:</strong> <%# Eval("donationPublishId") %><br />
                        <strong>Title:</strong> <%# Eval("title") %><br />
                        <strong>People Needed:</strong> <%# Eval("peopleNeeded") %><br />
                        <strong>Description:</strong> <%# Eval("description") %><br />
                        <strong>Restrictions:</strong>
                        <%# String.IsNullOrEmpty(Eval("restriction") as string) ? "No restriction submitted" : Eval("restriction") %><br />
                        <strong>Item Details:</strong><br />
                        <%# Eval("itemDetails") %><br />
                        <strong>Address:</strong> <%# Eval("address") %><br />
                        <strong>State:</strong> <%# Eval("donationState") %><br />
                        <strong>Date Published:</strong> <%# Eval("created_on") %><br />
                        <strong>Images:</strong><br />
                        <%# String.IsNullOrEmpty(Eval("donationImages") as string) ? "No image submitted" : Eval("donationImages") %><br />
                        <strong>Files:</strong><br />
                        <%# String.IsNullOrEmpty(Eval("donationFiles") as string) ? "No file submitted" : Eval("donationFiles") %><br />

                        <!-- Edit Button (conditionally rendered) -->
                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Edit" CssClass="btn btn-info btn-sm" Visible='<%# Eval("status").ToString() == "Pending Approval" || Eval("status").ToString() == "Opened" %>' OnClick="btnEdit_Click" />

                        <!-- Delete Button (always rendered) -->
                        <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Delete" CssClass="btn btn-danger btn-sm" OnClick="btnDelete_Click" />
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>

    </div>
</asp:Content>
