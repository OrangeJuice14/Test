//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace HRMWeb_Business.Model
{
    using System;
    using System.Collections.Generic;
    [DataContract]
    public partial class DTO_HoSoNhanVien
    {
        //Them vao
        [DataMember] 
        public Nullable<System.Guid> MaBoPhan { get; set; }
        //public DTO_BoPhan DTOBoPhan { get; set; }
        //
        [DataMember] 
        public System.Guid Oid { get; set; }
        [DataMember]
        public string MaQuanLy { get; set; }
        [DataMember]
        public string TenPhongBan { get; set; }
        [DataMember] 
        public string Ho { get; set; }
        [DataMember]
        public string Ten { get; set; }
        [DataMember] 
        public string HoTen { get; set; }
        //public string TenGoiKhac { get; set; }
        //public Nullable<System.DateTime> NgaySinh { get; set; }
        //public Nullable<System.Guid> NoiSinh { get; set; }
        [DataMember]
        public Nullable<byte> GioiTinh { get; set; }
        [DataMember] 
        public string CMND { get; set; }
        //public Nullable<System.DateTime> NgayCap { get; set; }
        //public Nullable<System.Guid> NoiCap { get; set; }
        //public string SoHoChieu { get; set; }
        //public Nullable<System.DateTime> NgayCapHoChieu { get; set; }
        //public string NoiCapHoChieu { get; set; }
        //public Nullable<System.DateTime> NgayHetHan { get; set; }
        //public Nullable<System.Guid> QueQuan { get; set; }
        //public Nullable<System.Guid> DiaChiThuongTru { get; set; }
        //public Nullable<System.Guid> NoiOHienNay { get; set; }
        [DataMember] 
        public string Email { get; set; }
        [DataMember] 
        public string DienThoaiDiDong { get; set; }
        [DataMember] 
        public string DienThoaiNhaRieng { get; set; }
        //public Nullable<System.Guid> TinhTrangHonNhan { get; set; }
        //public Nullable<System.Guid> DanToc { get; set; }
        //public Nullable<System.Guid> TonGiao { get; set; }
        //public Nullable<System.Guid> QuocTich { get; set; }
        //public Nullable<byte> HinhThucTuyenDung { get; set; }
        [DataMember] 
        public string GhiChu { get; set; }
        //public Nullable<int> OptimisticLockField { get; set; }
        //public Nullable<int> GCRecord { get; set; }
        //public Nullable<int> ObjectType { get; set; }
    
        //public virtual ICollection<Mdl_CC_ChamCongTheoNgay> CC_ChamCongTheoNgay { get; set; }
        //public virtual Mdl_NhanVien NhanVien { get; set; }
        //public virtual Mdl_WebUser WebUser { get; set; }



    }
}