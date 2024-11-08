<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgInventory2.aspx.cs" Inherits="DonorConnect.OrgInventory2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Organization Inventory</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script> 
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.js"></script>
    <script type="text/javascript" src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.charts.js"></script>  

    <style>
 
    body{
        background: rgb(249,247,247);
        background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
    }

    .page-header {
       
        padding-bottom: 10px;
        border-bottom: 2px solid #ddd;
        margin-bottom: 20px;
    }

   
    .dashboard-table {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0;
        margin-top: 20px;
        background-color: #fff;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        border-radius: 10px;
        overflow: hidden;
    }

    .dashboard-table thead th {
        background-color: #343a40; 
        color: #fff; 
        padding: 15px;
        text-align: center;
        border-bottom: 1px solid #dee2e6;
    }

    .dashboard-table tbody td {
        padding: 15px;
        text-align: center;
        border-bottom: 1px solid #dee2e6;
    }

    .dashboard-table tbody tr:hover {
        background-color: #f5f5f5; 
    }

    .table-header {
        background-color: #6c757d; 
        color: #fff;
        font-weight: bold;
        font-size: 16px;
    }

    .table-row {
        background-color: #fff; 
    }

    .table-alt-row {
        background-color: #f9f9f9;
    }


    .dashboard-table td {
        padding: 10px;
        border-bottom: 1px solid #e9ecef;
    }

    .dashboard-table th {
        padding: 12px;
        font-weight: bold;
        font-size: 16px;
        text-align: center;
        background-color: #002D62;
        color: white;
        border-bottom: 2px solid #e9ecef;
    }

    .expire {
        padding: 5px 10px;
        border-radius: 5px;
        font-size: 14px;
        display: inline-block;
    }

    .expire-danger {
        border: solid 2px #800000;
        background-color: #FAA0A0; 
        color: #800000;
        font-weight: bold;
    }

    .expire-warning {
        background-color: #f0dfa5; 
        border: solid 2px #c48519;
        color: #c48519;
        font-weight: bold;
    }

    .expire-success {
        background-color: #e8f9cc; 
        border: solid 2px #28a745;
        color: #28a745;
        font-weight: bold;
    }

    .small-image {
        width: 70px;
        height: 50px;
    }

    .img-fluid{
        border-radius: 5px;
    }

    .title{
        font-size: 1.8em;
        font-weight: bold;
        padding-bottom: 10px;
        color: #333;
        margin-bottom: 20px;
    }

    .side-view {
        position: absolute;
        width: 30%;
        height: 100%;
        background-color: #f8f9fa;
        box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
        transition: right 0.3s ease;
        z-index: 1000;
        padding: 20px;
        right: 0px;
        top: 0px;
        overflow: scroll;
    }

    .side-view-content h2 {
        margin-top: 0;
    }

    .side-view.show {
        display: block;
        right: 0;
    }

    .history-box {
        background-color: #ffffff;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        margin-bottom: 15px;
        padding: 15px;
        position: relative;
        transition: transform 0.3s ease;
    }

    .history-box:hover {
        transform: scale(1.02); 
    }

    .history-box-header {
        display: flex;
        justify-content: flex-end;
        margin-bottom: 10px;
    }

    .created-on {
        font-size: 12px;
        color: #888;
    }

    .history-box-content p {
        font-size: 16px;
        margin: 0;
    }

    .noData {
        display: flex;
        justify-content: center;
        align-items: center;
        height: 200px; 
        font-size: 18px;
        color: #555;
        text-align: center;
    }

