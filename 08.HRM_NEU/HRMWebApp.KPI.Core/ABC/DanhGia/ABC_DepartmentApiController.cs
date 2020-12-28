using AutoMapper;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_DepartmentApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DepartmentDTO> GetList()
        {
            List<ABC_DepartmentDTO> result = new List<ABC_DepartmentDTO>();
            try {
                SessionManager.DoWork(session =>
                {
                    List<Department> Departments = session.Query<Department>().Where(e => e.GCRecord == null).OrderBy(e => e.Name).ToList();
                    foreach (Department Department in Departments)
                    {
                        ABC_DepartmentDTO DepartmentDTO = new ABC_DepartmentDTO();
                        DepartmentDTO.Id = Department.Id;
                        DepartmentDTO.Name = Department.Name;
                        if (Department.ParentDepartment != null)
                            DepartmentDTO.ParentId = Department.ParentDepartment.Id;
                        result.Add(DepartmentDTO);
                    }
                });
            } catch (Exception ex) { Helper.ErrorLog("ABC_DepartmentApi/GetList", ex); throw ex; } 
            
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DepartmentDTO> GetListTree(Guid userId)
        {
            Mapper.CreateMap<Department, ABC_DepartmentDTO>()
               .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.items));
            List<ABC_DepartmentDTO> result = new List<ABC_DepartmentDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    result = session.Query<Department>().Where(e => e.GCRecord == null && e.ParentDepartment == null).OrderBy(e => e.Name).Map<ABC_DepartmentDTO>().ToList();

                });
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] = GetCheckedDepartmentDTO(result[i], userId);
                }

            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_DepartmentApi/GetList ", ex); throw ex;
            }

            return result;
        }

        public ABC_DepartmentDTO GetCheckedDepartmentDTO(ABC_DepartmentDTO Parents, Guid userId)
        {
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_QuanLyDonVi QLDV = session.Query<ABC_QuanLyDonVi>().SingleOrDefault(e => e.User.Id == userId && e.Department.Id == Parents.Id);
                    Parents.@checked = QLDV != null;
                    Parents.expanded = Parents.items.Count > 0;
                });
                for (int i = 0; i < Parents.items.Count; i++)
                {
                    Parents.items[i] = GetCheckedDepartmentDTO(Parents.items[i], userId);
                }
            } catch (Exception ex) { Helper.ErrorLog("ABC_DepartmentApi/GetCheckedDepartmentDTO ", ex); throw ex; } 
            return Parents;
        }
        //public Department GetCheckedDepartment(Department Parents, Guid userId)
        //{
        //    for (int i = 0; i < Parents.items.Count; i++)
        //    {
        //        ses
        //        Parents.items[i] = GetCheckedDepartment(Parents.items[i], userId);
        //    }
        //    return Parents;
        //}
    }
}
