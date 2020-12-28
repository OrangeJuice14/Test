using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class StaffRoleApiController : ApiController
    {
        public IEnumerable<StaffRoleDTO> GetList()
        {
            var result = new List<StaffRoleDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<StaffRole>().ToList().Map<StaffRoleDTO>();
            });
            return result;
        }
        public IEnumerable<PositionDTO> GetListPosition()
        {
            var result = new List<PositionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Position>().Where(p=>p.GCRecord==null).ToList().Map<PositionDTO>();
            });
            return result;
        }

        public int GetMaxId()
        {
            var result = new StaffRole();
            SessionManager.DoWork(session =>
            {
                result = session.Query<StaffRole>().OrderByDescending(s => s.Id).Take(1).SingleOrDefault();
            });
            return result.Id+1;
        }
        public StaffRoleDTO GetObj(int id)
        {
            var result = new StaffRoleDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<StaffRole>().SingleOrDefault(a => a.Id == id).Map<StaffRoleDTO>();
            });
            return result;
        }
        public PositionDTO GetPositionObj(Guid id)
        {
            var result = new PositionDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Position>().SingleOrDefault(a => a.Id == id).Map<PositionDTO>();
            });
            return result;
        }

        public Position PutPosition(PositionDTO obj)
        {
            Position position = new Position();
            position = obj.Map<Position>();
            SessionManager.DoWork(session => 
                {
                    if (position != null)
                    {
                        position.AgentObjectType = new AgentObjectType();
                        position.AgentObjectType.Id = obj.AgentObjectTypeId;                        
                    }
                    session.SaveOrUpdate(position);
                });
            return position;
        }
        public StaffRole Put(StaffRoleDTO obj)
        {
            StaffRole staffRole = new StaffRole();
            staffRole = obj.Map<StaffRole>();
            SessionManager.DoWork(session => 
                {
                    if (obj.Id==0)
                    {
                        staffRole.Id = GetMaxId();
                        if (obj.AgentObjectId!=Guid.Empty)
                        { 
                        staffRole.AgentObject = new AgentObject();
                        staffRole.AgentObject.Id = obj.AgentObjectId;
                        }
                    }
                    else
                    {
                        if (staffRole!=null)
                        {
                            staffRole = obj.Map<StaffRole>();
                            if (obj.AgentObjectId != Guid.Empty)
                            {
                                staffRole.AgentObject = new AgentObject();
                                staffRole.AgentObject.Id = obj.AgentObjectId;
                            }
                        }
                    }
                    session.SaveOrUpdate(staffRole);
                });
            return staffRole;
        }

        public StaffRole Delete(StaffRole obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
