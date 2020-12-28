using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_CapNhatQuanHeGiaDinh
    {
        [DataMember]
        public Guid Oid { get; set; }
        [DataMember]
        public Guid ThongTinNhanVien { get; set; }
        [DataMember]
        public string HoTenNguoiThan { get; set; }
        [DataMember]
        public Nullable<System.DateTime> NgaySinhFull { get; set; }
        [DataMember]
        public string NoiSinh { get; set; }
        [DataMember]
        public Nullable<System.Guid> QuocTich { get; set; }
        [DataMember]
        public string TenQuocTich { get; set; }
        [DataMember]
        public string CMND { get; set; }
        [DataMember]
        public string SoHoChieu { get; set; }
        [DataMember]
        public Nullable<System.Guid> QuanHe { get; set; }
        [DataMember]
        public string TenQuanHe { get; set; }
        [DataMember]
        public Nullable<System.Guid> LoaiGiamTruGiaCanh { get; set; }
        [DataMember]
        public string TenLoaiGiamTruGiaCanh { get; set; }
        [DataMember]
        public Nullable<bool> PhuThuoc { get; set; }
        [DataMember]
        public Nullable<bool> LienHeKhanCap { get; set; }
        [DataMember]
        public string DienThoaiDiDong { get; set; }
        [DataMember]
        public Nullable<int> GioiTinh { get; set; }
        [DataMember]
        public Nullable<System.Guid> QueQuan { get; set; }
        [DataMember]
        public string TenQueQuan { get; set; }
        [DataMember]
        public string NgheNghiepHienTai { get; set; }
        [DataMember]
        public string NoiLamViec { get; set; }
        [DataMember]
        public Nullable<int> TinhTrang { get; set; }
    }
}
