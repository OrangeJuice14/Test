using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
   public class DanhMucMTCL
    {
        public DanhMucMTCL()
        {
            NamHoc = new List<StudyYear>();
        }
        public virtual Guid Id { get; set; }
        public virtual ManageCode OidDanhMucCha { get; set; }
        public virtual string MaDanhMuc { get; set; }
        public virtual string TenDanhMuc { get; set; }
        public virtual int CapDanhMuc { get; set; }
        public virtual Department DonViPhuTrach { get; set; }
        public virtual Staff BGHPhuTrach { get; set; }
        public virtual IList<StudyYear> NamHoc { get; set; }
    }
}
