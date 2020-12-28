<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QuanLyNghiPhep.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.QuanLyNghiPhep" %>

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
            self.name = ko.observable("");
            self.isEdit = ko.observable(false);
            self.checkExits = function () {
                var check;

                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyNghiPhep_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        nam: self.year()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
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
                    { name: 'MaQuanLy', type: 'string' },
                    { name: 'HoTen', type: 'string' },
                    { name: 'TenPhongBan', type: 'string' },
                    { name: 'TongSoNgayPhep', type: 'string' },
                    { name: 'SoNgayPhepDaNghi', type: 'string' },
                    { name: 'SoNgayPhepConLai', type: 'string' },
                    { name: 'SoNgayPhepCongThem', type: 'string' }
                ],
                id: 'Id',
                url: "/Services/ChamCongService.asmx/QuanLyNghiPhep_Find",
                formatdata: function (data) {
                    return {
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
                                return "<div style='text-align:center;margin-top:15px;'>" + (value + 1) + "</div>";
                            }
                        },
                        {
                            text: 'Mã nhân sự', datafield: 'MaQuanLy', width: 90, align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Họ tên', datafield: 'HoTen', width: 200, align: 'center'
                        },
                        {
                            text: 'Đơn vị', datafield: 'TenPhongBan', align: 'center'
                        },
                        {
                            text: 'Tổng phép', datafield: 'TongSoNgayPhep', width: 90, align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Đã nghỉ', datafield: 'SoNgayPhepDaNghi', width: 90, align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Còn lại', datafield: 'SoNgayPhepConLai', width: 90, align: 'center', cellsalign: "middle"
                        },
                        {
                            text: 'Cộng thêm', datafield: 'SoNgayPhepCongThem', width: 90, align: 'center', cellsalign: "middle"
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
            save: function () {
                var self = this;
                if (self.checkExits(self.year())) {
                    alert('Đã tạo quản lý nghỉ phép !!');
                    return;
                }
                else
                {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/QuanLyNghiPhep_Create',
                        contentType: "application/json; charset=utf-8",
                        data: ko.toJSON({
                            nam: self.year()
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
            update: function () {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyNghiPhep_Update',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        boPhanId: self.departmentSelected() == undefined ? null : self.departmentSelected(),
                        nam: self.year()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Thành công!");
                        self.datagrid.jqxGrid('updatebounddata');
                    }
                });
            },
            remove: function () {
                var self = this;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyNghiPhep_Delete',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        nam: self.year()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        alert("Thành công!");
                        self.datagrid.jqxGrid('updatebounddata');
                    }
                });
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
                    <div class="row" style="text-align:center">
                        <div class="col-md-12">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                                <i class="btn-label glyphicon glyphicon-tags"></i>Tạo mới
                            </a>
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: update">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Cập nhật
                            </a>
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
                            </a>
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: search">
                                <i class="btn-label glyphicon glyphicon-pencil"></i>Tìm kiếm
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px; height: 32px; text-align: center" maxlength="4" />
            <select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả đơn vị'"></select>
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:Content>
