<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QuanLyViPham.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.QuanLyViPham" %>

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
    <script type="text/javascript">

        function ViewModel(datagrid) {

            var self = this;
            self.returnData = [];
            self.datagrid = datagrid;
            self.day = ko.observable(new Date().getDate()),
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.department = ko.observableArray();
            self.departmentSelected = ko.observable(null);
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
            self.source =
           {
               datatype: "json",
               datafields: [
                { name: 'Oid', type: 'string' },
                { name: 'SoHieuCongChuc', type: 'string' },
                { name: 'HoTen', type: 'string' },
                { name: 'TenPhongBan', type: 'string' },
                { name: 'TenHinhThucViPham', type: 'string' },
                { name: 'TGVaoSangQuyDinh', type: 'string' },
                { name: 'GioVaoSang', type: 'string' },
                { name: 'TGRaSangQuyDinh', type: 'string' },
                { name: 'GioRaSang', type: 'string' },
                { name: 'TGVaoChieuQuyDinh', type: 'string' },
                { name: 'GioVaoChieu', type: 'string' },
                { name: 'TGRaChieuQuyDinh', type: 'string' },
                { name: 'GioRaChieu', type: 'string' },
                { name: 'ThoiGianVaoTre', type: 'string' },
                { name: 'ThoiGianVeSom', type: 'string' },
                { name: 'Ngay', type: 'date', format: 'dd-MM-yyyy' }
               ],
               id: 'Id',
               url: "/Services/ChamCongService.asmx/QuanLyViPham_Find",
               formatdata: function (data) {
                   return {
                       ngay: self.day(),
                       thang: self.month(),
                       nam: self.year(),
                       bophan: self.departmentSelected() == undefined ? null : "'" + self.departmentSelected() + "'",                      
                   };
               },
               beforeprocessing: function (result) {
                   self.returnData = $.parseJSON(result.d);
                   return self.returnData;
               }
           };
            self.dataAdapter = new $.jqx.dataAdapter(self.source, { contentType: 'application/json; charset=utf-8' });
            self.datagrid.jqxGrid(
                {
                    source: self.dataAdapter,
                    //selectionmode: 'checkbox',
                    width: '100%',
                    pageable: true,
                    pagesize: 10,
                    sortable: true,
                    rowsheight: 50,
                    filterable: true,
                    autorowheight: true,
                    autoheight: true,
                    theme: "darkBlue",
                    columns: [
                       //{
                       //    text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                       //        return "<div style='text-align:center;margin-top:30px;'>" + (value + 1) + "</div>";
                       //    }
                       //},
                       {
                           text: 'Mã nhân sự', pinned: true, datafield: 'SoHieuCongChuc', width: 120, align: 'center', cellsalign: "middle"
                       },
                    {
                        text: 'Họ tên', pinned: true, datafield: 'HoTen', width: 120, align: 'center'
                    },
                    {
                        text: 'Đơn vị', datafield: 'TenPhongBan', width: 200, align: 'center'
                    },
                    {
                        text: 'Ngày', datafield: 'Ngay', width: 80, cellsformat: 'd/M/yyyy', align: 'center', cellsalign: "middle"
                    },
                    {
                        text: 'Hình thức vi phạm', datafield: 'TenHinhThucViPham', width: 150, cellsalign: "middle", sortable: false, align: 'center',
                    },
                    //{
                    //    text: 'Vào sáng QĐ', datafield: 'TGVaoSangQuyDinh', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    //    renderer: function (row) {
                    //        var res = '';
                    //        if (row.indexOf('<br>') > -1) {
                    //            // multi-line
                    //            res = '<div class="grid-column-header-multi">';
                    //        } else {
                    //            res = '<div class="grid-column-header">';
                    //        }
                    //        res += row + '</div>';
                    //    },
                    //},
                    {
                        text: 'TG vào sáng', datafield: 'GioVaoSang', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    },
                    //{
                    //    text: 'Ra sáng QĐ', datafield: 'TGRaSangQuyDinh', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    //},
                    //{
                    //    text: 'TG ra sáng', datafield: 'GioRaSang', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    //},
                    //{
                    //    text: 'Vào chiều QĐ', datafield: 'TGVaoChieuQuyDinh', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    //},
                    //{
                    //    text: 'TG vào chiều', datafield: 'GioVaoChieu', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    //},
                    //{
                    //    text: 'Ra chiều QĐ', datafield: 'TGRaChieuQuyDinh', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    //},
                    {
                        text: 'TG ra chiều', datafield: 'GioRaChieu', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    },
                    {
                        text: 'TG vào trễ', datafield: 'ThoiGianVaoTre', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    },
                    {
                        text: 'TG ra sớm', datafield: 'ThoiGianVeSom', width: 100, cellsalign: "middle", sortable: false, align: 'center',
                    }

                    ]
                });
        }
        ViewModel.prototype = {
            daysInMonth: function (month, year) {
                return new Date(year, month, 0).getDate();
            },
            validate: function () {
                var self = this;
                if (isNaN(self.day()) || self.day() < 0 || self.day() > parseInt(self.daysInMonth(self.month(), self.year()))) {
                    alert("Ngày không hợp lệ !!");
                    return true;
                } else if (isNaN(self.month()) || self.month() < 0 || self.month() > 12) {
                    alert("Tháng không hợp lệ !!");
                    return true;
                }
                else if (isNaN(self.year()) || self.year() < 0) {
                    alert("Năm không hợp lệ !!");
                    return true;
                }
                return false;
            },
            search: function () {
                var self = this;
                if (self.validate())
                    return;
                self.datagrid.jqxGrid('updatebounddata');
            },
        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#ChamCongNgayNghi")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ChamCongNgayNghi">
       
    <div style="margin: 10px 0px 10px 0px; text-align: center">
        <input type="text" placeholder="ngày" data-bind="value: day" style="width: 50px;height:32px; text-align: center" maxlength="2" />
        -
        <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px;height:32px; text-align: center" maxlength="2" />
        -
        <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px;height:32px; text-align: center" maxlength="4" />
        <select style="width: 200px" data-bind="options: department, optionsText: function (type) { return type.TenBoPhan  }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả Đơn vị'"></select>
        <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
    </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:Content>
