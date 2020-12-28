using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;
using System.Runtime.Caching;
using System.Collections.Concurrent;


namespace HRMWebApp.KPI.Core.Helpers
{
    public enum AgentObjectTypes : int
    {
        GiangVien = 1,
        NhanVien,
        PhongBan,
        BanGiamHieu,
        Khoa,
        BoMon,
        PhoPhongBan,
        PhoKhoa,
        PhoBoMon,
        HieuTruong,
        PhoHieuTruong
    }

    public static class ControllerHelpers
    {


        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId)
        {
            PlanKPIDetail pld = null;
            SessionManager.DoWork(session =>
            {
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (pld != null && pld.ParentPlanKPIDetail != null)
                {
                    while (pld.ParentPlanKPIDetail != null)
                    {
                        pld = pld.ParentPlanKPIDetail;
                    }
                }
            });

            return pld;
        }

        public static Staff GetSubjectLeaderFromCriterion(Criterion cri)
        {
            Staff staff = new Staff();
            SessionManager.DoWork(session =>
            {
                if (cri.FromPlanKPIDetail.PlanStaff!=null && cri.FromPlanKPIDetail.PlanStaff.Staff!=null)
                {
                    staff = session.Query<Staff>().Where(s => s.Id == cri.FromPlanKPIDetail.PlanStaff.Staff.Id).FirstOrDefault();
                }
            });

            return staff;
        }

        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId, Dictionary<Guid, PlanKPIDetail> planDetailsDic, NHibernate.ISession session)
        {
            PlanKPIDetail pld = null;

            pld = planDetailsDic[pldId];
            if (pld != null && pld.ParentPlanKPIDetail != null)
            {
                while (pld.ParentPlanKPIDetail != null)
                {
                    pld = pld.ParentPlanKPIDetail;
                }
            }
            return pld;
        }

        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId, NHibernate.ISession session)
        {
            PlanKPIDetail pld = null;

            //pld = GetPlanDetailDic(session)[pldId];
            if (GetPlanDetailDic(session).ContainsKey(pldId))
                pld = GetPlanDetailDic(session)[pldId];
            else
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
            if (pld != null && pld.ParentPlanKPIDetail != null)
            {
                while (pld.ParentPlanKPIDetail != null)
                {
                    pld = pld.ParentPlanKPIDetail;
                }
            }
            return pld;
        }

        public static List<PlanKPIDetail> GetAllChildPlanKPIDetail(Guid pldId, NHibernate.ISession session)
        {
            List<PlanKPIDetail> result = new List<PlanKPIDetail>(); 

            PlanKPIDetail pld = null;

            //pld = GetPlanDetailDic(session)[pldId];
            if (GetPlanDetailDic(session).ContainsKey(pldId))
                pld = GetPlanDetailDic(session)[pldId];
            else
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);

            if (pld != null)
            {
                result= GetPlanDetailDic(session).Where(p => GetOriginalParentPlanKPIDetail(p.Key,session).Id == pldId).Select(p=>p.Value).ToList();

                result = result.Where(p => p.Id != pldId).ToList();
            }
            return result;
        }

        public static PlanKPIDetail GetOriginalParentPlanKPIDetailTest(Guid pldId)
        {
            PlanKPIDetail pld = null;
            SessionManager.DoWork(session =>
            {
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (pld != null && pld.ParentPlanKPIDetail != null)
                    pld = GetOriginalParentPlanKPIDetailTest(pld.ParentPlanKPIDetail.Id);
            });
            return pld;
        }
        public static PlanKPIDetailDTO GetParentPlanKPIDetail(Guid pldId)
        {
            PlanKPIDetailDTO parentPlanDTO = new PlanKPIDetailDTO();
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail parentPlan = null;
                PlanKPIDetail planpld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
                if (planpld != null && planpld.ParentPlanKPIDetail != null)
                {
                    parentPlan = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == planpld.ParentPlanKPIDetail.Id);
                    parentPlanDTO.Id = parentPlan.Id;
                    parentPlanDTO.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                    foreach (PlanKPIDetail_KPI kpi in parentPlan.PlanKPIDetail_KPIs)
                    {
                        PlanKPIDetail_KPIDTO k = new PlanKPIDetail_KPIDTO();
                        k.Id = kpi.Id;
                        k.Name = kpi.Name;
                        parentPlanDTO.PlanKPIDetail_KPIs.Add(k);
                    }
                }
            });
            return parentPlanDTO;
        }

        public static PlanKPIDetailDTO GetParentPlanKPIDetail(Guid pldId, NHibernate.ISession session)
        {
            PlanKPIDetailDTO parentPlanDTO = new PlanKPIDetailDTO();

            PlanKPIDetail parentPlan = null;
            PlanKPIDetail planpld = new PlanKPIDetail();
            if (GetPlanDetailDic(session).ContainsKey(pldId))
                planpld = GetPlanDetailDic(session)[pldId];
            else
                planpld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
            if (planpld != null && planpld.ParentPlanKPIDetail != null)
            {
                parentPlan = GetPlanDetailDic(session)[planpld.ParentPlanKPIDetail.Id];
                parentPlanDTO.Id = parentPlan.Id;
                parentPlanDTO.PlanKPIDetail_KPIs = new List<PlanKPIDetail_KPIDTO>();
                foreach (PlanKPIDetail_KPI kpi in parentPlan.PlanKPIDetail_KPIs)
                {
                    PlanKPIDetail_KPIDTO k = new PlanKPIDetail_KPIDTO();
                    k.Id = kpi.Id;
                    k.Name = kpi.Name;
                    parentPlanDTO.PlanKPIDetail_KPIs.Add(k);
                }
                //parentPlanDTO.StaffLeader = new Staff();
                //parentPlanDTO.StaffLeader.Id = parentPlan.StaffLeader.Id;
                //parentPlanDTO.StaffLeader.StaffProfile = new StaffProfile();
                //parentPlanDTO.StaffLeader.StaffProfile.Name = parentPlan.StaffLeader.StaffProfile!=null ? parentPlan.StaffLeader.StaffProfile.Name :"";
            }
            return parentPlanDTO;
        }

        public static List<MethodDTO> GetOriginalMethods(Guid pldId)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId);
            SessionManager.DoWork(session =>
            {
                //PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
                if (originalPld != null)
                {
                    foreach (Method me in originalPld.Methods)
                    {
                        result.Add(me.Map<MethodDTO>());
                    }
                }
            });
            return result;
        }

        public static double GetOriginalDensity(Guid pldId, NHibernate.ISession session)
        {
            double result = 0;
            //List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail pld = null;
            if (GetPlanDetailDic(session).ContainsKey(pldId))
                pld = GetPlanDetailDic(session)[pldId];
            else
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
            if (pld != null && pld.ParentPlanKPIDetail != null)
            {
                while (pld.ParentPlanKPIDetail != null && pld.ParentPlanKPIDetail.MaxRecord != 0)
                {
                    pld = pld.ParentPlanKPIDetail;
                    result = pld.MaxRecord;
                }
            }

            if (result == 0)
            {
                if (GetPlanDetailDic(session).ContainsKey(pldId))
                    result = GetPlanDetailDic(session)[pldId].MaxRecord;
                else
                    result = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId).MaxRecord;
            }
         
            return result;
        }

        public static List<MethodDTO> GetOriginalMethods(Guid pldId, NHibernate.ISession session)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = new PlanKPIDetail();
            if (GetPlanDetailDic(session).ContainsKey(originalPld.Id))
                pld = GetPlanDetailDic(session)[originalPld.Id];
            else
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.Map<MethodDTO>());
                }
            }

            return result;
        }

        public static List<MethodDTO> GetOriginalMethods(Guid pldId, Dictionary<Guid, PlanKPIDetail> planDetailsDic, NHibernate.ISession session)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, planDetailsDic, session);

            PlanKPIDetail pld = new PlanKPIDetail();
            if (GetPlanDetailDic(session).ContainsKey(originalPld.Id))
                pld = GetPlanDetailDic(session)[originalPld.Id];
            else
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.Map<MethodDTO>());
                }
            }

            return result;
        }

        public static List<DateTime> GetOriginalMethodsStartTime(Guid pldId, NHibernate.ISession session)
        {
            List<DateTime> result = new List<DateTime>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.StartTime);
                }
            }

            return result;
        }

        public static List<DateTime> GetOriginalMethodsEndTime(Guid pldId, NHibernate.ISession session)
        {
            List<DateTime> result = new List<DateTime>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == originalPld.Id);
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.EndTime);
                }
            }



            return result;
        }

        public static Staff GetCurrentStaff(NHibernate.ISession session)
        {
            Staff result = new Staff();
            // Nếu user có thông tin nhân viên
            ApplicationUser currentUser = AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name);
            if (currentUser.Id != null)
            {
                Guid staffId = new Guid(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).Id);
                result = session.Query<Staff>().SingleOrDefault(s => s.Id == staffId);
                //if (currentUser.AgentObjectTypeId == "2")
                //{

                //    result.StaffInfo.Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 2 } };
                //}
                if (currentUser.IsKPIs)
                    // result.StaffInfo.Position.AgentObjectType = new AgentObjectType() { Id = Convert.ToInt32(currentUser.AgentObjectTypeId) };
                    SessionHelper.Data(SessionKey.IsKPIs, true);
                else
                    SessionHelper.Data(SessionKey.IsKPIs, false);
            }
            //Nếu user ủy quyền không có thông tin nhân viên, mặc định gán cho trưởng đơn vị
            else
            {
                Guid departmentId = Guid.Empty;
                int agentObjectTypeId = 0;
                if (currentUser.DepartmentId != null)
                {
                    departmentId = new Guid(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).DepartmentId);
                }
                if (currentUser.AgentObjectTypeId != null)
                {
                    agentObjectTypeId = Convert.ToInt16(AuthenticationHelper.GetUserById(new Guid(HttpContext.Current.User.Identity.GetUserId()), HttpContext.Current.User.Identity.Name).AgentObjectTypeId);

                }
                if (!currentUser.IsKPIs)
                    result = session.Query<Staff>().SingleOrDefault(a => a.Department.Id == departmentId && a.StaffInfo.Position.AgentObjectType.Id == agentObjectTypeId);
                else
                    result = new Staff()
                    {
                        Id = Guid.Empty,
                        Department = new Department() { Id = departmentId },
                        StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = agentObjectTypeId } } }
                    };
                SessionHelper.Data(SessionKey.IsKPIs, true);
            }
            //Nếu không có staff, trường hợp là admin KPIs, gắn mặc định cho 1 ng trong BGH
            if (result == null && !currentUser.IsKPIs)
            {
                result = new Staff()
                {
                    Id = Guid.Empty,
                    StaffProfile = new StaffProfile() { Name = currentUser.UserName },
                    //Department = new Department() { Id = departmentId },
                    StaffInfo = new StaffInfo() { Position = new Position() { AgentObjectType = new AgentObjectType() { Id = 4 } } }
                };
                result = session.Query<Staff>().Where(s => (s.StaffInfo.Position.AgentObjectType.Id == 10 || s.StaffInfo.Position.AgentObjectType.Id == 11) && s.StaffProfile.GCRecord == null && s.StaffStatus.NoLongerWork == 0).FirstOrDefault();
            }
            return result;
        }

        public static PlanKPIDetail ParsePlanKPIDetail(PlanKPIMakingDetailDTO originalPlan, PlanKPIDetail updatePlanKPIDetail, PlanStaff planStaff, TargetGroupPlanMakingDTO tg, bool isNew)
        {
            PlanKPIDetail pdn = new PlanKPIDetail();
            if (!isNew)
            {
                pdn = updatePlanKPIDetail;
                pdn.CreateTime = updatePlanKPIDetail.CreateTime;
            }
            else
            {
                pdn.Id = Guid.NewGuid();
                pdn.CreateTime = DateTime.Now;
            }

            pdn.Name = originalPlan.Name;
            pdn.TargetDetail = originalPlan.TargetDetail;
            pdn.BasicResource = originalPlan.BasicResource;
            pdn.IsAddition = originalPlan.IsAddition;
            pdn.ExecuteMethod = originalPlan.ExecuteMethod;
            //Bug kendo truyền dữ liệu trễ 1 ngày => AddDays(1)
            pdn.PreviousKPI = originalPlan.PreviousKPI;
            pdn.CurrentKPI = originalPlan.CurrentKPI;
            pdn.IsDisable = originalPlan.IsDisable;
            pdn.IsLocked = originalPlan.IsLocked;
            pdn.MaxRecord = originalPlan.MaxRecord;
            pdn.FromCriterion = originalPlan.FromCriterion != null ? new Criterion() { Id = originalPlan.FromCriterion.Id } : null;
            //pdn.FromCriterion = originalPlan.FromCriterionId != Guid.Empty ? new Criterion() { Id = originalPlan.FromCriterionId } : null;
            pdn.MeasureUnit = originalPlan.MeasureUnitDTO != null ? new MeasureUnit() { Id = originalPlan.MeasureUnitDTO.Id } : null;
            pdn.TargetGroupDetail = new TargetGroupDetail() { Id = tg.TargetGroupId };
            pdn.PlanStaff = planStaff;
            pdn.StaffLeader = originalPlan.StaffLeader != null ? new Staff() { Id = originalPlan.StaffLeader.Id } : null;
            pdn.LeadDepartment = originalPlan.LeadDepartment != null ? new Department() { Id = originalPlan.LeadDepartment.Id } : null;
            pdn.ManageCode = originalPlan.ManageCode != null ? new ManageCode() { Id = originalPlan.ManageCode.Id } : null;
            pdn.StartTime = DateTime.Now;
            pdn.EndTime = DateTime.Now;
            if (isNew)
            {
                foreach (Guid did in originalPlan.SubDepartmentIds)
                {
                    if (did.ToString() != "00000000-0000-0000-0000-000000000001" && did.ToString() != "00000000-0000-0000-0000-000000000002" && did.ToString() != "00000000-0000-0000-0000-000000000003")
                    {
                        pdn.SubDepartments.Add(new Department() { Id = did });
                    }

                }

                foreach (Guid did in originalPlan.SubStaffIds)
                {
                    pdn.SubStaffs.Add(new Staff() { Id = did });
                }
            }
            else
            {
                //Xoa dept cu
                List<Department> listSubDept = pdn.SubDepartments.ToList();
                List<Guid> originalSubDepartmentIds = pdn.SubDepartments.Select(s => s.Id).ToList();
                foreach (Guid osdid in originalSubDepartmentIds)
                {
                    Department removedep = listSubDept.Where(l => l.Id == osdid).SingleOrDefault();
                    pdn.SubDepartments.Remove(removedep);
                }
                //Add dept moi
                foreach (Guid did in originalPlan.SubDepartmentIds)
                {
                    if (did.ToString() != "00000000-0000-0000-0000-000000000001" && did.ToString() != "00000000-0000-0000-0000-000000000002" && did.ToString() != "00000000-0000-0000-0000-000000000003")
                    {
                        pdn.SubDepartments.Add(new Department() { Id = did });
                    }
                }

                //Xoa staff cu
                List<Staff> listSubStaff = pdn.SubStaffs.ToList();
                List<Guid> originalSubStaffIds = pdn.SubStaffs.Select(s => s.Id).ToList();

                foreach (Guid osdid in originalSubStaffIds)
                {
                    Staff removestaff = listSubStaff.Where(l => l.Id == osdid).SingleOrDefault();
                    pdn.SubStaffs.Remove(removestaff);
                }

                //Add staff moi
                foreach (Guid did in originalPlan.SubStaffIds)
                {
                    pdn.SubStaffs.Add(new Staff() { Id = did });
                }
                if (originalPlan.ActivityIds != null)
                {
                    foreach (Guid did in originalPlan.ActivityIds)
                    {
                        pdn.ProfessorOtherActivities.Add(new ProfessorOtherActivity() { Id = did });
                    }
                }
                if (originalPlan.ScienceResearchIds != null)
                {
                    foreach (Guid did in originalPlan.ScienceResearchIds)
                    {
                        pdn.ScienceResearches.Add(new ScienceResearch() { Id = did });
                    }
                }
            }
            return pdn;
        }

        public static PlanKPIDetail ParseProfessorPlanKPIDetail(PlanKPIMakingDetailDTO originalPlan, PlanKPIDetail updatePlanKPIDetail, PlanStaff planStaff, TargetGroupPlanMakingDTO tg, bool isNew, NHibernate.ISession session)
        {
            PlanKPIDetail pdn = new PlanKPIDetail();
            if (!isNew)
            {
                pdn = updatePlanKPIDetail;
                pdn.CreateTime = updatePlanKPIDetail.CreateTime;
            }
            else
            {
                pdn.Id = Guid.NewGuid();
                pdn.CreateTime = DateTime.Now;
            }

            pdn.Name = originalPlan.Name;
            pdn.TargetDetail = originalPlan.TargetDetail;
            pdn.BasicResource = originalPlan.BasicResource;
            pdn.ExecuteMethod = originalPlan.ExecuteMethod;
            pdn.PreviousKPI = originalPlan.PreviousKPI;
            pdn.CurrentKPI = originalPlan.CurrentKPI;
            pdn.FromProfessorCriterion = originalPlan.FromProfessorCriterionId != Guid.Empty ? new ProfessorCriterion() { Id = originalPlan.FromProfessorCriterionId } : null;
            pdn.TargetGroupDetail = new TargetGroupDetail() { Id = tg.TargetGroupId };
            pdn.PlanStaff = planStaff;
            pdn.StartTime = originalPlan.StartTime.ToLocalTime();
            pdn.EndTime = originalPlan.EndTime.ToLocalTime();
            //Kiểm tra hđ hiện tại nếu chưa có trong hđ cũ thì thêm mới
            if (originalPlan.ActivityIds != null)
            {
                foreach (Guid did in originalPlan.ActivityIds)
                {
                    bool check = updatePlanKPIDetail.ProfessorOtherActivities.Any(p => p.CriterionDictionary.Id == did);
                    if (check == false)
                    {
                        ProfessorOtherActivity pa = new ProfessorOtherActivity();
                        pa.Id = Guid.NewGuid();
                        pa.CriterionDictionary = new CriterionDictionary() { Id = did };
                        pdn.ProfessorOtherActivities.Add(pa);
                    }
                }
            }
            if (originalPlan.ScienceResearchIds != null)
            {
                foreach (Guid did in originalPlan.ScienceResearchIds)
                {
                    bool check = updatePlanKPIDetail.ScienceResearches.Any(p => p.CriterionDictionary.Id == did);
                    if (check == false)
                    {
                        ScienceResearch sr = new ScienceResearch();
                        sr.Id = Guid.NewGuid();
                        sr.CriterionDictionary = new CriterionDictionary() { Id = did };
                        pdn.ScienceResearches.Add(sr);
                    }
                }
            }

            return pdn;
        }

        public static PlanKPIMakingDetailDTO GetPlanDetailActivities(Guid planKPIDetailId)
        {
            PlanKPIMakingDetailDTO result = new PlanKPIMakingDetailDTO();
            SessionManager.DoWork(session =>
            {
                PlanKPIDetail pld = session.Query<PlanKPIDetail>().Where(p => p.Id == planKPIDetailId).SingleOrDefault();
                result.Id = pld.Id;
                foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                {
                    result.ActivityIds.Add(pa.Id);
                    result.ActivityNames.Add(pa.CriterionDictionary.Name);
                }
            });
            return result;
        }

        #region  Parse Plan Detail

        public static PlanKPIMakingDetailDTO ParsePlanDetail(Criterion cri, PlanKPIDetail pld, int type, NHibernate.ISession session)
        {
            PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
            switch (type)
            {
                case 1:
                    {
                        pd.FromCriterion = new CriterionDTO();
                        pd.FromCriterion.Id = cri.Id;
                        pd.FromCriterion.Name = cri.Name;
                        pd.FromCriterion.CriterionType = pd.FromCriterion.CriterionType != null ? new CriterionType() { Id = cri.CriterionType.Id } : null;
                        pd.FromCriterion.CriterionTypeId = cri.CriterionType.Id;
                        pd.FromCriterion.Department = pd.FromCriterion.Department != null ? new DepartmentDTO() { Id = cri.Department.Id } : null;
                        pd.FromCriterion.DepartmentId = cri.Department != null ? cri.Department.Id : Guid.Empty;
                        pd.FromCriterion.TargetGroupDetail = pd.FromCriterion.TargetGroupDetail != null ? new TargetGroupDetail() { Id = cri.TargetGroupDetail.Id } : null;
                        pd.FromCriterion.TargetGroupDetailId = cri.TargetGroupDetail.Id;
                        pd.FromCriterionId = pd.FromCriterion.Id;
                        pd.LeadDepartment = cri.FromPlanKPIDetail.LeadDepartment != null ? cri.FromPlanKPIDetail.LeadDepartment.Map<DepartmentDTO>() : null;
                        pd.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? cri.FromPlanKPIDetail.StaffLeader.Map<StaffDTO>() : null;
                        foreach (Department sub in cri.FromPlanKPIDetail.SubDepartments)
                        {
                            pd.SubDepartmentIds.Add(sub.Id);
                            pd.SubDepartmentNames.Add(sub.Name);
                        }
                        foreach (Staff subs in cri.FromPlanKPIDetail.SubStaffs)
                        {
                            pd.SubStaffIds.Add(subs.Id);
                            pd.SubStaffNames.Add(subs.StaffProfile.Name);
                        }
                        pd.TargetDetail =(cri.Name!=null && cri.Name != "") ?cri.Name : cri.FromPlanKPIDetail.TargetDetail;
                        pd.PreviousKPI = cri.FromPlanKPIDetail.PreviousKPI;
                        pd.CurrentKPI = cri.FromPlanKPIDetail.CurrentKPI;
                        pd.BasicResource = cri.FromPlanKPIDetail.BasicResource;
                        pd.CanDelete = false;
                        pd.IsAddition = cri.FromPlanKPIDetail.IsAddition;
                        pd.IsLockable = false;                       
                        pd.IsDisable = cri.FromPlanKPIDetail != null ? cri.FromPlanKPIDetail.IsDisable : false;
                        pd.CreateTime = DateTime.Now;
                        pd.ManageCode = cri.FromPlanKPIDetail.ManageCode != null ? new ManageCode() { Id = cri.FromPlanKPIDetail.ManageCode.Id } : null;

                        Guid parentPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(cri.FromPlanKPIDetail.Id, session).Id;
                        PlanKPIDetail parentPlanDetail = new PlanKPIDetail();
                        PlanKPIDetail oriParentPlanDetail = new PlanKPIDetail();

                        //SessionManager.DoWork(session =>
                        //{
                        if (cri.FromPlanKPIDetail.ParentPlanKPIDetail != null)
                        {
                            //parentPlanDetail= session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id ==new Guid("625d2ced-24cb-4c7c-bd56-3b8c103b0a93")); 
                            //parentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == cri.FromPlanKPIDetail.ParentPlanKPIDetail.Id);

                            parentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(cri.FromPlanKPIDetail.Id, session);
                        }

                        //Methods lấy từ cấp cha cao nhất
                        oriParentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == parentPlanDetailId);
                        if (oriParentPlanDetail.Methods != null)
                        {
                            foreach (Method m in oriParentPlanDetail.Methods)
                            {
                                MethodDTO method = m.Map<MethodDTO>();
                                method.StartTimeString = m.StartTime.ToString("dd/MM/yyyy");
                                method.EndTimeString = m.EndTime.ToString("dd/MM/yyyy");
                                pd.Methods.Add(method);
                            }
                        }
                        else
                        {
                            foreach (Method m in cri.FromPlanKPIDetail.Methods)
                            {
                                MethodDTO method = m.Map<MethodDTO>();
                                method.StartTimeString = m.StartTime.ToString("dd/MM/yyyy");
                                method.EndTimeString = m.EndTime.ToString("dd/MM/yyyy");
                                pd.Methods.Add(method);
                            }
                        }
                        /// KPIs
                        //try
                        //{
                            if (parentPlanDetail.Id != Guid.Empty && parentPlanDetail.PlanKPIDetail_KPIs != null)
                            {
                                foreach (PlanKPIDetail_KPI m in parentPlanDetail.PlanKPIDetail_KPIs)
                                {
                                    PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
                                    kpi.Id = m.Id;
                                    kpi.Name = m.Name;
                                    kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
                                    kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
                                    pd.PlanKPIDetail_KPIs.Add(kpi);
                                }
                            }
                            else
                            {
                                foreach (PlanKPIDetail_KPI m in cri.FromPlanKPIDetail.PlanKPIDetail_KPIs)
                                {
                                    PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
                                    kpi.Id = m.Id;
                                    kpi.Name = m.Name;
                                    kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
                                    kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
                                    pd.PlanKPIDetail_KPIs.Add(kpi);
                                }
                            }
                            //}
                            //catch (Exception e)
                            //{

                            //}
                        //});
                    }
                    break;
                case 2:
                    {

                        pd.Id = pld.Id;
                        //pd.TargetDetail = pld.TargetDetail;
                        pd.ExecuteMethod = pld.ExecuteMethod;
                        pd.BasicResource = pld.BasicResource;
                        pd.StaffLeader = pld.StaffLeader != null ? pld.StaffLeader.Map<StaffDTO>() : null;
                        pd.LeadDepartment = pld.LeadDepartment != null ? pld.LeadDepartment.Map<DepartmentDTO>() : null;
                        pd.PreviousKPI = pld.PreviousKPI;
                        pd.CurrentKPI = pld.CurrentKPI;
                        pd.MaxRecord = ControllerHelpers.GetOriginalDensity(pld.Id, session);
                        pd.TargetGroupDetail = pld.TargetGroupDetail != null ? new TargetGroupDetailDTO() { Id = pld.TargetGroupDetail.Id } : null;
                        pd.CreateTime = pld.CreateTime;
                        pd.StartTime = pld.StartTime;
                        pd.EndTime = pld.EndTime;
                        foreach (Staff subs in pld.SubStaffs)
                        {
                            pd.SubStaffIds.Add(subs.Id);
                            pd.SubStaffNames.Add(subs.StaffProfile.Name);
                        }
                        foreach (Department subd in pld.SubDepartments)
                        {
                            pd.SubDepartmentIds.Add(subd.Id);
                            pd.SubjectIds.Add(subd.Id);
                            pd.SubjectNames.Add(subd.Name);
                        }
                        if (pld.FromCriterion != null)
                        {
                            pd.FromCriterion = new CriterionDTO();
                            pd.FromCriterion.Id = pld.FromCriterion.Id;
                            pd.FromCriterion.Name = pld.FromCriterion.Name;
                            pd.FromCriterion.CriterionType = pd.FromCriterion.CriterionType != null ? new CriterionType() { Id = pld.FromCriterion.CriterionType.Id } : null;
                            pd.FromCriterion.CriterionTypeId = pld.FromCriterion.CriterionType != null ? pld.FromCriterion.CriterionType.Id : -1;
                            pd.FromCriterion.Department = pd.FromCriterion.Department != null ? new DepartmentDTO() { Id = pld.FromCriterion.Id } : null;
                            pd.FromCriterion.DepartmentId = pld.FromCriterion.Department != null ? pld.FromCriterion.Department.Id : Guid.Empty;
                            pd.FromCriterion.TargetGroupDetail = pd.FromCriterion.TargetGroupDetail != null ? new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id } : null;
                            pd.FromCriterion.TargetGroupDetailId = pld.TargetGroupDetail != null ? pld.TargetGroupDetail.Id : Guid.Empty;
                            pd.FromCriterionId = pd.FromCriterion.Id;
                            pd.IsLockable = false;
                            if (pld.FromCriterion.FromPlanKPIDetail != null)
                            {
                                foreach (Department subd in pld.FromCriterion.FromPlanKPIDetail.SubDepartments)
                                {
                                    //pd.SubDepartmentIds.Add(subd.Id);
                                    pd.SubDepartmentNames.Add(subd.Name);
                                }
                            }
                        }
                        else
                        {
                            pd.FromCriterion = null;
                            pd.IsLockable = true;

                            //foreach (Staff subs in pld.SubStaffs)
                            //{
                            //    pd.SubStaffIds.Add(subs.Id);
                            //    pd.SubStaffNames.Add(subs.StaffProfile.Name);
                            //}
                        }
                        if (pld.FromProfessorCriterion != null)
                        {
                            pd.FromProfessorCriterionId = pld.FromProfessorCriterion.Id;
                        }
                        pd.ActivityIds = new List<Guid>();
                        foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                        {
                            //pd.ProfessorOtherActivities.Add(new ProfessorOtherActivity() { Id = did });                         
                            if(pa.CriterionDictionary!=null)
                                pd.ActivityIds.Add(pa.CriterionDictionary.Id);
                            //pd.ActivityNames.Add(pa.CriterionDictionary.Name);
                        }
                        foreach (ScienceResearch pa in pld.ScienceResearches)
                        {
                            //pd.ProfessorOtherActivities.Add(new ProfessorOtherActivity() { Id = did });                         
                            pd.ScienceResearchIds.Add(pa.CriterionDictionary.Id);
                            //pd.ActivityNames.Add(pa.CriterionDictionary.Name);
                        }
                        Guid parentPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session) != null ? ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session).Id : Guid.Empty;
                        PlanKPIDetail parentPlanDetail = new PlanKPIDetail();

                        //Lấy thông tin từ kế hoạch cấp cao nhất
                        //SessionManager.DoWork(session =>
                        //{

                        parentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == parentPlanDetailId);
                        if (parentPlanDetail != null)
                        {
                            //Kiểm tra nếu là plandetail của BGH mới gắn BGH chỉ đạo
                            if (parentPlanDetail.PlanStaff.WebGroupId!=Guid.Empty)
                            {
                                pd.AdminLeaderId = parentPlanDetail.StaffLeader != null ? parentPlanDetail.StaffLeader.Id : Guid.Empty;
                                pd.AdminLeaderName = parentPlanDetail.StaffLeader != null ? parentPlanDetail.StaffLeader.StaffProfile.Name : "";
                            }

                            pd.TargetDetail = parentPlanDetail.TargetDetail;
                            pd.CreateTime = parentPlanDetail.CreateTime;
                            //pd.MaxRecord = parentPlanDetail.MaxRecord;
                            foreach (Method m in parentPlanDetail != null ? parentPlanDetail.Methods : pld.Methods)
                            {
                                MethodDTO method = m.Map<MethodDTO>();
                                method.StartTimeString = m.StartTime.ToString("dd/MM/yyyy");
                                method.EndTimeString = m.EndTime.ToString("dd/MM/yyyy");
                                pd.Methods.Add(method);
                            }
                            //pd.Methods = pd.Methods.OrderBy(m => m.StartTime).ThenBy(m => m.Name).ToList();
                            pd.Methods = pd.Methods.OrderBy(m => m.Name).ToList();
                            foreach (FileAttachment d in (parentPlanDetail != null && parentPlanDetail.FileAttachments.Count() > 0) ? parentPlanDetail.FileAttachments : pld.FileAttachments)
                            {
                                FileAttachmentDTO dFile = d.Map<FileAttachmentDTO>();
                                pd.PlanFileDTOs.Add(dFile);
                            }

                            foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs != null ? pld.PlanKPIDetail_KPIs : parentPlanDetail.PlanKPIDetail_KPIs)
                            {
                                PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
                                kpi.Id = m.Id;
                                kpi.Name = m.Name;
                                kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
                                kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
                                pd.PlanKPIDetail_KPIs.Add(kpi);
                            }
                        }
                        pd.CanDelete = pld.Criterions.Count > 0 ? false : true;
                        //pd.CanDelete = false;
                        pd.IsAddition = pld.IsAddition;
                        pd.IsDisable = pld.IsDisable;
                        pd.IsLocked = pld.IsLocked;
                        pd.ManageId = pld.ManageCode != null ? pld.ManageCode.Id : string.Empty;
                        pd.ManageCode = pld.ManageCode != null ? new ManageCode() { Id = pld.ManageCode.Id } : null;
                        //});


                    }
                    break;
            }
            return pd;
        }

        #endregion
        public static PlanKPIDetail ParsePlanDetailFromProCri(ProfessorCriterionPlanDTO cri, Guid targetId, PlanKPI plan)
        {
            PlanKPIDetail pd = new PlanKPIDetail();
            //pd.Id = Guid.NewGuid();
            pd.TargetGroupDetail = new TargetGroupDetail() { Id = targetId };
            pd.FromProfessorCriterion = new ProfessorCriterion() { Id = cri.Id };
            pd.TargetDetail = "";
            pd.BasicResource = "";
            pd.CreateTime = DateTime.Now;
            pd.StartTime = plan.StartTime;
            pd.EndTime = plan.EndTime;

            return pd;
        }

        public static PlanKPIMakingDetailDTO ParsePlanDetailFromProfessorCriterion(ProfessorCriterionPlanDTO crd, PlanKPIDetail planDetail, PlanKPI plan, TargetGroupDetail t, PlanStaff planStaff, NHibernate.ISession session, int type)
        {
            PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
            switch (type)
            {
                //Parse từ plandetail đã có
                case 1:
                    {
                        pld.Id = planDetail.Id;
                        pld.BasicResource = planDetail.BasicResource;
                        pld.PreviousKPI = planDetail.PreviousKPI;
                        pld.CurrentKPI = planDetail.CurrentKPI;
                        pld.StartTime = planDetail.StartTime;
                        pld.EndTime = planDetail.EndTime;
                        pld.ExecuteMethod = planDetail.ExecuteMethod;
                        pld.Name = planDetail.Name;
                        pld.FromProfessorCriterionId = crd.Id;
                        pld.TargetDetail = planDetail.TargetDetail;
                        pld.Record = pld.FromProfessorCriterionId != Guid.Empty ? session.Query<ProfessorCriterion>().SingleOrDefault(c => c.Id == pld.FromProfessorCriterionId).Record : 0;
                        pld.CriterionTypeId = planDetail.FromProfessorCriterion.CriterionType.Id;
                        pld.OrderNumber = planDetail.FromProfessorCriterion.OrderNumber;
                        pld.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                        foreach (ProfessorOtherActivity pa in planDetail.ProfessorOtherActivities)
                        {
                            ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary!=null? pa.CriterionDictionary.Name:"";
                            pad.NumberOfTime = pa.NumberOfTime;
                            pad.NumberOfHour = pa.CriterionDictionary != null ? pa.CriterionDictionary.NumberOfHour: 0 ;
                            pld.ProfessorOtherActivities.Add(pad);
                        }
                        pld.ScienceResearches = new List<ScienceResearchDTO>();
                        foreach (ScienceResearch pa in planDetail.ScienceResearches)
                        {
                            ScienceResearchDTO pad = new ScienceResearchDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary!=null? pa.CriterionDictionary.Name:"";
                            pad.NumberOfResearch = pa.NumberOfResearch;
                            pld.ScienceResearches.Add(pad);
                        }
                        pld.StartDateString = planDetail.StartTime.ToString("dd/MM/yyyy");
                        pld.EndTimeString = planDetail.EndTime.ToString("dd/MM/yyyy");
                    }
                    break;
                //Parse từ ProfessorCriterion chưa có plankpidetail + addnew plankpidetail
                case 2:
                    {
                        pld.FromProfessorCriterionId = crd.Id;
                        pld.TargetDetail = crd.Name;
                        pld.CriterionTypeId = crd.CriterionType.Id;
                        pld.StartTime = plan.StartTime;
                        pld.EndTime = plan.EndTime;

                        PlanKPIDetail pdn = new PlanKPIDetail();
                        pdn.Id = Guid.NewGuid();
                        pdn.TargetDetail = crd.Name;
                        pdn.CreateTime = DateTime.Now;
                        pdn.StartTime = plan.StartTime;
                        pdn.EndTime = plan.EndTime;
                        pdn.FromProfessorCriterion = new ProfessorCriterion() { Id = crd.Id };
                        pdn.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                        pdn.PlanStaff = planStaff;
                        session.Save(pdn);
                        pld.Id = pdn.Id;

                    }
                    break;
            }
            return pld;
        }
        #region ParsePlanDetail


        //private PlanKPIMakingDetailDTO ParsePlanDetail(Criterion cri, PlanKPIDetail pld, int type, NHibernate.ISession session)
        //{
        //    PlanKPIMakingDetailDTO pd = new PlanKPIMakingDetailDTO();
        //    switch (type)
        //    {
        //        case 1:
        //            {
        //                pd.FromCriterion = new CriterionDTO();
        //                pd.FromCriterion.Id = cri.Id;
        //                pd.FromCriterion.Name = cri.Name;
        //                pd.FromCriterion.CriterionType = pd.FromCriterion.CriterionType != null ? new CriterionType() { Id = cri.CriterionType.Id } : null;
        //                pd.FromCriterion.CriterionTypeId = cri.CriterionType.Id;
        //                pd.FromCriterion.Department = pd.FromCriterion.Department != null ? new DepartmentDTO() { Id = cri.Department.Id } : null;
        //                pd.FromCriterion.DepartmentId = cri.Department != null ? cri.Department.Id : Guid.Empty;
        //                pd.FromCriterion.TargetGroupDetail = pd.FromCriterion.TargetGroupDetail != null ? new TargetGroupDetail() { Id = cri.TargetGroupDetail.Id } : null;
        //                pd.FromCriterion.TargetGroupDetailId = cri.TargetGroupDetail.Id;
        //                pd.FromCriterionId = pd.FromCriterion.Id;
        //                pd.LeadDepartment = cri.FromPlanKPIDetail.LeadDepartment != null ? cri.FromPlanKPIDetail.LeadDepartment.Map<DepartmentDTO>() : null;
        //                pd.StaffLeader = cri.FromPlanKPIDetail.StaffLeader != null ? cri.FromPlanKPIDetail.StaffLeader.Map<StaffDTO>() : null;
        //                foreach (Department sub in cri.FromPlanKPIDetail.SubDepartments)
        //                {
        //                    pd.SubDepartmentIds.Add(sub.Id);
        //                    pd.SubDepartmentNames.Add(sub.Name);
        //                }
        //                foreach (Staff subs in cri.FromPlanKPIDetail.SubStaffs)
        //                {
        //                    pd.SubStaffIds.Add(subs.Id);
        //                    pd.SubStaffNames.Add(subs.StaffProfile.Name);
        //                }
        //                pd.PreviousKPI = cri.FromPlanKPIDetail.PreviousKPI;
        //                pd.CurrentKPI = cri.FromPlanKPIDetail.CurrentKPI;
        //                pd.CanDelete = false;
        //                pd.IsAddition = cri.FromPlanKPIDetail.IsAddition;
        //                pd.IsLockable = false;
        //                pd.TargetDetail = cri.Name;
        //                pd.IsDisable = cri.FromPlanKPIDetail != null ? cri.FromPlanKPIDetail.IsDisable : false;
        //                pd.CreateTime = DateTime.Now;
        //                pd.ManageCode = cri.FromPlanKPIDetail.ManageCode != null ? new ManageCode() { Id = cri.FromPlanKPIDetail.ManageCode.Id } : null;

        //                //Guid parentPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(cri.FromPlanKPIDetail.Id,session).Id;
        //                PlanKPIDetail parentPlanDetail = new PlanKPIDetail();
        //                PlanKPIDetail oriParentPlanDetail = new PlanKPIDetail();


        //                if (cri.FromPlanKPIDetail.ParentPlanKPIDetail != null)
        //                {
        //                    parentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == cri.FromPlanKPIDetail.ParentPlanKPIDetail.Id);
        //                }

        //                //Methods lấy từ cấp cha cao nhất
        //                oriParentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(cri.FromPlanKPIDetail.Id, session);
        //                if (oriParentPlanDetail.Methods != null)
        //                {
        //                    foreach (Method m in oriParentPlanDetail.Methods)
        //                    {
        //                        MethodDTO method = m.Map<MethodDTO>();
        //                        method.StartTimeString = m.StartTime.ToString("dd/MM/yyyy");
        //                        method.EndTimeString = m.EndTime.ToString("dd/MM/yyyy");
        //                        pd.Methods.Add(method);
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (Method m in cri.FromPlanKPIDetail.Methods)
        //                    {
        //                        MethodDTO method = m.Map<MethodDTO>();
        //                        method.StartTimeString = m.StartTime.ToString("dd/MM/yyyy");
        //                        method.EndTimeString = m.EndTime.ToString("dd/MM/yyyy");
        //                        pd.Methods.Add(method);
        //                    }
        //                }
        //                /// KPIs
        //                if (parentPlanDetail.Id != Guid.Empty && parentPlanDetail.PlanKPIDetail_KPIs != null)
        //                {
        //                    foreach (PlanKPIDetail_KPI m in parentPlanDetail.PlanKPIDetail_KPIs)
        //                    {
        //                        PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                        kpi.Id = m.Id;
        //                        kpi.Name = m.Name;
        //                        kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                        kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                        pd.PlanKPIDetail_KPIs.Add(kpi);
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (PlanKPIDetail_KPI m in cri.FromPlanKPIDetail.PlanKPIDetail_KPIs)
        //                    {
        //                        PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                        kpi.Id = m.Id;
        //                        kpi.Name = m.Name;
        //                        kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                        kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                        pd.PlanKPIDetail_KPIs.Add(kpi);
        //                    }
        //                }

        //            }
        //            break;
        //        case 2:
        //            {
        //                pd.Id = pld.Id;
        //                pd.TargetDetail = pld.TargetDetail;
        //                //pd.ExcecuteMethod = pld.ExcecuteMethod;
        //                pd.BasicResource = pld.BasicResource;
        //                pd.StaffLeader = pld.StaffLeader != null ? pld.StaffLeader.Map<StaffDTO>() : null;
        //                pd.LeadDepartment = pld.LeadDepartment != null ? pld.LeadDepartment.Map<DepartmentDTO>() : null;
        //                pd.PreviousKPI = pld.PreviousKPI;
        //                pd.CurrentKPI = pld.CurrentKPI;
        //                pd.MaxRecord = pld.MaxRecord;
        //                pd.TargetGroupDetail = new TargetGroupDetailDTO() { Id = pld.TargetGroupDetail.Id };
        //                pd.CreateTime = pld.CreateTime;
        //                pd.StartTime = pld.StartTime;
        //                pd.EndTime = pld.EndTime;
        //                foreach (Staff subs in pld.SubStaffs)
        //                {
        //                    pd.SubStaffIds.Add(subs.Id);
        //                    pd.SubStaffNames.Add(subs.StaffProfile.Name);
        //                }
        //                foreach (Department subd in pld.SubDepartments)
        //                {
        //                    pd.SubDepartmentIds.Add(subd.Id);
        //                    pd.SubjectNames.Add(subd.Name);
        //                }
        //                if (pld.FromCriterion != null)
        //                {
        //                    pd.FromCriterion = new CriterionDTO();
        //                    pd.FromCriterion.Id = pld.FromCriterion.Id;
        //                    pd.FromCriterion.Name = pld.FromCriterion.Name;
        //                    pd.FromCriterion.CriterionType = pd.FromCriterion.CriterionType != null ? new CriterionType() { Id = pld.FromCriterion.CriterionType.Id } : null;
        //                    pd.FromCriterion.CriterionTypeId = pld.FromCriterion.CriterionType.Id;
        //                    pd.FromCriterion.Department = pd.FromCriterion.Department != null ? new DepartmentDTO() { Id = pld.FromCriterion.Id } : null;
        //                    pd.FromCriterion.DepartmentId = pld.FromCriterion.Department != null ? pld.FromCriterion.Department.Id : Guid.Empty;
        //                    pd.FromCriterion.TargetGroupDetail = pd.FromCriterion.TargetGroupDetail != null ? new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id } : null;
        //                    pd.FromCriterion.TargetGroupDetailId = pld.TargetGroupDetail.Id;
        //                    pd.FromCriterionId = pd.FromCriterion.Id;
        //                    pd.IsLockable = false;
        //                    foreach (Department subd in pld.FromCriterion.FromPlanKPIDetail.SubDepartments)
        //                    {
        //                        //pd.SubDepartmentIds.Add(subd.Id);
        //                        pd.SubDepartmentNames.Add(subd.Name);
        //                    }
        //                }
        //                else
        //                {
        //                    pd.FromCriterion = null;
        //                    pd.IsLockable = true;

        //                    //foreach (Staff subs in pld.SubStaffs)
        //                    //{
        //                    //    pd.SubStaffIds.Add(subs.Id);
        //                    //    pd.SubStaffNames.Add(subs.StaffProfile.Name);
        //                    //}
        //                }

        //                //Guid parentPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id,session).Id;
        //                PlanKPIDetail parentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld.Id, session);

        //                //Lấy thông tin từ kế hoạch cấp cao nhất

        //                //parentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == parentPlanDetailId);
        //                if (parentPlanDetail != null)
        //                    pd.CreateTime = parentPlanDetail.CreateTime;
        //                foreach (Method m in parentPlanDetail != null ? parentPlanDetail.Methods : pld.Methods)
        //                {
        //                    MethodDTO method = m.Map<MethodDTO>();
        //                    method.StartTimeString = m.StartTime.ToString("dd/MM/yyyy");
        //                    method.EndTimeString = m.EndTime.ToString("dd/MM/yyyy");
        //                    pd.Methods.Add(method);
        //                }
        //                pd.Methods = pd.Methods.OrderBy(m => m.StartTime).ToList();
        //                foreach (FileAttachment d in (parentPlanDetail != null && parentPlanDetail.FileAttachments.Count() > 0) ? parentPlanDetail.FileAttachments : pld.FileAttachments)
        //                {
        //                    FileAttachmentDTO dFile = d.Map<FileAttachmentDTO>();

        //                    pd.PlanFileDTOs.Add(dFile);
        //                }

        //                foreach (PlanKPIDetail_KPI m in pld.PlanKPIDetail_KPIs != null ? pld.PlanKPIDetail_KPIs : parentPlanDetail.PlanKPIDetail_KPIs)
        //                {
        //                    PlanKPIDetail_KPIDTO kpi = new PlanKPIDetail_KPIDTO();
        //                    kpi.Id = m.Id;
        //                    kpi.Name = m.Name;
        //                    kpi.MeasureUnitId = m.MeasureUnit != null ? m.MeasureUnit.Id : 0;
        //                    kpi.MeasureUnitName = m.MeasureUnit != null ? m.MeasureUnit.Name : "";
        //                    pd.PlanKPIDetail_KPIs.Add(kpi);
        //                }
        //                pd.CanDelete = pld.Criterions.Count > 0 ? false : true;
        //                //pd.CanDelete = false;
        //                pd.IsAddition = pld.IsAddition;
        //                pd.IsDisable = pld.IsDisable;
        //                pd.ManageId = pld.ManageCode != null ? pld.ManageCode.Id : string.Empty;
        //                pd.ManageCode = pld.ManageCode != null ? new ManageCode() { Id = pld.ManageCode.Id } : null;


        //                break;
        //            }

        //    }
        //    return pd;
        //}
        #endregion

        public static Criterion ParseNewCriterion(string name, Department department, Staff staff, PlanKPIDetail fromPlanKPIDetail, PlanKPI plan, TargetGroupDetail targetGroupDetail, int criterionType, double maxRecord)
        {
            Criterion criterion = new Criterion();
            criterion.Id = Guid.NewGuid();
            criterion.Department = department != null ? new Department() { Id = department.Id } : null;
            criterion.TargetGroupDetail = targetGroupDetail != null ? new TargetGroupDetail() { Id = targetGroupDetail.Id } : null;
            criterion.PlanKPI = new PlanKPI() { Id = plan.Id };
            criterion.Name = name;
            criterion.Staff = staff != null ? new Staff() { Id = staff.Id } : null;
            criterion.FromPlanKPIDetail = fromPlanKPIDetail != null ? new PlanKPIDetail() { Id = fromPlanKPIDetail.Id } : null;
            criterion.MaxRecord = maxRecord;
            criterion.CriterionType = new CriterionType() { Id = criterionType };

            return criterion;
        }

        public static Criterion ParseCriterion(TargetGroupPlanMakingDTO tg, PlanDetailMakingDTO obj, PlanKPIDetail pdn, PlanKPIMakingDetailDTO p, Guid did, int type, bool isGetStaffLeader)
        {
            Criterion criterion = new Criterion();
            criterion.TargetGroupDetail = new TargetGroupDetail() { Id = tg.TargetGroupId };
            criterion.PlanKPI = new PlanKPI() { Id = obj.PlanId };
            criterion.Id = Guid.NewGuid();
            criterion.MaxRecord = p.MaxRecord;
            criterion.CriterionType = new CriterionType() { Id = 1 };
            criterion.FromPlanKPIDetail = new PlanKPIDetail();
            criterion.FromPlanKPIDetail.Id = p != null ? p.Id : Guid.Empty;
            criterion.StaffLeader = isGetStaffLeader ? (p.StaffLeader != null ? new Staff() { Id = p.StaffLeader.Id } : null) : null;

            switch (type)
            {
                //Tạo criterion người chỉ đạo
                case 1:
                    {
                        criterion.Name = pdn.TargetDetail;
                        //criterion.StaffLeader = new Staff() { Id = p.StaffLeaderId };
                        criterion.Staff = new Staff() { Id = did };
                    }
                    break;

                //Tạo criterion cho đơn vị phối hợp từ plandetail mới
                case 2:
                    {
                        criterion.Department = new Department() { Id = did };
                        criterion.Name = p.TargetDetail;
                        criterion.FromPlanKPIDetail = new PlanKPIDetail();
                        criterion.FromPlanKPIDetail.Id = p.Id;
                        criterion.FromPlanKPIDetail.PreviousKPI = p.PreviousKPI;
                        //criterion.FromPlanKPIDetail.CurrentKPI = p.CurrentKPI;
                        //criterion.FromPlanKPIDetail.MeasureUnit = p.MeasureUnitDTO != null?new MeasureUnit(){Id=p.MeasureUnitDTO.Id}:null;
                    }
                    break;
                //Tạo criterion cho người phối hợp từ plandetail mới
                case 3:
                    {
                        criterion.Staff = new Staff() { Id = did };
                        criterion.Name = p.TargetDetail;
                        criterion.FromPlanKPIDetail = new PlanKPIDetail();
                        criterion.FromPlanKPIDetail.Id = p.Id;
                        criterion.FromPlanKPIDetail.PreviousKPI = p.PreviousKPI;
                        criterion.FromPlanKPIDetail.CurrentKPI = p.CurrentKPI;
                        //criterion.FromPlanKPIDetail.MeasureUnit = p.MeasureUnitDTO != null ? new MeasureUnit() { Id = p.MeasureUnitDTO.Id } : null;
                    }
                    break;
                //update criterion cho đơn vị phối hợp
                case 4:
                    {
                        criterion.Department = new Department() { Id = did };
                        criterion.Name = p.TargetDetail;
                        criterion.FromPlanKPIDetail = new PlanKPIDetail();
                        criterion.FromPlanKPIDetail.Id = p.Id;
                        criterion.FromPlanKPIDetail.PreviousKPI = p.PreviousKPI;
                        //criterion.FromPlanKPIDetail.CurrentKPI = p.CurrentKPI;
                        //criterion.FromPlanKPIDetail.MeasureUnit = p.MeasureUnitDTO != null ? new MeasureUnit() { Id = p.MeasureUnitDTO.Id } : null;
                    }
                    break;
                //update criterion cho người phối hợp
                case 5:
                    {
                        criterion.Staff = new Staff() { Id = did };
                        criterion.Name = p.TargetDetail;
                        criterion.FromPlanKPIDetail = new PlanKPIDetail();
                        criterion.FromPlanKPIDetail.Id = p.Id;
                        criterion.FromPlanKPIDetail.PreviousKPI = p.PreviousKPI;
                        criterion.FromPlanKPIDetail.CurrentKPI = p.CurrentKPI;
                        //criterion.FromPlanKPIDetail.MeasureUnit = p.MeasureUnitDTO != null ? new MeasureUnit() { Id = p.MeasureUnitDTO.Id } : null;
                    }
                    break;
            }
            return criterion;
        }

        public static PlanKPIDetail CopyPlanKPIDetail(PlanKPIDetail pld)
        {
            PlanKPIDetail result = new PlanKPIDetail();

            result = new PlanKPIDetail();
            result.Id = Guid.NewGuid();
            result.LeadDepartment = pld.LeadDepartment != null ? new Department() { Id = pld.LeadDepartment.Id } : null;
            result.ManageCode = pld.ManageCode;
            result.StaffLeader = null;
            result.TargetGroupDetail = pld.TargetGroupDetail != null ? new TargetGroupDetail() { Id = pld.TargetGroupDetail.Id } : null;
            result.BasicResource = result.BasicResource;
            result.CreateTime = DateTime.Now;
            result.StartTime = DateTime.Now;
            result.EndTime = DateTime.Now;
            result.TargetDetail = pld.TargetDetail;
            result.PreviousKPI = pld.PreviousKPI;
            result.CurrentKPI = pld.CurrentKPI;
            return result;
        }

        public static ConcurrentDictionary<Guid, PlanKPIDetail> GetPlanDetailDic(NHibernate.ISession session)
        {
            ObjectCache cache = MemoryCache.Default;

            var result = cache.Get("PlanDetailDic") as ConcurrentDictionary<Guid, PlanKPIDetail>;
            if (result != null)
                return result;

            //SessionManager.DoWork(session =>
            //{
            result = new ConcurrentDictionary<Guid, PlanKPIDetail>(session.Query<PlanKPIDetail>().ToDictionary(p => p.Id, p => p));
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) };
            cache.Add("PlanDetailDic", result, policy);
            //});

            return result;
        }

        public static ConcurrentDictionary<Guid, PlanKPIDetail> UpdatePlanDetailDic(NHibernate.ISession session)
        {
            ObjectCache cache = MemoryCache.Default;


            ConcurrentDictionary<Guid, PlanKPIDetail> result = new ConcurrentDictionary<Guid, PlanKPIDetail>();
            //if (result != null)
            //    return result;

            //SessionManager.DoWork(session =>
            //{
            result = new ConcurrentDictionary<Guid, PlanKPIDetail>(session.Query<PlanKPIDetail>().ToDictionary(p => p.Id, p => p));
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) };
            cache.Add("PlanDetailDic", result, policy);
            //});

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planDetail"></param>
        /// <param name="actionType">1 save or update, 2 delete</param>
        /// <param name="session"></param>
        public static void UpdatePlanDetailDic(PlanKPIDetail planDetail, int actionType, NHibernate.ISession session)
        {
            ObjectCache cache = MemoryCache.Default;
            ConcurrentDictionary<Guid, PlanKPIDetail> result = new ConcurrentDictionary<Guid, PlanKPIDetail>();
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) };
            result = cache.Get("PlanDetailDic") as ConcurrentDictionary<Guid, PlanKPIDetail>;
            if (result == null)
            {
                //SessionManager.DoWork(session =>
                //{
                result = new ConcurrentDictionary<Guid, PlanKPIDetail>(session.Query<PlanKPIDetail>().ToDictionary(p => p.Id, p => p));
                cache.Add("PlanDetailDic", result, policy);
                //});
            }
            else
            {
                switch (actionType)
                {
                    case 1:
                        result.AddOrUpdate(planDetail.Id, planDetail, (key, oldvalue) => planDetail);
                        break;
                    case 2:
                        result.TryRemove(planDetail.Id, out planDetail);
                        break;
                }

                cache.Add("PlanDetailDic", result, policy);
            }

        }


    }
}
