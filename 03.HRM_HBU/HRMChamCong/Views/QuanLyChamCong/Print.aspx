<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Print.aspx.cs" Inherits="HRMChamCong.Views.QuanLyChamCong.Print" %>

<%@ Import Namespace="HRMChamCong.Helper" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Components/jqwidgets/jqx-all.js" type="text/javascript"></script>
    <link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
    <link href="/Components/jqwidgets/jqx.darkBlue.css" rel="stylesheet" />
    <script src="/Scripts/knockout-3.2.0.js"></script>
    <script src="/Components/jqwidgets/jqxcore.js"></script>
    <script src="/Components/jqwidgets/jqxknockout.js"></script>
    <script type="text/javascript">
        function ViewModel() {
            var self = this;
            self.TenDonVi = "";
            self.MaQuanLy = "";
            self.HoTen = "";
            self.ChucDanh = "";
            self.Nam = "";
            self.NamTruoc = "";
            self.NamSau = "";
            self.NgayVaoCoQuan = "";
            self.PhepConLai = "";
            self.LamViec5Den10Nam = "";
            self.LamViecTren10Nam = "";
            self.PhepNamNay = "";
            self.TongSoNgayPhepDuocNghi = "";
            self.TongSoPhepDaNghiTrongNam = "";
            self.SoPhepChuyenSangNamSau = "";
            self.ChiTietNghiPhep = ko.observableArray();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/QuanLyNghiPhep_InGiayNghiPhepNam',
                contentType: "application/json; charset=utf-8",
                data: ko.toJSON({
                    NhanVien: '<%#Request.QueryString["NhanVien"] %>',
                    Nam: '<%#Request.QueryString["Nam"] %>'
                }),
                dataType: "json",
                async: false,
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    self.TenDonVi = data.TenDonVi;
                    self.MaQuanLy = data.MaQuanLy;
                    self.HoTen = data.HoTen;
                    self.ChucDanh = data.ChucDanh;
                    self.Nam = data.Nam;
                    self.NamTruoc = data.Nam - 1;
                    self.NamSau = data.Nam + 1;
                    self.NgayVaoCoQuan = data.NgayVaoCoQuan;
                    self.PhepConLai = data.PhepConLai;
                    self.LamViec5Den10Nam = data.LamViec5Den10Nam;
                    self.LamViecTren10Nam = data.LamViecTren10Nam;
                    self.PhepNamNay = data.PhepNamNay;
                    self.TongSoNgayPhepDuocNghi = data.TongSoNgayPhepDuocNghi;
                    self.TongSoPhepDaNghiTrongNam = data.TongSoPhepDaNghiTrongNam;
                    self.SoPhepChuyenSangNamSau = data.SoPhepChuyenSangNamSau;
                    self.ChiTietNghiPhep(data.ChiTietNghiPhep);
                },
                error: function () {
                    alert("Lỗi!");
                    window.close();
                }
            });
        }
        $(function () {
            var self = this;
            view = new ViewModel();
            ko.applyBindings(view, $("#chamcong_detail")[0]);
            window.print();
        });
    </script>
    <style>
        @page {
            size: 21cm 29.7cm;
            margin: 15mm 10mm 15mm 15mm;
        }

        table {
            border-spacing: 0;
            border-collapse: collapse;
        }

        th, td {
            border: 1px solid;
        }

        .cenmid {
            text-align: center;
            vertical-align: middle;
        }

        .cenmidbold {
            text-align: center;
            vertical-align: middle;
            font-weight: bold;
        }

        .font10pt {
            font-size: 10pt;
        }

        .font9pt {
            font-size: 9pt;
        }

        .padding5 {
            padding: 5px;
        }
    </style>
