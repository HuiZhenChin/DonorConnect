<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminManageUser.aspx.cs" Inherits="DonorConnect.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>User Management</title>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://stackpath.bootstrapcdn.com/bootstrap/5.3.1/js/bootstrap.bundle.min.js"></script>
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

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
   <div style="padding: 10px;">
        <asp:Button ID="btnDonor" runat="server" Text="Donor" CssClass="btn btn-primary" OnClick="btnShowDonor_click" 
                    style="background-color: #1C6EA4; border: solid 1px #4A628A; margin-right: 10px;" />
        <asp:Button ID="btnOrg" runat="server" Text="Organization" CssClass="btn btn-secondary" OnClick="btnShowOrg_click" 
                    style="background-color: #A47D5D; border: solid 1px #493628; margin-right: 10px;" />
        <asp:Button ID="btnRider" runat="server" Text="Delivery Rider" CssClass="btn btn-success" OnClick="btnShowRider_click" 
                    style="background-color: #2E8B57; border: solid 1px #387478; margin-right: 10px;" />
        <asp:Button ID="btnAdmin" runat="server" Text="Administrator" CssClass="btn btn-warning" OnClick="btnShowAdmin_click" 
                    style="background-color: #5B3E2B; color: white; border: solid 1px #493628;" />
</div>


  <%--  gridview--%>

    <div class="card-deck">
        <div class="card mt-3 shadow-sm" style="background-color: transparent; padding: 10px; border: none;">
                <div class="row mb-3">
                    <div class="col-md-8">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by keyword..." style="border: solid 1px #4A628A;"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" style="background-color: #193441; border-color: #193441;"/>
                         <i class="fa fa-info-circle ms-3" style="cursor: pointer; margin-left: 20px;" data-toggle="modal" data-target="#searchInfoModal"></i>
                    </div>

                </div>

                <div id= "donor" style=" display: none;" runat="server">
                <asp:Label ID="lblDonor" runat="server" CssClass="d-block text-center mb-4" Text="List of Donors" Style="font-weight: bold; font-size: 28px;" ></asp:Label>
                    <asp:Button ID="btnTriggerWarningModal" runat="server" Text="Give Warning" CssClass="btn btn-warning my-4" OnClientClick="$('#warningModal').modal('show'); return false;" style="float: right; margin-top: -10px; background-color: #B22222; color: whitesmoke;"/>

                    <asp:GridView ID="gvDonors" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvDonors_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="donorId" HeaderText="ID" SortExpression="DonorID">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="donorUsername" HeaderText="Username" SortExpression="Username">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="donorName" HeaderText="Full Name" SortExpression="Full Name">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="donorEmail" HeaderText="Email Address" SortExpression="Email Address">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="donorContactNumber" HeaderText="Contact Number" SortExpression="Contact Number">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="createdOn" HeaderText="Date Registered" SortExpression="Date Registered">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Termination Reason">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminateReason" runat="server" Text='<%# Eval("terminateReason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>  
                                    <asp:LinkButton ID="btnViewDonor" runat="server" CommandArgument='<%# Eval("donorId") + "|" + Eval("donorUsername") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewDonor_Click" style="background-color: #00A388; border: #468585;"/>
                                    <asp:LinkButton ID="btnTerminateDonor" runat="server" Style="margin-left: 10px; background-color: #D24545;" CommandArgument='<%# Eval("donorId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" OnClientClick='<%# "showDonorTerminationModal(\"" + Eval("donorId") + "\"); return false;" %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

            </div>

                <div id= "org" style=" display: none;" runat="server">
                <asp:Label ID="lblOrg" runat="server" CssClass="d-block text-center mb-4" Text="List of Organizations" Style="font-weight: bold; font-size: 28px;"></asp:Label>

                    <asp:GridView ID="gvOrg" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvOrg_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="orgId" HeaderText="ID" SortExpression="OrgID">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="orgName" HeaderText="Name" SortExpression="Name">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Email Address">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblEmails" runat="server" Text='<%# Eval("orgEmail") + " / " + Eval("picEmail") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Contact Number">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblContactNumbers" runat="server" Text='<%# Eval("orgContactNumber") + " / " + Eval("picContactNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="orgAddress" HeaderText="Address" SortExpression="Address">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="createdOn" HeaderText="Date Registered" SortExpression="Date Registered">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("orgStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Termination Reason">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminateReason" runat="server" Text='<%# Eval("terminateReason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="adminId" HeaderText="Approved By" SortExpression="Approved By">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" /><HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnViewOrg" runat="server" CommandArgument='<%# Eval("orgId") + "|" + Eval("orgName") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewOrg_Click" style="background-color: #00A388; border: #468585;"/>
                                    <asp:LinkButton ID="btnBusinessLicense" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="Files" CssClass="btn btn-info btn-sm" OnClientClick='<%# "showBusinessLicenseModal(\"" + Eval("orgId") + "\"); return false;" %>' style="background-color: #07588A;"/>
                                    <asp:LinkButton ID="btnTerminateOrg" runat="server" Style="margin-left: 10px; margin-top: 10px; background-color: #D24545;" CommandArgument='<%# Eval("orgId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" OnClientClick='<%# "showOrgTerminationModal(\"" + Eval("orgId") + "\"); return false;" %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

            </div>

                <div id= "rider" style=" display: none;" runat="server">

                    <asp:Label ID="lblRider" runat="server" CssClass="d-block text-center mb-4" Text="List of Delivery Riders" Style="font-weight: bold; font-size: 28px;"></asp:Label>
                    <asp:Button ID="btnTriggerWarningModal2" runat="server" Text="Give Warning" CssClass="btn btn-warning my-4" OnClientClick="$('#warningModal2').modal('show'); return false;" style="float: right; margin-top: -10px; background-color: #B22222; color: whitesmoke;"/>

                    <asp:GridView ID="gvRider" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvRider_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="riderId" HeaderText="ID" SortExpression="riderID">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
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
                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("riderStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Termination Reason">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminateReason" runat="server" Text='<%# Eval("terminateReason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="adminId" HeaderText="Approved By" SortExpression="Approved By">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <%--view is to view information and delivery--%>
                                    <asp:LinkButton ID="btnViewRider" runat="server" CommandArgument='<%# Eval("riderId") + "|" + Eval("riderFullName") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewRider_Click" style="background-color: #00A388; border: #468585;"/>
                                    <asp:LinkButton ID="btnDrivingLicense" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="Files" CssClass="btn btn-info btn-sm" OnClientClick='<%# "showDrivingLicenseModal(\"" + Eval("riderId") + "\"); return false;" %>' style="background-color: #07588A;"/>
                                    <asp:LinkButton ID="btnTerminateRider" runat="server" Style="margin-left: 10px; margin-top: 10px; background-color: #D24545;" CommandArgument='<%# Eval("riderId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" OnClientClick='<%# "showRiderTerminationModal(\"" + Eval("riderId") + "\"); return false;" %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>

                <div id= "admin" style=" display: none;" runat="server">

                <asp:Label ID="lblAdmin" runat="server" CssClass="d-block text-center mb-4" Text="List of Administrators" Style="font-weight: bold; font-size: 28px;"></asp:Label>

                <asp:GridView ID="gvAdmin" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" DataKeyNames="adminId" 
                OnRowEditing="gvAdmin_RowEditing" OnRowCancelingEdit="gvAdmin_RowCancelingEdit" 
                OnRowUpdating="gvAdmin_RowUpdating" OnRowDataBound="gvAdmin_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="adminId" HeaderText="ID" SortExpression="adminID">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    </asp:BoundField>
                    <asp:BoundField DataField="adminUsername" HeaderText="Username" SortExpression="Username">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    </asp:BoundField>
                    <asp:BoundField DataField="adminEmail" HeaderText="Full Name" SortExpression="Full Name">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    </asp:BoundField>
                    <asp:BoundField DataField="created_on" HeaderText="Date Registered" SortExpression="Date Registered">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Termination Reason">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                        <ItemTemplate>
                            <asp:Label ID="lblTerminateReason" runat="server" Text='<%# Eval("terminateReason") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Head of Admin">
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                        <ItemTemplate>
                            <%# Eval("isMain").ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase) || 
                                Eval("isMain").ToString().Equals("True", StringComparison.OrdinalIgnoreCase) ? "Yes" : "No" %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlIsMain" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle BackColor="#134B70" ForeColor="White" />
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary mr-2" 
                                CommandName="Edit" style="background-color: #365486;"></asp:LinkButton>
                            <asp:LinkButton ID="btnTerminateAdmin" runat="server" Style="margin-left: 10px; background-color: #D24545;" 
                                CommandArgument='<%# Eval("adminId") %>' Text="Terminate" CssClass="btn btn-danger btn-sm" 
                                OnClientClick='<%# "showAdminTerminationModal(\"" + Eval("adminId") + "\"); return false;" %>'/>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-sm btn-success mr-2" 
                                CommandName="Update" Style="background-color: #006769;"></asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-secondary" 
                                CommandName="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

                </div>
                </div>
     
    </div>

    <%--Search Guidance Dialog Box--%>
    <div class="modal fade" id="searchInfoModal" tabindex="-1" aria-labelledby="infoModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="infoModalLabel">Searchable Criteria</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>Category</th>
                                <th>Criteria</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Donor</td>
                                <td>Username, Name, Email Address </td>
                            </tr>
                            <tr>
                                <td>Organization</td>
                                <td>Org Name, Email Address, PIC Name, Org Address</td>
                            </tr>
                            <tr>
                                <td>Delivery Rider</td>
                                <td>Username, Full Name, Email Address, Contact Number, Vehicle Plate Number</td>
                            </tr>
                            <tr>
                                <td>Admin</td>
                                <td>Username, Email Address</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Donor Termination -->
<div class="modal fade" id="terminateDonorModal" tabindex="-1" role="dialog" aria-labelledby="terminateModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="terminateModalLabel">Terminate Donor</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="hfDonorId" runat="server" />
        <div class="form-group">
          <label for="ddlReason">Reason for Termination</label>
          <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-control" AutoPostBack="false" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
            <asp:ListItem Text="Select Reason" Value="" Disabled= "True" />
            <asp:ListItem Text="Inactivity" Value="Inactivity" />
            <asp:ListItem Text="Violation of Terms" Value="Violation" />
            <asp:ListItem Text="Others" Value="Others" />
          </asp:DropDownList>
        </div>
        <div class="form-group" id="otherReason" style="display:none;">
          <label for="txtOtherReason">Other Reason</label>
          <asp:TextBox ID="txtOtherReason" runat="server" CssClass="form-control" />
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <asp:Button ID="btnTerminate" runat="server" CssClass="btn btn-danger" Text="Confirm Termination" OnClick="btnTerminateDonor_Click"/>
      </div>
    </div>
  </div>
</div>

    <div class="modal fade" id="terminateOrgModal" tabindex="-1" role="dialog" aria-labelledby="terminateModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="terminateOrgLabel">Terminate Organization</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="hfOrgId" runat="server" />
        <div class="form-group">
          <label for="ddlReason">Reason for Termination</label>
          <asp:DropDownList ID="ddlReasonOrg" runat="server" CssClass="form-control" AutoPostBack="false" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
            <asp:ListItem Text="Select Reason" Value="" Disabled= "True" />
            <asp:ListItem Text="Inactivity" Value="Inactivity" />
            <asp:ListItem Text="Violation of Terms" Value="Violation" />
            <asp:ListItem Text="Others" Value="Others" />
          </asp:DropDownList>
        </div>
        <div class="form-group" id="otherReasonOrg" style="display:none;">
          <label for="txtOtherReason">Other Reason</label>
          <asp:TextBox ID="txtOtherReasonOrg" runat="server" CssClass="form-control" />
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <asp:Button ID="btnTerminateOrg" runat="server" CssClass="btn btn-danger" Text="Confirm Termination" OnClick="btnTerminateOrg_Click"/>
      </div>
    </div>
  </div>
</div>

  <div class="modal fade" id="terminateRiderModal" tabindex="-1" role="dialog" aria-labelledby="terminateModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="terminateRiderLabel">Terminate Organization</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="hfRiderId" runat="server" />
        <div class="form-group">
          <label for="ddlReasonRider">Reason for Termination</label>
          <asp:DropDownList ID="ddlReasonRider" runat="server" CssClass="form-control" AutoPostBack="false" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
            <asp:ListItem Text="Select Reason" Value="" Disabled= "True" />
            <asp:ListItem Text="Inactivity" Value="Inactivity" />
            <asp:ListItem Text="Violation of Terms" Value="Violation" />
            <asp:ListItem Text="Others" Value="Others" />
          </asp:DropDownList>
        </div>
        <div class="form-group" id="otherReasonRider" style="display:none;">
          <label for="txtOtherReasonRider">Other Reason</label>
          <asp:TextBox ID="txtOtherReasonRider" runat="server" CssClass="form-control" />
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <asp:Button ID="btnTerminateRider" runat="server" CssClass="btn btn-danger" Text="Confirm Termination" OnClick="btnTerminateRider_Click"/>
      </div>
    </div>
  </div>
</div>

   <div class="modal fade" id="terminateAdminModal" tabindex="-1" role="dialog" aria-labelledby="terminateModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="terminateAdminLabel">Terminate Organization</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:HiddenField ID="hfAdminId" runat="server" />
        <div class="form-group">
          <label for="ddlReasonAdmin">Reason for Termination</label>
          <asp:DropDownList ID="ddlReasonAdmin" runat="server" CssClass="form-control" AutoPostBack="false" OnSelectedIndexChanged="ddlReason_SelectedIndexChanged">
            <asp:ListItem Text="Select Reason" Value="" Disabled= "True" />
            <asp:ListItem Text="Inactivity" Value="Inactivity" />
            <asp:ListItem Text="Violation of Terms" Value="Violation" />
            <asp:ListItem Text="Others" Value="Others" />
          </asp:DropDownList>
        </div>
        <div class="form-group" id="otherReasonAdmin" style="display:none;">
          <label for="txtOtherReasonAdmin">Other Reason</label>
          <asp:TextBox ID="txtOtherReasonAdmin" runat="server" CssClass="form-control" />
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <asp:Button ID="btnTerminateAdmin" runat="server" CssClass="btn btn-danger" Text="Confirm Termination" OnClick="btnTerminateAdmin_Click"/>
      </div>
    </div>
  </div>
</div>

    <!-- Business License Modal -->
<div class="modal fade" id="businessLicenseModal" tabindex="-1" role="dialog" aria-labelledby="businessLicenseModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="businessLicenseModalLabel">Business License</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <div id="licenseContent"></div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

    <div class="modal fade" id="drivingLicenseModal" tabindex="-1" role="dialog" aria-labelledby="drivingLicenseModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="drivingLicenseModalLabel">Driving License</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <div id="licenseContent2"></div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

    <div class="modal fade" id="warningModal" tabindex="-1" role="dialog" aria-labelledby="warningModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="warningModalLabel">Give Warning to Donor</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="ddlDonors">Select Donor</label>
                    <asp:DropDownList ID="ddlDonors" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select a Donor" Value="" Disabled="true"/>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtWarningReason">Warning Reason</label>
                    <asp:TextBox ID="txtWarningReason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Enter the reason for the warning..."></asp:TextBox>
                </div>
                <asp:Label ID="lblWarningStatus" runat="server" CssClass="text-danger mt-2" Style="display:none;"></asp:Label>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnGiveWarning" runat="server" Text="Give Warning" CssClass="btn btn-success" OnClientClick="return confirmGiveWarning(event);" />
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

    <div class="modal fade" id="warningModal2" tabindex="-1" role="dialog" aria-labelledby="warningModalLabel2" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="warningModalLabel2">Give Warning to Donor</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="ddlDonors">Select Donor</label>
                    <asp:DropDownList ID="ddlRiders" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select a Rider" Value="" Disabled="true"/>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label for="txtWarningReason">Warning Reason</label>
                    <asp:TextBox ID="txtWarningReasonRider" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Enter the reason for the warning..."></asp:TextBox>
                </div>
                <asp:Label ID="Label1" runat="server" CssClass="text-danger mt-2" Style="display:none;"></asp:Label>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnGiveWarningRider" runat="server" Text="Give Warning" CssClass="btn btn-success" OnClientClick="return confirmGiveWarning2(event);" />
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>


    <script>
        function showSuccess(message) {
            Swal.fire({
                text: message,
                icon: 'success',
                confirmButtonText: 'OK',

            });
        }

        function showError(message) {
            Swal.fire({
                text: message,
                icon: 'error',
                confirmButtonText: 'OK',

            });
        }

        function showDonorTerminationModal(donorId) {
            $('#<%= hfDonorId.ClientID %>').val(donorId);
            $('#terminateDonorModal').modal('show');

            
        }

        function showOrgTerminationModal(orgId) {
            $('#<%= hfOrgId.ClientID %>').val(orgId);
            $('#terminateOrgModal').modal('show');


        }

        function showRiderTerminationModal(riderId) {
            $('#<%= hfRiderId.ClientID %>').val(riderId);
            $('#terminateRiderModal').modal('show');

        }

        function showAdminTerminationModal(adminId) {
            $('#<%= hfAdminId.ClientID %>').val(adminId);
            $('#terminateAdminModal').modal('show');

        }

        $(document).ready(function () {
            var rejectReason = $('#<%= ddlReason.ClientID %>');
            var otherReason = $('#otherReason');
            var otherReasonText = $('#<%= txtOtherReason.ClientID %>'); 

            rejectReason.change(function () {
                if ($(this).val() === 'Others') {
                    otherReason.show();
                } else {
                    otherReason.hide();
                    otherReasonText.val(''); 
                }
            });

            $('#<%= btnTerminate.ClientID %>').click(function () {
                var selectedReason = rejectReason.val();
                var otherReasonValue = otherReasonText.val().trim();

                if (selectedReason === "") {
                    showError("Please select a valid reason for termination.");
                    return false; 
                }

                if (selectedReason === "Others" && otherReasonValue === "") {
                    showError("Please specify the reason for termination in the 'Other Reason' field.");
                    return false; 
                }

               
            });

            var rejectReasonOrg = $('#<%= ddlReasonOrg.ClientID %>');
            var otherReasonOrg = $('#otherReasonOrg');
            var otherReasonOrgText = $('#<%= txtOtherReasonOrg.ClientID %>'); 

            rejectReasonOrg.change(function () {
                if ($(this).val() === 'Others') {
                    otherReasonOrg.show();
                } else {
                    otherReasonOrg.hide();
                    otherReasonOrgText.val('');
                }
            });

            $('#<%= btnTerminateOrg.ClientID %>').click(function () {
                var selectedReasonOrg = rejectReasonOrg.val();
                var otherReasonOrgValue = otherReasonOrgText.val().trim();

                if (selectedReasonOrg === "") {
                    showError("Please select a valid reason for termination.");
                    return false;
                }

                if (selectedReasonOrg === "Others" && otherReasonOrgValue === "") {
                    showError("Please specify the reason for termination in the 'Other Reason' field.");
                    return false;
                }


            });

            var rejectReasonRider = $('#<%= ddlReasonRider.ClientID %>');
            var otherReasonRider = $('#otherReasonRider');
            var otherReasonRiderText = $('#<%= txtOtherReasonRider.ClientID %>');

            rejectReasonRider.change(function () {
                if ($(this).val() === 'Others') {
                    otherReasonRider.show();
                } else {
                    otherReasonRider.hide();
                    otherReasonRiderText.val('');
                }
            });

            $('#<%= btnTerminateRider.ClientID %>').click(function () {
                var selectedReasonRider = rejectReasonRider.val();
                var otherReasonRiderValue = otherReasonRiderText.val().trim();

                if (selectedReasonRider === "") {
                    showError("Please select a valid reason for termination.");
                    return false;
                }

                if (selectedReasonRider === "Others" && otherReasonRiderValue === "") {
                    showError("Please specify the reason for termination in the 'Other Reason' field.");
                    return false;
                }


            });

            var rejectReasonAdmin = $('#<%= ddlReasonAdmin.ClientID %>');
            var otherReasonAdmin = $('#otherReasonAdmin');
            var otherReasonAdminText = $('#<%= txtOtherReasonAdmin.ClientID %>');

            rejectReasonAdmin.change(function () {
                if ($(this).val() === 'Others') {
                    otherReasonAdmin.show();
                } else {
                    otherReasonAdmin.hide();
                    otherReasonAdminText.val('');
                }
            });

            $('#<%= btnTerminateAdmin.ClientID %>').click(function () {
                var selectedReasonAdmin = rejectReasonAdmin.val();
                var otherReasonAdminValue = otherReasonAdminText.val().trim();

                if (selectedReasonAdmin === "") {
                    showError("Please select a valid reason for termination.");
                    return false;
                }

                if (selectedReasonAdmin === "Others" && otherReasonAdminValue === "") {
                    showError("Please specify the reason for termination in the 'Other Reason' field.");
                    return false;
                }


            });


        });

        function showBusinessLicenseModal(orgId) {
            $('#businessLicenseModal').modal('show');

            $.ajax({
                type: "POST",
                url: "AdminManageUser.aspx/GetBusinessLicense", 
                data: JSON.stringify({ orgId: orgId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
              
                    $('#licenseContent').html(response.d); 
                },
                error: function () {
                    $('#licenseContent').html('<p>An error occurred while fetching the business license.</p>');
                }
            });
        }

        function showDrivingLicenseModal(riderId) {
            $('#drivingLicenseModal').modal('show');

            $.ajax({
                type: "POST",
                url: "AdminManageUser.aspx/GetDrivingLicense",
                data: JSON.stringify({ riderId: riderId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    $('#licenseContent2').html(response.d);
                },
                error: function () {
                    $('#licenseContent2').html('<p>An error occurred while fetching the driving license.</p>');
                }
            });
        }

        function confirmGiveWarning(event) {
            // Prevent the default action if an event is passed
            if (event) {
                event.preventDefault();
            }

            var ddlDonors = document.getElementById('<%= ddlDonors.ClientID %>');
            var selectedDonor = ddlDonors.options[ddlDonors.selectedIndex].text;

            if (ddlDonors.value === "") {
                // Call the custom showError function to show the error alert
                showError('No Donor Selected', 'Please select a donor before giving a warning.');
                return false; // Prevent form submission
            }

            // Show the confirmation dialog
            return Swal.fire({
                title: 'Are you sure?',
                text: "You are about to give a warning to " + selectedDonor + ".",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, give warning'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Trigger postback if confirmed
                    __doPostBack('<%= btnGiveWarning.UniqueID %>', '');
        }
    }).then(() => {
        return false; // Prevent the default action regardless of the confirmation result
    });
        }

        function confirmGiveWarning2(event) {
            // Prevent the default action if an event is passed
            if (event) {
                event.preventDefault();
            }

            var ddlRider = document.getElementById('<%= ddlRiders.ClientID %>');
            var selectedRider = ddlRider.options[ddlRider.selectedIndex].text;

            if (ddlRider.value === "") {
                // Call the custom showError function to show the error alert
                showError('No Rider Selected', 'Please select a rider before giving a warning.');
                return false; // Prevent form submission
            }

            // Show the confirmation dialog
            return Swal.fire({
                title: 'Are you sure?',
                text: "You are about to give a warning to " + selectedRider + ".",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, give warning'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Trigger postback if confirmed
                    __doPostBack('<%= btnGiveWarningRider.UniqueID %>', '');
            }
        }).then(() => {
            return false; // Prevent the default action regardless of the confirmation result
        });
        }



    </script>
</asp:Content>

