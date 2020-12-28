

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_BANGLUONGNHANVIEN
    {

        [DataMember]
        public DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan ChiTietThuNhapCaNhan { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietThuLaoGiangDay> DanhSach_ChiTietThuLaoGiangDay { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietPhuCap> DanhSach_ChiTietPhuCap { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietKhenThuongPhucLoi> DanhSach_ChiTietKhenThuongPhucLoi { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietNgoaiGio> DanhSach_ChiTietNgoaiGio { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietThuNhapKhac> DanhSach_ChiTietThuNhapKhac { get; set; }

        [DataMember]
        public IEnumerable<DTO_ModuleThongTinNhanSu_ChiTietKhauTruLuong> DanhSach_ChiTietKhauTruLuong { get; set; }

        [DataMember]
        public DTO_ModuleThongTinNhanSu_ChiTietThueTNCN ChiTietThueTNCN { get; set; }
    }
}
