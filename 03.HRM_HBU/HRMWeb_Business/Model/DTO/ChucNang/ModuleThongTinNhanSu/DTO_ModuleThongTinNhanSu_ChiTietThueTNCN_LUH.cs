

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChiTietThueTNCN_LUH
    {
        [DataMember]
        public decimal TongThuNhapChiuThue { get; set; }
        [DataMember]
        public decimal GiamTruGiaCanh { get; set; }
        [DataMember]
        public decimal GiamTruBanThan { get; set; }
        [DataMember]
        public decimal GiamTruNguoiPhuThuoc { get; set; }
        [DataMember]
        public decimal ThuNhapTinhThue { get; set; }
        [DataMember]
        public decimal ThueTNCNPhaiNop { get; set; }
    }

}
