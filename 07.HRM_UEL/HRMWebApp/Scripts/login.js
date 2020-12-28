
function ValidateUser() {
    $.ajax({
        url: '/Account/ValidateUser',
        data: { userName: $("#txtUsername").val(), password: $("#txtPassword").val() },
        dataType: 'application/json',
        type: "POST",
        complete: function (result) {
            if (result != null) {
                window.location = "/#/departmentdisplay";
            } else
                alert("Đăng nhập thất bại");
        }
    });
    return false;
}

$(function () {
    $("#txtPassword").keypress(function (e) {
        if (e.which == 13) {
            ValidateUser();
        }
    });
});

function LoginSuccess() {
    window.location = "/index.html";
}
        
function LoginFailed() {
    alert("Tên đăng nhập hoặc tài khoản không hợp lệ.");
}
