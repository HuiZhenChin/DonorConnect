<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgAnalytics.aspx.cs" Inherits="DonorConnect.OrgAnalytics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <title>Organization Inventory</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript" src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.js"></script>
    <script type="text/javascript" src="https://cdn.fusioncharts.com/fusioncharts/latest/fusioncharts.charts.js"></script>  
    <link href='https://cdn.jsdelivr.net/npm/fullcalendar@5.10.2/main.min.css' rel='stylesheet' />
    <script src='https://cdn.jsdelivr.net/npm/fullcalendar@5.10.2/main.min.js'></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap" rel="stylesheet">

    <style>
   
     body{
         background: rgb(249,247,247);
         background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
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

    .box {
        background-color: #f0f4f7;
        padding: 30px;
        text-align: center;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        font-size: 1.2em;
    }

    .small-box {
        height: 150px;
    }

    .column-box {
        grid-column: span 2; 
        height: auto;
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

    .chart-navigation-container {
        display: flex;
        align-items: center;
        justify-content: space-between;
        grid-column: span 2;
        height: auto;
        background-color: #f0f4f7;
        padding: 30px;
        text-align: center;
        border-radius: 10px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

    #chartContainer {
        flex-grow: 1;
        margin: 0 10px; 
    }

    #chartContainer2 {
        flex-grow: 1;
        margin: 0 10px; 
    }

    .chart-nav-btn {
        background-color: transparent;
        color: white;
        border: none;
        padding: 10px 20px;
        font-size: 16px;
        cursor: pointer;
        border-radius: 5px;
        display: flex;
        align-items: center;
    }

    .chart-nav-btn:hover {
        background-color: #005fa3;
    }

    .chart-nav-btn i {
        margin: 0 5px;
        color: black;
    }

    .box.long-box {
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 0;
        height: 100%; 
        width: 100%;
        overflow: visible; 
        grid-column: span 4; 
    }

    #calendar {
        width: 100%;
        height: 100%;
        max-width: 100%;
        box-sizing: border-box;
        overflow: hidden;
        padding: 20px;
        
    }

    .calendar-title {
        text-align: center;
        font-size: 24px;
        margin-bottom: 10px;
    }

    .legend-container {
        display: initial;
        justify-content: center;
        flex-direction: column; 
        align-items: center;
        margin-bottom: 20px;
        padding-right: 10px;
    }

    .legend-item {
        display: flex;
        align-items: center;
        margin-bottom: 5px; 
    }

    .legend-color {
        width: 20px;
        height: 20px; 
        display: inline-block;
        margin-right: 10px; 
    }

    .legend-label {
        font-size: 16px;
    }

    .fc .fc-toolbar-title{
        font-size: 24px;
       
    }

    #stockLevelChart {
        width: 100%;
        height: 100%;
        max-width: 100%;
        box-sizing: border-box;
        overflow: hidden;
        padding: 20px;
    
    }

    .dropdown-month {
        position: absolute;
        top: 10px;
        right: 10px;
        z-index: 10; 
        padding: 8px;
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

</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="content" id="content">
      
      <div class="dashboard-grid">
          <!-- Top small boxes -->
          <div class="box small-box" style="background-color: #F0EEC8;">
            <asp:Label ID="totalItemBox" runat="server" CssClass="aspLabel" Text=""></asp:Label>
            <br />
            <small>Total Items</small>
        </div>

        <div class="box small-box" style="position: relative; background-color: #B2D3BE;">
            <i class="fas fa-info-circle info-icon" onclick="showLowStockModal()"
                style="position: absolute; top: 10px; right: 10px; cursor: pointer; color: #053B50;"></i>

            <!-- Low Stock Label -->
            <asp:Label ID="lowStockBox" runat="server" CssClass="aspLabel" Text=""></asp:Label>
            <br />
            <small>Low Stock Items</small>
        </div>

        <div class="box small-box" style="background-color: #89A3B2;">
            <asp:Label ID="expiredBox" runat="server" CssClass="aspLabel" Text=""></asp:Label>
            <br />
            <small>Items</small>
        </div>

        <div class="box small-box" style="background-color: #1F6F8B;">
            <asp:Label ID="mostUsedCategoryBox" runat="server" CssClass="aspLabel" Text=""></asp:Label>
            <br />
            <small>Most Used Category</small>
        </div>


          <div class="box long-box" style="background-color: #F2F6F5;">
              <h2 class="calendar-title">Expiry Date Tracking</h2>

                <div class="legend-container">
                    <div class="legend-item">
                        <span class="legend-color" style="background-color: #FF6961;"></span>
                        <span class="legend-label">Expired</span>
                    </div>
                    <div class="legend-item">
                        <span class="legend-color" style="background-color: #FFCA4B;"></span>
                        <span class="legend-label">Expiring</span>
                    </div>
                    <div class="legend-item">
                        <span class="legend-color" style="background-color: #44D044;"></span>
                        <span class="legend-label">Not Yet</span>
                    </div>
                </div>

               <div id="calendar"></div>
          </div>

          <div class="chart-navigation-container" style="background-color: #E9EFEC;">
              <!-- Previous Button -->
              <button id="prevChartBtn" class="chart-nav-btn" onclick="showPreviousChart()" type="button">
                  <i class="fas fa-chevron-left"></i>
   
              </button>

              <!-- Chart Display -->
              <div id="chartContainer">    
                 
                  <canvas id="thresholdQtyChart" width="400" height="400"></canvas>
              </div>

              <!-- Next Button -->
              <button id="nextChartBtn" class="chart-nav-btn" onclick="showNextChart()" type="button">
                   <i class="fas fa-chevron-right"></i>
              </button>
          </div>
          <div class="chart-navigation-container" style="background-color: #E9EFEC;">
              <!-- Previous Button -->
              <button id="prevChartBtn2" class="chart-nav-btn" onclick="showPreviousChart2()" type="button">
                  <i class="fas fa-chevron-left"></i>

              </button>

              <!-- Chart Display -->
              <div id="chartContainer2" style="position: relative;">
                  <!-- Dropdown List positioned in the top right corner -->
                  <asp:DropDownList ID="ddlMonthFilter2" runat="server" AutoPostBack="false" class="dropdown-month">
                      <asp:ListItem Value="">Select Month</asp:ListItem>
                  </asp:DropDownList>
               
                 <!-- Canvas for Chart -->
                  <canvas id="stockItemChart" width="400" height="400" style="margin-top: 40px;"></canvas>
              </div>


              <!-- Next Button -->
              <button id="nextChartBtn2" class="chart-nav-btn" onclick="showNextChart2()" type="button">
                  <i class="fas fa-chevron-right"></i>
              </button>
            </div>
          <div class="box long-box" style="position: relative; background-color: #F2F6F5;" >
              <!-- Dropdown List -->
              <asp:DropDownList ID="ddlMonthFilter" runat="server" AutoPostBack="false" class="dropdown-month">
                  <asp:ListItem Value="">Select Month</asp:ListItem>
              </asp:DropDownList>
           
              <!-- Canvas for Chart -->
              <canvas id="inventoryChart" width="400" height="200" style="margin-top: 30px;"></canvas>
          </div>


      </div>
  </div>

<div class="modal fade" id="lowStockModal" tabindex="-1" role="dialog" aria-labelledby="lowStockModalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="lowStockModalLabel">Low Stock Items</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div id="lowStockItemList">
             
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>


    <script type="text/javascript">
        var chartDataStackedBar = []; 
        var chartDataLineChart = [];  
        var currentChartIndex = 0; 
        var currentItemIndex = 0;  
        var fusionChartStackedBar = null; 
        var fusionChartLineChart = null;  

        function drawFusionChart(data) {

            if (!data || data.length === 0) {
             
                renderStackedBarChartEmpty();
                return;
            }

            // if there is data, proceed with rendering the stacked bar chart
            chartDataStackedBar = data;
            renderStackedBarChart(currentChartIndex);  // render default chart
        }

        function renderStackedBarChart(index) {
            if (index >= 0 && index < chartDataStackedBar.length) {
                const canvas = document.getElementById("thresholdQtyChart");

                if (!canvas) {
                    console.error("Canvas element with id 'thresholdQtyChart' not found.");
                    return;
                }

                const ctx = canvas.getContext("2d");

                const data = chartDataStackedBar[index];
                const labels = data.categories[0].category.map(category => category.label);
                const itemCategory = data.chart.caption || 'Unknown Category';

                const colors = {
                    remainingQty: 'rgba(75, 192, 192, 0.6)',
                    threshold: 'rgba(255, 99, 132, 0.6)'
                };

                const datasets = data.dataset.map((series, i) => ({
                    label: series.seriesname,
                    data: series.data.map(d => Number(d.value)),
                    backgroundColor: i === 0 ? colors.remainingQty : colors.threshold,
                    borderColor: i === 0 ? 'rgba(75, 192, 192, 1)' : 'rgba(255, 99, 132, 1)',
                    borderWidth: 1
                }));

                if (window.fusionChartStackedBar && typeof window.fusionChartStackedBar.destroy === 'function') {
                    window.fusionChartStackedBar.destroy();
                }

                window.fusionChartStackedBar = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: datasets
                    },
                    options: {
                        scales: {
                            x: {
                                stacked: true,
                                title: {
                                    display: true,
                                    text: 'Items'
                                }
                            },
                            y: {
                                stacked: true,
                                title: {
                                    display: true,
                                    text: 'Quantity'
                                }
                            }
                        },
                        plugins: {
                            title: {
                                display: true,
                                text: `Remaining Item Quantity vs Threshold for ${itemCategory}`,
                                font: {
                                    size: 18
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        const datasetLabel = tooltipItem.dataset.label || '';
                                        const value = tooltipItem.raw || 0;
                                        return `${datasetLabel}: ${value}`;
                                    }
                                }
                            }
                        }
                    }
                });
            } else {
                console.error("Invalid index provided:", index);
                renderStackedBarChartEmpty();
            }
        }

        function renderStackedBarChartEmpty() {
            const canvas = document.getElementById("thresholdQtyChart");

            if (!canvas) {
                console.error("Canvas element with id 'thresholdQtyChart' not found.");
                return;
            }

            const ctx = canvas.getContext("2d");

            window.fusionChartStackedBar = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: ["No data available"],
                    datasets: [{
                        label: "No Data",
                        data: [0],
                        backgroundColor: 'rgba(200, 200, 200, 0.3)'
                    }]
                },
                options: {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Items'
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: 'Quantity'
                            },
                            beginAtZero: true
                        }
                    },
                    plugins: {
                        legend: { display: false },
                        tooltip: { enabled: false },
                        title: {
                            display: true,
                            text: "Remaining Item Quantity vs Threshold for Item Category`",
                            font: {
                                size: 16
                            }
                        }
                    }
                }
            });
        }

        function showPreviousChart(event) {
            if (event) event.preventDefault();

            if (currentChartIndex > 0) {
                currentChartIndex--;
                renderStackedBarChart(currentChartIndex);
            }
        }

        function showNextChart(event) {
            if (event) event.preventDefault();

            if (currentChartIndex < chartDataStackedBar.length - 1) {
                currentChartIndex++;
                renderStackedBarChart(currentChartIndex);
            }
        }

        function callMonthSelectedItems(monthYear) {

            const requestData = { monthYear: monthYear };

            fetch('OrgAnalytics.aspx/MonthSelectedItems', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            })
                .then(response => response.json())
                .then(data => {

                    if (data && data.d) {
                        console.log('Data received from server:', data);

                        const parsedData = typeof data.d === 'string' ? JSON.parse(data.d) : data.d;

                        renderItemStockLevel(parsedData);
                    } else {
                        console.error('No data received from server');
                    }
                })
                .catch(error => console.error('Error calling MonthSelectedItems:', error));
        }


        // line chart functions
        function renderItemStockLevel(data) {

            if (!data || data.length === 0 || !data[0].dates.length) {
                //console.log("No data available, showing empty chart.");
             
                renderLineChartEmpty();
                return;
            }

            chartDataLineChart = data;
            renderLineChart(0);  // default to first item
        }

        // function to render an empty chart
        function renderLineChartEmpty() {
            const ctx = document.getElementById('stockItemChart').getContext('2d');

            // destroy existing chart if it exists to avoid overlapping
            if (window.stockItemChart && typeof window.stockItemChart.destroy === 'function') {
                window.stockItemChart.destroy();
            }

            window.stockItemChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ['No Data'],  
                    datasets: [{
                        label: 'No Data',
                        data: [],  
                        borderColor: 'grey',
                        backgroundColor: 'rgba(211, 211, 211, 0.5)',
                        fill: false
                    }]
                },
                options: {
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Date'
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Cumulative Quantity'
                            }
                        }
                    },
                    plugins: {
                        title: {
                            display: true,  
                            text: "Stock Level Track of Item", 
                            font: {
                                size: 16  
                            }
                        }
                    }
                }
            });
        }



        function renderLineChart(index) {

            if (index >= 0 && index < chartDataLineChart.length) {

                const canvas = document.getElementById("stockItemChart");

                // check if canvas exists
                if (!canvas) {
                    console.error("Canvas element with id 'stockItemChart' not found.");
                    return;
                }

                const ctx = canvas.getContext("2d");

                const itemData = chartDataLineChart[index];
                const itemName = itemData.itemName || 'Unknown Item';
                const dates = itemData.dates || [];
                const cumulativeQty = itemData.cumulativeQty || [];
                const qtyInOut = itemData.qtyInOut || [];

                const labels = dates.map(d => d.label);
                const data = cumulativeQty.map(item => item.value);

                // destroy existing chart to avoid overlaps
                if (window.stockItemChart && typeof window.stockItemChart.destroy === 'function') {
                    window.stockItemChart.destroy();
                }

                // create a new line chart
                window.stockItemChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: labels,  // x-axis labels (dates)
                        datasets: [{
                            label: 'Cumulative Stock',
                            data: data,  // y-axis data (cumulative quantities)
                            borderColor: 'rgba(75, 192, 192, 1)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            fill: true,  
                            tension: 0.1,  
                        }]
                    },
                    options: {
                        scales: {
                            x: {
                                title: {
                                    display: true,
                                    text: 'Date'
                                }
                            },
                            y: {
                                title: {
                                    display: true,
                                    text: 'Cumulative Quantity'
                                }
                            }
                        },
                        plugins: {
                            title: {
                                display: true,  
                                text: `Stock Level Track of ${itemName}`, 
                                font: {
                                    size: 18  
                                }
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (tooltipItem) {
                                        const i = tooltipItem.dataIndex;
                                        const date = dates[i]?.label || 'Unknown';
                                        const qtyIn = qtyInOut[i]?.qtyIn || 0;
                                        const qtyOut = qtyInOut[i]?.qtyOut || 0;
                                        const cumulative = tooltipItem.raw;

                                        return [
                                            `Date: ${date}`,
                                            `Cumulative Quantity: ${cumulative}`,
                                            `Total In: ${qtyIn}`,
                                            `Total Out: ${qtyOut}`
                                        ];
                                    }
                                }
                            }
                        }
                    }
                });
            } else {
                console.error("Invalid index provided:", index);
            }
        }



        function showPreviousChart2(event) {
            if (event) event.preventDefault();

            if (currentItemIndex > 0) {
                currentItemIndex--;
                renderLineChart(currentItemIndex);
            }
        }

        function showNextChart2(event) {
            if (event) event.preventDefault();

            if (currentItemIndex < chartDataLineChart.length - 1) {
                currentItemIndex++;
                renderLineChart(currentItemIndex);
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var today = new Date();

            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                events: function (fetchInfo, successCallback, failureCallback) {
                    $.ajax({
                        url: 'OrgAnalytics.aspx/GetExpiryDate',
                        method: 'POST',
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (response) {
                            var events = response.d.map(function (item) {
                                var eventDate = new Date(item.expiryDate);

                                var backgroundColor = '';
                                var borderColor = '';

                                if (eventDate < today) {
                                    // expired
                                    backgroundColor = '#FF6961';
                                    borderColor = '#b30000';
                                } else if (eventDate >= today && eventDate <= new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000)) {
                                    // expiring soon (within 7 days)
                                    backgroundColor = '#FFCA4B';
                                    borderColor = '#807000';
                                } else {
                                    // not yet expiring
                                    backgroundColor = '#44D044';
                                    borderColor = '#1E771E';
                                }

                                return {
                                    title: item.item,
                                    start: item.expiryDate,
                                    backgroundColor: backgroundColor,
                                    borderColor: borderColor
                                };
                            });

                            successCallback(events);
                        },
                        error: function () {
                            failureCallback();
                        }
                    });
                }
            });

            calendar.render();
        });


        document.addEventListener('DOMContentLoaded', function () {
            const monthDropdown = document.getElementById('<%= ddlMonthFilter.ClientID %>');

            if (monthDropdown) {
                monthDropdown.addEventListener('change', function () {
                    const selectedMonth = monthDropdown.value;
                   
                    if (selectedMonth) {

                        callMonthSelected(selectedMonth);
                    }
                });
            }

            const monthDropdown2 = document.getElementById('<%= ddlMonthFilter2.ClientID %>');

            if (monthDropdown2) {
                monthDropdown2.addEventListener('change', function () {
                    const selectedMonth2 = monthDropdown2.value;

                    if (selectedMonth2) {
   
                        callMonthSelectedItems(selectedMonth2);
                    }
                });
            }
        });

        function callMonthSelected(monthYear) {

            const requestData = { monthYear: monthYear };

            fetch('OrgAnalytics.aspx/MonthSelected', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            })
                .then(response => response.json())  
                .then(data => {
                    if (data && data.d) {
                        console.log('Data received from server:', data);
                        const parsedData = JSON.parse(data.d);  

                        renderInventoryChart(parsedData);  
                    } else {
                        console.error('No data received from server');
                    }
                })
                .catch(error => console.error('Error calling MonthSelected:', error));
        }

        

        // hold the chart instance
        let chartInstance = null;

        function renderInventoryChart(parsedData) {
            //if (!parsedData || parsedData.length === 0) {
            //    // If no data, render an empty chart
            //    renderEmptyInventoryChart();
            //    return;
            //}

            const labels = parsedData.map(item => item.created_on); // x-axis: dates
            const quantityIn = parsedData.map(item => Number(item.quantityIn)); // y-axis: quantityIn
            const quantityOut = parsedData.map(item => Number(item.quantityOut)); // y-axis: quantityOut

            const ctx = document.getElementById('inventoryChart').getContext('2d');

            if (chartInstance) {
                chartInstance.destroy();
            }

            chartInstance = new Chart(ctx, {
                type: 'line', 
                data: {
                    labels: labels,  // X-axis: dates
                    datasets: [
                        {
                            label: 'Quantity In',
                            data: quantityIn,  // Quantity In data
                            borderColor: 'blue',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            borderWidth: 2,
                            fill: true
                        },
                        {
                            label: 'Quantity Out',
                            data: quantityOut,  // Quantity Out data
                            borderColor: 'red',
                            backgroundColor: 'rgba(255, 99, 132, 0.2)',
                            borderWidth: 2,
                            fill: true
                        }
                    ]
                },
                options: {
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Date'
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Quantity'
                            }
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: 'Item Inflows vs Outflows Over Time'
                        }
                    }
                }
            });
        }

        function renderEmptyInventoryChart() {
            const ctx = document.getElementById('inventoryChart').getContext('2d');

            if (window.chartInstance && typeof window.chartInstance.destroy === 'function') {
                window.chartInstance.destroy();
            }

            window.chartInstance = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ['No Data'],
                    datasets: [{
                        label: 'No Data',
                        data: [], 
                        borderColor: 'grey',
                        backgroundColor: 'rgba(211, 211, 211, 0.5)',
                        fill: false
                    }]
                },
                options: {
                    scales: {
                        x: {
                            title: {
                                display: true,
                                text: 'Date'
                            }
                        },
                        y: {
                            title: {
                                display: true,
                                text: 'Quantity'
                            }
                        }
                    },
                    plugins: {
                        title: {
                            display: true,  
                            text: "Item Inflows vs Outflows Over Time",  
                            font: {
                                size: 16  
                            }
                        }
                    }
                }
            });
        }



        function showLowStockModal() {
            $('#lowStockModal').modal('show'); 

            $.ajax({
                type: "POST",
                url: "OrgAnalytics.aspx/GetLowStockItem", 
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var items = response.d; 
                    var itemListDiv = document.getElementById("lowStockItemList");
                    itemListDiv.innerHTML = ""; 
                    console.log(items);
                    items.forEach(function (item) {
                        var listItem = document.createElement("p");
                        listItem.textContent = item.item + ": " + item.quantity + " left (Threshold: " + item.threshold + ")";
                        itemListDiv.appendChild(listItem);
                    });
                },
                error: function (xhr, status, error) {
                    console.error("Failed to fetch low stock items:", error);
                }
            });
        }

    </script>


</asp:Content>
