

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhDieuDong
    {
        [DataMember]
        public string SoQuyetDinh { get; set; }
        [DataMember]
        public DateTime NgayQuyetDinh { get; set; }
        [DataMember]
        public string DonViCu { get; set; }
        [DataMember]
        public string DonViMoi { get; set; }
        [DataMember]
        public DateTime NgayDieuChuyen { get; set; }
    }
}
