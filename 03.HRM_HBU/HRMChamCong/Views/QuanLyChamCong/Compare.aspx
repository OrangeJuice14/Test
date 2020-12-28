<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compare.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Compare" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Components/jqwidgets/jqx-all.js" type="text/javascript"></script>
    <link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
    <link href="/Components/jqwidgets/jqx.darkBlue.css" rel="stylesheet" />
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script src="/Components/jqwidgets/jqxcore.js"></script>
    <script src="/Components/jqwidgets/jqxknockout.js"></script>
    <script type="text/javascript">
        function viewModel(item) {
            var self = this;
            self.thang = ko.observable();
            self.nam = ko.observable();
            self.tuNgay = ko.observable();
            self.denNgay = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    ngay: '<%#Request.QueryString["Ngay"] %>',
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>'
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
            self.items = ko.observableArray(item);
            self.days = ko.observableArray([]);
            self.HinhThucNghiList = ko.observableArray(["", "+", "NB", "Cô", "H", "Ho", "Ro", "LĐ", "N", "1/2", "Ô", "P", "T", "TS", "P/2"]);
            self.HinhThucNghiListForUpdate = ko.observableArray([]);
            self.TenPhongBan = ko.observable();
            self.daylength = ko.observable();
            self.spanlength = ko.observable();
            self.STT = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetPhongBan_ById',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({ id: '<%#Request.QueryString["PhongBan"] %>' }),
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.TenPhongBan(obj.TenBoPhan);
                    self.STT(obj.STT);
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetListHinhThucNghiForUpdate',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.HinhThucNghiListForUpdate(obj);
                }
            });
        }
        function viewModelDetail(item) {
            var self = this;
            self.thang = ko.observable();
            self.nam = ko.observable();
            self.tuNgay = ko.observable();
            self.denNgay = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    ngay: '<%#Request.QueryString["Ngay"] %>',
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>'
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
            self.items = ko.observableArray(item);
            self.days = ko.observableArray([]);
            self.HinhThucNghiList = ko.observableArray(["", "+", "NB", "Cô", "H", "Ho", "Ro", "LĐ", "N", "1/2", "Ô", "P", "T", "TS", "P/2"]);
            self.HinhThucNghiListForUpdate = ko.observableArray([]);
            self.TenPhongBan = ko.observable();
            self.daylength = ko.observable();
            self.spanlength = ko.observable();
            self.STT = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetListHinhThucNghiForUpdate',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.HinhThucNghiListForUpdate(obj);
                }
            });
        }

        $(function () {
            var self = this;
            self.thang = ko.observable();
            self.nam = ko.observable();
            self.WebGroupID = ko.observable();
            self.WebGroup = ko.observable(0);
            self.WebGroup('<%#HttpContext.Current.Session[SessionKey.WebGroupId.ToString()]%>');
            switch (self.WebGroup()) {
                case "05a1bf24-bd1c-455f-96f6-7c4237f4659e":
                    self.WebGroup(1);
                    break;
                case "9290b6f5-a08f-4d5e-9e73-a20cff4cb825":
                    self.WebGroup(2);
                    break;
                case "00000000-0000-0000-0000-000000000001":
                    self.WebGroup(3);
                    break;
                case "00000000-0000-0000-0000-000000000002":
                    self.WebGroup(2);
                    break;
            }
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    ngay: '<%#Request.QueryString["Ngay"] %>',
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    obj = $.parseJSON(result.d);
                    self.thang(obj.Thang);
                    self.nam(obj.Nam);

                }
            });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        bophanId: '<%#Request.QueryString["PhongBan"] %>',
                    maNhanSu: '<%#Request.QueryString["Value"] %>',
                    idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>'
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
             $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThangDonVi',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        bophanId: '<%#Request.QueryString["PhongBan"] %>',
                    maNhanSu: '<%#Request.QueryString["Value"] %>',
                    idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>'
                }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        data2 = $.parseJSON(result.d);
                    },
                    error: function () {
                        alert("Chưa có dữ liệu chấm công!");
                        window.close();
                    }
             });
            $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.thang(),
                        nam: self.nam(),
                        bophanId: '<%#Request.QueryString["PhongBan"] %>',
                    maNhanSu: '<%#Request.QueryString["Value"] %>',
                    idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>'
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
            view = new viewModel(data);
            view2 = new viewModel(data2);
            //viewDetail = new viewModelDetail(data3);
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetList_NgayTrongKyChamCong',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.thang(),
                    nam: self.nam(),
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                    view.days(data);
                    view.daylength(data.length);
                    view.spanlength(data.length + 7);
                    view2.days(data);
                    view2.daylength(data.length);
                    view2.spanlength(data.length + 7);
                }
            });
            ko.applyBindings(view, $("#chamcong_detail")[0]);
            ko.applyBindings(view2, $("#chamcong_detail2")[0]);
        });
    </script>
    <style>
        @page {
            size: 29.7cm 21cm;
            margin: 15mm 10mm 15mm 15mm;
        }

        table {
            border-spacing: 0;
            border-collapse: collapse;
        }

        th, td {
            border: 1px solid;
        }
    </style>
