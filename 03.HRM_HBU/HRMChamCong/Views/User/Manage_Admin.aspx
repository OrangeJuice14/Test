<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage_Admin.aspx.cs" Inherits="HRMChamCong.Views.User.Manage_Admin" %>
<%@ Import Namespace="HRMChamCong.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .formGroup {
            padding: 10px 0px 0px 0px;
            margin: 0 auto;
        }

            .formGroup label {
                float: left;
                width: 200px;
            }

            .formGroup span {
                padding: 0px 10px;
            }

        .container {
            border: solid 1px #7F9DB9;
            width: 400px;
            height: 300px;
            overflow-y: scroll;
        }

        .form_checkbox {
            padding: 0 5px;
        }

        h3 {
            color: #3B6097;
        }

        .formEvent {
            float: right;
        }

            .formEvent a {
                color: #3B6097;
                width: 50px;
                float: left;
            }

        .validate {
            color: red;
        }

        table tr td {
            padding-bottom: 8px;
        }
    </style>
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
        function ViewModel_Detail() {
            var self = this;
            self.userName = ko.observable();
            self.passWord = ko.observable();
            self.fullName = ko.observable();
            self.email = ko.observable();
            self.accountList = ko.observableArray();
            self.accountSelected = ko.observable();
            self.status = ko.observable('true');
            self.departments = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/GetList_BoPhanWebGroup_GetList',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.departments(obj);
                    self.departments.unshift({ STT: 0, Oid: null, TenBoPhan: 'Tất cả' });
                }
            });

            ko.utils.arrayForEach(self.departments(), function (val) {
                val.Chon = ko.observable(val.Chon);
            });
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/WebGroup_GetList',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    self.accountList(obj);
                    var id = "53D57298-1933-4E4B-B4C8-98AFED036E21";
                    id = id.toLowerCase();
                    self.accountList.remove(function (obj) { return obj.ID == id });
                }
            });
            self.allSelected = ko.observable();
            self.grid_hosonhanvien = ko.observableArray();
            self.departmentSelected_nhansu = ko.observable(null);
            self.departmentSelected_nhansu.subscribe(function (newValue) {
                if (newValue === undefined) {
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/HoSoNhanVienBy_MaBoPhan',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ maBoPhan: newValue }),
                    async: false,
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        obj.push({ Oid: null, MaQuanLy: null, Ho: '', Ten:'' });
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
                var rows = $('#jqxgrid_hosonhanvien').jqxGrid('getrows');
                $("#jqxgrid_hosonhanvien").jqxGrid('selectrow',rows.length-1);
            });
        }
        function ViewModel(datagrid) {
            var self = this;
            self.datagrid = datagrid;
            self.items = ko.observableArray();

            self.source =
            {
                id: 'Id',
                datafields: [
                   { name: 'Oid', type: 'string' },
                   { name: 'MaNhanSu', type: 'string' },
                   { name: 'UserName', type: 'string' },
                   { name: 'HoVaTen', type: 'string' },
                   { name: 'Email', type: 'string' },
                   { name: 'HoatDong', type: 'bool' },
                   { name: 'TenBoPhan', type: 'string' },
                   { name: 'LoaiTaiKhoan', type: 'string' }
                ],
                datatype: "json",
                // localdata: self.items()
                url: "/Services/ChamCongService.asmx/GetList_QuanLyUserQuanTri",
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
                pagesize: 20,
                sortable: true,
                filterable: true,
                autorowheight: true,
                autoheight: true,
                theme: "darkBlue",
                columns: [
                    {
                        text: 'STT', columntype: 'number', width: 35,  editable: false,
                        cellsrenderer: function (row, column, value) {

                            return "<div style='text-align:center; margin-top:20px;'>" + (value + 1) + "</div>";
                        }
                    },
                    { text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 100, align: 'center' },
                    {
                        text: 'Tên user', datafield: 'UserName', width: 190, align: 'center',
                        cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                            var item = self.datagrid.jqxGrid('getrowdata', row);
                            return "<div style='margin-top:20px;padding-left:5px;'><a href='/Views/User/Detail.aspx?Id=" + item.Oid + "' style=''>" + cellvalue + "</a></div>";
                        }
                    },
                    { text: 'Họ tên', datafield: 'HoVaTen', width: 150, align: 'center' },
                    { text: 'Tên bộ phận', datafield: 'TenBoPhan', width: 150, align: 'center' },
                    { text: 'Loại tài khoản', datafield: 'LoaiTaiKhoan', width: 100, align: 'center' },
                    { text: 'Email', datafield: 'Email', width: 150, align: 'center' },
                    {
                        text: 'Trạng thái', datafield: 'HoatDong', columntype: 'checkbox', align: 'center', cellsalign: "middle"
                    }
                ]
            });
        }
        ViewModel_Detail.prototype = {
            refreshObj: function () {
                var self = this;
                self.userName("");
                self.passWord("");
                self.accountSelected("");
                self.status('true');
                self.departmentSelected_nhansu(undefined);
                self.allSelected(false);
            },
            selectAll: function () {
                var self = this;
                ko.utils.arrayForEach(self.departments(), function (val) {
                    val.Chon(!self.allSelected());
                });
                return true;
            },
            validate: function () {
                var self = this;
                return $.trim(self.userName()).length != 0 && $.trim(self.passWord()).length != 0;
            },
            validateUsernameExist: function (webUserId, userName) {
                var temp;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/WebUsers_KiemTraTrungUsername',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ webUserId: webUserId, userName: userName }),
                    async: false,
                    success: function (result) {
                        temp = $.parseJSON(result.d);
                    }
                });
                return temp;
            },

            save: function () {
                var self = this;
                if (!self.validate())
                    return;
                if (self.validateUsernameExist(null, self.userName())) {
                    alert("Tên đăng nhập đã tồn tại");
                    $("#txtUserName").focus();
                    return;
                }
                self.departments.remove(function (item) { return item.Oid == null });
                var getselectedrowindexes = $('#jqxgrid_hosonhanvien').jqxGrid('getselectedrowindexes');
                if (getselectedrowindexes != undefined)
                    var selectedrow_hsnhanvien = $('#jqxgrid_hosonhanvien').jqxGrid('getrowdata', getselectedrowindexes[0]);
                ko.utils.arrayForEach(self.departments(), function (val) {
                    val.Chon = val.Chon();
                });

                var obj =
                {
                    ThongTinNhanVien: self.departmentSelected_nhansu() == undefined ? null : selectedrow_hsnhanvien.Oid,
                    UserName: self.userName(),
                    Password: self.passWord(),
                    HoatDong: self.status(),
                    DanhSachDTO_BoPhan: self.departments(),
                    WebGroupID: self.accountSelected() == undefined ? null : self.accountSelected()
                };
                $.ajax({
                    data: JSON.stringify({ obj: obj }),
                    contentType: "application/json; charset=utf-8",
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/Save_QuanLyUser',
                    async: false,
                    dataType: "json",
                    success: function (result) {
                        alert("Lưu thành công !!");
                        $("#jqxgrid").jqxGrid('updatebounddata');
                        $("#popupWindow").jqxWindow('hide');

                    }
                });
            }

        };
        ViewModel.prototype = {
            createNew: function () {
                $.get('/Views/User/AddNewAdmin.aspx', function (data) {
                    $('#popupWindow').html(data);
                    ko.cleanNode($('#popupWindow')[0]);
                    ko.applyBindings(new ViewModel_Detail(), document.getElementById('popupWindow'));
                    $("#popupWindow").jqxWindow({
                        width: 550, theme: "darkBlue", height: 600, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
                    });
                    $("#Cancel").jqxButton({ theme: "darkBlue" });
                    $("#Save").jqxButton({ theme: "darkBlue" });
                    var offset = $("#jqxgrid").offset();
                    $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                    $("#popupWindow").jqxWindow('open');
                });

            },
            xoa: function () {
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
                            url: '/Services/ChamCongService.asmx/WebUsers_XoaUsers',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                userList: selectedRecords,
                                khoa: true
                            }),
                            success: function (result) {
                                alert("Xóa thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });
                    }
                } else {
                    return;
                }
            }
        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#userManage")[0]);
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                    <div class="row">
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: createNew">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Tạo mới
                            </a>
                        </div>
                      <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-danger" style="width: 158px;" data-bind="click: xoa">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
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
    <div style="padding: 10px 0px 0px 0px;">
        <div id="jqxgrid"></div>
    </div>
    <div id="popupWindow"></div>
</asp:Content>