</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 class="title">Donation Item Tracking</h1>
       <asp:LinkButton ID="pieChartButton" OnClientClick="showPieChartModal(event);" runat="server" >
            <i class="fas fa-chart-pie" style="font-size: 24px; color: black; cursor: pointer; float: right; margin-top: -55px; margin-right: 85px;"></i>
        </asp:LinkButton>

        <asp:LinkButton ID="calendarButton" runat="server" OnClick="showInventoryHistory">
            <i class="fas fa-calendar" style="font-size: 24px; color: black; cursor: pointer; float: right; margin-top: -55px;"></i>
        </asp:LinkButton>

    </div>

   <div id="sideViewContainer" class="side-view" runat="server">
    <div class="side-view-content">
        <button class="close-btn" onclick="closeSideView(event)">X</button>
        <h2 style="font-size: 24px;">Inventory Activity</h2>
        <asp:Repeater ID="inventoryHistoryRepeater" runat="server">
            <ItemTemplate>
                <!-- Each box for inventory history -->
                <div class="history-box">
                    <div class="history-box-header">
                        <span class="created-on"><%# Eval("createdOn") %></span>
                    </div>
                    <div class="history-box-content">
                        <p><%# Eval("content") %></p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>

    <asp:LinkButton ID="btnViewOther" runat="server" Text="View Other Items" CssClass="btn btn-primary" 
            OnClientClick="return triggerLoading();" OnClick="toggleGridViewChange" />


    <asp:LinkButton ID="btnAddItem" runat="server" CssClass="btn btn-success" OnClick="btnAddItem_Click" style="display: none; float: right;">
        <i class="fas fa-plus-circle" style="font-size: 18px;"></i> Add Item
    </asp:LinkButton>

    <asp:LinkButton ID="btnAddOtherItem" runat="server" CssClass="btn btn-success" OnClick="btnAddOtherItem_Click" style="display: none; float: right;">
        <i class="fas fa-plus-circle" style="font-size: 18px;"></i> Add Item
    </asp:LinkButton>

    <asp:HiddenField ID="hfInventoryId" runat="server" />

    <asp:HiddenField ID="hfThreshold" runat="server" ClientIDMode="Static" />

    <div id="loadingBar" style="display: none; text-align: center; margin-bottom: 10px;">
        <i class="fa fa-spinner fa-spin" style="font-size: 24px;"></i> Loading...
    </div>


    <div class="table-responsive">
        <asp:GridView ID="gvDonationItems" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-bordered table-striped dashboard-table"
              HeaderStyle-CssClass="table-header" RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-alt-row"
              OnRowDataBound="gvDonationItems_RowDataBound">
    <Columns>

        <asp:TemplateField HeaderText="Image">
            <ItemTemplate>
                <div class="image-control" style="display: flex; justify-content: center; align-items: center;">
                
                    <asp:Literal ID="ltImage" runat="server"></asp:Literal>

                    <asp:FileUpload ID="fuItemImage" runat="server" CssClass="file-upload" Style="display: none;" />
                </div>
            </ItemTemplate>
        </asp:TemplateField>

    
        <asp:BoundField DataField="itemCategory" HeaderText="Category" />


        <asp:TemplateField HeaderText="Item Name">
            <ItemTemplate>
                <div class="item-control" style="display: flex; justify-content: center; align-items: center;">
                  
                    <asp:Label ID="lblItem" runat="server" CssClass="lblItem" Text='<%# Eval("item") %>'></asp:Label>

          
                    <asp:TextBox ID="txtItem" runat="server" CssClass="txtItem" Text='<%# Eval("item") %>' style="width: 200px; text-align: center; display: none; border-radius: 5px;"></asp:TextBox>
                </div>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Quantity">
            <ItemTemplate>
                <div class="quantity-control" style="display: flex; justify-content: center; align-items: center;">
                    <a href="javascript:void(0);" onclick="changeQuantity(this, -1)" class="minus" style="display: none;">
                        <i class="fas fa-minus-circle" style="color: #0033cc;"></i>
                    </a>
                    <asp:Label ID="lblQuantity" runat="server" CssClass="lblQuantity" Text='<%# Eval("quantity") %>' style="margin-right: 10px; margin-left: 15px;"></asp:Label>

                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="txtQuantity" Text='<%# Eval("quantity") %>' style="width: 50px; text-align: center; display: none; margin-left: 10px; border-radius: 5px;"></asp:TextBox>

                    <a href="javascript:void(0);" onclick="changeQuantity(this, 1)" class="plus" style="margin-left: 10px; display: none;">
                        <i class="fas fa-plus-circle" style="color: #0033cc;"></i>
                    </a>
                </div>
            </ItemTemplate>
        </asp:TemplateField>

 
        <asp:TemplateField HeaderText="Expiry Date">
            <ItemTemplate>
                <div class="expiry-date-control" style="display: flex; justify-content: center; align-items: center;">
             
                    <asp:Label ID="lblExpiryDate" runat="server" CssClass="lblExpiryDate" Text='<%# Eval("expiryDate", "{0:yyyy-MM-dd}") %>'></asp:Label>

                    <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="txtExpiryDate" Text='<%# Eval("expiryDate", "{0:yyyy-MM-dd}") %>' style="width: 150px; text-align: center; display: none; border-radius: 5px;"></asp:TextBox>
                </div>
            </ItemTemplate>
        </asp:TemplateField>


        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
              <asp:LinkButton ID="thresholdBtn" runat="server" OnClientClick="openThresholdModal(event, this); return false;"  CommandArgument='<%# Eval("inventoryId") %>' data-command-argument='<%# Eval("inventoryId") %>' CssClass="threshold-button">
                <i class="fas fa-bell" style="color: #ff9900; font-size: 18px; padding-right: 10px;"></i>
            </asp:LinkButton>
                <asp:LinkButton ID="editSaveBtn" runat="server" OnClientClick="return toggleEditMode(this);" OnClick="btnSave_Click" CommandArgument='<%# Eval("inventoryId") %>' CssClass="edit-save">
                    <i class="fas fa-edit" style="color: #002080; font-size: 18px; padding-right: 10px;"></i>
                </asp:LinkButton>

                <asp:LinkButton ID="deleteBtn" runat="server" OnClientClick="return showDeleteConfirm(event, this);" CommandArgument='<%# Eval("inventoryId") %>' data-command-argument='<%# Eval("inventoryId") %>' CssClass="delete-button">
                    <i class="fas fa-trash" style="color: #dc3545; font-size: 18px;"></i>
                </asp:LinkButton>

            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

  

        <asp:GridView ID="gvNoExpiryItem" runat="server" AutoGenerateColumns="False" CssClass="table table-hover table-bordered table-striped dashboard-table"
            HeaderStyle-CssClass="table-header" RowStyle-CssClass="table-row" AlternatingRowStyle-CssClass="table-alt-row"
            OnRowDataBound="gvNoExpiryItem_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <div class="image-control" style="display: flex; justify-content: center; align-items: center;">
                          
                             <asp:Literal ID="ltImage2" runat="server"></asp:Literal>

                            <asp:FileUpload ID="fuItemImage2" runat="server" CssClass="file-upload2" Style="display: none;" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="itemCategory" HeaderText="Category" />
                <asp:TemplateField HeaderText="Item Name">
                    <ItemTemplate>
                        <div class="item-control" style="display: flex; justify-content: center; align-items: center;">
                     
                            <asp:Label ID="lblItem2" runat="server" CssClass="lblItem2" Text='<%# Eval("item") %>'></asp:Label>

                          
                            <asp:TextBox ID="txtItem2" runat="server" CssClass="txtItem2" Text='<%# Eval("item") %>' Style="width: 200px; text-align: center; display: none; border-radius: 5px;"></asp:TextBox>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Quantity">
                    <ItemTemplate>
                        <div class="quantity-control" style="display: flex; justify-content: center; align-items: center;">
                            <a href="javascript:void(0);" onclick="changeQuantity2(this, -1)" class="minus2" style="display: none;">
                                <i class="fas fa-minus-circle" style="color: #0033cc;"></i>
                            </a>
                            <asp:Label ID="lblQuantity2" runat="server" CssClass="lblQuantity2" Text='<%# Eval("quantity") %>' style="margin-right: 10px; margin-left: 15px;"></asp:Label>

                         
                            <asp:TextBox ID="txtQuantity2" runat="server" CssClass="txtQuantity2" Text='<%# Eval("quantity") %>' style="width: 50px; text-align: center; display: none; margin-left: 10px; border-radius: 5px;"></asp:TextBox>

                            <a href="javascript:void(0);" onclick="changeQuantity2(this, 1)" class="plus2" style="margin-left: 10px; display: none;">
                                <i class="fas fa-plus-circle" style="color: #0033cc;"></i>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
               
                        <asp:LinkButton ID="thresholdBtn" runat="server" OnClientClick="openThresholdModal(event, this); return false;" CommandArgument='<%# Eval("inventoryId") %>' data-command-argument='<%# Eval("inventoryId") %>' CssClass="threshold-button">
                            <i class="fas fa-bell" style="color: #ff9900; font-size: 18px; padding-right: 10px;"></i>
                        </asp:LinkButton>
                        <asp:LinkButton ID="editSaveBtn" runat="server" OnClientClick="return toggleEditMode2(this);" OnClick="btnSaveOther_Click" CommandArgument='<%# Eval("inventoryId") %>' CssClass="edit-save">
                            <i class="fas fa-edit" style="color: #002080; font-size: 18px; padding-right: 10px;"></i>
                        </asp:LinkButton>

                        <asp:LinkButton ID="deleteBtn" runat="server" OnClientClick="return showDeleteConfirm(event, this);" CommandArgument='<%# Eval("inventoryId") %>' data-command-argument='<%# Eval("inventoryId") %>' CssClass="delete-button2">
                             <i class="fas fa-trash" style="color: #dc3545; font-size: 18px;"></i>
                         </asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

   
    <asp:Label ID="message" Text="" runat="server" CssClass="noData"></asp:Label>

