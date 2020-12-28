
using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_ModuleThongTinNhanSu_ThongTinLuong
    {
        [DataMember]
        public Nullable<Decimal> MucLuong { get; set; }
        [DataMember]
        public Nullable<Decimal> ThuongHieuQuaTheoThang { get; set; }
        [DataMember]
        public Nullable<DateTime> NgayDieuChinhMucThuNhap { get; set; }
        [DataMember]
        public Nullable<bool> KhongThamGiaCongDoan { get; set; }
        [DataMember]
        public Nullable<bool> KhongDongBaoHiem { get; set; }
        [DataMember]
        public Nullable<DateTime> NgayCapMaSoThue { get; set; }
        [DataMember]
        public Nullable<Decimal> TinhThueTNCNTheoMacDinh { get; set; }
        [DataMember]
        public string PhuongThuocTinhThue { get; set; }
        [DataMember]
        public Nullable<Decimal> PCTienAn { get; set; }
        [DataMember]
        public Nullable<Decimal> PCDienThoai { get; set; }
        [DataMember]
        public Nullable<Decimal> PCTienXang { get; set; }
        [DataMember]
        public Nullable<Decimal> TienTroCapChucVu { get; set; }
        [DataMember]
        public Nullable<DateTime> NgayHuongTienTroCapChucVu { get; set; }
        [DataMember]
        public Nullable<Decimal> PhuCapTrachNhiemCongViec { get; set; }      
    }
}
