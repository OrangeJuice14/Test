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
    public partial class Mdl_GiayToHoSo
    {
        /*
    	ko xai
    	public Mdl_GiayToHoSo()
        {
            this.QuyetDinhCaNhans = new HashSet<Mdl_QuyetDinhCaNhan>();
            this.HopDongs = new HashSet<Mdl_HopDong>();
        }*/
    
    	[DataMember]
        public System.Guid Oid { get; set; }
    	[DataMember]
        public Nullable<System.Guid> HoSo { get; set; }
    	[DataMember]
        public Nullable<System.Guid> GiayTo { get; set; }
    	[DataMember]
        public string SoGiayTo { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgayBanHanh { get; set; }
    	[DataMember]
        public Nullable<System.DateTime> NgayLap { get; set; }
    	[DataMember]
        public string DuongDanFile { get; set; }
    	[DataMember]
        public Nullable<System.Guid> DangLuuTru { get; set; }
    	[DataMember]
        public Nullable<int> SoBan { get; set; }
    	[DataMember]
        public string TrichYeu { get; set; }
    	[DataMember]
        public string LuuTru { get; set; }
    	[DataMember]
        public Nullable<int> OptimisticLockField { get; set; }
    	[DataMember]
        public Nullable<int> GCRecord { get; set; }
    
    	//[DataMember]
        //public virtual Mdl_DangLuuTru DangLuuTru1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_GiayTo GiayTo1 { get; set; }
    	//[DataMember]
        //public virtual Mdl_HoSo HoSo1 { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_QuyetDinhCaNhan> QuyetDinhCaNhans { get; set; }
    	//[DataMember]
        //public virtual ICollection<Mdl_HopDong> HopDongs { get; set; }
    }
}