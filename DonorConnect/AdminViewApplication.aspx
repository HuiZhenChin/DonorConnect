<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminViewApplication.aspx.cs" Inherits="DonorConnect.AdminViewApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>New Application Management</title>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>

    <style>
          body{
              background: rgb(238,233,218);
              background: linear-gradient(180deg, rgba(238,233,218,1) 17%, rgba(221,228,232,1) 56%, rgba(147,191,207,1) 80%, rgba(123,178,209,1) 100%);
          }
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

    <div class="card-deck">
        <div class="card mt-3 shadow-sm">
            <div class="card-body" style="background-color: rgba(238, 238, 238, 0.8);">
                <div id="application" runat="server">
                    <asp:Label ID="lblApplication" runat="server" CssClass="d-block text-center mb-4" Text="New Applications" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                    <asp:Repeater ID="rptOrg" runat="server" Visible="false" OnItemDataBound="rptOrg_ItemDataBound">

                        <ItemTemplate>
                            <div class="row mb-2">
                                <div class="col-md-4 font-weight-bold">
                                    <asp:Literal ID="litLabel" runat="server" Text='<%# Eval("FieldName") %>'></asp:Literal>
                                    <asp:HyperLink ID="hyperLinkLicense" runat="server" NavigateUrl="https://www.ssm.com.my/Pages/e-Search.aspx" Text="(Verify SSM Here...)" Target="_blank" CssClass="ml-2" Visible="false"></asp:HyperLink>
                                </div>
                                <div class="col-md-8">
                                    <asp:Literal ID="litValue" runat="server" Text='<%# Eval("FieldValue") %>'></asp:Literal>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div class="row">
                                <div class="col-md-12 text-right">

                                    <asp:LinkButton ID="btnApproveOrg" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="Approve" CssClass="btn btn-info btn-lg" OnClick="btnApproveOrg_click" style="background-color: seagreen;"/>
                                    <asp:LinkButton ID="btnRejectOrg" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("orgId") %>' Text="Reject" CssClass="btn btn-danger btn-lg" OnClientClick='<%# "showRejectionModal(\"" + Eval("orgId") + "\"); return false;" %>' />
                                </div>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>

                    <asp:Repeater ID="rptRider" runat="server" Visible="false" OnItemDataBound="rptRider_ItemDataBound">

                        <ItemTemplate>
                            <div class="row mb-2">
                                <div class="col-md-4 font-weight-bold">
                                    <asp:Literal ID="litLabel2" runat="server" Text='<%# Eval("FieldName2") %>'></asp:Literal>
                                </div>
                                <div class="col-md-8">
                                    <asp:Literal ID="litValue2" runat="server" Text='<%# Eval("FieldValue2") %>'></asp:Literal>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div class="row">
                                <div class="col-md-12 text-right">
                                    <asp:LinkButton ID="btnApproveRider" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="Approve" CssClass="btn btn-info btn-lg" OnClick="btnApproveRider_click" style="background-color: seagreen;"/>
                                    <asp:LinkButton ID="btnRejectRider" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("riderId") %>' Text="Reject" CssClass="btn btn-danger btn-lg" OnClientClick='<%# "showRejectionModal2(\"" + Eval("riderId") + "\"); return false;" %>'/>
                                </div>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>

    <!-- Rejection Reason Dialog Box -->
    <div class="modal fade" id="rejectionModal" tabindex="-1" role="dialog" aria-labelledby="rejectionModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="rejectionModalLabel">Rejection Reason</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" Placeholder="Enter the reason for rejection: "></asp:TextBox>
                    <asp:HiddenField ID="hfOrgId" runat="server" />
                    <asp:HiddenField ID="hfRiderId" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnSubmitRejection" runat="server" CssClass="btn btn-danger" OnClick="btnReject_Click"> Submit Rejection</asp:LinkButton>
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
                timer: 5000, 
                timerProgressBar: true,
                willClose: () => {
                    
                    window.location.href = 'AdminManageApplication.aspx';
                }
            });
        }


        function showError(message) {
            Swal.fire({
                title: 'Error!',
                text: message,
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }

        function showRejectionModal(orgId) {
            $('#<%= hfOrgId.ClientID %>').val(orgId);
            $('#rejectionModal').modal('show');
        }

        function showRejectionModal2(riderId) {
            $('#<%= hfRiderId.ClientID %>').val(riderId);
            $('#rejectionModal').modal('show');
        }
        
     
    </script>

</asp:Content>
