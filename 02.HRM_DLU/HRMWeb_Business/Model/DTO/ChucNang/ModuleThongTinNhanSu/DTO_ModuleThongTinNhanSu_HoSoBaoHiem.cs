

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_HoSoBaoHiem
    {
        [DataMember]
        public string SoSoBHXH { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayThamGiaBHXH { get; set; }
        [DataMember]
        public string SoTheBHYT { get; set; }
        [DataMember]
        public Nullable<System.DateTime> TuNgay { get; set; }
        [DataMember]
        public Nullable<System.DateTime> DenNgay { get; set; }
        [DataMember]
        public string NoiDangKyKCB { get; set; }
        [DataMember]
        public string QuyenLoiHuongBHYT { get; set; }
        [DataMember]
        public string ThamGiaBHTN { get; set; }
    }
}
