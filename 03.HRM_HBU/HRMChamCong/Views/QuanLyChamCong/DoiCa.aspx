<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="DoiCa.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.DoiCa" %>

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
            self.year = ko.observable(new Date().getFullYear());
            self.department = ko.observableArray();
            self.departmentSelected = ko.observable(null);
            self.caChamCong = ko.observableArray();
            self.caChamCongSelected = ko.observable();
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
                url: '/Services/ChamCongService.asmx/GetList_CaChamCongForChange',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.caChamCong(obj);
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
                { name: 'TenCa', type: 'string' },
                { name: 'TuNgay', type: 'date' },
                { name: 'DenNgay', type: 'date' },
                { name: 'TrangThai', type: 'string' }
               ],
               id: 'Id',
               url: "/Services/ChamCongService.asmx/DangKyChamCong_Find",
               formatdata: function (data) {
                   return {
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
                    selectionmode: 'checkbox',
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
                       {
                           text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                               return "<div style='text-align:center;margin-top:17px;'>" + (value + 1) + "</div>";
                           }
                       },
                    {
                        text: 'Mã nhân sự', datafield: 'SoHieuCongChuc', width: 150, align: 'center', cellsalign: "middle",
                    },
                    {
                        text: 'Họ tên', datafield: 'HoTen', width: 200, align: 'center',
                    },
                    //{
                    //    text: 'Từ ngày', datafield: 'TuNgay', width: 100, align: 'center', cellsformat: 'd/M/yyyy', cellsalign: "middle",
                    //},
                    //{
                    //    text: 'Đến ngày', datafield: 'DenNgay', width: 100, align: 'center', cellsformat: 'd/M/yyyy', cellsalign: "middle",
                    //},
                    {
                        text: 'Đơn vị', datafield: 'TenPhongBan',  align: 'center',
                    },
                    {
                        text: 'Khung giờ', datafield: 'TenCa', width: 150, align: 'center', cellsalign: "middle",
                    },
                    //{
                    //    text: 'Trạng thái', datafield: 'TrangThai', align: 'center', width: 100,
                    //}
                    ]
                });
        }
        ViewModel.prototype = {
            validate: function () {
                var self = this;
                if (isNaN(self.year()) || self.year() < 0) {
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
            create: function () {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/DangKyChamCong_Create',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ca: self.caChamCongSelected()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Thành công!");
                        self.datagrid.jqxGrid('updatebounddata');
                    }
                });
            },
            doicatatca: function () {
                var self = this;
                var r = confirm("Bạn có muốn cập nhật khung giờ làm việc không ?");
                if (r == true) {
                    var selectedRecords = new Array();
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/DangKyChamCong_DoiCa',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            ca: self.caChamCongSelected(),
                            bophan: self.departmentSelected() == undefined ? null : self.departmentSelected(),
                            loai: 1,
                            list: selectedRecords
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            alert("Thành công!");
                            self.datagrid.jqxGrid('updatebounddata');
                        }
                    });
                }
            },
            doicadonvi: function () {
                var self = this;
                var r = confirm("Bạn có muốn cập nhật khung giờ làm việc không ?");
                if (r == true) {
                    var selectedRecords = new Array();
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/DangKyChamCong_DoiCa',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            ca: self.caChamCongSelected(),
                            bophan: self.departmentSelected() == undefined ? null : self.departmentSelected(),
                            loai: 2,
                            list: selectedRecords
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            alert("Thành công!");
                            self.datagrid.jqxGrid('updatebounddata');
                        }
                    });
                }
            },
            doicacanhan: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                var r = confirm("Bạn có muốn cập nhật khung giờ làm việc không ?");
                if (r == true) {
                    var selectedRecords = new Array();
                    for (var i = 0, l = rows.length; i < l ; i++) {
                        var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                        selectedRecords.push({
                            Oid: row.Oid
                        });
                    }
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/DangKyChamCong_DoiCa',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            ca: self.caChamCongSelected(),
                            bophan: self.departmentSelected() == undefined ? null :  + self.departmentSelected(),
                            loai: 3,
                            list: selectedRecords
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            alert("Thành công!");
                            self.datagrid.jqxGrid('updatebounddata');
                        }
                    });
                }
            },
            doicalinhdong: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn Đơn vị !!");
                    return;
                }
                var url = "DoiCaLinhDong.aspx?PhongBan=" + self.departmentSelected();
                var Width = 1150, Height = 600;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
            }
        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#ChamCongNgayNghi")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="ChamCongNgayNghi">
        <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                    <div class="row">
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: create">
                                <i class="btn-label glyphicon glyphicon-tags"></i>Đăng ký mới
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-danger" style="width: 158px;" data-bind="click: doicatatca">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Đổi ca tất cả
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: doicadonvi">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Đổi ca đơn vị
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-warning" style="width: 158px;" data-bind="click: doicacanhan">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Đổi ca cá nhân
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-warning" style="width: 158px;" data-bind="click: doicalinhdong">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Đổi ca linh động
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả đơn vị'"></select>           
            <select data-bind="options: caChamCong, optionsText: 'TenCa', optionsValue: 'Oid', value: caChamCongSelected"></select>
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px; height: 32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:Content>
