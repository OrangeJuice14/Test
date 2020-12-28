using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
   public class DanhMucMTCLDTO
    {
        public  Guid Id { get; set; }
        public ManageCode  OidDanhMucCha { get; set; }
        public string MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public int CapDanhMuc { get; set; }
        public Department DonViPhuTrach { get; set; }
        public Staff BGHPhuTrach { get; set; }
        public List<Guid> StudyYearIds { get; set; }
        public IList<StudyYear> NamHoc { get; set; }
    }
}
