<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminManageDonationRequest.aspx.cs" Inherits="DonorConnect.AdminManageDonationRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>Donation Request</title>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        .custom {
            border: 1px solid black; 
            border-collapse: separate;
            border-spacing: 0;
        }

        .custom th {
            background-color: lightgray; 
            color: black;
            font-weight: bold;
        }

        .custom td, .custom th {
            border: none; 
        }

        .custom th:first-child,
        .custom td:first-child {
            border-left: 1px solid black; 
        }

        .custom th:last-child,
        .custom td:last-child {
            border-right: 1px solid black; 
        }

        .custom tr:first-child th {
            border-top: 1px solid black; 
        }

        .custom tr:last-child td {
            border-bottom: 1px solid black; 
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   


  <%--  gridview--%>

    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control w-25 mb-4" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
        <asp:ListItem Text="Pending Approval" Value="Pending Approval" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
        <asp:ListItem Text="Opened" Value="Opened"></asp:ListItem>
        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
    </asp:DropDownList>


    <div class="card-deck">
        <div class="card mt-3 shadow-sm">
            <div class="card-body">

                <div id="donation_request" runat="server">
                    <asp:Label ID="lblRequest" runat="server" CssClass="d-block text-center mb-4" Text="List of Donation Requests" Style="font-weight: bold; font-size: 24px;"></asp:Label>
                    
                    <asp:GridView ID="gvDonation" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvDonation_RowDataBound">
                        <Columns>

                            <asp:TemplateField HeaderText="Urgency">
                                <ItemTemplate>
                                    <div class="text-center">
                                        <i class="fas fa-fire text-danger" runat="server" id="fireIcon" visible="false"></i>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:BoundField DataField="title" HeaderText="Title" SortExpression="Title" />
                            <asp:BoundField DataField="orgName" HeaderText="Organization" SortExpression="Organization" />
                            <asp:TemplateField HeaderText="Item Category">
                                <ItemTemplate>
                                    <asp:Literal ID="litItemCategory" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="address" HeaderText="Address" SortExpression="Address" />
                            <asp:BoundField DataField="donationState" HeaderText="State" SortExpression="State" />
                            <asp:BoundField DataField="restriction" HeaderText="Restriction" SortExpression="Restriction" />
                            <asp:BoundField DataField="created_on" HeaderText="Date Submitted" SortExpression="Date Submitted" />
                            <asp:BoundField DataField="adminId" HeaderText="Handled By" SortExpression="Handled By" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%--view is to view donations--%>
                                    <asp:LinkButton ID="btnViewDonation" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewDonation_click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                   
                </div>


            </div>
        </div>
    </div>


</asp:Content>
