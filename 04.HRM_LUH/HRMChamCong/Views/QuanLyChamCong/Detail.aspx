<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Detail" %>

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
        function getDaysInMonth(m, y) {
            var daysArray = [];
            daysInWeek = ['<span style="color:red">CN</span>', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'];
            daysIndex = { 'Sun': 0, 'Mon': 1, 'Tue': 2, 'Wed': 3, 'Thu': 4, 'Fri': 5, 'Sat': 6 };
            index = daysIndex[(new Date(y, m - 1, 1)).toString().split(' ')[0]];
            var numDaysInMonth = /8|3|5|10/.test(--m) ? 30 : m == 1 ? (!(y % 4) && y % 100) || !(y % 400) ? 29 : 28 : 31;
            for (i = 0, l = numDaysInMonth  ; i < l  ; i++) {
                daysArray.push((i + 1) + '<br/>' + daysInWeek[index++]);
                if (index == 7) index = 0;
            }
            return daysArray;
        }
        
        function viewModel(item) {
            var self = this;
            self.items = ko.observableArray(item);
            self.HinhThucNghiList = ko.observableArray();
            self.TenPhongBan = ko.observable();
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
                }
            });
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
            self.dayInMonth = getDaysInMonth('<%#Request.QueryString["Thang"] %>', '<%#Request.QueryString["Nam"] %>');
        }
        
        $(function () {
            var view;
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>',
                    bophanId: '<%#Request.QueryString["PhongBan"] %>',
                    maNhanSu: '<%#Request.QueryString["Value"] %>',
                    idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>',
                }),
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);
                }
            });
            view = new viewModel(data);
            ko.applyBindings(view, $("#chamcong_detail")[0]);
        });
    </script>
</head>
<body>
    <div id="chamcong_detail">
        <table border="0" cellpadding="0" cellspacing="0" width="1200px" >
            <tbody>
                <tr>
                    <td>
                        <img alt="Trường Đại Học Công Nghiệp TP.HCM" src="/Images/logo.png" align="middle"></td>
                    <td style="font-family: Arial,Tahoma; font-size: 20pt; font-weight: bold;padding-left: 75px;">TRƯỜNG ĐẠI HỌC CÔNG NGHIỆP TP.HCM</td>
                </tr>
            </tbody>
        </table>
        <div align="center" style="font-family: Arial, Tahoma; font-size: 14pt; font-weight: bold; padding-bottom: 5px; width: 1200px;text-align: center">BẢNG CHI TIẾT CHẤM CÔNG - <%#Request.QueryString["Thang"].ToString() %>/<%#Request.QueryString["Nam"] %></div>
        <table border="1" width="1200px" cellpadding="1" cellspacing="0">
            <tr>
                <td colspan="38" style="background-color: #888888; color: White; font-weight: bold; font-size: 14pt;" data-bind="text:TenPhongBan"></td>
            </tr>
            <tr style="height: 30px;">
                <th style="width: 25px" rowspan="2">STT</th>
                <th style="width: 400px" rowspan="2">Họ tên</th>
                <th data-bind="attr: { colspan: dayInMonth.length }">Ngày trong tháng</th>
                <th rowspan="2">Ngày công</th>
                <th rowspan="2">Nghỉ Ro</th>
                <th rowspan="2">Nghỉ phép</th>
                <th rowspan="2">Thai sản</th>
                <th rowspan="2">Nghỉ hè</th>
            </tr>
            <tr data-bind="foreach:dayInMonth">
                <td data-bind="html:$data" align="center"></td>
            </tr>
            <tbody data-bind="foreach:items">
                <tr>
                    <td align="center" data-bind="text:$index()+1"></td>
                    <td data-bind="text:HoTen"></td>
                    <!-- ko if:!LaNhanVienToChucHanhChinh -->
                    <td data-bind="attr: { colspan: $parent.dayInMonth .length}" ></td>
                    <!-- /ko -->
                    <!-- ko if:LaNhanVienToChucHanhChinh -->
                    <!-- ko foreach: ChiTietChamCong -->
                    <td align="center"  >
                        <!--ko if:$data.MaHinhThucNghi == '' -->
                        <div style="width: 100%; height: 100%; background-color: #FF9393">&nbsp;</div>
                        <!-- /ko -->
                        <span data-bind="text:$data.MaHinhThucNghi"></span>
                    </td>
                    <!-- /ko -->
                     <!-- /ko -->
                    <td data-bind="text:NgayCong" style="font-weight: bold; text-align: center"></td>
                    <td data-bind="text:NghiKhongPhepRo" style="text-align: center;"></td>
                    <td data-bind="text:NghiPhep" style="text-align: center;"></td>
                    <td data-bind="text:NghiThaiSan" style="text-align: center;"></td>
                    <td data-bind="text:NghiHe" style="text-align: center;"></td>
                </tr>
            </tbody>
        </table>
        <table border="0" style="width: 1200px; height: 100px; font-family: Arial, Tahoma; font-size: 10pt;">
            <tbody>
                <tr>
                    <td height="41" align="center" width="329"><b>Người chấm công</b><br>
                        (Ký, họ tên)</td>
                    <td height="41" align="center"><b>Phụ trách bộ phận</b><br>
                        (Ký, họ tên)</td>
                    <td height="41" align="center" width="327"><b>Người duyệt</b><br>
                        (Ký, họ tên)</td>
                </tr>
                <tr>
                    <td align="center" width="329">&nbsp;</td>
                    <td align="center">&nbsp;</td>
                    <td align="center" width="327">&nbsp;</td>
                </tr>
            </tbody>
        </table>
        <div style="width: 1200px;" align="left">
            <div style="font-family: Tahoma,Arial; font-size: 10pt; width: 200px; padding-top: 10px;" align="left">
                <span style="font-weight: bold;">Ghi chú:</span><br>
                <span style="padding-left: 30px;">- + : Làm cả ngày</span><br>
                <span style="padding-left: 30px;">- 1/2 : Làm nửa ngày</span><br>
                <span style="padding-left: 30px;">- P : Nghỉ có phép</span><br>
                <span style="padding-left: 30px;">- Ro : Nghỉ Ro</span><br>
                <span style="padding-left: 30px;">- TS : Nghỉ thai sản</span><br>
                <span style="padding-left: 30px;">- H : Nghỉ hè</span>
            </div>
        </div>
    </div>
</body>
</html>
