

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhBoiDuong
    {
        [DataMember]
        public string TuNgay { get; set; }
        [DataMember]
        public string DenNgay { get; set; }
        [DataMember]
        public string NoiBoiDuong { get; set; }
        [DataMember]
        public string NoiDungBoiDuong { get; set; }
        [DataMember]
        public string ChungChiDuocCap { get; set; }
    }
}
