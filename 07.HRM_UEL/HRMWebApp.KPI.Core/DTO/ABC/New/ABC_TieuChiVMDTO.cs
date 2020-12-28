using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO.ABC.New
{
    public class ABC_TieuChiVMDTO
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? BoTieuChiId { get; set; }
        public string ChiMuc { get; set; }
        public float? STT { get; set; }
        public string NoiDung { get; set; }
        public string ListDiem { get; set; }
        public float? DiemToiDa { get; set; }
        public bool? DiemTru { get; set; }
        public bool? ChildSelectOne { get; set; }
        public long? GCRecord { get; set; }
        public bool? IsDiemDanhGiaCongTac { get; set; }
        public bool? IsDiemThuong { get; set; }
        public DateTime? DeleteTime { get; set; }
        public Guid? DeleteUserId { get; set; }
        public DateTime? LastEditTime { get; set; }
        public Guid? LastEditUserId { get; set; }
        public DateTime? AddTime { get; set; }
        public Guid? AddUserId { get; set; }
    }
}
