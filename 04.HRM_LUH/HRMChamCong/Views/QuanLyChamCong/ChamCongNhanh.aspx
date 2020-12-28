<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ChamCongNhanh.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.ChamCongNhanh" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            var source;
            var pathname = window.location.pathname;
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/WebMenu_GetURLListBy_WebUserId',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    source = data;
                }
            });
            var check = $.inArray(pathname, source);
            if (check < 0) {
                window.location.href = "../../Default.aspx";
            }
        });
                    </script>
    <style type="text/css">
        .formGroup {
            padding: 10px 0px 0px 0px;
            margin: 0 auto;
        }

            .formGroup label {
                float: left;
                width: 120px;
            }

            .formGroup span {
                padding: 0px 10px;
            }

        .container {
            border: solid 1px #7F9DB9;
            width: 400px;
            height: 500px;
            overflow-y: scroll;
        }

        .form_checkbox {
            padding: 0 5px;
        }

        h3 {
            color: #3B6097;
        }

        .formEvent {
            float: right;
        }

            .formEvent a {
                color: #3B6097;
                width: 50px;
                float: left;
            }

        .validate {
            color: red;
        }
    </style>
    <script type="text/javascript">
        var month, year, status;
        function checkExits(month, year) {
            var check;
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/DoDuLieuChamCongThang_CheckExists',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({ thang: month, nam: year }),
                dataType: "json",
                async: false,
                success: function (result) {
                    check = result.d;
                }
            });
            return check;
        }
        function viewModel() {
            var self = this;
            self.day = ko.observable(),
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.HinhThucNghiList = ko.observableArray();
            self.hinhThucNghiSelected = ko.observable("");
            self.department = ko.observableArray();
            self.departmentSelected = ko.observable(null);
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetListHinhThucNghi',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.HinhThucNghiList(obj);
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetDepartmentsOfUser',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({ userId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>' }),
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.department(obj);
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetList_LoaiNhanSu',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.loaiNhanSu(obj);
                    self.loaiNhanSuSelected(obj[0].Oid);
                }
            });
        }
        viewModel.prototype = {
            daysInMonth: function (month, year) {
                return new Date(year, month, 0).getDate();
            },
            validate: function () {
                var self = this;
                if (isNaN(self.day()) || self.day() < 0 || self.day() > parseInt(self.daysInMonth(self.month(), self.year()))) {
                    alert("Ngày không hợp lệ !!");
                    $("#txtDay").focus();
                    return true;
                } else if (isNaN(self.month()) || self.month() < 0 || self.month() > 12) {
                    alert("Tháng không hợp lệ !!");
                    $("#txtMonth").focus();
                    return true;
                }
                else if (isNaN(self.year()) || self.year() < 0) {
                    alert("Năm không hợp lệ !!");
                    $("#txtYear").focus();
                    return true;
                }
                return false;
            },
            save: function () {
                var self = this;
                if (self.validate())
                    return;

                if (!checkExits(self.month(), self.year())) {
                    alert('Tháng này hiện tại chưa có dữ liệu để chấm công !!');
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNhanh',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ngay: self.day(),
                        thang: self.month(),
                        nam: self.year(),
                        idHinhThucNghi: self.hinhThucNghiSelected() == undefined ? null : self.hinhThucNghiSelected(),
                        idBoPhan: self.departmentSelected() == undefined ? null : self.departmentSelected(),
                        idLoaiNhanSu: self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected(),
                        webUserId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>',
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Chấm công thành công !!");
                    }
                });
            }
        };
        $(function () {
            var view = new viewModel();
            ko.applyBindings(view, $("#chamcongnhanh")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="chamcongnhanh">
        
        <div style="font-family:sans-serif; font-size:24px;color:cadetblue">Chấm công nhanh</div>
        <div class="formGroup">
            <label>Ngày chấm công</label>
            <input type="text" id="txtDay" data-bind="value:day" style="width: 50px; text-align:center; height:32px;" maxlength="2" />
            - 
        <input type="text" id="txtMonth" data-bind="value:month" style="width: 50px;text-align:center; height:32px;" maxlength="2" />
            - 
        <input type="text" id="txtYear" data-bind="value:year" style="width: 50px;text-align:center; height:32px;" maxlength="4" />
            (Ngày - Tháng - Năm)
        </div>
        <div class="formGroup">
            <label>Loại nhân sự:</label>
            <select style="width: 150px" data-bind="options:loaiNhanSu, optionsText: 'TenLoaiNhanSu', optionsValue: 'Oid',value:loaiNhanSuSelected,optionsCaption:'Tất cả'"></select>
        </div>
        <div class="formGroup">
            <label>Phòng ban:</label>
            <select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả phòng ban'"></select>
        </div>
        <div class="formGroup">
            <label>Trạng thái:</label>
            <select style="width: 150px" data-bind="options:HinhThucNghiList, optionsText: 'TenHinhThucNghi', optionsValue: 'Oid',value:hinhThucNghiSelected,optionsCaption:'Làm cả ngày'"></select>
        </div>
        <div class="formGroup">
            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                                            <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                                        </a>
                     <a href="javascript:history.back()" class="btn btn-labeled btn-blue"style="width: 158px;">
                <i class="btn-label glyphicon glyphicon-chevron-left"></i>Trở về
                                        </a>
                                     
        </div>
    </div>
</asp:Content>
