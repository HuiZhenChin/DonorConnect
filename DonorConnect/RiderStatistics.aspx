<%@ Page Title="Statistics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RiderStatistics.aspx.cs" Inherits="DonorConnect.RiderStatistics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Statistics</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        body{
            background: rgb(239,249,255);
            background: linear-gradient(180deg, rgba(239,249,255,1) 16%, rgba(155,190,200,1) 40%, rgba(248,237,227,1) 67%, rgba(175,143,111,1) 100%);
        }

        .wallet-card {
            background-color: #4A90E2;
            color: white;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        .wallet-amount {
            font-weight: bold;
        }

        .box-container {
            background-color: #f8f9fa;
            border: 1px solid #4A628A;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4" style="margin-top: 50px;">
    <div class="col-md-6 d-flex align-items-center justify-content-center mx-auto">
        <div class="box-container" style="width: 100%; margin-top: 100px; padding: 20px;">
            <div class="text-center">
                <h4>Statistics</h4>
                <p>Details about the number of delivery made.</p>
                <canvas id="deliveryChart" style="max-width: 100%; height: auto;"></canvas>
            </div>
        </div>
    </div>
</div>


    <script>


        function barChart(labels, deliveryCounts, totalEarnings) {
            const ctx = document.getElementById('deliveryChart').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Deliveries Made',
                            data: deliveryCounts,
                            backgroundColor: 'rgba(75, 192, 192, 0.6)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    responsive: true,
                    plugins: {
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    const deliveryCount = deliveryCounts[tooltipItem.dataIndex];
                                    const earnings = totalEarnings[tooltipItem.dataIndex].toFixed(2);
                                    return `Deliveries Made: ${deliveryCount}, Total Earnings: RM ${earnings}`;
                                }
                            }
                        },
                        datalabels: {
                            display: true,
                            formatter: (value) => `${value} Deliveries`,
                            color: 'black',
                            anchor: 'end',
                            align: 'top'
                        }
                    },
                    scales: {
                        y: { beginAtZero: true },
                        x: { title: { display: true, text: 'Month' } }
                    }
                }
            });
        }
      </script>
</asp:Content>
