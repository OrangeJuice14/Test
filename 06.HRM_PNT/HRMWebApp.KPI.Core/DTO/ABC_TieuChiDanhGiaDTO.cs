using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_TieuChiDanhGiaDTO
    {
        public Guid Id { get; set; }
        public string STT { get; set; }
        public string TenTieuChi { get; set; }
        public int DiemToiDa { get; set; }
        public bool ChildSelectOne { get; set; }
        public Guid? ParentId { get; set; }
        public bool ParentChildSelectOne { get; set; }
        public string ParentSTT { get; set; } 
        public Guid DanhGiaId { get; set; }
    }
}
