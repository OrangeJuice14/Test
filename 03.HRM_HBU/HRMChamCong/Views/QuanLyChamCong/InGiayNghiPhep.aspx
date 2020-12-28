<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="InGiayNghiPhep.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.InGiayNghiPhep" %>


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
            self.year = ko.observable(new Date().getFullYear());
            self.source =
            {
                datatype: "json",
                datafields: [
                    { name: 'Oid', type: 'string' },
                    { name: 'MaQuanLy', type: 'string' },
                    { name: 'TenPhongBan', type: 'string' },
                    { name: 'HoTen', type: 'string' }
                ],
                id: 'Id',
                url: "/Services/ChamCongService.asmx/HoSoNhanVien_ByCurrentUser",
                formatdata: function (data) {
                    return {
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
                            text: 'Họ tên', datafield: 'HoTen', width: 250, align: 'center'
                        },
                        {
                            text: 'Đơn vị', datafield: 'TenPhongBan', align: 'center'
                        },

                    ]
                });
        }
        ViewModel.prototype = {
            search: function () {
                var self = this;
                if (self.validate())
                    return;
                self.datagrid.jqxGrid('updatebounddata');
            },
            print: function () {
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

                    var self = this;
                    var url = "Print.aspx?NhanVien=" + selectedRecords[0].Oid + "&Nam=" + self.year();
                    var Width = 800, Height = 700;
                    var OffsetHeight = document.body.offsetHeight;
                    var OffsettWidth = document.body.offsetWidth;
                    var objWindow = window.open(url, "", "width=" + Width + ",height=" + Height + ",resizable=1,scrollbars=yes,location=0");
                    objWindow.moveTo((OffsettWidth - Width) / 2, (OffsetHeight - Height) / 2);

                }
            },

        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"));
            ko.applyBindings(model, $("#ChamCongNgayNghi")[0]);
        });
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div id="ChamCongNgayNghi">
        <div style="margin: 10px 0px 10px 0px; text-align: center">
            <input type="text" placeholder="năm" data-bind="value: year" style="width: 50px; height: 32px; text-align: center" maxlength="4" />
            <input type="button" value="In giấy nghỉ phép" data-bind="click: print" style="height: 32px;" />
        </div>
        <div style="padding: 0px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>              
    </div>
</asp:content>
