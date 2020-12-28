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
    public class ABC_KyDanhGiaApiController : ApiController
    {
        public List<ABC_KyDanhGia> GetList(ISession session)
        {
            return session.Query<ABC_KyDanhGia>().Where(e => e.GCRecord == null).OrderBy(e => e.TuNgay).ThenBy(e => e.Parent).ToList();
        }
        public ABC_KyDanhGia GetById(ISession session,Guid id)
        {
            return session.Query<ABC_KyDanhGia>().Single(e => e.Id == id && e.GCRecord == null);
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KyDanhGiaDTO> GetListDTO()
        {
            List<ABC_KyDanhGiaDTO> ListReusult = new List<ABC_KyDanhGiaDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                ListReusult = GetList(session).Map<ABC_KyDanhGiaDTO>();
            });
            return ListReusult;
        }
        [Authorize]
        [Route("")]
        public ABC_KyDanhGiaDTO GetDTOById(Guid id)
        {
            ABC_KyDanhGiaDTO Reusult = new ABC_KyDanhGiaDTO();
            SessionManager.DoWorkNoTransaction(session =>
            {
                Reusult = GetById(session, id).Map<ABC_KyDanhGiaDTO>();
            });
            return Reusult;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<int> GetListYear()
        {
            List<int> ListReusult = new List<int>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                ListReusult = GetList(session).Select(e=> e.Nam.Value).Distinct().ToList();
            });
            return ListReusult;
        }

        [Authorize]
        [Route("")]
        public IEnumerable<ABC_KyDanhGiaDTO> GetListByYear(int year)
        {
            List<ABC_KyDanhGiaDTO> ListReusult = new List<ABC_KyDanhGiaDTO>();
            SessionManager.DoWorkNoTransaction(session =>
            {
                ListReusult = session.Query<ABC_KyDanhGia>().Where(e => e.Nam == year && e.GCRecord == null).OrderBy(e => e.TuNgay).ThenBy(e => e.Parent).ToList().Map<ABC_KyDanhGiaDTO>();
            });
            return ListReusult;
        }

        [Authorize]
        [Route("")]
        public int PostByYear(int nam, Guid userId)
        {
            int result = 0;
            try
            {
                List<ABC_KyDanhGia> objOld = new List<ABC_KyDanhGia>();
                SessionManager.DoWork(session =>
                {
                    objOld = session.Query<ABC_KyDanhGia>().Where(e => e.Nam == nam).ToList();
                });

                if (objOld.Count != 0) // Nếu đã có thì không thêm nữa
                    return 0;

                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia objY = new ABC_KyDanhGia();
                    objY.Id = Guid.NewGuid();
                    Guid ParentId = objY.Id;
                    objY.TuNgay = new DateTime(nam, 1, 1);
                    objY.DenNgay = new DateTime(nam, 12, 31);
                    objY.Nam = nam;
                    objY.Name = "Năm " + nam;
                    objY.AddUserId = userId;
                    objY.AddTime = DateTime.Now;
                    objY.Loai = 12;  // Loai 12: Nam
                                    // Loai 6: 6 tháng
                                    // Loai 3: Quy'
                                    // Loai 1: Thang'
                    session.Save(objY);
                    for (int i = 1; i <= 12; i++)
                    {
                        ABC_KyDanhGia objM = new ABC_KyDanhGia();

                        objM.Id = Guid.NewGuid();
                        objM.Parent = new ABC_KyDanhGia() { Id = ParentId };
                        objM.Nam = nam;
                        objM.Loai = 1;
                        objM.Name = "Tháng " + i + " Năm " + nam;
                        objM.TuNgay = new DateTime(nam, i, 1);
                        objM.DenNgay = objM.TuNgay.Value.AddMonths(1).AddDays(-1);
                        objM.AddUserId = userId;
                        objM.AddTime = DateTime.Now;
                        if (i <= 8)
                            objM.NamHoc = string.Format("{0} - {1}", nam - 1, nam);
                        else
                            objM.NamHoc = string.Format("{0} - {1}", nam, nam + 1);

                        session.Save(objM);
                    }
                    result = 1;
                });
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_KyDanhGiaApi/PostByYear;nam=" + nam + "&userId=" + userId, ex); throw ex;
            }
            return result;
        }

        [Authorize]
        [Route("")]
        public int Put(Guid id, Guid userId, [FromBody]ABC_KyDanhGiaDTO obj)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                if (id != obj.Id)
                    return 0;
                SessionManager.DoWork(session =>
                {
                    ABC_KyDanhGia KyDanhGia = new ABC_KyDanhGia();
                    KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == id);
                    KyDanhGia.NgayBatDauDanhGia = obj.NgayBatDauDanhGia;
                    KyDanhGia.NgayKetThucDanhGia = obj.NgayKetThucDanhGia;
                    KyDanhGia.LastEditUserId = userId;
                    KyDanhGia.LastEditTime = DateTimeNow;
                    session.Update(KyDanhGia);
                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_KyDanhGiaApi/Put?id=" + id + "&userId=" + userId, ex); throw ex;
            }
            return result;
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
                    ABC_KyDanhGia KyDanhGia = new ABC_KyDanhGia();
                    KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == id);
                    KyDanhGia.GCRecord = Convert.ToInt64(DateTimeNow.ToString("yyyyMMddHHmmss"));
                    KyDanhGia.DeleteTime = DateTimeNow;
                    KyDanhGia.DeleteUserId = userId;
                    session.Update(KyDanhGia);
                });
                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_KyDanhGiaApi/DeleteById?id=" + id + "&userId=" + userId, ex); throw ex;
            }

            return result;
        }

        [Authorize]
        [Route("")]
        public int DeleteByYear([FromBody]List<ABC_KyDanhGiaDTO> list, Guid userId)
        {
            int result = 0;
            DateTime DateTimeNow = DateTime.Now;
            try
            {
                if (list != null)
                {
                    SessionManager.DoWork(session =>
                    {
                        foreach (ABC_KyDanhGiaDTO obj in list)
                        {
                            ABC_KyDanhGia KyDanhGia = new ABC_KyDanhGia();
                            KyDanhGia = session.Query<ABC_KyDanhGia>().Single(e => e.Id == obj.Id);
                            KyDanhGia.GCRecord = Convert.ToInt64(DateTimeNow.ToString("yyyyMMddHHmmss"));
                            KyDanhGia.DeleteTime = DateTimeNow;
                            KyDanhGia.DeleteUserId = userId;
                            session.Update(KyDanhGia);
                        }
                    });
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Helper.ErrorLog("ABC_KyDanhGiaApi/DeleteById?userId=" + userId, ex); throw ex;
            }

            return result;
        }
    }
}
