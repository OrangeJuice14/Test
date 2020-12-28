

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_NgoaiNgu
    {
        [DataMember]
        public string TenNgoaiNgu { get; set; }
        [DataMember]
        public string TrinhDo { get; set; }
        [DataMember]
        public Nullable<decimal> Diem { get; set; }
    }
}
