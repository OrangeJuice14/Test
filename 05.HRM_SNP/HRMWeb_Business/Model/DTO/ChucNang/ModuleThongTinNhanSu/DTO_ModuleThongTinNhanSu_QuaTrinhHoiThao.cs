

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhHoiThao
    {
        [DataMember]
        public string Nam { get; set; }
        [DataMember]
        public string NoiDung { get; set; }
        [DataMember]
        public string TenHoiThao { get; set; }
        [DataMember]
        public string CapHoiThao { get; set; }
        [DataMember]
        public string ThanhTich { get; set; }
        [DataMember]
        public string GhiChu { get; set; }
    }
}
