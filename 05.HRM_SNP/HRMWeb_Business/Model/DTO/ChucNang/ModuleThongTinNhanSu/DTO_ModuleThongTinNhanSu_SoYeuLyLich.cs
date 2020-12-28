

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_SoYeuLyLich //DTO_ModuleThongTinNhanSu_SoYeuLyLich
    {
        [DataMember]
        public string SoHieuCongChuc { get; set; }
        [DataMember]
        public byte[] HinhAnh { get; set; }
        [DataMember]
        public string HoTen { get; set; }
        [DataMember]
        public string TenGoiKhac { get; set; }
        [DataMember]
        public string GioiTinh { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        [DataMember]
        public string NoiSinh { get; set; }
        [DataMember]
        public string QueQuan { get; set; }
        [DataMember]
        public string DanToc { get; set; }
        [DataMember]
        public string TonGiao { get; set; }
        [DataMember]
        public string DiaChiThuongTru { get; set; }
        [DataMember]
        public string NoiOHienNay { get; set; }
        [DataMember]
        public string SoCMND { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayCap { get; set; }
        [DataMember]
        public string NoiCap { get; set; }
        [DataMember]
        public string QuocTich { get; set; }
        [DataMember]
        public string ThanhPhanXuatThan { get; set; }
        [DataMember]
        public string TinhTrangHonNhan { get; set; }
        [DataMember]
        public string UuTienGiaDinh { get; set; }
        [DataMember]
        public string UuTienBanThan { get; set; }
        [DataMember]
        public Nullable<decimal> ChieuCao { get; set; }
        [DataMember]
        public Nullable<decimal> CanNang { get; set; }
        [DataMember]
        public string TinhTrangSucKhoe { get; set; }
        [DataMember]
        public string NhomMau { get; set; }
        [DataMember]
        public string ChucDanh { get; set; }
        [DataMember]
        public string DienThoaiDiDong { get; set; }
        [DataMember]
        public string DienThoaiNha { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string DonViTuyenDung { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayTuyenDung { get; set; }
        [DataMember]
        public string HinhThucTuyenDung { get; set; }
        [DataMember]
        public string CongViecTuyenDung { get; set; }
        [DataMember]
        public string LoaiCanBo { get; set; }
        [DataMember]
        public string DonViCongTac { get; set; }
        [DataMember]
        public string ChucVuHienTai { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayBoNhiem { get; set; }
        [DataMember]
        public string ChucVuKiemNhiem { get; set; }
        [DataMember]
        public string ChucVuCaoNhatDaQua { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayVaoNganhGiaoDuc { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayVaoCoQuanHienNay { get; set; }
        [DataMember]
        public string CongViecHienNay { get; set; }
        [DataMember]
        public string HopDongHienTai { get; set; }
    }
}
