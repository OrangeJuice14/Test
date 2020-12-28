<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="GiangVienThinhGiang.aspx.cs" Inherits="HRMChamCong.Views.QuanLyThinhGiang.GiangVienThinhGiang" %>

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
    function ViewModel(datagrid) {
        var self = this;
        self.returnData = [];
        self.datagrid = datagrid;
        self.department = ko.observableArray();
        self.departmentSelected = ko.observable(null);
        self.name = ko.observable("");

        //Lấy danh sách đơn vị
       <%-- $.ajax({
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
        });--%>
        //Lấy danh sách giảng viên
        self.source =
        {
            datatype: "json",
            datafields: [
                { name: 'Oid', type: 'string' },
                { name: 'MaQuanLy', type: 'string' },
                { name: 'Ho', type: 'string' },
                { name: 'Ten', type: 'string' },
                { name: 'TenBoPhan', type: 'string' },
                { name: 'NgaySinh', type: 'date' },
                { name: 'GioiTinh', type: 'string' },
                { name: 'CMND', type: 'string' }
            ],
            id: 'Id',
            sortcolumn: 'Ten',
            sortdirection: 'asc',
            url: "/Services/ChamCongService.asmx/GiangVienThinhGiang_Find",
            formatdata: function (data) {
                return {
                    boPhanId: self.departmentSelected() == undefined ? null : "'" + self.departmentSelected() + "'",
                    maNhanSu: "'" + self.name() + "'",
                    webUserId: "'" + '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>' + "'"
                    //idLoaiNhanSu: self.loaiNhanSuSelected() == undefined ? null : "'" + self.loaiNhanSuSelected() + "'",
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
                        text: 'Mã nhân sự', datafield: 'MaQuanLy', width: 100, align: 'center', cellsalign: "middle"
                    },
                    {
                        text: 'Họ', datafield: 'Ho', width: 150, align: 'center'
                    },
                    {
                        text: 'Tên', datafield: 'Ten', width: 100, align: 'center'
                    },
                    {
                        text: 'Bộ môn', datafield: 'TenBoPhan', width: 300, align: 'center'
                    },
                    {
                        text: 'Ngày sinh', datafield: 'NgaySinh', width: 100, align: 'center', cellsformat: 'dd/MM/yyyy', ellsalign: "middle"
                    },
                    {
                        text: 'Giới tính', datafield: 'GioiTinh', align: 'center', width: 100,
                    },
                    {
                        text: 'CMND', datafield: 'CMND', align: 'center', width: 120,
                    }
                ]
            });
    }
    ViewModel.prototype = {
        search: function () {
            var self = this;
            self.datagrid.jqxGrid('updatebounddata');
        },
        Edit: function () {
            var self = this;
            //
            var getselectedrowindexes = $('#jqxgrid').jqxGrid('getselectedrowindexes');
            var selectedrow = $('#jqxgrid').jqxGrid('getrowdata', getselectedrowindexes[0]);
            if (getselectedrowindexes.length == 0) {
                alert("Chưa có dòng nào được chọn !!");
                return;
            }
            //
            window.location = "SuaGiangVienThinhGiang.aspx?id=" + selectedrow.Oid;
            //
        },
        Create: function () {
            var self = this;
            //
            window.location = "ThemGiangVienThinhGiang.aspx";
            //
        },
        Delete: function () {
            var self = this;
            var rows = self.datagrid.jqxGrid('selectedrowindexes');
            if (rows.length == 0) {
                alert("Chưa có dòng nào được chọn !!");
                return;
            }
            //
            var r = confirm("Bạn có chắc xóa hay không ?");
            if (r == true) {
                //
                var selectedRecords = new Array();
                for (var i = 0, l = rows.length; i < l ; i++) {
                    var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                    selectedRecords.push({
                        Oid: row.Oid
                    });
                }

                //Tiến hành xóa dữ liệu    
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/DeleteGiangVienThinhGiang',
                    async: false,
                    data: ko.toJSON({
                        list: selectedRecords
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        //
                        var obj = result;
                        //
                        if (obj.d == 'success') {
                            alert('Xóa liệu thành công.');
                            self.datagrid.jqxGrid('updatebounddata');
                            self.datagrid.jqxGrid('clearselection');
                        }
                        else {
                            alert('Xóa dữ liệu thất bại.');
                        }
                    }
                });
            }
        },
    };
    $(function () {
        var model = new ViewModel($("#jqxgrid"));
        ko.applyBindings(model, $("#giangvienthinhgiang")[0]);
        //
    });
</script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
<div id="giangvienthinhgiang">
     <div class="row">
        <div class="col-lg-12 col-xs-12 col-sm-12">
            <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                <div class="row">
                    <div class="col-lg-2 col-xs-12 col-sm-6">
                        <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: Create">
                            <i class="btn-label glyphicon glyphicon-ok"></i>Thêm
                        </a>
                    </div>
                     <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: Delete">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
                            </a>
                     </div>
                    <div class="col-lg-2 col-xs-12 col-sm-6" id="sua">
                        <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: Edit">
                            <i class="btn-label glyphicon glyphicon-th-list"></i>Sửa
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
     <div style="margin: 10px 0px 10px 0px; text-align: center">
            <select hidden style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected"></select>
            <input type="text" placeholder="Mã nhân sự"  data-bind="value: name" style="width: 110px;padding-left:5px; height:32px;" />
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
     </div>
    <div style="padding: 0px 0px 0px 0px;">
    <div id="jqxgrid"></div>
    </div>
</div>
</asp:content>

