using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_CapNhatHoSo
    {
        [DataMember]
        public Guid ThongTinNhanVien { get; set; }
        [DataMember]
        public Nullable<System.Guid> TonGiao { get; set; }
        [DataMember]
        public string TenTonGiao { get; set; }
        [DataMember]
        public Nullable<int> ChieuCao { get; set; }
        [DataMember]
        public Nullable<int> CanNang { get; set; }
        [DataMember]
        public Nullable<System.Guid> TinhTrangHonNhan { get; set; }
        [DataMember]
        public string TenTinhTrangHonNhan { get; set; }
        [DataMember]
        public string CMND { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgayCap { get; set; }
        [DataMember]
        public Nullable<System.Guid> NoiCap { get; set; }
        [DataMember]
        public string TenNoiCap { get; set; }
        [DataMember]
        public string DienThoaiDiDong { get; set; }
    }
}
