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
    public partial class Mdl_DiaChi
    {
        /*
    	ko xai
    	public Mdl_DiaChi()
        {
            this.HoSoes = new HashSet<Mdl_HoSo>();
            this.HoSoes1 = new HashSet<Mdl_HoSo>();
            this.HoSoes2 = new HashSet<Mdl_HoSo>();
            this.HoSoes3 = new HashSet<Mdl_HoSo>();
            this.Web_UpdateHoSoNhanVien = new HashSet<Mdl_Web_UpdateHoSoNhanVien>();
            this.Web_UpdateHoSoNhanVien1 = new HashSet<Mdl_Web_UpdateHoSoNhanVien>();
            this.Web_UpdateHoSoNhanVien2 = new HashSet<Mdl_Web_UpdateHoSoNhanVien>();
            this.Web_UpdateHoSoNhanVien3 = new HashSet<Mdl_Web_UpdateHoSoNhanVien>();
        }*/
    
    	[DataMember]
        public System.Guid Oid { get; set; }
    	[DataMember]
        public Nullable<System.Guid> QuocGia { get; set; }
    	[DataMember]
        public Nullable<System.Guid> TinhThanh { get; set; }
    	[DataMember]
        public Nullable<System.Guid> QuanHuyen { get; set; }
    	[DataMember]
        public Nullable<System.Guid> XaPhuong { get; set; }
    	[DataMember]
        public string SoNha { get; set; }
    	[DataMember]
        public string FullDiaChi { get; set; }
    	[DataMember]
        public Nullable<int> OptimisticLockField { get; set; }
    	[DataMember]
        public Nullable<int> GCRecord { get; set; }
    
    	//[DataMember]
        //public virtual Mdl_TinhThanh TinhThanh1 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_HoSo> HoSoes { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_HoSo> HoSoes1 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_HoSo> HoSoes2 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_HoSo> HoSoes3 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_Web_UpdateHoSoNhanVien> Web_UpdateHoSoNhanVien { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_Web_UpdateHoSoNhanVien> Web_UpdateHoSoNhanVien1 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_Web_UpdateHoSoNhanVien> Web_UpdateHoSoNhanVien2 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_Web_UpdateHoSoNhanVien> Web_UpdateHoSoNhanVien3 { get; set; }
    	//[DataMember]
        //public virtual Mdl_QuanHuyen QuanHuyen1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_QuocGia QuocGia1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_XaPhuong XaPhuong1 { get; set; }
    }
}