</div>

<div class="modal fade" id="addItemModal" tabindex="-1" role="dialog">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Add Item</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <p>Select how you want to add the item:</p>
        <button type="button" class="btn btn-primary" onclick="showExistingItems();">Existing Item</button>
        <button type="button" class="btn btn-secondary" onclick="showNewItemForm();">New Item</button>

        <!-- Existing Items Section -->
          <div id="existingItemsSection" style="display: none; margin-top: 15px;">
              <h5>Select Existing Item</h5>
              <asp:GridView ID="gvExistingItems" runat="server" AutoGenerateColumns="False" CssClass="table table-hover"
                  DataKeyNames="inventoryId" OnRowCommand="gvExistingItems_RowCommand">
                  <Columns>
                      <asp:BoundField DataField="item" HeaderText="Item" />
                      <asp:BoundField DataField="quantity" HeaderText="Quantity" />
                      <asp:ButtonField Text="Add" CommandName="Add" ButtonType="Button" />
                  </Columns>
              </asp:GridView>
          </div>


        <!-- New Item Form -->
          <div id="newItemForm" style="display: none; margin-top: 15px;">
              <h5>Enter New Item Details</h5>
              <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                  <asp:ListItem Text="Select Category" Value="" Disabled="true" Selected="true"/>
              </asp:DropDownList><br />
              <asp:TextBox ID="txtNewItemName" runat="server" Placeholder="Item Name" CssClass="form-control"></asp:TextBox><br />
              <asp:TextBox ID="txtNewItemQuantity" runat="server" Placeholder="Quantity" CssClass="form-control" Input="number"></asp:TextBox><br />
              <asp:TextBox ID="txtExpiryDate" runat="server" Placeholder="Select Expiry Date" CssClass="form-control" type="date"></asp:TextBox><br />
              <asp:Button ID="btnSaveNewItem" runat="server" Text="Save New Item" CssClass="btn btn-primary" OnClientClick="return validateNewItem();" OnClick="btnSaveNewItem_Click" />
          </div>

      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

