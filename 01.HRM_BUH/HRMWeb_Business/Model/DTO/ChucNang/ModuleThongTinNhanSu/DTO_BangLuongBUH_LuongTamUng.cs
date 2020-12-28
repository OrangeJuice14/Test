

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_BangLuongBUH_LuongTamUng
    {
        [DataMember]
        public string SoTaiKhoan { get; set; }
        [DataMember]
        public string Ho { get; set; }
        [DataMember]
        public string Ten { get; set; }
        [DataMember]
        public string MaNgach { get; set; }
        [DataMember]
        public Nullable<decimal> HeSoLuong { get; set; }
        [DataMember]
        public Nullable<decimal> VuotKhung { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCVuotKhung { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCChucVu { get; set; }
        [DataMember]
        public Nullable<decimal> TongHeSoLuong { get; set; }
        [DataMember]
        public Nullable<decimal> PTHuongLuong { get; set; }
        [DataMember]
        public Nullable<decimal> TongTienLuong { get; set; }
        [DataMember]
        public Nullable<decimal> PhuCapTienAn { get; set; }
        [DataMember]
        public decimal KTVayMuaNha { get; set; }
        [DataMember]
        public decimal KTDienNuoc { get; set; }
        [DataMember]
        public decimal KTThuePhong { get; set; }
        [DataMember]
        public decimal KTThueThuNhap { get; set; }
        [DataMember]
        public Nullable<decimal> TongKhoanTru { get; set; }
        [DataMember]
        public decimal TruyThu { get; set; }
        [DataMember]
        public Nullable<decimal> ThucLanh { get; set; }
        [DataMember]
        public Nullable<decimal> DPCD { get; set; }
        [DataMember]
        public Nullable<decimal> UH1NL { get; set; }
        [DataMember]
        public Nullable<decimal> KTKhac { get; set; }
    }
}
