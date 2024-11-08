<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgDashboard.aspx.cs" Inherits="DonorConnect.OrgInventory" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        body{
            background: rgb(249,247,247);
            background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
        }

        .sidebar {
            height: 100vh;
            width: 60px;
            position: fixed;
            top: 0;
            left: 0;
            background-color: #1E1E1E;
            overflow-x: hidden;
            transition: width 0.3s;
            border-radius: 0;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
           
        }

        .sidebar.expanded {
            width: 250px;
        }

        .sidebar-menu {
            list-style-type: none;
            padding: 0;
            margin: 0;
        }

        .sidebar-menu li {
            text-align: left;
            padding: 15px;
            color: white;
        }

        .sidebar-menu li a {
            display: flex;
            align-items: center;
            text-decoration: none;
            color: white;
        }

        .sidebar-menu li a .icon {
            font-size: 20px;
            margin-right: 10px;
        }

        .sidebar-menu li a .text {
            display: none;
            transition: opacity 0.3s ease;
        }

        .sidebar.expanded .text {
            display: inline;
        }

        .content {
            margin-left: 80px;
            padding: 20px;
            box-sizing: border-box;
            height: 100vh;
            overflow-y: auto;
            transition: margin-left 0.3s, width 0.3s; 
            width: calc(100% - 80px);
        }

        .content.shrink {
            margin-left: 250px; 
            width: calc(100% - 250px); 
        }

        h1 {
            text-align: center;
        }

        .dashboard-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr); 
            grid-template-rows: auto auto 1fr; 
            grid-gap: 20px; 
            width: 100%; 
            height: calc(100vh - 120px); 
        }

        .title{
            font-size: 1.8em;
            font-weight: bold;
            padding-bottom: 10px;
            color: #333;
            margin-bottom: 20px;
        }

        .box {
            background-color: #f0f4f7;
            padding: 30px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            font-size: 1.2em;
        }

        .box .aspLabel {
            font-family: 'Roboto', sans-serif;
            font-size: 24px; 
            font-weight: 700; 
            color: #333; 
        }

        .box small {
            font-family: 'Roboto', sans-serif;
            font-size: 14px;
            font-weight: 400; 
            color: black; 
        }

        .small-box {
            height: 150px;
        }

        .long-box {
            grid-column: span 4; 
            
        }

        .column-box {
            grid-column: span 2; 
            height: 300px;
        }

        @media screen and (max-width: 1024px) {
            .dashboard-grid {
                grid-template-columns: 1fr 1fr; 
            }

            .long-box, .column-box {
                grid-column: span 2; 
            }
        }

        @media screen and (max-width: 600px) {
            .dashboard-grid {
                grid-template-columns: 1fr; 
            }

            .small-box, .long-box, .column-box {
                grid-column: span 1; 
            }
        }

        @media (max-width: 768px) {
            .sidebar {
                width: 0;
                height: 100vh;
                position: fixed;
                top: 0;
                left: 0;
                background-color: #1E1E1E;
                overflow-y: auto;
                transition: width 0.3s;
                padding-top: 60px;
            }

            .sidebar.expanded {
                width: 250px;
            }

            .content {
                margin-left: 0;
                width: 100%;
            }

            .hamburger-menu {
                display: block;
                position: fixed;
                top: 15px;
                left: 15px;
                cursor: pointer;
                font-size: 24px;
                color: white;
                z-index: 10000 !important;
            }

            .sidebar.expanded + .hamburger-menu {
                color: white;
            }
        }
    </style>

     <script>

         document.addEventListener('DOMContentLoaded', function () {
             var sidebar = document.getElementById('sidebar');
             var content = document.getElementById('content');
             var hamburgerMenu = document.getElementById('hamburgerMenu');


             if (sidebar) {
                 sidebar.addEventListener('mouseenter', function () {
                     this.classList.add('expanded');
                     content.classList.add('shrink');
                 });

                 sidebar.addEventListener('mouseleave', function () {
                     this.classList.remove('expanded');
                     content.classList.remove('shrink');
                 });
             } else {
                 console.error('Sidebar element not found.');
             }
         });

         function toggleSidebar() {
             sidebar.classList.toggle('expanded');
             content.classList.toggle('shrink');

             if (sidebar.classList.contains('expanded')) {
                 hamburgerMenu.style.color = "white";
             } else {
                 hamburgerMenu.style.color = "black";
             }
         }

         function showSuccess(message) {
             Swal.fire({
                 text: message,
                 icon: 'success',
                 confirmButtonText: 'OK',
             });
         }

         function showInfo(message) {
             Swal.fire({
                 text: message,
                 icon: 'info',
                 confirmButtonText: 'OK',
             });
         }

         function markAsRead() {
             __doPostBack('MarkAsRead', '');
         }

         document.addEventListener('DOMContentLoaded', function () {
             if (typeof deliveryData !== 'undefined' && deliveryData.length > 0) {
                 const order = [
                     "Waiting for delivery rider",
                     "Accepted",
                     "Delivering in progress",
                     "Reached Destination",
                     "Cancelled"
                 ];

                 const statusCountMap = {
                     "Waiting for delivery rider": 0,
                     "Accepted": 0,
                     "Delivering in progress": 0,
                     "Reached Destination": 0,
                     "Cancelled": 0
                 };

                 deliveryData.forEach(d => {
                     if (statusCountMap.hasOwnProperty(d.status)) {
                         statusCountMap[d.status] = d.count;
                     }
                 });

                 const labels = order;
                 const data = order.map(status => statusCountMap[status]);

                 const backgroundColors = [
                     '#FAEAB1', '#E5BA73', '#FFDCA9', '#F4B183', '#DFA67B'
                 ];

                 const borderColors = [
                     '#C58940', '#C58940', '#C58940', '#C58940', '#C58940'
                 ];

                 var ctx = document.getElementById('deliveryStatusChart').getContext('2d');
                 var deliveryStatusChart = new Chart(ctx, {
                     type: 'bar',
                     data: {
                         labels: labels,
                         datasets: [{
                             label: 'Delivery Status Count',
                             data: data,
                             backgroundColor: backgroundColors,
                             borderColor: borderColors,
                             borderWidth: 1
                         }]
                     },
                     options: {
                         plugins: {
                             title: {
                                 display: true,
                                 text: 'Delivery Status Overview',
                                 font: { size: 20 }
                             }
                         },
                         scales: {
                             y: { beginAtZero: true }
                         }
                     }
                 });
             } else {
                 renderEmptyDeliveryStatusChart();
             }
         });

         function renderEmptyDeliveryStatusChart() {
             const ctx = document.getElementById('deliveryStatusChart').getContext('2d');

             new Chart(ctx, {
                 type: 'bar',
                 data: {
                     labels: ["No data available"],
                     datasets: [{
                         label: "No Data",
                         data: [0],
                         backgroundColor: 'rgba(211, 211, 211, 0.5)',
                         borderColor: 'grey',
                         borderWidth: 1
                     }]
                 },
                 options: {
                     plugins: {
                         title: {
                             display: true,
                             text: "Delivery Status Overview",
                             font: { size: 20 }
                         }
                     },
                     scales: {
                         x: { display: true },
                         y: { display: true }
                     }
                 }
             });
         }

         document.addEventListener('DOMContentLoaded', function () {
             if (typeof itemCategoryData !== 'undefined' && itemCategoryData.length > 0) {
                 const categories = Array.from(new Set(itemCategoryData.map(d => d.itemCategory)));
                 const statusTypes = ["Approved", "Pending", "Rejected"];

                 const datasets = statusTypes.map(status => ({
                     label: status,
                     data: categories.map(category => {
                         const match = itemCategoryData.find(d => d.itemCategory === category && d.requestStatus === status);
                         return match ? match.count : 0;
                     }),
                     backgroundColor: getStatusColor(status),
                     borderColor: getStatusColor(status),
                     borderWidth: 1
                 }));

                 function getStatusColor(status) {
                     switch (status) {
                         case "Approved": return '#D1BB9E';
                         case "Pending": return '#AF8F6F';
                         case "Rejected": return '#74512D';
                         default: return '#543310';
                     }
                 }

                 var ctx = document.getElementById('itemCategoryChart').getContext('2d');
                 var itemCategoryChart = new Chart(ctx, {
                     type: 'bar',
                     data: {
                         labels: categories,
                         datasets: datasets
                     },
                     options: {
                         plugins: {
                             title: {
                                 display: true,
                                 text: 'Item Category Status Overview',
                                 font: { size: 20 }
                             },
                             tooltip: {
                                 callbacks: {
                                     label: function (tooltipItem) {
                                         return `${tooltipItem.dataset.label}: ${tooltipItem.raw}`;
                                     }
                                 }
                             }
                         },
                         scales: {
                             x: { stacked: true },
                             y: { beginAtZero: true, stacked: true }
                         }
                     }
                 });
             } else {
                 renderEmptyItemCategoryChart();
             }
         });

         function renderEmptyItemCategoryChart() {
             const ctx = document.getElementById('itemCategoryChart').getContext('2d');

             new Chart(ctx, {
                 type: 'bar',
                 data: {
                     labels: ["No data available"],
                     datasets: [{
                         label: "No Data",
                         data: [0],
                         backgroundColor: 'rgba(211, 211, 211, 0.5)',
                         borderColor: 'grey',
                         borderWidth: 1
                     }]
                 },
                 options: {
                     plugins: {
                         title: {
                             display: true,
                             text: "Item Category Status Overview",
                             font: { size: 20 }
                         }
                     },
                     scales: {
                         x: { display: true },
                         y: { display: true }
                     }
                 }
             });
         }



     </script>
