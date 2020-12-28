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
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO.ResultDTOs;
using System.Configuration;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class ResultApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public RecordSingleResultDTO ParseResultRecord(Result result1)
        {
            RecordSingleResultDTO finalResult = new RecordSingleResultDTO();

            finalResult.Name = result1.PlanStaff.AgentObjectType.Name;
            finalResult.Record = result1.TotalRecordSecond <= 0 ? result1.TotalRecord : result1.TotalRecordSecond;
        
            SessionManager.DoWork(session =>
            {
                List<AgentObjectTypeRate> rates = session.Query<AgentObjectTypeRate>().ToList();
                AgentObjectTypeRate ar = session.Query<AgentObjectTypeRate>().SingleOrDefault(a => a.AgentObjectTypeId == result1.PlanStaff.AgentObjectType.Id);
                if (ar != null)
                {
                    finalResult.Rate = ar != null ? ar.ResultRate : 0;
                    finalResult.Rate2 = 100 - finalResult.Rate;
                }
            });

            return finalResult;
        }

        [Authorize]
        [Route("")]
        public Dictionary<string, object> GetResultList(Guid planId, Guid agentObjectId)
        {
            Dictionary<string, object> resultDic = new Dictionary<string, object>();
            ResultRecordDTO result = new ResultRecordDTO();
            SessionManager.DoWork(session =>
            {

                 Staff staff = ControllerHelpers.GetCurrentStaff(session);

                  PlanKPI plan = session.Query<PlanKPI>().Where(p => p.Id == planId).OrderByDescending(p => p.CreateTime).FirstOrDefault();
                  if (plan != null)
                  {

                      //Kết quả trưởng phòng
                      ResultRecordDTO manageRecordResultList = new ResultRecordDTO();
                      List<Result> mangeResult = new List<Result>();
                      List<AgentObjectType> agentObjectTypes = new List<AgentObjectType>();
                      PlanStaff planStaff = new PlanStaff();
                      if (staff.StaffInfo.Position!=null && staff.StaffInfo.Position.AgentObjectType != null)
                             agentObjectTypes.Add(staff.StaffInfo.Position.AgentObjectType);
                      //Chức vụ chính
                      foreach (AgentObjectType at in agentObjectTypes)
                      {
                          int AgentObjectTypeId = at.Id;

                         
                          if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id).FirstOrDefault();
                          else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id).FirstOrDefault();
                          else if (AgentObjectTypeId == 6)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.StaffInfo.Subject.Id).FirstOrDefault();
                          else if (AgentObjectTypeId == 7 || AgentObjectTypeId==8)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.Staff.Id == staff.Id).FirstOrDefault();
                          if (planStaff != null)
                          {
                              Result manageRatingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
                              mangeResult.Add(manageRatingResult);

                              if (manageRatingResult != null)
                              {
                                  RecordSingleResultDTO newRecord = ParseResultRecord(manageRatingResult);
                                  newRecord.IsMainPosition = true;
                                  newRecord.PositionRate = Convert.ToDouble(ConfigurationManager.AppSettings["TiLeChucVuChinh"]);
                                  manageRecordResultList.Results.Add(newRecord);
                              }
                          }
                      }

                      //Chức vụ kiêm nhiệm
                      foreach (SubPosition po in staff.StaffInfo.SubPositions)
                      {
                          int AgentObjectTypeId = po.Position.AgentObjectType.Id;


                          if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id).FirstOrDefault();
                          else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id).FirstOrDefault();
                          else if (AgentObjectTypeId == 6)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.StaffInfo.Subject.Id).FirstOrDefault();
                          else if (AgentObjectTypeId == 7 || AgentObjectTypeId == 8)
                              planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.Staff.Id == staff.Id).FirstOrDefault();
                          if (planStaff != null)
                          {
                              Result manageRatingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
                              mangeResult.Add(manageRatingResult);

                              if (manageRatingResult != null)
                              {
                                  RecordSingleResultDTO newRecord = ParseResultRecord(manageRatingResult);
                                  newRecord.IsMainPosition = false;
                                  newRecord.PositionRate = staff.StaffInfo.SubPositions.Count == 1 ? Convert.ToDouble(ConfigurationManager.AppSettings["TiLeChucVuKiemNhiem"]) : Convert.ToDouble(ConfigurationManager.AppSettings["TiLeChucVuChinh"]) / staff.StaffInfo.SubPositions.Count;
                                  manageRecordResultList.Results.Add(newRecord);
                              }
                          }
                      }

                      // Kết quản kpis giảng dạy
                      ResultRecordDTO teachingResultList = new ResultRecordDTO();
                      planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id==1).FirstOrDefault();
                      if (planStaff != null)
                      {
                          Result teachingRatingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
                          RecordSingleResultDTO rsgr = ParseResultRecord(teachingRatingResult);
                          teachingResultList.Results.Add(ParseResultRecord(teachingRatingResult));
                      }
                      //kết quả nhân viên văn phòng
                      ResultRecordDTO staffResultList = new ResultRecordDTO();
                      planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id && ps.AgentObjectType.Id == 2).FirstOrDefault();
                      if (planStaff != null)
                      {
                          Result staffRatingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
                          staffResultList.Results.Add(ParseResultRecord(staffRatingResult));
                      }

                      resultDic["manageRecordResultList"] = manageRecordResultList;
                      resultDic["teachingResultList"] = teachingResultList;
                      resultDic["staffResultList"] = staffResultList;

                      ResultRecordDTO generalManageResultList = new ResultRecordDTO();
                      double finalResult = 0;
                      foreach(RecordSingleResultDTO sr in manageRecordResultList.Results)
                      {
                          RecordSingleResultDTO newsr = new RecordSingleResultDTO();
                          newsr.Name = sr.Name;
                          newsr.PositionRate = sr.PositionRate;
                          newsr.Rate = sr.Rate;
                          newsr.Rate2 = 100 - newsr.Rate;
                          newsr.Record = sr.Record;
                          newsr.Record2 =teachingResultList.Results.FirstOrDefault()!=null? teachingResultList.Results.First().Record:0;
                          newsr.TotalRecord =Math.Round((newsr.Record * newsr.Rate/100) + (newsr.Record2 * newsr.Rate2/100),2);
                          finalResult += Math.Round(newsr.TotalRecord * newsr.PositionRate / 100,2);
                          generalManageResultList.Results.Add(newsr);
                      }

                      generalManageResultList.FinalResult = finalResult;




                      resultDic["generalManageResultList"] = generalManageResultList;
                      


                      //List<Result> ratingResults = new List<Result>();
                      ////Lấy danh sách các chức vụ, giảng dạy
                      //List<AgentObjectType> agentObjectTypes = new List<AgentObjectType>();
                     
                      //if(staff.StaffInfo.Position.AgentObjectType!=null)
                      //   agentObjectTypes.Add(staff.StaffInfo.Position.AgentObjectType);

                      //List<KPI_WebUser> webUsers = session.Query<KPI_WebUser>().Where(u => u.StaffInfo.Id == staff.Id).ToList();

                      //AgentObject ao = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();

                      //List<AgentObject> otherAgentObjects = staff.StaffInfo.AgentObjects.Where(a => !agentObjectTypes.Any(at => at.Id == a.AgentObjectType.Id)).ToList();

                      //foreach (AgentObject ag in otherAgentObjects)
                      //{
                      //    agentObjectTypes.Add(ag.AgentObjectType);
                      //}
                      //if (staff.StaffInfo.AgentObjects.Count > 1)
                      //{
                      //    AgentObject agentObj = session.Query<AgentObject>().Where(a => a.Id == agentObjectId).SingleOrDefault();                         
                      //}

                      
                      //foreach(AgentObjectType at in agentObjectTypes)
                      //{
                      //    int AgentObjectTypeId = at.Id;

                      //    PlanStaff planStaff = new PlanStaff();
                      //    if (AgentObjectTypeId == 1 || AgentObjectTypeId == 2)
                      //        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Staff.Id == staff.Id).FirstOrDefault();
                      //    else if (AgentObjectTypeId == 3 || AgentObjectTypeId == 5)
                      //        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id).FirstOrDefault();
                      //    else if (AgentObjectTypeId == 6)
                      //        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.StaffInfo.Subject.Id).FirstOrDefault();
                      //    else if (AgentObjectTypeId == 7)
                      //        planStaff = session.Query<PlanStaff>().Where(ps => ps.PlanKPI.Id == planId && ps.Department.Id == staff.Department.Id && ps.Staff.Id == staff.Id).FirstOrDefault();
                      //    Result ratingResult =new Result();
                      //    if (planStaff != null)
                      //    {
                      //        ratingResult = session.Query<Result>().SingleOrDefault(r => r.PlanStaff.Id == planStaff.Id);
                      //    }

                      //    ratingResults.Add(ratingResult);                                                  
                      //}
                      //RecordSingleResultDTO newRecord = ParseResultRecord(ratingResults[0], ratingResults[1]);
                      //result.Results.Add(newRecord);                          
                  }
            });
            return resultDic;
        }

        [Authorize]
        [Route("")]
        public ResultDTO GetClass(Guid id)
        {
            ResultDTO result = new ResultDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<Result>().SingleOrDefault(a => a.Id == id).Map<ResultDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public Result Put(Result obj)
        {
            if (obj.Id == Guid.Empty)
                obj.Id = Guid.NewGuid();
            SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            return obj;
        }

        [Authorize]
        [Route("")]
        public Result Delete(Result obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
        }
    }
}
