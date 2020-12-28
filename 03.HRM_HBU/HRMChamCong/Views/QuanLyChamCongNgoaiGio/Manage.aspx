<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCongNgoaiGio.Manage" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
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
        function formatDate(date) {
            return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
        }

        function ViewModel(datagrid) {
            var conditionArr = [
                { Id: -1, Name: "Tất cả trạng thái" },
                { Id: 0, Name: "Chưa chấm công" },
                { Id: 1, Name: "Đã chấm công" }
            ];
            var diHocArr = [
                { Id: null, Name: "Tất cả trạng thái" },
                { Id: false, Name: "Làm việc" },
                { Id: true, Name: "Đi học" }
            ];
            var self = this;
            self.day = ko.observable(new Date().getDate()),
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.returnData = [];
            self.datagrid = datagrid;
            self.BangLuong = ko.observableArray();
            self.BangLuongSelected = ko.observable();
            self.KyTinhLuong = ko.observableArray();
            self.KyTinhLuongSelected = ko.observable();
            self.condition = ko.observableArray(conditionArr);
            self.conditionSelected = ko.observable(-1);
            self.name = ko.observable("");
            self.department = ko.observableArray();
            self.departmentSelected = ko.observable();
            self.departmentSelectedName = ko.observable(null);
            self.departmentSelected_nhansu = ko.observable(null);
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
            self.grid_hosonhanvien = ko.observableArray();
            self.status = ko.observableArray();
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
            self.checkExist = function (ngay, nhanvienid) {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ngay: ngay,
                        nhanvienid: nhanvienid,

                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            self.checkNgay = function (kytinhluong, ngay) {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_CheckNgay',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        kytinhluong: kytinhluong,
                        ngay: ngay
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            self.departmentSelected_nhansu.subscribe(function (newValue) {
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/HoSoNhanVienBy_MaBoPhanChamCongNgoaiGio',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ maBoPhan: newValue == undefined ? null : newValue }),
                    async: false,
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        self.grid_hosonhanvien(obj);
                    }
                });

                var source =
                {
                    localdata: self.grid_hosonhanvien(),
                    datafields:
                    [
                        { name: 'Oid', type: 'string' },
                        { name: 'MaQuanLy', type: 'string' },
                        { name: 'Ho', type: 'string' },
                        { name: 'Ten', type: 'string' }
                    ],
                    datatype: "json"
                };
                var dataAdapter = new $.jqx.dataAdapter(source);
                // initialize jqxGrid
                $("#jqxdropdownbutton").jqxDropDownButton({ width: 150, height: 25 });
                $("#jqxgrid_hosonhanvien").jqxGrid(
                    {
                        width: 250,
                        source: dataAdapter,
                        pageable: true,
                        autoheight: true,
                        autorowheight: true,
                        columnsresize: true,
                        showfilterrow: true,
                        filterable: true,
                        columns: [
                            { text: 'Mã quản lý', datafield: 'MaQuanLy', width: 90, },
                            { text: 'Họ', datafield: 'Ho', width: 90, },
                            { text: 'Tên', datafield: 'Ten', columntype: 'textbox', width: 70, },
                        ]
                    });
                $("#jqxgrid_hosonhanvien").on('rowselect', function (event) {
                    var args = event.args;
                    var row = $("#jqxgrid_hosonhanvien").jqxGrid('getrowdata', args.rowindex);
                    if (row == undefined)
                        return;
                    var dropDownContent = '<div style="position: relative; margin-left: 3px; margin-top: 5px;">' + row['Ho'] + ' ' + row['Ten'] + '</div>';
                    $("#jqxdropdownbutton").jqxDropDownButton('setContent', dropDownContent);
                });
                $("#jqxgrid_hosonhanvien").jqxGrid('selectrow', 0);
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
                    //self.department.unshift({ Oid: undefined, TenBoPhan: 'Tất cả' });
                    //self.departmentSelected(obj[0].Oid);
                }
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyTinhLuong',
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    var arr = new Array();
                    data = $.Enumerable.From(data).Where(function (value) {
                        return value.Name = 'Kỳ tính lương tháng ' + value.Thang + '/' + value.Nam;
                    }).OrderBy(function (value) {
                        return value.Thang;
                    }).ToArray();
                    self.BangLuong(data);
                    self.BangLuongSelected(data[data.length - 1].Oid);
                    self.KyTinhLuong(data);
                    if (self.KyTinhLuong().length > 0) {
                        self.KyTinhLuongSelected(self.KyTinhLuong()[self.KyTinhLuong().length - 1].Oid);
                    }
                }
            });
            self.checkChot = function () {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCongNgoaiGio_CheckChot',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ngay: self.day(),
                        thang: self.month(),
                        nam: self.year(),
                        boPhanId: self.departmentSelected()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            }
            self.checkChotByKyTinhLuong = function () {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCongNgoaiGio_CheckChotByKy',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        boPhanId: self.departmentSelected(),
                        kyTinhLuong: self.KyTinhLuongSelected()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            }
            self.source =
             {
                 datatype: "json",
                 datafields: [
                     { name: 'Oid', type: 'string' },
                     { name: 'MaNhanSu', type: 'string' },
                     { name: 'HoTen', type: 'string' },
                     { name: 'TenPhongBan', type: 'string' },
                     { name: 'SoCongNgoaiGio', type: 'string' },
                    { name: 'SoCongNgoaiGioSau23Gio', type: 'string' },
                    { name: 'SoCongNgoaiGioT7CN', type: 'string' },
                    { name: 'SoCongNgoaiGioT7CNSau23Gio', type: 'string' },
                    { name: 'SoCongNgoaiGioLe', type: 'string' },
                    { name: 'SoCongNgoaiGioLeSau23Gio', type: 'string' },
                    { name: 'Ngay', type: 'string' },
                    { name: 'TuGio', type: 'string' },
                    { name: 'DenGio', type: 'string' },
                    { name: 'DaChinhSua', type: 'int' }
                 ],
                 id: 'Id',
                 // sortcolumn: 'HoTen',
                 //sortdirection: 'asc',
                 url: "/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_Find",
                 formatdata: function (data) {
                     return {
                         kytinhluong: self.KyTinhLuongSelected() == undefined ? null : "'" + self.KyTinhLuongSelected() + "'",
                         bophan: self.departmentSelected() == undefined ? null : "'" + self.departmentSelected() + "'"
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
                width: '100%',
                editable: true,
                selectionmode: 'checkbox',
                pageable: true,
                pagesize: 5,
                sortable: true,
                filterable: true,
                rowsheight: 50,
                columnsheight: 60,
                autorowheight: true,
                autoheight: true,
                theme: "darkBlue",
                columns: [
                  {
                      text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                          return "<div style='text-align:center;margin-top:15px;'>" + (value + 1) + "</div>";
                      }
                  },
                  {
                      text: 'Mã nhân sự', datafield: 'MaNhanSu', align: 'center', width: 100, align: 'center', cellsalign: "middle", editable: false,
                      cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                          var item = self.datagrid.jqxGrid('getrowdata', row);
                          if (item.DaChinhSua == 1) {
                              return '<div style="padding:15px 0 0 5px"><a href="javascript:detail(' + "'" + item.Oid.toString() + "'" + ')" style="color:red">' + cellvalue + '</a></div>';
                          }
                          if (item.DaChinhSua == 0) {
                              return "<div style='padding:15px 0 0 5px'><span>" + cellvalue + "</span></div>";
                          }
                      }
                  },
                  //{
                  //    text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 100, align: 'center', cellsalign: "middle", editable: false
                  //},
                  {
                      text: 'Họ tên', datafield: 'HoTen', width: 140, align: 'center', editable: false
                  },
                  {
                      text: 'Đơn vị', datafield: 'TenPhongBan', width: 200, align: 'center', editable: false
                  },
                  {
                      text: 'Ngày', datafield: 'Ngay', width: 100, align: 'center', cellsalign: "middle", editable: false, cellsrenderer: function (row, column, value) {
                          return "<div style='text-align:center;margin-top:15px;'>" + formatDate(new Date(value)) + "</div>";
                      }
                  },
                  {
                      text: 'Từ giờ', datafield: 'TuGio', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'Đến giờ', datafield: 'DenGio', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'Ngày thường', datafield: 'SoCongNgoaiGio', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'Ngày thường<br/>sau 23h', datafield: 'SoCongNgoaiGioSau23Gio', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'T7/CN', datafield: 'SoCongNgoaiGioT7CN', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'T7/CN<br/>sau 23h', datafield: 'SoCongNgoaiGioT7CNSau23Gio', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'Ngày lễ', datafield: 'SoCongNgoaiGioLe', width: 100, align: 'center', cellsalign: "middle",
                  },
                  {
                      text: 'Ngày lễ<br/>sau 23h', datafield: 'SoCongNgoaiGioLeSau23Gio', width: 100, align: 'center', cellsalign: "middle",
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
            New: function () {
                var self = this;
                $("#Cancel").jqxButton({ theme: "darkBlue" });
                $("#Save").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $("#jqxDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#popupWindow").jqxWindow('open');
                ko.applyBindings(self, $("#popupWindow")[0]);
            },
            Create: function () {
                var self = this;
                var getselectedrow_edit = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrow_edit[0]);
                var getselectedrowindexes = $('#jqxgrid_hosonhanvien').jqxGrid('getselectedrowindexes');
                var selectedrow_hsnhanvien = $('#jqxgrid_hosonhanvien').jqxGrid('getrowdata', getselectedrowindexes[0]);
                if (!self.checkNgay(self.BangLuongSelected(), $('#jqxDate').jqxDateTimeInput('getDate'))) {
                    alert("Ngày không nằm trong kỳ tính lương!");
                    return;
                }
                if (self.checkExist($('#jqxDate').jqxDateTimeInput('getDate'), selectedrow_hsnhanvien.Oid)) {
                    alert("Đã có dữ liệu!");
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_TaoMoi',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        nhanVienId: selectedrow_hsnhanvien.Oid,
                        kytinhluong: self.BangLuongSelected(),
                        ngay: $('#jqxDate').jqxDateTimeInput('getDate'),
                        SoCongNgoaiGio: $("#SoCongNgoaiGio").val(),
                        SoCongNgoaiGioSau23Gio: $("#SoCongNgoaiGioSau23Gio").val(),
                        SoCongNgoaiGioT7CN: $("#SoCongNgoaiGioT7CN").val(),
                        SoCongNgoaiGioT7CNSau23Gio: $("#SoCongNgoaiGioT7CNSau23Gio").val(),
                        SoCongNgoaiGioLe: $("#SoCongNgoaiGioLe").val(),
                        SoCongNgoaiGioLeSau23Gio: $("#SoCongNgoaiGioLeSau23Gio").val(),
                        GioTuGio: $("#GioTuGio").val(),
                        PhutTuGio: $("#PhutTuGio").val(),
                        GioDenGio: $("#GioDenGio").val(),
                        PhutDenGio: $("#PhutDenGio").val()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {

                    }
                });
                $("#jqxgrid").jqxGrid('updatebounddata');
                $("#popupWindow").jqxWindow('hide');
                return;
            },
            save: function () {
                var self = this;
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
                var selectedRecords = new Array();
                var rows = self.datagrid.jqxGrid('getrows');
                $(self.returnData).each(function (index, value) {
                    $(rows).each(function (index1, value1) {
                        value1.SoCongNgoaiGio = value1.SoCongNgoaiGio == 0 ? null : value1.SoCongNgoaiGio;
                        value1.SoCongNgoaiGioSau23Gio = value1.SoCongNgoaiGioSau23Gio == 0 ? null : value1.SoCongNgoaiGioSau23Gio;
                        value1.SoCongNgoaiGioT7CN = value1.SoCongNgoaiGioT7CN == 0 ? null : value1.SoCongNgoaiGioT7CN;
                        value1.SoCongNgoaiGioT7CNSau23Gio = value1.SoCongNgoaiGioT7CNSau23Gio == 0 ? null : value1.SoCongNgoaiGioT7CNSau23Gio;
                        value1.SoCongNgoaiGioLe = value1.SoCongNgoaiGioLe == 0 ? null : value1.SoCongNgoaiGioLe;
                        value1.SoCongNgoaiGioLeSau23Gio = value1.SoCongNgoaiGioLeSau23Gio == 0 ? null : value1.SoCongNgoaiGioLeSau23Gio;
                        if (
                            (value.SoCongNgoaiGio != value1.SoCongNgoaiGio && value.Oid == value1.Oid)
                            || (value.SoCongNgoaiGioSau23Gio != value1.SoCongNgoaiGioSau23Gio && value.Oid == value1.Oid)
                            || (value.SoCongNgoaiGioT7CN != value1.SoCongNgoaiGioT7CN && value.Oid == value1.Oid)
                            || (value.SoCongNgoaiGioT7CNSau23Gio != value1.SoCongNgoaiGioT7CNSau23Gio && value.Oid == value1.Oid)
                            || (value.SoCongNgoaiGioLe != value1.SoCongNgoaiGioLe && value.Oid == value1.Oid)
                            || (value.SoCongNgoaiGioLeSau23Gio != value1.SoCongNgoaiGioLeSau23Gio && value.Oid == value1.Oid)
                            )
                            selectedRecords.push({
                                Oid: value1.Oid,
                                SoCongNgoaiGio: value1.SoCongNgoaiGio == 0 ? null : value1.SoCongNgoaiGio,
                                SoCongNgoaiGioSau23Gio: value1.SoCongNgoaiGioSau23Gio == 0 ? null : value1.SoCongNgoaiGioSau23Gio,
                                SoCongNgoaiGioT7CN: value1.SoCongNgoaiGioT7CN == 0 ? null : value1.SoCongNgoaiGioT7CN,
                                SoCongNgoaiGioT7CNSau23Gio: value1.SoCongNgoaiGioT7CNSau23Gio == 0 ? null : value1.SoCongNgoaiGioT7CNSau23Gio,
                                SoCongNgoaiGioLe: value1.SoCongNgoaiGioLe == 0 ? null : value1.SoCongNgoaiGioLe,
                                SoCongNgoaiGioLeSau23Gio: value1.SoCongNgoaiGioLeSau23Gio == 0 ? null : value1.SoCongNgoaiGioLeSau23Gio,
                            });
                    });
                });
                if (selectedRecords.length == 0) {
                    alert("Chưa có dữ liệu nào thay đổi !!");
                    return;
                }
                if (selectedRecords.length > 0) {
                    //if (self.checkChot()) {
                    if (self.checkChotByKyTinhLuong()) {
                        alert('Tháng này đã chốt chấm công rồi !!');
                        return;
                    }
                    else {
                        $.ajax({
                            type: 'POST',
                            url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_Save',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                userList: selectedRecords
                            }),
                            success: function (result) {
                                alert("Lưu thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');

                            }
                        });
                    }
                }

            },
            remove: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                var r = confirm("Bạn có chắc xóa hay không ?");
                if (r == true) {
                    var selectedRecords = new Array();
                    for (var i = 0, l = rows.length; i < l ; i++) {
                        var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                        selectedRecords.push({
                            Oid: row.Oid
                        });
                    }
                    if (selectedRecords.length > 0) {

                        $.ajax({
                            type: 'POST',
                            url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_DeleteList',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                obj: selectedRecords
                            }),
                            success: function (result) {
                                if (result.d == true)
                                    alert("Xóa thành công !!");
                                else alert("Có lỗi xảy ra !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });

                    }
                } else {
                    return;
                }
            },
            Edit: function () {
                var self = this;
                var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[0]);
                if (getselectedrowindexes.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (getselectedrowindexes.length > 1) {
                    alert("Chỉ được chọn 1 dòng !!");
                    return;
                }
                $("#Cancel1").jqxButton({ theme: "darkBlue" });
                $("#Save1").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindowCCNGEdit").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_GetByID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ id: selectedrow.Oid }),
                    async: false,
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        $("#GioTuGio1").val(obj.GioTuGio);
                        $("#PhutTuGio1").val(obj.PhutTuGio);
                        $("#GioDenGio1").val(obj.GioDenGio);
                        $("#PhutDenGio1").val(obj.PhutDenGio);
                        $("#SoCongNgoaiGio1").val(obj.SoCongNgoaiGio);
                        $("#SoCongNgoaiGioSau23Gio1").val(obj.SoCongNgoaiGioSau23Gio);
                        $("#SoCongNgoaiGioT7CN1").val(obj.SoCongNgoaiGioT7CN);
                        $("#SoCongNgoaiGioT7CNSau23Gio1").val(obj.SoCongNgoaiGioT7CNSau23Gio);
                        $("#SoCongNgoaiGioLe1").val(obj.SoCongNgoaiGioLe);
                        $("#SoCongNgoaiGioLeSau23Gio1").val(obj.SoCongNgoaiGioLeSau23Gio);
                        $("#CCNGDonVi1").val(obj.TenBoPhan);
                        $("#CCNGNhanVien1").val(obj.TenNhanVien);
                        $("#CCNGNgay1").val(formatDate(new Date(obj.Ngay)));
                    }
                });
                $("#popupWindowCCNGEdit").jqxWindow('open');
                ko.applyBindings(self, $("#popupWindowCCNGEdit")[0]);
            },
            Update: function () {
                var self = this;
                var getselectedrow_edit = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrow_edit[0]);

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgay_Save2',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        Oid: selectedrow.Oid,
                        SoCongNgoaiGio: $("#SoCongNgoaiGio1").val(),
                        SoCongNgoaiGioSau23Gio: $("#SoCongNgoaiGioSau23Gio1").val(),
                        SoCongNgoaiGioT7CN: $("#SoCongNgoaiGioT7CN1").val(),
                        SoCongNgoaiGioT7CNSau23Gio: $("#SoCongNgoaiGioT7CNSau23Gio1").val(),
                        SoCongNgoaiGioLe: $("#SoCongNgoaiGioLe1").val(),
                        SoCongNgoaiGioLeSau23Gio: $("#SoCongNgoaiGioLeSau23Gio1").val(),
                        GioTuGio: $("#GioTuGio1").val(),
                        PhutTuGio: $("#PhutTuGio1").val(),
                        GioDenGio: $("#GioDenGio1").val(),
                        PhutDenGio: $("#PhutDenGio1").val(),

                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Lưu thành công !!");
                        $("#jqxgrid").jqxGrid('updatebounddata');
                        $("#popupWindowCCNGEdit").jqxWindow('hide');
                    }
                });
            },
            chot: function () {
                var self = this;
                if (self.KyTinhLuongSelected() == undefined) {
                    alert('Chưa chọn kỳ tính lương!');
                    return;
                }
                if (self.checkChotByKyTinhLuong()) {
                    alert('Đã chốt chấm công rồi!!');
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongNgoaiGio_Create',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        boPhanId: self.departmentSelected() == undefined ? null : self.departmentSelected(),
                        kyTinhLuong: self.KyTinhLuongSelected()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Chốt chấm công thành công !!");
                        location.reload();
                    }
                });
            },
            huychot: function () {
                var self = this;
                if (self.KyTinhLuongSelected() == undefined) {
                    alert('Chưa chọn kỳ tính lương!');
                    return;
                }
                if (self.departmentSelected() == undefined) {
                    alert('Chưa chọn đơn vị!');
                    return;
                }
                if (!self.checkChotByKyTinhLuong()) {
                    alert('Chưa có dữ liệu chốt !!');
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongNgoaiGio_Delete',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        boPhanId: self.departmentSelected(),
                        kyTinhLuong: self.KyTinhLuongSelected()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Hủy chốt chấm công thành công !!");
                        location.reload();
                    }
                });
            },
            detail: function () {
                var self = this;
                if (self.KyTinhLuongSelected() == undefined) {
                    alert('Chưa chọn kỳ tính lương!');
                    return;
                }
                //if (self.departmentSelected() == undefined) {
                //    alert('Chưa chọn đơn vị!');
                //    return;
                //}
                var url = "Detail.aspx?PhongBan=" + self.departmentSelected() + "&KyTinhLuong=" + self.KyTinhLuongSelected();
                var Width = 1000, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

            }
        };
        $(function () {
            $("#popupWindow").jqxWindow({
                width: 500, theme: "darkBlue", height: 560, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
            });
            $("#popupWindow").jqxWindow('hide');
            $("#popupWindowCCNGEdit").jqxWindow({
                width: 500, theme: "darkBlue", height: 520, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel1")
            });
            $("#popupWindowCCNGEdit").jqxWindow('hide');
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#quanlychamcong")[0]);
        });
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div id="quanlychamcong">
        <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                        <%--Nếu là thư ký--%>
                        <!-- ko if: WebGroup()==3 -->
                    <%--<div class="row">                    
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: New">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Thêm mới
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                            </a>
                        </div>                                                                                     
                    </div>--%>
                  <!-- /ko --> 
                <!-- ko if: WebGroup()==1 -->
                    <div class="row" style="text-align:center">   
                        <div class="col-md-12">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: New">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Thêm mới
                            </a>
                            <%--<a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                            </a>--%>
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: Edit">
                                <i class="btn-label fa fa-pencil"></i>Chỉnh sửa
                            </a>
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
                            </a>
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: detail">
                                <i class="btn-label glyphicon glyphicon-th-list"></i>Chi tiết
                            </a>
                        </div>        
                    </div>
                  <!-- /ko --> 
                </div>
            </div>
        </div>
     
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <%--<input type="text" placeholder="ngày" data-bind="value: day" style="width: 50px;height:32px; text-align: center;" maxlength="2" />
            -
            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px; height:32px;text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px; height:32px;text-align: center" maxlength="4" />--%>
            <select data-bind="options: KyTinhLuong, optionsText: 'Name', optionsValue: 'Oid', value: KyTinhLuongSelected, optionsCaption: '-- Chọn kỳ tính lương --'""></select>
            <select style="width: 200px" data-bind="options: department, optionsText: function (type) { return type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả'"></select>        
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
        </div>
         <!-- ko if: WebGroup()!=3 -->
           <div style="margin: 10px 0px 10px 0px; text-align: center">        
             <%--<select data-bind="options: KyTinhLuong, optionsText: 'Name', optionsValue: 'Oid', value: KyTinhLuongSelected, optionsCaption: '-- Chọn kỳ tính lương --'""></select>--%>
            <input type="button" value="Chốt chấm công" data-bind="click: chot" style="height:32px;" />
               <input type="button" value="Hủy chốt chấm công" data-bind="click: huychot" style="height:32px;" />
        </div>
        <!-- /ko -->       
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid">
                <script type="text/javascript">
                    function detail(Oid) {
                        var url = "Compare.aspx?Oid=" + Oid;
                        var Width = 430, Height = 400;
                        var OffsetHeight = document.body.offsetHeight;
                        var OffsettWidth = document.body.offsetWidth;
                        var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=0,scrollbars=yes,location=0");
                        objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
                    }
            </script>
            </div>
        </div>
    </div>
    <div id="popupWindow">
   <div>Chấm công ngày nghỉ</div>
   <div style="overflow: hidden;">
      <table>
                   <tr>
            <td style="text-align:right; height:40px">Kỳ tính lương:</td>
            <td>                         
               <select data-bind="options: BangLuong, optionsText: 'Name', optionsValue: 'Oid', value: BangLuongSelected"></select>
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Đơn vị:</td>
            <td>                         
               <select style="width: 250px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected_nhansu"></select>
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Nhân viên:</td>
            <td>
               <div id="jqxdropdownbutton">
                  <div style="border-color: transparent;" id="jqxgrid_hosonhanvien">
                  </div>
               </div>
            </td>
         </tr>
          <tr>
            <td style="text-align:right; height:40px">Ngày:</td>
            <td>
               <div id='jqxDate'></div>
            </td>
         </tr>
        <tr>
            <td style="text-align:right; height:40px">Từ giờ:</td>
            <td>
                <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioTuGio" value="00"  />
                <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutTuGio"  value="00" />
            </td>
        </tr>
        <tr>
            <td style="text-align:right; height:40px">Đến giờ:</td>
            <td>
                <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioDenGio" value="00"  />
                <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutDenGio"  value="00" />
            </td>
        </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày thường:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGio" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày thường sau 23h:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioSau23Gio" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ T7/CN:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioT7CN" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ T7/CN sau 23h:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioT7CNSau23Gio" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày lễ:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioLe" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày lễ sau 23h:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioLeSau23Gio" />
            </td>
         </tr>
         <tr>
            <td></td>
            <td style="padding-top: 10px;text-align:right">
               <input style="margin-right: 5px;" type="button" id="Save" value="Save" data-bind="click: Create" />
               <input id="Cancel" type="button" value="Cancel" />
            </td>
         </tr>
      </table>
   </div>
