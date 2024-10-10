<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminViewDonationRequest.aspx.cs" Inherits="DonorConnect.AdminViewDonationRequest" EnableViewState="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>New Donation Request</title>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>

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

    <div class="card-deck">
        <div class="card mt-3 shadow-sm">
            <div class="card-body">
                <div id="donationRequest" runat="server">
                    <asp:Label ID="lblDonation" runat="server" CssClass="d-block text-center mb-4" Text="New Donation Request" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                    <asp:Repeater ID="rptDonation" runat="server" OnItemDataBound="rptDonation_ItemDataBound">
                        <ItemTemplate>
                            <!-- Main Details Section -->
                            <div class="row mb-2">
                                <div class="col-md-4 font-weight-bold">
                                    <asp:Literal ID="litLabel" runat="server" Text='<%# Eval("FieldName") %>'></asp:Literal>
                                </div>
                                <div class="col-md-8">
                                    <asp:Literal ID="litValue" runat="server" Text='<%# Eval("FieldValue") %>'></asp:Literal>
                                </div>
                            </div>

                            <!-- Category Details Section -->
                            <div class="row mb-2" runat="server" id="categoryRow" visible="false">
                                <div class="col-md-12">
                                    <asp:Label runat="server" Text="Item Categories" style="padding-bottom: 10px; font-weight:bold; "></asp:Label>

                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Category</th>
                                                <th>Specific Items</th>
                                                <th runat="server">
                                                    <asp:PlaceHolder ID="phQuantityHeader" runat="server" Visible="false">Quantity</asp:PlaceHolder>
                                                </th>
                                                <th>Actions</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rptCategoryDetails" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" Style="width: fit-content;" Text='<%# Eval("Category") %>'></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSpecificItems" runat="server" CssClass="form-control"
                                                                Text='<%# Eval("SpecificItems") %>' Placeholder="No specified items stated"></asp:TextBox>
                                                            <asp:Literal ID="litItemIcon" runat="server" Visible='<%# Eval("InfoIcon").ToString() == "Yes" %>'>
                                                            <i class='fas fa-info-circle'></i>
                                                        </asp:Literal>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control"
                                                                Text='<%# Eval("Quantity") %>' Visible="false" TextMode="Number" Placeholder="No specified quantity stated"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btnAddCategory" runat="server" CommandArgument='<%# Eval("Category") %>'
                                                                OnCommand="AddNewCategory" Visible='<%# Eval("AddCategoryIcon").ToString() == "Yes" %>'>
                                                                <i class='fas fa-plus'></i>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnAddItem" runat="server" CommandArgument='<%# Eval("Category") + ";" + Eval("SpecificItems") %>'
                                                                OnCommand="AddNewItem" Visible='<%# Eval("AddItemIcon").ToString() == "Yes" %>'>
                                                                    <i class='fas fa-plus-circle'></i>
                                                                </asp:LinkButton>
                                                            </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>

                                </div>
                            </div>
                        </ItemTemplate>
                
                        <FooterTemplate>
                            <div class="row">
                                <div class="col-md-12 text-right">

                                    <asp:LinkButton ID="btnApprove" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="Approve" CssClass="btn btn-info btn-lg" style="background-color: seagreen;" OnClick="btnApprove_click"/>
                                    <asp:LinkButton ID="btnReject" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("donationPublishId") %>' Text="Reject" CssClass="btn btn-danger btn-lg" OnClientClick='<%# "showRejectionModal(\"" + Eval("donationPublishId") + "\"); return false;" %>'/>
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
                    <asp:HiddenField ID="hfDonationRequestId" runat="server" />
                    
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
                    
                    window.location.href = 'AdminManageDonationRequest.aspx';
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
            $('#<%= hfDonationRequestId.ClientID %>').val(orgId);
            $('#rejectionModal').modal('show');
        }


        
     
    </script>

</asp:Content>