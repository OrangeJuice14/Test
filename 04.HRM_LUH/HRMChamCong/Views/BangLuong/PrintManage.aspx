<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintManage.aspx.cs" Inherits="HRMChamCong.Views.BangLuong.PrintManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
   <script src="/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
<script src="/Components/jqwidgets/jqx-all.js" type="text/javascript"></script>
<link href="/Components/jqwidgets/jqx.base.css" rel="stylesheet" />
<link href="/Components/jqwidgets/jqx.darkBlue.css" rel="stylesheet" />
<script src="/Scripts/knockout-3.2.0.js"></script>
<script src="/Components/jqwidgets/jqxcore.js"></script>
<script src="/Components/jqwidgets/jqxknockout.js"></script>
<script type="text/javascript">
    function numberWithCommas(x) {
        x = Math.round(x);
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    function formatDate(date) {
        return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
    }
    function numberToFixed(num) {
        return parseFloat(num).toFixed(2);
    }
    
    $(function () {
        function ViewModel() {
            var self = this;
            var date = new Date();
            self.NgayHienTai = date.getDate() + '/' + (date.getMonth() + 1) + '/' + date.getFullYear();
            self.BangLuong = ko.observableArray();
            self.MaNhanSu = ko.observable();
            self.HoTen = ko.observable();
            self.DonVi = ko.observable();
            self.TieuDe = ko.observable();
            self.Sotien = ko.observableArray();
            self.Ghichu = ko.observable();
            self.ThangNam = ko.observable();
            self.BangThongTinThuNhap_LuongNganSach = ko.observableArray();
            self.BangThongTinThuNhap_LuongTangThem = ko.observableArray();
            self.BangThongTinThuNhap_Thue = ko.observableArray();
            self.BangThongTinThuNhap_ThuNhapKhac = ko.observableArray();
            self.BangThongTinThuNhap_TruyLinh = ko.observableArray();
            self.HSL = ko.observable();
            self.PCCV = ko.observable();
            self.PCVK = ko.observable();
            self.PCTN = ko.observable();
            self.PCCM = ko.observable();
            self.PCQL = ko.observable();
            self.PCKN1 = ko.observable();
            self.PCKN2 = ko.observable();
            self.PCDH = ko.observable();
            self.PCTNhiem = ko.observable();
            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/ThongTinNhanSu_BANGLUONG_Report',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: ko.toJSON({
                    webUserId: '<%#Request.QueryString["webUserId"] %>',
                    kyTinhLuong: '<%#Request.QueryString["kyTinhLuong"] %>'
                }),
                async: false,
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    // hien thi tháng năm
                    self.ThangNam = '<%#Request.QueryString["ThangNam"] %>';
                    //chi tiet thu nhap
                    if (data.ChiTietThuNhapCaNhan != null) {
                        self.MaNhanSu(data.ChiTietThuNhapCaNhan.MaNhanSu);
                        self.HoTen(data.ChiTietThuNhapCaNhan.HoTen);
                        self.DonVi(data.ChiTietThuNhapCaNhan.DonVi);
                    }
                    //BangThongTinThuNhap_LuongNganSach
                    self.BangThongTinThuNhap_LuongNganSach(data.BangThongTinThuNhap_LuongNganSach);                    
                    //BangThongTinThuNhap_LuongTangThem
                    self.BangThongTinThuNhap_LuongTangThem(data.BangThongTinThuNhap_LuongTangThem);
                    //BangThongTinThuNhap_Thue
                    self.BangThongTinThuNhap_Thue(data.BangThongTinThuNhap_Thue);
                    //BangThongTinThuNhap_ThuNhapKhac
                    self.BangThongTinThuNhap_ThuNhapKhac(data.BangThongTinThuNhap_ThuNhapKhac);
                    //BangThongTinThuNhap_TruyLinh
                    self.BangThongTinThuNhap_TruyLinh(data.BangThongTinThuNhap_TruyLinh);
                    //Chú thích hệ số
                    console.log(data.BangThongTinThuNhap_LuongNganSach);
                    self.HSL(data.BangThongTinThuNhap_HeSo.HSL);
                    self.PCCV(data.BangThongTinThuNhap_HeSo.PCCV);
                    self.PCVK(data.BangThongTinThuNhap_HeSo.PCVK);
                    self.PCTN(data.BangThongTinThuNhap_HeSo.PCTN);
                    self.PCCM(data.BangThongTinThuNhap_HeSo.PCCM);
                    self.PCQL(data.BangThongTinThuNhap_HeSo.PCQL);
                    self.PCKN1(data.BangThongTinThuNhap_HeSo.PCKN1);
                    self.PCKN2(data.BangThongTinThuNhap_HeSo.PCKN2);
                    self.PCDH(data.BangThongTinThuNhap_HeSo.PCDH);
                    self.PCTNhiem(data.BangThongTinThuNhap_HeSo.PCTNhiem);
                }
            });
           
           self.total_LuongNganSach = ko.computed(function(){
               var total = 0;
               for(var p = 0; p < self.BangThongTinThuNhap_LuongNganSach().length; ++p)
               {
                   total += self.BangThongTinThuNhap_LuongNganSach()[p].Sotien;
               }
               return numberWithCommas(total);
           });
           self.total_LuongTangThem = ko.computed(function () {
               var total = 0;
               for (var p = 0; p < self.BangThongTinThuNhap_LuongTangThem().length; ++p) {
                   total += self.BangThongTinThuNhap_LuongTangThem()[p].Sotien;
               }
               return numberWithCommas(total);
           });
           self.total_Thue = ko.computed(function () {
               var total = 0;
               for (var p = 0; p < self.BangThongTinThuNhap_Thue().length; ++p) {
                   total += self.BangThongTinThuNhap_Thue()[p].Sotien;
               }
               return numberWithCommas(total);
           });
           self.total_ThuNhapKhac = ko.computed(function () {
               var total = 0;
               for (var p = 0; p < self.BangThongTinThuNhap_ThuNhapKhac().length; ++p) {
                   total += self.BangThongTinThuNhap_ThuNhapKhac()[p].Sotien;
               }
               return numberWithCommas(total);
           });
           self.total_TruyLinh = ko.computed(function () {
               var total = 0;
               for (var p = 0; p < self.BangThongTinThuNhap_TruyLinh().length; ++p) {
                   total += self.BangThongTinThuNhap_TruyLinh()[p].Sotien;
               }
               return numberWithCommas(total);
           });
           self.total = ko.computed(function () {
               var total = 0;
               var total1 = 0;
               var total2 = 0;
               var total3 = 0;
               var total4 = 0;
               var total5 = 0;
               for (var p = 0; p < self.BangThongTinThuNhap_LuongNganSach().length; ++p) {
                   total1 += self.BangThongTinThuNhap_LuongNganSach()[p].Sotien;
               }
               for (var p = 0; p < self.BangThongTinThuNhap_LuongTangThem().length; ++p) {
                   total2 += self.BangThongTinThuNhap_LuongTangThem()[p].Sotien;
               }
               for (var p = 0; p < self.BangThongTinThuNhap_Thue().length; ++p) {
                   total3 += self.BangThongTinThuNhap_Thue()[p].Sotien;
               }
               for (var p = 0; p < self.BangThongTinThuNhap_ThuNhapKhac().length; ++p) {
                   total4 += self.BangThongTinThuNhap_ThuNhapKhac()[p].Sotien;
               }
               for (var p = 0; p < self.BangThongTinThuNhap_TruyLinh().length; ++p) {
                   total5 += self.BangThongTinThuNhap_TruyLinh()[p].Sotien;
               }
               total = total1 + total2 + total3 + total4 + total5;
               return numberWithCommas(total);
           })
        }

        var model = new ViewModel();
        ko.applyBindings(model, $("#bangLuong2")[0]); 
    });
