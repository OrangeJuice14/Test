

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_GiangVienThinhGiang
    {
        [DataMember]
        public Guid Oid { get; set; }
        [DataMember]
        public string MaQuanLy { get; set; }
        [DataMember]
        public string Ho { get; set; }
        [DataMember]
        public string Ten { get; set; }
        [DataMember]
        public string TenBoPhan { get; set; }
        [DataMember]
        public Nullable<Guid> BoPhan { get; set; }
        [DataMember]
        public Nullable<DateTime> NgaySinh { get; set; }
        [DataMember]
        public string GioiTinh { get; set; }
        [DataMember]
        public string CMND { get; set; }
        [DataMember]
        public Nullable<DateTime> NgayCap { get; set; }
        [DataMember]
        public Nullable<Guid> NoiCap { get; set; }
        [DataMember]
        public Nullable<Guid> QuocGia { get; set; }
        [DataMember]
        public Nullable<Guid> TinhTrangHonNhan { get; set; }
        [DataMember]
        public Nullable<Guid> DanToc { get; set; }
        [DataMember]
        public Nullable<Guid> TonGiao { get; set; }
        [DataMember]
        public Nullable<Guid> Oid_NoiSinh { get; set; }
        [DataMember]
        public Nullable<Guid> QuocGia_NoiSinh { get; set; }
        [DataMember]
        public Nullable<Guid> TinhThanh_NoiSinh { get; set; }
        [DataMember]
        public Nullable<Guid> QuanHuyen_NoiSinh { get; set; }
        [DataMember]
        public Nullable<Guid> XaPhuong_NoiSinh { get; set; }
        [DataMember]
        public string SoNha_NoiSinh { get; set; }
        [DataMember]
        public Nullable<DateTime> NgayVaoCoQuan { get; set; }
        [DataMember]
        public string DonViCongTac { get; set; }
        [DataMember]
        public string HopDongHienTai { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string DienThoaiDiDong { get; set; }
        [DataMember]
        public string DienThoaiNhaRieng { get; set; }
        [DataMember]
        public Nullable<Guid> TinhTrang { get; set; }
        [DataMember]
        public Nullable<Guid> Oid_DCTT { get; set; }
        [DataMember]
        public Nullable<Guid> QuocGia_DCTT { get; set; }
        [DataMember]
        public Nullable<Guid> TinhThanh_DCTT { get; set; }
        [DataMember]
        public Nullable<Guid> QuanHuyen_DCTT { get; set; }
        [DataMember]
        public Nullable<Guid> XaPhuong_DCTT { get; set; }
        [DataMember]
        public string SoNha_DCTT { get; set; }
        [DataMember]
        public Nullable<Guid> Oid_NOHN { get; set; }
        [DataMember]
        public Nullable<Guid> QuocGia_NOHN { get; set; }
        [DataMember]
        public Nullable<Guid> TinhThanh_NOHN { get; set; }
        [DataMember]
        public Nullable<Guid> QuanHuyen_NOHN { get; set; }
        [DataMember]
        public Nullable<Guid> XaPhuong_NOHN { get; set; }
        [DataMember]
        public string SoNha_NOHN { get; set; }

        //Trinh Do
        [DataMember]
        public Nullable<Guid> Oid_NVTD { get; set; }
        [DataMember]
        public Nullable<Guid> TrinhDoVanHoa { get; set; }
        [DataMember]
        public Nullable<Guid> TrinhDoTinHoc { get; set; }
        [DataMember]
        public Nullable<Guid> HocHam { get; set; }
        [DataMember]
        public Nullable<Guid> TrinhDoChuyenMon { get; set; }
        [DataMember]
        public Nullable<Guid> ChuyenNganhDaoTao { get; set; }
        [DataMember]
        public Nullable<Guid> TruongDaoTao { get; set; }
        [DataMember]
        public Nullable<Guid> HinhThucDaoTao { get; set; }
        [DataMember]
        public Nullable<int> NamTotNghiep { get; set; }
        [DataMember]
        public Nullable<Guid> NgoaiNgu { get; set; }
        [DataMember]
        public Nullable<Guid> TrinhDoNgoaiNgu { get; set; }
        //ThongTinLuong
        [DataMember]
        public Nullable<Guid> Oid_NVTTL { get; set; }
        [DataMember]
        public Nullable<Guid> CoQuanThue { get; set; }
        [DataMember]
        public string MaSoThue { get; set; }
        //TaiKhoanNganHang
        [DataMember]
        public Nullable<Guid> Oid_TKNH { get; set; }
        [DataMember]
        public string SoTaiKhoan { get; set; }
        [DataMember]
        public Nullable<Guid> NganHang  { get; set; }

    }
}
