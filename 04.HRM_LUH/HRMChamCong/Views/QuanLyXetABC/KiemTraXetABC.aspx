<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="KiemTraXetABC.aspx.cs" Inherits="HRMChamCong.Views.QuanLyXetABC.KiemTraXetABC" %>
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
            var daXetAbc = [
               { Id: null, Name: "Tất cả" },
               { Id: false, Name: "Chưa xét" },
               { Id: true, Name: "Đã xét" }
            ];
            var self = this;
            self.returnData = [];
            self.datagrid = datagrid;
            self.month = ko.observable(new Date().getMonth() + 1);
            self.year = ko.observable(new Date().getFullYear());
            self.daXetAbc = ko.observableArray(daXetAbc);
            self.daXetAbcSelected = ko.observable(null);
            self.items = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KiemTraPhongBanXetABC_Find',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    thang: self.month(),
                    nam: self.year(),
                    daXetXongAbc: self.daXetAbcSelected(),
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.items(data);
                }
            });
            self.source =
            {
                id: 'Id',
                datafields: [
                   { name: 'TenPhongBan', type: 'string' },
                   { name: 'ThangNam', type: 'string' },
                   { name: 'TrangThai', type: 'string' }
                ],
                datatype: "json",
                url: "/Services/ChamCongService.asmx/KiemTraPhongBanXetABC_Find",
                localdata: self.items(),
                //formatdata: function (data) {
                //    return {
                //        thang: self.month(),
                //        nam: self.year(),
                //        daXetXongAbc: self.daXetAbcSelected(),
                //    };
                //},
                //beforeprocessing: function (result) {
                //    self.returnData = $.parseJSON(result.d);
                //    return self.returnData;
                //}
            };
            self.dataAdapter = new $.jqx.dataAdapter(self.source);
            self.datagrid.jqxGrid(
                {
                    source: self.dataAdapter,
                    width: '100%',
                    pageable: true,
                    pagesize: 20,
                    sortable: true,
                    filterable: true,
                    autoheight: true,
                    theme: "darkBlue",
                    columns: [
                        {
                            text: 'STT', columntype: 'number', width: 35, editable: false, cellsrenderer: function (row, column, value) {
                                return "<div style='text-align:center;margin-top:5px;'>" + (value + 1) + "</div>";
                            }
                        },
                        { text: 'Phòng ban', datafield: 'TenPhongBan', width: 610, align: 'center' },
                        { text: 'Tháng', datafield: 'ThangNam', cellsalign: "middle", width: 100, align: 'center' },
                        { text: 'Trạng thái', datafield: 'TrangThai', cellsalign: "middle",  align: 'center' }

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
                //self.datagrid.jqxGrid('updatebounddata');
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KiemTraPhongBanXetABC_Find',
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        thang: self.month(),
                        nam: self.year(),
                        daXetXongAbc: self.daXetAbcSelected(),
                    }),
                    dataType: "json",
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        self.source.localdata = data;
                        self.dataAdapter = new $.jqx.dataAdapter(self.source);
                        self.datagrid.jqxGrid({ source: self.dataAdapter });
                    }
                });
            },
            excel: function () {
                this.datagrid.jqxGrid('exportdata', 'xls', 'jqxgrid');
            }

        };
        $(function () {
            var model = new ViewModel($("#jqxgrid"), $("#userManage"));
            ko.applyBindings(model, $("#kiemtraxetabc")[0]);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="kiemtraxetabc">
        <%--<div class="block">
            <div class="hoatdong">
            </div>
            <div class="chitiet">
                <a href="#" data-bind="click:excel">
                    <img src="/Images/chitiet.png">
                    Export</a>
            </div>
            <div class="trove">
                <a href="#">
                    <img src="/Images/trove.png">
                    Trở về</a>
            </div>
        </div>--%>
        <div class="row">
            <div class="col-lg-12 col-xs-12 col-sm-12">
                <div class="buttons-preview" id="userManage" style="margin-left: 15px;">
                    <div class="row">
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="#" class="btn btn-labeled btn-yellow" style="width: 158px;" data-bind="click: excel">
                                <i class="btn-label glyphicon glyphicon-random"></i>Xuất Excel
                            </a>
                        </div>
                        <div class="col-lg-2 col-xs-12 col-sm-6">
                            <a href="javascript:history.back()" class="btn btn-labeled btn-blue" style="width: 158px;" >
                                <i class="btn-label glyphicon glyphicon-chevron-left"></i>Trở về
                            </a>
                        </div>
                      
                        
                    </div>
                </div>
            </div>
        </div>
        <div style="margin: 10px 0px 5px 0px; text-align: center">

            <input type="text" placeholder="tháng" data-bind="value:month" style="width: 50px;height:32px; text-align: center" maxlength="2" />
            -
            <input type="text" placeholder="năm" data-bind="value:year" style="width: 50px;height:32px; text-align: center" maxlength="4" />
            <select data-bind="options:daXetAbc, optionsText: 'Name', optionsValue: 'Id',value:daXetAbcSelected"></select>
            <input type="button" value="Tìm" data-bind="click:search" style="width: 60px;height:32px;" />
        </div>
        <div style="padding: 10px 0px 0px 0px;">
            <div id="jqxgrid"></div>
        </div>
    </div>
</asp:Content>
