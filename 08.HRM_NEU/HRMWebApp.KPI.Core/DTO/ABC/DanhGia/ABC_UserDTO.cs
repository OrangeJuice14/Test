using HRMWebApp.Helpers;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_UserDTO
    {
        public ABC_UserDTO()
        {

        }
        public ABC_UserDTO(WebUser webUser)
        {
            ThamGiaGiangDay = webUser.StaffInfo.ThamGiaGiangDay;
            WebUserStaffInfoId = webUser.StaffInfo.Id;
            WebUserStaffInfoStaffProfileName = webUser.StaffInfo.StaffProfile.Name;
            WebUserUserName = webUser.UserName;
            WebUserId = webUser.Id;
            if (webUser.StaffInfo.Subject != null)
            {
                SubjectId = webUser.StaffInfo.Subject.Id;
                SubjectName = webUser.StaffInfo.Subject.Name;
            }
            DepartmentId = webUser.StaffInfo.Staff.Department.Id;
            DepartmentName = webUser.StaffInfo.Staff.Department.Name;

        }
        public ABC_UserDTO(List<ABC_User> listWebUser)
        {
            ListGroupDanhGia = new List<ABC_GroupDanhGiaDTO>();
            ThamGiaGiangDay = listWebUser[0].ThamGiaGiangDay;
            Status = listWebUser[0].Status;
            WebUserStaffInfoId = listWebUser[0].WebUser.StaffInfo.Id;
            WebUserStaffInfoStaffProfileName = listWebUser[0].WebUser.StaffInfo.StaffProfile.Name;
            WebUserUserName = listWebUser[0].WebUser.UserName;
            WebUserId = listWebUser[0].Id;
            if (listWebUser[0].WebUser.StaffInfo.Subject != null)
            {
                SubjectId = listWebUser[0].Subject.Id;
                SubjectName = listWebUser[0].WebUser.StaffInfo.Subject.Name;
            }
            DepartmentId = listWebUser[0].Department.Id;
            DepartmentName = listWebUser[0].Department.Name;

            int CountListWebUser = listWebUser.Count;

            for (int i = 0; i < CountListWebUser; i++)
            {
                ListGroupDanhGia.Add(listWebUser[i].GroupDanhGia.Map<ABC_GroupDanhGiaDTO>());
            }

        }
        public Guid Id { get; set; }
        public bool? ThamGiaGiangDay { get; set; }
        public bool? Status { get; set; }
        public DateTime? DeleteTime { get; set; }
        public DateTime? AddTime { get; set; }
        public Guid? DeleteUserId { get; set; }
        public Guid? AddUserId { get; set; }
        public Guid? WebUserStaffInfoId { get; set; }
        public string WebUserStaffInfoStaffProfileName { get; set; }
        public string WebUserUserName { get; set; }
        public string SubjectName { get; set; }
        public Guid? WebUserId { get; set; }
        public Guid? SubjectId { get; set; }
        public string DepartmentName { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? KyDanhGiaId { get; set; }
        public Guid? GroupDanhGiaId { get; set; }
        public string GroupDanhGiaName { get; set; }
        public string StaffTypeManageCode { get; set; }
        public bool? GroupDanhGiaHasQuanLyDonVi { get; set; }
        public List<ABC_DanhGiaDTO> ListDanhGia { get; set; }
        public List<ABC_GroupDanhGiaDTO> ListGroupDanhGia { get; set; }
        //public 
    }
}
