<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllDonations.aspx.cs" Inherits="DonorConnect.AllDonations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Item Donations</title>
    
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
  
     .category-box {
        display: inline-block;
        padding: 10px;
        margin: 5px;
        border: 2px solid;
        border-radius: 5px;
        text-align: center;
        min-width: 100px;
     }

        
     .border-food {
        border-color: #f39c12;
     }

     .border-books {
        border-color: #3498db;
     }

     .border-toys {
        border-color: #e74c3c;
     }

     .border-medical {
        border-color: #2ecc71;
     }

     .border-clothing{
         border-color: #702963;
     }

     .border-electronics{
         border-color: #EADDCA;
     }

     .border-furniture{
         border-color: #C9CC3F;
     }

     .border-hygiene{
         border-color: #C9A9A6;
     }

     .border-default{
         border-color: dimgray;
     }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="gvAllDonations" runat="server" AutoGenerateColumns="False" CssClass="centered-grid" DataKeyNames="donationPublishId" GridLines="None" BorderStyle="None" CellPadding="0">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="row">
                        <div class="col-md-8">
                            <div class="card mb-4 shadow-sm card-custom" style="width: 100%!important;">
                                <div class="card-body" style="width: 100%!important;">
                                    <div class="row">
                                        <%-- Left Column with Title and Details --%>
                                        <div class="col-md-8">
                                            <%-- Title --%>
                                            <h3 class="card-title text-center"><%# Eval("title") %></h3>

                                            <%-- People Needed and State --%>
                                            <div class="row mb-3">
                                                <div class="col-md-6">
                                                    <strong>People Needed:</strong>
                                                    <i class="icon icon-people-needed"></i><%# Eval("peopleNeeded") %>
                                                </div>
                                                <div class="col-md-6 text-right">
                                                    <strong>State:</strong>
                                                    <i class="icon icon-state"></i><%# Eval("donationState") %>
                                                </div>
                                            </div>

                                            <%-- Address and Description --%>
                                            <div class="mb-3">
                                                <strong>Address:</strong> <%# Eval("address") %><br />
                                                <strong>Description:</strong> <%# Eval("description") %>
                                            </div>

                                            <%-- Restrictions --%>
                                            <div class="mb-3">
                                                <strong>Restrictions:</strong>
                                                <asp:Label ID="lblRestrictions" runat="server" Text='<%# Eval("restriction") %>' Visible='<%# !String.IsNullOrEmpty(Eval("restriction") as string) %>'></asp:Label>
                                            </div>

                                            <%-- Item Details --%>
                                            <div class="mb-3">
                                                <strong>Item Details:</strong> <%# Eval("itemDetails") %>
                                            </div>

                                            <%-- Images and Files --%>
                                            <div class="mb-3">
                                                <%# String.IsNullOrEmpty(Eval("donationImages") as string) ? "" : "<strong>Images:</strong> " + Eval("donationImages") %>
                                            </div>
                                            <div>
                                                <%# String.IsNullOrEmpty(Eval("donationFiles") as string) ? "" : "<strong>Files:</strong> " + Eval("donationFiles") %>
                                            </div>
                                        </div>
                                        <div class="col-md-4 text-center">
                                            <%-- Item Category and Icons --%>
                                            <asp:Label ID="lblItemCategory" runat="server" Text='<%# GetItemCategoryWithIcon(Eval("itemDetails")) %>' />
                                            <div class="row mb-3">
                                                <div class="col-12 text-right position-absolute" style="bottom: 20px; right: 10px;">
                                                    <asp:Button ID="btnDonate" runat="server" type="submit" CssClass="btn btn-success" Text="Donate Now!" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="d-flex align-items-center justify-content-center">
                        <img src='<%# String.IsNullOrEmpty(Eval("orgProfilePic") as string) ? "/Image/default_picture.jpg" : "data:image/png;base64," + Eval("orgProfilePic") %>'
                             alt="Org Profile Picture"
                             style="width: 100px; height: 100px; border-radius: 50%; border: 1px solid black; margin-right: 10px;" />
                        <strong><%# Eval("orgName") %></strong>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

