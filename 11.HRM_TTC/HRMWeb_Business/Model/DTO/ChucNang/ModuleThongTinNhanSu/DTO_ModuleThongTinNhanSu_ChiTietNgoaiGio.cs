

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]

    public partial class DTO_ModuleThongTinNhanSu_ChiTietNgoaiGio
    {
        [DataMember]
        public Nullable<System.DateTime> NgayTinh { get; set; }
        [DataMember]
        public string DienGiai { get; set; }
        [DataMember]
        public decimal SoTien { get; set; }
        [DataMember]
        public Nullable<decimal> KhongTinhThue { get; set; }
    }
    
}
