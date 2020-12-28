

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_HBU
    {
        [DataMember]
        public string MaNhanSu { get; set; }
        [DataMember]
        public string HoTen { get; set; }
        [DataMember]
        public string DonVi { get; set; }
        [DataMember]
        public Nullable<decimal> LuongTheoNgayCong { get; set; }
        [DataMember]
        public Nullable<decimal> LuongNgoaiGio { get; set; }
        [DataMember]
        public Nullable<decimal> ThuongLeTet { get; set; }
        [DataMember]
        public Nullable<decimal> TruyLuong { get; set; }
        [DataMember]
        public Nullable<decimal> BHXH { get; set; }
        [DataMember]
        public Nullable<decimal> BHYT { get; set; }
        [DataMember]
        public Nullable<decimal> BHTN { get; set; }
        [DataMember]
        public Nullable<decimal> CongDoan { get; set; }
        [DataMember]
        public Nullable<decimal> TruyThu { get; set; }
        [DataMember]
        public Nullable<decimal> ThueTNCN { get; set; }
        [DataMember]
        public Nullable<decimal> ThuNhapChiuThue { get; set; }
        [DataMember]
        public Nullable<decimal> GiamTruBanThan { get; set; }
        [DataMember]
        public Nullable<decimal> GiamTruGiaCanh { get; set; }
        [DataMember]
        public Nullable<decimal> ThuNhapTinhThue { get; set; }
        [DataMember]
        public Nullable<decimal> ThucLanh { get; set; }
        [DataMember]
        public Nullable<decimal> MucLuongTruocDieuChinh { get; set; }
        [DataMember]
        public Nullable<decimal> MucLuongSauDieuChinh { get; set; }
        [DataMember]
        public Nullable<decimal> ThuongHieuQuaTruocDieuChinh { get; set; }
        [DataMember]
        public Nullable<decimal> ThuongHieuQuaSauDieuChinh { get; set; }
        [DataMember]
        public Nullable<decimal> PhuCapTienXang { get; set; }
        [DataMember]
        public Nullable<decimal> PhuCapDienThoai { get; set; }
        [DataMember]
        public Nullable<decimal> PhuCapTrachNhiemCongViec { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayDieuChinhMucThuNhap { get; set; }
        [DataMember]
        public Nullable<decimal> SoNgayCong { get; set; }
        [DataMember]
        public Nullable<decimal> SinhNhat { get; set; }
    }
}
