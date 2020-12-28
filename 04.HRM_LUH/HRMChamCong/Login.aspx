<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="HRMChamCong.Login" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
    <title>Đăng nhập</title>

    <meta name="description" content="login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="assets/css/font-awesome.min.css" rel="stylesheet" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,400,600,700,300" rel="stylesheet" type="text/css" />
    <link href="assets/css/beyond.min.css" rel="stylesheet" />
    <link href="assets/css/demo.min.css" rel="stylesheet" />
    <link href="assets/css/animate.min.css" rel="stylesheet" />
    <script src="assets/js/skins.min.js"></script>
<%--    <link href="/CSS/style.css" rel="stylesheet" />--%>
    <script src="/Scripts/jquery-1.11.1.min.js"></script>
    <script src="/Scripts/knockout-3.2.0.js"></script>

    <%--<style type="text/css">
        .captcha_login {
            /*clear: both;*/
            /*float: left;*/
            /*margin-top: 10px;
            margin-bottom: 10px;*/
        }

            .captcha_login a {
                /*float: right;*/
                /*margin-top: 15px;*/
                font: 12px Arial, Helvetica, sans-serif;
                color: #333;
                /*display: block;*/
                left: -40px;
                position: relative;
            }

                .captcha_login a:hover {
                    color: #000;
                }

            .captcha_login img {
                margin-right: 10px;
            }

        .nhapma_sub {
            clear: both;
            /*position: relative;*/
            top: -20px;
            /*padding: 0 40px;*/
        }
    </style>--%>
    <script type="text/javascript">
        function ValidateUser(parameters) {
            if ($("#txtUserName").val() == '' || $("#txtPassword").val() == '') {
                $("#txtUserName").focus();
                alert('Vui lòng nhập tên người dùng và mật khẩu');
                return;
            }
            if (parseInt(window.sessionStorage.Count) >= 4) {
                $(".captcha_login").show();
                if ($("#txtCaptcha").val() == '') {
                    alert('Vui lòng nhập mã captcha');
                    $("#txtCaptcha").focus();
                    return;
                }
            }
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/CheckForLogin_WebUser',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({
                    userName: $("#txtUserName").val(),
                    passWord: $("#txtPassword").val(),
                    captchaString: $("#txtCaptcha").val()
                }),
                async: false,
                success: function (data) {
                    var result = data.d;
                    if (parseInt(window.sessionStorage.Count) >= 4) {
                        if (result == 0) {
                            alert('Mã captcha không hợp lệ');
                            $("#txtCaptcha").focus();
                            return;
                        }
                    }
                    if (result == 1)
                        LoginSuccess();
                    else
                        LoginFailed();
                }
            });
        }
        function ValidateEmail(parameters) {
            if ($("#txtUserName").val() == '' || $("#txtPassword").val() == '') {
                $("#txtUserName").focus();
                alert('Vui lòng nhập tên người dùng và mật khẩu');
                return;
            }
            if (parseInt(window.sessionStorage.Count) >= 4) {
                $(".captcha_login").show();
                if ($("#txtCaptcha").val() == '') {
                    alert('Vui lòng nhập mã captcha');
                    $("#txtCaptcha").focus();
                    return;
                }
            }
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/CheckForLogin_WebUser',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({
                    userName: $("#txtUserName").val(),
                    passWord: $("#txtPassword").val(),
                    captchaString: $("#txtCaptcha").val()
                }),
                async: false,
                success: function (data) {
                    var result = data.d;
                    if (parseInt(window.sessionStorage.Count) >= 4) {
                        if (result == 0) {
                            alert('Mã captcha không hợp lệ');
                            $("#txtCaptcha").focus();
                            return;
                        }
                    }
                    if (result == 1)
                        LoginSuccess();
                    else
                        LoginFailed();
                }
            });
        }
        function LoginSuccess() {
            window.location = "/Views/HoSoNhanSu/Manage.aspx?Id=00000000-0000-0000-0000-000000000017";
            //window.location = "/Views/User/AccountInfo.aspx";
            window.sessionStorage.removeItem("Count");
        }
        function LoginFailed() {
            alert("Tên đăng nhập hoặc tài khoản không hợp lệ.");
            $("#txtUserName").focus();
            if (window.sessionStorage.Count == null) {
                window.sessionStorage.Count = 1;
            }
            else {
                window.sessionStorage.Count = parseInt(window.sessionStorage.Count) + 1;
            }
        }
        function refreshImage() {
            var newImage = new Image();
            newImage.src = '/Services/CaptchaImage.ashx?' + new Date().getTime();
            document.getElementById("imgCaptcha").src = newImage.src;
        }
        function searchKeyPress(e) {
            // look for window.event in case event isn't passed in
            e = e || window.event;
            if (e.keyCode == 13) {
                ValidateUser();
            }
        }
        function LoginGmail() {
            window.location = '/LoginGoogleApi.aspx';
        }
    </script>