</head>
<body>

     <div class="hamburger-menu" id="hamburgerMenu" onclick="toggleSidebar()" style="color: black;"> 
     <i class="fas fa-bars"></i>
    </div>
    
    <div class="sidebar" id="sidebar">
        <ul class="sidebar-menu">
            <li><a href="OrgDashboard.aspx"><span class="icon">🏠</span><span class="text">Home</span></a></li>
            <li><a href="OrgDonations.aspx"><span class="icon">🤝🏻</span><span class="text">Donations</span></a></li>
            <li><a href="Delivery.aspx"><span class="icon">🚚</span><span class="text">Delivery</span></a></li>
            <li><a href="OrgViewDonationRequest.aspx"><span class="icon">🆕</span><span class="text">Donation Requests</span></a></li>
            <li><a href="OrgAnalytics.aspx"><span class="icon">📊</span><span class="text">Analytics</span></a></li>
            <li><a href="OrgInventory2.aspx"><span class="icon">📦</span><span class="text">Inventory</span></a></li>
            <li><a href="OrgReport.aspx"><span class="icon">📋</span><span class="text">Report</span></a></li>
            <li><a href="OrgCreateSignature.aspx"><span class="icon">🖋️</span><span class="text">Receipt Signature</span></a></li>
            <li><a href="OrgSettings.aspx"><span class="icon">⚙️</span><span class="text">Settings</span></a></li>
        </ul>

    </div>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:HiddenField ID="hfMarkAsRead" runat="server" />

    <div class="content" id="content">
        <h2 class="title">Welcome to Dashboard</h2>
      
        <div class="dashboard-grid">
            <!-- Top small boxes -->
            <div class="box small-box" style="background-color: #EED690;">
                <asp:Label ID="newRequestBox" runat="server" Text="" CssClass="aspLabel"></asp:Label>
                <br />
                <small>New Request</small>
            </div>

            <!-- Ongoing Delivery -->
            <div class="box small-box" style="background-color: #6AC1B8;">
                <asp:Label ID="ongoingDeliveryBox" runat="server" Text="" CssClass="aspLabel"></asp:Label>
                <br />
                <small>Ongoing Delivery</small>
            </div>

            <!--Completed Request -->
            <div class="box small-box" style="background-color: #1AA59A;">
                <asp:Label ID="completedRequestBox" runat="server" Text="" CssClass="aspLabel"></asp:Label>
                <br />
                <small>Completed Request</small>
            </div>

            <!-- Request Made -->
            <div class="box small-box" style="background-color: #236a8e;">
                <asp:Label ID="requestMadeBox" runat="server" Text="" CssClass="aspLabel"></asp:Label>
                <br />
                <small>Request Made</small>
            </div>

            <!-- Middle long box -->
            <div class="box long-box" style="background-color: #FAF8F1;">
                <canvas id="deliveryStatusChart" style= "width: auto; height: auto; align-items: center;"></canvas>

            </div>

            <div class="box long-box" style="background-color: #F1F1E8;">
                <canvas id="itemCategoryChart" style= "width: auto; height: auto; align-items: center;"></canvas>

            </div>
        </div>
    </div>
        </form>
   
</body>
</html>
