using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class DoDuLieuDanhGiaDTO
    {
        public Guid Id { get; set; }
        public StaffInfo IdNhanVien { get; set; }
        public string MaCanBo { get; set; }
        public string TenCanBo { get; set; }
        public string NamHoc { get; set; }
        public string HocKy { get; set; }
        public DateTime? CreationTime { get; set; }
        public string MaTieuChi { get; set; }
        public string LoaiGiaTri { get; set; }
        public string DatMuc { get; set; }
        public double GiaTriThuc { get; set; }
        public string DonViCungCap { get; set; }
        public CriterionDictionary CriterionDictionaryId { get; set; }
        public FileGiangDay Path { get; set; }
        public string DuLieuMinhChung { get; set; }
        public string GhiChu { get; set; }
        public string Muc { get; set; }
        public string MoTa { get; set; }
        public string GiaTri { get; set; }
        public Department DonVi { get; set; }
        public string DepartmentName { get; set; }
        //public string FileName
        //{
        //    get
        //    {
        //        return this.Name + this.Extension;
        //    }
        //}
    }
}
