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
    public partial class DTO_QuanLyNghiPhep_Find
    {
        //Them vao
        [DataMember]
        public String MaQuanLy { get; set; }
        [DataMember]
        public String HoTen { get; set; }
        [DataMember]
        public String TenPhongBan { get; set; }
        [DataMember]
        public System.Guid Oid { get; set; }     
        [DataMember]
        public string TongSoNgayPhep { get; set; }
        [DataMember]
        public string SoNgayPhepDaNghi { get; set; }
        [DataMember]
        public string SoNgayPhepConLai { get; set; }
        [DataMember]
        public string SoNgayPhepCongThem { get; set; }


    }
}