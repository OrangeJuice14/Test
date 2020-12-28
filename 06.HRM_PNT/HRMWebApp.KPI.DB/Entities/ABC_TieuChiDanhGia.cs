using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_TieuChiDanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual string STT { get; set; } 
        public virtual string TenTieuChi { get; set; }
        public virtual int DiemToiDa { get; set; }
        public virtual bool ChildSelectOne { get; set; }
        public virtual ABC_TieuChiDanhGia Parent { get; set; }
        public virtual ABC_DanhGia DanhGia { get; set; } 
    }
}