</head>
<body>
    
        <div class="login-container animated fadeInDown">
        <div class="loginbox bg-white">
            <%--<div class="loginbox-title" style="font-family:sans-serif; margin-bottom:10px;">ĐĂNG NHẬP</div>--%>
            <div class="loginbox-title" style="font-family:sans-serif; "></div>
            <div class="loginbox-social"">
                <div class="logo">
                <img id="logo" src="#" runat="server" />
                
            </div>
                
                <div class="social-title " style="padding-top:10px;">Web thông tin nhân viên và chấm công</div>
            </div>
            <div class="loginbox-or">
                <div class="or-line"></div>
                <div class="or">-*-</div>
            </div>
            <div class="loginbox-textbox">
                <input type="text" class="form-control" placeholder="Tên đăng nhập" id="txtUserName"/>
            </div>
            <div class="loginbox-textbox">
                <input type="password" class="form-control" placeholder="Mật khẩu" id="txtPassword" onkeypress="searchKeyPress(event);"  />
            </div>
                        <div class="captcha_login" style="display: none">
                <img id="imgCaptcha" src="/Services/CaptchaImage.ashx" style="padding: 5px 40px;" />
                <a onclick="refreshImage()" href="javascript:void(0)" class="refreshLink" id="btnRefreshImageLink">
                    <img src="/Images/reload.png" /></a>
                 <div class="loginbox-textbox">
                    <input type="text" id="txtCaptcha" class="form-control" value="" placeholder="Nhập mã captcha" />
                </div>
            </div>
            <div class="loginbox-forgot">
                <a href="#">Quên mật khẩu</a>
            </div>
            <div class="loginbox-submit">
                <input type="button" class="btn btn-primary btn-block" value="Đăng nhập" onclick="ValidateUser()"/>
            </div>
            <div class="loginbox-submit"  >
               <%-- <a href="#" id="loginEmail" class="btn btn-primary btn-block" onclick="ValidateEmail()" runat="server"></a>--%>
                     <input type="button" onclick="LoginGmail()" class="btn btn-primary btn-block" value="Đăng nhập Email" />
            </div>
    </div>
</div>
    <!--Basic Scripts-->
    <script src="assets/js/jquery-2.0.3.min.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/slimscroll/jquery.slimscroll.min.js"></script>

    <!--Beyond Scripts-->
    <script src="assets/js/beyond.js"></script>
    <%--<div class="wrapper_login">
        <div class="login_UIH">
            <div class="logo_UIH">
                <img id="logo" src="#" runat="server">
                
            </div>
            <h3>Web thông tin nhân viên và chấm công</h3>
            <p>
                <input class="gray_login" type="text" placeholder="Tên đăng nhập"   id="txtUserName"/></p>
            <p>
                <input class="gray_login" type="password" placeholder="Mật khẩu" id="txtPassword" onkeypress="searchKeyPress(event);" /></p>
            <div class="captcha_login" style="display: none">
                <img id="imgCaptcha" src="/Services/CaptchaImage.ashx" style="padding: 5px 40px;" />
                <a onclick="refreshImage()" href="javascript:void(0)" class="refreshLink" id="btnRefreshImageLink">
                    <img src="/Images/reload.png"></a>
                <p class="nhapma_sub">
                    <input type="text" id="txtCaptcha" value="" style="height: 25px; width: 245px" placeholder="nhập mã captcha" />
                </p>
            </div>
            <a href="#" class="qmk">Quên mật khẩu</a>
            <a href="#" class="action-button shadow animate blue" onclick="ValidateUser()">Đăng nhập</a>
            <a id="loginEmail" href="#" class="action-button shadow animate blue" onclick="ValidateEmail()" runat="server">Đăng nhập Email</a>
        </div>
        <!--End login UIH-->
    </div>--%>
    <!--End wrapper login-->
    <%--<div class="wrapper">
        <div class="login_UIH">
            <img class="logo_dn" src="/Images/logo_DN.png">
            <h2>Web thông tin nhân viên và chấm công</h2>
            <p>
                <input class="bg_dn" type="text" value="" id="txtUserName">
            </p>
            <p>
                <input class="bg_pass" type="password" value="" id="txtPassword" onkeypress="searchKeyPress(event);">
            </p>
            <a href="#">Quên mật khẩu</a>
            <div class="captcha_login" style="display: none">
                <img id="imgCaptcha" src="/Services/CaptchaImage.ashx" style="padding: 5px 40px;"/>
                <a onclick="refreshImage()" href="javascript:void(0)" class="refreshLink" id="btnRefreshImageLink">
                    <img src="/Images/reload.png"></a>
                <p class="nhapma_sub">
                    <input type="text" id="txtCaptcha" value="" style="height: 25px;width: 245px"/>
                </p>
            </div>

            <p class="login">
                <input type="button" value="Đăng nhập" onclick="ValidateUser()"><img src="/Images/arrow_login.png">
            </p>

        </div>
        <!--End login_UIH-->
    </div>--%>
    <!--End wrapper-->
</body>
</html>
