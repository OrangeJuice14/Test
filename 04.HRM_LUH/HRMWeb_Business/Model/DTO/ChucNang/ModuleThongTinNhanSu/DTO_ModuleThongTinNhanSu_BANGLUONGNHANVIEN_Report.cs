

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Report
    {
        [DataMember]
        public DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan_LUH ChiTietThuNhapCaNhan { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_LuongNganSach> BangThongTinThuNhap_LuongNganSach { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_LuongTangThem> BangThongTinThuNhap_LuongTangThem { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_Thue> BangThongTinThuNhap_Thue { get; set; }
  
        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_ThuNhapKhac> BangThongTinThuNhap_ThuNhapKhac { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_TruyLinh> BangThongTinThuNhap_TruyLinh { get; set; }

        [DataMember]
        public DTO_ModuleThongTinNhanSu_BangThongTinThuNhap_HeSo BangThongTinThuNhap_HeSo { get; set; }
    }
}
