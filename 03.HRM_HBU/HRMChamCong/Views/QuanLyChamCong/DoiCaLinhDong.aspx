<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DoiCaLinhDong.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.DoiCaLinhDong" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.linq.min.js"></script>
    <script src="/Components/jqwidgets/jqx-all.js" type="text/javascript"></script>
    <link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
    <link href="/Components/jqwidgets/jqx.darkBlue.css" rel="stylesheet" />
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script src="/Components/jqwidgets/jqxcore.js"></script>
    <script src="/Components/jqwidgets/jqxknockout.js"></script>
    <script src="/Scripts/moment.js"></script>
    <script type="text/javascript">
        function getDaysInWeek(date) {
            var d = parseInt($.jqx.dataFormat.formatdate(date, 'dd'));
            var m = parseInt($.jqx.dataFormat.formatdate(date, 'MM'));
            var y = parseInt($.jqx.dataFormat.formatdate(date, 'yyyy'));
            var daysArray = [];
            daysInWeek = ['T2', 'T3', 'T4', 'T5', 'T6', '<span style="color:red">T7</span>', '<span style="color:red">CN</span>'];
            daysIndex = { 'Mon': 0, 'Tue': 1, 'Wed': 2, 'Thu': 3, 'Fri': 4, 'Sat': 5, 'Sun': 6 };
            index = daysIndex[(new Date(y, m - 1, d)).toString().split(' ')[0]];
            var numDaysInMonth = /8|3|5|10/.test(--m) ? 30 : m == 1 ? (!(y % 4) && y % 100) || !(y % 400) ? 29 : 28 : 31;

            //truyền vào 1 ngày bất kỳ lấy ra danh sách cả tuần bắt đầu từ thứ 2
            for (var m = moment(new Date(date)) ; m.days() != 1; m.add(-1, 'days')) {

            }
            var num = 0;
            for (var i = m; num <= 6; i.add(1, 'days')) {
                daysArray.push(i.date() + '<br/>' + daysInWeek[num++]);
            }
            return daysArray;
        }

        function viewModel(item) {
            var self = this;
            self.items = ko.observableArray(item);
            self.TenPhongBan = ko.observable();
            self.caChamCong = ko.observableArray([]);
            self.caChamCongSelected = ko.observable();
            self.days = ko.observableArray([]);
            var date = $("#jqxDateDoiCaLinhDong").jqxDateTimeInput('getDate');
            self.days(getDaysInWeek(date));
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetList_CaChamCongForChange',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    obj.unshift({ Oid: '00000000-0000-0000-0000-000000000000', TenCa: ' ' });
                    self.caChamCong = ko.observableArray(obj);
                }
            });
            <%--$.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/HoSoNhanVienBy_MaBoPhan',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    maBoPhan: '<%#Request.QueryString["PhongBan"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                    self.items = ko.observableArray(data);
                }
            });--%>
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetPhongBan_ById',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    id: '<%#Request.QueryString["PhongBan"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.TenPhongBan(obj.TenBoPhan);
                }
            });
            self.Search = function (val) {
                var self = this;
                var date = $("#jqxDateDoiCaLinhDong").jqxDateTimeInput('getDate');
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/DangKyChamCong_DoiCaLinhDong_Find',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ngay: date,
                        boPhanId: '<%#Request.QueryString["PhongBan"] %>'
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        data = $.parseJSON(result.d);
                        //self.items = ko.observableArray(data); //dùng kiểu này không bind dynamic được
                        self.items(data);
                    }
                });
                self.days(getDaysInWeek(date));
            };
            self.Search();
            self.Save = function (val) {
                var self = this;
                var item = new Array();
                $(val.items()).each(function (index, value) {
                    value.ChamCongNgay = new Array();
                    $(value.ChiTietChamCong).each(function (index1, value1) {
                        if (value1.OldValue != value1.CC_CaChamCong)
                            value.ChamCongNgay.push({ CC_ChamCongTheoNgayOid: value1.CC_ChamCongTheoNgayOid, CC_CaChamCong: value1.CC_CaChamCong });
                    });
                    item.push({ ChiTietChamCong: value.ChamCongNgay });
                });
                var obj = $.Enumerable.From(item).Where(function (x) {
                    return x.ChiTietChamCong.length > 0;
                }).ToArray();
                if (obj.length == 0)
                    return;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/DangKyChamCong_DoiCaLinhDong_SaveList',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({
                        chamcongthang: obj
                    }),
                    async: false,
                    success: function (result) {
                        alert("Lưu thành công !!");
                        //window.close();
                    }
                });
            };
        }
        
        $(function () {
            $("#jqxDateDoiCaLinhDong").jqxDateTimeInput({ width: '100px', height: '25px' });
            var date = $("#jqxDateDoiCaLinhDong").jqxDateTimeInput('getDate');

            view = new viewModel();
            ko.applyBindings(view, document.getElementById("doicalinhdong"));
        });
    </script>
