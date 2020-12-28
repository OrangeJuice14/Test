<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ChamCongNgayNghi_Manage.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.ChamCongNgayNghi_Manage" %>

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
            self.departmentSelected_nhansu = ko.observable(null);
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
            self.name = ko.observable("");
            self.HinhThucNghiList = ko.observableArray();
            self.hinhThucNghiSelected = ko.observable();
            self.isEdit = ko.observable(false);
            self.grid_hosonhanvien = ko.observableArray();
            self.departmentSelected_nhansu.subscribe(function (newValue) {
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/HoSoNhanVienBy_MaBoPhan',
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
                }
            });
            self.source =
            {
                datatype: "json",
                datafields: [
                    { name: 'Oid', type: 'string' },
                    { name: 'MaNhanSu', type: 'string' },
                    { name: 'HoTen', type: 'string' },
                    { name: 'TenPhongBan', type: 'string' },
                    { name: 'HinhThucNghi_Name', type: 'string' },
                    { name: 'TuNgay', type: 'date', format: 'dd-MM-yyyy' },
                    { name: 'DenNgay', type: 'date' },
                    { name: 'DienGiai', type: 'string' },
                    { name: 'TrangThai', type: 'int' }
                ],
                id: 'Id',
                // sortcolumn: 'HoTen',
                //sortdirection: 'asc',
                url: "/Services/ChamCongService.asmx/ChamCongNgayNghi_Find",
                formatdata: function (data) {
                    return {
                        ngay: self.day(),
                        thang: self.month(),
                        nam: self.year(),
                        boPhanId: self.departmentSelected() == undefined ? null : "'" + self.departmentSelected() + "'",
                        maNhanSu: self.name(),
                        webUserId: "'" + '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>' + "'",
                        idLoaiNhanSu: self.loaiNhanSuSelected() == undefined ? null : "'" + self.loaiNhanSuSelected() + "'",
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
                    selectionmode: 'checkbox',
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
                                return "<div style='text-align:center;margin-top:15px;'>" + (value + 1) + "</div>";
                            }
                        },
                        {
                            text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 100, align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Họ tên', datafield: 'HoTen', width: 200, align: 'center'
                        },
                        {
                            text: 'Từ ngày', datafield: 'TuNgay', width: 80, cellsformat: 'd/M/yyyy', align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Đến ngày', datafield: 'DenNgay', width: 80, cellsformat: 'd/M/yyyy', align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Phòng ban', datafield: 'TenPhongBan', width: 200, align: 'center'
                        },
                        {
                            text: 'Hình thức nghỉ', datafield: 'HinhThucNghi_Name', width: 150, cellsalign: "middle", sortable: false, align: 'center',
                        },
                        {
                            text: 'Diễn giải', datafield: 'DienGiai', align: 'center', width: 200,
                        },
                        {
                            text: 'Trạng thái', datafield: 'TrangThai', align: 'center', cellsalign: "middle", width: 80,
                            cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                                var str = "";
                                if (cellvalue == -1) {
                                    str = "<img src='/Images/InfoSmall.jpg' style='padding:10px 0px 0px 30px;' />";
                                }
                                if (cellvalue == 1) {
                                    str = "<img src='/Images/TT_yes.png' style='padding:10px 0px 0px 30px;'/>";
                                }
                                if (cellvalue == 0) {
                                    str = "<img src='/Images/TT_no.png' style='padding:10px 0px 0px 30px;'/>";
                                }
                                return str;
                            }
                        }

                    ]
                });
        }
        ViewModel.prototype = {
            daysInMonth: function (month, year) {
                return new Date(year, month, 0).getDate();
            },
            validateFromDateToDate: function (chamCongNgayNghiOid, tuNgay, denNgay, nhanVienID) {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: ko.toJSON({
                        chamCongNgayNghiOid: chamCongNgayNghiOid,
                        tuNgay: tuNgay,
                        denNgay: denNgay,
                        nhanVienID: nhanVienID
                    }),
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        check = obj;
                    }
                });
                return check;
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
                self.isEdit(false);
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
                $("#Cancel").jqxButton({ theme: "darkBlue" });
                $("#Save").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $("#jqxFromDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#jqxToDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#popupWindow").jqxWindow('open');
                ko.applyBindings(self, $("#popupWindow")[0]);
            },
            Edit: function () {
                var self = this;
                self.isEdit(true);
                var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[0]);
                if (getselectedrowindexes.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
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


                $("#popupWindow").on('open', function () {
                    $("#txtContent").jqxInput('selectAll');
                });
                $("#Cancel").jqxButton({ theme: "darkBlue" });
                $("#Save").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $("#jqxFromDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#jqxToDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#popupWindow").jqxWindow('open');
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_GetByID',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ id: selectedrow.Oid }),
                    async: false,
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        self.departmentSelected_nhansu(obj.IDBoPhan);
                        self.hinhThucNghiSelected(obj.IDHinhThucNghi);
                        $("#txtContent").val(obj.DienGiai);
                        $('#jqxFromDate ').jqxDateTimeInput('setDate', obj.TuNgay);
                        $('#jqxToDate ').jqxDateTimeInput('setDate', obj.DenNgay);
                    }
                });
                ko.applyBindings(self, $("#popupWindow")[0]);
            },
            CreateObj: function () {
                var self = this;
                var getselectedrow_edit = $('#jqxgrid').jqxGrid('getselectedrowindexes');
                var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrow_edit[0]);
                var getselectedrowindexes = $('#jqxgrid_hosonhanvien').jqxGrid('getselectedrowindexes');
                var selectedrow_hsnhanvien = $('#jqxgrid_hosonhanvien').jqxGrid('getrowdata', getselectedrowindexes[0]);
                if (!self.validateFromDateToDate(getselectedrow_edit.length > 0 ? selectedrow.Oid : null, $('#jqxFromDate').jqxDateTimeInput('getDate'), $('#jqxToDate').jqxDateTimeInput('getDate'), selectedrow_hsnhanvien.Oid)) {
                    alert("Trùng hoặc giao ngày với dữ liệu trước");
                    return;
                }
                if (self.isEdit())
                {

                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_Save',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            obj:
                            {
                                Oid: selectedrow.Oid,
                                DienGiai: $("#txtContent").val(),
                                IDHinhThucNghi: self.hinhThucNghiSelected(),
                                TuNgay: $('#jqxFromDate').jqxDateTimeInput('getDate'),
                                DenNgay: $('#jqxToDate').jqxDateTimeInput('getDate')
                            }
                        }),
                        dataType: "json",
                        async: false,
                        success: function (result) {
                            var obj = $.parseJSON(result.d);

                        }
                    });
                    $("#jqxgrid").jqxGrid('updatebounddata');
                    $("#popupWindow").jqxWindow('hide');

                }
                else
                {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_TaoMoi',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            nhanVienId: selectedrow_hsnhanvien.Oid,
                            noiDung: $("#txtContent2").val(),
                            idHinhThucNghi: self.hinhThucNghiSelected(),
                            tuNgay: $('#jqxFromDate').jqxDateTimeInput('getDate'),
                            denNgay: $('#jqxToDate').jqxDateTimeInput('getDate'),
                            webUserId: '<%#HttpContext.Current.Session[HRMChamCong.Helper.SessionKey.UserId.ToString()]%>'

                    }),
                        dataType: "json",
                        async: false,
                        success: function (result) {

                        }
                    });
                $("#jqxgrid").jqxGrid('updatebounddata');
                $("#popupWindow").jqxWindow('hide');
                return;
            }


            },
            save: function () {
                var self = this;
                var selectedRecords = new Array();
                var rows = self.datagrid.jqxGrid('getrows');
                $(self.returnData).each(function (index, value) {
                    $(rows).each(function (index1, value1) {
                        value1.IDHinhThucNghi = value1.IDHinhThucNghi == 0 ? null : value1.IDHinhThucNghi;
                        if (value.IDHinhThucNghi != value1.IDHinhThucNghi && value.Oid == value1.Oid)
                            selectedRecords.push({
                                Oid: value1.Oid,
                                IDHinhThucNghi: value1.IDHinhThucNghi == 0 ? null : value1.IDHinhThucNghi,
                                DaChamCong: true
                            });
                    });
                });
                if (selectedRecords.length == 0) {
                    alert("Chưa có dữ liệu nào thay đổi !!");
                    return;
                }
                if (selectedRecords.length > 0) {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/QuanLyChamCong_Save',
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
                            url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_DeleteList',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                obj: selectedRecords
                            }),
                            success: function (result) {
                                alert("Xóa thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });

                    }
                } else {
                    return;
                }
            },
            accept: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                var r = confirm("Bạn có muốn duyệt hay không ?");
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
                            url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_AcceptList',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                obj: selectedRecords
                            }),
                            success: function (result) {
                                alert("Thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });

                    }
                } else {
                    return;
                }
            },
            cancel: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                var r = confirm("Bạn có muốn không chấp nhận hay không ?");
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
                            url: '/Services/ChamCongService.asmx/ChamCongNgayNghi_CancelList',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                obj: selectedRecords
                            }),
                            success: function (result) {
                                alert("Thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });

                    }
                } else {
                    return;
                }
            },

        };
    $(function () {
        $("#popupWindow").jqxWindow({
            width: 500, theme: "darkBlue", height: 400, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
        });
        $("#popupWindow").jqxWindow('hide');
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
<%--                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: New">
                                <i class="btn-label glyphicon glyphicon-tags"></i>Tạo mới
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: Edit">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Hiệu chỉnh
                            </a>
                        </div>--%>

                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
                            </a>
                        </div>
                         <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: accept">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Chấp nhận
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-yellow" style="width: 158px;" data-bind="click: cancel">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Không chấp nhận
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <div style="display: none">
                <input type="text" placeholder="ngày" data-bind="value: day" style="width: 50px; height: 32px; text-align: center;" maxlength="2" />
                -
            </div>
            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px; height: 32px; text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px; height: 32px; text-align: center" maxlength="4" />
            <select style="width: 200px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected"></select>
            <%--<select style="width: 200px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan  }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả phòng ban'"></select>--%>
            <select data-bind="options: loaiNhanSu, optionsText: 'TenLoaiNhanSu', optionsValue: 'Oid', value: loaiNhanSuSelected, optionsCaption: 'Tất cả'"></select>
            <input type="text" placeholder="Mã nhân sự" data-bind="value: name" style="width: 110px; height: 32px; padding: 5px;" />
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px; height: 32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
        <div class="row" style="padding-top:20px">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" style="margin-left: 15px;">
                    <div class="row">
                        <div class="col-lg-4 col-xs-12 col-sm-4">
                            <img src='/Images/InfoSmall.jpg' />
                            <span>Chờ xét</span>
                        </div>
                        <div class="col-lg-4 col-xs-12 col-sm-4">
                            <img src='/Images/TT_yes.png' />
                            <span>Chấp nhận</span>

                        </div>

                        <div class="col-lg-4 col-xs-12 col-sm-4">
                            <img src='/Images/TT_no.png' />
                            <span>Không chấp nhận</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="popupWindow">
            <div>Chấm công ngày nghỉ</div>
            <div style="overflow: hidden;">
                <table>
                    <tr data-bind="visible: !isEdit()">
                        <td align="right">Phòng ban:</td>
                        <td align="left">
                            <%--<select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected_nhansu, optionsCaption: 'Tất cả phòng ban'"></select></td>--%>
                            <select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected_nhansu"></select></td>
                    </tr>
                    <tr data-bind="visible: !isEdit()">
                        <td align="right">Nhân viên:</td>
                        <td align="left">
                            <div id="jqxdropdownbutton">
                                <div style="border-color: transparent;" id="jqxgrid_hosonhanvien">
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Nội dung:</td>
                        <td align="left" data-bind="visible: !isEdit()">
                            <textarea id="txtContent2" style="width: 300px; height: 180px;"></textarea>
                        </td>
                        <td align="left" data-bind="visible: isEdit()">
                            <textarea id="txtContent" style="width: 300px; height: 180px;"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Từ ngày:</td>
                        <td align="left">
                            <div id='jqxFromDate'></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Đến ngày:</td>
                        <td align="left">
                            <div id='jqxToDate'></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Hình thức nghỉ:</td>
                        <td align="left">
                            <select data-bind="options: HinhThucNghiList, optionsText: 'TenHinhThucNghi', optionsValue: 'Oid', value: hinhThucNghiSelected"></select>

                        </td>
                    </tr>
                    <tr>
                        <td align="right"></td>
                        <td style="padding-top: 10px;" align="right">
                            <input style="margin-right: 5px;" type="button" id="Save" value="Save" data-bind="click: CreateObj" />
                            <input id="Cancel" type="button" value="Cancel" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
