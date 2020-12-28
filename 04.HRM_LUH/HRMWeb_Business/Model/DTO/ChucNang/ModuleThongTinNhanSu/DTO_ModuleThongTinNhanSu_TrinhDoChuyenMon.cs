
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_TrinhDoChuyenMon
    {
        [DataMember]
        public string TrinhDoHocVan { get; set; }
        [DataMember]
        public string TrinhDoChuyenMonCaoNhat { get; set; }
        [DataMember]
        public string ChuyenNganhDaoTao { get; set; }
        [DataMember]
        public string TruongDaoTao { get; set; }
        [DataMember]
        public string HinhThucDaoTao { get; set; }
        [DataMember]
        public Nullable<int> NamTotNghiep { get; set; }
        [DataMember]
        public string HocHam { get; set; }
        [DataMember]
        public string NamCongNhan { get; set; }
        [DataMember]
        public string DangTheoHoc { get; set; }
        [DataMember]
        public string LyLuanChinhTri { get; set; }
        [DataMember]
        public string QuanLyNhaNuoc { get; set; }
        [DataMember]
        public string QuanLyKinhTe { get; set; }
        [DataMember]
        public string QuanLyGiaoDuc { get; set; }
        [DataMember]
        public string TrinhDoTinHoc { get; set; }
        [DataMember]
        public string NgoaiNguChinh { get; set; }
        [DataMember]
        public string TrinhDoNgoaiNguChinh { get; set; }
    }
}
