<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compare.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCongNgoaiGio.Compare" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

     <link href="/assets/css/bootstrap.min.css" rel="stylesheet" />
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/helper.js"></script>
    <script src="/Scripts/jquery.linq.min.js"></script>


    <script src="/assets/js/skins.min.js"></script>
    <script type="text/javascript">
        $(function () {
            var self = this;
            self.SoCongNgoaiGio = ko.observable(0);
            self.SoCongNgoaiGioDonVi = ko.observable(0);
            self.SoCongNgoaiGioSau23Gio = ko.observable(0);
            self.SoCongNgoaiGioSau23GioDonVi = ko.observable(0);
            self.SoCongNgoaiGioT7CN = ko.observable(0);
            self.SoCongNgoaiGioT7CNDonVi = ko.observable(0);
            self.SoCongNgoaiGioLe = ko.observable(0);
            self.SoCongNgoaiGioLeDonVi = ko.observable(0);
            self.SoCongNgoaiGioLeSau23Gio = ko.observable(0);
            self.SoCongNgoaiGioLeSau23GioDonVi = ko.observable(0);
            self.SoCongNgoaiGioT7CNSau23Gio = ko.observable(0);
            self.SoCongNgoaiGioT7CNSau23GioDonVi = ko.observable(0);
            var data = null;
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ChamCongNgoaiGioTheoNgayThayDoi_GetByOid',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    Oid: '<%#Request.QueryString["Oid"] %>'
                    }),
                     dataType: "json",
                     async: false,
                     success: function (result) {
                         data = $.parseJSON(result.d);
                         if (data != null) {
                             self.SoCongNgoaiGio(data.SoCongNgoaiGio);
                             self.SoCongNgoaiGioDonVi(data.SoCongNgoaiGioDonVi);
                             self.SoCongNgoaiGioSau23Gio(data.SoCongNgoaiGioSau23Gio);
                             self.SoCongNgoaiGioSau23GioDonVi(data.SoCongNgoaiGioSau23GioDonVi);
                             self.SoCongNgoaiGioT7CN(data.SoCongNgoaiGioT7CN);
                             self.SoCongNgoaiGioT7CNDonVi(data.SoCongNgoaiGioT7CNDonVi);
                             self.SoCongNgoaiGioLe(data.SoCongNgoaiGioLe);
                             self.SoCongNgoaiGioLeDonVi(data.SoCongNgoaiGioLeDonVi);
                             self.SoCongNgoaiGioLeSau23Gio(data.SoCongNgoaiGioLeSau23Gio);
                             self.SoCongNgoaiGioLeSau23GioDonVi(data.SoCongNgoaiGioLeSau23GioDonVi);
                             self.SoCongNgoaiGioT7CNSau23Gio(data.SoCongNgoaiGioT7CNSau23Gio);
                             self.SoCongNgoaiGioT7CNSau23GioDonVi(data.SoCongNgoaiGioT7CNSau23GioDonVi);
                         }

                     }
                 });
            ko.applyBindings(data, $("#chamcong_detail")[0]);
        });
    </script>
</head>
<body>
    <div id="chamcong_detail">
        <table class="table table-bordered table-hover table-striped" style="font-size: 14px;text-align:center">
            <thead>
                <tr>
                    <td></td>
                    <td style="text-align:center;font-weight:bold" >Đơn vị chấm
                    </td>
                    <td style="text-align:center;font-weight:bold" >Phòng TCCB chấm
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr data-bind="if: SoCongNgoaiGioDonVi != SoCongNgoaiGio">
                    <td style="text-align:left">Ngày thường</td>
                    <td data-bind="text: SoCongNgoaiGioDonVi "></td>
                    <td data-bind="text: SoCongNgoaiGio "></td>
                </tr>
                <tr data-bind="if: SoCongNgoaiGioSau23GioDonVi != SoCongNgoaiGioSau23Gio">
                    <td style="text-align:left">Ngày thường sau 23h</td>
                    <td data-bind="text: SoCongNgoaiGioSau23GioDonVi "></td>
                    <td data-bind="text: SoCongNgoaiGioSau23Gio "></td>
                </tr>
                <tr data-bind="if: SoCongNgoaiGioT7CNDonVi != SoCongNgoaiGioT7CN">
                    <td style="text-align:left">T7/CN</td>
                    <td data-bind="text: SoCongNgoaiGioT7CNDonVi "></td>
                    <td data-bind="text: SoCongNgoaiGioT7CN "></td>
                </tr>
                <tr data-bind="if: SoCongNgoaiGioT7CNSau23GioDonVi != SoCongNgoaiGioT7CNSau23Gio">
                    <td style="text-align:left">T7/CN sau 23h</td>
                    <td data-bind="text: SoCongNgoaiGioT7CNSau23GioDonVi "></td>
                    <td data-bind="text: SoCongNgoaiGioT7CNSau23Gio "></td>
                </tr>
                <tr data-bind="if: SoCongNgoaiGioLeDonVi != SoCongNgoaiGioLe">
                    <td style="text-align:left">Ngày lễ</td>
                    <td data-bind="text: SoCongNgoaiGioLeDonVi "></td>
                    <td data-bind="text: SoCongNgoaiGioLe "></td>
                </tr>
                <tr data-bind="if: SoCongNgoaiGioLeSau23GioDonVi != SoCongNgoaiGioLeSau23Gio">
                    <td style="text-align:left">Ngày lễ sau 23h</td>
                    <td data-bind="text: SoCongNgoaiGioLeSau23GioDonVi "></td>
                    <td data-bind="text: SoCongNgoaiGioLeSau23Gio "></td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>
