using System;
using System.Collections.Generic;
using System.Linq;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.KPI.Core.Helpers;
using HRMWebApp.KPI.Core.DTO.ABC.New;
using NHibernate;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_TieuChiApiController : ApiController
    {
        public IEnumerable<ABC_TieuChi> GetByBoTieuChiId(Guid boTieuChiId, ISession session)
        {
            List<ABC_TieuChi> Result = new List<ABC_TieuChi>();
            Result = session.Query<ABC_TieuChi>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GCRecord == null).OrderBy(e => e.STT).ToList();
            return Result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TieuChi> GetByParentId(Guid parentId, ISession session)
        {
            List<ABC_TieuChi> Result = new List<ABC_TieuChi>();
            Result = session.Query<ABC_TieuChi>().Where(e => e.Parent.Id == parentId && e.GCRecord == null).OrderBy(e => e.STT).ToList();
            return Result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TieuChiVMDTO> GetListDTO()
        {
            List<ABC_TieuChiVMDTO> ListReusult = new List<ABC_TieuChiVMDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                ListReusult = session.Query<ABC_TieuChi>().Where(e => e.GCRecord == null).OrderBy(e => e.STT).ToList().Map<ABC_TieuChiVMDTO>();
            });
            return ListReusult;
        }

        [Authorize]
        [Route("")]
        public ABC_TieuChiVMDTO GetDTOById(Guid id)
        {
            ABC_TieuChiVMDTO Result = new ABC_TieuChiVMDTO();
            SessionManager.DoWorkNoTransaction(session =>
            {
                Result = session.Query<ABC_TieuChi>().Single(e => e.Id == id).Map<ABC_TieuChiVMDTO>();
            });
            return Result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TieuChiVMDTO> GetDTOByBoTieuChiId(Guid boTieuChiId)
        {
            List<ABC_TieuChiVMDTO> Result = new List<ABC_TieuChiVMDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                Result = GetByBoTieuChiId(boTieuChiId, session).Map<ABC_TieuChiVMDTO>();
            });
            return Result;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TieuChiVMDTO> GetDTOByParentId(Guid parentId)
        {
            List<ABC_TieuChiVMDTO> Reusult = new List<ABC_TieuChiVMDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                Reusult = GetByParentId(parentId, session).Map<ABC_TieuChiVMDTO>();
            });
            return Reusult;
        }

        [Authorize]
        [Route("")]
        public int Put(Guid? id, Guid userId, [FromBody]ABC_TieuChiCreateDTO obj)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_TieuChi TieuChi = new ABC_TieuChi();
                    if (id == Guid.Empty || id == null || id != obj.Id)
                    {
                        TieuChi.Id = Guid.NewGuid();
                        TieuChi.AddTime = DateTimeNow;
                        TieuChi.AddUserId = userId;
                    }
                    else
                    {
                        TieuChi.Id = id.Value;
                        TieuChi.LastEditTime = DateTimeNow;
                        TieuChi.LastEditUserId = userId;
                    }
                    TieuChi.NoiDung = obj.NoiDung;
                    TieuChi.ListDiem = obj.ListDiem;
                    if (obj.ParentId.HasValue)
                        TieuChi.Parent = new ABC_TieuChi() { Id = obj.ParentId.Value };
                    TieuChi.STT = obj.STT;
                    TieuChi.DiemTru = obj.DiemTru;
                    TieuChi.ChildSelectOne = obj.ChildSelectOne;
                    TieuChi.IsDiemDanhGiaCongTac = obj.IsDiemDanhGiaCongTac;
                    TieuChi.IsDiemThuong = obj.IsDiemThuong;
                    TieuChi.DiemToiDa = obj.DiemToiDa;
                    TieuChi.ChiMuc = obj.ChiMuc;
                    TieuChi.BoTieuChi = new ABC_BoTieuChi() { Id = obj.BoTieuChiId.Value };

                    session.SaveOrUpdate(TieuChi);

                    Log(TieuChi, DateTimeNow, session);

                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_TieuChiApi/Put?id=" + id + "&userId=" + userId, ex); throw ex;
            }
            return result;
        }
        public void DeleteById(Guid id, Guid userId, DateTime dateTimeNow, ISession session)
        {
            ABC_TieuChi TieuChi = new ABC_TieuChi();
            TieuChi = session.Query<ABC_TieuChi>().Single(e => e.Id == id);
            TieuChi.GCRecord = Convert.ToInt64(dateTimeNow.ToString("yyyyMMddHHmmss"));
            TieuChi.DeleteTime = dateTimeNow;
            TieuChi.DeleteUserId = userId;
            session.Update(TieuChi);

            Log(TieuChi, dateTimeNow, session);
            DeleteChildByParentId(id, userId, dateTimeNow, session);
        }
        public void DeleteChildByParentId(Guid parentId, Guid userId, DateTime dateTimeNow, ISession session)
        {
            IEnumerable<ABC_TieuChi> ListTieuChi = GetByParentId(parentId, session);

            foreach (ABC_TieuChi TieuChi in ListTieuChi)
            {
                DeleteById(TieuChi.Id, userId, dateTimeNow, session);
            }
        }
        [Authorize]
        [Route("")]
        public int DeleteById(Guid id, Guid userId)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                SessionManager.DoWork(session =>
                {
                    DeleteById(id, userId, DateTimeNow, session);
                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_TieuChiApi/DeleteById?id=" + id + "&userId=" + userId, ex); throw ex;
            }

            return result;
        }
        public void Log(ABC_TieuChi tieuChi, DateTime dateTimeNow, ISession session)
        {
            ABC_LogTieuChi LogBoTieuChi = new ABC_LogTieuChi()
            {
                Id = Guid.NewGuid(),
                NoiDung = tieuChi.NoiDung,
                ChiMuc = tieuChi.ChiMuc,
                DiemToiDa = tieuChi.DiemToiDa,
                DiemTru = tieuChi.DiemTru,
                STT = tieuChi.STT,
                TieuChiId = tieuChi.Id,
                AddTime = tieuChi.AddTime,
                AddUserId = tieuChi.AddUserId,
                DeleteTime = tieuChi.DeleteTime,
                DeleteUserId = tieuChi.DeleteUserId,
                GCRecord = tieuChi.GCRecord,
                ChildSelectOne = tieuChi.ChildSelectOne,
                LastEditTime = tieuChi.LastEditTime,
                LastEditUserId = tieuChi.LastEditUserId,
                TimeLog = dateTimeNow,
                ListDiem = tieuChi.ListDiem
            };

            if (tieuChi.Parent != null)
                LogBoTieuChi.ParentId = tieuChi.Parent.Id;

            if (tieuChi.BoTieuChi != null)
                LogBoTieuChi.BoTieuChiId = tieuChi.BoTieuChi.Id;

            session.Save(LogBoTieuChi);

        }
    }
}
