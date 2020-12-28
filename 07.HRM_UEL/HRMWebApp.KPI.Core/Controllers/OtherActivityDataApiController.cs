using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using NHibernate.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.Helpers;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class OtherActivityDataApiController : ApiController
    {

        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetList()
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<OtherActivityData>().OrderBy(d => d.StaffCode).Map<OtherActivityDataDTO>().ToList();

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetListByStaffId(Guid staffId)
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<OtherActivityData>().Where(d => d.StaffProfile.Id == staffId).OrderByDescending(d => d.StudyYear).Map<OtherActivityDataDTO>().ToList();

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<OtherActivityDataDTO> GetSearchOtherActivity(Guid deptId, string studyYear, string activityManageCode, string manageCode, string studyTerm)
        {
            var result = new List<OtherActivityDataDTO>();
            SessionManager.DoWork(session =>
            {
                bool allDept = (deptId == Guid.Empty) ? true : false;
                bool allYear = (studyYear == null || studyYear == "" || studyYear == "null") ? true : false;
                bool allActivity = (activityManageCode == null || activityManageCode == "" || activityManageCode == "null") ? true : false;
                bool allManage = (manageCode == null || manageCode == "" || manageCode == "null") ? true : false;
                bool allTerm = (studyTerm == null || studyTerm == "" || studyTerm == "null") ? true : false;
                List<OtherActivityData> temp = session.Query<OtherActivityData>().Where(d =>
                (allDept || d.Department.Id == deptId)
                && (allYear || d.StudyYear == studyYear)
                && (allActivity || d.ActivityManageCode == activityManageCode)
                && (allManage || d.ManageCode == manageCode)
                && (allTerm || d.StudyTerm == studyTerm)
                ).ToList();
                foreach (OtherActivityData o in temp)
                {
                    OtherActivityDataDTO od = new OtherActivityDataDTO();
                    od.Id = o.Id;
                    od.ActivityManageCode = o.ActivityManageCode;
                    od.DepartmentName = o.Department != null ? o.Department.Name : "";
                    od.ManageCode = o.ManageCode;
                    od.Name = o.Name;
                    od.NumberOfTime = o.NumberOfTime;
                    od.StaffCode = o.StaffCode;
                    od.StaffName = o.StaffProfile != null ? o.StaffProfile.Name : "";
                    od.StudyTerm = o.StudyTerm;
                    od.StudyYear = o.StudyYear;
                    result.Add(od);
                }

            });
            return result;
        }
        [Authorize]
        [Route("")]
        public OtherActivityDataDTO GetObj(Guid id)
        {
            var result = new OtherActivityDataDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<OtherActivityData>().SingleOrDefault(a => a.Id == id).Map<OtherActivityDataDTO>();
                //if (sr != null)
                //{
                //    result.Id = sr.Id;
                //    result.Name = sr.CriterionDictionary.Name;
                //    result.PlanKPIDetailId = sr.PlanKPIDetail != null ? sr.PlanKPIDetail.Id : Guid.Empty;
                //    result.NumberOfResearch = sr.NumberOfResearch;

                //}
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListProfessorCriterion()
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorCriterion> temp = session.Query<ProfessorCriterion>().Where(p => p.CriterionType.Id == 4 && p.TargetGroupDetail.Id == new Guid("E5C77D1A-5C95-464A-9846-23A987698C11")).ToList();
                foreach (ProfessorCriterion p in temp)
                {
                    ProfessorCriterionDTO pd = new ProfessorCriterionDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    pd.ManageCode = p.ManageCode;
                    result.Add(pd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<PlanKPIDTO> GetListStudyYear()
        {
            List<PlanKPIDTO> result = new List<PlanKPIDTO>();
            SessionManager.DoWork(session =>
            {
                List<PlanKPI> temp = session.Query<PlanKPI>().Where(p => p.StudyYear != null).ToList();
                List<string> tempstring = new List<string>();
                foreach (PlanKPI pl in temp)
                {
                    tempstring.Add(pl.StudyYear);
                }
                tempstring = tempstring.Distinct().ToList();
                foreach (string year in tempstring)
                {
                    PlanKPIDTO pld = new PlanKPIDTO();
                    pld.StudyYear = year;
                    result.Add(pld);
                }

            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionaryDTO> GetListDictionaryByManageCode(string manageCode)
        {
            List<CriterionDictionaryDTO> result = new List<CriterionDictionaryDTO>();
            SessionManager.DoWork(session =>
            {
                List<CriterionDictionary> temp = session.Query<CriterionDictionary>().Where(p => p.ProfessorCriterion.ManageCode == manageCode && manageCode != null && manageCode != "").ToList();
                foreach (CriterionDictionary p in temp)
                {
                    CriterionDictionaryDTO pd = new CriterionDictionaryDTO();
                    pd.Id = p.Id;
                    pd.Name = p.Name;
                    pd.ManageCode = p.ManageCode;
                    result.Add(pd);
                }
            });
            return result;
        }

        //[Authorize]
        //[Route("")]
        //public Guid Put(OtherActivityData obj)
        //{
        //    Guid result = Guid.Empty;
        //    try
        //    {
        //        if (obj.Id==Guid.Empty)
        //        {
        //            obj.Id = Guid.NewGuid();
        //        }
        //        SessionManager.DoWork(session =>
        //        {
        //            Staff staff = session.Query<Staff>().Where(s => s.Id == obj.StaffProfile.Id).SingleOrDefault();
        //            obj.StaffCode = staff.StaffInfo.WebUser.UserName;
        //            CriterionDictionary cri = session.Query<CriterionDictionary>().Where(c => c.ManageCode.ToUpper() == obj.ManageCode.ToUpper()).SingleOrDefault();
        //            obj.Name = cri.Name;
        //            session.SaveOrUpdate(obj);
        //        });
        //}
        //    catch (Exception e)
        //    {

        //    }
        //    return result;
        //}
        [Authorize]
        [Route("")]
        public Guid Put(OtherActivityDataDTO obj)
        {
            Guid result = Guid.Empty;
            try
            {
                //if (obj.Id == Guid.Empty)
                //{
                //    obj.Id = Guid.NewGuid();
                //}
                SessionManager.DoWork(session =>
                {
                    foreach (Guid staffId in obj.StaffIds)
                    {
                        OtherActivityData oad = new OtherActivityData();
                        oad.Id = Guid.NewGuid();
                        oad.OrderNumber = obj.OrderNumber;
                        oad.Date = obj.Date;
                        oad.TotalParticipants = obj.TotalParticipants;
                        oad.ActivityManageCode = obj.ActivityManageCode;
                        oad.ManageCode = obj.ManageCode;
                        oad.Name = obj.Name;
                        oad.NumberOfTime = 1;
                        oad.StudyTerm = obj.StudyTerm;
                        oad.StudyYear = obj.StudyYear;
                        Staff staff = session.Query<Staff>().Where(s => s.Id == staffId).SingleOrDefault();
                        oad.StaffCode = staff.StaffInfo.ManageCode;
                        oad.StaffProfile = new StaffProfile { Id = staffId };
                        oad.Department = new Department { Id = staff.Department.Id };
                        session.Save(oad);
                    }
                });
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public OtherActivityData Delete(OtherActivityData obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;
        }
    }
}
