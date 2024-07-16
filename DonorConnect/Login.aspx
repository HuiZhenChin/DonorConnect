<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DonorConnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="/Content/Login.css" rel="stylesheet" type="text/css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="card">
                <div class="row no-gutters flex-column flex-md-row">
                    <div class="col-12 col-md-6 image-container">
                        <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/img1.webp"
                             alt="login form" class="img-fluid image-side" />
                    </div>
                    <div class="col-12 col-md-6 d-flex align-items-center">
                        <div class="card-body p-4 p-lg-5 text-black">
                            <div class="d-flex align-items-center mb-3 pb-1">
                                <i class="fas fa-cubes fa-2x me-3" style="color: #ff6219;"></i>
                                <span class="h1 fw-bold mb-0">Logo</span>
                            </div>
                            <h5 class="fw-normal mb-3 pb-3" style="letter-spacing: 1px;">Sign into your account</h5>
                            <div class="form-outline mb-4">
                                <input type="email" id="form2Example17" class="form-control form-control-lg" />
                                <label class="form-label" for="form2Example17">Email address</label>
                            </div>
                            <div class="form-outline mb-4">
                                <input type="password" id="form2Example27" class="form-control form-control-lg" />
                                <label class="form-label" for="form2Example27">Password</label>
                            </div>
                            <div class="pt-1 mb-4">
                                <button class="btn btn-dark btn-lg btn-block" type="button">Login</button>
                            </div>
                            <a class="small text-muted" href="#!">Forgot password?</a>
                            <p class="mb-5 pb-lg-2" style="color: #393f81;">Don't have an account? <a href="#!" style="color: #393f81;">Register here</a></p>
                            <a href="#!" class="small text-muted">Terms of use.</a>
                            <a href="#!" class="small text-muted">Privacy policy</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://kit.fontawesome.com/a076d05399.js"></script>
</body>
</html>

