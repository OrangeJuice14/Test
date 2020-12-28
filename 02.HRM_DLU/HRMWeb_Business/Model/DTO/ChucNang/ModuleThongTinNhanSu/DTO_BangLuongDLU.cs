

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_BangLuongDLU
    {
        [DataMember]
        public string NoiDung { get; set; }
        [DataMember]
        public DateTime NgayChi { get; set; }
        [DataMember]
        public decimal SoTien { get; set; }
        [DataMember]
        public string GhiChu { get; set; }
        [DataMember]
        public double TongTien { get; set; }
        [DataMember]
        public string TongTienBangChu { get; set; }
    }
}