<div class="modal fade" id="addOtherItemModal" tabindex="-1" role="dialog">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Add Item</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">

        <!-- New Item Form  -->
        <div id="newOtherItemForm" style="margin-top: 15px;">
          <h5>Enter New Item Details</h5>
          <asp:DropDownList ID="ddlOtherCategory" runat="server" CssClass="form-control">
                  <asp:ListItem Text="Select Category" Value="" Disabled="true" Selected="true"/>
              </asp:DropDownList><br />
  
          <asp:TextBox ID="txtOtherNewItemName" runat="server" Placeholder="Item Name" CssClass="form-control"></asp:TextBox><br />
 
          <asp:TextBox ID="txtOtherNewItemQuantity" runat="server" Placeholder="Quantity" CssClass="form-control" Input="number"></asp:TextBox><br />
     
          <asp:Button ID="btnSaveOtherNewItem" runat="server" Text="Save New Item" CssClass="btn btn-primary" OnClientClick="return validateOtherNewItem();" OnClick="btnSaveOtherNewItem_Click" />
        </div>

      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

   <%-- Expiry Date--%>
<div class="modal fade" id="expiryDateModal" tabindex="-1" role="dialog" aria-labelledby="expiryDateModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Select Expiry Date</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <asp:TextBox ID="txtExpiryDateModal" runat="server" CssClass="form-control" type="date"></asp:TextBox>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <asp:Button ID="btnSubmitExpiryDate" runat="server" CssClass="btn btn-primary" Text="Save Expiry Date" OnClick="btnSubmitExpiryDate_Click" />
      </div>
    </div>
  </div>