</head>
<body>
    <div id="doicalinhdong">
        <table style="display:flex; justify-content:center; align-items:center; text-align:center;">
            <tr>
                <td style="height: 36px">Ngày: </td>
                <td style="padding: 5px;">
                    <div id='jqxDateDoiCaLinhDong'></div>
                </td>
            </tr>
            <%--<tr>
                <td></td>
                <td style="padding-top: 10px;">
                    <input style="margin-right: 5px;" type="button" id="Search" value="Tìm" onclick="Tim()" />
                    <input style="margin-right: 5px;" type="button" id="Save" value="Lưu" data-bind="click: Save" />
                </td>
            </tr>--%>
        </table>
        <table style="display:flex; justify-content:center; align-items:center; text-align:center;">
            <tr>
                <td></td>
                <td style="padding-top: 10px;">
                    <input style="margin-right: 5px;" type="button" value="Tìm" data-bind="click: Search" />
                    <input style="margin-right: 5px;" type="button" value="Lưu" data-bind="click: Save" />
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="1" cellspacing="0" style="font-family: Arial, Tahoma; font-size: 10pt; border: solid 1px #CCCCCC;">
            <tr>
                <%--<td data-bind="attr: { colspan: $root.spanlength }, text: TenPhongBan" style="background-color: #888888; color: White; font-weight: bold; font-size: 14pt;"></td>--%>
                <td data-bind="text: TenPhongBan" colspan="10" style="background-color: #888888; color: White; font-weight: bold; font-size: 14pt;"></td>
            </tr>
            <tr style="height: 30px; border-top: solid 1px #CCCCCC;">
                <th style="width: 30px; border-right: solid 1px #CCCCCC;" rowspan="2">STT</th>
                <th style="width: 110px; border-right: solid 1px #CCCCCC;" rowspan="2">Mã quản lý</th>
                <th style="width: 200px; border-right: solid 1px #CCCCCC;" rowspan="2">Họ tên</th>
                <th style="width: 300px; border-bottom: solid 1px #CCCCCC;" colspan="7">Ngày trong tuần</th>
            </tr>
            <tr data-bind="foreach: days">
                <th style="width: 109px; border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="html: $data" align="center"></th>
            </tr>

            <tbody data-bind="foreach: items">
                <tr>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC;" align="center" data-bind="text: $index() + 1"></td>
                    <td style="border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC; text-align: center; white-space: nowrap;" data-bind="text: MaNhanSu"></td>
                    <td style="border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC; white-space: nowrap;" data-bind="text: HoTen"></td>

                    <!-- ko foreach: ChiTietChamCong -->
                    <td>
                        <select data-bind="options: $root.caChamCong, optionsText: 'TenCa', optionsValue: 'Oid', value: $data.CC_CaChamCong" style="width: 110px; text-align-last: center; padding: 6px 3px; -webkit-appearance: none; -moz-appearance: none;"></select>
                    </td>
                    <!-- /ko -->
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>
