<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgReport.aspx.cs" Inherits="DonorConnect.OrgReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Report</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script> 
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

     <style>

      body {
         background: rgb(249,247,247);
         background: linear-gradient(180deg, rgba(249,247,247,1) 0%, rgba(219,226,239,1) 40%, rgba(233,239,236,1) 68%, rgba(106,156,137,1) 100%);
     }
 
     .page-header {
    
         padding-bottom: 10px;
         border-bottom: 2px solid #ddd;
         margin-bottom: 20px;
     }

     .title{
        font-size: 1.8em;
        font-weight: bold;
        padding-bottom: 10px;
        color: #333;
        margin-bottom: 20px;
    }

     .report-section {
        background-color: #f9f9f9;
        border: 1px solid #ddd;
        padding: 20px;
        border-radius: 8px;
        margin-bottom: 20px;
    }

    .section-title {
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 10px;
        color: #333;
    }

    .form-group {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .form-label {
        font-weight: bold;
        margin-right: 10px;
        min-width: 120px;
    }

    .form-control {
        width: 100%;
        max-width: 300px;
        padding: 8px;
        border: 1px solid #ccc;
        border-radius: 4px;
    }

    .btn {
        padding: 8px 15px;
        font-size: 14px;
        font-weight: bold;
        border-radius: 4px;
        cursor: pointer;
        text-transform: uppercase;
        transition: background-color 0.3s ease;
    }

    .btn-primary {
        background-color: #007bff;
        color: #fff;
        border: none;
    }

    .btn-primary:hover {
        background-color: #0056b3;
    }

    .pdf-preview {
        margin-top: 20px;
        background-color: #f9f9f9;
        border: 1px solid #ddd;
        padding: 15px;
        border-radius: 8px;
    }

    .pdf-viewer {
        border-radius: 4px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }

   </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1 class="title">Monthly Report</h1>
    </div>
   
  <div class="report-section">
    <h3 class="section-title">Existing Reports</h3>
    <div class="form-group">
        <asp:Label ID="lblExistingReports" runat="server" Text="Select Report:" CssClass="form-label"></asp:Label>
        <asp:DropDownList ID="ddlExistingReports" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExistingReports_SelectedIndexChanged"></asp:DropDownList>
        <asp:Button ID="btnDownloadReport" runat="server" Text="Download Report" CssClass="btn btn-primary" OnClick="btnDownloadReport_Click" />
    </div>
</div>

    <!-- PDF Preview Section -->
    <div class="pdf-preview">
        <h3 class="section-title">Report Preview</h3>
       <iframe id="pdfViewer" runat="server" class="pdf-viewer" src="NoFileChosen.html" width="100%" height="600px" style="border: 1px solid #ccc;"></iframe>
    </div>



    <script>
        function showInfo(message) {
            Swal.fire({
                text: message,
                icon: 'info',
                confirmButtonText: 'OK',
            });
        }
    </script>
</asp:Content>

        

