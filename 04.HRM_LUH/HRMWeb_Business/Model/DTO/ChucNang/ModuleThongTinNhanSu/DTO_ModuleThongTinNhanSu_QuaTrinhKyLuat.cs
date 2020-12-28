

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhKhenThuong
    {
        [DataMember]
        public Nullable<System.DateTime> NgayKhenThuong { get; set; }
        [DataMember]
        public string DanhHieu { get; set; }
        [DataMember]
        public string SoQuyetDinh { get; set; }
        [DataMember]
        public string LyDo { get; set; }
    }
}
