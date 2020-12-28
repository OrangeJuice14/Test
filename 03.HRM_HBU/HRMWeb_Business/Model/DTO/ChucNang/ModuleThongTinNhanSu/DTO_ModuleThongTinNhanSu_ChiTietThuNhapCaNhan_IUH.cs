

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan_IUH
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
        public decimal LuongKy1 { get; set; }
        [DataMember]
        public decimal PhuCapTrachNhiem { get; set; }
        [DataMember]
        public decimal PhuCapThamNien { get; set; }
        [DataMember]
        public decimal LuongKy2 { get; set; }
        [DataMember]
        public decimal PhuCapTienSi { get; set; }
        [DataMember]
        public decimal LuongThuViec { get; set; }
        [DataMember]
        public decimal ThuNhapKhac { get; set; }
        [DataMember]
        public Nullable<decimal> KhauTruLuong { get; set; }
        [DataMember]
        public decimal ThueTNCN { get; set; }
        [DataMember]
        public Nullable<decimal> TongThuNhap { get; set; }
        [DataMember]
        public Nullable<decimal> ThucNhan { get; set; }
    }
}
