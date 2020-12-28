
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_CongDoan
    {
        [DataMember]
        public string ToChucCongDoan { get; set; }
        [DataMember]
        public string SoQuyetDinh { get; set; }
        [DataMember]
        public string SoThe { get; set; }
        [DataMember]
        public string ChucVu { get; set; }
      
    }
}
