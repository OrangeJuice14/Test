<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="XemBangChamCong.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.XemBangChamCong" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function getDaysInMonth(m, y) {
            var daysArray = [];
            daysInWeek = ['<span style=" background-color: lightgrey"">CN</span>', 'T2', 'T3', 'T4', 'T5', 'T6', '<span style=" background-color: lightgrey"">T7</span>'];
            daysIndex = { 'Sun': 0, 'Mon': 1, 'Tue': 2, 'Wed': 3, 'Thu': 4, 'Fri': 5, 'Sat': 6 };
            index = daysIndex[(new Date(y, m - 1, 1)).toString().split(' ')[0]];
            var numDaysInMonth = /8|3|5|10/.test(--m) ? 30 : m == 1 ? (!(y % 4) && y % 100) || !(y % 400) ? 29 : 28 : 31;
            for (i = 0, l = numDaysInMonth  ; i < l  ; i++) {
                daysArray.push((i + 1) + '<br/>' + daysInWeek[index++]);
                if (index == 7) index = 0;
            }
            return daysArray;
        }
        function ViewModel(datagrid) {
            var self = this;
            self.day = ko.observable(new Date().getDate()),
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.department = ko.observableArray();
            self.departmentSelected = ko.observable(null);
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
            self.name = ko.observable("");
            self.items = ko.observableArray();
            self.daylength = ko.observable();
            self.spanlength = ko.observable();
            self.days = ko.observableArray([]);
            self.checkChot = ko.observable(false);
            self.webGroupId = ko.observable('<%#HttpContext.Current.Session[SessionKey.WebGroupId.ToString()]%>'.toUpperCase());
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
                    if (obj.length > 0)
                        self.departmentSelected(obj[0].Oid);
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
                    //if (obj.length > 0)
                    //    self.loaiNhanSuSelected(obj[0].Oid);
                }
            });
            self.search();
            self.dayInMonth = getDaysInMonth(self.month(), self.year());
        }
        ViewModel.prototype = {
            search: function () {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_CheckChotDonVi',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.month(),
                        nam: self.year(),
                        boPhanId: self.departmentSelected() == undefined ? 
                            '<%#HttpContext.Current.Session[SessionKey.BoPhanId.ToString()]%>'
                            : self.departmentSelected()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        self.checkChot(result.d);
                    }
                });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/GetList_NgayTrongKyChamCong',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.month(),
                        nam: self.year()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        self.days(data);
                        self.daylength(data.length);
                        self.spanlength(data.length + 7);
                    }
                });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_ChamCongThang',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({
                        thang: self.month(),
                        nam: self.year(),
                        bophanId: self.departmentSelected() == undefined ? null : self.departmentSelected(),
                        maNhanSu: self.name(),
                        idLoaiNhanSu: self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected(),
                    }),
                    async: false,
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        for (var i = 0, l = data.length; i < l; i++) {
                            for (var j = 0, ll = self.days().length; j < ll; j++) {
                                if (data[i].ChiTietChamCong.findIndex(q => q.Ngay == self.days()[j].NgayChamCong) === -1) {
                                    data[i].ChiTietChamCong.push({
                                        MaHinhThucNghi: '',
                                        KyHieu: '',
                                        Ngay: self.days()[j].NgayChamCong
                                    });
                                }
                            }
                            data[i].ChiTietChamCong.sort(function (a, b) { return (a.Ngay > b.Ngay) ? 1 : ((b.Ngay > a.Ngay) ? -1 : 0); });
                        }
                        self.items(data);
                    }
                });
            },
            daysInMonth: function (month, year) {
                return new Date(year, month, 0).getDate();
            },
            //print: function () {
            //    var self = this;
            //    if (self.webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21') {
            //        if (self.departmentSelected() == undefined) {
            //            alert("Vui lòng chọn phòng ban !!");
            //            return;
            //        }
            //    }
            //    var url = "Detail.aspx?PhongBan=" + self.departmentSelected() + "&IdLoaiNhanSu=" + (self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected()) + "&Thang=" + self.month() + "&Nam=" + self.year() + "&Value=" + self.name();
            //    var Width = 800, Height = 700;
            //    var OffsetHeight = document.body.offsetHeight;
            //    var OffsettWidth = document.body.offsetWidth;
            //    var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
            //    objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

            //},
            chart: function () {
                var self = this;
                if (self.webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21') {
                    if (self.departmentSelected() == undefined) {
                        alert("Vui lòng chọn phòng ban !!");
                        return;
                    }
                }
                var url = "Chart.aspx?PhongBan=" + self.departmentSelected() + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year();
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
            }
        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#xembangchamcong")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="xembangchamcong">
                <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                    <div class="row">