</head>
<body>
    <div id="chamcong_detail" style="width: 185mm">
        <div style="font-size: 16pt; font-weight: bold; padding-bottom: 10px; text-align: center">ĐƠN XIN NGHỈ PHÉP NĂM</div>
        <div style="text-align: center; padding-top: 20px;">
            <div style="font-weight: bold; padding-bottom: 5px; text-align: left;">Đơn vị :<span data-bind="text: TenDonVi"></span></div>
            <table style="width: 100%">
                <tr>
                    <td rowspan="2" class="cenmid font10pt" style="width: 65px">MSNV</td>
                    <td data-bind="text: MaQuanLy" rowspan="2" class="cenmidbold"></td>
                    <td class="font10pt" style="padding: 5px; width: 100px;">Họ tên</td>
                    <td data-bind="text: HoTen" class="cenmidbold"></td>
                    <td class="font10pt" style="padding: 5px">Năm</td>
                    <td data-bind="text: Nam" class="cenmidbold"></td>
                </tr>
                <tr>
                    <td class="font10pt" style="padding: 5px">Chức danh</td>
                    <td data-bind="text: ChucDanh" class="cenmidbold"></td>
                    <td class="font10pt" style="padding: 5px">Ngày vào</td>
                    <td data-bind="text: NgayVaoCoQuan" class="cenmidbold"></td>
                </tr>
            </table>
            <table style="width: 100%; margin-top: 10px">
                <tr>
                    <td rowspan="2" class="cenmid font10pt" style="width: 100px;">PHÉP NĂM</td>
                    <td class="font9pt cenmid" style="padding: 5px;width: 150px;">Phép năm còn lại năm <span data-bind="text: NamTruoc"></span>
                        <br />
                        (20/12/<span data-bind="text: NamTruoc"></span>)</td>
                    <td class="font9pt cenmid">Làm việc từ 5 năm đến 10 năm</td>
                    <td class="font9pt cenmid">Làm việc từ 10 năm trở lên</td>
                    <td class="font9pt cenmid"  style="width: 130px;">Phép năm năm <span data-bind="text: Nam"></span></td>
                    <td class="font9pt cenmidbold"  style="width: 120px;">Tổng số ngày phép được nghỉ năm <span data-bind="text: Nam"></span></td>
                </tr>
                <tr>
                    <td data-bind="text: PhepConLai" style="padding:5px" class="cenmidbold"></td>
                    <td data-bind="text: LamViec5Den10Nam" class="cenmidbold"></td>
                    <td data-bind="text: LamViecTren10Nam" class="cenmidbold"></td>
                    <td data-bind="text: PhepNamNay" class="cenmidbold"></td>
                    <td data-bind="text: TongSoNgayPhepDuocNghi" class="cenmidbold"></td>
                </tr>
                <tr>
                    <td class="font10pt cenmidbold" colspan="2">Tổng số ngày phép đã nghỉ trong năm <span data-bind="text: Nam"></span></td>
                    <td data-bind="text: TongSoPhepDaNghiTrongNam" colspan="2" class="cenmidbold"></td>
                    <td class="font10pt cenmidbold" >Số ngày phép còn lại chuyển sang năm  <span data-bind="text: NamSau"></span></td>
                    <td data-bind="text: SoPhepChuyenSangNamSau" class="cenmidbold"></td>
                </tr>
            </table>
            <table style="width: 100%; margin-top: 10px">
                <tr>
                    <td colspan="2" class="font9pt cenmid">Ngày nghỉ
                    </td>
                    <td rowspan="2" class="font9pt cenmid">Số ngày xin nghỉ
                    </td>
                    <td rowspan="2" class="font9pt cenmid">Số ngày còn lại
                    </td>
                    <td rowspan="2" class="font9pt cenmid">Lý do
                    </td>
                    <td rowspan="2" class="font9pt cenmid">Người xin nghỉ
                    </td>
                    <td rowspan="2" class="font9pt cenmid">Người kiểm tra
                    </td>
                    <td rowspan="2" class="font9pt cenmid">Trưởng đơn vị
                    </td>
                    <td colspan="2" class="font9pt cenmid" style="padding:5px">P.TCHC xác nhận
                    </td>
                </tr>
                <tr>
                    <td class="font9pt cenmid" style="padding:5px">Từ
                    </td>
                    <td class="font9pt cenmid">Đến
                    </td>
                    <td class="font9pt cenmid">Ngày
                    </td>
                    <td class="font9pt cenmid">Ký
                    </td>
                </tr>
                <tbody data-bind="foreach: ChiTietNghiPhep">
                    <tr>
                        <td class="font9pt cenmid"style="width:50px" data-bind="text: TuNgay"></td>
                        <td class="font9pt cenmid"style="width:50px" data-bind="text: DenNgay"></td>
                        <td class="font9pt cenmid" style="width:50px" data-bind="text: SoNgayXinNghi"></td>
                        <td class="font9pt cenmid" style="width:50px" data-bind="text: SoNgayConLai"></td>
                        <td class="font9pt"style="text-align:left;padding:5px;" data-bind="text: LyDo"></td>
                        <td class="font9pt cenmid" style="width:60px"></td>
                        <td class="font9pt cenmid" style="width:60px"></td>
                        <td class="font9pt cenmid" style="width:60px"></td>
                        <td class="font9pt cenmid" style="width:60px"></td>
                        <td class="font9pt cenmid" style="width:60px"></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>
