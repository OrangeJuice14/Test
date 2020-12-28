
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_DienBienLuong
    {
        [DataMember]
        public Nullable<System.DateTime> NgayQuyetDinh { get; set; }
        [DataMember]
        public string SoQuyetDinh { get; set; }
        [DataMember]
        public string NoiDung { get; set; }
        [DataMember]
        public Nullable<System.DateTime> TuNgay { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DenNgay { get; set; }
        [DataMember]
        public string NgachLuong { get; set; }
        [DataMember]
        public string BacLuong { get; set; }
        [DataMember]
        public string LuongCoBan { get; set; }
        [DataMember]
        public string LuongKinhDoanh { get; set; }
        [DataMember]
        public string TongPhuCap { get; set; }
    }
}
