<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminManageUser.aspx.cs" Inherits="DonorConnect.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>User Management</title>
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
   
    <asp:Button ID="btnDonor" runat="server" Text="Donor" CssClass="btn btn-primary" OnClick="btnShowDonor_click" />
    <asp:Button ID="btnOrg" runat="server" Text="Organization" CssClass="btn btn-secondary" OnClick="btnShowOrg_click"/>
    <asp:Button ID="btnRider" runat="server" Text="Delivery Rider" CssClass="btn btn-success" OnClick="btnShowRider_click"/>
    <asp:Button ID="btnAdmin" runat="server" Text="Administrator" CssClass="btn btn-warning" OnClick="btnShowAdmin_click"/>


  <%--  gridview--%>

    <div class="card-deck">
        <div class="card mt-3 shadow-sm">
            <div class="card-body">

                <div id= "donor" style=" display: none;" runat="server">
                <asp:Label ID="lblDonor" runat="server" CssClass="d-block text-center mb-4" Text="List of Donors" Style="font-weight: bold; font-size: 24px;" ></asp:Label>

                <asp:GridView ID="gvDonors" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvDonors_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="donorId" HeaderText="ID" SortExpression="DonorID" />
                        <asp:BoundField DataField="donorUsername" HeaderText="Username" SortExpression="Username" />
                        <asp:BoundField DataField="donorName" HeaderText="Full Name" SortExpression="Full Name" />
                        <asp:BoundField DataField="donorEmail" HeaderText="Email Address" SortExpression="Email Address" />
                        <asp:BoundField DataField="donorContactNumber" HeaderText="Contact Number" SortExpression="Contact Number" />
                        <asp:BoundField DataField="donorAddress1" HeaderText="Address" SortExpression="Address" />
                        <asp:BoundField DataField="createdOn" HeaderText="Date Registered" SortExpression="Date Registered" />
                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="Status" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--view is to view donations--%>
                                <asp:LinkButton ID="btnViewDonor" runat="server" CommandArgument='<%# Eval("donorId") %>' Text="View" CssClass="btn btn-info btn-sm" />
                                <asp:LinkButton ID="btnTerminateDonor" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("donorId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

                <div id= "org" style=" display: none;" runat="server">
                <asp:Label ID="lblOrg" runat="server" CssClass="d-block text-center mb-4" Text="List of Organizations" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                <asp:GridView ID="gvOrg" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvOrg_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="orgId" HeaderText="ID" SortExpression="OrgID" />
                        <asp:BoundField DataField="orgName" HeaderText="Name" SortExpression="Name" />
                        <asp:BoundField DataField="orgEmail" HeaderText="Email Address" SortExpression="Email Address" />
                        <asp:BoundField DataField="orgContactNumber" HeaderText="Contact Number" SortExpression="Contact Number" />
                        <asp:BoundField DataField="orgAddress" HeaderText="Address" SortExpression="Address" />
                        <asp:BoundField DataField="picName" HeaderText="Person In-Charge's Name" SortExpression="PIC Name" />
                        <asp:BoundField DataField="picEmail" HeaderText="Person In-Charge's Email Address" SortExpression="PIC Email Address" />
                        <asp:BoundField DataField="picContactNumber" HeaderText="Person In-Charge's Contact Number" SortExpression="PIC Contact Number" />
                        <asp:BoundField DataField="orgDescription" HeaderText="Description" SortExpression="Description" />
                        <asp:BoundField DataField="orgRegion" HeaderText="Region" SortExpression="Region" />
                        <asp:BoundField DataField="createdOn" HeaderText="Date Registered" SortExpression="Date Registered" />
                        <asp:BoundField DataField="orgStatus" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="adminId" HeaderText="Approved By" SortExpression="Approved By" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--view is to view donations--%>
                                <asp:LinkButton ID="btnViewOrg" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="View" CssClass="btn btn-info btn-sm" />
                                <asp:LinkButton ID="btnBusinessLicense" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="Files" CssClass="btn btn-info btn-sm" />
                                <asp:LinkButton ID="btnTerminateOrg" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("orgId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

                <div id= "rider" style=" display: none;" runat="server">

                    <asp:Label ID="lblRider" runat="server" CssClass="d-block text-center mb-4" Text="List of Delivery Riders" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                    <asp:GridView ID="gvRider" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvRider_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="riderId" HeaderText="ID" SortExpression="riderID" />
                            <asp:BoundField DataField="riderUsername" HeaderText="Username" SortExpression="Username" />
                            <asp:BoundField DataField="riderFullName" HeaderText="Full Name" SortExpression="Full Name" />
                            <asp:BoundField DataField="riderEmail" HeaderText="Email Address" SortExpression="Email Address" />
                            <asp:BoundField DataField="riderContactNumber" HeaderText="Contact Number" SortExpression="Contact Number" />
                            <asp:BoundField DataField="vehicleType" HeaderText="Vehicle Type" SortExpression="Vehicle Type" />
                            <asp:BoundField DataField="vehiclePlateNumber" HeaderText="Plate Number" SortExpression="Plate Number" />
                            <asp:BoundField DataField="registerDate" HeaderText="Date Registered" SortExpression="Date Registered" />
                            <asp:BoundField DataField="riderStatus" HeaderText="Status" SortExpression="Status" />
                            <asp:BoundField DataField="adminId" HeaderText="Approved By" SortExpression="Approved By" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%--view is to view donations--%>
                                    <asp:LinkButton ID="btnViewRider" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="View" CssClass="btn btn-info btn-sm" />
                                    <asp:LinkButton ID="btnDrivingLicense" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="Files" CssClass="btn btn-info btn-sm" />
                                    <asp:LinkButton ID="btnTerminateOrg" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("riderId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>

                <div id= "admin" style=" display: none;" runat="server">

                <asp:Label ID="lblAdmin" runat="server" CssClass="d-block text-center mb-4" Text="List of Administrators" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                <asp:GridView ID="gvAdmin" runat="server" CssClass="table table-bordered custom" AutoGenerateColumns="False" OnRowDataBound="gvAdmin_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="adminId" HeaderText="ID" SortExpression="adminID" />
                        <asp:BoundField DataField="adminUsername" HeaderText="Username" SortExpression="Username" />
                        <asp:BoundField DataField="adminEmail" HeaderText="Full Name" SortExpression="Full Name" />
                        <asp:BoundField DataField="created_on" HeaderText="Date Registered" SortExpression="Date Registered" />
                        <asp:BoundField DataField="status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="isMain" HeaderText="Head of Admin" SortExpression="Head of Admin" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--view is to view donations--%>
                                <asp:LinkButton ID="btnViewAdmin" runat="server" CommandArgument='<%# Eval("adminId") %>' Text="View" CssClass="btn btn-info btn-sm" />
                                <asp:LinkButton ID="btnTerminateAdmin" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("adminId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                </div>
                </div>
        </div>
    </div>


</asp:Content>

