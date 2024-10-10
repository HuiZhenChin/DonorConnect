<%@ Page Title="Delivery Tracking" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Delivery.aspx.cs" Inherits="DonorConnect.Delivery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Delivery Tracking</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    
    <style>

  
    :root {
        --default-color: black;
        --grey-color: black;
        --main-color: pink;
    }

    #progress_bar {
        display: table;
        width: 100%;
        padding: 15px 15px 0;
        table-layout: fixed;
        counter-reset: step;
        margin-top: 20px; 
        margin-bottom: 20px;
    }

    #progress_bar li {
        list-style-type: none;
        display: table-cell;
        width: 20%;
        font-size: 16px;
        position: relative;
        text-align: center;
    }

        #progress_bar li:before {
            width: 50px;
            height: 50px;
            color: lightgrey;
            content: counter(step);
            counter-increment: step;
            line-height: 50px;
            font-size: 18px;
            border: 1px solid var(--grey-color);
            display: block;
            text-align: center;
            margin: 0 auto 10px auto;
            border-radius: 50%;
            background-color: #fff;
        }

        #progress_bar li:after {
            width: 100%;
            height: 10px;
            content: '';
            position: absolute;
            background-color: #fff;
            top: 25px;
            left: -50%;
            z-index: -1;
        }

        #progress_bar li:first-child:after {
            content: none;
        }

        #progress_bar li.step-done {
            color: var(--main-color);
        }

            #progress_bar li.step-done:before {
                border-color: var(--main-color);
                background-color: var(--main-color);
                color: #fff;
                content: "\f00c"; 
                font-family: "FontAwesome";
            }

            #progress_bar li.step-done + li:after {
                background-color: var(--main-color);
            }

        #progress_bar li.step-active {
            color: var(--main-color);
        }

            #progress_bar li.step-active:before {
                border-color: var(--main-color);
                color: var(--main-color);
                font-weight: 700;
            }


    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="row">
        <div class="col-12 text-center">
            <h3>
                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></h3>
            <p class="tracking-status">
                <asp:Label ID="lblDate" runat="server" Text=""></asp:Label></p>
            <button class="btn btn-outline-primary">Track Order Details</button>
        </div>
    </div>

    <ol id="progress_bar">
        <li id="step1" runat="server" class="step-todo">To Accept<br>
            <span id="date1" runat="server"></span></li>
        <li id="step2" runat="server" class="step-todo">To Pick Up<br>
            <span id="date2" runat="server"></span></li>
        <li id="step3" runat="server" class="step-todo">To Reach<br>
            <span id="date3" runat="server"></span></li>
        <li id="step4" runat="server" class="step-todo">Completed<br>
            <span id="date4" runat="server"></span></li>
    </ol>



</asp:Content>

