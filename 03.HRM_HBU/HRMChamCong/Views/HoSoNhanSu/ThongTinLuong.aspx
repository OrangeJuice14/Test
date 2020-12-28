<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="ThongTinLuong.aspx.cs" Inherits="HRMChamCong.Views.HoSoNhanSu.ThongTinLuong" %>

<html>
<head></head>
<body>
    <div id="HoSo_ThongTinLuong">
        <h3 style="font-weight: bold; color: #333">Thông tin lương</h3>
        <div class="hrm_clear">
        </div>
        <div class="hrm_content">
            <div class="hrm_box">
                <div class="hrm_left">
                    Mức lương:
                </div>
                <div class="hrm_right"
                     data-bind="text:MucLuong!=null? numberWithCommas(MucLuong):''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Thưởng hiệu quả theo tháng:
                </div>
                <div class="hrm_right"
                    data-bind="text: ThuongHieuQuaTheoThang != null ? numberWithCommas(ThuongHieuQuaTheoThang) : ''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày điều chỉnh mức thu nhập:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayDieuChinhMucThuNhap == null ? '' : formatDate(new Date(NgayDieuChinhMucThuNhap))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Tham gia Công Đoàn:
                </div>
                <div class="hrm_right"
                    data-bind="text: KhongThamGiaCongDoan==true?'Không':'Có'">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Đóng bảo hiểm:
                </div>
                <div class="hrm_right"
                    data-bind="text: KhongDongBaoHiem==true?'Không':'Có'">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_subtitle1">
                Phụ cấp
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Phụ cấp tiền ăn:
                </div>
                <div class="hrm_right"
                    data-bind="text: PCTienAn != null ? numberWithCommas(PCTienAn) : ''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Phụ cấp điện thoại:
                </div>
                <div class="hrm_right"
                    data-bind="text: PCDienThoai != null ? numberWithCommas(PCDienThoai) : ''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Phụ cấp tiền xăng:
                </div>
                <div class="hrm_right"
                    data-bind="text: PCTienXang != null ? numberWithCommas(PCTienXang) : ''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Tiền trợ cấp chức vụ:
                </div>
                <div class="hrm_right"
                    data-bind="text: TienTroCapChucVu != null ? numberWithCommas(TienTroCapChucVu) : ''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày hưởng tiền trợ cấp chức vụ:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayHuongTienTroCapChucVu == null ? '' : formatDate(new Date(NgayHuongTienTroCapChucVu))">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Phụ cấp trách nhiệm công việc:
                </div>
                <div class="hrm_right"
                    data-bind="text: PhuCapTrachNhiemCongViec != null ? numberWithCommas(PhuCapTrachNhiemCongViec) : ''">
                </div>
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_subtitle">
                Thông tin thuế TNCN
            </div>
            <div class="hrm_clear">
            </div>
            <div class="hrm_box">
                <div class="hrm_left">
                    Ngày cấp mã số thuế:
                </div>
                <div class="hrm_right"
                    data-bind="text: NgayCapMaSoThue == null ? '' : formatDate(new Date(NgayCapMaSoThue))">
                </div>
            </div>
        </div>
        <div class="hrm_clear">
        </div>
        <div class="hrm_box">
            <div class="hrm_left">
                Thuế TNCN:
            </div>
            <div class="hrm_right"
                data-bind="text: TinhThueTNCNTheoMacDinh != null ? numberWithCommas(TinhThueTNCNTheoMacDinh) : ''">
            </div>
        </div>
        <div class="hrm_clear">
        </div>
        <div class="hrm_box">
            <div class="hrm_left">
                Phương thức tính thuế:
            </div>
            <div class="hrm_right"
                data-bind="text: PhuongThuocTinhThue">
            </div>
        </div>
        <div class="hrm_clear">
        </div>
        <div>
            <div class="hrm_subtitle">
                Tài khoản ngân hàng
            </div>
            <div class="hrm_line">
            </div>
            <div>
                <table width="100%" border="1" style="border-collapse: collapse">
                    <tr class="backGroundTitle">
                        <th class="textToCenter">Số tài khoản</th>
                        <th class="textToCenter">Ngân hàng</th>
                        <th class="textToCenter">Tài khoản chính</th>
                    </tr>
                    <tbody data-bind="foreach: DanhSach_TaiKhoanNganHang">
                        <tr>
                            <td data-bind="text: SoTaiKhoan" class="textToCenter"></td>
                            <td data-bind="text: NganHang" class="textToCenter"></td>
                            <td class="textToCenter">
                                <input type="checkbox" data-bind="checked: TaiKhoanChinh" /></td>
                        </tr>
                    </tbody>
                </table>
                <br />
            </div>
        </div>
        <div class="hrm_clear">
        </div>
        <div>
            <div class="hrm_subtitle">
                Danh sách người phụ thuộc
            </div>
            <div class="hrm_line">
            </div>
            <div>
                <table width="100%" border="1" style="border-collapse: collapse">
                    <tr class="backGroundTitle">
                        <th class="textToCenter">Họ tên</th>
                        <th class="textToCenter">Quan hệ</th>
                        <th class="textToCenter">Từ ngày</th>
                        <th class="textToCenter">Đến ngày</th>
                    </tr>
                    <tbody data-bind="foreach: DanhSach_NguoiPhuThuoc">
                        <tr>
                            <td data-bind="text: HoTen" class="textToCenter"></td>
                            <td data-bind="text: QuanHe" class="textToCenter"></td>
                            <td data-bind="text: TuNgay == null ? '' : formatDate(new Date(TuNgay))" class="textToCenter"></td>
                            <td data-bind="text: DenNgay == null ? '' : formatDate(new Date(DenNgay))" class="textToCenter"></td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="hrm_clear">
        </div>
    </div>
    </div>
</body>
</html>