</script>

<style type="text/css">

    .boldText {
        font-weight: bold; 
         padding:5px;
    }

    .textToCenter {
        text-align: center;
    }

    .textToRight {
        text-align: right;
    }
    .textToLeft{
        padding-left: 5px;
        text-align:left;
    }
    .tableHoten{
        margin-left: auto;
        margin-right: auto;
        width: 50%;
    }

    .backGroundTitle {
        background: darkgray;
    }
    .auto-style2 {
        width: 60%;
    }
    .auto-style3 {
        height: 92px;
        width: 595px;
    }
    .auto-style4 {
        width:16.66%;
        height: 5px;
        padding:0px;
        margin-top:-5px;
    }
    .auto-style5 {
        width: 40%;
    }
    #bangLuong2{
        width: 1000px;
        margin:0 auto;
    }    
    
</style>
</head>
<body>
    <div id="bangLuong2">
        <div style="text-align: center">
            <table style="width: 100%">
                <tr style="text-align: center; font-weight: bold; font-size: 14px;">
                    <td colspan="3">TRƯỜNG ĐẠI HỌC LUẬT TP.HỒ CHÍ MINH

                   <br />
                        <a>PHÒNG HÀNH CHÍNH TỔNG HỢP </a>
                    </td>
                    <td colspan="3">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
                   <br />
                        <a style="text-align: center; font-weight: bold;">Độc lập - Tự do - Hạnh phúc</a>
                    </td>

                </tr>
                <tr>
                    <td class="auto-style4"></td>
                    <td class="auto-style4" style="border-top: 1px solid black"></td>
                    <td class="auto-style4"></td>
                    <td class="auto-style4"></td>
                    <td class="auto-style4" style="border-top: 1px solid black"></td>
                    <td class="auto-style4"></td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center">
                        <a style="font-weight: bold; font-size: 20px">BẢNG THÔNG TIN THU NHẬP THÁNG </a><a style="font-weight: bold; font-size: 20px" data-bind="text: ThangNam"></a>
                        <table class="tableHoten">
                            <tr class="textToLeft">
                                <td style="font-weight: bold;" class="auto-style5">Họ và tên:</td>
                                <td><a data-bind="text: HoTen"></a></td>
                            </tr>
                            <tr class="textToLeft">
                                <td style="font-weight: bold" class="auto-style5">Đơn vị công tác:</td>
                                <td><a data-bind="text: DonVi"></a></td>
                            </tr>
                        </table>
                        <a></a>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table style="border-collapse: collapse; border-color: black;" border="1">
                <tr style="font-weight: bold" class="textToCenter">
                    <td style="width: 30px;">TT</td>
                    <td style="width: 250px;">Chi tiết</td>
                    <td style="width: 100px;">Số tiền</td>
                    <td style="width: 200px;">Ghi chú</td>
                </tr>
                <tr style="font-weight: bold">
                    <td class="textToCenter">1</td>
                    <td class="textToLeft">Bảng lương ngân sách</td>
                    <td class="textToRight" data-bind="text: total_LuongNganSach"></td>
                    <td></td>
                </tr>
                <tbody data-bind="foreach: BangThongTinThuNhap_LuongNganSach">
                    <tr>
                        <td class="textToCenter">-</td>
                        <td data-bind="text: TieuDe"></td>
                        <td class="textToRight" data-bind="text: numberWithCommas(Sotien)"></td>
                        <td class="textToLeft" data-bind="text: Ghichu"></td>
                    </tr>
                </tbody>
                <tr style="font-weight: bold">
                    <td class="textToCenter">2</td>
                    <td class="textToLeft">Bảng lương tăng thêm</td>
                    <td class="textToRight" data-bind="text: total_LuongTangThem"></td>
                    <td></td>
                </tr>
                <tbody data-bind="foreach: BangThongTinThuNhap_LuongTangThem">
                    <tr>
                        <td class="textToCenter">-</td>
                        <td data-bind="text: TieuDe"></td>
                        <td class="textToRight" data-bind="text: numberWithCommas(Sotien)"></td>
                        <td data-bind="text: Ghichu"></td>
                    </tr>
                </tbody>
                <tr style="font-weight: bold">
                    <td class="textToCenter">3</td>
                    <td class="textToLeft">Bảng lương truy lĩnh (thu)</td>
                    <td class="textToRight" data-bind="text: total_TruyLinh"></td>
                    <td></td>
                </tr>
                <tbody data-bind="foreach: BangThongTinThuNhap_TruyLinh">
                    <tr>
                        <td class="textToCenter">-</td>
                        <td data-bind="text: TieuDe"></td>
                        <td class="textToRight" data-bind="text: numberWithCommas(Sotien)"></td>
                        <td class="textToLeft" data-bind="text: Ghichu"></td>
                    </tr>
                </tbody>
                <tr style="font-weight: bold">
                    <td class="textToCenter">4</td>
                    <td class="textToLeft">Bảng thu nhập khác</td>
                    <td class="textToRight" data-bind="text: total_ThuNhapKhac"></td>
                    <td></td>
                </tr>
                <tbody data-bind="foreach: BangThongTinThuNhap_ThuNhapKhac">
                    <tr>
                        <td class="textToCenter">-</td>
                        <td data-bind="text: TieuDe"></td>
                        <td class="textToRight" data-bind="text: numberWithCommas(Sotien)"></td>
                        <td class="textToLeft" data-bind="text: Ghichu"></td>
                    </tr>
                </tbody>
                <tr style="font-weight: bold">
                    <td class="textToCenter">5</td>
                    <td class="textToLeft">Thông tin thuế</td>
                    <td class="textToRight" data-bind="text: total_Thue"></td>
                    <td></td>
                </tr>
                <tbody data-bind="foreach: BangThongTinThuNhap_Thue">
                    <tr>
                        <td class="textToCenter">-</td>
                        <td data-bind="text: TieuDe"></td>
                        <td class="textToRight" data-bind="text: numberWithCommas(Sotien)"></td>
                        <td class="textToLeft" data-bind="text: Ghichu"></td>
                    </tr>
                </tbody>
                <tr style="font-weight: bold">
                    <td></td>
                    <td class="textToLeft">Số tiền thực nhận</td>
                    <td class="textToRight" data-bind="text: total"></td>
                    <td></td>
                </tr>
            </table>

            <table style="width: 100%">
                <tr>
                    <td style="font-weight: bold; text-decoration: underline" class="auto-style2">Ghi chú:</td>
                    <td style="font-style: italic; text-align: center">TP Hồ Chí Minh, ngày <span data-bind="text: NgayHienTai"></span>
                    </td>
                </tr>
                <tr>
                    <td  class="auto-style2" style="vertical-align:top">
                        <div style="font-style: italic; text-align: justify; ">
                        Để biết thêm các khoản thu nhập hàng tháng và in sổ phụ, Thầy/Cô vui lòng liên lạc với ngân hàng NN&amp;PTNT, điện thoại: (848) 39400989, máy nhánh 187 hoặc (848) 39433756
                        </div>
                        <p style="font-style:italic; text-decoration:underline;margin:10px 0 5px 0">Chú thích</p>
                        <div>
                            <table style="width: 50%; float: left">
                                <tbody>
                                    <tr>
                                        <td style="width: 55%">Hệ số lương</td>
                                        <td style="width: 10%">HSL</td>
                                        <td style="width: 35%" data-bind="text: numberToFixed(HSL())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC chức vụ</td>
                                        <td>PCCV</td>
                                        <td data-bind="text: numberToFixed(PCCV())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC vượt khung</td>
                                        <td>PCVK</td>
                                        <td data-bind="text: numberToFixed(PCVK())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC thâm niên</td>
                                        <td>PCTN</td>
                                        <td data-bind="text: numberToFixed(PCTN())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC chuyên môn</td>
                                        <td>PCCM</td>
                                        <td data-bind="text: numberToFixed(PCCM())"></td>
                                    </tr>
                                </tbody>
                            </table>
                            <table style="width: 50%; float: left">
                                <tbody>
                                    <tr>
                                        <td style="width: 60%">Hệ số PC quản lý</td>
                                        <td style="width: 10%">HSQL</td>
                                        <td style="width: 30%" data-bind="text: numberToFixed(PCQL())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC kiêm nhiệm 1</td>
                                        <td>PCKN1</td>
                                        <td data-bind="text: numberToFixed(PCKN1())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC kiêm nhiệm 2</td>
                                        <td>PCKN2</td>
                                        <td data-bind="text: numberToFixed(PCKN2())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC độc hại</td>
                                        <td>PCDH</td>
                                        <td data-bind="text: numberToFixed(PCDH())"></td>
                                    </tr>
                                    <tr>
                                        <td>Hệ số PC trách nhiệm</td>
                                        <td>PCTN</td>
                                        <td data-bind="text: numberToFixed(PCTN())"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    </td>

                    <td style="text-align: center; font-weight: bold; vertical-align:top;">
                        <div style="padding:30px 0 15px 0">
                            TL HIỆU TRƯỞNG
                            <br />
                            TRƯỞNG PHÒNG HC-TH<br />
                        </div>
                        <p style="margin:100px 0">Phan Lê Hoàng Toàn</p>                        
                    </td>
                </tr>                
            </table>

        </div>
    </div>
    <a href="http://selectpdf.com/save-as-pdf">Save as Pdf</a>
</body>
</html>
