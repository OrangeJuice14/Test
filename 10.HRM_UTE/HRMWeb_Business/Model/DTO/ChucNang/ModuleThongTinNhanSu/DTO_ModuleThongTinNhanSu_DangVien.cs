
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_DangVien
    {
        [DataMember]
        public string DangBo { get; set; }
        [DataMember]
        public string ChiBoDang { get; set; }
        [DataMember]
        public string SoLyLich { get; set; }
        [DataMember]
        public string NgayVaoDang { get; set; }
        [DataMember]
        public string NgayVaoDangChinhThuc { get; set; }
        [DataMember]
        public string SoThe { get; set; }
        [DataMember]
        public string ChucVu { get; set; }
        [DataMember]
        public string NgayCap { get; set; }
        [DataMember]
        public string NoiCap { get; set; }
      
    }
}
