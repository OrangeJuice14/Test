

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan
    {
        [DataMember]
        public string MaNhanSu { get; set; }
        [DataMember]
        public string HoTen { get; set; }
        [DataMember]
        public string DonVi { get; set; }
        [DataMember]
        public string NgachLuong { get; set; }
        [DataMember]
        public string BacLuong { get; set; }
        [DataMember]
        public decimal HeSoLuong { get; set; }
        [DataMember]
        public decimal LuongCoBan { get; set; }
        [DataMember]
        public string XepLoai { get; set; }
        [DataMember]
        public decimal ThuLaoGiangDay { get; set; }
        [DataMember]
        public decimal LuongNganSach { get; set; }
        [DataMember]
        public decimal PhuCap { get; set; }
        [DataMember]
        public decimal ThuNhapTangThem { get; set; }
        [DataMember]
        public decimal ThuNhapKhac { get; set; }
        [DataMember]
        public decimal NgoaiGio { get; set; }
        [DataMember]
        public decimal KhenThuong { get; set; }
        [DataMember]
        public decimal KhauTruLuong { get; set; }
        [DataMember]
        public decimal ThueTNCN { get; set; }
        [DataMember]
        public Nullable<decimal> TongThuNhap { get; set; }
        [DataMember]
        public Nullable<decimal> ThucNhan { get; set; }
    }
}
