

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_Thue
    {
        [DataMember]
        public string TieuDe { get; set; }
        [DataMember]
        public decimal Sotien { get; set; }
        [DataMember]
        public string Ghichu { get; set; }
    }

}
