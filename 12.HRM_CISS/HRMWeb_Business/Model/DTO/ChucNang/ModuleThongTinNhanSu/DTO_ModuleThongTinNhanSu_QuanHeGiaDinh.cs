

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_QuanHeGiaDinh
    {
        [DataMember]
        public string HoTen { get; set; }
        [DataMember]
        public string GioiTinh { get; set; }
        [DataMember]
        public Nullable<int> NamSinh { get; set; }
        [DataMember]
        public string QuanHe { get; set; }
        [DataMember]
        public string DanToc { get; set; }
        [DataMember]
        public string TonGiao { get; set; }
        [DataMember]
        public string NuocCuTru { get; set; }
        [DataMember]
        public string QuocTich { get; set; }
        [DataMember]
        public string DienThoaiDiDong { get; set; }
        [DataMember]
        public string NgheNghiep { get; set; }
        [DataMember]
        public string NoiLamViec { get; set; }
        [DataMember]
        public string NoiOHienNay { get; set; }
        [DataMember]
        public string TinhTrang { get; set; }
    }
}
