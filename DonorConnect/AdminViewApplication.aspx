<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminViewApplication.aspx.cs" Inherits="DonorConnect.AdminViewApplication" %>
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

    <div class="card-deck">
        <div class="card mt-3 shadow-sm">
            <div class="card-body">
                <div id="application" runat="server">
                    <asp:Label ID="lblApplication" runat="server" CssClass="d-block text-center mb-4" Text="New Applications" Style="font-weight: bold; font-size: 24px;"></asp:Label>

                    <asp:Repeater ID="rptOrg" runat="server">
                        <ItemTemplate>
                            <div class="row mb-2">
                                <div class="col-md-4 font-weight-bold">
                                    <asp:Literal ID="litLabel" runat="server" Text='<%# Eval("FieldName") %>'></asp:Literal>
                                </div>
                                <div class="col-md-8">
                                    <asp:Literal ID="litValue" runat="server" Text='<%# Eval("FieldValue") %>'></asp:Literal>
                                </div>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div class="row">
                                <div class="col-md-12 text-center">
                                    <asp:LinkButton ID="btnApproveOrg" runat="server" CommandArgument='<%# Eval("orgId") %>' Text="Approve" CssClass="btn btn-info btn-sm" />
                                    <asp:LinkButton ID="btnRejectOrg" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("orgId") %>' Text="Reject" CssClass="btn btn-danger btn-sm" />
                                </div>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                     <asp:Repeater ID="rptRider" runat="server">
                     <ItemTemplate>
                         <div class="row mb-2">
                             <div class="col-md-4 font-weight-bold">
                                 <asp:Literal ID="litLabel" runat="server" Text='<%# Eval("FieldName") %>'></asp:Literal>
                             </div>
                             <div class="col-md-8">
                                 <asp:Literal ID="litValue" runat="server" Text='<%# Eval("FieldValue") %>'></asp:Literal>
                             </div>
                         </div>
                     </ItemTemplate>
                     <FooterTemplate>
                         <div class="row">
                             <div class="col-md-12 text-center">
                                 <asp:LinkButton ID="btnApproveRider" runat="server" CommandArgument='<%# Eval("riderId") %>' Text="Approve" CssClass="btn btn-info btn-sm" />
                                 <asp:LinkButton ID="btnRejectRider" runat="server" Style="margin-left: 10px;" CommandArgument='<%# Eval("riderId") %>' Text="Reject" CssClass="btn btn-danger btn-sm" />
                             </div>
                         </div>
                     </FooterTemplate>
                 </asp:Repeater>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
