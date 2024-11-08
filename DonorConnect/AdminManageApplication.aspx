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
         body{
             background: rgb(238,233,218);
             background: linear-gradient(180deg, rgba(238,233,218,1) 17%, rgba(221,228,232,1) 56%, rgba(147,191,207,1) 80%, rgba(123,178,209,1) 100%);
         }

        .table-bordered {
            background-color: #f9f9f9;
            border-collapse: separate;
            border-spacing: 0;
            border-radius: 10px;
            overflow: hidden;
        }

            .table-bordered thead th {
                background-color: #4a90e2;
                color: #fff;
                font-weight: bold;
            }

            .table-bordered tbody tr:nth-child(even) {
                background-color: #f1f1f1;
            }

            .table-bordered tbody tr:nth-child(odd) {
                background-color: #ffffff;
            }

            .table-bordered thead th:first-child {
                border-top-left-radius: 10px;
            }

            .table-bordered thead th:last-child {
                border-top-right-radius: 10px;
            }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   


  <%--  gridview--%>

    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control w-25 mb-4" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" style="margin: 10px; border: solid 1px #4A628A; background-color: #F9F6F2;">
        <asp:ListItem Text="Pending Approval" Value="Pending Approval" Selected="True"></asp:ListItem>
        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
    </asp:DropDownList>


    <div class="card-deck">
            <div class="card mt-3 shadow-sm" style="background-color: transparent; padding: 10px; border: none;">
                    <div id="application" runat="server">
                        <asp:Label ID="lblApplication" runat="server" CssClass="d-block text-center mb-4" Text="List of Applications" Style="font-weight: bold; font-size: 28px;"></asp:Label>
                        
                        <!-- Organization Applications -->
                        <asp:Label ID="lblOrg" runat="server" CssClass="d-block mb-4" Text="Organizations" Visible="false" Style="font-weight: bold; font-size: 20px;"></asp:Label>
                        <asp:GridView ID="gvOrg" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvOrg_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="orgName" HeaderText="Name" SortExpression="Name">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="orgEmail" HeaderText="Email Address" SortExpression="Email Address">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="orgContactNumber" HeaderText="Contact Number" SortExpression="Contact Number">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="orgAddress" HeaderText="Address" SortExpression="Address">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="orgRegion" HeaderText="Region" SortExpression="Region">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                <asp:BoundField DataField="createdOn" HeaderText="Date Registered" SortExpression="Date Registered">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnViewOrg" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewOrg_click" style="background-color: #00A388; border: #468585;"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblNoOrgApplications" runat="server" Text="No organization applications found." CssClass="text-danger" Visible="false" />

                        <!-- Delivery Rider Applications -->
                        <asp:Label ID="lblRider" runat="server" CssClass="d-block mb-4" Text="Delivery Riders" Visible="false" Style="font-weight: bold; font-size: 20px; padding-top: 10px;"></asp:Label>
                        <asp:GridView ID="gvRider" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvRider_RowDataBound">
                            <Columns>
                                 <asp:BoundField DataField="riderUsername" HeaderText="Username" SortExpression="Username">
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                </asp:BoundField>
                                    <asp:BoundField DataField="riderFullName" HeaderText="Full Name" SortExpression="Full Name">
                                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="riderEmail" HeaderText="Email Address" SortExpression="Email Address">
                                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="riderContactNumber" HeaderText="Contact Number" SortExpression="Contact Number">
                                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vehicleType" HeaderText="Vehicle Type" SortExpression="Vehicle Type">
                                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vehiclePlateNumber" HeaderText="Plate Number" SortExpression="Plate Number">
                                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="registerDate" HeaderText="Date Registered" SortExpression="Date Registered">
                                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    </asp:BoundField>
                                <asp:TemplateField>
                                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnViewRider" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewRider_click" style="background-color: #00A388; border: #468585;"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblNoRiderApplications" runat="server" Text="No delivery rider applications found." CssClass="text-danger" Visible="false" />
                    </div>
                </div>         
        </div>


</asp:Content>

