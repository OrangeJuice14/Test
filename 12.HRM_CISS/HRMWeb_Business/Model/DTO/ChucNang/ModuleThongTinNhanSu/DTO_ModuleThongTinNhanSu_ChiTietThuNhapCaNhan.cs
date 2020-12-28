

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan
    {
        [DataMember]
        public string MaTapDoan { get; set; }

        [DataMember]
        public string MaNhanVien { get; set; }

        [DataMember]
        public string HoTen { get; set; }

        [DataMember]
        public string TenChucVu { get; set; }

        [DataMember]
        public string TenChucDanh { get; set; }

        [DataMember]
        public string TenBoPhan { get; set; }

        [DataMember]
        public string NgayVaoCT { get; set; }

        [DataMember]
        public string LuongChucDanh_QD { get; set; }

        [DataMember]
        public string LuongHieuQuaCongViec_QD { get; set; }

        [DataMember]
        public string LuongDongBaoHiem { get; set; }

        [DataMember]
        public string LuongGross { get; set; }

        [DataMember]
        public string NgayCongChuan { get; set; }

        [DataMember]
        public string NgayCongThucTe { get; set; }

        [DataMember]
        public string NgayHuongLuong { get; set; }

        [DataMember]
        public string NgayHuongBHXH { get; set; }

        [DataMember]
        public string NgayKhongLuong { get; set; }

        [DataMember]
        public string NgayPhep { get; set; }

        [DataMember]
        public string PhanTramTinhLuong { get; set; }

        [DataMember]
        public string LuongChucDanh { get; set; }

        [DataMember]
        public string LuongHieuQuaCongViec { get; set; }

        [DataMember]
        public string LuongThangNay { get; set; }

        [DataMember]
        public string LuongThang13 { get; set; }

        [DataMember]
        public string DieuChinhLuong_ChiuThue { get; set; }

        [DataMember]
        public string PhuCapChiuThue { get; set; }

        [DataMember]
        public string LuongTangCaChiuThue { get; set; }

        [DataMember]
        public string KhoanChiuThueNgoaiLuong { get; set; }

        [DataMember]
        public string PhuCapKhongChiuThue { get; set; }

        [DataMember]
        public string LuongTangCaKhongChiuThue { get; set; }

        [DataMember]
        public string KhoanKhongChiuThueNgoaiLuong { get; set; }

        [DataMember]
        public string ThuNhapChiuThue { get; set; }

        [DataMember]
        public string BHXH_NLD { get; set; }

        [DataMember]
        public string BHYT_NLD { get; set; }

        [DataMember]
        public string BHTN_NLD { get; set; }

        [DataMember]
        public string TongBaoHiem { get; set; }

        [DataMember]
        public string CongDoan { get; set; }

        [DataMember]
        public string TongGiamTruGiaCanh { get; set; }

        [DataMember]
        public string ThueTNCN { get; set; }

        [DataMember]
        public string KhoanDieuChinhSauThue { get; set; }

        [DataMember]
        public string ThucLanh { get; set; }

        [DataMember]
        public string KyTinhLuong { get; set; }

        [DataMember]
        public string KyTinhLuongThang { get; set; }

        [DataMember]
        public string KyTinhLuongNam { get; set; }

        [DataMember]
        public string TenCongTy { get; set; }
    }
}
