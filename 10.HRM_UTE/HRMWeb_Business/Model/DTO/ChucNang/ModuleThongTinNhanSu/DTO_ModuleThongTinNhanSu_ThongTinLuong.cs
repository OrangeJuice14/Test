
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ThongTinLuong
    {
        [DataMember]
        public string NhomNgachLuong { get; set; }
        [DataMember]
        public string MaNgach { get; set; }
        [DataMember]
        public string TenNgach { get; set; }
        [DataMember]
        public string NgayBoNhiemNgach { get; set; }
        [DataMember]
        public string BacLuong { get; set; }
        [DataMember]
        public string HeSoLuong { get; set; }
        [DataMember]
        public string NgayHuongLuong { get; set; }
        [DataMember]
        public string MocNangLuong { get; set; }
        [DataMember]
        public string MocNangLuongLanSau { get; set; }
        [DataMember]
        public Nullable<int> DuocHuongHSPCChuyenVien { get; set; }
        [DataMember]
        public string MucLuongDuocHuong { get; set; }
        [DataMember]
        public string HSPCChucVu { get; set; }
        [DataMember]
        public string NgayHuongHSPCChucVu { get; set; }
        [DataMember]
        public string HSPCKiemNhiem { get; set; }
        [DataMember]
        public string HeSoPhuCapTrachNhiem { get; set; }
        [DataMember]
        public string HSPCLuuDong { get; set; }
        [DataMember]
        public string HSPCDocHai { get; set; }
        [DataMember]
        public string HSPCKhac { get; set; }
        [DataMember]
        public string PhanTramUuDai { get; set; }
        [DataMember]
        public string HSPCUuDai { get; set; }
        [DataMember]
        public string PCThuHut { get; set; }
        [DataMember]
        public string PCDacBiet { get; set; }
        [DataMember]
        public string PCDacThu { get; set; }
        [DataMember]
        public string VuotKhung { get; set; }
        [DataMember]
        public string HSPCVuotKhung { get; set; }
        [DataMember]
        public string HSPCThamNienTruong { get; set; }
        [DataMember]
        public string ThamNien { get; set; }
        [DataMember]
        public string HSPCThamNien { get; set; }
        [DataMember]
        public string NgayTinhThamNienNhaGiao { get; set; }
        [DataMember]
        public string NgayHuongHSPCThamNien { get; set; }
        [DataMember]
        public Nullable<int> HSPCLanhDao { get; set; }
        [DataMember]
        public string HSPCQuanLy { get; set; }
        [DataMember]
        public string HSPCChuyenMon { get; set; }
        [DataMember]
        public string HSPCKiemNhiem1 { get; set; }
        [DataMember]
        public string HSPCKiemNhiem2 { get; set; }
        [DataMember]
        public string PhuCapTrachNhiemCongViec { get; set; }
        [DataMember]
        public string PhuCapTienXang { get; set; }
        [DataMember]
        public Nullable<int> NgayHuongHSPCChuyenMon { get; set; }
        [DataMember]
        public string HSPCTrachNhiem { get; set; }
        [DataMember]
        public Nullable<int> NgayHuongHSPCTracNhiem { get; set; }
        [DataMember]
        public Nullable<int> HSPCKiemNhiemTrongTruong { get; set; }
        [DataMember]
        public string MaSoThue { get; set; }
        [DataMember]
        public string CoQuanThue { get; set; }
        [DataMember]
        public string SoNguoiPhuThuoc { get; set; }
        [DataMember]
        public string SoThangGiamTru { get; set; }
        [DataMember]
        public string LoaiCuTru { get; set; }
    }
}
