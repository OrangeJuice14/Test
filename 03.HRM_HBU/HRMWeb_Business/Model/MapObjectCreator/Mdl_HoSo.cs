//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRMWeb_Business.Model.MapObjectCreator
{
    using System;
    using System.Collections.Generic;
    
    using System.Runtime.Serialization;//[DataContract],[DataMember]
    [DataContract]
    public partial class Mdl_HoSo
    {
        /*
    	ko xai
    	public Mdl_HoSo()
        {
            this.CC_ChamCongNgayNghi = new HashSet<Mdl_CC_ChamCongNgayNghi>();
            this.GiayToHoSoes = new HashSet<Mdl_GiayToHoSo>();
            this.WebUsers = new HashSet<Mdl_WebUser>();
            this.CC_KhaiBaoCongTac = new HashSet<Mdl_CC_KhaiBaoCongTac>();
            this.CC_ChamCongTheoNgay = new HashSet<Mdl_CC_ChamCongTheoNgay>();
        }*/
    
    	[DataMember]
        public System.Guid Oid { get; set; }
    	[DataMember]
        public string STT { get; set; }
    	[DataMember]
        public string MaQuanLy { get; set; }
    	[DataMember]
        public string Ho { get; set; }
    	[DataMember]
        public string Ten { get; set; }
    	[DataMember]
        public string HoTen { get; set; }
    	[DataMember]
        public string TenGoiKhac { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgaySinh { get; set; }
    	[DataMember]
        public Nullable<System.Guid> NoiSinh { get; set; }
    	[DataMember]
        public Nullable<byte> GioiTinh { get; set; }
    	[DataMember]
        public string CMND { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgayCap { get; set; }
    	[DataMember]
        public Nullable<System.Guid> NoiCap { get; set; }
    	[DataMember]
        public string SoHoChieu { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgayCapHoChieu { get; set; }
    	[DataMember]
        public string NoiCapHoChieu { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgayHetHan { get; set; }
    	[DataMember]
        public Nullable<System.Guid> QueQuan { get; set; }
    	[DataMember]
        public Nullable<System.Guid> DiaChiThuongTru { get; set; }
    	[DataMember]
        public Nullable<System.Guid> NoiOHienNay { get; set; }
    	[DataMember]
        public string Email { get; set; }
    	[DataMember]
        public string DienThoaiDiDong { get; set; }
    	[DataMember]
        public string DienThoaiNhaRieng { get; set; }
    	[DataMember]
        public Nullable<System.Guid> TinhTrangHonNhan { get; set; }
    	[DataMember]
        public Nullable<System.Guid> DanToc { get; set; }
    	[DataMember]
        public Nullable<System.Guid> TonGiao { get; set; }
    	[DataMember]
        public Nullable<System.Guid> QuocTich { get; set; }
    	[DataMember]
        public Nullable<byte> HinhThucTuyenDung { get; set; }
    	[DataMember]
        public string GhiChu { get; set; }
    	[DataMember]
        public Nullable<int> IDNhanSu_ChamCong { get; set; }
    	[DataMember]
        public Nullable<int> OptimisticLockField { get; set; }
    	[DataMember]
        public Nullable<int> GCRecord { get; set; }
    	[DataMember]
        public Nullable<int> ObjectType { get; set; }
    
    	//[DataMember]
        //public virtual ICollection<Mdl_CC_ChamCongNgayNghi> CC_ChamCongNgayNghi { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_GiayToHoSo> GiayToHoSoes { get; set; }
    	//[DataMember]
        //public virtual Mdl_NhanVien NhanVien { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_WebUser> WebUsers { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_CC_KhaiBaoCongTac> CC_KhaiBaoCongTac { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_CC_ChamCongTheoNgay> CC_ChamCongTheoNgay { get; set; }
    	//[DataMember]
        //public virtual Mdl_DanToc DanToc1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_QuocGia QuocGia { get; set; }
    	//[DataMember]
        //public virtual Mdl_TinhTrangHonNhan TinhTrangHonNhan1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_TonGiao TonGiao1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_TinhThanh TinhThanh { get; set; }
    	//[DataMember]
        //public virtual Mdl_DiaChi DiaChi { get; set; }
    	//[DataMember]
        //public virtual Mdl_DiaChi DiaChi1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_DiaChi DiaChi2 { get; set; }
    	//[DataMember]
        //public virtual Mdl_DiaChi DiaChi3 { get; set; }
    }
}