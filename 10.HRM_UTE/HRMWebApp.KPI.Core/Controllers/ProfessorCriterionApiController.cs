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
using System.Configuration;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ProfessorCriterionApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetList()
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ProfessorCriterion>().ToList().Map<ProfessorCriterionDTO>();
            });
            return result;
        }

        //[Authorize]
        //[Route("")]
        //public ProfessorCriterionDTO GetObj(Guid id)
        //{
        //    ProfessorCriterionDTO result = new ProfessorCriterionDTO();
        //    SessionManager.DoWork(session =>
        //    {
        //        ProfessorCriterion tgd = session.Query<ProfessorCriterion>().SingleOrDefault(a => a.Id == id);
        //        if (tgd != null)
        //        {
        //            result = tgd.Map<ProfessorCriterionDTO>();
        //            result.StudyYearIds = new List<Guid>();                   
        //            foreach (StudyYear y in tgd.StudyYears)
        //            {
        //                result.StudyYearIds.Add(y.Id);
        //            }                  
        //        }
        //    });
        //    return result;
        //}

        [Authorize]
        [Route("")]
        public IEnumerable<StudyYearDTO> GetStudyYear()
        {
            List<StudyYearDTO> result = new List<StudyYearDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<StudyYear>().ToList().Map<StudyYearDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ChucDanhDTO> GetListChucDanh()
        {
            List<ChucDanhDTO> result = new List<ChucDanhDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ChucDanh>().ToList().Map<ChucDanhDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListByTargetGroupDetailId(Guid targetId)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                List<ProfessorCriterion> cri = session.Query<ProfessorCriterion>().Where(p => p.TargetGroupDetail.Id == targetId).ToList();
                foreach (ProfessorCriterion c in cri)
                {
                    ProfessorCriterionDTO crd = new ProfessorCriterionDTO();
                    crd.Id = c.Id;
                    crd.Name = c.Name;
                    crd.NumberOfHour = c.NumberOfHour;
                    crd.Record = c.Record;              
                    result.Add(crd);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<CriterionType> GetCriterionTypeList()
        {
            List<CriterionType> result = new List<CriterionType>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionType>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<DonViTinh> GetDonViTinhList()
        {
            List<DonViTinh> result = new List<DonViTinh>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<DonViTinh>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<DanhGiaChiTiet> GetDanhGiaChiTietList()
        {
            List<DanhGiaChiTiet> result = new List<DanhGiaChiTiet>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<DanhGiaChiTiet>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public ProfessorCriterionDTO GetListByClass(Guid id)
        {
            ProfessorCriterionDTO result = new ProfessorCriterionDTO();
            SessionManager.DoWork(session =>
            {
                ProfessorCriterion criterion = session.Query<ProfessorCriterion>().SingleOrDefault(a => a.Id == id);
                result = criterion.Map<ProfessorCriterionDTO>();
                result.TargetGroupDetail = new TargetGroupDetail() { Id = criterion.TargetGroupDetail.Id };
                result.CriterionType = new CriterionType() { Id = result.CriterionType != null ? result.CriterionType.Id : 0 };
                result.StudyYearIds = new List<Guid>();
                foreach (StudyYear y in criterion.StudyYears)
                {
                    result.StudyYearIds.Add(y.Id);
                }
                result.DonViTinh = new DonViTinh() { Id = result.DonViTinh != null ? result.DonViTinh.Id : 0 };
                result.DanhGiaChiTiet = new DanhGiaChiTiet() { Id = result.DanhGiaChiTiet != null ? result.DanhGiaChiTiet.Id : 0 };
                result.DepartmentIds = new List<Guid>();
                foreach(Department d in criterion.DonViCungCap)
                {
                    result.DepartmentIds.Add(d.Id);
                }
                result.ChucDanhIds = new List<Guid>();
                foreach(ChucDanh c in criterion.ChucDanh)
                {
                    result.ChucDanhIds.Add(c.Id);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetListbyId(Guid classId)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ProfessorCriterion>().Where(a => a.TargetGroupDetail.Id == classId).Map<ProfessorCriterionDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ProfessorCriterionDTO> GetSearch(Guid targetGroupDetailId,Guid studyYear)
        {
            List<ProfessorCriterionDTO> result = new List<ProfessorCriterionDTO>();
            SessionManager.DoWork(session =>
            {
                var list = session.Query<ProfessorCriterion>().Where(a => (targetGroupDetailId == Guid.Empty || targetGroupDetailId == a.TargetGroupDetail.Id) && (studyYear == Guid.Empty || a.StudyYears.Any(aa => aa.Id == studyYear))).OrderBy(a => a.OrderNumber);//.Map<ProfessorCriterionDTO>();
                foreach(var i in list)
                {
                    ProfessorCriterionDTO newItem = new ProfessorCriterionDTO();
                    newItem.Id = i.Id;
                    newItem.Name = i.Name;
                    newItem.OrderNumber = i.OrderNumber;
                    newItem.Record = i.Record;
                    newItem.Tooltip = i.Tooltip;
                    newItem.TargetGroupDetail = i.TargetGroupDetail;
                    newItem.TargetGroupDetailId = i.TargetGroupDetail.Id;
                    newItem.ManageCode = i.ManageCode;
                    newItem.StudyYearIds = new List<Guid>();
                    foreach (StudyYear n in i.StudyYears)
                    {
                        newItem.StudyYearIds.Add(n.Id);
                    }                   
                    result.Add(newItem);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(ProfessorCriterionDTO obj)
        {
            
            int result = 1;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ProfessorCriterion prof = new ProfessorCriterion();
                    if (obj.Id == Guid.Empty)
                        prof.Id = Guid.NewGuid();
                    else
                    {
                        prof = session.Query<ProfessorCriterion>().Where(r => r.Id == obj.Id).SingleOrDefault();
                    }
                    // prof = session.Query<ProfessorCriterion>().Where(r => r.Id == obj.Id).SingleOrDefault();
                    TargetGroupDetail tg = session.Query<TargetGroupDetail>().Where(t => t.Id == obj.TargetGroupDetail.Id).SingleOrDefault();
                    if (tg.TargetGroupDetailType.Id == 4)
                    {
                        prof.CriterionType = new CriterionType();
                        prof.CriterionType.Id = 4;
                    }
                    prof.ManageCode = obj.ManageCode;
                    prof.TargetGroupDetail = new TargetGroupDetail() { Id = obj.TargetGroupDetail.Id };
                    prof.CriterionType = new CriterionType() { Id = obj.CriterionType.Id };
                    prof.Record = obj.Record;
                    prof.OrderNumber = obj.OrderNumber;
                    prof.Name = obj.Name;
                    prof.Tooltip = obj.Tooltip;
                    prof.StudyYears = new List<StudyYear>();
                    if (obj.StudyYearIds != null)
                    {
                        foreach (var id in obj.StudyYearIds)
                        {
                            prof.StudyYears.Add(new StudyYear() { Id = id });
                        }
                    }
                    prof.DonViTinh = new DonViTinh() { Id = obj.DonViTinh.Id };
                    prof.DanhGiaChiTiet = new DanhGiaChiTiet() { Id = obj.DanhGiaChiTiet.Id };
                    prof.DonViCungCap = new List<Department>();
                    if(obj.DepartmentIds != null)
                    {
                        foreach(var i in obj.DepartmentIds)
                        {
                            prof.DonViCungCap.Add(new Department() { Id = i });
                        }
                    }
                    prof.ChucDanh = new List<ChucDanh>();
                    if(obj.ChucDanhIds != null)
                    {
                        foreach(var c in obj.ChucDanhIds)
                        {
                            prof.ChucDanh.Add(new ChucDanh() { Id = c });
                        }
                    }
                    
                    session.SaveOrUpdate(prof);
                    result = 1;
                });
            }
            catch (Exception e)
            {
                Helper.ErrorLog("LayoutApiController/Put", e);
                throw e;
            }
            return result;
        }

        //public ProfessorCriterion  Put(ProfessorCriterionDTO obj)
        //{
        //    ProfessorCriterion prof = new ProfessorCriterion();           
        //    SessionManager.DoWork(session =>
        //        {
        //            if (obj.Id == Guid.Empty)
        //                prof.Id = Guid.NewGuid();
        //            else
        //            {
        //                prof = session.Query<ProfessorCriterion>().Where(r => r.Id == obj.Id).SingleOrDefault();
        //            }               
        //            TargetGroupDetail tg = session.Query<TargetGroupDetail>().Where(t => t.Id == obj.TargetGroupDetail.Id).SingleOrDefault();
        //            if (tg.TargetGroupDetailType.Id == 4)
        //            {
        //                prof.CriterionType = new CriterionType();
        //                prof.CriterionType.Id = 4;
        //            }
        //            prof.CriterionType.Id = obj.CriterionTypeId;
        //            prof.Record = obj.Record;
        //            prof.OrderNumber = obj.OrderNumber;

        //            if (obj.StudyYearIds != null)
        //            {
        //                foreach (var id in obj.StudyYearIds)
        //                {
        //                    prof.StudyYears.Add(new StudyYear() { Id = id });
        //                }
        //            }
        //            session.SaveOrUpdate(prof);
        //        });
        //    return prof;
        //}

        [Authorize]
        [Route("")]
        public bool GetCheckHasDictionary(Guid id)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                List<CriterionDictionary> data = new List<CriterionDictionary>();
                data = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == id).ToList();
                if (data.Count!=0)
                {
                    result = true;
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int Delete(ProfessorCriterion obj)
        {
            try
            {
                int result = 1;
                SessionManager.DoWork(session =>
                {
                    bool check = session.Query<PlanKPIDetail>().Any(p => p.FromProfessorCriterion.Id == obj.Id);
                    if (check==true)
                    {
                        result = 2;
                    }
                    else
                    {
                        session.Delete(obj);
                        result = 1;
                    }
                    
                });
                return result;
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        //Criterion Dictionary
        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionaryDTO> GetDictionnaryByCriterionId(Guid id)
        {
            List<CriterionDictionaryDTO> result = new List<CriterionDictionaryDTO>();
            SessionManager.DoWork(session =>
            {
                var tempresult = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == id).ToList();
                foreach (CriterionDictionary cri in tempresult)
                {
                    CriterionDictionaryDTO cridto = new CriterionDictionaryDTO();
                    cridto.Id = cri.Id;
                    cridto.ManageCode = cri.ManageCode;
                    cridto.NumberOfHour = cri.NumberOfHour;
                    cridto.Record = cri.Record;
                    cridto.Name = cri.Name;
                    cridto.OrderNumber = cri.OrderNumber;
                    cridto.DataMaxRecord = cri.DataMaxRecord;
                    cridto.DataRecord = cri.DataRecord;
                    result.Add(cridto);
                }
                result = result.OrderBy(q => q.OrderNumber).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionaryDTO> GetDictionnaryByProfessorCriterionId(Guid id)
        {
            List<CriterionDictionaryDTO> result = new List<CriterionDictionaryDTO>();
            SessionManager.DoWork(session =>
            {
                var tempresult = session.Query<CriterionDictionary>().Where(c => c.ProfessorCriterion.Id == id).OrderByDescending(c => c.Record).ToList();
                foreach (CriterionDictionary cri in tempresult)
                {
                    CriterionDictionaryDTO cridto = new CriterionDictionaryDTO();
                    cridto.Id = cri.Id;
                    cridto.Name = cri.Name;
                    result.Add(cridto);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<CriterionDictionary> GetDictionnaryByTargetGroupDetailId(Guid id)
        {
            List<CriterionDictionary> result = new List<CriterionDictionary>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<CriterionDictionary>().Where(c =>c.TargetGroupDetail!=null && c.TargetGroupDetail.Id == id).OrderByDescending(c => c.Record).ToList();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public CriterionDictionaryDTO GetDictionary(Guid id)
        {
            CriterionDictionaryDTO result = new CriterionDictionaryDTO();
            SessionManager.DoWork(session =>
            {
                CriterionDictionary crd = session.Query<CriterionDictionary>().SingleOrDefault(a => a.Id == id);
                result.Id = crd.Id;
                result.Name = crd.Name;
                result.NumberOfHour = crd.NumberOfHour;
                result.ManageCode = crd.ManageCode;
                result.LevelIndex = crd.LevelIndex;
                result.OrderNumber = crd.OrderNumber;
                result.Record = crd.Record;
                result.MaxRecord = crd.MaxRecord;
                result.CriterionId = crd.ProfessorCriterion.Id;
                string GiangDaySoTietChuan = ConfigurationManager.AppSettings["GiangDaySoTietChuan"];
                string ChatLuongGiangDay = ConfigurationManager.AppSettings["ChatLuongGiangDay"];
                if (result.CriterionId.ToString().ToUpper() == GiangDaySoTietChuan)
                {
                    result.Exception = 1;
                }
                if (result.CriterionId.ToString().ToUpper() == ChatLuongGiangDay)
                {
                    result.Exception = 2;
                }
                result.DataRecord = crd.DataRecord;
                result.DataMaxRecord = crd.DataMaxRecord;
                result.StudyYearIds = new List<Guid>();
                foreach (StudyYear n in crd.StudyYears)
                {
                    result.StudyYearIds.Add(n.Id);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int PutDictionary(CriterionDictionaryDTO obj)
        {
            int result = 1;
            try
            {
                SessionManager.DoWork(session =>
                {
                    CriterionDictionary prof = new CriterionDictionary();
                    if (obj.Id == Guid.Empty)
                        prof.Id = Guid.NewGuid();
                    else
                    {
                        prof = session.Query<CriterionDictionary>().Where(r => r.Id == obj.Id).SingleOrDefault();
                    }
                    prof.ManageCode = obj.ManageCode;
                    prof.Record = obj.Record;
                    prof.OrderNumber = obj.OrderNumber;
                    prof.Name = obj.Name;
                    prof.Tooltip = obj.Tooltip;
                    prof.DataRecord = obj.DataRecord;
                    prof.DataMaxRecord = obj.DataMaxRecord;
                    prof.StudyYears = new List<StudyYear>();
                    if (obj.StudyYearIds != null)
                    {
                        foreach (var id in obj.StudyYearIds)
                        {
                            prof.StudyYears.Add(new StudyYear() { Id = id });
                        }
                    }
                    prof.ProfessorCriterion = new ProfessorCriterion() { Id = obj.ProfessorCriterionId };
                    prof.TargetGroupDetail = new TargetGroupDetail() { Id = obj.targetGroupDetailId };
                    session.SaveOrUpdate(prof);
                    result = 1;
                });
            }
            catch (Exception e)
            {
                Helper.ErrorLog("LayoutApiController/PutDictionary", e);
                throw e;
            }
            return result;
            //if (obj.Id == Guid.Empty)
            //    obj.Id = Guid.NewGuid();

            //SessionManager.DoWork(session => session.SaveOrUpdate(obj));
            //return obj;
        }

        [Authorize]
        [Route("")]
        public CriterionDictionary DeleteDictionary(CriterionDictionary obj)
        {
            SessionManager.DoWork(session => session.Delete(obj));
            return obj;           
        }   
      

    }
}
