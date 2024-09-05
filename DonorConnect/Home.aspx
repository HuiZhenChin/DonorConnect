<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="DonorConnect.Home" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/Home.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Itim&family=Istok+Web:wght@400;700&display=swap" rel="stylesheet">
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
</asp:Content>


 