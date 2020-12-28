
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_DoanVien
    {
        [DataMember]
        public string ToChucDoan { get; set; }
        [DataMember]
        public string SoThe { get; set; }
        [DataMember]
        public string NgayCap { get; set; }
        [DataMember]
        public string NgayKetNap { get; set; }
        [DataMember]
        public Nullable<System.Guid> NoiKetNap { get; set; }
        [DataMember]
        public string ChucVu { get; set; }
      
    }
}
