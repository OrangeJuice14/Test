

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong
    {
        //Them
        [DataMember]
        public string SoChungTu { get; set; }
        /// //////////////////////////////////////
        [DataMember]
        public string HoTen { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        [DataMember]
        public string DiaChiThuongTru { get; set; }
        [DataMember]
        public string NoiSinh { get; set; }
        [DataMember]
        public string TenBoPhan { get; set; }
        [DataMember]
        public string TenChucDanh { get; set; }
        [DataMember]
        public string NgachLuong { get; set; }
        [DataMember]
        public string BacLuong { get; set; }
        [DataMember]
        public Nullable<decimal> HeSoLuong { get; set; }
    }
}
