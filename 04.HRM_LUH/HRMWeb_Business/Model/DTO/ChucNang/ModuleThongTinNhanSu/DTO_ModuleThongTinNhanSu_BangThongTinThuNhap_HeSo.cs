

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_HeSo
    {
        [DataMember]
        public decimal HSL { get; set; }
        [DataMember]
        public decimal PCCV { get; set; }
        [DataMember]
        public decimal PCVK { get; set; }
        [DataMember]
        public decimal PCTN { get; set; }
        [DataMember]
        public decimal PCCM { get; set; }
        [DataMember]
        public decimal PCQL { get; set; }
        [DataMember]
        public decimal PCKN1 { get; set; }
        [DataMember]
        public decimal PCKN2 { get; set; }
        [DataMember]
        public decimal PCDH { get; set; }
        [DataMember]
        public decimal PCTNhiem { get; set; }
    }
}
