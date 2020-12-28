using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ThongTinDanhGia
    {
        public virtual Guid Id { get; set; }
        public virtual StaffInfo StaffInfo { get; set; }
        public virtual bool DanhGia { get; set; }
        public virtual ABC_RatingType ABC_RatingType { get; set; }
        public virtual string MaDoiTuongDanhGia { get; set; }
        public virtual ABC_EvaluationBoard ABC_EvaluationBoard { get; set; }
    }
}
