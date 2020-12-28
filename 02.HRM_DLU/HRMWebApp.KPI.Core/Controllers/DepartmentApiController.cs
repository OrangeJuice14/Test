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
        public IEnumerable<DepartmentDTO> GetList()
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid spkt = new Guid(ConfigurationUtil.ReadAppSetting("SchoolGuid"));
                result = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d => d.Name).ToList().Map<DepartmentDTO>();
            });
            return result;
        }
        public IEnumerable<DepartmentDTO> GetDepartmentManagedByAdmin(Guid adminId)
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Staff admin = session.Query<Staff>().Where(s => s.Id == adminId && s.StaffStatus.NoLongerWork == 0 && s.StaffProfile.GCRecord == null).SingleOrDefault();
                if (admin!=null)
                {
                    result = admin.Departments.ToList().Map<DepartmentDTO>();
                }
            });
            return result;
        }

        //public IEnumerable<DepartmentDTO> GetList()
        //{
        //    var result = new List<DepartmentDTO>();
        //    var result1 = new List<DepartmentDTO>();
        //    SessionManager.DoWork(session =>
        //    {
        //        result = session.Query<Staff>().Select(s => s.Department).Distinct().ToList().Map<DepartmentDTO>();
        //        result1 = result.Map<DepartmentDTO>();
        //    });
        //    return result1;
        //}
        public IEnumerable<DepartmentDTO> GetDepartmentTreeList()
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid spkt = new Guid(ConfigurationUtil.ReadAppSetting("SchoolGuid"));
                List<Department> parentList = new List<Department>();
                List<Department> childList = new List<Department>();
                parentList = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord==null).ToList();
                foreach (Department pl in parentList)
                {
                    DepartmentDTO pd = pl.Map<DepartmentDTO>();
                    pd.ParentId = null;
                    result.Add(pd);
                }
                childList = session.Query<Department>().Where(d => d.ParentDepartment.Id != spkt && d.ParentDepartment.Id != null && d.GCRecord == null).ToList();
                foreach (Department cl in childList)
                {
                    DepartmentDTO cd = cl.Map<DepartmentDTO>();
                    cd.ParentId = cl.ParentDepartment.Id;
                    result.Add(cd);
                }
            });
            return result;
        }
        //Lấy danh sách phòng ban
        public IEnumerable<DepartmentDTO> GetMainDepartmentList()
        {
           List<DepartmentDTO> result = new List<DepartmentDTO>();
           SessionManager.DoWork(session =>
           {
               Staff staff = ControllerHelpers.GetCurrentStaff(session);
               Guid spkt = new Guid(ConfigurationUtil.ReadAppSetting("SchoolGuid"));
               List<Department> parentList = new List<Department>();
               List<Department> childList = new List<Department>();
               parentList = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d=>d.Name).ToList();
               foreach (Department pl in parentList)
               {
                   DepartmentDTO pd = pl.Map<DepartmentDTO>();
                   pd.ParentId = null;
                   switch(pd.DepartmentType)
                   {
                       case 1:
                           {
                               pd.DepartmentTypeName="Phòng ban";
                           }
                           break;
                       case 4:
                           {
                               pd.DepartmentTypeName = "Khoa/Viện/Trung tâm";
                           }
                           break;
                       default:
                           {
                               pd.DepartmentTypeName = "Khác";
                           }
                           break;
                   }
                   bool check = staff.Departments!=null ? staff.Departments.Any(d => d.Id == pl.Id) : false;
                   pd.IsManaged = check;
                   result.Add(pd);
               }
           });
           return result;
        }
        public IEnumerable<DepartmentHierarchyDTO> GetMainDepartmentListHierarchy(Guid planDetailId)
        {
            List<DepartmentHierarchyDTO> result = new List<DepartmentHierarchyDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> subDepartmentIds = new List<Guid>();
                PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p=>p.Id==planDetailId);
                if (pld != null)
                    subDepartmentIds = pld.SubDepartments.Select(d => d.Id).ToList();


                Guid spkt = new Guid(ConfigurationUtil.ReadAppSetting("SchoolGuid"));
                List<Department> deptList = new List<Department>();
                deptList = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d=>d.Name).ToList();

                DepartmentHierarchyDTO phong = new DepartmentHierarchyDTO();
                phong.Id = new Guid("00000000-0000-0000-0000-000000000001");
                phong.Name = "Phòng ban";
                result.Add(phong);
                DepartmentHierarchyDTO khoa = new DepartmentHierarchyDTO();
                khoa.Id = new Guid("00000000-0000-0000-0000-000000000002");
                khoa.Name = "Khoa/Viện/Trung tâm";
                result.Add(khoa);
                DepartmentHierarchyDTO khac = new DepartmentHierarchyDTO();
                khac.Id = new Guid("00000000-0000-0000-0000-000000000003");
                khac.Name = "Khác";
                result.Add(khac);

                List<DepartmentDTO> listDept=new List<DepartmentDTO>();
                foreach (Department pl in deptList)
                {
                    DepartmentDTO pd = pl.Map<DepartmentDTO>();
                    switch (pd.DepartmentType)
                    {
                        case 1:
                            {
                                pd.ParentId = new Guid("00000000-0000-0000-0000-000000000001");
                            }
                            break;
                        case 4:
                            {
                                pd.ParentId = new Guid("00000000-0000-0000-0000-000000000002");
                            }
                            break;
                        default:
                            {
                                pd.ParentId = new Guid("00000000-0000-0000-0000-000000000003");
                            }
                            break;
                    }
                    listDept.Add(pd);
                }
                foreach (DepartmentHierarchyDTO dh in result)
                {
                    dh.items = new List<DepartmentDTO>();
                    foreach (DepartmentDTO dt in listDept)
                    {
                        if (dt.ParentId==dh.Id)
                        {
                            if (subDepartmentIds.Contains(dt.Id))
                                dt.@checked = true;
                            dh.items.Add(dt);
                        }
                    }
                }
            });
            return result;
        }
        public IEnumerable<DepartmentHierarchyDTO> GetAdminDepartmentListHierarchy(Guid adminId)
        {
            List<DepartmentHierarchyDTO> result = new List<DepartmentHierarchyDTO>();
            SessionManager.DoWork(session =>
            {
                List<Guid> departmentIds = new List<Guid>();
                Staff staff = session.Query<Staff>().SingleOrDefault(s => s.Id == adminId);
                if (staff != null)
                    departmentIds = staff.Departments.Select(d => d.Id).ToList();


                Guid spkt = new Guid(ConfigurationUtil.ReadAppSetting("SchoolGuid"));
                List<Department> deptList = new List<Department>();
                deptList = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d => d.Name).ToList();

                DepartmentHierarchyDTO phong = new DepartmentHierarchyDTO();
                phong.Id = new Guid("00000000-0000-0000-0000-000000000001");
                phong.Name = "Phòng ban";
                result.Add(phong);
                DepartmentHierarchyDTO khoa = new DepartmentHierarchyDTO();
                khoa.Id = new Guid("00000000-0000-0000-0000-000000000002");
                khoa.Name = "Khoa/Viện/Trung tâm";
                result.Add(khoa);
                DepartmentHierarchyDTO khac = new DepartmentHierarchyDTO();
                khac.Id = new Guid("00000000-0000-0000-0000-000000000003");
                khac.Name = "Khác";
                result.Add(khac);

                List<DepartmentDTO> listDept = new List<DepartmentDTO>();
                foreach (Department pl in deptList)
                {
                    DepartmentDTO pd = pl.Map<DepartmentDTO>();
                    switch (pd.DepartmentType)
                    {
                        case 1:
                            {
                                pd.ParentId = new Guid("00000000-0000-0000-0000-000000000001");
                            }
                            break;
                        case 4:
                            {
                                pd.ParentId = new Guid("00000000-0000-0000-0000-000000000002");
                            }
                            break;
                        default:
                            {
                                pd.ParentId = new Guid("00000000-0000-0000-0000-000000000003");
                            }
                            break;
                    }
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
        public IEnumerable<DepartmentDTO> GetSubjectDepartment(Guid departmentId)
        {
            List<DepartmentDTO> result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                result=session.Query<Department>().Where(d=>d.ParentDepartment.Id==departmentId).ToList().Map<DepartmentDTO>();
            });
            return result;
        }
        public string GetStaffId(Guid DepartmentId, int RoleId)
        {
            var result = new StaffDTO();          
            string StaffId = "";
            SessionManager.DoWork(session =>
            {
                Staff staff = session.Query<Department>().SingleOrDefault(d => d.Id == DepartmentId).Staffs.Where(s => s.StaffRoles.Count > 0 && s.StaffRoles.Any(sr => sr.Id == RoleId)).SingleOrDefault();
                if (staff != null)
                {
                    result = staff.Map<StaffDTO>();
                    StaffId = result.Id.ToString();
                }
            });
            return StaffId;
        }

        public IEnumerable<StaffRoleDTO> GetListStaffRole()
        {
            var result = new List<StaffRoleDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<StaffRole>().ToList().Map<StaffRoleDTO>();
            });
            return result;
        }

        public DepartmentDTO GetClass(Guid id)
        {
            var result = new DepartmentDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Department>().SingleOrDefault(a => a.Id == id).Map<DepartmentDTO>();
            });
            return result;
        }

        //public Department Put(Department obj)
        //{
        //    if (obj.Id == Guid.Empty)
        //        obj.Id = Guid.NewGuid();
        //    SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        //    return obj;
        //}

        public DepartmentDTO Put(DepartmentDTO obj)
        {
            SessionManager.DoWork(session =>
                {
                    Staff st = session.Query<Staff>().SingleOrDefault(a => a.Id == obj.StaffId);
                    StaffRole str = st.StaffRoles.SingleOrDefault(a => a.Id == obj.StaffRoleId);
                    if (str != null)
                    {
                        st.StaffRoles.Remove(str);
                    }
                    st.StaffRoles.Add(new StaffRole() { Id = obj.StaffRoleId });
                    session.SaveOrUpdate(st);
                });
            return obj;
        }

        public Department Delete(Department obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
