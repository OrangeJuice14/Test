
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem
    {
        [DataMember]
        public string SoQuyetDinh { get; set; }
        [DataMember]
        public string TuNam { get; set; }
        [DataMember]
        public string DenNam { get; set; }
        [DataMember]
        public string ChucVu { get; set; }
        [DataMember]
        public decimal PhuCapChucVu { get; set; }
        [DataMember]
        public System.DateTime NgayHuongChucVu { get; set; }
    }
}