</head>
<body>
    <div id="chamcong_detail" style="width: 1282px;">
        <table style="font-size: 14pt; font-weight: bold; width: 100%; border: 0px none white">
            <tbody>
                <tr>
                    <td style="border: 0px; text-align: center">
                        <img alt="TRƯỜNG ĐẠI HỌC ĐẠI HỌC QUỐC TẾ HỒNG BÀNG" src="/Images/logo_HBU-log-in.png" width="50%" /></td>
                    <td style="font-family: Arial,Tahoma; font-size: 20pt; font-weight: bold; border: 0px">TRƯỜNG ĐẠI HỌC ĐẠI HỌC QUỐC TẾ HỒNG BÀNG</td>
                </tr>
            </tbody>
        </table>
        <div style="font-size: 14pt; font-weight: bold; padding-bottom: 10px; text-align: center">BẢNG CHI TIẾT CHẤM CÔNG - <span data-bind="text: thang"></span>/ <span data-bind="    text: nam"></span></div>
        <div style="font-size: 12pt; padding-bottom: 10px; text-align: center">(<span data-bind="text: tuNgay"></span> - <span data-bind="    text: denNgay"></span>)</div>
        <table border="0" style="font-family: Arial, Tahoma; font-size: 11pt; padding: 5px;">
            <tr>
                <td data-bind="attr: { colspan: $root.spanlength }" style="font-weight: bold; font-size: 14pt;">
                    <span data-bind="text: TenPhongBan"></span>
                    <span> - Quản trị chấm công</span>
                </td>
            </tr>
            <tr style="height: 30px;">
                <th style="width: 25px;" rowspan="2">STT</th>
                <th style="width: 170px;" rowspan="2">Họ tên</th>
                <th data-bind="attr: { colspan: $root.daylength }">Ngày trong tháng</th>
                <th colspan="4">Quy ra công</th>
                <th style="width: 80px;" rowspan="2">Tổng cộng ngày công tính lương</th>
            </tr>
            <tr>
                <!-- ko foreach: $root.days -->
                <!-- ko if:T7CN -->
                <td style="width: 25px; background-color: lightgray" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                <!-- /ko -->
                <!-- ko if:!T7CN -->
                <td style="width: 25px;" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                <!-- /ko -->
                <!-- /ko -->
                <td style="width: 30px;" align="center">NC</td>
                <td style="width: 30px;" align="center">Phép</td>
                <td style="width: 30px;" align="center">KL</td>
                <td style="width: 30px;" align="center">BHXH</td>
            </tr>
            <tbody data-bind="foreach: items">
                <tr>
                    <td style="text-align: center;" align="center" data-bind="text: $index() + 1"></td>
                    <td style="width: 140px; white-space: nowrap; padding: 5px;" data-bind="text: HoTen"></td>

                    <!-- ko foreach: ChiTietChamCong -->
                     <!-- ko if:DaThayDoi -->
                    <td data-bind="text: $data.MaHinhThucNghi" style="text-align: center;background-color:darkseagreen"></td>
                    <!-- /ko -->
                    <!-- ko if:!DaThayDoi -->
                    <td data-bind="text: $data.MaHinhThucNghi" style="text-align: center;"></td>
                    <!-- /ko -->
                    <!-- /ko -->

                    <td style="text-align: center;" data-bind="text: HuongLuong"></td>
                    <td style="text-align: center;" data-bind="text: Phep"></td>
                    <td style="text-align: center;" data-bind="text: KhongLuong"></td>
                    <td style="text-align: center;" data-bind="text: BHXH"></td>
                    <td style="text-align: center;" data-bind="text: TongCong"></td>
                </tr>
            </tbody>
        </table>           
    </div>
    <div id="chamcong_detail2" style="width: 1282px;">
         <table border="0" style="font-family: Arial, Tahoma; font-size: 11pt; padding: 5px;">
            <tr>
                <td data-bind="attr: { colspan: $root.spanlength }" style="font-weight: bold; font-size: 14pt;">
                    <span data-bind="text: TenPhongBan"></span>
                    <span> - Đơn vị chấm công</span>
                </td>
            </tr>
            <tr style="height: 30px;">
                <th style="width: 25px;" rowspan="2">STT</th>
                <th style="width: 170px;" rowspan="2">Họ tên</th>
                <th data-bind="attr: { colspan: $root.daylength }">Ngày trong tháng</th>
                <th colspan="4">Quy ra công</th>
                <th style="width: 80px;" rowspan="2">Tổng cộng ngày công tính lương</th>
            </tr>
            <tr>
                <!-- ko foreach: $root.days -->
                <!-- ko if:T7CN -->
                <td style="width: 25px; background-color: lightgray" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                <!-- /ko -->
                <!-- ko if:!T7CN -->
                <td style="width: 25px;" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                <!-- /ko -->
                <!-- /ko -->
                <td style="width: 30px;" align="center">NC</td>
                <td style="width: 30px;" align="center">Phép</td>
                <td style="width: 30px;" align="center">KL</td>
                <td style="width: 30px;" align="center">BHXH</td>
            </tr>
            <tbody data-bind="foreach: items">
                <tr>
                    <td style="text-align: center;" align="center" data-bind="text: $index() + 1"></td>
                    <td style="width: 140px; white-space: nowrap; padding: 5px;" data-bind="text: HoTen"></td>

                    <!-- ko foreach: ChiTietChamCong -->
                     <!-- ko if:DaThayDoi -->
                    <td data-bind="text: $data.MaHinhThucNghi" style="text-align: center;background-color:darkseagreen"></td>
                    <!-- /ko -->
                    <!-- ko if:!DaThayDoi -->
                    <td data-bind="text: $data.MaHinhThucNghi" style="text-align: center;"></td>
                    <!-- /ko -->
                    <!-- /ko -->

                    <td style="text-align: center;" data-bind="text: HuongLuong"></td>
                    <td style="text-align: center;" data-bind="text: Phep"></td>
                    <td style="text-align: center;" data-bind="text: KhongLuong"></td>
                    <td style="text-align: center;" data-bind="text: BHXH"></td>
                    <td style="text-align: center;" data-bind="text: TongCong"></td>
                    <%--style: { backgroundColor: LaNhanVienToChucHanhChinh == false ? '#f99191' : '' }--%>
                </tr>
            </tbody>
        </table>
        <div style="width: 100%;" align="left">
            <div style="font-family: Tahoma,Arial; font-size: 10pt; padding-top: 10px;" align="left">
                <span style="font-weight: bold;">Ghi chú:</span><br />
                <table style="float: left; width: 33%; border: 0px">
                    <thead>
                        <tr>
                            <td style="border: 0px;">- +:</td>
                            <td style="border: 0px;">Làm cả ngày</td>
                        </tr>
                    </thead>
                    <tbody data-bind="foreach: HinhThucNghiListForUpdate">
                        <!-- ko if: $index() < 11 -->
                        <tr>
                            <td style="border: 0px;" data-bind="html: '- ' + $data.KyHieu + ':'"></td>
                            <td style="width: 80%; border: 0px;" data-bind="html: $data.TenHinhThucNghi"></td>
                        </tr>
                        <!-- /ko -->
                    </tbody>
                </table>
                <table style="float: left; width: 33%; border: none">
                    <tbody data-bind="foreach: HinhThucNghiListForUpdate">
                        <!-- ko if: $index() >=11 -->
                        <tr>
                            <td style="border: 0px;" data-bind="html: '- ' + $data.KyHieu + ':'"></td>
                            <td style="width: 80%; border: 0px;" data-bind="html: $data.TenHinhThucNghi"></td>
                        </tr>
                        <!-- /ko -->
                    </tbody>
                </table>
            </div>
            <table style="float: left; width: 33%; border: 0px">
                <tr>
                    <td style="border: 0px;">- NC:</td>
                    <td style="border: 0px;">Số công hưởng lương</td>
                </tr>
                <tr>
                    <td style="border: 0px;">- Phép:</td>
                    <td style="border: 0px;">Số ngày nghỉ phép</td>
                </tr>
                <tr>
                    <td style="border: 0px;">- KL:</td>
                    <td style="border: 0px;">Nghỉ không lương</td>
                </tr>
                <tr>
                    <td style="border: 0px;">- BHXH:</td>
                    <td style="border: 0px;">Số công hưởng BHXH</td>
                </tr>
            </table>
        </div>
        <table style="border: 0px; width: 100%; font-size: 11pt;">
            <tbody>
                <tr>
                    <td style="border: 0px; height: 10px"></td>
                </tr>
                <tr>
                    <td style="width: 33%; border: 0px; text-align: center"><b>Người chấm công</b><br>
                        (Ký, họ tên)</td>
                    <td style="width: 33%; border: 0px; text-align: center"><b>Phụ trách bộ phận</b><br>
                        (Ký, họ tên)</td>
                    <td style="width: 33%; border: 0px; text-align: center"><b>Người duyệt</b><br>
                        (Ký, họ tên)</td>
                </tr>
                <tr>
                    <td style="width: 33%; border: 0px; text-align: center"></td>
                    <td style="width: 33%; border: 0px; text-align: center"></td>
                    <td style="width: 33%; border: 0px; text-align: center"></td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>
