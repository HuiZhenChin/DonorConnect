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
        <asp:ListItem Text="Opened" Value="Opened"></asp:ListItem>
        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
    </asp:DropDownList>


    <div class="card-deck">
        <div class="card mt-3 shadow-sm" style="background-color: transparent; padding: 10px; border: none;">         
                <div id="donation_request" runat="server">
                    <asp:Label ID="lblRequest" runat="server" CssClass="d-block text-center mb-4" Text="List of Donation Requests" Style="font-weight: bold; font-size: 28px;"></asp:Label>
                    
                    <asp:GridView ID="gvDonation" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvDonation_RowDataBound">
                        <Columns>

                            <asp:TemplateField HeaderText="Urgency">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <div class="text-center">
                                        <i class="fas fa-fire text-danger" runat="server" id="fireIcon" visible="false"></i>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="title" HeaderText="Title" SortExpression="Title">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="orgName" HeaderText="Organization" SortExpression="Organization">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Item Category">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <asp:Literal ID="litItemCategory" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="address" HeaderText="Address" SortExpression="Address">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="donationState" HeaderText="State" SortExpression="State">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="restriction" HeaderText="Restriction" SortExpression="Restriction">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="created_on" HeaderText="Date Submitted" SortExpression="Date Submitted">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:BoundField DataField="adminId" HeaderText="Handled By" SortExpression="Handled By">
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <HeaderStyle BackColor="#134B70" ForeColor="White" />
                                <ItemTemplate>
                                    <%--view is to view donations--%>
                                    <asp:LinkButton ID="btnViewDonation" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' Text="View" CssClass="btn btn-info btn-sm" OnClick="btnViewDonation_click" style="background-color: #00A388; border: #468585;"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                   
                </div>


            </div>
        
    </div>


</asp:Content>
