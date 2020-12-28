using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class HistoryThongTinHoSoNhanVien
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime? TimeLog { get; set; }
        public virtual bool? ThamGiaGiangDay { get; set; }
        public virtual Department Department { get; set; }
        public virtual StaffInfo StaffInfo { get; set; }
        public virtual Position Position { get; set; }
        public virtual StaffType StaffType { get; set; }
        public virtual Department Subject { get; set; }
    }
}
