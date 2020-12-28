using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ImportStaffInOtherActivity
    {
        public virtual Guid Id { get; set; }
        public virtual string MaCanBo { get; set; }
        public virtual string TenCanBo { get; set; }
        public virtual float SoGio { get; set; }
        public virtual Department DonVi { get; set; }
    }
}
