
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhDaoTao
    {
        [DataMember]
        public Nullable<int> NamNhapHoc { get; set; }
        [DataMember]
        public Nullable<int> NamTotNghiep { get; set; }
        [DataMember]
        public string TrinhDoChuyenMon { get; set; }
        [DataMember]
        public string ChuyenNganhDaoTao { get; set; }
        [DataMember]
        public string TruongDaoTao { get; set; }
        [DataMember]
        public string HinhThucDaoTao { get; set; }
    }
}
