using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.Helpers;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class DepartmentApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetList()
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid buh = new Guid("77FC97D9-6EE2-410A-B730-0444FE7AF7AE"); //Trường Đại học Ngân hàng
                result = session.Query<Department>().Where(d => d.ParentDepartment.Id == buh && d.GCRecord == null).OrderBy(d => d.Name).ToList().Map<DepartmentDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetDepartmentManagedByAdmin(Guid adminId)
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Staff admin = session.Query<Staff>().Where(s => s.Id == adminId && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).SingleOrDefault();
                if (admin != null)
                {
                    result = admin.Departments.ToList().Map<DepartmentDTO>();
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentHierarchyDTO> GetAdminDepartmentListHierarchy(Guid adminId, Guid congTyId)
        {
            List<DepartmentHierarchyDTO> result = new List<DepartmentHierarchyDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> departmentIds = new List<Guid>();
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == adminId);
                if (staff != null)
                    departmentIds = staff.Departments.Select(d => d.Id).ToList();
                
                List<Department> deptList = new List<Department>();
                deptList = session.Query<Department>().Where(d => d.Id == congTyId && d.GCRecord == null).SelectManyRecursive(d => d.Childs).OrderBy(d => d.Name).ToList();
                
                DepartmentHierarchyDTO khac = new DepartmentHierarchyDTO();
                khac.Id = new Guid("00000000-0000-0000-0000-000000000001");
                khac.Name = "Tất cả";
                result.Add(khac);

                List<DepartmentDTO> listDept = new List<DepartmentDTO>();
                foreach (Department pl in deptList)
                {
                    DepartmentDTO pd = pl.Map<DepartmentDTO>();
                    pd.ParentId = new Guid("00000000-0000-0000-0000-000000000001");
                    listDept.Add(pd);
                }
                foreach (DepartmentHierarchyDTO dh in result)
                {
                    dh.items = new List<DepartmentDTO>();
                    foreach (DepartmentDTO dt in listDept)
                    {
                        if (dt.ParentId == dh.Id)
                        {
                            if (departmentIds.Contains(dt.Id))
                                dt.@checked = true;
                            dh.items.Add(dt);
                        }
                    }
                }
            });
            return result;
        }
    }
}
