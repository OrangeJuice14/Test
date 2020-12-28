<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.QuanLyXetABC.Manage" %>

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
    <script src="/Components/jqwidgets/jqxdata.export.js"></script>
    <script type="text/javascript">
        function ViewModel(datagrid) {
            var diHocArr = [
                { Id: null, Name: "Tất cả trạng thái" },
                { Id: false, Name: "Làm việc" },
                { Id: true, Name: "Đi học" }
            ];
            var self = this;
            self.returnData = [];
            self.datagrid = datagrid;
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
            self.diHoc = ko.observableArray(diHocArr);
            self.diHocSelected = ko.observable(null);
            self.bophan = ko.observableArray();
            self.bophanSelected = ko.observable();
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
                    self.bophanSelected(obj[0].Oid);
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
                    //self.loaiNhanSuSelected(obj[0].Oid);
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
                   { name: 'ThongTinNhanVien', type: 'string' },
                   { name: 'MaNhanSu', type: 'string' },
                   { name: 'HoVaTen', type: 'string' },
                   { name: 'DanhGiaList', value: 'DanhGia', values: { source: self.categoriesAdapter.records, value: 'Name', name: 'Name' } },
                   { name: 'DanhGiaTDCList', value: 'DanhGiaTruocDieuChinh', values: { source: self.categoriesAdapter.records, value: 'Name', name: 'Name' } },
                   { name: 'DanhGia', type: 'string' },
                   { name: 'DanhGiaTruocDieuChinh', type: 'string' },
                   { name: 'SoNgayCong', type: 'decimal' },
                   { name: 'DienGiai', type: 'string' },
                   { name: 'TrangThai', type: 'bool' },
                   { name: 'NghiNuaNgay', type: 'int' },
                   { name: 'NgayNghiFormat', type: 'string' },
                   { name: 'NghiCoPhep', type: 'int' },
                   { name: 'NghiHe', type: 'int' },
                   { name: 'NghiRo', type: 'int' },
                   { name: 'NghiThaiSan', type: 'int' },
                   { name: 'Khoa', type: 'bool' }
                ],
                id: 'Id',
                //async:false,
                //pagesize: 10,
                sortcolumn: 'Name',
                sortdirection: 'asc',
                url: "/Services/ChamCongService.asmx/QuanLyXetABC_Find",
                //sort: function (value, row) {
                //    self.datagrid.jqxGrid('updatebounddata');
                //},
                formatdata: function (data) {
                    return {
                        thang: self.month(),
                        nam: self.year(),
                        bophan: self.bophanSelected() == undefined ? null : "'" + self.bophanSelected() + "'",
                        idLoaiNhanSu: self.loaiNhanSuSelected() == undefined ? null : "'" + self.loaiNhanSuSelected() + "'",
                        diHoc: self.diHocSelected(),
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
                editable: true,
                selectionmode: 'checkbox',
                //virtualmode: true,
                filterable: true,
                pageable: true,
                pagesize: 20,
                sortable: true,
                rowsheight: 40,
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
                      text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 100, align: 'center', editable: false
                  },
                  {
                      text: 'Họ tên', datafield: 'HoVaTen', width: 150, align: 'center', editable: false
                  },
                  {
                      text: 'Đi làm', datafield: 'SoNgayCong', width: 50, align: 'center', cellsalign: "middle", editable: false, value:21.5
                  },
                  {
                      text: 'Loại', datafield: 'DanhGia', displayfield: 'DanhGiaList', columntype: 'dropdownlist', width: 50, cellsalign: "middle", sortable: false, align: 'center',
                      createeditor: function (row, value, editor) {
                          editor.jqxDropDownList({ source: self.categoriesAdapter, displayMember: 'Name', valueMember: 'Name', autoDropDownHeight: true });
                      }
                  },
                  {
                      text: 'Loại TĐC', datafield: 'DanhGiaTruocDieuChinh', displayfield: 'DanhGiaTDCList', columntype: 'dropdownlist', width: 70, cellsalign: "middle", sortable: false, align: 'center',
                      createeditor: function (row, value, editor) {
                          editor.jqxDropDownList({ source: self.categoriesAdapter, displayMember: 'Name', valueMember: 'Name', autoDropDownHeight: true });
                      }
                  },
                  {
                      text: 'Ngày nghỉ', datafield: 'NgayNghiFormat', width: 160, align: 'center', cellsalign: "middle", editable: false
                  },
                  //{
                  //    text: 'Nghỉ nửa ngày', datafield: 'NghiNuaNgay', width: 110, align: 'center', cellsalign: "middle", editable: false
                  //},
                  //{
                  //    text: 'Nghỉ có phép', datafield: 'NghiCoPhep', width: 100, align: 'center', cellsalign: "middle", editable: false
                  //},
                  //{
                  //    text: 'Nghỉ Ro', datafield: 'NghiRo', width: 70, align: 'center', cellsalign: "middle", editable: false
                  //},
                  //{
                  //    text: 'Nghỉ thai sản', datafield: 'NghiThaiSan', width: 100, align: 'center', cellsalign: "middle", editable: false
                  //},
                  //{
                  //    text: 'Nghỉ hè', datafield: 'NghiHe', width: 70, align: 'center', cellsalign: "middle", editable: false
                  //},
                  {
                      text: 'Giải trình', datafield: 'DienGiai', columntype: 'template', width: 170, cellsalign: "middle", align: 'center',
                      cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                          return "<div  style='text-align:center;margin-top:7px;'><input class='jqx-widget-content jqx-input jqx-widget jqx-rc-all' type='text' value='" + cellvalue + "'  style='width:155px;height:20px;text-align:center;' /></div>";
                      },
                      createeditor: function (row, cellvalue, editor, cellText, width, height) {
                          var str = "<div style='text-align:center;margin-top:7px;'><input  type='text' id='status' value='" + cellvalue + "'  style='width:155px;height:20px;text-align:center;' /></div>";
                          editor.append(str);
                      },
                      initeditor: function (row, cellvalue, editor, celltext, pressedkey) {
                          var status = editor.find('input#status');
                          status.val(cellvalue);
                          status.jqxInput({ width: 155, height: 20 });
                          status.jqxInput("focus");
                      },
                      geteditorvalue: function (row, cellvalue, editor) {
                          return editor.find('input#status').val();
                      }
                  },

                  {
                      text: 'Đã xét', datafield: 'TrangThai', align: 'center', columntype: 'checkbox', cellsalign: "middle", width: 60
                  },
                  {
                      text: 'Đã khóa', datafield: 'Khoa', align: 'center', columntype: 'checkbox', cellsalign: "middle", editable: false,
                  }
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
                var d = new Date();
                var n = d.getDate()
                if (self.validate())
                    return;
                self.datagrid.jqxGrid('updatebounddata');
                var khoaso;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KiemTraKhoaSo_KyTinhLuong',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ month: self.month, year: self.year }),
                    async: false,
                    success: function (result) {
                        khoaso = $.parseJSON(result.d);
                    }
                });
                var thoigian;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/CauHinhXetABC_GetThoiGian',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        thoigian = $.parseJSON(result.d);
                    }
                });
                if (khoaso == true || thoigian < n)
                {
                    document.getElementById("btnsave").className = "btn btn-labeled btn-palegreen disabled";
                    document.getElementById("btnmokhoa").className = "btn btn-labeled btn-darkorange disabled";
                    document.getElementById("btnkhoa").className = "btn btn-labeled btn-yellow disabled";
                }
                else
                {
                    document.getElementById("btnsave").className = "btn btn-labeled btn-palegreen";
                    document.getElementById("btnmokhoa").className = "btn btn-labeled btn-darkorange";
                    document.getElementById("btnkhoa").className = "btn btn-labeled btn-yellow";
                }
            },
            save: function () {
                var self = this;
                var selectedRecords = new Array();
                var rows = self.datagrid.jqxGrid('getrows');
                $(self.returnData).each(function (index, value) {
                    $(rows).each(function (index1, value1) {
                        if ((value.DanhGia != value1.DanhGia || value.DanhGiaTruocDieuChinh != value1.DanhGiaTruocDieuChinh || value.DienGiai != value1.DienGiai || value.TrangThai != value1.TrangThai) && value.Oid == value1.Oid && !value1.Khoa)
                            selectedRecords.push({
                                Oid: value1.Oid,
                                DanhGia: value1.DanhGia,
                                DanhGiaTruocDieuChinh: value1.DanhGiaTruocDieuChinh,
                                DienGiai: value1.DienGiai,
                                TrangThai: value1.TrangThai
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
                        url: '/Services/ChamCongService.asmx/QuanLyXetABC_Save',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: ko.toJSON({
                            objList: selectedRecords
                        }),
                        success: function (result) {
                            alert("Lưu thành công");
                            self.datagrid.jqxGrid('updatebounddata');
                            self.datagrid.jqxGrid('clearselection');
                        }
                    });

                }
            },
            khoa: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
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
                        url: '/Services/ChamCongService.asmx/QuanLyXetABC_KhoaVaMoKhoaList',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: ko.toJSON({
                            userList: selectedRecords,
                            khoa: true
                        }),
                        success: function (result) {
                            alert("Khóa thành công !!");
                            self.datagrid.jqxGrid('updatebounddata');
                            self.datagrid.jqxGrid('clearselection');
                        }
                    });
                }
            },
            chuakhoa: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
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
                        url: '/Services/ChamCongService.asmx/QuanLyXetABC_KhoaVaMoKhoaList',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        data: ko.toJSON({
                            userList: selectedRecords,
                            khoa: false
                        }),
                        success: function (result) {
                            alert("Mở khóa thành công !!");
                            self.datagrid.jqxGrid('updatebounddata');
                            self.datagrid.jqxGrid('clearselection');
                        }
                    });


                }
            },
            detail: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (rows.length > 1) {
                    alert("Chọn quá nhiều dòng !!");
                    return;
                }
                var row = self.datagrid.jqxGrid('getrowdata', rows[0]);
                var url = "Detail.aspx?Id=" + row.ThongTinNhanVien + "&Thang=" + self.month() + "&Nam=" + self.year();
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

            },
            chart: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                if (rows.length > 1) {
                    alert("Chọn quá nhiều dòng !!");
                    return;
                }
                else {
                    var row = self.datagrid.jqxGrid('getrowdata', rows[0]);
                    var url = "Chart.aspx?Id=" + row.ThongTinNhanVien + "&Thang=" + self.month() + "&Nam=" + self.year();
                    var Width = 800, Height = 700;
                    var OffsetHeight = document.body.offsetHeight;
                    var OffsettWidth = document.body.offsetWidth;
                    var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                    objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
                }
            },
            print: function () {
                var self = this;
                self.TenBoPhan = ko.observable();
                self.STT = ko.observable();
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/GetPhongBan_ById',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({ id: self.bophanSelected() }),
                    async: false,
                    success: function (result) {
                        var obj = $.parseJSON(result.d);
                        self.TenBoPhan = obj.TenBoPhan;
                        self.STT = obj.STT;
                    }
                });
                var str = '<table style="width="680";table-layout: fixed;" border="1" cellspacing="0" cellpadding="0">';
                str += '<tr style="font-size:13px;">';
                str += '<th style="padding:5px;">STT</th>';
                str += '<th style="padding:5px;">Mã</th>';
                str += '<th style="padding:5px;">Họ tên</th>';
                str += '<th style="padding:5px;">Ngày công</th>';
                str += '<th style="padding:5px;">Số ngày nghỉ</th>';
                str += '<th style="padding:5px;">Loại</th>';
                str += '<th style="padding:5px;">Giải trình</th>';
                str += '</tr>';
                for (var i = 0; i < this.dataAdapter.records.length; i++) {
                    var item = this.dataAdapter.records[i];
                    str += '<tr style="font-size:13px;">';
                    str += '<td style="text-align:center;padding:5px;width: 40px">' + (i + 1) + '</td>';
                    str += '<td style="padding:5px;width: 70px">' + item.MaNhanSu + '</td>';
                    str += '<td style="padding:5px;width: 160px">' + item.HoVaTen + '</td>';
                    str += '<td style="text-align:center;padding:5px;width: 50px">' + item.SoNgayCong + '</td>';
                    str += '<td style="padding:5px;width: 140px"> - Nghỉ Nửa ngày: ' + item.NghiNuaNgay + '<br> - Nghỉ có phép :' + item.NghiCoPhep + '<br> - Nghỉ Ro:' + item.NghiRo + '<br> - Nghỉ thai sản:' + item.NghiThaiSan + '<br> - Nghỉ Hè: ' + item.NghiHe + '</td>';
                    str += '<td style="text-align:center;padding:5px;width: 40px">' + item.DanhGia + '</td>';
                    str += '<td style="padding:5px;width: 180px">' + (item.DienGiai == null ? '' : item.DienGiai) + '</td>';
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
                    '<style>@page {size: 21cm 29.7cm; margin: 5mm 10mm 15mm 15mm;}</style>' +
                    '<div style="width:680px; text-align:center;"><img src="/Images/logo_UIH.png" width="150"><img src="/Images/name_school.png" width="400" ><br/></div>' +
                    '<div style="text-align:center;font-weight:bold;font-size:16px;width:680px;padding:10px 0px 0px 0px">DANH SÁCH CBVC XÉT ABC ­' + this.month() + '/' + this.year() + '</div>' +
                    '<div style="font-weight:bold;font-size:16px;width:680px;padding:10px 0px 10px 0px">' + self.STT + '. ' + self.TenBoPhan + '</div>' +
                    str +
                    '\n</body>\n</html>';
                document.write(pageContent);
                document.close();
                newWindow.print();
            },
            excel: function () {
                var self = this;
                this.datagrid.jqxGrid('hidecolumn', 'Khoa');
                var arr = new Array();
                var rows = self.datagrid.jqxGrid('getrows');
                for (var i = 0; i < rows.length; i++) {
                    var item = rows[i];
                    arr.push({
                        MaNhanSu: item.MaNhanSu,
                        HoVaTen: item.HoVaTen,
                        SoNgayCong: item.SoNgayCong,
                        TrangThai: item.TrangThai ? 'Đã xét' : 'Không xét',
                        NghiNuaNgay: item.NghiNuaNgay,
                        NghiCoPhep: item.NghiCoPhep,
                        NghiHe: item.NghiHe,
                        NghiRo: item.NghiRo,
                        NghiThaiSan: item.NghiThaiSan,
                        DienGiai: item.DienGiai,
                        DanhGiaList: item.DanhGiaList,
                        DanhGiaTDCList: item.DanhGiaTDCList
                    });
                }
                this.datagrid.jqxGrid('exportdata', 'xls', 'jqxgrid', true, arr);
                this.datagrid.jqxGrid('showcolumn', 'Khoa');
            }

        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#quanlyxetabc")[0]);
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="quanlyxetabc">
           <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left:15px;">
                    <div class="row">
                        <div class="col-lg-3 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" id="btnsave" style="width: 158px;" data-bind="click: save">
                                <i class="btn-label glyphicon glyphicon-check"></i>Lưu
                            </a>
                        </div>
                        <div class="col-lg-3 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" id="btnmokhoa" style="width: 158px;" data-bind="click: chuakhoa">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Mở khóa
                            </a>
                        </div>
                        <div class=" col-lg-3 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-yellow" id="btnkhoa" style="width: 158px;" data-bind="click: khoa">
                                <i class="btn-label glyphicon glyphicon-lock"></i>Khóa lại
                            </a>
                        </div>
                        <div class="col-lg-3 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: chart">
                                <i class="btn-label glyphicon glyphicon-stats"></i>Biểu đồ
                            </a>
                        </div>
                        <div class="col-lg-3 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: print">
                                <i class="btn-label glyphicon glyphicon-print"></i>In
                            </a>
                        </div>
                        <div class="col-lg-3 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: detail">
                                <i class="btn-label glyphicon glyphicon-th-list"></i>Chi tiết
                            </a>
                        </div>
                        
                        <div class="col-lg-3 col-xs-12 col-sm-6">
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

            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px; height:32px; text-align: center"; maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px;height:32px; text-align: center"; maxlength="4" />
            <select data-bind="options: bophan, optionsText: function (type) { return type.TenBoPhan }, optionsValue: 'Oid', value: bophanSelected" style="width:200px;"></select>
            <select data-bind="options: loaiNhanSu, optionsText: 'TenLoaiNhanSu', optionsValue: 'Oid', value: loaiNhanSuSelected, optionsCaption: 'Tất cả'"></select>
            <select data-bind="options: diHoc, optionsText: 'Name', optionsValue: 'Id', value: diHocSelected" style="width:120px;"></select>
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px; height:32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:Content>
