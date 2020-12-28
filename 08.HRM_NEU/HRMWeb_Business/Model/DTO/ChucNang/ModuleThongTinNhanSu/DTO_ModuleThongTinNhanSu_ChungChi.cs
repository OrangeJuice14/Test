

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChungChi
    {
        [DataMember]
        public string LoaiChungChi { get; set; }
        [DataMember]
        public string TrinhDoChuyenMon { get; set; }
        [DataMember]
        public string NoiCap { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayCap { get; set; }
        [DataMember]
        public Nullable<decimal> Diem { get; set; }
        [DataMember]
        public string XepLoai { get; set; }
    }
}
