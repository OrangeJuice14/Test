<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="KyChamCong.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.KyChamCong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        th, td { padding: 5px; }
    </style>
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
            self.source =
            {
                datatype: "json",
                datafields: [
                   { name: 'Oid', type: 'string' },
                   { name: 'TuNgay', type: 'date' },
                   { name: 'DenNgay', type: 'date' },
                   { name: 'Thang', type: 'int' },
                   { name: 'Nam', type: 'int' }
                ],
                id: 'Id',
                pagesize: 12,
                sortcolumn: 'Name',
                sortdirection: 'asc',
                url: "/Services/ChamCongService.asmx/KyChamCong_Find",
                formatdata: function (data) {
                    return {
                        nam: self.year()
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
                pagesize: 12,
                sortable: true,
                filterable: true,
                autoheight: true,
                autorowheight: true,
                theme: "darkBlue",
                columns: [
                  {
                      text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                          return "<div style='text-align:center;margin-top:5px;'>" + (value + 1) + "</div>";
                      }
                  },
                    {
                        text: 'Tháng', datafield: 'Thang', align: 'center', cellsalign: "middle"
                    },
                  {
                      text: 'Năm', datafield: 'Nam', align: 'center', cellsalign: "middle"
                  },
                  {
                      text: 'Từ ngày', datafield: 'TuNgay',  align: 'center', cellsalign: "middle", columnGroup: 'ThoiGian', cellsformat: 'dd/MM/yyyy'
                  },
                  {
                      text: 'Đến ngày', datafield: 'DenNgay',  align: 'center', cellsalign: "middle", columnGroup: 'ThoiGian', cellsformat: 'dd/MM/yyyy'
                  }
                ],
                columnGroups: [
                   { text: 'Thời gian', cellsAlign: 'center', align: 'center', name: 'ThoiGian' }
                ]
            });

        }

        ViewModel.prototype = {
            validate: function () {
                var self = this;
                if (isNaN(self.year()) || self.year() < 0) {
                    alert("Năm không hợp lệ");
                    return true;
                }
                return false;
            },
            validateFromDateToDate: function (tuNgay, denNgay) {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyChamCong_KiemTraTuNgayDenNgayCoHopLe',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: ko.toJSON({
                        tuNgay: tuNgay,
                        denNgay: denNgay
                    }),
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        check = obj;
                    }
                });
                return check;
            },
            checkExist: function (thang,nam) {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyChamCong_CheckExist',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: ko.toJSON({
                        thang: thang,
                        nam: nam
                    }),
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        check = obj;
                    }
                });
                return check;
            },
            search: function () {
                var self = this;
                if (self.validate())
                    return;
                self.datagrid.jqxGrid('updatebounddata');
            },
            add: function () {
                $("#Cancel").jqxButton({ theme: "darkBlue" });
                $("#Save").jqxButton({ theme: "darkBlue" });
                var offset = $("#jqxgrid").offset();
                $("#popupWindow").jqxWindow({ position: { x: parseInt(offset.left) + 120, y: parseInt(offset.top) - 120 } });
                $("#jqxFromDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#jqxToDate").jqxDateTimeInput({ width: '250px', height: '25px' });
                $("#popupWindow").jqxWindow('open');
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
                            url: '/Services/ChamCongService.asmx/KyChamCong_DeleteList',
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            data: ko.toJSON({
                                list: selectedRecords
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
            }
        };
        $(function () {

            var model = new ViewModel($("#jqxgrid"));

            $("#popupWindow").jqxWindow({
                width: 450, theme: "darkBlue", height: 250, resizable: true, isModal: false, autoOpen: false, cancelButton: $("#Cancel")
            });
            $("#Save").click(function () {
                if (!model.checkExist($("#txtMonth").val(), $("#txtYear").val())) {
                    alert("Kỳ chấm công đã tồn tại!");
                    return;
                }
                else if (!model.validateFromDateToDate($('#jqxFromDate').jqxDateTimeInput('getDate'), $('#jqxToDate').jqxDateTimeInput('getDate'))) {
                    alert("Trùng hoặc giao ngày với dữ liệu trước!");
                    return;
                }
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyChamCong_AddNew',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: $("#txtMonth").val(),
                        nam: $("#txtYear").val(),
                        tungay: $('#jqxFromDate').jqxDateTimeInput('getDate'),
                        denngay: $('#jqxToDate').jqxDateTimeInput('getDate')
                    }),
                        dataType: "json",
                        async: false,
                        success: function (result) {

                        }
                    });
                $("#jqxgrid").jqxGrid('updatebounddata');
                $("#popupWindow").jqxWindow('hide');
            });
            ko.applyBindings(model, $("#kychamcong")[0]);

        });
    </script>
    <style type="text/css">
        .chitiet {
            width: 120px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="kychamcong">

        <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                    <div class="row">
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: add">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Tạo mới
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Xóa
                            </a>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <input type="text" placeholder="Năm" data-bind="value: year" style="width: 50px; height: 32px; text-align: center" maxlength="4" />
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px; height: 32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
        <div id="popupWindow">
            <div>Kỳ chấm công</div>
            <div style="overflow: hidden;">
                <table>
                    <tr>
                        <td align="right">Tháng:</td>
                        <td align="left">
                            <input type="text" id="txtMonth" style="width: 50px;" /></td>
                    </tr>
                    <tr>
                        <td align="right">Năm:</td>
                        <td align="left">
                            <input type="text" id="txtYear"  style="width: 50px;" /></td>
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
                        <td align="right"></td>
                        <td style="padding-top: 10px;" align="right">
                            <input style="margin-right: 5px;" type="button" id="Save" value="Save" data-bind="click: save" />
                            <input id="Cancel" type="button" value="Cancel" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
