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
    public partial class Mdl_ThongTinNghiPhep
    {
        /*
    	ko xai
    	public Mdl_ThongTinNghiPhep()
        {
            this.ChiTietNghiPheps = new HashSet<Mdl_ChiTietNghiPhep>();
        }*/
    
    	[DataMember]
        public System.Guid Oid { get; set; }
    	[DataMember]
        public Nullable<System.Guid> QuanLyNghiPhep { get; set; }
    	[DataMember]
        public Nullable<System.Guid> BoPhan { get; set; }
    	[DataMember]
        public Nullable<System.Guid> ThongTinNhanVien { get; set; }
    	[DataMember]
        public Nullable<decimal> SoNgayPhepCoBan { get; set; }
    	[DataMember]
        public Nullable<decimal> SoNgayPhepDaNghi { get; set; }
    	[DataMember]
        public Nullable<decimal> SoNgayPhepConLai { get; set; }
    	[DataMember]
        public string GhiChu { get; set; }
    	[DataMember]
        public Nullable<int> OptimisticLockField { get; set; }
    	[DataMember]
        public Nullable<int> GCRecord { get; set; }
    	[DataMember]
        public Nullable<decimal> SoNgayPhepCongThem { get; set; }
    	[DataMember]
        public Nullable<byte> TruNgayDiDuong { get; set; }
    	[DataMember]
        public Nullable<decimal> PhepNamTruocConLai { get; set; }
    	[DataMember]
        public Nullable<decimal> PhepNamNayConLai { get; set; }
    	[DataMember]
        public Nullable<decimal> SoNgayPhepNamTruoc { get; set; }
    
    	//[DataMember]
        //public virtual Mdl_BoPhan BoPhan1 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_ChiTietNghiPhep> ChiTietNghiPheps { get; set; }
    	//[DataMember]
        //public virtual Mdl_QuanLyNghiPhep QuanLyNghiPhep1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_ThongTinNhanVien ThongTinNhanVien1 { get; set; }
    }
}