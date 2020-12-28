

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN_LUH
    {
        [DataMember]
        public DTO_HoSoNhanVien HoSo { get; set; }
        [DataMember]
        public DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan_LUH ChiTietThuNhapCaNhan { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietThuLaoGiangDay_LUH> DanhSach_ChiTietThuLaoGiangDay { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietPhuCap_LUH> DanhSach_ChiTietPhuCap { get; set; }
  
        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietKhenThuongPhucLoi_LUH> DanhSach_ChiTietKhenThuongPhucLoi { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietNgoaiGio_LUH> DanhSach_ChiTietNgoaiGio { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietThuNhapKhac_LUH> DanhSach_ChiTietThuNhapKhac { get; set; }
        
        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietKhauTruLuong_LUH> DanhSach_ChiTietKhauTruLuong { get; set; }

        [DataMember]
        public DTO_ModuleThongTinNhanSu_ChiTietThueTNCN_LUH ChiTietThueTNCN { get; set; }
    }
}
