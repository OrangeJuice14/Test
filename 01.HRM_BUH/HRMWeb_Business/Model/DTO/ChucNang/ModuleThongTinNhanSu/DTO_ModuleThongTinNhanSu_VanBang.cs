

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_VanBang
    {
        [DataMember]
        public string TenTrinhDoChuyenMon { get; set; }
        [DataMember]
        public string ChuyenNganhDaoTao { get; set; }
        [DataMember]
        public string TruongDaoTao { get; set; }
        [DataMember]
        public string HinhThucDaoTao { get; set; }
        [DataMember]
        public Nullable<int> NamTotNghiep { get; set; }
        [DataMember]
        public string XepLoai { get; set; }
        [DataMember]
        public Nullable<decimal> DiemTrungBinh { get; set; }
        [DataMember]
        public string GhiChu { get; set; }
    }
}
