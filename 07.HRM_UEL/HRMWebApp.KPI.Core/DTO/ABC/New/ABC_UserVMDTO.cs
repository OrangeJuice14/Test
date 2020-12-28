using HRMWebApp.Helpers;
using HRMWebApp.KPI.DB.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.Core.DTO
{
    public class ABC_UserVMDTO
    {
        public ABC_UserVMDTO()
        {
            ListGroupDanhGia = new List<ABC_GroupDanhGiaVMDTO>();

        }
        public ABC_UserVMDTO(DataRow row)
        {
            Id = Guid.Parse(row["WebUserId"].ToString());
            WebUserId = Guid.Parse(row["WebUserId"].ToString());
            ThamGiaGiangDay = bool.Parse(row["ThamGiaGiangDay"].ToString());
            if(row["WebUserStaffInfoId"].ToString() != "")
            WebUserStaffInfoId = Guid.Parse(row["WebUserStaffInfoId"].ToString());
            WebUserStaffInfoStaffProfileName = row["WebUserStaffInfoStaffProfileName"].ToString();
            WebUserUserName = row["WebUserUserName"].ToString();
            SubjectName = row["SubjectName"].ToString();
            if(row["SubjectId"].ToString() != "")
            SubjectId = Guid.Parse(row["SubjectId"].ToString());
            DepartmentName = row["DepartmentName"].ToString();
            if(row["DepartmentId"].ToString() != "")
            DepartmentId = Guid.Parse(row["DepartmentId"].ToString());
        }
        public ABC_UserVMDTO(WebUser objWebUser, HistoryThongTinHoSoNhanVien objHistoryThongTinHoSoNhanVien, List<ABC_WebUserGroupDanhGiaRole> listUserGroupDanhGiaRole)
        {
            WebUserUserName = objWebUser.UserName;
            WebUserId = objWebUser.Id;
            Id = objWebUser.Id;
            ThamGiaGiangDay = objHistoryThongTinHoSoNhanVien.ThamGiaGiangDay;
            WebUserStaffInfoId = objHistoryThongTinHoSoNhanVien.StaffInfo.Id;
            WebUserStaffInfoStaffProfileName = objHistoryThongTinHoSoNhanVien.StaffInfo.StaffProfile.Name;
            if (objHistoryThongTinHoSoNhanVien.StaffInfo.Subject != null)
            {
                SubjectId = objHistoryThongTinHoSoNhanVien.Subject.Id;
                SubjectName = objHistoryThongTinHoSoNhanVien.Subject.Name;
            }
            DepartmentId = objHistoryThongTinHoSoNhanVien.Department.Id;
            DepartmentName = objHistoryThongTinHoSoNhanVien.Department.Name;

            ListGroupDanhGia = listUserGroupDanhGiaRole.Select(e => e.GroupDanhGia).ToList().Map<ABC_GroupDanhGiaVMDTO>();
        }
        public ABC_UserVMDTO(WebUser objWebUser, HistoryThongTinHoSoNhanVien objHistoryThongTinHoSoNhanVien, ABC_GroupDanhGia objGroupDanhGia)
        {
            WebUserUserName = objWebUser.UserName;
            WebUserId = objWebUser.Id;
            Id = objWebUser.Id;
            ThamGiaGiangDay = objHistoryThongTinHoSoNhanVien.ThamGiaGiangDay;
            WebUserStaffInfoId = objHistoryThongTinHoSoNhanVien.StaffInfo.Id;
            WebUserStaffInfoStaffProfileName = objHistoryThongTinHoSoNhanVien.StaffInfo.StaffProfile.Name;
            if (objHistoryThongTinHoSoNhanVien.StaffInfo.Subject != null)
            {
                SubjectId = objHistoryThongTinHoSoNhanVien.Subject.Id;
                SubjectName = objHistoryThongTinHoSoNhanVien.Subject.Name;
            }
            DepartmentId = objHistoryThongTinHoSoNhanVien.Department.Id;
            DepartmentName = objHistoryThongTinHoSoNhanVien.Department.Name;
            GroupDanhGiaId = objGroupDanhGia.Id;
            GroupDanhGiaHasQuanLyDonVi = objGroupDanhGia.HasQuanLyDonVi;
            GroupDanhGiaName = objGroupDanhGia.Name;
        }
        public ABC_UserVMDTO(WebUser objWebUser, HistoryThongTinHoSoNhanVien objHistoryThongTinHoSoNhanVien)
        {
            WebUserUserName = objWebUser.UserName;
            WebUserId = objWebUser.Id;
            Id = objWebUser.Id;
            ThamGiaGiangDay = objHistoryThongTinHoSoNhanVien.ThamGiaGiangDay;
            WebUserStaffInfoId = objHistoryThongTinHoSoNhanVien.StaffInfo.Id;
            WebUserStaffInfoStaffProfileName = objHistoryThongTinHoSoNhanVien.StaffInfo.StaffProfile.Name;
            if (objHistoryThongTinHoSoNhanVien.StaffInfo.Subject != null)
            {
                SubjectId = objHistoryThongTinHoSoNhanVien.Subject.Id;
                SubjectName = objHistoryThongTinHoSoNhanVien.Subject.Name;
            }
            DepartmentId = objHistoryThongTinHoSoNhanVien.Department.Id;
            DepartmentName = objHistoryThongTinHoSoNhanVien.Department.Name;
        }
        public ABC_UserVMDTO(WebUser webUser)
        {
            ThamGiaGiangDay = webUser.StaffInfo.ThamGiaGiangDay;
            WebUserStaffInfoId = webUser.StaffInfo.Id;
            WebUserStaffInfoStaffProfileName = webUser.StaffInfo.StaffProfile.Name;
            WebUserUserName = webUser.UserName;
            WebUserId = webUser.Id;
            Id = webUser.Id;
            if (webUser.StaffInfo.Subject != null)
            {
                SubjectId = webUser.StaffInfo.Subject.Id;
                SubjectName = webUser.StaffInfo.Subject.Name;
            }
            DepartmentId = webUser.StaffInfo.Staff.Department.Id;
            DepartmentName = webUser.StaffInfo.Staff.Department.Name;

        }
        //public ABC_WebUserVMDTO(List<ABC_User> listWebUser)
        //{
        //    ListGroupDanhGia = new List<ABC_GroupDanhGiaDTO>();
        //    ThamGiaGiangDay = listWebUser[0].ThamGiaGiangDay;
        //    Status = listWebUser[0].Status;
        //    WebUserStaffInfoId = listWebUser[0].WebUser.StaffInfo.Id;
        //    WebUserStaffInfoStaffProfileName = listWebUser[0].WebUser.StaffInfo.StaffProfile.Name;
        //    WebUserUserName = listWebUser[0].WebUser.UserName;
        //    WebUserId = listWebUser[0].Id;
        //    if (listWebUser[0].WebUser.StaffInfo.Subject != null)
        //    {
        //        SubjectId = listWebUser[0].Subject.Id;
        //        SubjectName = listWebUser[0].WebUser.StaffInfo.Subject.Name;
        //    }
        //    DepartmentId = listWebUser[0].Department.Id;
        //    DepartmentName = listWebUser[0].Department.Name;

        //    int CountListWebUser = listWebUser.Count;

        //    for (int i = 0; i < CountListWebUser; i++)
        //    {
        //        ListGroupDanhGia.Add(listWebUser[i].GroupDanhGia.Map<ABC_GroupDanhGiaDTO>());
        //    }

        //}
        public Guid Id { get; set; }
        public Guid? WebUserId { get; set; }
        public bool? ThamGiaGiangDay { get; set; }
        public Guid? WebUserStaffInfoId { get; set; }
        public string WebUserStaffInfoStaffProfileName { get; set; }
        public string WebUserUserName { get; set; }
        public string SubjectName { get; set; }
        public Guid? SubjectId { get; set; }
        public string DepartmentName { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? KyDanhGiaId { get; set; }
        public Guid? GroupDanhGiaId { get; set; }
        public string GroupDanhGiaName { get; set; }
        public string StaffTypeManageCode { get; set; }
        public bool? GroupDanhGiaHasQuanLyDonVi { get; set; }
        public List<ABC_DanhGiaVMDTO> ListDanhGia { get; set; }
        public List<ABC_GroupDanhGiaVMDTO> ListGroupDanhGia { get; set; }
    }
}
