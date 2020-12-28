using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_LogUser
    {
        public virtual Guid Id { get; set; }
        public virtual ABC_User ABC_User { get; set; }
        public virtual Position ChucVu { get; set; }
        public virtual StaffType LoaiNhanSu { get; set; }
        public virtual Department TaiBoMon { get; set; }
        public virtual Department DonVi { get; set; }
        public virtual ABC_KyDanhGia KyDanhGia { get; set; }
        public virtual ABC_GroupDanhGia GroupDanhGia { get; set; }
        public virtual bool? ThamGiaGiangDay { get; set; }
        public virtual bool? Status { get; set; }
        public virtual DateTime? TimeLog { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual Guid DeleteUserId { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid AddUserId { get; set; }
        public virtual long GCRecord { get; set; }
    }
}