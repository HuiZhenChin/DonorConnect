<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="DonorConnect.Home" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/Home.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Itim&family=Istok+Web:wght@400;700&family=Rethink+Sans&display=swap" rel="stylesheet">

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main aria-labelledby="title">
        <div class="main-background">
            <div class="text2">CONNECT WITH IMPACT AND LOVE</div>
            <div class="text1">Inspiring Changes</div>
             <asp:button class="button-view-donations" OnClick="btnViewDonation_Click" runat="server" Text="View Donations"></asp:button>
        </div>
        <div style="width: 100%; background: white; padding: 20px 0;"></div>
        <div>
           
        </div>

        <div class="achievement-section">
            <div class="container">
                <div class="achievement-title">OUR ACHIEVEMENTS</div>
                <div class="achievement-counters">
                    <div class="achievement-counter">
                        <asp:Label ID="totalDonationsLabel" class="count" runat="server" Text="0"></asp:Label>
                        <div class="count-label">Total Donations Made</div>
                    </div>
                    <div class="achievement-counter">
                        <asp:Label ID="totalDeliveriesLabel" class="count" runat="server" Text="0"></asp:Label>
                        <div class="count-label">Deliveries Completed</div>
                    </div>
                    <div class="achievement-counter">
                        <asp:Label ID="registeredUsersLabel" class="count" runat="server" Text="0"></asp:Label>
                        <div class="count-label">Registered Users</div>
                    </div>
                </div>
            </div>
        </div>


        <div class="section-about">
            <div class="container">
                <div class="title">ABOUT US</div>
                <div class="content">
                    <div>
                        <img src="/Image/items_home.jpg" />
                        <div class="description">Gathers available item donations in Malaysia</div>
                    </div>
                    <div>
                        <img src="/Image/phone_home.jpg" />
                        <div class="description">Search and filter preferred item donations based on location and donated item compatibility</div>
                    </div>
                    <div>
                        <img src="/Image/delivery_home.jpg" />
                        <div class="description">Provides delivery tracking from point of donations to destinations</div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <script>
    function animateCounter(element, start, end, duration) {
        let startTime = null;

        function step(timestamp) {
            if (!startTime) startTime = timestamp;
            const progress = Math.min((timestamp - startTime) / duration, 1);
            element.innerText = Math.floor(progress * (end - start) + start);
            if (progress < 1) {
                window.requestAnimationFrame(step);
            }
        }
        window.requestAnimationFrame(step);
    }

    document.addEventListener("DOMContentLoaded", function() {
        animateCounter(document.getElementById('<%= totalDonationsLabel.ClientID %>'), 0, parseInt(document.getElementById('<%= totalDonationsLabel.ClientID %>').innerText), 2000);
        animateCounter(document.getElementById('<%= totalDeliveriesLabel.ClientID %>'), 0, parseInt(document.getElementById('<%= totalDeliveriesLabel.ClientID %>').innerText), 2000);
        animateCounter(document.getElementById('<%= registeredUsersLabel.ClientID %>'), 0, parseInt(document.getElementById('<%= registeredUsersLabel.ClientID %>').innerText), 2000);
    });
</script>

</asp:Content>


 