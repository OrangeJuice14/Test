<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Manage" %>

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
            self.returnData = [];
            self.datagrid = datagrid;
            self.day = ko.observable(new Date().getDate()),
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.condition = ko.observableArray(conditionArr);
            self.conditionSelected = ko.observable(-1);
            self.diHoc = ko.observableArray(diHocArr);
            self.diHocSelected = ko.observable(null);
            self.name = ko.observable("");
            self.department = ko.observableArray();
            self.departmentSelected = ko.observable(null);
            self.loaiNhanSu = ko.observableArray();
            self.loaiNhanSuSelected = ko.observable();
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
            self.checkExits = function () {
                var check;

                var thang = 0;
                var nam = 0;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ngay: self.day(),
                        thang: self.month(),
                        nam: self.year()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        thang = data.Thang;
                        nam = data.Nam;
                    }
                });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThang_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: thang,
                        nam: nam,
                        boPhanId: self.departmentSelected(),
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        check = result.d;
                    }
                });
                return check;
            };
            self.checkExitsDept = function () {
                var check;

                var thang = 0;
                var nam = 0;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyChamCong_FindByDate',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        ngay: self.day(),
                        thang: self.month(),
                        nam: self.year()
                    }),
                    dataType: "json",
                    async: false,
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        thang = data.Thang;
                        nam = data.Nam;
                    }
                });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ChotChamCongThangDonVi_CheckExists',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: thang,
                        nam: nam,
                        boPhanId: self.departmentSelected(),
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
                    self.departmentSelected(obj[0].Oid);
                }
            });

            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyChamCong_GetListHinhThucNghi',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    obj.push({ Oid: 0, TenHinhThucNghi: 'Làm cả ngày' });
                    var data = $.Enumerable.From(obj).OrderBy(function (x) {
                        return x.TenHinhThucNghi;
                    }).ToArray();
                    self.status(data);
                    //self.categoriesJson = JSON.stringify(data);
                }
            });
            self.checkChot = function (month, year) {
                var check;
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/QuanLyChamCong_CheckChot',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: month,
                        nam: year,
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
            self.categoriesSource =
            {
                datatype: "json",
                datafields: [
                    { name: 'Oid' },
                    { name: 'TenHinhThucNghi' }
                ],
                localdata: self.status
            };
            self.categoriesAdapter = new $.jqx.dataAdapter(self.categoriesSource, { contentType: 'application/json; charset=utf-8', autoBind: true });
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
            self.source =
            {
                datatype: "json",
                datafields: [
                    { name: 'Oid', type: 'string' },
                    { name: 'MaNhanSu', type: 'string' },
                    { name: 'HoTen', type: 'string' },
                    { name: 'TenPhongBan', type: 'string' },
                    { name: 'IDHinhThucNghi', type: 'string' },
                    { name: 'TrangThaiList', value: 'IDHinhThucNghi', values: { source: self.categoriesAdapter.records, value: 'Oid', name: 'TenHinhThucNghi' } },
                    { name: 'Ngay', type: 'date' },
                    { name: 'DaChamCong', type: 'bool' }
                ],
                id: 'Id',
                //sortcolumn: 'TenPhongBan',
                sortdirection: 'asc',
                url: "/Services/ChamCongService.asmx/QuanLyChamCong_Find",
                formatdata: function (data) {
                    return {
                        ngay: self.day(),
                        thang: self.month(),
                        nam: self.year(),
                        bophan: self.departmentSelected() == undefined ? null : "'" + self.departmentSelected() + "'",
                        trangthaichamcong: self.conditionSelected(),
                        diHoc: self.diHocSelected(),
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
                editable: true,
                selectionmode: 'checkbox',
                pageable: true,
                pagesize: 5,
                sortable: true,
                filterable: true,
                rowsheight: 80,
                autorowheight: true,
                autoheight: true,
                theme: "darkBlue",
                columns: [
                  {
                      text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                          return "<div style='text-align:center;margin-top:30px;'>" + (value + 1) + "</div>";
                      }
                  },
                  {
                      text: 'Mã nhân sự', datafield: 'MaNhanSu', width: 100, align: 'center', cellsalign: "middle", editable: false
                  },
                  {
                      text: 'Họ tên', datafield: 'HoTen', width: 140, align: 'center', editable: false
                  },
                  {
                      text: 'Ngày', datafield: 'Ngay', width: 80, align: 'center', cellsformat: 'd/M/yyyy', cellsalign: "middle", editable: false
                  },
                  {
                      text: 'Đơn vị', datafield: 'TenPhongBan', width: 200, align: 'center', editable: false
                  },
                  {
                      text: 'Trạng thái', datafield: 'IDHinhThucNghi', displayfield: 'TrangThaiList', columntype: 'dropdownlist', width: 150, cellsalign: "middle", sortable: false, align: 'center',
                      cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                          var str = cellvalue == "" ? "<div style='text-overflow: ellipsis; overflow: hidden; padding-bottom: 2px; text-align: center; margin-top: 32px;'>Làm cả ngày</div>" : "<div style='text-overflow: ellipsis; overflow: hidden; padding-bottom: 2px; text-align: center; margin-top: 32px;'>" + cellvalue + "</div>";
                          return str;
                      },
                      createeditor: function (row, value, editor) {
                          editor.jqxDropDownList({ source: self.categoriesAdapter, displayMember: 'TenHinhThucNghi', valueMember: 'Oid', autoDropDownHeight: true });
                      }
                  },
                  //{
                  //    text: 'Trạng thái', datafield: 'IDHinhThucNghi', width: 150, cellsalign: "middle", sortable: false, align: 'center',
                  //    cellsrenderer: function (row, value, editor) {
                  //        var a = value;
                  //    }
                  //},
                  //{
                  //    text: 'Trạng thái', datafield: 'IDHinhThucNghi', columntype: 'template', width: 300, cellsalign: "middle", sortable: false, align: 'center',
                  //    cellsrenderer: function (row, columnfield, cellvalue, defaulthtml, columnproperties) {
                  //        var str = "";
                  //        $(self.status()).each(function (index, value) {
                  //            var checked = value.Oid == cellvalue ? "checked" : "";
                  //            var color = value.Oid == cellvalue ? "red" : "";
                  //            str += "<div style='padding-top:2px;margin-left:15px;float:left;width:134px;'>" +
                  //                "<input type='radio' class='jqx-radiobutton-default' name='status" + row + "'";
                  //            str += "value='" + value.Oid + "'" + checked + "/><" +
                  //                "span style='color:" + color + "'>" + value.TenHinhThucNghi + "</span></div>";
                  //            str += index == 2 ? "<br/>" : "";
                  //        });
                  //        return str;
                  //    },
                  //    initeditor: function (row, cellvalue, editor, cellText, width, height) {
                  //        var str = "";
                  //        cellvalue = cellvalue == null ? 0 : cellvalue;
                  //        $(self.status()).each(function (index, value) {
                  //            var checked = value.Oid == cellvalue ? "checked" : "";
                  //            var color = value.Oid == cellvalue ? "red" : "";
                  //            str += "<div style='padding-top:2px;margin-left:15px;float:left;width:134px;'>" +
                  //                "<input type='radio' class='jqx-radiobutton-default' name='status" + row + "'";
                  //            str += "value='" + value.Oid + "'" + checked + "/><" +
                  //                "span style='color:" + color + "'>" + value.TenHinhThucNghi + "</span></div>";
                  //            str += index == 2 ? "<br/>" : "";
                  //        });
                  //        editor.html('');
                  //        editor.append(str);
                  //    },
                  //    geteditorvalue: function (row, cellvalue, editor) {
                  //        return editor.find('input:checked').val();
                  //    }
                  //},
                   {
                       text: 'Chấm công', datafield: 'DaChamCong', align: 'center', columntype: 'checkbox', editable: false
                   },

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
                    if (self.checkChot(self.month(), self.year())) {
                        alert('Tháng này đã chốt chấm công rồi !!');
                        return;
                    }
                    else {
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
                }
            },
            remove: function () {
                var self = this;
                var rows = self.datagrid.jqxGrid('selectedrowindexes');
                if (rows.length == 0) {
                    alert("Chưa có dòng nào được chọn !!");
                    return;
                }
                var r = confirm("Bạn có chắc hủy hay không ?");
                if (r == true) {
                    var selectedRecords = new Array();
                    var HinhThucNghi = $.Enumerable.From(self.status()).Single(function (x) {
                        return x.KyHieu == '';
                    });
                    for (var i = 0, l = rows.length; i < l ; i++) {
                        var row = self.datagrid.jqxGrid('getrowdata', rows[i]);
                        selectedRecords.push({
                            Oid: row.Oid,
                            IDHinhThucNghi: HinhThucNghi.Oid,
                            DaChamCong: false
                        });
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
                                alert("Hủy thành công !!");
                                self.datagrid.jqxGrid('updatebounddata');
                                self.datagrid.jqxGrid('clearselection');
                            }
                        });

                    }
                } else {
                    return;
                }
            },
            compare: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn  !!");
                    return;
                }
                if (!self.checkExitsDept()) {
                    alert('Đơn vị chưa chốt chấm công !!');
                    return;
                }
                if (!self.checkExits()) {
                    alert('Quản trị chưa chốt chấm công !!');
                    return;
                }
                var url = "Compare.aspx?PhongBan=" + self.departmentSelected() + "&IdLoaiNhanSu=" + (self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected()) + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year() + "&Value=" + self.name();
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

            },
            detail: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn Đơn vị !!");
                    return;
                }
                var url = "Detail.aspx?PhongBan=" + self.departmentSelected() + "&IdLoaiNhanSu=" + (self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected()) + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year() + "&Value=" + self.name();
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

            },
            detailDept: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn Đơn vị !!");
                    return;
                }
                if (!self.checkExitsDept()) {
                    alert('Đơn vị chưa chốt chấm công !!');
                    return;
                }
                var url = "Detail.aspx?PhongBan=" + self.departmentSelected() + "&IdLoaiNhanSu=" + (self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected()) + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year() + "&Value=" + self.name();
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

            },
            detailAll: function () {
                var self = this;
                var url = "DetailAll.aspx?&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year();
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo(0, 0);
                objWindow.resizeTo(screen.availWidth, screen.availHeight);

            },
            chart: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn Đơn vị !!");
                    return;
                }

                var url = "Chart.aspx?PhongBan=" + self.departmentSelected() + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year();
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
            },
            update: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn Đơn vị !!");
                    return;
                }
                if (!self.checkExitsDept()) {
                    alert('Đơn vị chưa chốt chấm công !!');
                    return;
                }
                var url = "Update.aspx?PhongBan=" + self.departmentSelected() + "&IdLoaiNhanSu=" + (self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected()) + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year() + "&Value=" + self.name() + "&UserId=" + '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>';
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
            },
            updateDept: function () {
                var self = this;
                if (self.departmentSelected() == undefined) {
                    alert("Vui lòng chọn Đơn vị !!");
                    return;
                }
                var url = "Update.aspx?PhongBan=" + self.departmentSelected() + "&IdLoaiNhanSu=" + (self.loaiNhanSuSelected() == undefined ? null : self.loaiNhanSuSelected()) + "&Ngay=" + self.day() + "&Thang=" + self.month() + "&Nam=" + self.year() + "&Value=" + self.name() + "&UserId=" + '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>';
                var Width = 800, Height = 700;
                var OffsetHeight = document.body.offsetHeight;
                var OffsettWidth = document.body.offsetWidth;
                var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);
            },
            exportScan: function () {
                var self = this;
                window.open("/ExcelExport/InBangCong.ashx?ngay=" + self.day() + "&thang=" + self.month() + "&nam=" + self.year() + "&bophanId=" + self.departmentSelected());
            }
        };
        $(function () {
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
                    <div class="row">
                        <!-- ko if: WebGroup()==1 || WebGroup()==3 -->
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: save">
                                <i class="btn-label glyphicon glyphicon-ok"></i>Lưu
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: remove">
                                <i class="btn-label glyphicon glyphicon-remove"></i>Hủy chấm công
                            </a>
                        </div>
                        <!-- /ko -->
                        <div class=" col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: chart">
                                <i class="btn-label glyphicon glyphicon-stats"></i>Biểu đồ
                            </a>
                        </div>
                        <!-- ko if: WebGroup()==1 -->
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: detailAll">
                                <i class="btn-label glyphicon glyphicon-th-list"></i>Xuất báo cáo tổng
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: update">
                                <i class="btn-label glyphicon glyphicon-upload"></i>Chấm công tháng
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: detailDept">
                                <i class="btn-label glyphicon glyphicon-th-list"></i>Chi tiết
                            </a>
                        </div>
                        <!-- /ko -->
                        <!-- ko if: WebGroup()== 2 || WebGroup()==3 -->
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: updateDept">
                                <i class="btn-label glyphicon glyphicon-upload"></i>Chấm công tháng
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-darkorange" style="width: 158px;" data-bind="click: detail">
                                <i class="btn-label glyphicon glyphicon-th-list"></i>Chi tiết
                            </a>
                        </div>
                        <!-- /ko -->
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-palegreen" style="width: 158px;" data-bind="click: compare">
                                <i class="btn-label glyphicon glyphicon-th-list"></i>So sánh dữ liệu
                            </a>
                        </div>
                        <!-- ko if: WebGroup()== 2 || WebGroup()==3 || WebGroup()==1 -->
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-blue" style="width: 158px;" data-bind="click: exportScan">
                                <i class="btn-label glyphicon glyphicon-print"></i>Xuất giờ quét gốc
                            </a>
                        </div>
                        <!-- /ko -->
                    </div>
                </div>
            </div>
        </div>
     
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <input type="text" placeholder="ngày" data-bind="value: day" style="width: 50px;height:32px; text-align: center;" maxlength="2" />
            -
            <input type="text" placeholder="tháng" data-bind="value: month" style="width: 50px; height:32px;text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px; height:32px;text-align: center" maxlength="4" />
            <%--<select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected, optionsCaption: 'Tất cả phòng ban'"></select>--%>
            <select style="width: 150px" data-bind="options: department, optionsText: function (type) { return type.TenBoPhan }, optionsValue: 'Oid', value: departmentSelected"></select>
            <select data-bind="options: condition, optionsText: 'Name', optionsValue: 'Id', value: conditionSelected"></select>
            <select data-bind="options: diHoc, optionsText: 'Name', optionsValue: 'Id', value: diHocSelected"></select>
            <select data-bind="options: loaiNhanSu, optionsText: 'TenLoaiNhanSu', optionsValue: 'Oid', value: loaiNhanSuSelected, optionsCaption: 'Tất cả'"></select>
            <input type="text" placeholder="Mã nhân sự" data-bind="value: name" style="width: 110px;padding-left:5px; height:32px;" />
            <input type="button" value="Tìm" data-bind="click: search" style="width: 60px;height:32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:content>
