

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN
    {
        [DataMember]
        public DTO_ModuleThongTinNhanSu_TrinhDoChuyenMon TrinhDoChuyenMon { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_VanBang> DanhSach_VanBang { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_NgoaiNgu> DanhSach_NgoaiNgu { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChungChi> DanhSach_ChungChi { get; set; }

    }
}
