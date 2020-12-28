
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_DienBienLuong
    {
        [DataMember]
        public Nullable<System.DateTime> TuNgay { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DenNgay { get; set; }
        [DataMember]
        public string NgachLuong { get; set; }
        [DataMember]
        public string BacLuong { get; set; }
        [DataMember]
        public Nullable<decimal> HeSoLuong { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCChucVu { get; set; }
        [DataMember]
        public Nullable<int> VuotKhung { get; set; }
        [DataMember]
        public Nullable<decimal> ThamNien { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCKhac { get; set; }
    }
}