<%--                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: print">
                                <i class="btn-label glyphicon glyphicon-print"></i>In
                            </a>
                        </div>--%>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: chart" >
                                <i class="btn-label glyphicon glyphicon-stats"></i>Biểu đồ
                            </a>
                        </div>
                        
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="javascript:history.back()" class="btn btn-labeled btn-blue" style="width: 158px;">
                                <i class="btn-label glyphicon glyphicon-chevron-left"></i>Trở về
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <input type="text"  placeholder="ngày" data-bind="value: day" style="width: 50px; height:32px; text-align: center;" maxlength="2" />
            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px;height:32px; text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px;height:32px; text-align: center" maxlength="4" />
            <select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected, visible: webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21' "></select>
            <select data-bind="options: loaiNhanSu, optionsText: 'TenLoaiNhanSu', optionsValue: 'Oid', value: loaiNhanSuSelected, optionsCaption: 'Tất cả', visible: webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21'"></select>
            <input type="text" placeholder="Mã nhân sự" data-bind="value: name, visible: webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21'" style="width: 150px; height:32px; padding-left:5px;" />
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
        </div>
        <!-- ko if: webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21' || checkChot() -->
        <div style="padding: 0px 0px 0px 0px; overflow: auto">
            <table border="1" cellpadding="1" cellspacing="0" width="1200px" style="padding:5px; background-color:white;">
                <tr style="height: 30px;"">
                    <th style="width: 25px;" rowspan="2">STT</th>
                    <th style="text-align:center" rowspan="2">Mã quản lý</th>
                    <th style="text-align:center" rowspan="2">Họ tên</th>
                    <th style="text-align:center" data-bind="attr: { colspan: $root.daylength }">Ngày trong tháng</th>
                    <th style="text-align:center" colspan="4">Quy ra công</th>
                    <th style="width: 80px;text-align:center" rowspan="2">Tổng cộng</th>
                </tr>
                <tr>
                    <!-- ko foreach: $root.days -->
                    <!-- ko if:T7CN -->
                    <td style="width: 25px; background-color: lightgrey" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                    <!-- /ko -->
                    <!-- ko if:!T7CN -->
                    <td style="width: 25px;" data-bind="html: $data.Ngay + '<br>' + $data.Thu" align="center"></td>
                    <!-- /ko -->
                    <!-- /ko -->
                    <td style="width: 30px;" align="center">HL</td>
                    <td style="width: 30px;" align="center">Phép</td>
                    <td style="width: 30px;" align="center">KL</td>
                    <td style="width: 30px;" align="center">BHXH</td>
                </tr>
                <tbody data-bind="foreach: items">
                    <tr >
                        <td align="center" data-bind="text: $index() + 1" ></td>
                        <td data-bind="text: MaNhanSu" style="padding:5px;width: 100px; text-align:center"></td>
                        <td data-bind="text: HoTen" style="padding:5px;width: 300px;"></td>

                        <%--<td data-bind="attr: { colspan: $parent.daylength}"></td>--%>
                        <!-- ko foreach: ChiTietChamCong -->

                        <td align="center" style="padding:0;margin:0;">
                            <span data-bind="text: $data.MaHinhThucNghi"></span>                           
                        </td>
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
        <!-- /ko -->
        <!-- ko if: webGroupId() == '53D57298-1933-4E4B-B4C8-98AFED036E21' && !checkChot() -->
        <div style="font-size: 18px; text-align: center">
            Tháng này chưa chốt dữ liệu chấm công!
        </div>
        <!-- /ko -->
    </div>
</asp:Content>
