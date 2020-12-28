<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCongNgoaiGio.Detail" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
    <link href="/Components/jqwidgets/jqx.darkBlue.css" rel="stylesheet" />
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script src="/Scripts/jquery.linq.min.js"></script>
    <script type="text/javascript">
        function viewModel(item) {
            var self = this;
            self.items = ko.observableArray(item);
            self.thang = ko.observable();
            self.nam = ko.observable();
            self.tuNgay = ko.observable();
            self.denNgay = ko.observable();
            self.TenPhongBan = ko.observable();
             $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyChamCong_FindByKyTinhLuong',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    KyTinhLuong: '<%#Request.QueryString["KyTinhLuong"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                    self.thang(data.Thang);
                    self.nam(data.Nam);
                    self.tuNgay(data.TuNgayString);
                    self.denNgay(data.DenNgayString);
                }
            });
            <%--$.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetPhongBan_ById',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({ id: '<%#Request.QueryString["PhongBan"] %>' }),
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.TenPhongBan(obj.TenBoPhan);
                }
            });--%>
        }

        $(function () {
            var self = this;

            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCongNgoaiGio_ChamCongThang',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    PhongBan: '<%#Request.QueryString["PhongBan"] %>',
                    KyTinhLuong: '<%#Request.QueryString["KyTinhLuong"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                },
                error: function () {
                    alert("Chưa có dữ liệu chấm công!");
                    window.close();
                }
            });

            var view = new viewModel(data);
            ko.applyBindings(view, document.getElementById("chamcongupdate"));
        });
    </script>
    <style>
        td {
            text-align: center;
            padding: 5px;
        }
    </style>
</head>
<body>
    <div id="chamcongupdate">
        <div style="font-family: Arial; font-size: 18px; font-weight: bold; padding-bottom: 5px; text-align: center">BẢNG CHẤM CÔNG NGOÀI GIỜ THÁNG <span data-bind="text: thang"></span>/<span data-bind="text: nam"></span></div>
        <div style="font-family: Arial; font-size: 16px; padding-bottom: 5px; text-align: center">(<span data-bind="text: tuNgay"></span> - <span data-bind="    text: denNgay"></span>)</div>
        <div style="width: 100%; height: 100%">
            <table border="1" style="font-family: Arial; font-size: 14px; border-collapse: collapse; margin: 0 auto">
                <%--<tr>
                    <td data-bind="text: TenPhongBan" colspan="8" style="font-weight: bold; font-size: 16px; text-align: left; background-color: lightgray"></td>
                </tr>--%>
                <tr style="background-color: lightgray">
                    <th style="width: 30px;">STT</th>
                    <th style="width: 200px;">Họ tên</th>
                    <th style="width: 200px;">Đơn vị</th>
                    <th style="width: 120px;">Ngày thường</th>
                    <th style="width: 120px;">Ngày thường sau 23h</th>
                    <th style="width: 120px;">T7/CN</th>
                    <th style="width: 120px;">T7/CN sau 23h</th>
                    <th style="width: 120px;">Ngày lễ</th>
                    <th style="width: 120px;">Ngày lễ sau 23h</th>
                </tr>
                <tbody data-bind="foreach: items">
                    <tr>
                        <td style="text-align: center;" data-bind="text: $index() + 1"></td>
                        <td style="width: 140px; text-align: left; white-space: nowrap;" data-bind="text: HoTen"></td>
                        <td style="width: 140px; text-align: left; white-space: nowrap;" data-bind="text: TenPhongBan"></td>
                        <td data-bind="text: SoCongNgoaiGio"></td>
                        <td data-bind="text: SoCongNgoaiGioSau23Gio"></td>
                        <td data-bind="text: SoCongNgoaiGioT7CN"></td>
                        <td data-bind="text: SoCongNgoaiGioT7CNSau23Gio"></td>
                        <td data-bind="text: SoCongNgoaiGioLe"></td>
                        <td data-bind="text: SoCongNgoaiGioLeSau23Gio"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
