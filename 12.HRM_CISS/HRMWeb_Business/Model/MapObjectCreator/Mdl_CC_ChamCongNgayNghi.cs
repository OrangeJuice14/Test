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
    public partial class Mdl_CC_ChamCongNgayNghi
    {
    	[DataMember]
        public System.Guid Oid { get; set; }
    	[DataMember]
        public Nullable<System.Guid> IDBoPhan { get; set; }
    	[DataMember]
        public Nullable<System.Guid> IDNhanVien { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> TuNgay { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> DenNgay { get; set; }
    	[DataMember]
        public Nullable<decimal> SoNgay { get; set; }
    	[DataMember]
        public string DienGiai { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgayTao { get; set; }
    	[DataMember]
        public Nullable<int> OptimisticLockField { get; set; }
    	[DataMember]
        public Nullable<int> GCRecord { get; set; }
    	[DataMember]
        public Nullable<System.Guid> IDWebUser { get; set; }
    	[DataMember]
        public Nullable<int> LoaiNghiPhep { get; set; }
    	[DataMember]
        public string DiaChiLienHe { get; set; }
    	[DataMember]
        public string TenGiayXinPhep { get; set; }
    	[DataMember]
        public Nullable<int> TrangThai_TP { get; set; }
    	[DataMember]
        public Nullable<int> TrangThai_Admin { get; set; }
    	[DataMember]
        public Nullable<System.Guid> IDHinhThucNghi { get; set; }
    	[DataMember]
        public string NguoiBanGiao { get; set; }
    	[DataMember]
        public Nullable<int> TrangThai_HT { get; set; }
    	[DataMember]
        public Nullable<int> TrangThai_HDQT { get; set; }
    	[DataMember]
        public Nullable<bool> IsBanGiamHieu { get; set; }
    	[DataMember]
        public Nullable<bool> IsTruongPhong { get; set; }
    	[DataMember]
        public Nullable<byte> Buoi { get; set; }
    	[DataMember]
        public Nullable<byte> JobUpdated { get; set; }
    	[DataMember]
        public Nullable<System.Guid> WebUserDuyet_TP { get; set; }
    	[DataMember]
        public Nullable<System.Guid> WebUserDuyet_Admin { get; set; }
    	[DataMember]
        public Nullable<System.Guid> WebUserDuyet_HT { get; set; }
    	[DataMember]
        public Nullable<System.Guid> WebUserDuyet_HDQT { get; set; }
    	[DataMember]
        public string PhanHoi_TP { get; set; }
    	[DataMember]
        public string PhanHoi_Admin { get; set; }
    	[DataMember]
        public string PhanHoi_HT { get; set; }
    	[DataMember]
        public string PhanHoi_HDQT { get; set; }
    
    	//[DataMember]
        //public virtual Mdl_ThongTinNhanVien ThongTinNhanVien { get; set; }
    	//[DataMember]
        //public virtual Mdl_WebUser WebUser { get; set; }
    	//[DataMember]
        //public virtual Mdl_CC_HinhThucNghi CC_HinhThucNghi { get; set; }
    	//[DataMember]
        //public virtual Mdl_BoPhan BoPhan { get; set; }
    }
}