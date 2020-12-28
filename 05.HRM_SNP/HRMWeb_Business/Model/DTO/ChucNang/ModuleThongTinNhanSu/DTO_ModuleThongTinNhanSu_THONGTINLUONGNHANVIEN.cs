
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN
    {
        [DataMember]
        public DTO_ModuleThongTinNhanSu_ThongTinLuong ThongTinLuong { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_TaiKhoanNganHang> DanhSach_TaiKhoanNganHang { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_NguoiPhuThuoc> DanhSach_NguoiPhuThuoc { get; set; }

    }
}
