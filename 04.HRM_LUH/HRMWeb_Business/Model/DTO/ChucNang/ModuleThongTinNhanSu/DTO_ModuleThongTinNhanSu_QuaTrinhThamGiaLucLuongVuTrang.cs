

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang
    {
        [DataMember]
        public string NgayNhapNgu { get; set; }
        [DataMember]
        public string NgayXuatNgu { get; set; }
        [DataMember]
        public string QuanHam { get; set; }
        [DataMember]
        public string NoiDung { get; set; }
    }
}
