<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.QuanLyCongTac.Manage" %>

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
             var trangthaiArr = [
                 { Id: -1, Name: "Chờ xét" },
                 { Id: 0, Name: "Không chấp nhận" },
                 { Id: 1, Name: "Chấp nhận" }
             ];
             var self = this;
             self.returnData = [];
             self.datagrid = datagrid;
             self.month = ko.observable(new Date().getMonth() + 1);
             self.year = ko.observable(new Date().getFullYear());
             self.trangthaiList = ko.observableArray(trangthaiArr);
             self.trangthaiSelected = ko.observable(-1);
             self.bophan = ko.observableArray();
             self.bophanSelected = ko.observable();
             self.maNhanSu = ko.observable("");
             $.ajax({
                 type: 'POST',
                 url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetDepartmentsOfUser',
                 contentType: "application/json; charset=utf-8",
                 data: ko.toJSON({ userId: '<%#HttpContext.Current.Session[HRMChamCong.Helper.SessionKey.UserId.ToString()]%>' }),
                 dataType: "json",
                 async: false,
                 success: function (result) {
                     var obj = $.parseJSON(result.d);
                     self.bophan(obj);
                     self.bophanSelected(obj[0].Oid);
                 }
             });
             self.source =
             {
                 datatype: "json",
                 datafields: [
                    { name: 'Oid', type: 'string' },
                    { name: 'MaNhanSu', type: 'string' },
                    { name: 'HoTen', type: 'string' },
                    { name: 'TuNgay', type: 'date' },
                    { name: 'DenNgay', type: 'date' },
                    { name: 'Buoi', type: 'string' },
                    { name: 'NoiDung', type: 'int' },
                    { name: 'NgayTao', type: 'date' },
                    { name: 'TrangThai', type: 'int' }
                 ],
                 id: 'Id',
                 //async:false,
                 //pagesize: 10,
                 sortcolumn: 'Name',
                 sortdirection: 'asc',
                 url: "/Services/ChamCongService.asmx/QuanLyKhaiBaoCongTac_Find",
                 //sort: function (value, row) {
                 //    self.datagrid.jqxGrid('updatebounddata');
                 //},
                 formatdata: function (data) {
                     return {
                         thang: self.month(),
                         nam: self.year(),
                         bophan: self.bophanSelected() == undefined ? null : "'" + self.bophanSelected() + "'",
                         trangthai: self.trangthaiSelected() == undefined ? null : "'" + self.trangthaiSelected() + "'",
                         maNhanSu: "'" + self.maNhanSu() + "'",
                         webUserId: "'" + '<%#HttpContext.Current.Session[HRMChamCong.Helper.SessionKey.UserId.ToString()]%>' + "'"
                        //startIndex: (data.pagenum || 0) * (data.pagesize || 10),
                        //pageSize: data.pagesize,
                        //sortorder: "'" + data.sortorder + "'",
                        //sortdatafield: "'" + data.sortdatafield + "'"
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
                //editable: true,
                selectionmode: 'checkbox',
                //virtualmode: true,
                pageable: true,
                pagesize: 10,
                sortable: true,
                filterable: true,
                //rowsheight: 40,
                autoheight: true,
                autorowheight: true,
                theme: "darkBlue",
                //rendergridrows: function (args) {
                //    return args.data;
                //},
                columns: [
                  {
                      text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                          return "<div style='text-align:center;margin-top:10px;'>" + (value + 1) + "</div>";
                      }
                  },
                  {
                      text: 'Họ tên', datafield: 'HoTen', width: 150, align: 'center'
                  },
                  {
                      text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 150, align: 'center'
                  },
                  {
                      text: 'Từ ngày', datafield: 'TuNgay', width: 150, align: 'center', cellsalign: "middle", columnGroup: 'NgayCongTac', cellsformat: 'd/M/yyyy'
                  },
                  {
                      text: 'Đến ngày', datafield: 'DenNgay', width: 150, align: 'center', cellsalign: "middle", columnGroup: 'NgayCongTac', cellsformat: 'd/M/yyyy'
                  },
                    {
                        text: 'Buổi', datafield: 'Buoi', width: 100, align: 'center', cellsalign: "middle",
                    },
                  {
                      text: 'Nội dung', datafield: 'NoiDung', width: 250, align: 'center', cellsalign: "middle"
                  },
                  {
                      text: 'Ngày tạo', datafield: 'NgayTao', width: 100, align: 'center', cellsalign: "middle", cellsformat: 'd/M/yyyy'
                  },
                  {
                      text: 'Trạng thái', datafield: 'TrangThai', width: 80, align: 'center', cellsalign: "middle",
                      cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                          var str = "";
                          if (cellvalue == -1) {
                              str = "<img src='/Images/InfoSmall.jpg' style='padding:2px 0px 0px 30px;'/>";
                          }
                          if (cellvalue == 1) {
                              str = "<img src='/Images/TT_yes.png' style='padding:2px 0px 0px 30px;'/>";
                          }
                          if (cellvalue == 0) {
                              str = "<img src='/Images/TT_no.png' style='padding:2px 0px 0px 30px;'/>";
                          }
                          return str;
                      }
                  }
                ],
                columnGroups: [
                   { text: 'Ngày công tác', cellsAlign: 'center', align: 'center', name: 'NgayCongTac' }
                ]
            });
        }
        ViewModel.prototype = {
            validate: function () {
                var self = this;
                if (isNaN(self.month()) || self.month() < 0 || self.month() > 12) {
                    alert("Tháng không hợp lệ");
                    return true;
                }
                else if (isNaN(self.year()) || self.year() < 0) {
                    alert("Năm không hợp lệ");
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
            remove: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                var r = confirm("Bạn có chắc xóa hay không ?");
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
                            url: '/Services/ChamCongService.asmx/QuanLyKhaiBaoCongTac_Delete',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                list: selectedRecords
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
            },
            accept: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                var selectedRecords = new Array();
                for (var i = 0, l = rows.length; i < l ; i++) {
                    var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                    selectedRecords.push({
                        Oid: row.Oid
                    });
                }
                if (selectedRecords.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (selectedRecords.length > 0) {
                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: ko.toJSON({
                            list: selectedRecords,
                            trangthai: 1
                        }),
                        success: function (result) {
                            alert("Chấp nhận thành công !!");
                            self.datagrid.jqxGrid('updatebounddata');
                            self.datagrid.jqxGrid('clearselection');
                        }
                    });

                }
            },
            cancel: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                var selectedRecords = new Array();
                for (var i = 0, l = rows.length; i < l ; i++) {
                    var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                    selectedRecords.push({
                        Oid: row.Oid
                    });
                }
                if (selectedRecords.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (selectedRecords.length > 0) {

                    $.ajax({
                        type: 'POST',
                        url: '/Services/ChamCongService.asmx/QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: ko.toJSON({
                            list: selectedRecords,
                            trangthai: 0
                        }),
                        success: function (result) {
                            alert("Thành công !!");
                            self.datagrid.jqxGrid('updatebounddata');
                            self.datagrid.jqxGrid('clearselection');
                        }
                    });


                }
            }
        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#quanlycongtac")[0]);
        });
    </script>
    <style type="text/css">
        .chitiet
        {
            width: 120px !important;
        }
    </style>    
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
        <div id="quanlycongtac">
        <%--<div class="block" id="userManage">
 
            <div class="hoatdong">
                <a href="#" data-bind="click:remove">
                    <img src="/Images/huy.png" />
                   Xóa</a>
            </div>
            <div class="khoalai" data-bind="click:accept">
                <a href="#">
                    <img src="/Images/hoatdong.png" />
                   Chấp nhận</a>
            </div>
            <div class="trove">
                <a href="#" data-bind="click:cancel">
                    <img src="/Images/huy.png" />
                    Không chấp nhận</a>
            </div>

        </div>--%>
            <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left:15px;">
                    <div class="row">
                        
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
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

            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px;height:32px; text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px;height:32px; text-align: center" maxlength="4" />
            <select style="width: 150px" data-bind="options: bophan, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: bophanSelected, optionsCaption: 'Tất cả'"></select>
            <select data-bind="options: trangthaiList, optionsText: 'Name', optionsValue: 'Id', value: trangthaiSelected, optionsCaption: 'Tất cả'"></select>
            <input type="text" placeholder="Mã nhân sự" data-bind="value: maNhanSu" style="height:32px;padding:5px;" />
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:content>
