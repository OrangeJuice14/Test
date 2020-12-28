
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhCongTac
    {
        [DataMember]
        public decimal STT { get; set; }
        [DataMember]
        public string LoaiQuyetDinh { get; set; }
        [DataMember]
        public string SoQuyetDinh { get; set; }
        [DataMember]
        public DateTime NgayQuyetDinh { get; set; }
        [DataMember]
        public DateTime NgayBanHanh { get; set; }
        [DataMember]
        public string TuNam { get; set; }
        [DataMember]
        public string DenNam { get; set; }
        [DataMember]
        public string NoiDung { get; set; }
    }
}