</div>

    <!-- Threshold Modal -->
<div class="modal fade" id="thresholdModal" tabindex="-1" role="dialog" aria-labelledby="thresholdModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Set Threshold for Item</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
          <div id="currentThresholdDiv" class="form-group" style="display: none;">
              <label for="currentLabel" style="font-weight: 600;">Current Threshold:</label>
              <span id="currentLabel"></span>
          </div>
        <div class="form-group">
          <label for="thresholdInput">Threshold</label>
          <input type="number" id="thresholdInput" class="form-control" placeholder="Enter threshold value" />
        <small> <em style="color: #606060;"> This value is used as a boundary to determine whether your inventory items are running low and require replenishment."</em></small>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-primary" onclick="saveThreshold()">Save</button>
      </div>
    </div>
  </div>
</div>

<div class="modal fade" id="pieChartModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Item Category Distribution</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div id="itemCategoryPieChart"></div> 
            </div>
        </div>
    </div>
</div>




   <script type="text/javascript">
       function triggerLoading() {
           showLoadingBar();
           setTimeout(function () {
               __doPostBack('<%= btnViewOther.UniqueID %>', '');
        }, 2000);
           return false;
       }

       function showLoadingBar() {
           document.getElementById('loadingBar').style.display = 'block';
       }

       function hideLoadingBar() {
           document.getElementById('loadingBar').style.display = 'none';
       }

       function toggleEditMode2(element) {
           var row = element.closest('tr');
         
           var lblItem = row.querySelector('.lblItem2');
           var txtItem = row.querySelector('.txtItem2');
           var lblQuantity = row.querySelector('.lblQuantity2');
           var txtQuantity = row.querySelector('.txtQuantity2');
      
           var minusIcon = row.querySelector('.minus2');
           var plusIcon = row.querySelector('.plus2');

           var deleteBtn = row.querySelector('.delete-button2');

           var fuItemImg = row.querySelector('.file-upload2');

           if (lblItem && txtItem && lblQuantity && txtQuantity && minusIcon && plusIcon && deleteBtn && fuItemImg) {
               var isEditMode = txtItem.style.display === 'none';
               if (isEditMode) {
             
                   lblItem.style.display = 'none';
                   txtItem.style.display = 'block';
                   lblQuantity.style.display = 'none';
                   txtQuantity.style.display = 'block';
                   minusIcon.style.display = 'inline-block';
                   plusIcon.style.display = 'inline-block';
                   element.innerHTML = '<i class="fas fa-save" style="color: #28a745; font-size: 18px; padding-right: 10px;"></i>';
                   
                   fuItemImg.style.display = 'block'; 
                   
                   deleteBtn.innerHTML = '<i class="fas fa-times" style="color: #dc3545; font-size: 18px;"></i>';
                   deleteBtn.onclick = function () {
                   
                       lblItem.style.display = 'block';
                       txtItem.style.display = 'none';
                       lblQuantity.style.display = 'block';
                       txtQuantity.style.display = 'none';
                       minusIcon.style.display = 'none';
                       plusIcon.style.display = 'none';
                       fuItemImg.style.display = 'none';

                       element.innerHTML = '<i class="fas fa-edit" style="color: #002080; font-size: 18px; padding-right: 10px;"></i>';
                       deleteBtn.innerHTML = '<i class="fas fa-trash" style="color: #dc3545; font-size: 18px;"></i>';
                       deleteBtn.onclick = function () {
                           return showDeleteConfirm(event, this);
                       };

                       return false;
                   };

                   return false;

               } else {
           
                   lblItem.style.display = 'block';
                   txtItem.style.display = 'none';
                   lblQuantity.style.display = 'block';
                   txtQuantity.style.display = 'none';
                   minusIcon.style.display = 'none';
                   plusIcon.style.display = 'none';
                   element.innerHTML = '<i class="fas fa-edit" style="color: #002080; font-size: 18px; padding-right: 10px;"></i>';

                   lblItem.textContent = txtItem.value;
                   lblQuantity.textContent = txtQuantity.value;

                   fuItemImg.style.display = 'none';

                   deleteBtn.innerHTML = '<i class="fas fa-trash" style="color: #dc3545; font-size: 18px;"></i>';
                   deleteBtn.onclick = function () {
                       return showDeleteConfirm(event, this);
                   };

                   return true;
               }
           } else {
               console.error('Elements not found.');
               return false;
           }
       }


       function changeQuantity2(element, delta) {
           var row = element.closest('tr');
           var txtQuantity = row.querySelector('.txtQuantity2');
           var lblQuantity = row.querySelector('.lblQuantity2');
           var qty = parseInt(txtQuantity.value || lblQuantity.textContent);

           qty += delta;
           if (qty < 0) qty = 0; // ensure quantity doesn't go below 0

           lblQuantity.textContent = qty;
           txtQuantity.value = qty;
       }

       function toggleEditMode(element) {
           var row = element.closest('tr');

           var lblItem = row.querySelector('.lblItem');
           var txtItem = row.querySelector('.txtItem');

   
           var lblQuantity = row.querySelector('.lblQuantity');
           var txtQuantity = row.querySelector('.txtQuantity');

           var minusIcon = row.querySelector('.minus');
           var plusIcon = row.querySelector('.plus');

     
           var lblExpiryDate = row.querySelector('.lblExpiryDate');
           var txtExpiryDate = row.querySelector('.txtExpiryDate');

           var deleteBtn = row.querySelector('.delete-button');

         
           var fuItemImg = row.querySelector('.file-upload');

           if (lblItem && txtItem && lblQuantity && txtQuantity && lblExpiryDate && txtExpiryDate && minusIcon && plusIcon && deleteBtn && fuItemImg) {
               var isEditMode = txtItem.style.display === 'none';

               if (isEditMode) {
                
                   lblItem.style.display = 'none';
                   txtItem.style.display = 'block';

                   lblQuantity.style.display = 'none';
                   txtQuantity.style.display = 'block';

                   lblExpiryDate.style.display = 'none';
                   txtExpiryDate.style.display = 'block';
                   txtExpiryDate.type = 'date';  

                   minusIcon.style.display = 'inline-block';
                   plusIcon.style.display = 'inline-block';
                 
                
                   fuItemImg.style.display = 'block'; 
                   
                   element.innerHTML = '<i class="fas fa-save" style="color: #28a745; font-size: 18px; padding-right: 10px;"></i>'; // Change icon to save

                   deleteBtn.innerHTML = '<i class="fas fa-times" style="color: #dc3545; font-size: 18px;"></i>';
                   deleteBtn.onclick = function () {

                       lblItem.style.display = 'block';
                       txtItem.style.display = 'none';
                       lblQuantity.style.display = 'block';
                       txtQuantity.style.display = 'none';
                       minusIcon.style.display = 'none';
                       plusIcon.style.display = 'none';
                       lblExpiryDate.style.display = 'block';
                       txtExpiryDate.style.display = 'none';

                       fuItemImg.style.display = 'none'; 

                       element.innerHTML = '<i class="fas fa-edit" style="color: #002080; font-size: 18px; padding-right: 10px;"></i>';
                       deleteBtn.innerHTML = '<i class="fas fa-trash" style="color: #dc3545; font-size: 18px;"></i>';
                       deleteBtn.onclick = function () {
                           return showDeleteConfirm(event, this);
                       };

                       return false;
                   };

                   return false;

               } else {
          
                   lblItem.style.display = 'block';
                   txtItem.style.display = 'none';

                   lblQuantity.style.display = 'block';
                   txtQuantity.style.display = 'none';

                   lblExpiryDate.style.display = 'block';
                   txtExpiryDate.style.display = 'none';

                   minusIcon.style.display = 'none';
                   plusIcon.style.display = 'none';

                   fuItemImg.style.display = 'none'; 

                   lblItem.textContent = txtItem.value;
                   lblQuantity.textContent = txtQuantity.value;
                   lblExpiryDate.textContent = txtExpiryDate.value;

                   element.innerHTML = '<i class="fas fa-edit" style="color: #002080; font-size: 18px;"></i>'; // Change icon back to edit

                   deleteBtn.innerHTML = '<i class="fas fa-trash" style="color: #dc3545; font-size: 18px;"></i>';
                   deleteBtn.onclick = function () {
                       return showDeleteConfirm(event, this);
                   };

                   return true;
               }
           } else {
               console.error('Elements not found.');

               return false;
           }
       }


       // change quantity interactively using plus/minus buttons
       function changeQuantity(element, delta) {
           var row = element.closest('tr');
           var txtQuantity = row.querySelector('.txtQuantity');
           var lblQuantity = row.querySelector('.lblQuantity');
           var qty = parseInt(txtQuantity.value || lblQuantity.textContent);

           // update quantity
           qty += delta;
           if (qty < 0) qty = 0;

           lblQuantity.textContent = qty;
           txtQuantity.value = qty;
       }

             

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

       function showDeleteConfirm(event, element) {
           event.preventDefault();
           var inventoryId = element.getAttribute('data-command-argument');
           console.log(inventoryId);
         
           Swal.fire({
               title: 'Are you sure?',
               text: "Have you used up all items or do you just want to remove the data?",
               icon: 'warning',
               showDenyButton: true,
               showCancelButton: true,
               confirmButtonText: 'Finished All Items',
               denyButtonText: 'Remove Item',
               cancelButtonText: 'Cancel',
               confirmButtonColor: '#3085d6',
               denyButtonColor: '#FF9900',
               cancelButtonColor: '#d33'
           }).then((result) => {
               if (result.isConfirmed) {
                   
                   itemOut(inventoryId, element);
               } else if (result.isDenied) {
                  
                   removeItem(inventoryId, element);
               }
           });

           return false; 
       }

       function itemOut(inventoryId, element) {
           $.ajax({
               type: "POST",
               url: "OrgInventory2.aspx/itemOut", 
               data: JSON.stringify({ inventoryId: inventoryId }),
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   Swal.fire('Deleted!', 'The item has been removed.', 'success').then(() => {
                       $(element).closest('tr').remove(); 
                   });
               },
               error: function (xhr, status, error) {
                   Swal.fire('Error!', 'There was a problem removing the item.', 'error');
               }
           });
       }

       function removeItem(inventoryId, element) {
           $.ajax({
               type: "POST",
               url: "OrgInventory2.aspx/DeleteItem", 
               data: JSON.stringify({ inventoryId: inventoryId }),
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   Swal.fire('Deleted!', 'The item has been deleted.', 'success').then(() => {
                       $(element).closest('tr').remove(); 
                   });
               },
               error: function (xhr, status, error) {
                   Swal.fire('Error!', 'There was a problem deleting the item.', 'error');
               }
           });
       }


       function showAddModal() {
           $('#addItemModal').modal('show');
       }

       function showExistingItems() {
           document.getElementById('existingItemsSection').style.display = 'block';
           document.getElementById('newItemForm').style.display = 'none';
           
       }

       function showNewItemForm() {
           document.getElementById('newItemForm').style.display = 'block';
           document.getElementById('existingItemsSection').style.display = 'none';
       }

       function validateNewItem() {
           var category = document.getElementById('<%= ddlCategory.ClientID %>').value;
           var itemName = document.getElementById('<%= txtNewItemName.ClientID %>').value;
           var quantity = document.getElementById('<%= txtNewItemQuantity.ClientID %>').value;
           var expiryDate = document.getElementById('<%= txtExpiryDate.ClientID %>').value;

           if (category === "" || itemName === "" || quantity === "" || expiryDate === "") {
               showError("All fields are required.");
               return false; 
           }
            return true; 
        }

       function validateOtherNewItem() {
           var category = document.getElementById('<%= ddlOtherCategory.ClientID %>').value;
           var itemName = document.getElementById('<%= txtOtherNewItemName.ClientID %>').value;
           var quantity = document.getElementById('<%= txtOtherNewItemQuantity.ClientID %>').value;
          
           if (category === "" || itemName === "" || quantity === "" ) {
               showError("All fields are required.");
               return false;
           }
           return true;
       }

       function openThresholdModal(event, element) {
           event.preventDefault();

           var inventoryId = $(element).attr('data-command-argument');

           $('#hfThreshold').val(inventoryId);

           $.ajax({
               type: "POST",
               url: "OrgInventory2.aspx/GetThreshold",
               data: JSON.stringify({ inventoryId: inventoryId }),
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   if (response.d) {
                       $('#currentLabel').text(response.d);
                       $('#currentThresholdDiv').show();
                   } else {
                       $('#currentThresholdDiv').hide(); 
                   }

                   $('#thresholdModal').modal('show');
               },
               error: function (xhr, status, error) {
                   console.error("Error fetching threshold:", error);
                   $('#thresholdModal').modal('show');
               }
           });
       }

       function saveThreshold() {
           var inventoryId = $('#hfThreshold').val();

           var threshold = $('#thresholdInput').val();

           if (!threshold) {
               showError('Please enter a valid threshold value.');
               return;
           }

           console.log("Inventory ID: ", inventoryId);

           $.ajax({
               type: "POST",
               url: "OrgInventory2.aspx/SaveThreshold",
               data: JSON.stringify({ inventoryId: inventoryId, threshold: parseInt(threshold) }),
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   
                   $('#thresholdModal').modal('hide');
                   showSuccess('Threshold saved successfully.');
               },
               error: function (xhr, status, error) {
                   showError('Error saving threshold. Please try again.');
                   //console.error("Test Method Error:", xhr, status, error);
               }
           });
       }

       function closeSideView(event) {
        
           if (event) {
               event.preventDefault(); 
           }

           var sideView = document.getElementById('<%= sideViewContainer.ClientID %>'); 
           if (sideView) {
               sideView.style.right = '-30%';  w
           }
       }

       function showPieChartModal(event) {
           if (event) {
               event.preventDefault();
           }

           $('#pieChartModal').modal('show');  
           loadPieChart();  
       }

       function loadPieChart() {
      
           $.ajax({
               type: "POST",
               url: "OrgInventory2.aspx/GetItemCategory",  
               contentType: "application/json; charset=utf-8",
               dataType: "json",
               success: function (response) {
                   var chartData = response.d;  

                   var chart = new FusionCharts({
                       type: 'pie2d',
                       renderAt: 'itemCategoryPieChart',
                       width: '100%',
                       height: '400',
                       dataFormat: 'json',
                       dataSource: {
                           "chart": {
                               "caption": "Item Categories in Inventory",
                               "bgColor": "#ffffff",  
                               "theme": "fusion"
                           },
                           "data": chartData
                       }
                   });

                   chart.render();
               },
               error: function (error) {
                   console.error("Error fetching data for pie chart", error);
               }
           });
       }

   </script>


</asp:Content>
