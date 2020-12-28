

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_BangLuongBUH_LuongNganSach
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
        public Nullable<decimal> ThamNien { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCThamNienNhaGiao { get; set; }
        [DataMember]
        public Nullable<decimal> TongHeSoLuong { get; set; }
        [DataMember]
        public Nullable<decimal> PTHuongLuong { get; set; }
        [DataMember]
        public Nullable<decimal> TongTienLuong { get; set; }
        [DataMember]
        public Nullable<decimal> SoNgayNghiViec { get; set; }
        [DataMember]
        public Nullable<decimal> TienNgayNghiViec { get; set; }
        [DataMember]
        public Nullable<decimal> SoNgayBHXHTra { get; set; }
        [DataMember]
        public Nullable<decimal> TienBHXHTra { get; set; }
        [DataMember]
        public Nullable<decimal> BHYT { get; set; }
        [DataMember]
        public Nullable<decimal> BHTN { get; set; }
        [DataMember]
        public Nullable<decimal> BHXH { get; set; }
        [DataMember]
        public Nullable<decimal> DPCD { get; set; }
        [DataMember]
        public Nullable<decimal> TongKhoanTru { get; set; }
        [DataMember]
        public Nullable<decimal> PhuCapUuDai { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCUuDai { get; set; }
        [DataMember]
        public Nullable<decimal> PhuCapKiemNhiem { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCKiemNhiem { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCDocHai { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCTrachNhiem { get; set; }
        [DataMember]
        public Nullable<decimal> TongHSPC { get; set; }
        [DataMember]
        public Nullable<decimal> TongTienPhuCap { get; set; }
        [DataMember]
        public Nullable<decimal> TruyThu { get; set; }
        [DataMember]
        public Nullable<decimal> ThucLanh { get; set; }

    }
}