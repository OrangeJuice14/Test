<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.DangVien.Manage" %>
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
        function ViewModel_HoSoCongDoan() {
            var self = this;
            self.ChucVu = "";
            self.SoQuyetDinh = "";
            self.SoThe = "";
            self.ToChucCongDoan = "";

            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_CongDoan',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.ChucVu = data.ChucVu;
                    self.SoQuyetDinh = data.SoQuyetDinh;
                    self.SoThe = data.SoThe;
                    self.ToChucCongDoan = data.ToChucCongDoan;
                }
            });
        }
        
        function ViewModel_HoSoDangVien() {
            var self = this;
            self.ChiBoDang = "";
            self.ChucVu = "";
            self.DangBo = "";
            self.NgayCap = "";
            self.NgayVaoDang = "";
            self.NgayVaoDangChinhThuc = "";
            self.NoiCap = "";
            self.SoLyLich = "";
            self.SoThe = "";
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_DangVien',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.ChiBoDang = data.ChiBoDang;
                    self.ChucVu = data.ChucVu;
                    self.DangBo = data.DangBo;
                    self.NgayCap = data.NgayCap;
                    self.NgayVaoDang = data.NgayVaoDang;
                    self.NgayVaoDangChinhThuc = data.NgayVaoDangChinhThuc;
                    self.NoiCap = data.NoiCap;
                    self.SoLyLich = data.SoLyLich;
                    self.SoThe = data.SoThe;
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
                success: function (result) {
                    var obj = $.parseJSON(result.d);
                    $(obj).each(function (index, value) {
                        value.tabId = value.Url.split("/")[3].split(".")[0];
                    });
                    self.menuList(obj);

                }
            });
        }
        
        function ViewModel_HoSoDoanVien() {
            var self = this;
            self.ChucVu = "";
            self.NgayCap = "";
            self.NgayKetNap = "";
            self.NoiKetNap = "";
            self.SoThe = "";
            self.ToChucDoan = "";
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ModuleThongTinNhanSu_DoanVien',
                async: false,
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>'
                }),
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.ChucVu = data.ChucVu;
                    self.NgayCap = data.NgayCap;
                    self.NgayKetNap = data.NgayKetNap;
                    self.NoiKetNap = data.NoiKetNap;
                    self.SoThe = data.SoThe;
                    self.ToChucDoan = data.ToChucDoan;
                }
            });
        }
        
        $(document).ready(function () {
            var menuTab = new LoadMenuTab();
            ko.applyBindings(menuTab, $('#jqxTabs_MenuList_DangVien')[0]);
            $('#jqxTabs_MenuList_DangVien').jqxTabs({ width: '100%', theme: 'darkBlue', scrollStep: 500 });
            var loadPage = function (url, tabIndex, value) {
                $.get(url, function (data) {
                    $('#' + value).html(data);
                    ko.cleanNode($('#' + value)[0]);
                    ko.applyBindings(new window["ViewModel_" + value](), document.getElementById(value));
                });
            };
            loadPage(menuTab.menuList()[0].Url, 1, menuTab.menuList()[0].Url.split('/')[3].split('.')[0]);
            $('#jqxTabs_MenuList_DangVien').on('selected', function (event) {
                var pageIndex = event.args.item;
                var contentDiv = $("#jqxTabs_MenuList_DangVien .jqx-tabs-content-element")[pageIndex];
                if (contentDiv.id == '')
                    return;
                loadPage('/Views/DangVien/' + contentDiv.id + '.aspx', pageIndex, contentDiv.id);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div id='jqxTabs_MenuList_DangVien'>
        <ul >
             <!-- ko foreach:menuList -->
             <li data-bind="text: Name" style="margin-left: 5px;"></li>
             <!-- /ko -->
   
        </ul>
        <!-- ko foreach:menuList -->
            <div data-bind="attr: { id: $data.tabId }" style="padding: 10px 10px;"></div>
        <!-- /ko -->
     
    </div>
</asp:Content>
