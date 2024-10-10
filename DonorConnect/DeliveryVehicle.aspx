<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryVehicle.aspx.cs" Inherits="DonorConnect.DeliveryVehicle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <table class="table table-bordered">
        
        <thead>
            <tr>
                <th>Vehicle Type</th>
                <th>Sample Vehicle View</th>
                <th>Measurement (meters)</th>
                <th>Weight Limit (kilograms)</th>
                <th>Dimensions (meters)</th>
            </tr>
        </thead>
        <tbody>
           <!-- 4x4 Pickup -->
             <tr>
                 <td><strong>4x4 Pickup</strong>
                     <p style="font-weight: lighter; padding-top: 20px;">Recommended items: seater sofa, end tables, bicycle etc.</p>
                 </td>
                 <td>
                     <img src="/Image/4x4.png" alt="4x4 Pickup Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                 </td>
                 <td>
                    <img src="/Image/4x4_m1.png" alt="4x4 Pickup Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                   
                </td>
                  <td>
                    <p>40kg</p>
   
                </td>
                 <td>
                     <p>Length: 2.2m</p>
                     <p>Width: 1.1m</p>
                     <p>Height: 0.5m</p>
                 </td>
             </tr>
            <!-- Van Row -->
            <tr>
                <td><strong>Small Van 7 Feet</strong>
                    <p style="font-weight: lighter; padding-top: 20px;">Recommended items: seater sofa, refrigerator, small-sized mixed furniture etc.</p>
                </td>
                <td>
                    <img src="/Image/van7feet.png" alt="Van Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                </td>
                 <td>
                    <img src="/Image/van7feet_m1.jpg" alt="Van Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                   
                </td>
                 <td>
                    <p>500kg</p>
   
                </td>
                <td>
                    <p>Length: 1.7m</p>
                    <p>Width: 1.1m</p>
                    <p>Height: 1.2m</p>
                </td>
            </tr>
            <tr>
            <td><strong>Big Van 9 Feet</strong>
                <p style="font-weight: lighter; padding-top: 20px;">Recommended items: single bed, coffee tables etc.</p>
            </td>
            <td>
                <img src="/Image/van9feet.png" alt="Van Dimensions" style="width: 100%; max-width: 150px; height: auto;">
            </td>
             <td>
                <img src="/Image/van9feet_m1.jpg" alt="Van Dimensions" style="width: 100%; max-width: 150px; height: auto;">
       
            </td>
            <td>
                <p>800kg</p>
   
            </td>
            <td>
                <p>Length: 2.7m</p>
                <p>Width: 1.3m</p>
                <p>Height: 1.2m</p>
            </td>
               
        </tr>
            <!-- Truck Row -->
            <tr>
                <td><strong>Small Lorry 10 Feet</strong>
                    <p style="font-weight: lighter; padding-top: 20px;">Recommended items: single bed, sofa set, large refrigerator, printer set etc.</p>
                </td>
                <td>
                    <img src="/Image/lorry10feet.png" alt="Truck Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                </td>
                 <td>
                    <img src="/Image/lorry10feet_m1.png" alt="Truck Dimensions" style="width: 100%; max-width: 150px; height: auto;">
       
                </td>
                 <td>
                    <p>1000kg</p>
   
                </td>
                <td>
                    <p>Length: 2.75m</p>
                    <p>Width: 1.52m</p>
                    <p>Height: 1.52m</p>
                </td>
            </tr>
            <tr>
            <td><strong>Medium Lorry 14 Feet</strong>
                <p style="font-weight: lighter; padding-top: 20px;">Recommended items: King size bed, dining table set, cupboards etc.</p>
            </td>
            <td>
                <img src="/Image/lorry14feet.png" alt="Truck Dimensions" style="width: 100%; max-width: 150px; height: auto;">
            </td>
             <td>
                <img src="/Image/lorry14feet_m1.png" alt="Truck Dimensions" style="width: 100%; max-width: 150px; height: auto;">
       
            </td>
             <td>
                <p>2500kg</p>
   
            </td>
            <td>
                <p>Length: 4.27m</p>
                <p>Width: 2.2m</p>
                <p>Height: 2.13m</p>
            </td>
        </tr>
            <tr>
                <td><strong>Big Lorry 17 Feet</strong>
                    <p style="font-weight: lighter; padding-top: 20px;">Recommended items: Queen size bed, large wardrobe, gym equipment etc.</p>
                </td>
                <td>
                    <img src="/Image/lorry17feet.png" alt="Truck Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                </td>
                <td>
                    <img src="/Image/lorry17feet_m1.png" alt="Truck Dimensions" style="width: 100%; max-width: 150px; height: auto;">
                </td>
                <td>
                    <p>4000kg</p>

                </td>
                <td>
                    <p>Length: 5.1m</p>
                    <p>Width: 2.0m</p>
                    <p>Height: 2.10m</p>
                </td>
            </tr>
        </tbody>
    </table>
    <p class="mt-3">The images shown above are only for reference purposes. Please ensure your furniture can fit into the selected vehicle for delivery.</p>
</div>
</asp:Content>
