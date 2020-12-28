

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_HopDong
    {
        [DataMember]
        public string SoHopDong { get; set; }
        [DataMember]
        public string LoaiHopDong { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayKy { get; set; }
        [DataMember]
        public string ChucVuNguoiKy { get; set; }
        [DataMember]
        public string NguoiKy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> TuNgay { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DenNgay { get; set; }
        [DataMember]
        public string MucLuongDuocHuong { get; set; }
        [DataMember]
        public string ChucDanhChuyenMon { get; set; }
        [DataMember]
        public string MaNgachLuong { get; set; }
        [DataMember]
        public string TenNgachLuong { get; set; }
        [DataMember]
        public string BacLuong { get; set; }
        [DataMember]
        public decimal HeSoLuong { get; set; }
        [DataMember]
        public string LuongCoBan { get; set; }
        [DataMember]
        public string LuongKinhDoanh { get; set; }
    }
}
