<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UpdateDelivery.aspx.cs" Inherits="DonorConnect.UpdateDelivery1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Donations</title>
    <link href="/Content/PreviewPublicInfo.css" rel="stylesheet" type="text/css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/jsqr/dist/jsQR.js"></script>
    
    <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBHd69lOb31ywFRMu99sos-ysgl-uCtidY&callback=initMap&libraries=places"></script>
    
    
    <style>

      body{
        background: rgb(231,246,242);
        background: linear-gradient(180deg, rgba(231,246,242,1) 18%, rgba(165,201,202,1) 60%, rgba(57,91,100,1) 86%, rgba(44,51,51,1) 100%);
      }

    :root {
        --default-color: black;
        --grey-color: black;
        --main-color: #1B262C;
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
        
        .map-link {
            color: #304463;
            text-decoration: none;
            font-weight: 700;
            float: right;
            padding: 10px 20px;
            border: 2px solid #304463; 
            border-radius: 8px;
            background-color: #EBF4F6; 
            display: inline-block;
            margin-left: 20px;
        }

        .map-link:hover {
            background-color: #304463; 
            color: #ffffff; 
        }

        .map-link2 {
            color: #003C43;
            text-decoration: none;
            font-weight: 700;
            float: right;
            padding: 10px 20px;
            border: 2px solid #304463; 
            border-radius: 8px;
            background-color: #E3FEF7; 
            display: inline-block;
            margin-left: 20px;
        }

        .map-link2:hover {
            background-color: #77B0AA; 
            color: #ffffff; 
        }

        @media (max-width: 480px) {
        .map-link2  {
            margin-left: 0px;
        }

        .map-link {
            width: 40%;
        }

       
    }



    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div class="row">
        <div class="col-12 text-center">
            <h3 style="padding-top: 20px; font-weight: bold; font-size: 34px;">
                <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label></h3>
           <div id="dateInfo" runat="server">
            <span style="font-size: 18px;">📅</span><asp:Label ID="pickupDateLbl" runat="server" Text=" " style="font-size: 20px; font-weight: 500; padding-left: 10px;"></asp:Label><br />           
            <span style="font-size: 18px;">🕒</span><asp:Label ID="scheduledTimeLbl" runat="server" Text=" " style="font-size: 20px; font-weight: 500; padding-left: 10px;"></asp:Label><br />

            <span style="font-size: 20px; padding-right: 10px; font-style: italic;">Delivered by: </span><asp:HyperLink ID="lblRider" runat="server" Text="" style="font-size: 20px; font-style: italic; font-weight: 500; color: #1A3636;"></asp:HyperLink><br />
            <span style="font-size: 20px; padding-right: 10px; font-style: italic;">Note to Rider: </span><asp:Label ID="lblNote" runat="server" Text="" style="font-size: 20px; font-style: italic; font-weight: 500; color: #1A3636;"></asp:Label><br />
            <asp:Button class="btn btn-outline-primary" Text="UPDATE DELIVERY" runat="server" OnClick="btnUpdate_Click" style="margin-top: 20px; background-color: #1A3636; color: white; border: solid 1px #1A3636;"></asp:Button>
        </div>
            <%--<asp:Button Id="btnTrack" class="btn btn-outline-primary" Text="Track Order Details" runat="server" style="display: none; align-items: center;"></asp:Button>--%>
        </div>
    </div>

    <div class="row" style="padding: 20px;">
    <!-- Donor Details Column -->
    <div class="col-md-6" runat="server" id="donor">
        <div class="card">
            <div class="card-body" style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #FEF5ED; border-radius: 8px;">
                <h5 class="card-title" style="font-weight: bold;">Donor</h5>
                <p class="card-text">
                    <span style="font-size: 18px;">🏡</span> <asp:Label ID="lblDonorName" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📞</span> <asp:Label ID="lblDonorContact" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📍</span> <asp:Label ID="lblDonorAddress" runat="server" Text="" style="font-size: 16px;"></asp:Label>
                </p>
            </div>
        </div>
    </div>

    <!-- Organization Details Column -->
    <div class="col-md-6" runat="server" id="org">
        <div class="card">
            <div class="card-body" style="box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #EEF0E5; border-radius: 8px;">
                <h5 class="card-title" style="font-weight: bold;">Organization</h5>
                <p class="card-text">
                    <span style="font-size: 18px;">🏘️</span> <asp:Label ID="lblOrgName" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📞</span> <asp:Label ID="lblOrgContact" runat="server" Text="" style="font-size: 16px;"></asp:Label><br />
                    <span style="font-size: 18px;">📍</span> <asp:Label ID="lblOrgAddress" runat="server" Text="" style="font-size: 16px;"></asp:Label>
                </p>
            </div>
        </div>
    </div>
</div>


    <!-- Bootstrap Modal -->
    <div class="modal fade" id="cameraModal" tabindex="-1" aria-labelledby="cameraModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="cameraModalLabel">QR Code Scanner</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mt-3">
                        <video id="video" autoplay style="width: 100%; max-width: 100%;"></video>
                        <canvas id="canvas" style="display: none;"></canvas>
                        <p id="outputMessage">Scanning for QR code...</p>
                        <p id="outputData"></p>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


    <ol id="progress_bar">
        <!-- Step 1: To Accept -->
        <li id="step1" runat="server" class="step-done">To Accept<br>
            <span id="date1" runat="server"></span>
        </li>

        <!-- Step 2: To Pick Up -->
        <li id="step2" runat="server" class="step-todo">To Pick Up<br>
            <span id="date2" runat="server"></span>
            <br>
            <i id="showPickupImg" class="fas fa-file-image" style="font-size: 24px; display: none; color: black; cursor: pointer; padding-top: 10px;" runat="server" onclick="showPickupImage();"></i>
            <!-- trigger modal for Pick Up image upload -->
            <asp:Button ID="pickupBtn" runat="server" CssClass="btn btn-primary" Style="display: none; background-color: #387478; margin-top: 10px; border: solid 1px #243642;" Text="Upload Item Picture" OnClientClick="return showPickUpModal();" />


        </li>

        <!-- Step 3: To Reach -->
        <li id="step3" runat="server" class="step-todo">To Reach<br>
            <span id="date3" runat="server"></span>
            <br>
            <!-- trigger modal for Reach image upload -->
            <asp:Button ID="reachBtn" runat="server" CssClass="btn btn-primary" Style="display: none; background-color: #387478; margin-top: 10px; border: solid 1px #243642;" Text="Upload Destination Picture" OnClientClick="return showReachModal();" />
            <i id="showReachImg" class="fas fa-file-image" style="font-size: 24px; color: black; cursor: pointer; display: none; padding-top: 10px;" runat="server" onclick="showReachImage();"></i>
        </li>

        <!-- Step 4: Completed -->
        <li id="step4" runat="server" class="step-todo">Completed<br>
            <span id="date4" runat="server"></span>
            <br>
            <a id="receiptAttch" runat="server" style="display: none;" target="_blank">
                <i class="fa fa-file-pdf" style="font-size: 24px; color: black; cursor: pointer; padding-top: 10px;"></i>
            </a>
        </li>
    </ol>
        
        
        <div id="googleMapsLinkContainer" style="padding-top: 20px;"></div>
    <div id="liveLinkContainer" style="position: absolute;" Visible="false">
        <a href="#" id="openModal" class="map-link2" onclick="openModal()"><i class="fas fa-map-marker-alt" style="padding-right: 5px;"></i>Share Live Location</a>
    </div>
         <div id="map" style="height: 300px; width: 100%;"></div>


    <div id="liveLocationModal" class="modal" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0, 0, 0, 0.5);">
    <div class="modal-content" style="background-color: #fff; padding: 20px; max-width: 500px; margin: 100px auto; border-radius: 5px;">
        <h2>Share Live Location</h2>
        <label for="liveLocationLink">Enter the live location link:</label>
        <asp:TextBox ID="liveLocationLink" runat="server" CssClass="form-control" placeholder="paste your live share link here" Width="100%"></asp:TextBox>

        <!-- Save and Cancel buttons -->
        <div style="display: flex; justify-content: space-between; margin-top: 20px;">
            <button onclick="closeModal()" style="background-color: #f44336; color: white; padding: 10px; border: none; cursor: pointer;">Cancel</button>
            <button onclick="saveLiveLink()" style="background-color: #4CAF50; color: white; padding: 10px; border: none; cursor: pointer;">Save Link</button>
        </div>
    </div>