</div>
<div id="popupWindowCCNGEdit">
   <div>Chấm công ngày nghỉ</div>
   <div style="overflow: hidden;">
      <table>
         <tr>
            <td style="text-align:right; height:40px">Đơn vị:</td>
            <td>                         
               <input type="text" disabled="disabled" id="CCNGDonVi1" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Nhân viên:</td>
            <td>
               <input type="text" disabled="disabled" id="CCNGNhanVien1" />
            </td>
         </tr>
          <tr>
            <td style="text-align:right; height:40px">Ngày:</td>
            <td>
               <input type="text" disabled="disabled" id="CCNGNgay1" />
            </td>
         </tr>
        <tr>
            <td style="text-align:right; height:40px">Từ giờ:</td>
            <td>
                <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioTuGio1" value="00"  />
                <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutTuGio1"  value="00" />
            </td>
        </tr>
        <tr>
            <td style="text-align:right; height:40px">Đến giờ:</td>
            <td>
                <input type="number" maxlength="2" min="1" max="24" style="height: 20px; width: 40px; text-align: center" id="GioDenGio1" value="00"  />
                <input type="number" maxlength="2" min="0" max="59" style="height: 20px; width: 40px; text-align: center" id="PhutDenGio1"  value="00" />
            </td>
        </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày thường:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGio1" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày thường sau 23h:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioSau23Gio1" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ T7/CN:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioT7CN1" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ T7/CN sau 23h:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioT7CNSau23Gio1" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày lễ:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioLe1" />
            </td>
         </tr>
         <tr>
            <td style="text-align:right; height:40px">Số giờ ngày lễ sau 23h:</td>
            <td>
               <input style="height:32px;width:60px;padding-left:5px" type="number" id="SoCongNgoaiGioLeSau23Gio1" />
            </td>
         </tr>
         <tr>
            <td></td>
            <td style="padding-top: 10px;text-align:right">
               <input style="margin-right: 5px;" type="button" id="Save1" value="Save" data-bind="click: Update" />
               <input id="Cancel1" type="button" value="Cancel" />
            </td>
         </tr>
      </table>
   </div>
</div>
</asp:content>
