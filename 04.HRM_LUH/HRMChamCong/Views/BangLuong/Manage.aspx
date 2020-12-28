<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.BangLuong.Manage" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " ₫";
        }
        function formatDate(date) {
            return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
        }
        function ViewModel() {
            var self = this;
            self.BangLuong = ko.observableArray();
            self.BangLuongSelected = ko.observable();
            self.MaNhanSu = ko.observable();
            self.HoTen = ko.observable();
            self.MaQuanLy = ko.observable();
            self.DonVi = ko.observable();
            self.NgachLuong = ko.observable();
            self.BacLuong = ko.observable();
            self.HeSoLuong = ko.observable();
            self.MucLuongCoBan = ko.observable(0);
            self.XepLoai = ko.observable();
            self.TongThuNhap = ko.observable(0);
            self.ThuLaoGiangDay = ko.observable(0);
            self.TienLuongNganSach = ko.observable(0);
            self.ThuNhapTangThem = ko.observable(0);
            self.CacKhoangPhuCap = ko.observable(0);
            self.KhenThuong = ko.observable(0);
            self.TienNgoaiGio = ko.observable(0);
            self.ThuNhapKhac = ko.observable(0);
            self.KhauTruLuong = ko.observable(0);
            self.ThueTNCN = ko.observable(0);
            self.SoTienThucNhan = ko.observable(0);
            self.thuLaoGiangDay = ko.observableArray();
            self.chiTietPhuCap = ko.observableArray();
            self.khenThuongPhucLoi = ko.observableArray();
            self.tienNgoaiGio = ko.observableArray();
            self.thuNhapKhac = ko.observableArray();
            self.khauTruLuong = ko.observableArray();
            self.TNCN_TongThueCaNhan = ko.observable(0);
            self.TNCN_GiamTruGiaCanh = ko.observable(0);
            self.TNCN_GiamTruBanThan = ko.observable(0);
            self.TNCN_GiamTruNguoiPhuThuoc = ko.observable(0);
            self.TNCN_ThuNhapThueCaNhan = ko.observable(0);
            self.TNCN_ThuePhaiNop = ko.observable(0);
            self.ChonNam = ko.observableArray();
            self.ChonNamSelected = ko.observable();          

            $.ajax({
                type: 'POST',
                url: '/Services/ChamCongService.asmx/KyTinhLuong',
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var data = $.parseJSON(result.d);
                    var arr = new Array();
                    data = $.Enumerable.From(data).Where(function (value) {
                        return value.ChonNam =  value.Nam;
                    }).OrderBy(function (value) {
                        return value.Nam;
                    }).Distinct(function (value) {
                        return value.Nam;
                    }).ToArray();
                    self.ChonNam(data);
                }
            });
            self.ChonNamSelected.subscribe(function (year) {
                var Year = $('#selectNam').find('option:selected').val();
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/KyTinhLuong_ByYear',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: ko.toJSON({
                        year: Year
                    }),
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        var arr = new Array();
                        data = $.Enumerable.From(data).Where(function (value) {
                            return value.Name = 'Bảng lương tháng ' + value.Thang + '/' + value.Nam;
                        }).OrderBy(function (value) {
                            return value.Thang;
                        }).ToArray();
                        self.BangLuong(data);
                    }
                });

            });

            self.BangLuongSelected.subscribe(function (newValue) {
           
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ThongTinNhanSu_BANGLUONG',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>',
                        kyTinhLuong: newValue,
                        loaiLuong: 1
                    }),
                    dataType: "json",
                    success: function (result) {
                        var data = $.parseJSON(result.d);                       
                        //ChiTietThuNhapCaNhan
                        if (data.ChiTietThuNhapCaNhan != null) {
                            self.MaNhanSu(data.ChiTietThuNhapCaNhan.MaNhanSu);
                            self.HoTen(data.ChiTietThuNhapCaNhan.HoTen);
                            self.DonVi(data.ChiTietThuNhapCaNhan.DonVi);
                            self.NgachLuong(data.ChiTietThuNhapCaNhan.NgachLuong);
                            self.BacLuong(data.ChiTietThuNhapCaNhan.BacLuong);
                            self.HeSoLuong(data.ChiTietThuNhapCaNhan.HeSoLuong);
                            self.MucLuongCoBan(data.ChiTietThuNhapCaNhan.LuongCoBan);
                            self.XepLoai(data.ChiTietThuNhapCaNhan.XepLoai);
                            self.TongThuNhap(data.ChiTietThuNhapCaNhan.TongThuNhap);
                            self.ThuLaoGiangDay(data.ChiTietThuNhapCaNhan.ThuLaoGiangDay);
                            self.TienLuongNganSach(data.ChiTietThuNhapCaNhan.LuongNganSach);
                            self.ThuNhapTangThem(data.ChiTietThuNhapCaNhan.ThuNhapTangThem);
                            self.CacKhoangPhuCap(data.ChiTietThuNhapCaNhan.PhuCap);
                            self.KhenThuong(data.ChiTietThuNhapCaNhan.KhenThuong);
                            self.TienNgoaiGio(data.ChiTietThuNhapCaNhan.NgoaiGio);
                            self.ThuNhapKhac(data.ChiTietThuNhapCaNhan.ThuNhapKhac);
                            self.KhauTruLuong(data.ChiTietThuNhapCaNhan.KhauTruLuong);
                            self.ThueTNCN(data.ChiTietThuNhapCaNhan.ThueTNCN);
                            self.SoTienThucNhan(data.ChiTietThuNhapCaNhan.ThucNhan);
                        }
                        if (data.HoSo != null) {
                            self.MaQuanLy(data.HoSo.MaQuanLy);                          
                        }
                        //DanhSach_ChiTietThuLaoGiangDay
                        self.thuLaoGiangDay(data.DanhSach_ChiTietThuLaoGiangDay);
                        //DanhSach_ChiTietPhuCap
                        self.chiTietPhuCap(data.DanhSach_ChiTietPhuCap);
                        //DanhSach_ChiTietKhenThuongPhucLoi
                        self.khenThuongPhucLoi(data.DanhSach_ChiTietKhenThuongPhucLoi);
                        //DanhSach_ChiTietNgoaiGio
                        self.tienNgoaiGio(data.DanhSach_ChiTietNgoaiGio);
                        //DanhSach_ChiTietThuNhapKhac
                        self.thuNhapKhac(data.DanhSach_ChiTietThuNhapKhac);
                        //DanhSach_ChiTietKhauTruLuong
                        self.khauTruLuong(data.DanhSach_ChiTietKhauTruLuong);
                        //DanhSach_ChiTietThueTNCN     
                        if (data.ChiTietThueTNCN != null) {
                            self.TNCN_TongThueCaNhan(data.ChiTietThueTNCN.TongThuNhapChiuThue);
                            self.TNCN_GiamTruGiaCanh(data.ChiTietThueTNCN.GiamTruGiaCanh);
                            self.TNCN_GiamTruBanThan(data.ChiTietThueTNCN.GiamTruBanThan);
                            self.TNCN_GiamTruNguoiPhuThuoc(data.ChiTietThueTNCN.GiamTruNguoiPhuThuoc);
                            self.TNCN_ThuNhapThueCaNhan(data.ChiTietThueTNCN.ThuNhapTinhThue);
                            self.TNCN_ThuePhaiNop(data.ChiTietThueTNCN.ThueTNCNPhaiNop);
                        }
                    }
                });
            });
            self.Sum_SoTien = function (value, arr) {
                if (arr() == undefined || arr().length == 0)
                    return "";
                return $.Enumerable.From(arr()).Sum(function (x) {
                    return x.SoTien;
                });

            };
            self.Sum_KhongTinhThue = function (value, arr) {
                if (arr() == undefined || arr().length == 0)
                    return "";

                return $.Enumerable.From(arr()).Sum(function (x) {
                    return x.KhongTinhThue;
                });
            };
           
        }
        ViewModel.prototype = {
            print: function () {
                var valueDate = $("#select").find('option:selected').text();
                var ThangNam = valueDate.substr(19, 100).replace('/', ' NĂM ');
                var webUserId = '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>';
                var kytinhluong = $("#select").find(":selected").val();
                var url = "/Views/BangLuong/PrintManage.aspx?webUserId=" + webUserId + "&kyTinhLuong=" + kytinhluong + "&ThangNam=" + ThangNam;
                window.open(url, '_blank', 'left=100,top=100,width=1000,height=800,toolbar=1,resizable=0');
            }
        }
     
        $(function () {
            var model = new ViewModel();
            ko.applyBindings(model, $("#bangLuong")[0]);
        });
    </script>

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
            background: darkgray;
        }
        #select{
            float:left;
        }
        #selectNam{
            float:left;
        }
        #title-bangluong
        {
            float:left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="bangLuong">
        <div id="title-bangluong">
 <a href="#">Bảng lương và phụ cấp : </a> &nbsp&nbsp
        </div>         
        <div id="selectNam" >
                        <select data-bind="options: ChonNam, optionsText: 'ChonNam', optionsValue: 'ChonNam', value: ChonNamSelected, optionsCaption: '-- Chọn năm --'"></select>
            &nbsp&nbsp
        </div>
        <div id="select">
          
            <select data-bind="options: BangLuong, optionsText: 'Name', optionsValue: 'Oid', value: BangLuongSelected, optionsCaption: '-- Chọn bảng lương --'"></select>
            <input class="btn btn-primary" type="button" value="Print" data-bind="click: print"/>
        </div>

        <div data-bind="visible: BangLuongSelected() != undefined" style="clear:both">
            <p style="padding: 10px 0px 10px 0px;">
                    Mã nhân sự: <b data-bind="text: MaQuanLy"></b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Họ và tên: <b data-bind="    text: HoTen"></b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Đơn vị: <b data-bind="    text: DonVi"></b>
                </p>
            <%--<div>                
                <table style="border-collapse: collapse;" border="1" width="100%">
                    <tr class="boldText textToCenter">
                        <td></td>
                        <td colspan="2">Diễn giải</td>
                        <td width="150px">Số tiền</td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" class="boldText">Thông tin lương</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="300px">Ngạch lương</td>
                        <td data-bind="text: NgachLuong"></td>
                        <td rowspan="5"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="300px">Bậc lương</td>
                        <td data-bind="text: BacLuong"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="300px">Hệ số lương</td>
                        <td data-bind="text: HeSoLuong"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="300px">Mức lương cơ bản</td>
                        <td data-bind="text: numberWithCommas(MucLuongCoBan())"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td width="300px">Xếp loại</td>
                        <td data-bind="text: XepLoai"></td>
                    </tr>

                    <tr class="boldText">
                        <td class="textToCenter">I</td>
                        <td colspan="2" class="boldText">Tổng thu nhập(=1 + 2 + 3 + 4 + 5 + 6)</td>
                        <td data-bind="text: numberWithCommas(TongThuNhap())" class="textToRight"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">1</td>
                        <td colspan="2">Thù lao giảng dạy</td>
                        <td data-bind="text: numberWithCommas(ThuLaoGiangDay())" class="textToRight">></td>
                    </tr>
                    <tr>
                        <td class="textToRight">2</td>
                        <td colspan="2">Tiền lương ngân sách ((Hệ số lương + Hệ số chức vụ) x</td>
                        <td data-bind="text: numberWithCommas(TienLuongNganSach())" class="textToRight">></td>
                    </tr>

                    <tr>
                        <td class="textToRight">3</td>
                        <td colspan="2">Thu nhập tăng thêm [(Hệ số lương + Hệ số chức vụ) x</td>
                        <td data-bind="text: numberWithCommas(ThuNhapTangThem())" class="textToRight">></td>
                    </tr>
                    <tr>
                        <td class="textToRight">4</td>
                        <td colspan="2">Các khoản phụ cấp</td>
                        <td data-bind="text: numberWithCommas(CacKhoangPhuCap())" class="textToRight">></td>
                    </tr>
                    <tr>
                        <td class="textToRight">5</td>
                        <td colspan="2">Khen thưởng - Phúc lợi</td>
                        <td data-bind="text: numberWithCommas(KhenThuong())" class="textToRight">></td>
                    </tr>
                    <tr>
                        <td class="textToRight">6</td>
                        <td colspan="2">Tiền ngoài giờ</td>
                        <td data-bind="text: numberWithCommas(TienNgoaiGio())" class="textToRight">></td>
                    </tr>
                    <tr>
                        <td class="textToRight">7</td>
                        <td colspan="2">Thu nhập khác</td>
                        <td data-bind="text: numberWithCommas(ThuNhapKhac())" class="textToRight">></td>
                    </tr>
                    <tr class="boldText">
                        <td class="textToCenter">II</td>
                        <td colspan="2" class="boldText">Khấu trừ lương</td>
                        <td data-bind="text: numberWithCommas(KhauTruLuong())" class="textToRight">></td>
                    </tr>
                    <tr class="boldText">
                        <td class="textToCenter">III</td>
                        <td colspan="2" class="boldText">Thuế TNCN phải nộp</td>
                        <td data-bind="text: numberWithCommas(ThueTNCN())" class="textToRight">></td>
                    </tr>
                    <tr class="boldText">
                        <td class="textToCenter">IV</td>
                        <td colspan="2" class="boldText">Số tiền thực nhận (=I - II - III)</td>
                        <td data-bind="text: numberWithCommas(SoTienThucNhan())" class="textToRight">></td>
                    </tr>

                </table>
                <br />
                <div data-bind="if: thuLaoGiangDay().length > 0">
                    <h3>CHI TIẾT THÙ LAO GIẢNG DẠY:</h3>
                    <table border="1" style="border-collapse: collapse;" width="100%">
                        <tr class="backGroundTitle">
                            <th style="width: 2%;"></th>
                            <th style="width: 13%;">Ngày</th>
                            <th style="width: 35%;">Nội dung</th>
                            <th style="width: 25%;">Số tiền</th>
                            <th style="width: 25%;">Không tính thuế</th>
                        </tr>
                        <tbody data-bind="foreach: thuLaoGiangDay">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToCenter"></td>
                                <td data-bind="text: numberWithCommas(KhongTinhThue)" class="textToCenter"></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" class="textToCenter boldText">Tổng số</td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), thuLaoGiangDay))"></td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_KhongTinhThue(BangLuongSelected(), thuLaoGiangDay))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>
                <div data-bind="if: chiTietPhuCap().length > 0">
                    <h3>CHI TIẾT PHỤ CẤP:</h3>
                    <table border="1" style="border-collapse: collapse;" width="100%">
                        <tr class="backGroundTitle">
                            <th style="width: 2%;"></th>
                            <th style="width: 13%;">Ngày</th>
                            <th style="width: 35%;">Nội dung</th>
                            <th style="width: 25%;">Số tiền</th>
                            <th style="width: 25%;">Không tính thuế</th>
                        </tr>
                        <tbody data-bind="foreach: chiTietPhuCap">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToCenter"></td>
                                <td data-bind="text: numberWithCommas(KhongTinhThue)" class="textToCenter"></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" class="textToCenter boldText">Tổng số</td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), chiTietPhuCap))"></td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_KhongTinhThue(BangLuongSelected(), chiTietPhuCap))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>

                <div data-bind="if: khenThuongPhucLoi().length > 0">
                    <h3>CHI TIẾT KHEN THƯỞNG - PHÚC LỢI:</h3>
                    <table border="1" style="border-collapse: collapse;" width="100%">
                        <tr class="backGroundTitle">
                            <th style="width: 2%;"></th>
                            <th style="width: 13%;">Ngày</th>
                            <th style="width: 35%;">Nội dung</th>
                            <th style="width: 25%;">Số tiền</th>
                            <th style="width: 25%;">Không tính thuế</th>
                        </tr>
                        <tbody data-bind="foreach: khenThuongPhucLoi">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToCenter"></td>
                                <td data-bind="text: numberWithCommas(KhongTinhThue)" class="textToCenter"></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" class="textToCenter boldText">Tổng số</td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), khenThuongPhucLoi))"></td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_KhongTinhThue(BangLuongSelected(), khenThuongPhucLoi))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>

                <div data-bind="if: tienNgoaiGio().length > 0">
                    <h3>CHI TIẾT TIỀN NGOÀI GIỜ:</h3>
                    <table border="1" style="border-collapse: collapse;" width="100%">
                        <tr class="backGroundTitle">
                            <th style="width: 2%;"></th>
                            <th style="width: 13%;">Ngày</th>
                            <th style="width: 35%;">Nội dung</th>
                            <th style="width: 25%;">Số tiền</th>
                            <th style="width: 25%;">Không tính thuế</th>
                        </tr>
                        <tbody data-bind="foreach: tienNgoaiGio">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToCenter"></td>
                                <td data-bind="text: numberWithCommas(KhongTinhThue)" class="textToCenter"></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" class="textToCenter boldText">Tổng số</td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), tienNgoaiGio))"></td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_KhongTinhThue(BangLuongSelected(), tienNgoaiGio))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>

                <div data-bind="if: thuNhapKhac().length > 0">
                    <h3>CHI TIẾT THU NHẬP KHÁC:</h3>
                    <table border="1" style="border-collapse: collapse;" width="100%">
                        <tr class="backGroundTitle">
                            <th style="width: 2%;"></th>
                            <th style="width: 13%;">Ngày</th>
                            <th style="width: 35%;">Nội dung</th>
                            <th style="width: 25%;">Số tiền</th>
                            <th style="width: 25%;">Không tính thuế</th>
                        </tr>
                        <tbody data-bind="foreach: thuNhapKhac">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToCenter"></td>
                                <td data-bind="text: numberWithCommas(KhongTinhThue)" class="textToCenter"></td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="3" class="textToCenter boldText">Tổng số</td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), thuNhapKhac))"></td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_KhongTinhThue(BangLuongSelected(), thuNhapKhac))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>

                <div data-bind="if: khauTruLuong().length > 0">
                    <h3>CHI TIẾT KHẤU TRỪ LƯƠNG:</h3>
                    <table border="1" style="border-collapse: collapse;" width="100%">
                        <tr class="backGroundTitle">
                            <th style="width: 2%;"></th>
                            <th style="width: 13%;">Ngày</th>
                            <th style="width: 35%;">Nội dung</th>
                            <th style="width: 25%;">Tỷ lệ</th>
                            <th style="width: 25%;">Số tiền</th>

                        </tr>
                        <tbody data-bind="foreach: khauTruLuong">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai"></td>
                                <td data-bind="text: TyLe" class="textToCenter"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToCenter"></td>

                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="4" class="textToCenter boldText">Tổng số</td>
                                <td class="textToCenter boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), khauTruLuong))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>
                
                <h3>CHI TIẾT THUẾ TNCN:</h3>
                <table width="70%" style="border-collapse: collapse;" border="1">
                    <tr>
                        <td style="width: 10%" class="textToRight">1.</td>
                        <td style="width: 50%">Tổng thu nhập chịu thuế thu nhập cá nhân</td>
                        <td style="width: 40%" class="textToRight" width="150px" data-bind="text: numberWithCommas(TNCN_TongThueCaNhan())"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">2.</td>
                        <td>Giảm trừ gia cảnh</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_GiamTruGiaCanh())"></td>
                    </tr>

                    <tr>
                        <td class="textToRight">a.</td>
                        <td>Giảm trừ bản thân</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_GiamTruBanThan())"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">b.</td>
                        <td>Giảm trừ người phụ thuộc</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_GiamTruNguoiPhuThuoc())"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">3.</td>
                        <td>Thu nhập tính thuế thu nhập cá nhân (9a - 9b - 7a)</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_ThuNhapThueCaNhan())"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">4.</td>
                        <td>Số thuế thu nhập cá nhân phải nộp</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_ThuePhaiNop())"></td>
                    </tr>
                </table>
            </div>--%>
        </div>
    </div>
   <div id='jqxwindow'>
<div></div>
</div> 
</asp:Content>