</div>


    <!-- Modal for Pick Up image upload -->
    <div class="modal fade" id="pickUpModal" tabindex="-1" role="dialog" aria-labelledby="pickUpModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="pickUpModalLabel">Upload Item Picture (Pick Up)</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- File Upload for Pick Up -->
                    <asp:FileUpload ID="fuPickUp" runat="server" AllowMultiple="true" Style="height: 45px; margin-top: 10px;" OnChange="validateAttachmentUpload(this)" />
                    <span id="attchUploadError" class="text-danger" style="display: none;"></span>
                    <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                        <em>Accepted Formats: .jpg, .jpeg, .png, Maximum 5 attachments.</em>
                    </small>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="UploadButtonPickUp" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="btnUpdateImg_Click" />
                    <asp:Label ID="StatusLabelPickUp" runat="server" Text="" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal for Reach image upload -->
    <div class="modal fade" id="reachModal" tabindex="-1" role="dialog" aria-labelledby="reachModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="reachModalLabel">Upload Destination Picture (To Reach)</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- File Upload for Reaching Destination -->
                    <asp:FileUpload ID="fuReach" runat="server" AllowMultiple="true" Style="height: 45px; margin-top: 10px;" OnChange="validateAttachmentUpload2(this)" />
                    <span id="attchUploadError2" class="text-danger" style="display: none;"></span>
                    <small class="form-text text-muted" style="color: #282a2c!important; font-weight: 500">
                        <em>Accepted Formats: .jpg, .jpeg, .png, Maximum 5 attachments.</em>
                    </small>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="UploadButtonReach" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="btnUpdate2_Click" />
                    <asp:Label ID="StatusLabelReach" runat="server" Text="" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal for displaying images -->
    <div class="modal fade" id="pickupImg" tabindex="-1" role="dialog" aria-labelledby="imagesModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="imagesModalLabel">Uploaded Files</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div id="pickupImageContent"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal for displaying images -->
    <div class="modal fade" id="reachImg" tabindex="-1" role="dialog" aria-labelledby="imagesModalLabel2" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="imagesModalLabe2">Uploaded Files</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div id="reachImageContent"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>




    <script>
        window.onload = function () {
            initMap();
        };

        // reference the video, canvas, and output elements
        const video = document.getElementById('video');
        const canvas = document.getElementById('canvas');
        const outputMessage = document.getElementById('outputMessage');
        const outputData = document.getElementById('outputData');
        const context = canvas.getContext('2d');
        let videoStream = null;

        function showPickUpModal() {
           
            $('#pickUpModal').modal('show');
            return false; 
        }

        function showReachModal() {

            $('#reachModal').modal('show');
            return false;
        }

        function showError(message) {
            Swal.fire({
                text: message,
                icon: 'error',
                confirmButtonText: 'OK',

            });
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
                title:'Image Required',
                text: message,
                icon: 'info',
                confirmButtonText: 'OK',

            });
        }

        // start video feed from the camera
        function startVideo() {
            // access the user's camera
            navigator.mediaDevices.getUserMedia({ video: { facingMode: "environment" } })
                .then(stream => {
                    // show the modal once the camera is successfully accessed
                    var modal = new bootstrap.Modal(document.getElementById('cameraModal'));
                    modal.show();

                    // set the video stream
                    video.srcObject = stream;
                    video.setAttribute("playsinline", true); // to prevent fullscreen in iOS
                    video.play();
                    videoStream = stream;

                    // start scanning for QR codes
                    requestAnimationFrame(scanQRCode);
                })
                .catch(err => {
                    console.error("Error accessing the camera: ", err);
                    outputMessage.innerText = "Unable to access camera.";
                });
        }


        // scan for QR code
        function scanQRCode() {
            
            if (video.readyState === video.HAVE_ENOUGH_DATA) {
                
                canvas.width = video.videoWidth;
                canvas.height = video.videoHeight;
                context.drawImage(video, 0, 0, canvas.width, canvas.height);

                // capture the image from the video feed
                const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
                
                // read QR code from the captured image
                const code = jsQR(imageData.data, imageData.width, imageData.height, { inversionAttempts: "dontInvert" });

                if (code) {
                    console.log("QR Code detected:", code.data); 
                    outputMessage.hidden = true;
                    outputData.innerText = "QR Code Data: " + code.data;

                    // if is a URL
                    if (isValidUrl(code.data)) {
                        console.log("Valid URL detected.");
                        const url = new URL(code.data);                      

                        decryptToken(url);
                        
                    } else {
                        console.error("The detected QR code is not a valid URL.");
                    }
                } else {
                    
                    outputMessage.hidden = false;
                    outputMessage.innerText = "Scanning for QR code...";
                    outputData.innerText = "";
                }
            } else {
                console.log("Video feed is not ready yet.");
            }
            requestAnimationFrame(scanQRCode);
        }

        // check if a string is base64 encoded
        function isBase64(str) {
            try {
                return btoa(atob(str)) === str;
            } catch (err) {
                return false;
            }
        }


      // send the encrypted token to the backend for decryption
        function decryptToken(url) {
            fetch('UpdateDelivery.aspx/DecryptUrl', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ encryptedUrl: url }) 
            })
                .then(response => response.json())
                .then(data => {
                    console.log("Response from backend:", data);

                    if (data.d.isDecrypted) { 
                        console.log("Decryption successful, checking delivery ID...");

                        const currentUrlParams = new URLSearchParams(window.location.search);
                        const currentDeliveryId = currentUrlParams.get("deliveryId");

                        const decryptedUrlParams = new URLSearchParams(data.d.deliveryId.search);
                        const decryptedDeliveryId = data.d.deliveryId;

                        console.log(currentDeliveryId);
                        console.log(decryptedDeliveryId);

                        if (currentDeliveryId === decryptedDeliveryId) {
                            console.log("Delivery ID matches, redirecting to decrypted URL...");
                            outputMessage.innerText = "Valid QR Code";
                            stopVideoStream();

                            window.location.href = data.d.decryptedUrl;
                        } else {
                            console.log("Delivery ID does not match.");

                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: 'Delivery ID does not match the QR code. Please try again.'
                            });
                        }
                    } else {
                        outputMessage.innerText = data.d.message || "Error during decryption.";
                    }
                })
                .catch(error => {
                    //console.error("Error during decryption: ", error);
                    outputMessage.innerText = "Error during decryption.";
                });
        }


        

        // validate if the scanned data is a valid URL
        function isValidUrl(string) {
            try {
                new URL(string);
                return true;
            } catch (_) {
                return false;
            }
        }


        function stopVideoStream() {
            if (videoStream) {
                videoStream.getTracks().forEach(track => track.stop()); 
                videoStream = null; 
                video.pause(); 
                video.srcObject = null; 
                console.log("Video stream stopped.");
            } else {
                console.log("No video stream to stop.");
            }
        }

        function validateAttachmentUpload(fileInput) {
            var allowedExtensions = ['png', 'jpg', 'jpeg'];
            var errorMsg = document.getElementById('attchUploadError');
            var files = fileInput.files;
            var isValid = true;

            // check if the number of files exceeds 10
            if (files.length > 5) {
                errorMsg.textContent = 'You can only upload a maximum of 5 files.';
                errorMsg.style.display = 'block';
                fileInput.value = ''; // clear the file input
                return;
            }

            for (var i = 0; i < files.length; i++) {
                var fileExtension = files[i].name.split('.').pop().toLowerCase();
                if (!allowedExtensions.includes(fileExtension)) {
                    isValid = false;
                    break;
                }
            }

            if (!isValid) {
                errorMsg.textContent = 'Only PNG, JPG, and JPEG files are allowed.';
                errorMsg.style.display = 'block';
                fileInput.value = ''; // clear the file input
            } else {
                errorMsg.style.display = 'none';
            }
        }


        function validateAttachmentUpload2(fileInput) {
            var allowedExtensions = ['png', 'jpg', 'jpeg'];
            var errorMsg = document.getElementById('attchUploadError2');
            var files = fileInput.files;
            var isValid = true;

            // check if the number of files exceeds 10
            if (files.length > 5) {
                errorMsg.textContent = 'You can only upload a maximum of 5 files.';
                errorMsg.style.display = 'block';
                fileInput.value = ''; // clear the file input
                return;
            }

            for (var i = 0; i < files.length; i++) {
                var fileExtension = files[i].name.split('.').pop().toLowerCase();
                if (!allowedExtensions.includes(fileExtension)) {
                    isValid = false;
                    break;
                }
            }

            if (!isValid) {
                errorMsg.textContent = 'Only PNG, JPG, and JPEG files are allowed.';
                errorMsg.style.display = 'block';
                fileInput.value = ''; // clear the file input
            } else {
                errorMsg.style.display = 'none';
            }
        }

        function showPickupImage() {
            
            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);

            // deliveryId from the query string
            const deliveryId = url.get('deliveryId');  

            if (deliveryId) {
               
                $.ajax({
                    type: "POST",
                    url: "UpdateDelivery.aspx/GetPickupImage",
                    data: JSON.stringify({ deliveryId: deliveryId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                       
                        var processedImagesHtml = response.d;

                        // inject the processed HTML into the modal body
                        document.getElementById('pickupImageContent').innerHTML = processedImagesHtml;

                        // show the modal
                        $('#pickupImg').modal('show');
                    },
                    error: function (xhr, status, error) {
                        console.log("Error fetching images: " + error);
                    }
                });
            } else {
                console.log("No deliveryId found in the query string.");
            }
        }

        function showReachImage() {

            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);

            // deliveryId from the query string
            const deliveryId = url.get('deliveryId');

            if (deliveryId) {

                $.ajax({
                    type: "POST",
                    url: "UpdateDelivery.aspx/GetReachImage",
                    data: JSON.stringify({ deliveryId: deliveryId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        var processedImagesHtml = response.d;

                        // inject the processed HTML into the modal body
                        document.getElementById('reachImageContent').innerHTML = processedImagesHtml;

                        // show the modal
                        $('#reachImg').modal('show');
                    },
                    error: function (xhr, status, error) {
                        console.log("Error fetching images: " + error);
                    }
                });
            } else {
                console.log("No deliveryId found in the query string.");
            }
        }

        function initMap() {
            const centerLocation = { lat: 3.1390, lng: 101.6869 }; // Kuala Lumpur
            const map = new google.maps.Map(document.getElementById('map'), {
                center: centerLocation,
                zoom: 12,
            });

            const directionsService = new google.maps.DirectionsService();
            const directionsRenderer = new google.maps.DirectionsRenderer();
            directionsRenderer.setMap(map);

            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);
            const deliveryId = url.get('deliveryId');
            console.log(deliveryId);

            // send rider's location to the backend
            startSendingLocation(deliveryId);

            // fetch pickup and destination addresses from the server
            $.ajax({
                url: 'UpdateDelivery.aspx/GetDeliveryAddress',
                method: 'POST',
                data: JSON.stringify({ deliveryId: deliveryId }),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (response) {
                    const data = JSON.parse(response.d);
                    const pickupAddress = data.pickupAddress;
                    const destinationAddress = data.destinationAddress;

                    console.log("Pickup Address:", pickupAddress);
                    console.log("Destination Address:", destinationAddress);

                    // create Google Maps link for external navigation
                    const googleMapsLink = `https://www.google.com/maps/dir/${encodeURIComponent(pickupAddress)}/${encodeURIComponent(destinationAddress)}`;

                    // display the Google Maps link on the page for the rider to copy
                    const linkContainer = document.getElementById('googleMapsLinkContainer');
                    linkContainer.innerHTML = `<a href="${googleMapsLink}" target="_blank" class="map-link"><i class="fas fa-road"></i> Open Route in Google Maps</a>`;
                 
                    const liveContainer = document.getElementById('liveLinkContainer');
                    liveContainer.style.visibility = 'visible';

                    // copy the link to clipboard
                    navigator.clipboard.writeText(googleMapsLink).then(() => {
                        console.log("Google Maps link copied to clipboard!");
                    }).catch(err => {
                        console.error("Could not copy link to clipboard:", err);
                    });

                    // display route from pickup to destination on the map
                    const geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ address: pickupAddress }, (pickupResults, status) => {
                        if (status === 'OK') {
                            geocoder.geocode({ address: destinationAddress }, (destinationResults, status) => {
                                if (status === 'OK') {
                                    const request = {
                                        origin: pickupResults[0].geometry.location,
                                        destination: destinationResults[0].geometry.location,
                                        travelMode: google.maps.TravelMode.DRIVING,
                                    };

                                    directionsService.route(request, (result, status) => {
                                        if (status === 'OK') {
                                            directionsRenderer.setDirections(result);
                                        } else {
                                            console.error("Could not display directions due to: " + status);
                                        }
                                    });
                                } else {
                                    console.error("Geocode failed for destination: " + status);
                                }
                            });
                        } else {
                            console.error("Geocode failed for pickup: " + status);
                        }
                    });
                },
                error: function () {
                    console.error("Error fetching delivery details.");
                }
            });
        }


        function startSendingLocation(deliveryId) {
            if (navigator.geolocation) {
                navigator.geolocation.watchPosition(
                    (position) => {
                        const latitude = position.coords.latitude;
                        const longitude = position.coords.longitude;

                        // detected location with timestamp 
                        console.log("Detected Location:", { latitude, longitude, time: new Date().toLocaleString() });

                        // send location to backend
                        $.ajax({
                            url: 'UpdateDelivery.aspx/UpdateRiderLocation',
                            method: 'POST',
                            data: JSON.stringify({
                                deliveryId: deliveryId,
                                latitude: latitude,
                                longitude: longitude
                            }),
                            contentType: "application/json; charset=utf-8",
                            dataType: 'json',
                            success: function (response) {
                                console.log("Location sent to backend:", response);
                            },
                            error: function () {
                                console.error("Error sending location to backend.");
                            }
                        });
                    },
                    (error) => {
                        console.error("Error detecting location:", error.message);
                        if (error.code === error.PERMISSION_DENIED) {
                            console.error("Location access denied by user.");
                        } else if (error.code === error.POSITION_UNAVAILABLE) {
                            console.error("Location position unavailable.");
                        } else if (error.code === error.TIMEOUT) {
                            console.error("Location request timed out.");
                        }
                    },
                    {
                        enableHighAccuracy: true,
                        maximumAge: 10000,
                        timeout: 5000
                    }
                );
            } else {
                console.error("Geolocation is not supported by this browser.");
            }
        }
        //startSendingLocation(deliveryId);
    

       function openModal() {
            document.getElementById("liveLocationModal").style.display = "block";
        }

        function closeModal() {
            document.getElementById("liveLocationModal").style.display = "none";
        }

        function saveLiveLink() {
            const liveLink = document.getElementById('<%= liveLocationLink.ClientID %>').value;

            if (!liveLink) {
                showError("Please enter a valid live location link.");
                return;
            }

            const queryString = window.location.search;
            const url = new URLSearchParams(queryString);
            const deliveryId = url.get('deliveryId');
            console.log(deliveryId);

            $.ajax({
                url: 'UpdateDelivery.aspx/saveLiveLink', 
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ deliveryId: deliveryId, liveLink: liveLink }),
                success: function (response) {
                    if (response.success) {
                        showSuccess("Live location link saved successfully!");
                        closeModal();
                    } else {
                       
                    }
                },
                error: function (xhr, status, error) {
                   // console.error("Error:", error);
                    showError("An error occurred while saving the link.");
                }
            });
        }

        
    </script>


</asp:Content>
