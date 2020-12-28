<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="HRMChamCong.Views.BangLuong.Manage" %>

<%@ Import Namespace="HRMChamCong.Helper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function numberWithCommas(x) {
            if (x == null || x == undefined) {
                return 0;
            } else {
                return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + " ₫";
            }
        }
        function formatDate(date) {
            return date.getDate() + "/" + parseInt(date.getMonth() + 1) + "/" + date.getFullYear();
        }
        function PrintElem(elem) {
            $("#bangLuong").printThis({
                importStyle: false,
                importCSS: true,
                loadCSS: "/CSS/PrintBangLuong.css",
            });
        }
        function ViewModel() {
            var self = this;
            self.BangLuong = ko.observableArray();
            self.BangLuongSelected = ko.observable();
            self.MaNhanSu = ko.observable();
            self.HoTen = ko.observable();
            self.DonVi = ko.observable();
            self.LuongTheoNgayCong = ko.observable(0);
            self.LuongNgoaiGio = ko.observable(0);
            self.ThuongLeTet = ko.observable(0);
            self.TruyLuong = ko.observable(0);
            self.BHXH = ko.observable(0);
            self.BHYT = ko.observable(0);
            self.BHTN = ko.observable(0);
            self.CongDoan = ko.observable(0);
            self.TruyThu = ko.observable(0);
            self.ThueTNCN = ko.observable(0);
            self.ThuNhapChiuThue = ko.observable(0);
            self.GiamTruBanThan = ko.observable(0);
            self.GiamTruGiaCanh = ko.observable(0);
            self.ThuNhapTinhThue = ko.observable(0);
            self.ThucLanh = ko.observable(0);
            self.MucLuongTruocDieuChinh = ko.observable(0);
            self.MucLuongSauDieuChinh = ko.observable(0);
            self.ThuongHieuQuaTruocDieuChinh = ko.observable(0);
            self.ThuongHieuQuaSauDieuChinh = ko.observable(0);
            self.PhuCapTienXang = ko.observable(0);
            self.PhuCapDienThoai = ko.observable(0);
            self.PhuCapTrachNhiemCongViec = ko.observable(0);
            self.NgayDieuChinhMucThuNhap = ko.observable();
            self.SoNgayCong = ko.observable(0);
            self.SinhNhat = ko.observable(0);

            self.TenBangLuong = ko.observable("aa");
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
            self.BangLuongSelected.subscribe(function (newValue) {
                $.each(self.BangLuong(), function (idx, item) {
                    if (item.Oid == newValue) {
                        self.TenBangLuong(item.Name);
                    }
                });
                $.ajax({
                    type: 'POST',
                    url: '/Services/ChamCongService.asmx/ThongTinNhanSu_BANGLUONG',
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    data: ko.toJSON({
                        kyTinhLuong: newValue,
                        loaiLuong: 1
                    }),
                    dataType: "json",
                    success: function (result) {
                        var data = $.parseJSON(result.d);
                        //ChiTietThuNhapCaNhan
                        if (data != null) {
                            self.MaNhanSu(data.MaNhanSu);
                            self.HoTen(data.HoTen);
                            self.DonVi(data.DonVi);
                            self.LuongTheoNgayCong(data.LuongTheoNgayCong);
                            self.LuongNgoaiGio(data.LuongNgoaiGio);
                            self.ThuongLeTet(data.ThuongLeTet);
                            self.TruyLuong(data.TruyLuong);
                            self.BHXH(data.BHXH);
                            self.BHYT(data.BHYT);
                            self.BHTN(data.BHTN);
                            self.CongDoan(data.CongDoan);
                            self.TruyThu(data.TruyThu);
                            self.ThueTNCN(data.ThueTNCN);
                            self.ThuNhapChiuThue(data.ThuNhapChiuThue);
                            self.GiamTruBanThan(data.GiamTruBanThan);
                            self.GiamTruGiaCanh(data.GiamTruGiaCanh);
                            self.ThuNhapTinhThue(data.ThuNhapTinhThue);
                            self.ThucLanh(data.ThucLanh);
                            self.MucLuongTruocDieuChinh(data.MucLuongTruocDieuChinh);
                            self.MucLuongSauDieuChinh(data.MucLuongSauDieuChinh);
                            self.ThuongHieuQuaTruocDieuChinh(data.ThuongHieuQuaTruocDieuChinh);
                            self.ThuongHieuQuaSauDieuChinh(data.ThuongHieuQuaSauDieuChinh);
                            self.PhuCapTienXang(data.PhuCapTienXang);
                            self.PhuCapDienThoai(data.PhuCapDienThoai);
                            self.PhuCapTrachNhiemCongViec(data.PhuCapTrachNhiemCongViec);
                            self.NgayDieuChinhMucThuNhap(data.NgayDieuChinhMucThuNhap);
                            self.SoNgayCong(data.SoNgayCong);
                            self.SinhNhat(data.SinhNhat);
                        }
                    }
                });
            });
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <button onclick="PrintElem('bangLuong')">In Bảng Lương</button>
    </div>
    <div id="bangLuong">
        <h3 id="tenBangLuong" style="visibility: hidden; display: none;" data-bind="text: TenBangLuong"></h3>
        <div id="chonBangLuong">
            <a href="#">Bảng lương và phụ cấp : </a>
            <select data-bind="options: BangLuong, optionsText: 'Name', optionsValue: 'Oid', value: BangLuongSelected, optionsCaption: '-- Chọn bảng lương --'"></select>
        </div>
        <div data-bind="visible: (BangLuongSelected() != undefined) ">
            <div>
                <div class="row" style="margin-top: 10px;">
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
                <div class="well">
                    <div class="row">
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Mức lương trước điều chỉnh: <span data-bind="text: numberWithCommas(MucLuongTruocDieuChinh())"></span>
                        </label>
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Thưởng hiệu quả trước điều chỉnh: <span data-bind="text: numberWithCommas(ThuongHieuQuaTruocDieuChinh())"></span>
                        </label>
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Ngày điều chỉnh mức thu nhập: <span data-bind="text: NgayDieuChinhMucThuNhap() == null ? '' : formatDate(new Date(NgayDieuChinhMucThuNhap()))"></span>
                        </label>
                    </div>
                    <div class="row">
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Mức lương sau điều chỉnh: <span data-bind="text: numberWithCommas(MucLuongSauDieuChinh())"></span>
                        </label>
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Thưởng hiệu quả sau điều chỉnh: <span data-bind="text: numberWithCommas(ThuongHieuQuaSauDieuChinh())"></span>
                        </label>
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Phụ cấp tiền xăng: <span data-bind="text: numberWithCommas(PhuCapTienXang())"></span>
                        </label>
                    </div>
                    <div class="row">

                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Phụ cấp điện thoại: <span data-bind="text: numberWithCommas(PhuCapDienThoai())"></span>
                        </label>
                        <label class="col-lg-4 col-md-4 col-sm-6">
                            Phụ cấp trách nhiệm công việc: <span data-bind="text: numberWithCommas(PhuCapTrachNhiemCongViec())"></span>
                        </label>
                    </div>
                </div>
                <div class="row" style="padding: 0 15px 0 15px">
                    <table class="table table-bordered table-hover table-striped" style="font-size: 14px;">
                        <thead>
                            <tr class="active">
                                <th class="text-align-center padding-10">Diễn giải</th>
                                <th class="text-align-center" style="width: 150px;">Số tiền</th>
                            </tr>
                        </thead>

                        <tr>

                            <td class="boldText" style="width: 300px; padding: 10px;" colspan="2">Thông tin lương</td>

                        </tr>
                        <tr>

                            <td style="padding: 10px;">Tổng ngày công</td>
                            <td data-bind="text: SoNgayCong()" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: LuongTheoNgayCong() > 0">

                            <td style="padding: 10px;">Lương theo ngày công</td>
                            <td data-bind="text: numberWithCommas(LuongTheoNgayCong())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: LuongNgoaiGio() > 0">

                            <td style="padding: 10px;">Lương ngoài giờ</td>
                            <td data-bind="text: numberWithCommas(LuongNgoaiGio())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: ThuongLeTet() > 0">

                            <td style="padding: 10px;">Khen thưởng - Phúc lợi</td>
                            <td data-bind="text: numberWithCommas(ThuongLeTet())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: SinhNhat() > 0">

                            <td style="padding: 10px;">Sinh nhật</td>
                            <td data-bind="text: numberWithCommas(SinhNhat())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: BHXH() > 0">

                            <td style="padding: 10px;">Bảo hiểm xã hội</td>
                            <td data-bind="text: numberWithCommas(BHXH())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: BHYT() > 0">

                            <td style="padding: 10px;">Bảo hiểm y tế</td>
                            <td data-bind="text: numberWithCommas(BHYT())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: BHTN() > 0">

                            <td style="padding: 10px;">Bảo hiểm thất nghiệp</td>
                            <td data-bind="text: numberWithCommas(BHTN())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: CongDoan() > 0">

                            <td style="padding: 10px;">Kinh phí công đoàn</td>
                            <td data-bind="text: numberWithCommas(CongDoan())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: TruyThu() > 0">

                            <td style="padding: 10px;">Truy thu</td>
                            <td data-bind="text: numberWithCommas(TruyThu())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: TruyLuong() > 0">

                            <td style="padding: 10px;">Truy lĩnh</td>
                            <td data-bind="text: numberWithCommas(TruyLuong())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: ThuNhapChiuThue() > 0">

                            <td style="padding: 10px;">Thu nhập chịu thuế</td>
                            <td data-bind="text: numberWithCommas(ThuNhapChiuThue())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: ThueTNCN() > 0">

                            <td style="padding: 10px;">Thuế TNCN</td>
                            <td data-bind="text: numberWithCommas(ThueTNCN())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: GiamTruBanThan() > 0">

                            <td style="padding: 10px;">Giảm trừ bản thân</td>
                            <td data-bind="text: numberWithCommas(GiamTruBanThan())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: GiamTruGiaCanh() > 0">

                            <td style="padding: 10px;">Giảm trừ gia cảnh</td>
                            <td data-bind="text: numberWithCommas(GiamTruGiaCanh())" class="textToRight"></td>
                        </tr>
                        <tr data-bind="visible: ThuNhapTinhThue() > 0">

                            <td style="padding: 10px;">Thu nhập tính thuế</td>
                            <td data-bind="text: numberWithCommas(ThuNhapTinhThue())" class="textToRight"></td>
                        </tr>
                        <tr class="boldText">
                            <td class="boldText table-success">Số tiền thực lãnh</td>
                            <td data-bind="text: numberWithCommas(ThucLanh())" class="textToRight">></td>
                        </tr>

                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
