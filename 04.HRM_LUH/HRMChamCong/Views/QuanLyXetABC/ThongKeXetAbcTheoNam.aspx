<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ThongKeXetAbcTheoNam.aspx.cs" Inherits="HRMChamCong.Views.QuanLyXetABC.ThongKeXetAbcTheoNam" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/Components/jqwidgets/jqxdata.export.js"></script>
    <script type="text/javascript">
        function ViewModel(datagrid) {
            var self = this;
            self.returnData = [];
            self.datagrid = datagrid;
            self.year = ko.observable(new Date().getFullYear());
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
            self.bophan = ko.observableArray();
            self.bophanSelected = ko.observable();
            self.name = ko.observable("");
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
                    self.bophan(obj);
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
                    self.loaiNhanSuSelected(obj[0].Oid);
                }
            });
            self.categoriesSource =
            {
                datatype: "json",
                datafields: [
                    { name: 'Name', type: 'string' }
                ],
                localdata:
                    [
                      { Name: "A" },
                      { Name: "B" },
                      { Name: "C" },
                      { Name: "D" },
                      { Name: "Không xét" }
                    ]
            };
            self.categoriesAdapter = new $.jqx.dataAdapter(self.categoriesSource, { contentType: 'application/json; charset=utf-8', autoBind: true });
            self.source =
            {
                datatype: "json",
                datafields: [
                   { name: 'Oid', type: 'string' },
                   { name: 'MaNhanSu', type: 'string' },
                   { name: 'HoVaTen', type: 'string' },
                   { name: 'TenPhongBan', type: 'string' },
                   { name: 'LoaiA', type: 'int' },
                   { name: 'LoaiB', type: 'int' },
                   { name: 'LoaiC', type: 'int' },
                   { name: 'LoaiD', type: 'int' },
                   { name: 'LoaiKhongXet', type: 'int' }
                ],
                id: 'Id',
                //async:false,
                //pagesize: 10,
                sortcolumn: 'Name',
                sortdirection: 'asc',
                url: "/Services/ChamCongService.asmx/ThongKeXetABCTheoNam_Find",
                //sort: function (value, row) {
                //    self.datagrid.jqxGrid('updatebounddata');
                //},
                formatdata: function (data) {
                    return {
                        nam: self.year(),
                        bophan: self.bophanSelected() == undefined ? null : "'" + self.bophanSelected() + "'",
                        idLoaiNhanSu: self.loaiNhanSuSelected() == undefined ? null : "'" + self.loaiNhanSuSelected() + "'",
                        maNhanSu: self.name(),
                        webUserId: "'" + '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>' + "'"
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
                   //selectionmode: 'checkbox',
                   pageable: true,
                   pagesize: 20,
                   sortable: true,
                   filterable: true,
                   autoheight: true,
                   autorowheight: true,
                   theme: "darkBlue",
                   columns: [
                       {
                           text: 'STT', columntype: 'number', width: 35, editable: false,
                           cellsrenderer: function (row, column, value) {
                               return "<div style='text-align:center;margin-top:5px;'>" + (value + 1) + "</div>";
                           }
                       },
                       { text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 120, align: 'center' },
                       {
                           text: 'Họ và tên', datafield: 'HoVaTen', width: 200, align: 'center',
                           //cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                           //    var item = self.datagrid.jqxGrid('getrowdata', row);
                           //    return "<div style='padding:5px 0px 0px 5px;'><a href='/Views/QuanLyXetABC/ChiTietThongKeXetABC.aspx?Id=" + item.Oid + "' style=''>" + cellvalue + "</a></div>";
                           //}
                       },
                       { text: 'Tên phòng ban', datafield: 'TenPhongBan', width: 220, align: 'center' },
                       { text: 'Loại A', datafield: 'LoaiA', width: 80, align: 'center', cellsalign: "middle" },
                       { text: 'Loại B', datafield: 'LoaiB', width: 80, align: 'center', cellsalign: "middle" },
                       { text: 'Loại C', datafield: 'LoaiC', width: 80, align: 'center', cellsalign: "middle" },
                       { text: 'Loại D', datafield: 'LoaiD', width: 80, align: 'center', cellsalign: "middle" },
                       { text: 'Không xét', datafield: 'LoaiKhongXet',  align: 'center', cellsalign: "middle" }

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
            search: function () {
                var self = this;
                if (self.validate())
                    return;
                self.datagrid.jqxGrid('updatebounddata');
            },
            print: function () {
                var self = this;
                var str = '<table style="width: 100%;table-layout: fixed;" border="1" cellspacing="0" cellpadding="0">';
                str += '<tr>';
                str += '<th style="padding:10px 5px;width: 41px">STT</th>';
                str += '<th style="padding:10px 5px;width: 94px">Mã</th>';
                str += '<th style="padding:10px 5px;width: 150px">Họ tên</th>';
                str += '<th style="padding:10px 5px;width: 126px">Tên phòng ban</th>';
                str += '<th style="padding:10px 5px;width: 80px">Loại A</th>';
                str += '<th style="padding:10px 5px;width: 80px">Loại B</th>';
                str += '<th style="padding:10px 5px;width: 80px">Loại C</th>';
                str += '<th style="padding:10px 5px;width: 80px">Loại D</th>';
                str += '<th style="padding:10px 5px;width: 80px">Không xét</th>';
                str += '</tr>';
                for (var i = 0; i < this.dataAdapter.records.length; i++) {
                    var item = this.dataAdapter.records[i];
                    str += '<tr>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + (i + 1) + '</td>';
                    str += '<td style="padding:10px 5px;">' + item.MaNhanSu + '</td>';
                    str += '<td style="padding:10px 5px;">' + item.HoVaTen + '</td>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + item.TenPhongBan + '</td>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + item.LoaiA + '</td>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + item.LoaiB + '</td>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + item.LoaiC + '</td>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + item.LoaiD + '</td>';
                    str += '<td style="text-align:center;padding:10px 5px;">' + item.LoaiKhongXet + '</td>';
                    str += '</tr>';
                };
                str += '</table>';
                var newWindow = window.open('', '', 'width=800, height=500'),
                document = newWindow.document.open(),
                pageContent =
                    '<!DOCTYPE html>\n' +
                    '<html>\n' +
                    '<head>\n' +
                    '<meta charset="utf-8" />\n' +
                    '<title>HUI chấm công</title>\n' +
                    '</head>\n' +
                    '<body>' +
                    '<img src="/Images/logo_UIH.png"><img src="/Images/name_school.png" style="padding-bottom:18px;"><br/>\n' +
                    '<div style="text-align:center;font-weight:bold;font-size:16px;">DANH SÁCH THỐNG KÊ XÉT ABC THEO NĂM­ ' + this.year() + '</div>' +
                    str +
                    '\n</body>\n</html>';
                document.write(pageContent);
                document.close();
                newWindow.print();
            },
            excel: function () {
                var self = this;
                this.datagrid.jqxGrid('exportdata', 'xls', 'jqxgrid');
            }
        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#thongkeabctheonam")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="thongkeabctheonam">
        <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left:15px;">
                    <div class="row">
                        
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: print">
                                <i class="btn-label glyphicon glyphicon-print"></i>In
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-yellow" style="width: 158px;" data-bind="click: excel">
                                <i class="btn-label glyphicon glyphicon-random"></i>Xuất Excel
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
            <input type="text" placeholder="năm" data-bind="value:year" style="width: 50px;height:32px; text-align: center" maxlength="4" />
            <select style="width: 150px" data-bind="options: bophan, optionsText: function (type) { return type.STT + '. ' + type.TenBoPhan }, optionsValue: 'Oid', value: bophanSelected, optionsCaption: 'Tất cả phòng ban', visible: webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21'"></select>
            <input type="text" placeholder="Mã nhân sự" data-bind="value: name, visible: webGroupId() != '53D57298-1933-4E4B-B4C8-98AFED036E21'" style="width: 110px;height:32px;padding:5px;" />
            <input type="button" value="Tìm" data-bind="click:search" style="width: 60px;height:32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:Content>
