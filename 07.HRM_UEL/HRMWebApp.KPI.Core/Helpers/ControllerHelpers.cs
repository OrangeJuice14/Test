using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.Core.DTO.PlanMakingDTOs;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Security;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;

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
                if (cri.FromPlanKPIDetail.PlanStaff != null && cri.FromPlanKPIDetail.PlanStaff.Staff != null)
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

        //public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId, NHibernate.ISession session)
        //{
        //    PlanKPIDetail pld = null;

        //    //pld = GetPlanDetailDic(session)[pldId];
        //    if (GetPlanDetailDic(session).ContainsKey(pldId))
        //        pld = GetPlanDetailDic(session)[pldId];
        //    else
        //        pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
        //    if (pld != null && pld.ParentPlanKPIDetail != null)
        //    {
        //        while (pld.ParentPlanKPIDetail != null)
        //        {
        //            pld = pld.ParentPlanKPIDetail;
        //        }
        //    }
        //    return pld;
        //}

        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(Guid pldId, NHibernate.ISession session)
        {
            //if (pldId == new Guid("EBFF7DE3-306C-4A58-B63A-69E5E5FC2586"))
            //    int a = 5;
            PlanKPIDetail pld = null;

            //pld = GetPlanDetailDic(session)[pldId];
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

        public static PlanKPIDetail GetOriginalParentPlanKPIDetail(PlanKPIDetail pld, NHibernate.ISession session)
        {
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
            //if (GetPlanDetailDic(session).ContainsKey(pldId))
            //    pld = GetPlanDetailDic(session)[pldId];
            //else
                pld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);

            if (pld != null)
            {
                result = session.Query<PlanKPIDetail>().Where(p => GetOriginalParentPlanKPIDetail(p.Id, session).Id == pldId).Select(p => p).ToList();

                result = result.Where(p => p.Id != pldId).ToList();
            }
            return result;
        }
        public static PlanKPIDetail GetPlanDetail(Guid Id, NHibernate.ISession session)
        {
            PlanKPIDetail result = new PlanKPIDetail();
            //if (GetPlanDetailDic(session).ContainsKey(Id))
            //    result = GetPlanDetailDic(session)[Id];
            //else
                result = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == Id);
            return result;
        }
        public static List<PlanKPIDetail> GetListPlanDetailByCriterion(Guid Id, NHibernate.ISession session)
        {
            List<PlanKPIDetail> result = new List<PlanKPIDetail>();
            result =session.Query<PlanKPIDetail>().Where(p => p.FromCriterion != null && p.FromCriterion.Id == Id).Select(p => p).ToList();
            return result;
        }
        public static List<PlanKPIDetail> GetListDeletedPlanDetailByCriterion(Guid Id, NHibernate.ISession session)
        {
            List<PlanKPIDetail> result = new List<PlanKPIDetail>();
            result = session.Query<PlanKPIDetail>().Where(p => p.FromCriterion != null && p.FromCriterion.Id == Id && p.IsDelete == true).Select(p => p).ToList();
            return result;
        }
        public static List<PlanKPIDetail> GetAllChildStaffPlanKPIDetail(Guid pldId, NHibernate.ISession session)
        {
            List<PlanKPIDetail> result = new List<PlanKPIDetail>();
            //Cri cấp 1 lấy từ plan truyền vào
            List<Guid> listCri1 = session.Query<Criterion>().Where(c => c.FromPlanKPIDetail.Id == pldId).Select(c => c.Id).ToList();
            foreach (Guid cri1 in listCri1)
            {
                //Plan cấp 1 lấy từ cri cấp 1
                List<PlanKPIDetail> listPlan1 = GetListPlanDetailByCriterion(cri1, session);
                result = result.Concat(listPlan1).ToList();
                foreach (PlanKPIDetail pl1 in listPlan1)
                {
                    //Cri cấp 2 lấy từ plan cấp 1
                    List<Guid> listCri2 = session.Query<Criterion>().Where(c => c.FromPlanKPIDetail.Id == pl1.Id).Select(c => c.Id).ToList();
                    foreach (Guid cri2 in listCri2)
                    {
                        //Plan cấp 2 lấy từ cri cấp 2
                        List<PlanKPIDetail> listPlan2 = GetListPlanDetailByCriterion(cri2, session);
                        result = result.Concat(listPlan2).ToList();
                        foreach (PlanKPIDetail pl2 in listPlan2)
                        {
                            //Cri cấp 3 lấy từ plan cấp 2
                            List<Guid> listCri3 = session.Query<Criterion>().Where(c => c.FromPlanKPIDetail.Id == pl2.Id).Select(c => c.Id).ToList();
                            foreach (Guid cri3 in listCri3)
                            {
                                //Plan cấp 3 lấy từ cri cấp 3
                                List<PlanKPIDetail> listPlan3 = GetListPlanDetailByCriterion(cri3, session);
                                result = result.Concat(listPlan3).ToList();
                                foreach (PlanKPIDetail pl3 in listPlan3)
                                {
                                    //Cri cấp 4 lấy từ plan cấp 3
                                    List<Guid> listCri4 = session.Query<Criterion>().Where(c => c.FromPlanKPIDetail.Id == pl3.Id).Select(c => c.Id).ToList();
                                    foreach (Guid cri4 in listCri4)
                                    {
                                        //Plan cấp 4 lấy từ cri cấp 4
                                        List<PlanKPIDetail> listPlan4 = GetListPlanDetailByCriterion(cri4, session);
                                        result = result.Concat(listPlan4).ToList();
                                        foreach (PlanKPIDetail pl4 in listPlan4)
                                        {
                                            //Cri cấp 5 lấy từ plan cấp 4
                                            List<Guid> listCri5 = session.Query<Criterion>().Where(c => c.FromPlanKPIDetail.Id == pl4.Id).Select(c => c.Id).ToList();
                                            foreach (Guid cri5 in listCri5)
                                            {
                                                //Plan cấp 5 lấy từ cri cấp 5
                                                List<PlanKPIDetail> listPlan5 = GetListPlanDetailByCriterion(cri5, session);
                                                result = result.Concat(listPlan5).ToList();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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
            //if (GetPlanDetailDic(session).ContainsKey(pldId))
            //    planpld = GetPlanDetailDic(session)[pldId];
            //else
                planpld = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId);
            if (planpld != null && planpld.ParentPlanKPIDetail != null)
            {
                parentPlan = session.Query<PlanKPIDetail>().SingleOrDefault(p=>p.Id== planpld.ParentPlanKPIDetail.Id);
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

        public static PlanKPIDetailDTO GetParentPlanKPIDetail(PlanKPIDetail pld, NHibernate.ISession session)
        {
            PlanKPIDetailDTO parentPlanDTO = new PlanKPIDetailDTO();

            PlanKPIDetail parentPlan = null;
            PlanKPIDetail planpld = new PlanKPIDetail();
            //if (GetPlanDetailDic(session).ContainsKey(pldId))
            //    planpld = GetPlanDetailDic(session)[pldId];
            //else
            planpld = pld;
            if (planpld != null && planpld.ParentPlanKPIDetail != null)
            {
                parentPlan = planpld.ParentPlanKPIDetail;
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
            //if (GetPlanDetailDic(session).ContainsKey(pldId))
            //    pld = GetPlanDetailDic(session)[pldId];
            //else
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
                //if (GetPlanDetailDic(session).ContainsKey(pldId))
                //    result = GetPlanDetailDic(session)[pldId].MaxRecord;
                //else
                    result = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == pldId).MaxRecord;
            }

            return result;
        }

        public static List<MethodDTO> GetOriginalMethods(Guid pldId, NHibernate.ISession session)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = new PlanKPIDetail();

            pld = originalPld;
            if (pld != null)
            {
                foreach (Method me in pld.Methods)
                {
                    result.Add(me.Map<MethodDTO>());
                }
            }

            return result;
        }

        public static List<MethodDTO> GetOriginalMethods(PlanKPIDetail pldId, NHibernate.ISession session)
        {
            List<MethodDTO> result = new List<MethodDTO>();
            PlanKPIDetail originalPld = GetOriginalParentPlanKPIDetail(pldId, session);

            PlanKPIDetail pld = new PlanKPIDetail();

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
                //if (currentUser.IsKPIs)
                //    result.StaffInfo.Position.AgentObjectType = new AgentObjectType() { Id = Convert.ToInt32(currentUser.AgentObjectTypeId) };
                //SessionHelper.Data(SessionKey.IsKPIs, true);
                //else
                //    SessionHelper.Data(SessionKey.IsKPIs, false);
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
                //SessionHelper.Data(SessionKey.IsKPIs, true);
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

            pdn.OrderNumber = originalPlan.OrderNumber; //Thêm số thứ tự
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
                        pd.OrderNumber = cri.OrderNumber; //Thêm số thứ tự
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
                        pd.IsLocked = false;
                        pd.IsOriginal = false;
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
                        pd.TargetDetail = (cri.Name != null && cri.Name != "") ? cri.Name : cri.FromPlanKPIDetail.TargetDetail;
                        pd.PreviousKPI = cri.FromPlanKPIDetail.PreviousKPI;
                        pd.CurrentKPI = cri.FromPlanKPIDetail.CurrentKPI;
                        pd.BasicResource = cri.FromPlanKPIDetail.BasicResource;
                        pd.CanDelete = false;
                        pd.IsAddition = cri.FromPlanKPIDetail.IsAddition;
                        pd.IsLockable = false;
                        pd.IsDisable = cri.FromPlanKPIDetail != null ? cri.FromPlanKPIDetail.IsDisable : false;
                        pd.CreateTime = DateTime.Now;
                        pd.ManageCode = cri.FromPlanKPIDetail.ManageCode != null ? new ManageCode() { Id = cri.FromPlanKPIDetail.ManageCode.Id } : null;

                        Guid parentPlanDetailId = ControllerHelpers.GetOriginalParentPlanKPIDetail(cri.FromPlanKPIDetail, session).Id;
                        PlanKPIDetail parentPlanDetail = new PlanKPIDetail();
                        PlanKPIDetail oriParentPlanDetail = new PlanKPIDetail();

                        //SessionManager.DoWork(session =>
                        //{
                        if (cri.FromPlanKPIDetail.ParentPlanKPIDetail != null)
                        {
                            //parentPlanDetail= session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id ==new Guid("625d2ced-24cb-4c7c-bd56-3b8c103b0a93")); 
                            //parentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == cri.FromPlanKPIDetail.ParentPlanKPIDetail.Id);

                            parentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(cri.FromPlanKPIDetail, session);
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
                                kpi.OrderNumber = m.OrderNumber; //Thêm số thứ tự
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
                                kpi.OrderNumber = m.OrderNumber; //Thêm số thứ tự
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
                        pd.OrderNumber = pld.OrderNumber; //Thêm số thứ tự
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
                            pd.CriterionTypeId = pld.FromProfessorCriterion.CriterionType.Id;
                        }
                        pd.ActivityIds = new List<Guid>();
                        foreach (ProfessorOtherActivity pa in pld.ProfessorOtherActivities)
                        {
                            //pd.ProfessorOtherActivities.Add(new ProfessorOtherActivity() { Id = did });                         
                            if (pa.CriterionDictionary != null)
                                pd.ActivityIds.Add(pa.CriterionDictionary.Id);
                            //pd.ActivityNames.Add(pa.CriterionDictionary.Name);
                        }
                        foreach (ScienceResearch pa in pld.ScienceResearches)
                        {
                            //pd.ProfessorOtherActivities.Add(new ProfessorOtherActivity() { Id = did });   
                            if (pa.CriterionDictionary != null)                      
                                pd.ScienceResearchIds.Add(pa.CriterionDictionary.Id);
                            //pd.ActivityNames.Add(pa.CriterionDictionary.Name);
                        }
                        PlanKPIDetail parentPlanDetail = ControllerHelpers.GetOriginalParentPlanKPIDetail(pld, session) != null ? ControllerHelpers.GetOriginalParentPlanKPIDetail(pld, session) : null; ;
                        Guid parentPlanDetailId = parentPlanDetail != null ? parentPlanDetail.Id : Guid.Empty;
                        

                        //Lấy thông tin từ kế hoạch cấp cao nhất
                        //SessionManager.DoWork(session =>
                        //{

                        //parentPlanDetail = session.Query<PlanKPIDetail>().SingleOrDefault(p => p.Id == parentPlanDetailId);

                        if (parentPlanDetailId == Guid.Empty || parentPlanDetailId == pld.Id)
                        {
                            pd.IsOriginal = true;
                        }

                        if (parentPlanDetail != null)
                        {
                            //Kiểm tra nếu là plandetail của BGH mới gắn BGH chỉ đạo
                            if (parentPlanDetail.PlanStaff.WebGroupId != Guid.Empty)
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
                                kpi.OrderNumber = m.OrderNumber; //Thêm số thứ tự
                                pd.PlanKPIDetail_KPIs.Add(kpi);
                            }
                        }
                        pd.CanDelete = pld.Criterions.Count > 0 ? false : true;
                        //pd.CanDelete = false;
                        pd.IsAddition = pld.IsAddition;
                        pd.IsDisable = pld.IsDisable;
                        pd.IsLocked = pld.IsLocked;
                        if (cri != null)
                        {
                            pd.IsLocked = false;
                            pd.IsOriginal = false;
                        }
                        pd.ManageId = pld.ManageCode != null ? pld.ManageCode.Id : string.Empty;
                        pd.ManageCode = pld.ManageCode != null ? new ManageCode() { Id = pld.ManageCode.Id, Name = pld.ManageCode.Name } : null;
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
        public static bool CheckParentPlanDetailTime(DateTime planStart, DateTime planEnd, DateTime parentPlanStart, DateTime parentPlanEnd)
        {
            if (parentPlanStart > planEnd)
                return false;
            if (parentPlanEnd < planStart)
                return false;
            return true;
        }
        public static bool CheckParentPlanDetail(PlanKPIDetail planDetail, PlanKPIDetail parentPlanDetail, int TargetGroupDetailTypeId)
        {
            switch (TargetGroupDetailTypeId)
            {
                case 0:
                    {
                        if (planDetail.Name == null && planDetail.BasicResource == null && planDetail.ExecuteMethod == null)
                        {
                            if (parentPlanDetail.Name != null || parentPlanDetail.BasicResource != null || parentPlanDetail.ExecuteMethod != null)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case 5:
                    {
                        if (planDetail.ScienceResearches.Count == 0 && planDetail.BasicResource == null && planDetail.ExecuteMethod == null && planDetail.PreviousKPI == null)
                        {
                            if (parentPlanDetail.ScienceResearches.Count > 0 || parentPlanDetail.BasicResource != null || parentPlanDetail.ExecuteMethod != null || parentPlanDetail.PreviousKPI != null)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case 4:
                    {
                        if (planDetail.ProfessorOtherActivities.Count == 0 && planDetail.BasicResource == null && planDetail.ExecuteMethod == null && planDetail.PreviousKPI == null)
                        {
                            if (parentPlanDetail.ProfessorOtherActivities.Count > 0 || parentPlanDetail.BasicResource != null || parentPlanDetail.ExecuteMethod != null || parentPlanDetail.PreviousKPI != null)
                            {
                                return true;
                            }
                        }
                    }
                    break;
            }

            return false;
        }
        public static PlanKPIMakingDetailDTO ParsePlanDetailFromParent(ProfessorCriterionPlanDTO crd, PlanKPIDetail planDetail, PlanKPIDetail parentPlanDetail, PlanKPI plan, TargetGroupDetail t, PlanStaff planStaff, NHibernate.ISession session, int type)
        {
            PlanKPIMakingDetailDTO pld = new PlanKPIMakingDetailDTO();
            switch (type)
            {
                case 1:
                    {
                        pld.Id = planDetail.Id;
                        pld.FromProfessorCriterionId = crd.Id;
                        pld.TargetDetail = crd.Name;
                        pld.CriterionTypeId = crd.CriterionType.Id;
                        pld.StartTime = plan.StartTime;
                        pld.EndTime = plan.EndTime;
                        pld.StartDateString = plan.StartTime.ToString("dd/MM/yyyy");
                        pld.EndTimeString = plan.EndTime.ToString("dd/MM/yyyy");
                        pld.Name = parentPlanDetail.Name;
                        pld.BasicResource = parentPlanDetail.BasicResource;
                        pld.ExecuteMethod = parentPlanDetail.ExecuteMethod;
                        pld.PreviousKPI = parentPlanDetail.PreviousKPI;
                        pld.CurrentKPI = parentPlanDetail.CurrentKPI;
                        pld.OrderNumber = crd.OrderNumber;

                        pld.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                        foreach (ProfessorOtherActivity pa in parentPlanDetail.ProfessorOtherActivities)
                        {
                            ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfTime = pa.NumberOfTime;
                            pad.NumberOfHour = pa.CriterionDictionary != null ? pa.CriterionDictionary.NumberOfHour : pa.NumberOfHour;
                            //pad.OrderNumber = pa.OrderNumber;
                            pad.OrderNumber = GetMaxOrderNumberActivity(planDetail.Id, session) + 1;
                            pld.ProfessorOtherActivities.Add(pad);
                        }
                        pld.ProfessorOtherActivities = pld.ProfessorOtherActivities.OrderBy(p => p.OrderNumber).ToList();

                        pld.ScienceResearches = new List<ScienceResearchDTO>();
                        foreach (ScienceResearch pa in parentPlanDetail.ScienceResearches)
                        {
                            ScienceResearchDTO pad = new ScienceResearchDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfResearch = pa.NumberOfResearch;
                            //pad.OrderNumber = pa.OrderNumber;
                            pad.OrderNumber = GetMaxOrderNumberResearch(planDetail.Id, session) + 1;
                            pld.ScienceResearches.Add(pad);
                        }
                        pld.ScienceResearches = pld.ScienceResearches.OrderBy(p => p.OrderNumber).ToList();

                        planDetail.TargetDetail = crd.Name;
                        planDetail.Name = parentPlanDetail.Name;
                        planDetail.BasicResource = parentPlanDetail.BasicResource;
                        planDetail.ExecuteMethod = parentPlanDetail.ExecuteMethod;
                        planDetail.PreviousKPI = parentPlanDetail.PreviousKPI;
                        planDetail.CurrentKPI = parentPlanDetail.CurrentKPI;
                        planDetail.CreateTime = DateTime.Now;
                        //planDetail.StartTime = plan.StartTime;
                        //planDetail.EndTime = plan.EndTime;
                        //planDetail.FromProfessorCriterion = new ProfessorCriterion() { Id = crd.Id };
                        //planDetail.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                        //planDetail.PlanStaff = planStaff;

                        foreach (ProfessorOtherActivity poa in parentPlanDetail.ProfessorOtherActivities)
                        {
                            ProfessorOtherActivity professor = new ProfessorOtherActivity();
                            professor.Id = Guid.NewGuid();
                            professor.NumberOfTime = poa.NumberOfTime;
                            professor.CriterionDictionary = poa.CriterionDictionary;
                            professor.Name = poa.Name;
                            professor.NumberOfHour = poa.NumberOfHour;
                            //professor.OrderNumber = poa.OrderNumber;
                            professor.OrderNumber = GetMaxOrderNumberActivity(planDetail.Id, session) + 1;
                            planDetail.ProfessorOtherActivities.Add(professor);
                        }
                        foreach (ScienceResearch sr in parentPlanDetail.ScienceResearches)
                        {
                            ScienceResearch science = new ScienceResearch();
                            science.Id = Guid.NewGuid();
                            science.Name = sr.Name;
                            science.NumberOfResearch = sr.NumberOfResearch;
                            science.CriterionDictionary = sr.CriterionDictionary;
                            //science.OrderNumber = sr.OrderNumber;
                            science.OrderNumber = GetMaxOrderNumberResearch(planDetail.Id, session) + 1;
                            planDetail.ScienceResearches.Add(science);
                        }

                        session.Update(planDetail);
                    }
                    break;
                case 2:
                    {
                        //Lấy danh sách của kế hoạch năm được thêm vào sau (không có trong kế hoạch học kỳ)
                        List<ProfessorOtherActivity> activityList = new List<ProfessorOtherActivity>();
                        List<ScienceResearch> researchList = new List<ScienceResearch>();

                        if (crd.CriterionType.Id == 5)
                        {
                            activityList = parentPlanDetail.ProfessorOtherActivities.Where(p => !planDetail.ProfessorOtherActivities.Any(pp => p.Name == pp.Name)).ToList();
                            researchList = parentPlanDetail.ScienceResearches.Where(p => !planDetail.ScienceResearches.Any(pp => p.Name == pp.Name)).ToList();
                        }
                        else
                        {
                            activityList = parentPlanDetail.ProfessorOtherActivities.Where(p => p.CriterionDictionary != null && !planDetail.ProfessorOtherActivities.Any(pp => pp.CriterionDictionary != null && p.CriterionDictionary.Id == pp.CriterionDictionary.Id)).ToList();
                            researchList = parentPlanDetail.ScienceResearches.Where(p => p.CriterionDictionary != null && !planDetail.ScienceResearches.Any(pp => pp.CriterionDictionary != null && p.CriterionDictionary.Id == pp.CriterionDictionary.Id)).ToList();
                        }

                        pld.Id = planDetail.Id;
                        pld.FromProfessorCriterionId = crd.Id;
                        pld.TargetDetail = crd.Name;
                        pld.CriterionTypeId = crd.CriterionType.Id;
                        pld.StartTime = plan.StartTime;
                        pld.EndTime = plan.EndTime;
                        pld.StartDateString = plan.StartTime.ToString("dd/MM/yyyy");
                        pld.EndTimeString = plan.EndTime.ToString("dd/MM/yyyy");
                        pld.Name = planDetail.Name;
                        pld.BasicResource = planDetail.BasicResource;
                        pld.ExecuteMethod = planDetail.ExecuteMethod;
                        pld.PreviousKPI = planDetail.PreviousKPI;
                        pld.CurrentKPI = planDetail.CurrentKPI;
                        pld.OrderNumber = crd.OrderNumber;

                        //hiển thị lên giao diện
                        pld.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                        foreach (ProfessorOtherActivity pa in planDetail.ProfessorOtherActivities) //parse từ KH học kỳ
                        {
                            ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfTime = pa.NumberOfTime;
                            pad.NumberOfHour = pa.CriterionDictionary != null ? pa.CriterionDictionary.NumberOfHour : pa.NumberOfHour;
                            pad.OrderNumber = pa.OrderNumber;
                            pld.ProfessorOtherActivities.Add(pad);
                        }
                        foreach (ProfessorOtherActivity pa in activityList) //parse thêm KH năm
                        {
                            ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfTime = pa.NumberOfTime;
                            pad.NumberOfHour = pa.CriterionDictionary != null ? pa.CriterionDictionary.NumberOfHour : pa.NumberOfHour;
                            //pad.OrderNumber = pa.OrderNumber;
                            pad.OrderNumber = GetMaxOrderNumberActivity(planDetail.Id, session) + 1;
                            pld.ProfessorOtherActivities.Add(pad);
                        }

                        pld.ScienceResearches = new List<ScienceResearchDTO>();
                        foreach (ScienceResearch pa in planDetail.ScienceResearches)
                        {
                            ScienceResearchDTO pad = new ScienceResearchDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfResearch = pa.NumberOfResearch;
                            pad.OrderNumber = pa.OrderNumber;
                            pld.ScienceResearches.Add(pad);
                        }
                        foreach (ScienceResearch pa in researchList)
                        {
                            ScienceResearchDTO pad = new ScienceResearchDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfResearch = pa.NumberOfResearch;
                            //pad.OrderNumber = pa.OrderNumber;
                            pad.OrderNumber = GetMaxOrderNumberResearch(planDetail.Id, session) + 1;
                            pld.ScienceResearches.Add(pad);
                        }

                        //add vào kế hoạch học kỳ (nếu có)
                        foreach (ProfessorOtherActivity poa in activityList)
                        {
                            ProfessorOtherActivity professor = new ProfessorOtherActivity();
                            professor.Id = Guid.NewGuid();
                            professor.NumberOfTime = poa.NumberOfTime;
                            professor.CriterionDictionary = poa.CriterionDictionary;
                            professor.Name = poa.Name;
                            professor.NumberOfHour = poa.NumberOfHour;
                            //professor.OrderNumber = poa.OrderNumber;
                            professor.OrderNumber = GetMaxOrderNumberActivity(planDetail.Id, session) + 1;
                            planDetail.ProfessorOtherActivities.Add(professor);
                        }
                        foreach (ScienceResearch sr in researchList)
                        {
                            ScienceResearch science = new ScienceResearch();
                            science.Id = Guid.NewGuid();
                            science.Name = sr.Name;
                            science.NumberOfResearch = sr.NumberOfResearch;
                            science.CriterionDictionary = sr.CriterionDictionary;
                            //science.OrderNumber = sr.OrderNumber;
                            science.OrderNumber = GetMaxOrderNumberResearch(planDetail.Id, session) + 1;
                            planDetail.ScienceResearches.Add(science);
                        }

                        session.Update(planDetail);
                    }
                    break;
                case 3: //trường hợp học kỳ chưa soạn kế hoạch
                    {
                        pld.FromProfessorCriterionId = crd.Id;
                        pld.TargetDetail = crd.Name;
                        pld.CriterionTypeId = crd.CriterionType.Id;
                        pld.StartTime = plan.StartTime;
                        pld.EndTime = plan.EndTime;
                        pld.StartDateString = plan.StartTime.ToString("dd/MM/yyyy");
                        pld.EndTimeString = plan.EndTime.ToString("dd/MM/yyyy");
                        pld.Name = parentPlanDetail.Name;
                        pld.BasicResource = parentPlanDetail.BasicResource;
                        pld.ExecuteMethod = parentPlanDetail.ExecuteMethod;
                        pld.PreviousKPI = parentPlanDetail.PreviousKPI;
                        pld.CurrentKPI = parentPlanDetail.CurrentKPI;
                        pld.OrderNumber = crd.OrderNumber;

                        pld.ProfessorOtherActivities = new List<ProfessorOtherActivityDTO>();
                        foreach (ProfessorOtherActivity pa in parentPlanDetail.ProfessorOtherActivities)
                        {
                            ProfessorOtherActivityDTO pad = new ProfessorOtherActivityDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfTime = pa.NumberOfTime;
                            pad.NumberOfHour = pa.CriterionDictionary != null ? pa.CriterionDictionary.NumberOfHour : pa.NumberOfHour;
                            pad.OrderNumber = pa.OrderNumber;
                            pld.ProfessorOtherActivities.Add(pad);
                        }

                        pld.ScienceResearches = new List<ScienceResearchDTO>();
                        foreach (ScienceResearch pa in parentPlanDetail.ScienceResearches)
                        {
                            ScienceResearchDTO pad = new ScienceResearchDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfResearch = pa.NumberOfResearch;
                            pad.OrderNumber = pa.OrderNumber;
                            pld.ScienceResearches.Add(pad);
                        }

                        PlanKPIDetail pdn = new PlanKPIDetail();
                        pdn.Id = Guid.NewGuid();
                        pdn.TargetDetail = crd.Name;
                        pdn.Name = parentPlanDetail.Name;
                        pdn.BasicResource = parentPlanDetail.BasicResource;
                        pdn.ExecuteMethod = parentPlanDetail.ExecuteMethod;
                        pdn.PreviousKPI = parentPlanDetail.PreviousKPI;
                        pdn.CurrentKPI = parentPlanDetail.CurrentKPI;
                        pdn.CreateTime = DateTime.Now;
                        pdn.StartTime = plan.StartTime;
                        pdn.EndTime = plan.EndTime;
                        pdn.FromProfessorCriterion = new ProfessorCriterion() { Id = crd.Id };
                        pdn.TargetGroupDetail = new TargetGroupDetail() { Id = t.Id };
                        pdn.PlanStaff = planStaff;

                        pdn.ProfessorOtherActivities = new List<ProfessorOtherActivity>();
                        foreach (ProfessorOtherActivity poa in parentPlanDetail.ProfessorOtherActivities)
                        {
                            ProfessorOtherActivity professor = new ProfessorOtherActivity();
                            professor.Id = Guid.NewGuid();
                            professor.NumberOfTime = poa.NumberOfTime;
                            professor.CriterionDictionary = poa.CriterionDictionary;
                            professor.Name = poa.Name;
                            professor.NumberOfHour = poa.NumberOfHour;
                            professor.OrderNumber = poa.OrderNumber;
                            pdn.ProfessorOtherActivities.Add(professor);
                        }
                        pdn.ScienceResearches = new List<ScienceResearch>();
                        foreach (ScienceResearch sr in parentPlanDetail.ScienceResearches)
                        {
                            ScienceResearch science = new ScienceResearch();
                            science.Id = Guid.NewGuid();
                            science.Name = sr.Name;
                            science.NumberOfResearch = sr.NumberOfResearch;
                            science.CriterionDictionary = sr.CriterionDictionary;
                            science.OrderNumber = sr.OrderNumber;
                            pdn.ScienceResearches.Add(science);
                        }

                        session.Save(pdn);
                        pld.Id = pdn.Id;
                    }
                    break;
            }

            return pld;
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
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfTime = pa.NumberOfTime;
                            pad.NumberOfHour = pa.CriterionDictionary != null ? pa.CriterionDictionary.NumberOfHour : pa.NumberOfHour;
                            pad.OrderNumber = pa.OrderNumber;
                            pld.ProfessorOtherActivities.Add(pad);
                        }
                        pld.ScienceResearches = new List<ScienceResearchDTO>();
                        foreach (ScienceResearch pa in planDetail.ScienceResearches)
                        {
                            ScienceResearchDTO pad = new ScienceResearchDTO();
                            pad.Id = pa.Id;
                            pad.Name = pa.CriterionDictionary != null ? pa.CriterionDictionary.Name : pa.Name;
                            pad.NumberOfResearch = pa.NumberOfResearch;
                            pad.OrderNumber = pa.OrderNumber;
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
                        pld.OrderNumber = crd.OrderNumber;

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
            criterion.OrderNumber = p.OrderNumber; //Kế thừa số thứ tự

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
            result.OrderNumber = pld.OrderNumber; //Thêm số thứ tự
            return result;
        }

        //public static ConcurrentDictionary<Guid, PlanKPIDetail> GetPlanDetailDic(NHibernate.ISession session)
        //{
        //    ObjectCache cache = MemoryCache.Default;

        //    var result = cache.Get("PlanDetailDic") as ConcurrentDictionary<Guid, PlanKPIDetail>;
        //    if (result != null)
        //        return result;

        //    //SessionManager.DoWork(session =>
        //    //{
        //    result = new ConcurrentDictionary<Guid, PlanKPIDetail>(session.Query<PlanKPIDetail>().Cacheable().CacheMode(NHibernate.CacheMode.Normal).ToDictionary(p => p.Id, p => p));
        //    CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) };
        //    cache.Add("PlanDetailDic", result, policy);
        //    //});

        //    return result;
        //}

        //public static ConcurrentDictionary<Guid, PlanKPIDetail> UpdatePlanDetailDic(NHibernate.ISession session)
        //{
        //    ObjectCache cache = MemoryCache.Default;


        //    ConcurrentDictionary<Guid, PlanKPIDetail> result = new ConcurrentDictionary<Guid, PlanKPIDetail>();
        //    //if (result != null)
        //    //    return result;

        //    //SessionManager.DoWork(session =>
        //    //{
        //    result = new ConcurrentDictionary<Guid, PlanKPIDetail>(session.Query<PlanKPIDetail>().ToDictionary(p => p.Id, p => p));
        //    CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) };
        //    cache.Add("PlanDetailDic", result, policy);
        //    //});

        //    return result;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planDetail"></param>
        /// <param name="actionType">1 save or update, 2 delete</param>
        /// <param name="session"></param>
        //public static void UpdatePlanDetailDic(PlanKPIDetail planDetail, int actionType, NHibernate.ISession session)
        //{
        //    ObjectCache cache = MemoryCache.Default;
        //    ConcurrentDictionary<Guid, PlanKPIDetail> result = new ConcurrentDictionary<Guid, PlanKPIDetail>();
        //    CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddDays(30) };
        //    result = cache.Get("PlanDetailDic") as ConcurrentDictionary<Guid, PlanKPIDetail>;
        //    if (result == null)
        //    {
        //        //SessionManager.DoWork(session =>
        //        //{
        //        result = new ConcurrentDictionary<Guid, PlanKPIDetail>(session.Query<PlanKPIDetail>().Cacheable<PlanKPIDetail>().CacheMode(NHibernate.CacheMode.Normal).ToDictionary(p => p.Id, p => p));
        //        cache.Add("PlanDetailDic", result, policy);
        //        //});
        //    }
        //    else
        //    {
        //        switch (actionType)
        //        {
        //            case 1:
        //                result.AddOrUpdate(planDetail.Id, planDetail, (key, oldvalue) => planDetail);
        //                break;
        //            case 2:
        //                result.TryRemove(planDetail.Id, out planDetail);
        //                break;
        //        }

        //        cache.Add("PlanDetailDic", result, policy);
        //    }

        //}
        public static int GetMaxOrderNumberPlanKPIDetail(Guid planStaffId, Guid targetGroupId, NHibernate.ISession session)
        {
            int result = 0;
            List<PlanKPIDetail> list = new List<PlanKPIDetail>();
            list = session.Query<PlanKPIDetail>().Where(p => p.PlanStaff.Id == planStaffId && p.TargetGroupDetail.Id == targetGroupId).ToList();
            if (list.Count > 0)
                result = session.Query<PlanKPIDetail>().Where(p => p.PlanStaff.Id == planStaffId && p.TargetGroupDetail.Id == targetGroupId).Max(p => p.OrderNumber);
            return result;
        }

        public static int GetMaxOrderNumberActivity(Guid planKPIDetailId, NHibernate.ISession session)
        {
            int result = 0;
            List<ProfessorOtherActivity> list = new List<ProfessorOtherActivity>();
            list = session.Query<ProfessorOtherActivity>().Where(p => p.PlanKPIDetail != null && p.PlanKPIDetail.Id == planKPIDetailId).ToList();
            if (list.Count > 0)
                result = list.Max(p => p.OrderNumber);
            return result;
        }

        public static int GetMaxOrderNumberResearch(Guid planKPIDetailId, NHibernate.ISession session)
        {
            int result = 0;
            List<ScienceResearch> list = new List<ScienceResearch>();
            list = session.Query<ScienceResearch>().Where(p => p.PlanKPIDetail != null && p.PlanKPIDetail.Id == planKPIDetailId).ToList();
            if (list.Count > 0)
                result = list.Max(p => p.OrderNumber);
            return result;
        }

        public static int SendMail(List<MailListDTO> obj)
        {
            SessionManager.DoWork(session =>
            {
                //Lấy list người đc gửi 
                List<Guid> listStaff = obj.Select(m => m.StaffId).Distinct().ToList();
                //Lấy list đơn vị đc gửi 
                List<Guid> listDepartment = obj.Select(m => m.DepartmentId).Distinct().ToList();
                #region Gửi mail Cá nhân
                foreach (Guid st in listStaff)
                {
                    if (st != Guid.Empty)
                    {
                        string mailContent = "";
                        //Lấy mail của người được gửi
                        StaffProfile staff = session.Query<StaffProfile>().Where(s => s.Id == st).SingleOrDefault();

                        //Nếu có mail mới bắt đầu gửi
                        if (staff.Email != null && staff.Email != "")
                        {
                            //Lấy list criterion được phân cho staff
                            List<Guid> listCri = obj.Where(o => o.StaffId == st).Select(o => o.CriterionId).ToList();
                            int idx = 1;
                            foreach (Guid c in listCri)
                            {
                                //Lấy thông tin criterion đưa vào mail
                                Criterion criterion = session.Query<Criterion>().Where(cr => cr.Id == c).SingleOrDefault();
                                string StaffLeaderName = "";
                                try
                                {
                                    StaffLeaderName = criterion.FromPlanKPIDetail.StaffLeader != null ? criterion.FromPlanKPIDetail.StaffLeader.StaffProfile.Name : "";
                                }
                                catch (Exception e)
                                {
                                    throw e;
                                }
                                mailContent += "<tr><td style = 'text-align:center; padding:5px;width:50px'>" + idx.ToString() + "</td ><td style = 'padding:5px;'>" + criterion.FromPlanKPIDetail.TargetDetail + "</td><td style = 'padding:5px;'>" + StaffLeaderName + "</td></tr>";
                                idx++;

                            }

                            //Gửi mail
                            string to = "";

                            //Cho phép gửi mail tới mail nhân viên, còn ko gửi mail vào tk test
                            string allow = WebConfigurationManager.AppSettings["AllowSendMailToStaffMail"];
                            if (allow == "1")
                            {
                                to = staff.Email;
                            }
                            else
                            {
                                to = "ute.test.test@gmail.com";
                            }

                            string from = "ute.test.test@gmail.com"; //From address    
                            MailMessage message = new MailMessage(from, to);

                            string mailbody = "<div style='width:100%'><h4 style='text-align:center;font-size:13pt;font-family:Times New Roman'>THÔNG TIN CÔNG VIỆC ĐƯỢC GIAO</h4><table border='1' style='border-collapse:collapse; width:100%;font-size:11pt;'>";
                            mailbody += "<thead><tr><td style='padding:5px;width:50px;font-family:Times New Roman;text-align:center; font-weight:bold;background-color:lightgray'>STT</td>";
                            mailbody += "<td style='padding:5px;width:50px;font-family:Times New Roman;text-align:center; font-weight:bold;background-color:lightgray'>Mục tiêu cụ thể</td>";
                            mailbody += "<td style='padding:5px;width:50px;font-family:Times New Roman;text-align:center; font-weight:bold;background-color:lightgray'>Chỉ đạo</td></tr></thead><tbody>";
                            mailbody += mailContent + "</tbody></table></div>";
                            message.Subject = "HCMUTE KPIs - Thông báo";
                            message.Body = mailbody;
                            message.BodyEncoding = Encoding.UTF8;
                            message.IsBodyHtml = true;
                            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                            System.Net.NetworkCredential basicCredential1 = new
                            System.Net.NetworkCredential("ute.test.test@gmail.com", "ute@2016");
                            client.EnableSsl = true;
                            client.UseDefaultCredentials = false;
                            client.Credentials = basicCredential1;
                            try
                            {
                                client.Send(message);
                            }

                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                #endregion
                #region Gửi mail Đơn vị
                foreach (Guid dept in listDepartment)
                {
                    if (dept != Guid.Empty)
                    {
                        Department department = session.Query<Department>().Where(d => d.Id == dept).SingleOrDefault();
                        Staff staff = new Staff();

                        //Nếu đơn vị là phòng thì lấy trưởng phòng
                        if (department.DepartmentType == 1)
                        {
                            staff = session.Query<Staff>().Where(s => s.Department.Id == department.Id && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType.Id == 3).FirstOrDefault();
                        }
                        //Nếu đơn vị là khoa thì lấy trưởng khoa
                        else if (department.DepartmentType == 4)
                        {
                            staff = session.Query<Staff>().Where(s => s.Department.Id == department.Id && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType.Id == 5).FirstOrDefault();
                        }
                        //Nếu đơn vị là bộ môn thì lấy trưởng bộ môn
                        else if (department.DepartmentType == 3)
                        {
                            staff = session.Query<Staff>().Where(s => s.Department.Id == department.Id && s.StaffInfo.Position != null && s.StaffInfo.Position.AgentObjectType.Id == 6).FirstOrDefault();
                        }
                        if (staff!=null && staff.Id != Guid.Empty)
                        {
                            string mailContent = "";
                            //Lấy mail của trưởng đơn vị
                            //StaffProfile staff = session.Query<StaffProfile>().Where(s => s.).SingleOrDefault();

                            //Nếu có mail mới bắt đầu gửi
                            if (staff.StaffProfile.Email != null && staff.StaffProfile.Email != "")
                            {
                                //Lấy list criterion được phân cho staff
                                List<Guid> listCri = obj.Where(o => o.DepartmentId == dept).Select(o => o.CriterionId).ToList();
                                int idx = 1;
                                foreach (Guid c in listCri)
                                {
                                    //Lấy thông tin criterion đưa vào mail
                                    Criterion criterion = session.Query<Criterion>().Where(cr => cr.Id == c).SingleOrDefault();
                                    string StaffLeaderName = "";
                                    try
                                    {
                                        StaffLeaderName = criterion.FromPlanKPIDetail.StaffLeader != null ? criterion.FromPlanKPIDetail.StaffLeader.StaffProfile.Name : "";
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    mailContent += "<tr><td style = 'text-align:center; padding:5px;width:50px'>" + idx.ToString() + "</td ><td style = 'padding:5px;'>" + criterion.FromPlanKPIDetail.TargetDetail + "</td><td style = 'padding:5px;'>" + StaffLeaderName + "</td></tr>";
                                    idx++;

                                }

                                //Gửi mail
                                string to = "";

                                //Cho phép gửi mail tới mail nhân viên, còn ko gửi mail vào tk test
                                string allow = WebConfigurationManager.AppSettings["AllowSendMailToStaffMail"];
                                if (allow == "1")
                                {
                                    to = staff.StaffProfile.Email;
                                }
                                else
                                {
                                    to = "ute.test.test@gmail.com";
                                }

                                string from = "ute.test.test@gmail.com"; //From address    
                                MailMessage message = new MailMessage(from, to);

                                string mailbody = "<div style='width:100%'><h4 style='text-align:center;font-size:13pt;font-family:Times New Roman'>THÔNG TIN CÔNG VIỆC ĐƯỢC GIAO</h4><table border='1' style='border-collapse:collapse; width:100%;font-size:11pt;'>";
                                mailbody += "<thead><tr><td style='padding:5px;width:50px;font-family:Times New Roman;text-align:center; font-weight:bold;background-color:lightgray'>STT</td>";
                                mailbody += "<td style='padding:5px;width:50px;font-family:Times New Roman;text-align:center; font-weight:bold;background-color:lightgray'>Mục tiêu cụ thể</td>";
                                mailbody += "<td style='padding:5px;width:50px;font-family:Times New Roman;text-align:center; font-weight:bold;background-color:lightgray'>Chỉ đạo</td></tr></thead><tbody>";
                                mailbody += mailContent + "</tbody></table></div>";
                                message.Subject = "HCMUTE KPIs - Thông báo";
                                message.Body = mailbody;
                                message.BodyEncoding = Encoding.UTF8;
                                message.IsBodyHtml = true;
                                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                                System.Net.NetworkCredential basicCredential1 = new
                                System.Net.NetworkCredential("ute.test.test@gmail.com", "ute@2016");
                                client.EnableSsl = true;
                                client.UseDefaultCredentials = false;
                                client.Credentials = basicCredential1;
                                try
                                {
                                    client.Send(message);
                                }

                                catch (Exception ex)
                                {
                                }
                            }
                        }

                    }
                }
                #endregion
            });
            return 1;
        }

    }
}
