

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhSangKien
    {
        [DataMember]
        public string Nam { get; set; }
        [DataMember]
        public string NoiDung { get; set; }
        [DataMember]
        public string TenSangKien { get; set; }
        [DataMember]
        public string Dat { get; set; }
        [DataMember]
        public string GhiChu { get; set; }
    }
}
