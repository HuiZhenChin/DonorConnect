<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Favourites.aspx.cs" Inherits="DonorConnect.Favourites" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Item Donations</title>
    
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/2.9.3/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

   
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

     .dropdown {
    position: relative;
    display: inline-block;
}

    .dropdown-toggle {
       
        color: white;
        border: none;
        padding: 10px 20px;
        font-size: 16px;
        cursor: pointer;
    }

    .dropdown-menu {
        display: none;
        position: absolute;
        background-color: #f9f9f9;
        min-width: 160px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        z-index: 1;
    }

    .dropdown-menu .dropdown-submenu {
        position: relative;
    }

    .dropdown-menu .dropdown-item {
        color: #000;
        padding: 12px 16px;
        text-decoration: none;
        display: block;
    }

    .dropdown-menu .dropdown-item:hover {
        background-color: #f1f1f1;
    }

    /* Submenu styles */
    .submenu {
        display: none;
        position: absolute;
        left: 100%;
        top: 0;
        background-color: #f9f9f9;
        min-width: 160px;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
    }

    .dropdown-submenu:hover .submenu {
        display: block;
    }

    .submenu-item {
        padding: 12px 16px;
        text-decoration: none;
        display: block;
        color: #000;
    }

    .submenu-item:hover {
        background-color: #f1f1f1;
    }

    /* Show the dropdown menu on hover */
    .dropdown:hover .dropdown-menu {
        display: block;
    }

    /* Container for category columns */
    .category-container {
        display: flex; 
        flex-wrap: wrap; 
        gap: 10px; 
        padding: 10px; 
    }

    /* Style for each category column */
    .category-column {
        flex: 1 1 200px; 
        box-sizing: border-box;
        margin: 10px;
        border: 1px solid #ccc; 
        padding: 10px; 
    }

    /* Style for checkboxes in category columns */
    .category-checkbox {
        display: block;
        margin-bottom: 10px;
    }

    /* Style for items within each category */
    .item-column {
        margin-bottom: 5px;
    }

    /* Style for checkboxes in item columns */
    .item-checkbox {
        display: block;
    }

    /* Container for state cards */
    .state-container {
        display: flex; 
        flex-wrap: wrap; 
        gap: 10px; 
        padding: 10px; 
    }

    /* Style for each state card */
    .state-card {
        flex: 1 1 150px; 
        box-sizing: border-box;
        margin: 10px;
        border: 1px solid #ccc; 
        padding: 10px;
    }

    /* Style for checkboxes in state cards */
    .state-checkbox {
        display: block;
        margin: 0;
    }

    .urgent-card {
        background-color: #FFF5EE;
    }

    .card-body {
        position: relative;
        padding-top: 20px; 
    }

    .urgent-card::before {
        content: "";
        position: absolute;
        top: -8px;
        left: 0;
        width: 100%;
        height: 8px; 
        background: repeating-linear-gradient(
            45deg,
            #FAA0A0,
            #FAA0A0 10px,
            white 10px,
            white 20px
        );
        z-index: 1; 
        border-radius: 5px 5px 0 0; 

    }

    .image-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
        gap: 10px;
    }

    .image-item img {
        width: 200px;
        height: 200px;
        object-fit: cover;
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <asp:Label ID="lblFav" runat="server" CssClass="d-block text-center mb-4" Text="Saved Donations" Style="font-weight: bold; font-size: 24px;" ></asp:Label>

      <asp:Label ID="noSavedFav" runat="server" CssClass="d-block text-center mb-4" Text="No saved donations yet" Style="font-size: 18px;" ></asp:Label>
    
    <asp:GridView ID="gvFavourites" runat="server" AutoGenerateColumns="False" CssClass="centered-grid" DataKeyNames="donationPublishId" GridLines="None" BorderStyle="None" CellPadding="0">
      <Columns>
          <asp:TemplateField>
              <ItemTemplate>
                  <div class="row">
                      <%-- Profile Picture and Organization Name --%>
                      <div class="col-md-3 d-flex">
                          <img src='<%# String.IsNullOrEmpty(Eval("orgProfilePic") as string) ? "/Image/default_picture.jpg" : "data:image/png;base64," + Eval("orgProfilePic") %>'
                               alt="Org Profile Picture"
                               style="width: 100px; height: 100px; border-radius: 50%; border: 1px solid black;" />
                          <strong class="ml-2"><%# Eval("orgName") %></strong>
                      </div>

                      <%-- Card Body with Donation Details --%>
                    <div class="col-md-9">
                      <div class="card mb-4 shadow-sm card-custom" style="width: 100%!important;">
                          <div class="card-body <%# Eval("cardBody") %>" style="width: 100%!important;">
                              
                              <div class="row">
                                  <div class="col-md-8">
                                      <%-- URGENT Label --%>
                                      <%# Eval("urgentLabel") %>
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
                                                  <asp:LinkButton class="fas fa-trash " style="font-size: 24px; color: black; cursor: pointer; padding-right: 20px;" title="Remove from Favourites" runat="server" CommandArgument='<%# Eval("donationPublishId") %>' OnClick="btnDeleteFav_Click"></asp:LinkButton>
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
      </Columns>
  </asp:GridView>

   
    <script type="text/javascript">
        function confirmDelete(donationPublishId) {
            Swal.fire({
                title: 'Are you sure?',
                text: 'This will remove the donation from your favourite list.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    
                    $.ajax({
                        type: 'POST',
                        url: 'Favourites.aspx/DeleteFavorite',
                        data: JSON.stringify({ donationPublishId: donationPublishId }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (response) {
                            if (response.d === 'Success') {
                               
                                Swal.fire(
                                    'Deleted!',
                                    'Your item has been removed from the list.',
                                    'success'
                                ).then(() => {
                                   
                                    window.location.href = 'Favourites.aspx';
                                });
                            } else {
                                Swal.fire(
                                    'Error!',
                                    'There was a problem deleting the item.',
                                    'error'
                                );
                            }
                        },
                        error: function () {
                            Swal.fire(
                                'Error!',
                                'There was a problem with the server.',
                                'error'
                            );
                        }
                    });
                }
            });
        }
    </script>


</asp:Content>
