<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.QuaTrinhCongTac.Manage" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/CSS/hrmmain.css" rel="stylesheet" />
    <style type="text/css">
        .boldText {
            font-weight: bold;
        }

        .textToCenter {
            text-align: center;
        }

        .textToRight {
            text-align: right;
        }

        .backGroundTitle {
            background: #DCDCDC;
        }
    </style>
    <script type="text/javascript">
        function formatDate(date) {
            return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
        }
        function ViewModel_LichSuBanThan() {
            var self = this;
            self.DanhSach_LichSuBanThan = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_LichSuBanThan',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_LichSuBanThan(data);
                }
            });
        }
        function ViewModel_DienBienLuong() {
            var self = this;
            self.DanhSach_DienBienLuong = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_DienBienLuong',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_DienBienLuong(data);
                }
            });
        }

        function ViewModel_QuaTrinhDaoTao() {
            var self = this;
            self.DanhSach_QuaTrinhDaoTao = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhDaoTao',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhDaoTao(data);
                }
            });
        }

        function ViewModel_QuaTrinhBoiDuong() {
            var self = this;
            self.DanhSach_QuaTrinhBoiDuong = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhBoiDuong',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhBoiDuong(data);
                }
            });
        }

        function ViewModel_QuaTrinhBoNhiem() {
            var self = this;
            self.DanhSach_QuaTrinhBoNhiem = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhBoNhiem',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhBoNhiem(data);
                }
            });
        }

        function ViewModel_QuaTrinhDiNuocNgoai() {
            var self = this;
            self.DanhSach_QuaTrinhDiNuocNgoai = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhDiNuocNgoai(data);
                }
            });
        }

        function ViewModel_QuaTrinhKhenThuong() {
            var self = this;
            self.DanhSach_QuaTrinhKhenThuong = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhKhenThuong',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhKhenThuong(data);
                }
            });
        }

        function ViewModel_QuaTrinhKyLuat() {
            var self = this;
            self.DanhSach_QuaTrinhKyLuat = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhKyLuat',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhKyLuat(data);
                }
            });
        }

        function ViewModel_QuaTrinhNghienCuu() {
            var self = this;
            self.DanhSach_QuaTrinhNghienCuu = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhNghienCuu(data);
                }
            });
        }

        function ViewModel_QuaTrinhHoatDongXaHoi() {
            var self = this;
            self.DanhSach_QuaTrinhHoatDongXaHoi = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhHoatDongXaHoi',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhHoatDongXaHoi(data);
                }
            });
        }

        function ViewModel_QuaTrinhHoiThao() {
            var self = this;
            self.DanhSach_QuaTrinhThamGiaHoiThao = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhHoiThao',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhThamGiaHoiThao(data);
                }
            });
        }

        function ViewModel_QuaTrinhLucLuongVuTrang() {
            var self = this;
            self.DanhSach_QuaTrinhThamGiaLucLuongVuTrang = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhThamGiaLucLuongVuTrang(data);
                }
            });
        }

        function ViewModel_QuaTrinhCongTac() {
            var self = this;
            self.DanhSach_QuaTrinhCongTac = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_QuaTrinhCongTac',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.DanhSach_QuaTrinhCongTac(data);
                }
            });
        }
        
        function LoadMenuTab() {
            var self = this;
            self.menuList = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/WebMenu_GetChildMenuListBy_WebUserId_AndMenuId',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.UserId.ToString()]%>',
                    menuId: '<%#HttpContext.Current.Request.QueryString["Id"]%>',
                }),
                dataType: "json",
                success: function(result) {
                    var obj = $.parseJSON(result.d);
                    $(obj).each(function (index, value) {
                        value.tabId = value.Url.split("/")[3].split(".")[0];
                    });
               
                    self.menuList(obj);
                
                }
            });
        }

        $(document).ready(function () {
            var menuTab = new LoadMenuTab();
            ko.applyBindings(menuTab, $('#jqxTabs_MenuList_QuaTrinhCongTac')[0]);
            $('#jqxTabs_MenuList_QuaTrinhCongTac').jqxTabs({ width: '100%', theme: 'darkBlue', scrollStep: 500, scrollPosition: 'both' });
            var loadPage = function (url, tabIndex, value) {
                $.get(url, function (data) {
                    $('#' + value).html(data);
                    ko.cleanNode($('#' + value)[0]);                    
                    ko.applyBindings(new window["ViewModel_" + value](), document.getElementById(value));                   
                });
            };
            loadPage(menuTab.menuList()[0].Url, 1, menuTab.menuList()[0].Url.split('/')[3].split('.')[0]);
            $('#jqxTabs_MenuList_QuaTrinhCongTac').on('selected', function (event) {
                var pageIndex = event.args.item;
                var contentDiv = $("#jqxTabs_MenuList_QuaTrinhCongTac .jqx-tabs-content-element")[pageIndex];
                if (contentDiv.id == '')
                    return;
                loadPage('/Views/QuaTrinhCongTac/' + contentDiv.id + '.aspx', pageIndex, contentDiv.id);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id='jqxTabs_MenuList_QuaTrinhCongTac'>
        <ul >
             <!-- ko foreach:menuList -->
             <li data-bind="text: Name" style="margin-left: 5px;"></li>
             <!-- /ko -->
   
        </ul>
        <!-- ko foreach:menuList -->
            <div data-bind="attr: {id: $data.tabId }" style="padding: 10px 10px;"></div>
        <!-- /ko -->
     
    </div>


</asp:Content>
