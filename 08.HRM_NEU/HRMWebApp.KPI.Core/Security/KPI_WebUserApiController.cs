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
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Helpers;


namespace HRMWebApp.KPI.Core.Security
{
    public class KPI_WebUserApiController : ApiController
    {
        public IEnumerable<AgentObjectDTO> GetList()
        {
            List<AgentObjectDTO> result = new List<AgentObjectDTO>();
            SessionManager.DoWork(session =>
            {
                List<AgentObject> objectResult = session.Query<AgentObject>().ToList();
                result = session.Query<AgentObject>().ToList().Map<AgentObjectDTO>();
            });
            return result;
        }
        public IEnumerable<AgentObjectDTO> GetListProfessor()
        {
            List<AgentObjectDTO> result = new List<AgentObjectDTO>();
            SessionManager.DoWork(session =>
            {
                List<AgentObject> objectResult = session.Query<AgentObject>().ToList();
                result = session.Query<AgentObject>().Where(a => a.AgentObjectType.Id == 1).ToList().Map<AgentObjectDTO>();
            });
            return result;
        }
        public AgentObjectDTO GetClass(Guid id)
        {
            AgentObjectDTO result = new AgentObjectDTO();
            SessionManager.DoWork(session =>
            {
                AgentObject agentObject = session.Query<AgentObject>().SingleOrDefault(a => a.Id == id);
                if (agentObject != null)
                {
                    result = agentObject.Map<AgentObjectDTO>();
                    foreach (TargetGroupDetail tg in agentObject.TargetGroupDetails)
                    {
                        result.TargetGroupDetailIds.Add(tg.Id);
                    }
                    AgentObjectDetail AgentObjectDetail = session.Query<AgentObjectDetail>().SingleOrDefault(d => d.Id == id);
                    if (AgentObjectDetail != null)
                    {
                        result.NumberOfSection = AgentObjectDetail.NumberOfSection;
                        result.ScienceResearch = AgentObjectDetail.ScienceResearch;
                    }
                }
            });
            return result;
        }


        public Guid GetUserAgentObjectId(Guid userId)
        {
            Guid result = Guid.Empty;
            SessionManager.DoWork(session =>
            {
                if (userId != Guid.Empty)
                {
                    Staff staff = session.Query<Staff>().SingleOrDefault(a => a.Id == userId);
                    result = session.Query<AgentObject>().FirstOrDefault(a => a.AgentObjectType.Id == 4).Id;
                }

            });
            return result;
        }
        //public int GetUserAgentObjectTypeId()
        //{
        //    int AgentObjectTypeId = -1;
        //    SessionManager.DoWork(session =>
        //    {
        //        Guid staffId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id);
        //        if (staffId != Guid.Empty)
        //        {
        //            Staff staff = session.Query<Staff>().SingleOrDefault(a => a.Id == staffId);                   
        //            if (staff.StaffInfo.Position == null)
        //            {
        //                if (staff.StaffInfo.StaffType.ManageCode == "3")
        //                    AgentObjectTypeId = 2; //Nhân viên
        //                else
        //                    AgentObjectTypeId = 1; //Giảng viên
        //            }
        //            else
        //            {
        //                AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
        //            }
        //        }

        //    });
        //    return AgentObjectTypeId;
        //}
        public int GetUserAgentObjectTypeId()
        {
            int AgentObjectTypeId = -1;
            SessionManager.DoWork(session =>
            {
                Guid staffId = Guid.Empty;
                if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
                {
                    staffId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id);
                }
                //Trường hợp là user không ủy quyền
                if (staffId != Guid.Empty)
                {
                    Staff staff = session.Query<Staff>().SingleOrDefault(a => a.Id == staffId);
                    //Trường hợp thuần không kiêm nhiệm
                    if (staff.StaffInfo.AgentObjects.Count <= 1)
                    {
                        if (staff.StaffInfo.Position == null)
                        {
                            if (staff.StaffInfo.StaffType.ManageCode == "3")
                                AgentObjectTypeId = 2; //Nhân viên
                            else
                                AgentObjectTypeId = 1; //Giảng viên
                        }
                        else
                        {
                            AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                        }
                    }
                    else
                    {
                        bool all = false;
                        bool semester = false;
                        bool month = false;
                        //Lấy danh sách đối tượng của user
                        all = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 3 || ao.AgentObjectType.Id == 4 || ao.AgentObjectType.Id == 5 || ao.AgentObjectType.Id == 6);
                        semester = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 1 || ao.AgentObjectType.Id == 7 || ao.AgentObjectType.Id == 8 || ao.AgentObjectType.Id == 9);
                        month = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 2);
                        //Cả 3 loại kế hoạch
                        if (all == true)
                        {
                            AgentObjectTypeId = 100;
                        }
                        else
                            //Chỉ hk, tháng
                            if (semester == true && month == true)
                            {
                                AgentObjectTypeId = 99;
                            }
                            else
                                //Chỉ hk
                                if (semester == true && month == false)
                                {
                                    AgentObjectTypeId = 98;
                                }
                                else
                                    //Chỉ tháng
                                    if (semester == false && month == true)
                                    {
                                        AgentObjectTypeId = 97;
                                    }
                    }
                }

