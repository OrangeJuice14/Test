using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_UserDanhGiaDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Guid? StaffInfoId { get; set; }
        public string StaffInfoStaffProfileName { get; set; }
        public string GroupDanhGiaName { get; set; } 
        public string GroupDanhGiaId { get; set; } 
        public string StaffInfoStaffSubjectName { get; set; } 
        public string StaffInfoStaffDepartmentName { get; set; } 
        public List<ABC_DanhGiaDTO> ListDanhGia { get; set; } 
        //public 
    }
}
