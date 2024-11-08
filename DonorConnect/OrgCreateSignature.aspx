<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Org.Master" AutoEventWireup="true" CodeBehind="OrgCreateSignature.aspx.cs" Inherits="DonorConnect.OrgCreateSignature" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Signature</title>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

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

        .title{
            font-size: 1.8em;
            font-weight: bold;
            padding-bottom: 10px;
            color: #333;
            margin-bottom: 20px;
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  
    <div class="page-header" >
        <h1 class="title" style="font-weight: bold;">Acknowledgment Receipt Settings</h1>
       
        <div style="float: right; top: -55px; position: relative;">
            <i class="fa fa-eye" style="cursor: pointer;" onclick="showSampleModal();"></i><asp:Label Text=" View Sample Receipt" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Two Columns Section -->
    <div class="row">
        <!-- Signature Section -->
        <div class="col-md-6">
            <h4 style="font-weight: bold;">Signature</h4>
            <canvas id="signatureBoard" width="530" height="200" style="border: 1px solid #000;" placeholder="Try draw something..."></canvas>
            <br />
            <span id="signatureError" class="text-danger" style="display: none;"></span>
            <br>
            <div style="float: right;">                
                <button class="btn btn-secondary mt-2" onclick="clearCanvas()">Clear Signature</button>
                <button class="btn btn-primary mt-2" onclick="saveSignature(event);">Save Signature</button>
            </div>
            <img id="signatureImage" style="display: none; margin-top: 10px;" />
            <img id="signatureDB" runat="server" style="display: none;" />
            
        </div>

        <!-- Person in Charge Section -->
        <div class="col-md-6">
            <h4 style="font-weight: bold;">Person in Charge</h4>
            <asp:Label ID="lblPersonInCharge" runat="server" Text="Enter the name displayed in receipt: " />
            <asp:TextBox ID="txtPersonInCharge" CssClass="form-control mt-2" runat="server" placeholder="Enter name"></asp:TextBox>
            <span id="picError" class="text-danger" style="display: none;"></span>
            <br>
            <div style="float: right;">              
               <asp:Button class="btn btn-secondary mt-2" runat="server" Text="Discard"></asp:Button>
               <asp:Button class="btn btn-primary mt-2" OnClick="btnSavePIC_Click" OnClientClick="return validateInput();" runat="server" Text="Save"></asp:Button>
            </div>
           
        </div>
    </div>


    <!-- Modal for Sample Receipt -->
    <div class="modal fade" id="sampleReceiptModal" tabindex="-1" aria-labelledby="sampleReceiptModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="sampleReceiptModalLabel">Sample Receipt</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body text-center">
                
                    <img src="/Image/receiptSample.png" alt="Sample Receipt" class="img-fluid">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        var canvas = document.getElementById('signatureBoard');
        var context = canvas.getContext('2d');
        var drawing = false;

        // canvas is cleared before starting to draw
        function clearCanvas() {
            context.clearRect(0, 0, canvas.width, canvas.height);
            document.getElementById('signatureImage').style.display = 'none'; 
        }

        canvas.addEventListener('mousedown', function (e) {
            drawing = true;
            context.beginPath();
            context.moveTo(e.offsetX, e.offsetY);
        });

        canvas.addEventListener('mousemove', function (e) {
            if (drawing) {
                context.lineTo(e.offsetX, e.offsetY);
                context.stroke();
            }
        });

        canvas.addEventListener('mouseup', function () {
            drawing = false;
        });

        // save the signature with a transparent background
        function saveSignature(event) {
            var signatureBoard = document.getElementById('signatureBoard');
            var signatureError = document.getElementById('signatureError');

            // if empty canvas
            if (isCanvasEmpty(signatureBoard)) {
                signatureError.style.display = 'block';
                signatureError.innerHTML = 'Please draw a signature before saving.';

                if (event) {
                    event.preventDefault(); 
                }

                return;
            } else {
                signatureError.style.display = 'none'; 
                signatureError.innerHTML = ''; 
            }

            // convert the canvas to a data URL for saving
            var dataURL = signatureBoard.toDataURL('image/png'); 
            document.getElementById('signatureImage').src = dataURL; 
            document.getElementById('signatureImage').style.display = 'block';

            // save the base64 image to the server
            saveSignatureToServer(dataURL);
        }

        // save the signature image to the server 
        function saveSignatureToServer(dataURL) {
            $.ajax({
                type: 'POST',
                url: '/OrgCreateSignature.aspx/SaveSignature',
                data: JSON.stringify({ imageBase64: dataURL }),
                contentType: 'application/json',
                success: function (response) {
                    location.reload();
                    showSuccess('Signature saved successfully!');
                },
                error: function () {
                    showError('Error saving signature');
                }
            });
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

        function showSampleModal() {

            $('#sampleReceiptModal').modal('show');
            return false;
        }

        function validateInput() {          
            var picName = document.getElementById('<%= txtPersonInCharge.ClientID %>').value.trim();           
            var picError = document.getElementById('picError');
           
            var valid = true;

            if (picName === '') {
                picError.style.display = 'block';                
                picError.innerHTML = 'Please enter a name before saving.';

                valid = false;


            } else {
                picError.style.display = 'none';
                picError.innerHTML = "";
            }

            return valid; 
        }

        // if the signature canvas is empty
        function isCanvasEmpty(canvas) {
            const context = canvas.getContext('2d');
            const pixelData = context.getImageData(0, 0, canvas.width, canvas.height).data;

            return !pixelData.some((value, index) => index % 4 === 3 && value !== 0);
        }
    </script>


</asp:Content>
