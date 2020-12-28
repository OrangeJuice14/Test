using HRMWebApp.Helpers;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_UserCreateDTO
    {
        public Guid Id { get; set; }
        public bool? ThamGiaGiangDay { get; set; }
        public bool? Status { get; set; }
        public DateTime? DeleteTime { get; set; }
        public DateTime? AddTime { get; set; }
        public Guid? DeleteUserId { get; set; }
        public Guid? AddUserId { get; set; }
        public Guid? WebUserId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? KyDanhGiaId { get; set; }
        public Guid? GroupDanhGiaId { get; set; }
        public string StaffTypeManageCode { get; set; }
        //public List<ABC_DanhGiaVMDTO> ListDanhGia { get; set; }
        public List<ABC_GroupDanhGiaVMDTO> ListGroupDanhGia { get; set; }
        //public 
    }
}
