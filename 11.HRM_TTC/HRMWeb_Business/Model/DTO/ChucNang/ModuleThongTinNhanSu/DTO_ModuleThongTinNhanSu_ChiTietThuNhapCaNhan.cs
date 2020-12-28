

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ChiTietThuNhapCaNhan
    {

        [DataMember]
        public string MaNhanVien { get; set; }

        [DataMember]
        public string MaTapDoan { get; set; }

        [DataMember]
        public string TenBoPhan { get; set; }

        [DataMember]
        public string HoTen { get; set; }

        [DataMember]
        public string TenChucDanh { get; set; }

        [DataMember]
        public string LuongChucDanh { get; set; }

        [DataMember]
        public string LuongHieuQuaCongViec { get; set; }

        [DataMember]
        public string LuongChucDanh_QD { get; set; }

        [DataMember]
        public string LuongHieuQuaCongViec_QD { get; set; }

        [DataMember]
        public string LuongGross { get; set; }

        [DataMember]
        public string NgayVaoCT { get; set; }

        [DataMember]
        public string NgayCongChuan { get; set; }

        [DataMember]
        public string NgayCongThucTe { get; set; }

        [DataMember]
        public string NgayHuongBHXH { get; set; }

        [DataMember]
        public string NgayKhongLuong { get; set; }

        [DataMember]
        public string NgayPhep { get; set; }

        [DataMember]
        public string PhanTramTinhLuong { get; set; }

        [DataMember]
        public string PhuCapTrachNhiem { get; set; }

        [DataMember]
        public string PhuCapKiemNhiem { get; set; }

        [DataMember]
        public string NgoaiGio { get; set; }

        [DataMember]
        public string HuuTri { get; set; }

        [DataMember]
        public string TruyLanh { get; set; }

        [DataMember]
        public string LuongThang13 { get; set; }

        [DataMember]
        public string ThuNhapKhac { get; set; }

        [DataMember]
        public string TongThuNhap { get; set; }

        [DataMember]
        public string TongBaoHiem { get; set; }

        [DataMember]
        public string CongDoan { get; set; }

        [DataMember]
        public string TruyThu { get; set; }

        [DataMember]
        public string KhauTruKhac { get; set; }

        [DataMember]
        public string ThueTNCN { get; set; }

        [DataMember]
        public string TienDongPhuc { get; set; }

        [DataMember]
        public string ThucLanh { get; set; }

        [DataMember]
        public string CacKhoanCong { get; set; }

        [DataMember]
        public string TongTru { get; set; }

        [DataMember]
        public string KyTinhLuong { get; set; }

        [DataMember]
        public string DuGio { get; set; }

        [DataMember]
        public string TenCongTy { get; set; }
    }
}
