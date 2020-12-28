<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AccountInfo.aspx.cs" Inherits="HRMChamCong.Views.User.AccountInfo" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<asp:Content ID="Contaent1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var id;
        function ViewModel(data) {
            var self = this;
            self.MaNhanSu = data.MaNhanSu;
            self.userName = data.UserName;
            self.password = ko.observable(data.Password);
            self.confirmPassword = ko.observable(data.Password);
            self.fullName = ko.observable(data.HoVaTen);
            self.email = ko.observable(data.Email);
            self.accountType = ko.observable(data.UserChamCong == null ? "" : data.UserChamCong.toString());
            self.status = ko.observable(data.HoatDong.toString());
            self.departments = ko.observableArray(data.DanhSachDTO_BoPhan);
            self.changePassword = ko.observable(false);

        }
        ViewModel.prototype =
        {
            validate: function () {
                var self = this;
                return $.trim(self.password()).length != 0 && $.trim(self.confirmPassword()).length != 0 && self.password() == self.confirmPassword();
            },
            save: function () {
                var self = this;
                if (!self.validate())
                    return;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChangePassword_WebUser',
                    async: false,
                    data: JSON.stringify({ webUserId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>', passWord: self.password() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        alert("Lưu thành công !!");
                        location.reload();
                    }
                });
            }
        };

        $(function () {
            id = '<%#Request.QueryString["Id"]%>';

            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetDetail_QuanLyUser',
                async: false,
                data: JSON.stringify({ id: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>' }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    var model = new ViewModel(data);
                    ko.applyBindings(model, $("#accountInfo")[0]);
                }
            });

        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="accountInfo">
        
        <div id="userDetail">
            <div class="row">
                <div class="col-lg-2"></div>
                <div class="col-lg-8 col-sm-12 col-xs-12">
                    <div class="widget flat radius-bordered">
                        <div class="widget-header bg-blue">
                            <span class="widget-caption">THÔNG TIN NGƯỜI QUẢN TRỊ</span>
                        </div>
                        <div class="widget-body">
                            <div class="form-horizontal form-bordered">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label no-padding-right">Mã nhân sự</label>
                                    <div class="col-sm-10">
                                        <input class="form-control" type="text" data-bind="value: MaNhanSu, disable: true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label no-padding-right">Tên user</label>
                                    <div class="col-sm-10">
                                        <input class="form-control" type="text" data-bind="value: userName, disable: true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label no-padding-right">Mật khẩu <span class="validate" data-bind="visible: $.trim(password()).length == 0 || confirmPassword() != password()">(*)</span></label>
                                    <div class="col-sm-7">
                                        <input type="password" class="form-control" data-bind="value: password, disable: changePassword() == false ? true : false" />
                                    </div>
                                    <div class="col-sm-3" style="padding-top: 5px;">
                                        <input type="checkbox" data-bind="checked: changePassword" /><label>Đổi mật khẩu</label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label no-padding-right">Xác nhận mật khẩu <span class="validate" data-bind="visible: $.trim(confirmPassword()).length == 0 || confirmPassword() != password()">(*)</span></label>
                                    <div class="col-sm-7">
                                        <input type="password" class="form-control" data-bind="value: confirmPassword, disable: changePassword() == false ? true : false" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label no-padding-right">Họ tên</label>
                                    <div class="col-sm-10">
                                        <input class="form-control" type="text" data-bind="value: fullName, disable: true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label no-padding-right">Email</label>
                                    <div class="col-sm-10">
                                        <input class="form-control" type="text" data-bind="value: email, disable: true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-2"></div>
                                    <div class="col-sm-10">
                                        <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                                            <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                                        </a>
                                        <a href="javascript:history.back()" class="btn btn-labeled btn-blue" style="width: 158px;">
                                            <i class="btn-label glyphicon glyphicon-chevron-left"></i>Trở về
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-2"></div>
            </div>
        </div>
    </div>
    <script src="assets/js/beyond.min.js"></script>
</asp:Content>
