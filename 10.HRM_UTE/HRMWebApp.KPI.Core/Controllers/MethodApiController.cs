using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.Helpers;
using System.Web.Http;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class MethodApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public IEnumerable<DepartmentDTO> GetList()
        {
            var result = new List<DepartmentDTO>();
            SessionManager.DoWork(session =>
            {
                Guid spkt = new Guid("E054A602-E077-444C-B843-E856D643CA7F");
                result = session.Query<Department>().Where(d => d.ParentDepartment.Id == spkt && d.GCRecord == null).OrderBy(d => d.Name).ToList().Map<DepartmentDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public int GetMaxOrderNumberMethods(Guid planKPIDetailId)
        {
            int result = 0;
            List<Method> list = new List<Method>();
            SessionManager.DoWork(session =>
            {
                list = session.Query<Method>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).ToList();
                if (list.Count > 0)
                    result = session.Query<Method>().Where(p => p.PlanKPIDetail.Id == planKPIDetailId).Max(p => p.OrderNumber);
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public bool GetCheckPlanDetailMethod(Guid planDetailId)
        {
            bool result = false;
            SessionManager.DoWork(session =>
            {
                List<MethodDTO> originalMethods = ControllerHelpers.GetOriginalMethods(planDetailId, session);
                result = originalMethods.Count() >= 0 ? true : false;
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<MethodStaffDTO> GetListByPlanDetail(Guid Id)
        {
            var result = new List<MethodStaffDTO>();
            SessionManager.DoWork(session =>
            {
                List<Method> list = session.Query<Method>().Where(d => d.PlanKPIDetail.Id == Id).OrderBy(d => d.OrderNumber).ToList();
                foreach (Method m in list)
                {
                    MethodStaffDTO mdto = new MethodStaffDTO();
                    mdto.Id = m.Id;
                    mdto.Name = m.Name;
                    mdto.OrderNumber = m.OrderNumber;
                    mdto.PlanKPIDetailId = m.PlanKPIDetail.Id;
                    mdto.StartTime = m.StartTime;
                    mdto.EndTime = m.EndTime;
                    mdto.StartTimeString = m.StartTime.ToShortDateString();
                    mdto.EndTimeString = m.EndTime.ToShortDateString();
                    // mdto.LeadDepartment = m.LeadDepartment != null ? new DepartmentDTO() { Id = m.LeadDepartment.Id } : null;
                    mdto.LeadDepartment = new List<Method_DepartmentDTO>();
                    foreach (Method_Department de in m.LeadDepartment)
                    {
                        Method_DepartmentDTO metd = new Method_DepartmentDTO();
                        metd.Id = de.Id;
                        metd.Diem1 = de.Diem1;
                        metd.Diem2 = de.Diem2;
                        metd.Diem3 = de.Diem3;
                        metd.Diem4 = de.Diem4;
                        metd.DiemSo = de.DiemSo;
                        metd.DepartmentId = new DepartmentDTO() { Id = de.DepartmentId.Id, Name = de.DepartmentId.Name };
                        mdto.LeadDepartment.Add(metd);
                    }
                    mdto.PlanDetailSubStaffs = new List<MethodSubStaffDTO>();
                    foreach (Staff y in m.SubStaffs)
                    {
                        StaffProfile st = session.Query<StaffProfile>().Where(r => r.Id == m.Id).FirstOrDefault();
                        MethodSubStaffDTO i = new MethodSubStaffDTO();
                        i.StaffId = m.Id;
                        i.StaffName = m.Name;
                        mdto.PlanDetailSubStaffs.Add(i);
                    }
                    result.Add(mdto);
                }
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<MethodDTO> GetMethodDetail(Guid Id)
        {
            var result = new List<MethodDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<MethodDetail>().Where(d => d.Method.Id == Id).OrderBy(d => d.OrderNumber).ToList().Map<MethodDTO>();
            });
            return result;
        }

        [Authorize]
        [Route("")]
        public MethodStaffDTO GetObj(Guid id)
        {
            var result = new MethodStaffDTO();
            SessionManager.DoWork(session =>
            {
                Method method = session.Query<Method>().Where(a => a.Id == id).FirstOrDefault();
                PlanKPIDetail plan = session.Query<PlanKPIDetail>().Where(a => a.Id == method.PlanKPIDetail.Id).SingleOrDefault();
                Method parentmethod = session.Query<Method>().Where(r => r.PlanKPIDetail.Id == plan.ParentPlanKPIDetail.Id).SingleOrDefault();
                //foreach (Method m in method)
                //{
                //  MethodStaffDTO mdto = new MethodStaffDTO();
                result.Id = method.Id;
                result.Name = method.Name;
                result.OrderNumber = method.OrderNumber;
                result.PlanKPIDetailId = method.PlanKPIDetail.Id;
                result.StartTime = method.StartTime;
                result.EndTime = method.EndTime;
                result.StartTimeString = method.StartTime.ToShortDateString();
                result.EndTimeString = method.EndTime.ToShortDateString();
                // result.LeadDepartment = method.LeadDepartment != null ? new DepartmentDTO() { Id = method.LeadDepartment.Id } : null;
                result.LeadDepartment = new List<Method_DepartmentDTO>();

                if(method.LeadDepartment.Count == 0)
                {
                    foreach (Method_Department de in parentmethod.LeadDepartment)
                    {
                        Method_DepartmentDTO metd = new Method_DepartmentDTO();
                        metd.Id = de.Id;
                        metd.Diem1 = de.Diem1;
                        metd.Diem2 = de.Diem2;
                        metd.Diem3 = de.Diem3;
                        metd.Diem4 = de.Diem4;
                        metd.DiemSo = de.DiemSo;
                        metd.DepartmentId = new DepartmentDTO() { Id = de.DepartmentId.Id, Name = de.DepartmentId.Name };
                        result.LeadDepartment.Add(metd);
                    }
                }
                else
                {
                    foreach (Method_Department de in method.LeadDepartment)
                    {
                        Method_DepartmentDTO metd = new Method_DepartmentDTO();
                        metd.Id = de.Id;
                        metd.Diem1 = de.Diem1;
                        metd.Diem2 = de.Diem2;
                        metd.Diem3 = de.Diem3;
                        metd.Diem4 = de.Diem4;
                        metd.DiemSo = de.DiemSo;
                        metd.DepartmentId = new DepartmentDTO() { Id = de.DepartmentId.Id, Name = de.DepartmentId.Name };
                        result.LeadDepartment.Add(metd);
                    }
                }
              
                result.PlanDetailSubStaffs = new List<MethodSubStaffDTO>();
                foreach (Staff y in method.SubStaffs)
                {
                    StaffProfile st = session.Query<StaffProfile>().Where(r => r.Id == y.Id).FirstOrDefault();
                    MethodSubStaffDTO i = new MethodSubStaffDTO();
                    i.StaffId = y.Id;
                    i.StaffName = st.Name;
                    result.PlanDetailSubStaffs.Add(i);
                }
                // result.Add(mdto);
                //}
            });
            return result;
        }

        //[Authorize]
        //[Route("")]
        //public bool GetUpdatePlanDetailDic(Guid planDetailId)
        //{
        //    bool result = false;
        //    SessionManager.DoWork(session =>
        //    {
        //        PlanKPIDetail planDetail = session.Query<PlanKPIDetail>().Where(p => p.Id == planDetailId).SingleOrDefault();
        //        ControllerHelpers.UpdatePlanDetailDic(planDetail, 1, session);
        //        result = true;
        //    });
        //    return result;
        //}

        [Authorize]
        [Route("")]
        public Guid Put(MethodStaffDTO obj)
        {
            Guid result = Guid.Empty;
            //try { 
            SessionManager.DoWork(session =>
            {
                Method meth = new Method();

                if (obj.Id == Guid.Empty)
                {
                    // obj.Id = Guid.NewGuid();
                    if (obj.PlanKPIDetail.Id == Guid.Empty)
                    {
                        //Thêm vào plandetail mới (parent)
                        PlanKPIDetail pld = new PlanKPIDetail();
                        pld.Id = Guid.NewGuid();
                        pld.PlanStaff = obj.Planstaff != null ? new PlanStaff() { Id = obj.Planstaff } : null;
                        pld.TargetGroupDetail = obj.PlanKPIDetail.TargetGroupDetail;
                        pld.StartTime = DateTime.Now;
                        pld.EndTime = DateTime.Now;
                        pld.CreateTime = DateTime.Now;

                        meth.StartTime = obj.StartTime.ToLocalTime();
                        meth.EndTime = obj.EndTime.ToLocalTime();
                        meth.OrderNumber = obj.OrderNumber;
                        meth.PlanKPIDetail = pld;
                        meth.Id = Guid.NewGuid();
                        meth.Name = obj.Name;
                        // meth.LeadDepartment = obj.LeadDepartment != null ? new Department() { Id = obj.LeadDepartment.Id } : null;
                        meth.SubStaffs = new List<Staff>();
                        if (obj.PlanDetailSubStaffs != null)
                        {
                            foreach (var id in obj.PlanDetailSubStaffs)
                            {
                                meth.SubStaffs.Add(new Staff() { Id = id.StaffId });
                            }
                        }

                        pld.Methods = new List<Method>();
                        pld.Methods.Add(meth);
                        session.SaveOrUpdate(pld);
                        if (obj.LeadDepartment != null)
                        {
                            foreach (var de in obj.LeadDepartment)
                            {
                                Method_Department d = new Method_Department();
                                d.Id = Guid.NewGuid();
                                d.MethodId = new Method() { Id = obj.Id };
                                d.DepartmentId = new Department() { Id = de.DepartmentId.Id };
                                d.Diem1 = de.Diem1;
                                d.Diem2 = de.Diem2;
                                d.Diem3 = de.Diem3;
                                d.Diem4 = de.Diem4;
                                d.DiemSo = de.DiemSo;
                                session.Save(d);
                            }
                        }
                    }
                    else
                    {
                        PlanKPIDetail par = session.Query<PlanKPIDetail>().Where(r => r.Id == obj.PlanKPIDetail.Id).FirstOrDefault();
                        PlanKPIDetail pl = new PlanKPIDetail();
                        if (par.ParentPlanKPIDetail != null)
                            pl = session.Query<PlanKPIDetail>().Where(r => r.Id == par.ParentPlanKPIDetail.Id).FirstOrDefault();
                        if (pl.Id != Guid.Empty)
                        {
                            Method meths = new Method();
                            meths.StartTime = obj.StartTime.ToLocalTime();
                            meths.EndTime = obj.EndTime.ToLocalTime();
                            meths.OrderNumber = obj.OrderNumber;
                            meths.Id = Guid.NewGuid();
                            meths.Name = obj.Name;
                            meths.PlanKPIDetail = new PlanKPIDetail() { Id = pl.Id };
                            meths.SubStaffs = new List<Staff>();
                            if (obj.PlanDetailSubStaffs != null)
                            {
                                foreach (var id in obj.PlanDetailSubStaffs)
                                {
                                    meths.SubStaffs.Add(new Staff() { Id = id.StaffId });
                                }
                            }
                            session.SaveOrUpdate(meths);
                            if (obj.LeadDepartment != null)
                            {
                                foreach (var de in obj.LeadDepartment)
                                {
                                    Method_Department d = new Method_Department();
                                    d.Id = Guid.NewGuid();
                                    d.MethodId = new Method() { Id = meths.Id };
                                    d.DepartmentId = new Department() { Id = de.DepartmentId.Id };
                                    d.Diem1 = de.Diem1;
                                    d.Diem2 = de.Diem2;
                                    d.Diem3 = de.Diem3;
                                    d.Diem4 = de.Diem4;
                                    d.DiemSo = de.DiemSo;
                                    session.SaveOrUpdate(d);
                                }
                            }
                        }
                        meth.StartTime = obj.StartTime.ToLocalTime();
                        meth.EndTime = obj.EndTime.ToLocalTime();
                        meth.OrderNumber = obj.OrderNumber;
                        meth.Id = Guid.NewGuid();
                        meth.Name = obj.Name;
                        meth.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetail.Id };
                        meth.SubStaffs = new List<Staff>();
                        if (obj.PlanDetailSubStaffs != null)
                        {
                            foreach (var id in obj.PlanDetailSubStaffs)
                            {
                                meth.SubStaffs.Add(new Staff() { Id = id.StaffId });
                            }
                        }
                        session.SaveOrUpdate(meth);
                        if (obj.LeadDepartment != null)
                        {
                            foreach (var de in obj.LeadDepartment)
                            {
                                Method_Department d = new Method_Department();
                                d.Id = Guid.NewGuid();
                                d.MethodId = new Method() { Id = meth.Id };
                                d.DepartmentId = new Department() { Id = de.DepartmentId.Id };
                                d.Diem1 = de.Diem1;
                                d.Diem2 = de.Diem2;
                                d.Diem3 = de.Diem3;
                                d.Diem4 = de.Diem4;
                                d.DiemSo = de.DiemSo;
                                session.SaveOrUpdate(d);
                            }
                        }
                    }
                }
                else
                {
                    PlanKPIDetail par = session.Query<PlanKPIDetail>().Where(r => r.Id == obj.PlanKPIDetail.Id).FirstOrDefault();
                    List<PlanKPIDetail> newpld = new List<PlanKPIDetail>();
                    if (par.ParentPlanKPIDetail != null)
                        newpld = session.Query<PlanKPIDetail>().Where(r => r.Id == par.ParentPlanKPIDetail.Id).ToList();


                    meth.Id = obj.Id;
                    meth.Name = obj.Name;
                    meth.PlanKPIDetail = new PlanKPIDetail() { Id = obj.PlanKPIDetail.Id };
                    meth.OrderNumber = obj.OrderNumber;
                    meth.StartTime = obj.StartTime.ToLocalTime();
                    meth.EndTime = obj.EndTime.ToLocalTime();
                    meth.SubStaffs = new List<Staff>();

                    if (obj.PlanDetailSubStaffs != null)
                    {
                        foreach (var id in obj.PlanDetailSubStaffs)
                        {
                            meth.SubStaffs.Add(new Staff() { Id = id.StaffId });
                        }
                    }
                    List<Method_Department> metds = session.Query<Method_Department>().Where(r => r.MethodId.Id == obj.Id).ToList();
                    foreach (var i in metds)
                    {
                        session.Delete(i);
                    }
                    if (obj.LeadDepartment != null)
                    {
                        foreach (var de in obj.LeadDepartment)
                        {
                            Method_Department d = new Method_Department();
                            d.Id = Guid.NewGuid();
                            d.MethodId = new Method() { Id = obj.Id };
                            d.DepartmentId = new Department() { Id = de.DepartmentId.Id };
                            d.Diem1 = de.Diem1;
                            d.Diem2 = de.Diem2;
                            d.Diem3 = de.Diem3;
                            d.Diem4 = de.Diem4;
                            d.DiemSo = de.DiemSo;
                            session.SaveOrUpdate(d);
                        }
                    }
                    session.Update(meth);
                    //// sửa method ở cấp trên 
                    foreach (PlanKPIDetail pl in newpld)
                    {
                        if (pl.Id != Guid.Empty)
                        {
                            List<Method> n = session.Query<Method>().Where(r => r.PlanKPIDetail.Id == pl.Id && r.Name.Trim() == obj.Name.Trim()).ToList();
                            foreach (Method item in n)
                            {
                                item.SubStaffs = new List<Staff>();
                                if (obj.PlanDetailSubStaffs != null)
                                {
                                    foreach (var id in obj.PlanDetailSubStaffs)
                                    {
                                        item.SubStaffs.Add(new Staff() { Id = id.StaffId });
                                    }
                                }
                                List<Method_Department> me = session.Query<Method_Department>().Where(r => r.MethodId.Id == item.Id).ToList();
                                foreach (var i in me)
                                {
                                    session.Delete(i);
                                }
                                if (obj.LeadDepartment != null)
                                {
                                    foreach (var de in obj.LeadDepartment)
                                    {
                                        Method_Department d = new Method_Department();
                                        d.Id = Guid.NewGuid();
                                        d.MethodId = new Method() { Id = item.Id };
                                        d.DepartmentId = new Department() { Id = de.DepartmentId.Id };
                                        d.Diem1 = de.Diem1;
                                        d.Diem2 = de.Diem2;
                                        d.Diem3 = de.Diem3;
                                        d.Diem4 = de.Diem4;
                                        d.DiemSo = de.DiemSo;
                                        session.SaveOrUpdate(d);
                                    }
                                }
                                session.Update(item);
                            }

                        }
                    }

                }
                result = obj.PlanKPIDetail.Id;
            });
            return result;

        }

        [Authorize]
        [Route("")]
        public Method Delete(Method obj)
        {
            SessionManager.DoWork(session =>
            {
                List<Method_Department> me = session.Query<Method_Department>().Where(r => r.MethodId.Id == obj.Id).ToList();
                if (me.Count > 0)
                {
                    foreach (var m in me)
                    {
                        session.Delete(m);
                    }
                }
                obj = session.Query<Method>().Where(m => m.Id == obj.Id).SingleOrDefault();
                session.Delete(obj);
                //Update cache
                //ControllerHelpers.UpdatePlanDetailDic(obj.PlanKPIDetail, 2, session);
            });
            return obj;
        }
    }
}
