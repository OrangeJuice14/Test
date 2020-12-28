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


namespace HRMWebApp.KPI.Core.Controllers
{
    public class AgentObjectApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<AgentObjectDTO> GetList()
        {
            List<AgentObjectDTO> result = new List<AgentObjectDTO>();
            SessionManager.DoWork(session =>
            {
                List<AgentObject> objectResult = session.Query<AgentObject>().ToList();
                //result = session.Query<AgentObject>().ToList().Map<AgentObjectDTO>();
                MapAgentObjectDTO(result, objectResult);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<AgentObjectDTO> GetListProfessor()
        {
            List<AgentObjectDTO> result = new List<AgentObjectDTO>();
            SessionManager.DoWork(session =>
            {
                List<AgentObject> objectResult = session.Query<AgentObject>().ToList();
                //result = session.Query<AgentObject>().Where(a=>a.AgentObjectType.Id==1).ToList().Map<AgentObjectDTO>();
                MapAgentObjectDTO(result, objectResult);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public AgentObjectDTO GetClass(Guid id)
        {
            AgentObjectDTO result = new AgentObjectDTO();
            SessionManager.DoWork(session =>
            {
                AgentObject agentObject = session.Query<AgentObject>().SingleOrDefault(a => a.Id == id);
                List<AgentObject_TargetGroup> AgentObject_TargetGroup = session.Query<AgentObject_TargetGroup>().Where(r => r.AgentObjectId.Id == agentObject.Id).ToList();
                if (agentObject != null)
                {
                    //result = agentObject.Map<AgentObjectDTO>();
                    result.Id = agentObject.Id;
                    result.Name = agentObject.Name;
                    result.AgentObjectTypeId = agentObject.AgentObjectType != null ? agentObject.AgentObjectType.Id : 0;
                    foreach (AgentObject_TargetGroup tg in AgentObject_TargetGroup)
                    {
                        result.TargetGroupDetailIds.Add(tg.TargetGroupDetailId.Id);
                    }
                    result.TiTrong = AgentObject_TargetGroup.Select(r => r.TiTrong).FirstOrDefault();
                    //AgentObjectDetail AgentObjectDetail = session.Query<AgentObjectDetail>().SingleOrDefault(d => d.Id == id);
                    //if (AgentObjectDetail!=null)
                    //{
                    //    result.NumberOfSection = AgentObjectDetail.NumberOfSection;
                    //    result.ScienceResearch = AgentObjectDetail.ScienceResearch;
                    //    result.OtherActivity = AgentObjectDetail.OtherActivity;
                    //}                              
                    agentObject.AgentObjectDetails = agentObject.AgentObjectDetails.OrderBy(q => q.WorkingMode != null ? q.WorkingMode.OrderNumber : 0).ToList();
                    foreach (AgentObjectDetail ao in agentObject.AgentObjectDetails)
                    {
                        //result.AgentObjectDetails.Add(ao.Map<AgentObjectDetailDTO>());
                        AgentObjectDetailDTO aod = new AgentObjectDetailDTO();
                        aod.Id = ao.Id;
                        aod.NumberOfSection = ao.NumberOfSection;
                        aod.ScienceResearch = ao.ScienceResearch;
                        aod.OtherActivity = ao.OtherActivity;
                        aod.NumberOfSectionDensity = ao.NumberOfSectionDensity;
                        aod.ScienceResearchDensity = ao.ScienceResearchDensity;
                        aod.OtherActivityDensity = ao.OtherActivityDensity;
                        aod.AgentObjectId = ao.AgentObject != null ? ao.AgentObject.Id : Guid.Empty;
                        aod.WorkingModeId = ao.WorkingMode != null ? ao.WorkingMode.Id : Guid.Empty;
                        aod.WorkingModeName = ao.WorkingMode != null ? ao.WorkingMode.Name : "";
                        result.AgentObjectDetails.Add(aod);
                    }
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
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

        [Authorize]
        [Route("")]
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
                    if (staff.StaffInfo.Position == null)
                    {
                        if (staff.StaffInfo.StaffType.ManageCode == "3")
                            AgentObjectTypeId = 2; //Nhân viên
                        else
                            AgentObjectTypeId = 1; //Giảng viên
                    }
                    else
                    {
                        if (staff.StaffInfo.Position.AgentObjectType != null)
                            AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                    }
                    ////Trường hợp thuần không kiêm nhiệm
                    //if (staff.StaffInfo.AgentObjects.Count<=1)
                    //{
                    //    if (staff.StaffInfo.Position == null)
                    //    {
                    //        if (staff.StaffInfo.StaffType.ManageCode == "3")
                    //            AgentObjectTypeId = 2; //Nhân viên
                    //        else
                    //            AgentObjectTypeId = 1; //Giảng viên
                    //    }
                    //    else
                    //    {
                    //        if (staff.StaffInfo.Position.AgentObjectType!=null)
                    //            AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                    //    }
                    //}
                    //else
                    //{
                    //    bool all = false;
                    //    bool semester = false;
                    //    bool month = false;
                    //    //Lấy danh sách đối tượng của user
                    //    all = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 3 || ao.AgentObjectType.Id == 4 || ao.AgentObjectType.Id == 5 || ao.AgentObjectType.Id == 6 || ao.AgentObjectType.Id == 10 || ao.AgentObjectType.Id == 11);
                    //    semester = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 1 || ao.AgentObjectType.Id == 7 || ao.AgentObjectType.Id == 8 || ao.AgentObjectType.Id == 9 );
                    //    month= staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 2);                       
                    //    //Cả 3 loại kế hoạch
                    //    if (all==true)
                    //    {
                    //        AgentObjectTypeId = 100;
                    //    }
                    //    else
                    //    //Chỉ hk, tháng
                    //    if (semester==true && month==true)
                    //    {
                    //        AgentObjectTypeId = 99;
                    //    }
                    //    else
                    //    //Chỉ hk
                    //    if (semester == true && month == false)
                    //    {
                    //        AgentObjectTypeId = 98;
                    //    }
                    //    else
                    //    //Chỉ tháng
                    //    if (semester==false && month==true)
                    //    {
                    //        AgentObjectTypeId = 97;
                    //    }
                    //}
                }

                //Trường hợp user ủy quyền không có thông tin nhân viên, gán staff là Trưởng đơn vị
                else
                {
                    Staff staff = ControllerHelpers.GetCurrentStaff(session);
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
                    ////Trường hợp thuần không kiêm nhiệm
                    //if (staff.StaffInfo.AgentObjects.Count <= 1)
                    //{
                    //    if (staff.StaffInfo.Position == null)
                    //    {
                    //        if (staff.StaffInfo.StaffType.ManageCode == "3")
                    //            AgentObjectTypeId = 2; //Nhân viên
                    //        else
                    //            AgentObjectTypeId = 1; //Giảng viên
                    //    }
                    //    else
                    //    {
                    //        AgentObjectTypeId = staff.StaffInfo.Position.AgentObjectType.Id;
                    //    }
                    //}
                    //else
                    //{
                    //    bool all = false;
                    //    bool semester = false;
                    //    bool month = false;
                    //    //Lấy danh sách đối tượng của user
                    //    all = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 3 || ao.AgentObjectType.Id == 4 || ao.AgentObjectType.Id == 5 || ao.AgentObjectType.Id == 6);
                    //    semester = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 1 || ao.AgentObjectType.Id == 7 || ao.AgentObjectType.Id == 8 || ao.AgentObjectType.Id == 9);
                    //    month = staff.StaffInfo.AgentObjects.Any(ao => ao.AgentObjectType.Id == 2);
                    //    //Cả 3 loại kế hoạch
                    //    if (all == true)
                    //    {
                    //        AgentObjectTypeId = 100;
                    //    }
                    //    else
                    //    //Chỉ hk, tháng
                    //    if (semester == true && month == true)
                    //    {
                    //        AgentObjectTypeId = 99;
                    //    }
                    //    else
                    //    //Chỉ hk
                    //    if (semester == true && month == false)
                    //    {
                    //        AgentObjectTypeId = 98;
                    //    }
                    //    else
                    //    //Chỉ tháng
                    //    if (semester == false && month == true)
                    //    {
                    //        AgentObjectTypeId = 97;
                    //    }
                    //}
                }
               
            });
            return AgentObjectTypeId;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<AgentObjectDTO> GetListbyId(Guid classId)
        {
            List<AgentObjectDTO> result = new List<AgentObjectDTO>();
            SessionManager.DoWork(session =>
            {
                if (classId == Guid.Empty)
                {
                    //result = session.Query<AgentObject>().OrderBy(a => a.AgentObjectType.Id).ToList().Map<AgentObjectDTO>();
                    var agentObjectList = session.Query<AgentObject>().ToList();
                    MapAgentObjectDTO(result, agentObjectList);
                }
                else
                {
                    //result = session.Query<AgentObject>().Where(a => a.TargetGroupDetails.Any(ag => ag.Id == classId)).OrderBy(a => a.AgentObjectType.Id).Map<AgentObjectDTO>();
                    List<AgentObject_TargetGroup> at = session.Query<AgentObject_TargetGroup>().Where(r => r.TargetGroupDetailId.Id == classId).ToList();
                    //  var agentObjectList = session.Query<AgentObject>().Where(a => a.TargetGroupDetailId.Id == classId).ToList();
                    foreach (AgentObject_TargetGroup item in at)
                    {
                        AgentObject AgentObject = session.Query<AgentObject>().Where(r => r.Id == item.AgentObjectId.Id).FirstOrDefault();
                        AgentObjectDTO newitem = new AgentObjectDTO();
                        newitem.Id = item.AgentObjectId.Id;
                        newitem.Name = AgentObject.Name;
                        newitem.TiTrong = item.TiTrong;
                        //newitem.AgentObjectDetails = new List<AgentObjectDetailDTO>() AgentObject.AgentObjectDetails;
                        //newitem.AgentObjectTypeId = AgentObject.AgentObjectType;
                        result.Add(newitem);
                    }
                  //  MapAgentObjectDTO(result, agentObjectList);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<AgentObject_TargetGroupDTO> GetListagentObject()
        {
            List<AgentObject_TargetGroupDTO> result = new List<AgentObject_TargetGroupDTO>();
            SessionManager.DoWork(session =>
            {                
                List<AgentObject_TargetGroup> at = session.Query<AgentObject_TargetGroup>().OrderBy(r=>r.AgentObjectId.Id).ToList();
                foreach(AgentObject_TargetGroup item in at)
                {
                    AgentObject_TargetGroupDTO newitem = new AgentObject_TargetGroupDTO();
                    newitem.Id = item.Id;
                    newitem.AgentObjectId = new AgentObjectDTO() { Id = item.AgentObjectId.Id };
                    newitem.TargetGroupDetailId = new TargetGroupDetailDTO() { Id = item.TargetGroupDetailId.Id };
                    newitem.TargetGroupDetailName = item.TargetGroupDetailName;
                    newitem.AgentObjectName = item.AgentObjectName;
                    newitem.TiTrong = item.TiTrong;
                    newitem.Name = item.AgentObjectName + " - " + item.TargetGroupDetailName;
                    result.Add(newitem);

                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public AgentObject_TargetGroupDTO Gettagent_targetGroupObj(Guid Id)
        {
            AgentObject_TargetGroupDTO result = new AgentObject_TargetGroupDTO();
            SessionManager.DoWork(session =>
            {
                AgentObject_TargetGroup at = session.Query<AgentObject_TargetGroup>().Where(r=>r.Id == Id).FirstOrDefault();
                AgentObject agType = session.Query<AgentObject>().Where(r => r.Id == at.AgentObjectId.Id).FirstOrDefault();
                TargetGroupDetail tgType = session.Query<TargetGroupDetail>().Where(r => r.Id == at.TargetGroupDetailId.Id).FirstOrDefault();
                result.Id = at.Id;
                result.AgentObjectId = new AgentObjectDTO() { Id = at.AgentObjectId.Id , Name = at.AgentObjectName};
                result.TargetGroupDetailId = new TargetGroupDetailDTO() { Id = at.TargetGroupDetailId.Id,Name = at.TargetGroupDetailName };
                result.TargetGroupDetailName = at.TargetGroupDetailName;
                result.AgentObjectName = at.AgentObjectName;
                result.TiTrong = at.TiTrong;
                result.AgentObjectTypeId = agType.AgentObjectType.Id;
                result.TargetGroupDetailTypeId = tgType.TargetGroupDetailType.Id;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Putagent_targetGroup(AgentObject_TargetGroupDTO obj)
        {
            int result = 0;
            AgentObject_TargetGroup item = new AgentObject_TargetGroup();
            SessionManager.DoWork(session =>
            {
                item.Id = obj.Id;
                item.AgentObjectId = new AgentObject() { Id = obj.AgentObjectId.Id };
                item.TargetGroupDetailId = new TargetGroupDetail() { Id = obj.TargetGroupDetailId.Id };
                item.AgentObjectName = obj.AgentObjectName;
                item.TargetGroupDetailName = obj.TargetGroupDetailName;
                item.TiTrong = obj.TiTrong;
                session.SaveOrUpdate(item);
                result = 1;
            });
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return result;
        }
        private static void MapAgentObjectDTO(List<AgentObjectDTO> result, List<AgentObject> agentObjectList)
        {
            foreach (var ao in agentObjectList)
            {
                AgentObjectDTO aod = new AgentObjectDTO();
                aod.Id = ao.Id;
                aod.Name = ao.Name;
                aod.AgentObjectTypeId = ao.AgentObjectType != null ? ao.AgentObjectType.Id : 0;
                SessionManager.DoWork(session =>
                {
                    List<AgentObject_TargetGroup> target = session.Query<AgentObject_TargetGroup>().Where(r => r.AgentObjectId.Id == aod.Id).ToList();
                    aod.TargetGroupDetailIds = target.Select(q => q.Id).ToList();
                });
                    
                result.Add(aod);
            }
        }

        //Agent ObjectType Rate By Id
        [Authorize]
        [Route("")]
        public IEnumerable<AgentObjectTypeRateDTO> GetAgentObjectTypeRatingList()
        {
            var result = new List<AgentObjectTypeRateDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<AgentObjectTypeRate>().ToList().Map<AgentObjectTypeRateDTO>();
                foreach(AgentObjectTypeRateDTO agt in result)
                {
                    AgentObjectType aot = session.Query<AgentObjectType>().SingleOrDefault(a => a.Id == agt.AgentObjectTypeId);
                    agt.AgentObjectTypeName = aot.Name;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public AgentObjectTypeRateDTO GetAgentObjectTypeRateById(int Id)
        {
            AgentObjectTypeRateDTO result =new AgentObjectTypeRateDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<AgentObjectTypeRate>().SingleOrDefault(a => a.AgentObjectTypeId == Id).Map<AgentObjectTypeRateDTO>();
                AgentObjectType aot = session.Query<AgentObjectType>().SingleOrDefault(a => a.Id == result.AgentObjectTypeId);
                result.AgentObjectTypeName = aot.Name;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutAgentObjectTypeRate(AgentObjectTypeRateDTO obj)
        {
            int result = 0;
            AgentObjectTypeRate agentObjectTypeRate = obj.Map<AgentObjectTypeRate>();
            SessionManager.DoWork(session =>
            {

                session.SaveOrUpdate(agentObjectTypeRate);
                result = 1;
            });
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return result;
        }

        //---------------------------------------------

        [Authorize]
        [Route("")]
        public IEnumerable<AgentObjectTypeDTO> GetListAgentObjectType()
        {
            var result = new List<AgentObjectTypeDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<AgentObjectType>().ToList().Map<AgentObjectTypeDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public AgentObject Put(AgentObjectDTO obj)
        {
            AgentObject agentObject = new AgentObject();
            //agentObject = obj.Map<AgentObject>();
            SessionManager.DoWork(session =>
            {
                if (obj.Id == Guid.Empty)
                {
                    agentObject.Id = Guid.NewGuid();
                    agentObject.Name = obj.Name;

                    agentObject.AgentObjectType = new AgentObjectType();
                    agentObject.AgentObjectType.Id = obj.AgentObjectTypeId;

                    //if (obj.AgentObjectTypeId == 1)
                    //{
                    //    foreach (var ao in obj.AgentObjectDetails)
                    //    {
                    //        AgentObjectDetail item = new AgentObjectDetail();
                    //        item.Id = Guid.NewGuid();
                    //        item.AgentObject = new AgentObject() { Id = agentObject.Id };
                    //        //item.WorkingMode = 
                    //        item.NumberOfSection = ao.NumberOfSection;
                    //        item.ScienceResearch = ao.ScienceResearch;
                    //        item.OtherActivity = ao.OtherActivity;
                    //        agentObject.AgentObjectDetails.Add(item);
                    //    }
                    //}
                }
                else
                {
                    agentObject.Id = obj.Id;
                    agentObject.Name = obj.Name;
                    agentObject.AgentObjectType = new AgentObjectType();
                    agentObject.AgentObjectType.Id = obj.AgentObjectTypeId;

                    if (obj.AgentObjectTypeId == 1) // số giờ chuẩn của giảng viên
                    {
                        agentObject.AgentObjectDetails = new List<AgentObjectDetail>();
                        foreach (var ao in obj.AgentObjectDetails)
                        {
                            AgentObjectDetail item = session.Query<AgentObjectDetail>().SingleOrDefault(q => q.Id == ao.Id);
                            if (item != null)
                            {
                                item.NumberOfSection = ao.NumberOfSection;
                                item.ScienceResearch = ao.ScienceResearch;
                                item.OtherActivity = ao.OtherActivity;
                                item.NumberOfSectionDensity = ao.NumberOfSectionDensity;
                                item.ScienceResearchDensity = ao.ScienceResearchDensity;
                                item.OtherActivityDensity = ao.OtherActivityDensity;
                                agentObject.AgentObjectDetails.Add(item);
                            }
                        }
                    }
                }
                List<AgentObject_TargetGroup> AgentObject_TargetGroup = session.Query<AgentObject_TargetGroup>().Where(r => r.AgentObjectId.Id == obj.Id).ToList();
                foreach (var i in AgentObject_TargetGroup)
                {
                     session.Delete(i);
                }

                foreach (var id in obj.TargetGroupDetailIds)
                {
                    var tgName = session.Query<TargetGroupDetail>().Where(r => r.Id == id).FirstOrDefault();
                    AgentObject_TargetGroup ad = new DB.Entities.AgentObject_TargetGroup();
                    ad.Id = Guid.NewGuid();
                    ad.AgentObjectId = new AgentObject() { Id = obj.Id };
                    ad.AgentObjectName = obj.Name;
                    ad.TargetGroupDetailId = new TargetGroupDetail { Id = id };
                    ad.TargetGroupDetailName = tgName.Name;
                    ad.TiTrong = obj.TiTrong;
                    session.Save(ad);
                    //agentObject.TargetGroupDetails.Add(new TargetGroupDetail() { Id = id });
                }
                session.SaveOrUpdate(agentObject);
            });
            return agentObject;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<WorkingMode> GetWorkingModeListForAdding(Guid agentObjectId)
        {
            var result = new List<WorkingMode>();
            SessionManager.DoWork(session =>
            {
                var listWorkingMode = session.Query<AgentObjectDetail>().Where(q => q.AgentObject != null && q.AgentObject.Id == agentObjectId && q.WorkingMode != null).Select(q => q.WorkingMode.Id);
                result = session.Query<WorkingMode>().Where(q => !listWorkingMode.Any(u => u == q.Id)).OrderBy(q => q.OrderNumber).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<AgentObjectDetailDTO> GetWorkingModeListByAgentObject(Guid agentObjectId)
        {
            var result = new List<AgentObjectDetailDTO>();
            SessionManager.DoWork(session =>
            {
                var detailLists = session.Query<AgentObjectDetail>().Where(q => q.AgentObject != null && q.AgentObject.Id == agentObjectId && q.WorkingMode != null).OrderBy(q => q.WorkingMode != null ? q.WorkingMode.OrderNumber : 0);
                foreach (var detail in detailLists)
                {
                    AgentObjectDetailDTO aod = new AgentObjectDetailDTO();
                    aod.Id = detail.Id;
                    aod.NumberOfSection = detail.NumberOfSection;
                    aod.ScienceResearch = detail.ScienceResearch;
                    aod.OtherActivity = detail.OtherActivity;
                    aod.NumberOfSectionDensity = detail.NumberOfSectionDensity;
                    aod.ScienceResearchDensity = detail.ScienceResearchDensity;
                    aod.OtherActivityDensity = detail.OtherActivityDensity;
                    aod.AgentObjectId = detail.AgentObject != null ? detail.AgentObject.Id : Guid.Empty;
                    aod.WorkingModeId = detail.WorkingMode != null ? detail.WorkingMode.Id : Guid.Empty;
                    aod.WorkingModeName = detail.WorkingMode != null ? detail.WorkingMode.Name : "";
                    result.Add(aod);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public AgentObjectDetailDTO PutWorkingModeDetail(AgentObjectDetailDTO obj)
        {
            AgentObjectDetailDTO result = new AgentObjectDetailDTO();
            SessionManager.DoWork(session =>
            {
                AgentObjectDetail agentObjectDetail = new AgentObjectDetail();
                agentObjectDetail.Id = Guid.NewGuid();
                agentObjectDetail.AgentObject = new AgentObject() { Id = obj.AgentObjectId };
                agentObjectDetail.WorkingMode = new WorkingMode() { Id = obj.WorkingModeId };
                agentObjectDetail.NumberOfSection = obj.NumberOfSection;
                agentObjectDetail.ScienceResearch = obj.ScienceResearch;
                agentObjectDetail.OtherActivity = obj.OtherActivity;
                agentObjectDetail.NumberOfSectionDensity = obj.NumberOfSectionDensity;
                agentObjectDetail.ScienceResearchDensity = obj.ScienceResearchDensity;
                agentObjectDetail.OtherActivityDensity = obj.OtherActivityDensity;
                session.Save(agentObjectDetail);

                var workingModeName = session.Query<WorkingMode>().SingleOrDefault(q => q.Id == obj.WorkingModeId).Name;
                result.Id = agentObjectDetail.Id;
                result.NumberOfSection = agentObjectDetail.NumberOfSection;
                result.ScienceResearch = agentObjectDetail.ScienceResearch;
                result.OtherActivity = agentObjectDetail.OtherActivity;
                result.NumberOfSectionDensity = agentObjectDetail.NumberOfSectionDensity;
                result.ScienceResearchDensity = agentObjectDetail.ScienceResearchDensity;
                result.OtherActivityDensity = agentObjectDetail.OtherActivityDensity;
                result.AgentObjectId = agentObjectDetail.AgentObject != null ? agentObjectDetail.AgentObject.Id : Guid.Empty;
                result.WorkingModeId = agentObjectDetail.WorkingMode != null ? agentObjectDetail.WorkingMode.Id : Guid.Empty;
                result.WorkingModeName = workingModeName;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Delete(AgentObjectDTO obj)
        {
            var agentObject = new AgentObject();
            SessionManager.DoWork(session =>
            {
                agentObject = session.Query<AgentObject>().SingleOrDefault(q => q.Id == obj.Id);
                foreach (var detail in agentObject.AgentObjectDetails)
                {
                    session.Delete(detail);
                }
            });
            SessionManager.DoWork(session => 
            {
                agentObject.AgentObjectDetails = null;
                session.Delete(agentObject);
            });
            return 1;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
