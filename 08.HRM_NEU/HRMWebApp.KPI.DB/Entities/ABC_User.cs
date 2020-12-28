using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class ABC_User
    {
        public ABC_User()
        { 
        }
        public ABC_User(WebUser webUser)
        {
            ThamGiaGiangDay = webUser.StaffInfo.ThamGiaGiangDay;
            WebUser = webUser;
            Position = webUser.StaffInfo.Position;
            StaffType = webUser.StaffInfo.StaffType;
            if(webUser.StaffInfo.Subject!=null)
            Subject = webUser.StaffInfo.Subject;
            Department = webUser.StaffInfo.Staff.Department;
        }
        public virtual Guid Id { get; set; }
        public virtual bool? ThamGiaGiangDay { get; set; }
        public virtual bool? Status { get; set; }
        public virtual DateTime? DeleteTime { get; set; }
        public virtual DateTime? AddTime { get; set; }
        public virtual Guid? DeleteUserId { get; set; }
        public virtual Guid? AddUserId { get; set; }
        public virtual WebUser WebUser { get; set; }
        public virtual Position Position { get; set; }
        public virtual StaffType StaffType { get; set; }
        public virtual Department Subject { get; set; }
        public virtual Department Department { get; set; }
        public virtual ABC_KyDanhGia KyDanhGia { get; set; } 
        public virtual ABC_GroupDanhGia GroupDanhGia { get; set; } 
    }
}
