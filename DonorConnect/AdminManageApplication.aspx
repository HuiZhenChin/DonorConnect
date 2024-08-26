<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminManageApplication.aspx.cs" Inherits="DonorConnect.AdminManageApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>New Application Management</title>
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
    </asp:DropDownList>


    <div class="card-deck">
        <div class="card mt-3 shadow-sm">
            <div class="card-body">

                <div id="application" runat="server">
                    <asp:Label ID="lblApplication" runat="server" CssClass="d-block text-center mb-4" Text="List of Applications" Style="font-weight: bold; font-size: 24px;"></asp:Label>
                    <asp:Label ID="lblOrg" runat="server" CssClass="d-block mb-4" Text="Organizations" Visible="false" Style="font-weight: bold; font-size: 20px;"></asp:Label>
                    <asp:GridView ID="gvOrg" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvOrg_RowDataBound">
                        <Columns>

                            <asp:BoundField DataField="orgName" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="orgEmail" HeaderText="Email Address" SortExpression="Email Address" />
                            <asp:BoundField DataField="orgContactNumber" HeaderText="Contact Number" SortExpression="Contact Number" />
                            <asp:BoundField DataField="orgAddress" HeaderText="Address" SortExpression="Address" />
                            <asp:BoundField DataField="orgRegion" HeaderText="Region" SortExpression="Region" />
                            <asp:BoundField DataField="createdOn" HeaderText="Date Registered" SortExpression="Date Registered" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%--view is to view donations--%>
                                    <asp:LinkButton ID="btnViewOrg" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewOrg_click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:Label ID="lblRider" runat="server" CssClass="d-block mb-4" Text="Delivery Riders" Visible="false" Style="font-weight: bold; font-size: 20px;"></asp:Label>
                    <asp:GridView ID="gvRider" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvRider_RowDataBound">
                        <Columns>

                             <asp:BoundField DataField="riderUsername" HeaderText="Username" SortExpression="Username" />
                             <asp:BoundField DataField="riderFullName" HeaderText="Full Name" SortExpression="Full Name" />
                             <asp:BoundField DataField="riderEmail" HeaderText="Email Address" SortExpression="Email Address" />
                             <asp:BoundField DataField="riderContactNumber" HeaderText="Contact Number" SortExpression="Contact Number" />
                             <asp:BoundField DataField="vehicleType" HeaderText="Vehicle Type" SortExpression="Vehicle Type" />
                             <asp:BoundField DataField="vehiclePlateNumber" HeaderText="Plate Number" SortExpression="Plate Number" />
                             <asp:BoundField DataField="registerDate" HeaderText="Date Registered" SortExpression="Date Registered" />
                          
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%--view is to view donations--%>
                                    <asp:LinkButton ID="btnViewRider" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewRider_click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>


            </div>
        </div>
    </div>


</asp:Content>

