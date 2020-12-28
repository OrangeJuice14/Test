

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaBHXH
    {
        [DataMember]
        public Nullable<System.DateTime> TuNam { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DenNam { get; set; }
        [DataMember]
        public string NoiLamViec { get; set; }
        [DataMember]
        public Nullable<decimal> HeSoLuong { get; set; }
        [DataMember]
        public Nullable<decimal> HSPCChucVu { get; set; }
        [DataMember]
        public Nullable<int> VuotKhung { get; set; }
        [DataMember]
        public Nullable<decimal> ThamNien { get; set; }
        
    }
}
