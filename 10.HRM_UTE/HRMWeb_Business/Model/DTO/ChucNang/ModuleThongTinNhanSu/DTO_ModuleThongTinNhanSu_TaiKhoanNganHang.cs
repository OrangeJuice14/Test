
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_TaiKhoanNganHang
    {
        [DataMember]
        public string SoTaiKhoan { get; set; }
        [DataMember]
        public string NganHang { get; set; }
        [DataMember]
        public Nullable<bool> TaiKhoanChinh { get; set; }
    }
}
