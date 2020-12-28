

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao
    {
        [DataMember]
        public string TenHoiThao { get; set; }
        [DataMember]
        public Nullable<System.DateTime> TuNgay { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DenNgay { get; set; }
        [DataMember]
        public string DiaDiem { get; set; }
        [DataMember]
        public string QuocGia { get; set; }
        [DataMember]
        public Nullable<bool> CoBaiThamLuan { get; set; }
    }
}
