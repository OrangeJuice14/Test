<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Update" %>

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
            self.dayInMonth = getDaysInMonth('<%#Request.QueryString["Thang"] %>', '<%#Request.QueryString["Nam"] %>');
            self.HinhThucNghiList = ko.observableArray(["", "+", "1/2", "P", "Ro", "TS", "H"]);
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
            self.save = function (val) {
                var self = this;
                var item = new Array();
                $(val.items()).each(function (index, value) {
                    value.ChamCongNgay = new Array();
                    $(value.ChiTietChamCong).each(function (index1, value1) {
                        if (value1.OldValue != value1.MaHinhThucNghi)
                            value.ChamCongNgay.push({ CC_ChamCongTheoNgayOid: value1.CC_ChamCongTheoNgayOid, MaHinhThucNghi: value1.MaHinhThucNghi });
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
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang_Save',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({
                        chamcongthang: obj
                    }),
                    async: false,
                    success: function (result) {
                        alert("Lưu thành công !!");
                        location.reload();
                    }
                });
            };
        }

        $(function () {
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: '<%#Request.QueryString["Thang"] %>',
                    nam: '<%#Request.QueryString["Nam"] %>',
                    bophanId: '<%#Request.QueryString["PhongBan"] %>',
                    maNhanSu: '<%#Request.QueryString["Value"] %>',
                    idLoaiNhanSu: '<%#Request.QueryString["IdLoaiNhanSu"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    data = $.parseJSON(result.d);

                }
            });
            var view = new viewModel(data);
            ko.bindingHandlers.rename = {
                update: function (element, valueAccessor, AllBindings, data) {
                    data["OldValue"] = valueAccessor();
                    var value = ko.observable(valueAccessor());
                    var interceptor = ko.computed({
                        read: function () {
                            return value();
                        },
                        write: function (newValue) {
                            var validate = $.Enumerable.From(view.HinhThucNghiList()).Count(function (x) {
                                return x == newValue;
                            });
                            if (validate == 0) {
                                alert("Ký hiệu không hợp lệ !!");
                                $(element).focus();
                                value(null);
                            } else {
                                value(newValue);
                            }
                            value.valueHasMutated();
                        }
                    }).extend({ notify: 'always' });
                    ko.applyBindingsToNode(element, {
                        value: interceptor
                    });
                }
            };
            ko.applyBindings(view, document.getElementById("chamcongupdate"));
        });
    </script>
</head>
<body>

    <div id="chamcongupdate">
        <table border="0" cellpadding="0" cellspacing="0" width="1400px">
            <tbody>
                <tr>
                    <td>
                        <img alt="Trường Đại Học Công Nghiệp TP.HCM" src="/Images/logo.png" align="middle"></td>
                    <td style="font-family: Arial,Tahoma; font-size: 20pt; font-weight: bold; padding-left: 150px">TRƯỜNG ĐẠI HỌC CÔNG NGHIỆP TP.HCM</td>
                </tr>
            </tbody>
        </table>
        <div align="center" style="font-family: Arial, Tahoma; font-size: 14pt; font-weight: bold; padding-bottom: 5px; width: 1400px;">BẢNG CHI TIẾT CHẤM CÔNG - <%#Request.QueryString["Thang"].ToString() %>/<%#Request.QueryString["Nam"] %></div>
        <table border="0" cellpadding="1" cellspacing="0" style="font-family: Arial, Tahoma; font-size: 10pt; border: solid 1px #CCCCCC; width: 1500px">
            <tr>
                <td colspan="38" style="background-color: #888888; color: White; font-weight: bold; font-size: 14pt;" data-bind="text:TenPhongBan"></td>
            </tr>
            <tr style="height: 30px; border-top: solid 1px #CCCCCC;">
                <th style="width: 25px; border-right: solid 1px #CCCCCC;" rowspan="2">STT</th>
                <th style="width: 200px; border-right: solid 1px #CCCCCC;" rowspan="2">Họ tên</th>
                <th style="border-bottom: solid 1px #CCCCCC;" data-bind="attr: { colspan: dayInMonth.length }">Ngày trong tháng</th>
                <th rowspan="2" style="border-left: solid 1px #CCCCCC;">Ngày công</th>
                <th rowspan="2" style="border-left: solid 1px #CCCCCC;">Nghỉ Ro</th>
                <th rowspan="2" style="border-left: solid 1px #CCCCCC;">Nghỉ phép</th>
                <th rowspan="2" style="border-left: solid 1px #CCCCCC;">Thai sản</th>
                <th rowspan="2" style="border-left: solid 1px #CCCCCC;">Nghỉ hè</th>
            </tr>
            <tr data-bind="foreach:dayInMonth">
                <td style="border-bottom: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="html:$data" align="center"></td>
            </tr>
            <tbody data-bind="foreach:items">
                <tr>
                     <!-- ko if:LaNhanVienToChucHanhChinh -->
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC;" align="center" data-bind="text: $index() + 1"></td>
                    <td style="border-top: solid 1px #CCCCCC; border-right: solid 1px #CCCCCC; width: 140px; white-space: nowrap;" data-bind="text: HoTen"></td>
                   
                    <!-- ko foreach: ChiTietChamCong -->
                    <td>
                        <input data-bind="rename: $data.MaHinhThucNghi, value: $data.MaHinhThucNghi" style="width: 30px; text-align: center;" />
                    </td>
                    <!-- /ko -->
                            
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: NgayCong"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: NghiKhongPhepRo"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: NghiPhep"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: NghiThaiSan"></td>
                    <td style="text-align: center; border-top: solid 1px #CCCCCC; border-left: solid 1px #CCCCCC;" data-bind="text: NghiHe"></td>
                     <!-- /ko -->
                    <%--style: { backgroundColor: LaNhanVienToChucHanhChinh == false ? '#f99191' : '' }--%>
                </tr>
            </tbody>
        </table>
        <center style="padding-top: 10px;">
            <input type="button" name="btnSave" value="Lưu lại" data-bind="click:save" style="font-family:Times New Roman;font-size:18pt;width:150px;font-weight:bold;">
       </center>
        <div style="width: 1000px;" align="left">
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
