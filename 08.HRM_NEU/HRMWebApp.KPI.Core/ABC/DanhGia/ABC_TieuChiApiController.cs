using HRMWebApp.Helpers;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class ABC_TieuChiApiController : ApiController
    {

        [Authorize]
        [Route("")]
        public ABC_TieuChiDTO GetById(Guid id)
        {
            ABC_TieuChiDTO result = new ABC_TieuChiDTO();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_TieuChi>().SingleOrDefault(e => e.Id == id).Map<ABC_TieuChiDTO>();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public float GetMaxDiem(Guid? TieuChiId, Guid ParentId)
        {
            float result = -1;
            ABC_TieuChi objParent = new ABC_TieuChi();
            List<ABC_TieuChi> objChildrent = new List<ABC_TieuChi>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    objParent = session.Query<ABC_TieuChi>().SingleOrDefault(e => e.Id == ParentId);
                    objChildrent = session.Query<ABC_TieuChi>().Where(e => e.Parent.Id == ParentId).ToList();
                });
                result = objParent.DiemToiDa.Value;
                foreach (ABC_TieuChi item in objChildrent)
                {
                    if (item.Id != TieuChiId)
                        result -= item.DiemToiDa.Value;
                }
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_TieuChiApi/GetMaxDiem", ex); throw ex; }
            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_TieuChiDTO> GetByBoTieuChiId(Guid boTieuChiId)
        {
            List<ABC_TieuChiDTO> result = new List<ABC_TieuChiDTO>();
            SessionManager.DoWork(session =>
            {
                result = session.Query<ABC_TieuChi>().Where(e => e.BoTieuChi.Id == boTieuChiId && e.GCRecord == null).OrderBy(e => e.STT).ThenBy(e => e.STTSapXep).Map<ABC_TieuChiDTO>().ToList();
            });
            return result;
        }
        [Authorize]
        [Route("")]
        public int PutSaveOrUpdate(ABC_TieuChiDTO obj)
        {
            int resultReturn = 0;
            try
            {
                ABC_TieuChi result = new ABC_TieuChi();
                SessionManager.DoWork(session =>
                {
                    DateTime TimeNow = DateTime.Now;
                    if (obj.Id == Guid.Empty)
                    {
                        result.Id = Guid.NewGuid();
                        result.AddUserId = obj.AddUserId;
                        result.AddTime = TimeNow;
                    }
                    else
                    {
                        result = session.Query<ABC_TieuChi>().Single(e => e.Id == obj.Id);
                        result.LastEditUserId = obj.LastEditUserId;
                        result.LastEditTime = TimeNow;
                    }
                    result.BoTieuChi = new ABC_BoTieuChi() { Id = obj.BoTieuChiId.Value };
                    if (obj.ParentId != Guid.Empty && obj.ParentId != null)
                        result.Parent = new ABC_TieuChi() { Id = obj.ParentId.Value };
                    result.STT = obj.STT;
                    result.STTSapXep = obj.STTSapXep;
                    result.NoiDung = obj.NoiDung;
                    result.DiemToiDa = obj.DiemToiDa;
                    result.HeSoTieuChiCon = obj.HeSoTieuChiCon;
                    result.DiemTru = obj.DiemTru;
                    result.IsAutoScore = obj.IsAutoScore;
                    if (obj.IsAutoScore == true)
                    {   
                        result.DieuKienLoaiDiem = obj.DieuKienLoaiDiem; // 0: Tổng điểm của bộ tiêu chí đã chọn; 1: Điểm tính theo các tiêu chí được chọn
                        result.DieuKienThoiGian = obj.DieuKienThoiGian; // 0: Năm; 12: 12 tháng; 1: Tính theo danh sách tháng đã chọn
                        if (result.DieuKienThoiGian == 1)
                        {
                            result.DieuKienListThoiGian = obj.DieuKienListThoiGian;
                        }
                        result.DieuKienDiemNhanVien = obj.DieuKienDiemNhanVien; // 1: Theo từng người;  2: Trung bình tất cả các thành viên trong nhóm
                        result.CongThucTinhDiem = obj.CongThucTinhDiem;
                        result.IsTeacher = obj.IsTeacher;
                        result.CongThucTinhDiemTeacher = obj.IsTeacher == true ? obj.CongThucTinhDiemTeacher : "";
                    }

                    session.SaveOrUpdate(result);
                    //Log tiêu chí
                    ABC_LogTieuChi LogTieuChi = new ABC_LogTieuChi()
                    {
                        Id = Guid.NewGuid(),
                        TieuChi = new ABC_TieuChi() { Id = result.Id},
                        AddTime = result.AddTime,
                        AddUserId = result.AddUserId,
                        BoTieuChiId = result.BoTieuChi.Id,
                        CongThucTinhDiem = result.CongThucTinhDiem,
                        CongThucTinhDiemTeacher = result.CongThucTinhDiemTeacher,
                        DiemToiDa = result.DiemToiDa,
                        DiemTru = result.DiemTru,
                        DieuKienDiemNhanVien = result.DieuKienDiemNhanVien,
                        DieuKienListThoiGian = result.DieuKienListThoiGian,
                        DieuKienLoaiDiem = result.DieuKienLoaiDiem,
                        DieuKienThoiGian = result.DieuKienThoiGian,
                        GCRecord = result.GCRecord,
                        HeSoTieuChiCon = result.HeSoTieuChiCon,
                        IsAutoScore = result.IsAutoScore,
                        IsTeacher = result.IsTeacher,
                        LastEditTime = result.LastEditTime,
                        LastEditUserId = result.LastEditUserId,
                        NoiDung = result.NoiDung,
                        STT = result.STT,
                        STTSapXep = result.STTSapXep,
                        TimeDelete = result.TimeDelete,
                        TimeLog = result.TimeDelete,
                        UserDeleteId = result.UserDeleteId,
                    };

                    if (result.Parent != null)
                        LogTieuChi.ParentId = result.Parent.Id;
                    session.Save(LogTieuChi);
                });
                resultReturn = 1;
            }
            catch (Exception ex)
            {
                resultReturn = 0;
                Helper.ErrorLog("ABC_TieuChiApi/GetMaxDiem", ex); throw ex;
            }
            return resultReturn;
        }
        [Authorize]
        [Route("")]
        public int GetDeleteById(Guid id, Guid userId)
        {
            int result = 0;
            try
            {
                SessionManager.DoWork(session =>
                {
                    List<ABC_DanhGiaChiTiet> ListDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().Where(e => e.TieuChi.Id == id).ToList();
                    if (ListDanhGiaChiTiet.Count > 0)
                    {
                        result = 2;
                    }
                    else
                    {
                        ABC_TieuChi ObjDel = session.Query<ABC_TieuChi>().SingleOrDefault(e => e.Id == id);
                        ObjDel.GCRecord = (int)DateTime.Now.Ticks;
                        ObjDel.TimeDelete = DateTime.Now;
                        ObjDel.UserDeleteId = userId;
                        session.Update(ObjDel);
                        result = 1;
                    }
                });
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
    }
}
