

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc
    {
        [DataMember]
        public Nullable<int> TuNam { get; set; }
        [DataMember]
        public Nullable<int> DenNam { get; set; }
        [DataMember]
        public string CapQuanLy { get; set; }
        [DataMember]
        public string CoQuanChuTri { get; set; }
        [DataMember]
        public string ChucDanhThamGia { get; set; }
        [DataMember]
        public string TenDeTai { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayNghiemThu { get; set; }
        [DataMember]
        public string NoiQuanLyKetQua { get; set; }
    }
}