                //Trường hợp user ủy quyền không có thông tin nhân viên, gán staff là Trưởng đơn vị
                else
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
                    //Trường hợp thuần không kiêm nhiệm
                    if (staff.StaffInfo.AgentObjects.Count <= 1)
                    {
                        if (staff.StaffInfo.Position == null)
                        {
                            if (staff.StaffInfo.StaffType.ManageCode == "3")
                                AgentObjectTypeId = 2; //Nhân viên
                            else
                                AgentObjectTypeId = 1; //Giảng viên
                        }
                        else
                        {
                            AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                        }
                    }
                    else
                    {
                        bool all = false;
                        bool semester = false;
                        bool month = false;
                        //Lấy danh sách đối tượng của user
                        all = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 3 || ao.AgentObjectType.Id == 4 || ao.AgentObjectType.Id == 5 || ao.AgentObjectType.Id == 6);
                        semester = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 1 || ao.AgentObjectType.Id == 7 || ao.AgentObjectType.Id == 8 || ao.AgentObjectType.Id == 9);
                        month = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 2);
                        //Cả 3 loại kế hoạch
                        if (all == true)
                        {
                            AgentObjectTypeId = 100;
                        }
                        else
                            //Chỉ hk, tháng
                            if (semester == true && month == true)
                            {
                                AgentObjectTypeId = 99;
                            }
                            else
                                //Chỉ hk
                                if (semester == true && month == false)
                                {
                                    AgentObjectTypeId = 98;
                                }
                                else
                                    //Chỉ tháng
                                    if (semester == false && month == true)
                                    {
                                        AgentObjectTypeId = 97;
                                    }
                    }
                }

            });
            return AgentObjectTypeId;
        }

        public IEnumerable<AgentObjectDTO> GetListbyId(Guid classId)
        {
            List<AgentObjectDTO> result = new List<AgentObjectDTO>();
            SessionManager.DoWork(session =>
            {
                if (classId == Guid.Empty)
                {
                    result = session.Query<AgentObject>().OrderBy(a => a.AgentObjectType.Id).ToList().Map<AgentObjectDTO>();
                }
                else
                {
                    result = session.Query<AgentObject>().Where(a => a.TargetGroupDetails.Any(ag => ag.Id == classId)).OrderBy(a => a.AgentObjectType.Id).Map<AgentObjectDTO>();
                }
            });
            return result;
        }

        public IEnumerable<AgentObjectTypeDTO> GetListAgentObjectType()
        {
            var result = new List<AgentObjectTypeDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<AgentObjectType>().ToList().Map<AgentObjectTypeDTO>();
            });
            return result;
        }
        public AgentObject Put(AgentObjectDTO obj)
        {

            AgentObject agentObject = new AgentObject();
            agentObject = obj.Map<AgentObject>();
            SessionManager.DoWork(session =>
            {
                if (obj.Id == Guid.Empty)
                {
                    agentObject.Id = Guid.NewGuid();

                    agentObject.AgentObjectType = new AgentObjectType();
                    agentObject.AgentObjectType.Id = obj.AgentObjectTypeId;

                    agentObject.AgentObjectDetail = new AgentObjectDetail();
                    agentObject.AgentObjectDetail.Id = agentObject.Id;
                    agentObject.AgentObjectDetail.NumberOfSection = obj.NumberOfSection;
                    agentObject.AgentObjectDetail.ScienceResearch = obj.ScienceResearch;
                }
                else
                {
                    if (agentObject != null)
                    {
                        agentObject = agentObject = obj.Map<AgentObject>();
                        agentObject.AgentObjectType = new AgentObjectType();
                        agentObject.AgentObjectType.Id = obj.AgentObjectTypeId;
                        agentObject.TargetGroupDetails = new List<TargetGroupDetail>();
                        agentObject.AgentObjectDetail = new AgentObjectDetail();
                        agentObject.AgentObjectDetail.Id = obj.Id;
                        agentObject.AgentObjectDetail.NumberOfSection = obj.NumberOfSection;
                        agentObject.AgentObjectDetail.ScienceResearch = obj.ScienceResearch;
                    }
                }
                foreach (var id in obj.TargetGroupDetailIds)
                {
                    agentObject.TargetGroupDetails.Add(new TargetGroupDetail() { Id = id });
                }
                session.SaveOrUpdate(agentObject);
                session.SaveOrUpdate(agentObject.AgentObjectDetail);
            });
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return agentObject;
        }

        public AgentObject Delete(AgentObject obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
