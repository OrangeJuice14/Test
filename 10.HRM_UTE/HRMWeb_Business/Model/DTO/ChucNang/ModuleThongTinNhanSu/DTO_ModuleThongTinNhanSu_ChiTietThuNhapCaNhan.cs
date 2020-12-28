

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan
    {
        [DataMember]
        public string TenBoPhan { get; set; }
        [DataMember]
        public string HoTen { get; set; }
        [DataMember]
        public string TenKy { get; set; }
        [DataMember]
        public decimal LuongCoBan { get; set; }
        [DataMember]
        public decimal HeSoLuong { get; set; }
        [DataMember]
        public decimal HSPCVuotKhung { get; set; }
        [DataMember]
        public decimal HSPCChucVu { get; set; }
        [DataMember]
        public decimal HSPCUuDai { get; set; }
        [DataMember]
        public decimal HSPCThamNien { get; set; }
        [DataMember]
        public decimal HSPCDocHai { get; set; }
        [DataMember]
        public decimal HSPCKhac { get; set; }
        [DataMember]
        public decimal HeSoKPI { get; set; }
        [DataMember]
        public decimal TongHeSoLuongNhaNuoc { get; set; }
        [DataMember]
        public decimal LuongNhaNuoc { get; set; }
        [DataMember]
        public decimal HSLTangThem { get; set; }
        [DataMember]
        public string TiLeTangThem { get; set; }
        [DataMember]
        public decimal LuongTangThem { get; set; }
        [DataMember]
        public decimal HSPCThamNienHC { get; set; }
        [DataMember]
        public string PhanTramThamNienHC { get; set; }
        [DataMember]
        public decimal LuongThamNienHanhChinh { get; set; }
        [DataMember]
        public decimal TongLuongTangThem { get; set; }
        [DataMember]
        public decimal HSPCKhoiHanhChinh { get; set; }
        [DataMember]
        public decimal HSPCTrachNhiem1 { get; set; }
        [DataMember]
        public decimal HSPCTrachNhiem2 { get; set; }
        [DataMember]
        public decimal HSPCTrachNhiem3 { get; set; }
        [DataMember]
        public decimal HSPCTrachNhiem4 { get; set; }
        [DataMember]
        public decimal HSPCTrachNhiem5 { get; set; }
        [DataMember]
        public decimal HSPCTrachNhiem6 { get; set; }
        [DataMember]
        public decimal TongHSTrachNhiem { get; set; }
        [DataMember]
        public decimal LuongPhuCapKhoiHanhChinh { get; set; }
        [DataMember]
        public decimal LuongPhuCapTrachNhiem { get; set; }
        [DataMember]
        public decimal MucLuongTangThem { get; set; }
        [DataMember]
        public decimal PTBHXH { get; set; }
        [DataMember]
        public decimal PTBHYT { get; set; }
        [DataMember]
        public decimal PTBHTN { get; set; }
        [DataMember]
        public decimal BHTN { get; set; }
        [DataMember]
        public decimal BHYT { get; set; }
        [DataMember]
        public decimal BHXH { get; set; }
        [DataMember]
        public decimal ThueTNCN { get; set; }
        [DataMember]
        public decimal TruHocNuocNgoai { get; set; }
        [DataMember]
        public decimal TongThuNhapChiuThue { get; set; }
        [DataMember]
        public decimal TienKhauTruThieuTiet { get; set; }
        [DataMember]
        public decimal TienKhauTruThueTNCN { get; set; }
        [DataMember]
        public decimal TienKhauTruKhac { get; set; }
        [DataMember]
        public decimal TongKhauTruLuong { get; set; }
        [DataMember]
        public decimal TongThuNhap { get; set; }
        [DataMember]
        public decimal TongKhauTru { get; set; }
        [DataMember]
        public decimal ThucLanh { get; set; }
        [DataMember]
        public string NamTruoc { get; set; }
        [DataMember]
        public decimal PhuCapQuanLyDaoTao { get; set; }
        [DataMember]
        public decimal PhuCapQuanLyDaoTao_CLC { get; set; }
    }
}
