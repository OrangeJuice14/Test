<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage_IUH.aspx.cs" Inherits="HRMChamCong.Views.BangLuong.Manage_IUH" %>

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
            var loaiLuongArr = [
                { Id: null, Name: "Tất cả" },
                { Id: 0, Name: "Lương kỳ 1" },
                { Id: 1, Name: "Lương kỳ 2" },
                { Id: 2, Name: "Lương Tiến sĩ" },
                { Id: 3, Name: "Lương thử việc" }
            ];
            var self = this;
            self.BangLuong = ko.observableArray();
            self.LoaiLuong = ko.observableArray(loaiLuongArr);
            self.LoaiLuongSelected = ko.observable();
            self.BangLuongSelected = ko.observable();
            self.MaNhanSu = ko.observable();
            self.HoTen = ko.observable();
            self.DonVi = ko.observable();
            self.NgachLuong = ko.observable();
            self.BacLuong = ko.observable();
            self.HeSoLuong = ko.observable();
            self.LuongCoBan = ko.observable(0);
            self.LuongKy1 = ko.observable(0);
            self.PhuCapTrachNhiem = ko.observable(0);
            self.PhuCapThamNien = ko.observable(0);
            self.LuongKy2 = ko.observable(0);
            self.PhuCapTienSi = ko.observable(0);
            self.LuongThuViec = ko.observable(0);
            self.TongThuNhap = ko.observable(0);
            self.ThuNhapKhac = ko.observable(0);
            self.KhauTruLuong = ko.observable(0);
            self.ThueTNCN = ko.observable(0);
            self.SoTienThucNhan = ko.observable(0);
            self.khauTruLuong = ko.observableArray();
            self.TNCN_TongThueCaNhan = ko.observable(0);
            self.TNCN_GiamTruGiaCanh = ko.observable(0);
            self.TNCN_GiamTruBanThan = ko.observable(0);
            self.TNCN_GiamTruNguoiPhuThuoc = ko.observable(0);
            self.TNCN_ThuNhapThueCaNhan = ko.observable(0);
            self.TNCN_ThuePhaiNop = ko.observable(0);
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
                        return value.Name = 'Bảng lương tháng ' + value.Thang + '/' + value.Nam;
                    }).OrderBy(function (value) {
                        return value.Thang;
                    }).ToArray();
                    self.BangLuong(data);
                }
            });


            self.BangLuongSelected.subscribe(
                function (newValue) {
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ThongTinNhanSu_BANGLUONG',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>',
                        kyTinhLuong: newValue,
                        loaiLuong: self.LoaiLuongSelected()
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
                            self.LuongCoBan(data.ChiTietThuNhapCaNhan.LuongCoBan);
                            self.LuongKy1(data.ChiTietThuNhapCaNhan.LuongKy1);
                            self.PhuCapTrachNhiem(data.ChiTietThuNhapCaNhan.PhuCapTrachNhiem);
                            self.PhuCapThamNien(data.ChiTietThuNhapCaNhan.PhuCapThamNien);
                            self.PhuCapTienSi(data.ChiTietThuNhapCaNhan.PhuCapTienSi);
                            self.LuongKy2(data.ChiTietThuNhapCaNhan.LuongKy2);
                            self.LuongThuViec(data.ChiTietThuNhapCaNhan.LuongThuViec);
                            self.TongThuNhap(data.ChiTietThuNhapCaNhan.TongThuNhap);
                            self.ThuNhapKhac(data.ChiTietThuNhapCaNhan.ThuNhapKhac);
                            self.KhauTruLuong(data.ChiTietThuNhapCaNhan.KhauTruLuong);
                            self.ThueTNCN(data.ChiTietThuNhapCaNhan.ThueTNCN);
                            self.SoTienThucNhan(data.ChiTietThuNhapCaNhan.ThucNhan);
                        }
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
            self.LoaiLuongSelected.subscribe(
               function (newValue) {
                   $.ajax({
                       type: 'POST',
                       url: '/Services/ChamCongService.asmx/ThongTinNhanSu_BANGLUONG',
                       async: false,
                       contentType: "application/json; charset=utf-8",
                       data: ko.toJSON({
                           webUserId: '<%#HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()]%>',
                           loaiLuong: newValue,
                           kyTinhLuong: self.BangLuongSelected()
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
                            self.LuongCoBan(data.ChiTietThuNhapCaNhan.LuongCoBan);
                            self.LuongKy1(data.ChiTietThuNhapCaNhan.LuongKy1);
                            self.PhuCapTrachNhiem(data.ChiTietThuNhapCaNhan.PhuCapTrachNhiem);
                            self.PhuCapThamNien(data.ChiTietThuNhapCaNhan.PhuCapThamNien);
                            self.PhuCapTienSi(data.ChiTietThuNhapCaNhan.PhuCapTienSi);
                            self.LuongKy2(data.ChiTietThuNhapCaNhan.LuongKy2);
                            self.LuongThuViec(data.ChiTietThuNhapCaNhan.LuongThuViec);
                            self.TongThuNhap(data.ChiTietThuNhapCaNhan.TongThuNhap);
                            self.ThuNhapKhac(data.ChiTietThuNhapCaNhan.ThuNhapKhac);
                            self.KhauTruLuong(data.ChiTietThuNhapCaNhan.KhauTruLuong);
                            self.ThueTNCN(data.ChiTietThuNhapCaNhan.ThueTNCN);
                            self.SoTienThucNhan(data.ChiTietThuNhapCaNhan.ThucNhan);
                        }
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
        $(function () {
            var model = new ViewModel();
            ko.applyBindings(model, $("#bangLuong")[0]);
        });
    </script>
    <style type="text/css">
        .boldText
        {
            font-weight: bold;
            padding:5px;
        }
        .boldTextCenter
        {
            font-weight: bold;
            padding:5px;
            text-align:center;
        }
        .textToCenter
        {
            text-align: center;
            padding:5px;
        }

        .textToRight
        {
            text-align: right;
            padding:5px;
        }

        .backGroundTitle
        {
            background: darkgray;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="bangLuong">
        <div class="row">
            <div class="col-lg-6 col-xs-12 col-sm-6" style="padding-bottom:5px;">
                <div class="col-lg-5 col-xs-5 col-sm-5" style="text-align:right; margin-top:5px; ">
                <a href="#">Loại lương  </a>
                    </div>
                <div class="col-lg-7 col-xs-7 col-sm-7">
                <select class="form-control" id="cbbloailuong" data-bind="options: LoaiLuong, optionsText: 'Name', optionsValue: 'Id', value: LoaiLuongSelected"></select><span>&nbsp</span>
                    </div>
            </div>
            <div class="col-lg-6 col-xs-12 col-sm-6" style="padding-bottom:5px;">
                 <div class="col-lg-5 col-xs-5 col-sm-5" style="text-align:right; margin-top:5px;">
                <a href="#">Bảng lương và phụ cấp  </a>
                     </div>
                <div class="col-lg-7 col-xs-7 col-sm-7">
                <select class="form-control" data-bind="options: BangLuong, optionsText: 'Name', optionsValue: 'Oid', value: BangLuongSelected, optionsCaption: '-- Chọn bảng lương --'"></select>
                    </div>
            </div>
        </div>

        <div data-bind="visible: (BangLuongSelected() != undefined && LoaiLuongSelected() != undefined) || (BangLuongSelected() != undefined && LoaiLuongSelected() == undefined) ">
            <div>
                <div class="row">
                    <div class="col-lg-1"></div>
                    <div class="col-lg-10 col-md-10 padding-bottom-10">
                        <div class="col-lg-4 col-md-4 padding-5">
                            Mã nhân sự: <b data-bind="text: MaNhanSu"></b>
                        </div>
                        <div class="col-lg-4 col-md-4 padding-5">
                            Họ và tên: <b data-bind="    text: HoTen"></b>
                        </div>
                        <div class="col-lg-4 col-md-4 padding-5">
                            Đơn vị: <b data-bind="    text: DonVi"></b>
                        </div>
                    </div>
                    <div class="col-lg-1"></div>
                </div>
                <table class="table table-bordered table-hover">
                    <thead>
                        <tr class="active">
                        <th style="width: 30px; padding:5px;"></th>
                        <th class="text-align-center padding-10">Diễn giải</th>
                        <th class="text-align-center" style="width:150px;">Số tiền</th>
                            </tr>
                    </thead>
                    
                    <tr>
                        <td></td>
                        <td class="boldText" style="width:300px;padding:10px;">Thông tin lương</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding:10px;">Ngạch lương</td>
                        <td data-bind="text: NgachLuong" class="textToRight"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding:10px;">Bậc lương</td>
                        <td data-bind="text: BacLuong" class="textToRight"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding:10px;">Hệ số lương</td>
                        <td data-bind="text: HeSoLuong" class="textToRight"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding:10px;">Lương cơ bản</td>
                        <td data-bind="text: numberWithCommas(LuongCoBan())" class="textToRight"></td>
                    </tr>
                    <tr class="boldText">
                        <td class="textToCenter">I</td>
                        <td class="boldText">Tổng thu nhập(=1 + 2 + 3 + 4 + 5 + 6 + 7)</td>
                        <td data-bind="text: numberWithCommas(TongThuNhap())" class="textToRight"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">1</td>
                        <td style="padding:10px;">Lương kỳ 1</td>
                        <td data-bind="text: numberWithCommas(LuongKy1())" class="textToRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="textToRight">2</td>
                        <td style="padding:10px;">Phụ cấp trách nhiệm</td>
                        <td data-bind="text: numberWithCommas(PhuCapTrachNhiem())" class="textToRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="textToRight">3</td>
                        <td style="padding:10px;">Phụ cấp thâm niên</td>
                        <td data-bind="text: numberWithCommas(PhuCapThamNien())" class="textToRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="textToRight">4</td>
                        <td style="padding:10px;">Lương kỳ 2</td>
                        <td data-bind="text: numberWithCommas(LuongKy2())" class="textToRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="textToRight">5</td>
                        <td style="padding:10px;">Phụ cấp Tiến sĩ</td>
                        <td data-bind="text: numberWithCommas(PhuCapTienSi())" class="textToRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="textToRight">6</td>
                        <td style="padding:10px;">Lương thử việc</td>
                        <td data-bind="text: numberWithCommas(LuongThuViec())" class="textToRight">&nbsp;</td>
                    </tr>
                    <tr>
                        <td class="textToRight">7</td>
                        <td style="padding:10px;">Thu nhập khác</td>
                        <td data-bind="text: numberWithCommas(ThuNhapKhac())" class="textToRight">></td>
                    </tr>
                    <tr class="boldText">
                        <td class="jqx-center-align">II</td>
                        <td class="boldText">Khấu trừ lương</td>
                        <td data-bind="text: numberWithCommas(KhauTruLuong())" class="textToRight">></td>
                    </tr>
                    <tr class="boldText">
                        <td class="jqx-center-align">III</td>
                        <td class="boldText">Thuế TNCN phải nộp</td>
                        <td data-bind="text: numberWithCommas(ThueTNCN())" class="textToRight">></td>
                    </tr>
                    <tr class="boldText">
                        <td class="jqx-center-align">IV</td>
                        <td class="boldText">Số tiền thực nhận (=I - II - III)</td>
                        <td data-bind="text: numberWithCommas(SoTienThucNhan())" class="textToRight">></td>
                    </tr>

                </table>
                <br />
                <div data-bind="if: khauTruLuong().length > 0">
                    <div class="alert alert-info"><h4 class="margin-top-5">CHI TIẾT KHẤU TRỪ LƯƠNG:</h4></div>
                    <table class="table table-bordered table-hover">
                        <thead>
                        <tr class="active">
                            <th style="width: 2%;" class="boldTextCenter"></th>
                            <th style="width: 13%;" class="boldTextCenter">Ngày</th>
                            <th style="width: 35%;" class="boldTextCenter">Nội dung</th>
                            <th style="width: 25%;" class="boldTextCenter">Tỷ lệ</th>
                            <th style="width: 25%;" class="boldTextCenter">Số tiền</th>

                        </tr>
                            </thead>
                        <tbody data-bind="foreach: khauTruLuong">
                            <tr>
                                <td data-bind="text: $index() + 1" class="textToCenter"></td>
                                <td data-bind="text: formatDate(new Date(NgayTinh))" class="textToCenter"></td>
                                <td data-bind="text: DienGiai" style="padding:10px;"></td>
                                <td data-bind="text: TyLe" class="textToRight"></td>
                                <td data-bind="text: numberWithCommas(SoTien)" class="textToRight"></td>

                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="4" class="textToCenter boldText">Tổng số</td>
                                <td class="textToRight boldText" data-bind="text: numberWithCommas(Sum_SoTien(BangLuongSelected(), khauTruLuong))"></td>
                            </tr>
                        </tfoot>
                    </table>
                    <br />
                </div>
                <div class="alert alert-info"><h4 class="margin-top-5">CHI TIẾT THUẾ TNCN:</h4></div>
                
                <table class="table table-bordered table-hover">
                    <tr>
                        <td style="width: 10%" class="text-align-center">1.</td>
                        <td style="width: 50%;padding:10px;">Tổng thu nhập chịu thuế thu nhập cá nhân</td>
                        <td style="width: 40%;padding:10px;" class="textToRight" width="150px" data-bind="text: numberWithCommas(TNCN_TongThueCaNhan())"></td>
                    </tr>
                    <tr>
                        <td class="text-align-center">2.</td>
                        <td style="padding:10px;">Giảm trừ gia cảnh</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_GiamTruGiaCanh())"></td>
                    </tr>

                    <tr>
                        <td class="textToRight">a.</td>
                        <td style="padding:10px;">Giảm trừ bản thân</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_GiamTruBanThan())"></td>
                    </tr>
                    <tr>
                        <td class="textToRight">b.</td>
                        <td style="padding:10px;">Giảm trừ người phụ thuộc</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_GiamTruNguoiPhuThuoc())"></td>
                    </tr>
                    <tr>
                        <td class="text-align-center">3.</td>
                        <td style="padding:10px;">Thu nhập tính thuế thu nhập cá nhân (9a - 9b - 7a)</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_ThuNhapThueCaNhan())"></td>
                    </tr>
                    <tr>
                        <td class="text-align-center">4.</td>
                        <td style="padding:10px;">Số thuế thu nhập cá nhân phải nộp</td>
                        <td class="textToRight" data-bind="text: numberWithCommas(TNCN_ThuePhaiNop())"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
