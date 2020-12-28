

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_LichSuBanThan
    {
        [DataMember]
        public string TuNam { get; set; }
        [DataMember]
        public string DenNam { get; set; }
        [DataMember]
        public string NoiDung { get; set; }
        [DataMember]
        public string ChucDanh { get; set; }
        [DataMember]
        public string CongTy { get; set; }
    }
}
