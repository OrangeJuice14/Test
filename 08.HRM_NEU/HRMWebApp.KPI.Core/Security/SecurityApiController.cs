using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;

using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using System.Web;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO;


namespace HRMWebApp.KPI.Core.Security
{
    public class SecurityApiController : ApiController
    {
        public IEnumerable<Role> GetUserViewRoles(int agentObjectTypeId)
        {
            List<Role> result = new List<Role>();
            SessionManager.DoWork(session =>
            {
                AgentObjectType agt = session.Query<AgentObjectType>().SingleOrDefault(a => a.Id == agentObjectTypeId);
                if (agt != null)
                    result = agt.Roles.ToList();               
            });
            return result;
        }

        public IEnumerable<string> GetUserViewRoleIds(string userId)
        {          
            List<string> result = new List<string>();
            SessionManager.DoWork(session =>
            {
                ApplicationUser applicationUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
                Staff staff = ControllerHelpers.GetCurrentStaff(session);
                List<int> AgentObjectTypeIds = new List<int>();

                if (applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000000" || applicationUser.WebGroupId == "00000000-0000-0000-0000-000000000001" )
                {
                    AgentObjectTypeIds.Add(4);
                }
                if (staff!=null)
                {

 
                    if (staff.StaffInfo.Position == null)
                    {
                        if (staff.StaffInfo.StaffType?.ManageCode == "3")
                            AgentObjectTypeIds.Add(2); //Nhân viên
                        else
                            AgentObjectTypeIds.Add(1); //Giảng viên
                    }
                    else
                    {
                        if (!applicationUser.IsKPIs && staff.StaffInfo.Position.AgentObjectType!=null)
                            AgentObjectTypeIds.Add(staff.StaffInfo.Position.AgentObjectType.Id);
                        else
                            AgentObjectTypeIds.Add(Convert.ToInt32(applicationUser.AgentObjectTypeId));
                    }
                    //if (!SessionHelper.Data<bool>(SessionKey.IsKPIs) && staff.StaffInfo.AgentObjects.Count > 1)
                    //{
                    //    foreach(AgentObject ab in staff.StaffInfo.AgentObjects)
                    //    {
                    //        if(!AgentObjectTypeIds.Any(at=>at==ab.AgentObjectType.Id))
                    //        AgentObjectTypeIds.Add(ab.AgentObjectType.Id);
                    //    }                     
                    //}

                    //foreach(int aId in AgentObjectTypeIds)
                    //{
                    //    AgentObjectType agt = session.Query<AgentObjectType>().SingleOrDefault(a => a.Id == aId);
                    //    if (agt != null)
                    //        result.AddRange(agt.Roles.Select(r => r.Id).ToList());
                    //}
                }

            });
            return result;
        }
        public IEnumerable<Role> GetList()
        {
            List<Role> result = new List<Role>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Role>().ToList();
            });
            return result;
        }
        public Role GetObj(string id)
        {
            Role result = new Role();
            id = id.ToLower();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Role>().Where(r=>r.Id.ToLower()==id).SingleOrDefault();
            });
            return result;
        }
        public Role Put(Role obj)
        {
            if (obj.Id != "")
            {
                SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            }           
            return obj;
        }
        public int Delete(Role obj)
        {
            try
            {
                SessionManager.DoWork(session => session.Delete(obj));
                return 1;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        public IEnumerable<AgentObjectTypeDTO> GetListAgent()
        {
            List<AgentObjectTypeDTO> result = new List<AgentObjectTypeDTO>();
            SessionManager.DoWork(session =>
            {
               List<AgentObjectType> temp = session.Query<AgentObjectType>().ToList();
                foreach( AgentObjectType ag in temp)
                {
                    AgentObjectTypeDTO agd = new AgentObjectTypeDTO();
                    agd.Id = ag.Id;
                    agd.Name = ag.Name;
                    result.Add(agd);
                }
            });
            return result;
        }
        public AgentObjectTypeDTO GetAgentObj(int id)
        {
            AgentObjectTypeDTO result = new AgentObjectTypeDTO();
            SessionManager.DoWork(session =>
            {
                AgentObjectType temp = session.Query<AgentObjectType>().Where(r => r.Id == id).SingleOrDefault();
                result.Id = temp.Id;
                result.Name = temp.Name;
            });
            return result;
        }
        public IEnumerable<RoleHierarchyDTO> GetRoleHierarchy(int agentObjectTypeId)
        {
            List<RoleHierarchyDTO> result = new List<RoleHierarchyDTO>();
            SessionManager.DoWork(session =>
            {
                List<string> roleIds = new List<string>();
                AgentObjectType agentOT = session.Query<AgentObjectType>().SingleOrDefault(s => s.Id == agentObjectTypeId);
                if (agentOT != null)
                    roleIds = agentOT.Roles.Select(d => d.Id).ToList();

                List<Role> roleList = new List<Role>();
                roleList = session.Query<Role>().OrderBy(r=>r.Name).ToList();

                RoleHierarchyDTO all = new RoleHierarchyDTO();
                all.Id = "00000000-0000-0000-0000-000000000001";
                all.Name = "Tất cả";
                result.Add(all);

                List<RoleDTO> listRole = new List<RoleDTO>();
                foreach (Role pl in roleList)
                {
                    RoleDTO pd = pl.Map<RoleDTO>();
                    pd.ParentId = "00000000-0000-0000-0000-000000000001";
                    listRole.Add(pd);
                }
                foreach (RoleHierarchyDTO dh in result)
                {
                    dh.items = new List<RoleDTO>();
                    foreach (RoleDTO dt in listRole)
                    {
                        if (dt.ParentId == dh.Id)
                        {
                            if (roleIds.Contains(dt.Id))
                                dt.@checked = true;
                            dh.items.Add(dt);
                        }
                    }
                }
            });
            return result;
        }
        public int PutAOTR(AgentObjectTypeDTO obj)
        {
            AgentObjectType aot = new AgentObjectType();
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    aot = session.Query<AgentObjectType>().SingleOrDefault(a => a.Id == obj.Id);
                    aot.Roles = new List<Role>();
                    obj.RoleIds.Remove("00000000-0000-0000-0000-000000000001");
                    foreach (var id in obj.RoleIds)
                    {
                        Role role = new Role { Id = id };
                            aot.Roles.Add(role);
                            session.Update(aot);
                            result = 1;
                    }
                });
            }
            catch (Exception e)
            {
                result = 0;
            }
            return result;
        }
    }
}
