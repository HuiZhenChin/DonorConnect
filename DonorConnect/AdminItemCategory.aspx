<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminItemCategory.aspx.cs" Inherits="DonorConnect.AdminItemCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <meta charset="UTF-8">
     <meta name="viewport" content="width=device-width, initial-scale=1.0">
     <title>Item Category Management</title>
     <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
     <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
     <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>

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
    <div class="container">
        <h2 style="text-align: center; padding-top: 10px; font-weight: bold; font-size: 28px;">Item Category Management</h2>
        <asp:Button ID="btnAddNewCategory" runat="server" Text="Add New Category" CssClass="btn btn-primary my-4" OnClientClick="$('#addCategoryModal').modal('show'); return false;" style="float: right; background-color: #193441; border-color: #193441;"/>

        <asp:GridView ID="gvItemCategories" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowEditing="gvItemCategories_RowEditing" OnRowCancelingEdit="gvItemCategories_RowCancelingEdit" OnRowUpdating="gvItemCategories_RowUpdating" OnRowDataBound="gvItemCategories_RowDataBound">
            <Columns>
    
                <asp:TemplateField HeaderText="Category Name">
                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    <ItemTemplate>
                        <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("categoryName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

      
                <asp:TemplateField HeaderText="Specific Items">
                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    <ItemTemplate>
                        <asp:Label ID="lblSpecificItems" runat="server" Text='<%# Eval("specificItems") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSpecificItems" runat="server" Text='<%# Eval("specificItems") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
        
                <asp:TemplateField HeaderText="Has Expiry Date">
                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    <ItemTemplate>
                        <%# Eval("hasExpiryDate").ToString().Equals("Yes", StringComparison.OrdinalIgnoreCase) || 
                            Eval("hasExpiryDate").ToString().Equals("True", StringComparison.OrdinalIgnoreCase) ? "Yes" : "No" %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlHasExpiryDate" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Category Icon">
                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    <ItemTemplate>
                        <i class='<%# "fa " + Eval("categoryIcon") %>' style="font-size: 24px;"></i>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlCategoryIcon" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions">
                    <HeaderStyle BackColor="#134B70" ForeColor="White" />
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-sm btn-primary mr-2" CommandName="Edit" style="background-color: #365486;"></asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-sm btn-danger" style="background-color: #D24545;" CommandArgument='<%# Eval("categoryName") %>' OnClick="btnDelete_Click"></asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-sm btn-success mr-2" CommandName="Update" style="background-color: #006769;"></asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-secondary" CommandName="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

     <div class="modal fade" id="addCategoryModal" tabindex="-1" role="dialog" aria-labelledby="addCategoryModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="addCategoryModalLabel">Add New Category</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtNewCategoryName">Category Name</label>
                            <asp:TextBox ID="txtNewCategoryName" runat="server" CssClass="form-control" Placeholder="Enter category name"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveNewCategory" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnSaveNewCategory_Click" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

    <script>
        function showSuccess(message) {
            Swal.fire({
                title: 'Success!',
                text: message,
                icon: 'success',
                confirmButtonText: 'OK'
            });
        }
    </script>
    
</asp:Content>

