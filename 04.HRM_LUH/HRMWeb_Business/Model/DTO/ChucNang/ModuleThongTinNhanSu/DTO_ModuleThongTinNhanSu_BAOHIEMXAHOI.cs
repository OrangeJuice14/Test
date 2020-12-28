

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BAOHIEMXAHOI
    {
        [DataMember]
        public DTO_ModuleThongTinNhanSu_HoSoBaoHiem HoSoBaoHiem { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_QuaTrinhThamGiaBHXH> DanhSach_QuaTrinhThamGiaBHXH { get; set; }

   
    }
}
