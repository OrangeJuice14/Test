
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhHuanLuyenDaoTao
    {
        [DataMember]
        public Nullable<System.DateTime> NgayQuyetDinh { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayHetHieuLuc { get; set; }
        [DataMember]
        public string ChuongTrinhDaoTao { get; set; }
        [DataMember]
        public string CapDoDaoTao { get; set; }
        [DataMember]
        public string Diem { get; set; }
        [DataMember]
        public string Dat { get; set; }
    }
}
