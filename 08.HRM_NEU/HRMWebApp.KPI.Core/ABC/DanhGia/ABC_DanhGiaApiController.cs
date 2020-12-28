using AutoMapper;
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
    public class ABC_DanhGiaApiController : ApiController
    {
        [Authorize]
        [Route("")]
        public ABC_DanhGiaDTO GetDanhGiaByFK(Guid userDuocDanhGiaId, Guid userDanhGiaId, Guid kyDanhGiaId, Guid boTieuChiId, Guid groupDanhGiaId)
        {
            ABC_DanhGiaDTO objDanhGia = new ABC_DanhGiaDTO();
            ABC_DanhGia obj = new ABC_DanhGia();

            if (userDuocDanhGiaId != userDanhGiaId)
            {
                // Khi đồng nghiệp đánh giá thì kiểm tra xem 'userDuocDanhGiaId' tự đánh giá chưa
                ABC_DanhGia ObjTuDanhGia = new ABC_DanhGia();
                SessionManager.DoWork(session =>
                {
                    ObjTuDanhGia = session.Query<ABC_DanhGia>().SingleOrDefault(e => e.UserDanhGia.Id == userDuocDanhGiaId && e.UserDuocDanhGia.Id == userDuocDanhGiaId && e.KyDanhGia.Id == kyDanhGiaId && e.BoTieuChi.Id == boTieuChiId && e.IsLock == true);
                });
                if (ObjTuDanhGia == null)
                    return null;
            }

            SessionManager.DoWork(session =>
            {
                obj = session.Query<ABC_DanhGia>().SingleOrDefault(e => e.UserDanhGia.Id == userDanhGiaId && e.UserDuocDanhGia.Id == userDuocDanhGiaId && e.KyDanhGia.Id == kyDanhGiaId && e.BoTieuChi.Id == boTieuChiId);
                List<ABC_BoTieuChi_Role> ListBoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().Where(e => e.BoTieuChi.Id == boTieuChiId).ToList();
                
                if (obj != null)
                {
                    objDanhGia.Id = obj.Id;
                    objDanhGia.KyDanhGiaId = obj.KyDanhGia.Id;
                    objDanhGia.ThoiGianDanhGia = obj.ThoiGianDanhGia;
                    objDanhGia.IsLock = obj.IsLock;
                    objDanhGia.TongDiem = obj.TongDiem;
                    objDanhGia.UserDanhGiaId = obj.UserDanhGia.Id;
                    objDanhGia.UserDuocDanhGiaId = obj.UserDuocDanhGia.Id;
                    objDanhGia.UserDanhGia_GroupId = obj.UserDanhGia_Group.Id;
                    objDanhGia.UserDanhGia_GroupName = obj.UserDanhGia_Group.Name;
                    objDanhGia.BoTieuChi = obj.BoTieuChi.Map<ABC_BoTieuChiDTO>();
                    ABC_GroupDanhGia GroupDanhGia = session.Query<ABC_GroupDanhGia>().SingleOrDefault(e => e.Id == groupDanhGiaId);

                    objDanhGia.LoaiDanhGia = userDuocDanhGiaId == userDanhGiaId ? "Tự" : GroupDanhGia.Name;
                    objDanhGia.LoaiTuDanhGia = ListBoTieuChiRole[0].GroupTuDanhGia.Name;
                }
            });
            if (objDanhGia.Id == Guid.Empty)
                return InsertDanhGia(userDuocDanhGiaId, userDanhGiaId, kyDanhGiaId, boTieuChiId, groupDanhGiaId);
            return objDanhGia;
        }

        public ABC_DanhGiaDTO InsertDanhGia(Guid userDuocDanhGiaId, Guid userDanhGiaId, Guid kyDanhGiaId, Guid boTieuChiId, Guid groupDanhGiaId)
        {
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_DanhGia obj = new ABC_DanhGia()
                    {
                        Id = Guid.NewGuid(),
                        UserDuocDanhGia = new ABC_User() { Id = userDuocDanhGiaId },
                        UserDanhGia = new ABC_User() { Id = userDanhGiaId },
                        KyDanhGia = new ABC_KyDanhGia() { Id = kyDanhGiaId },
                        BoTieuChi = new ABC_BoTieuChi() { Id = boTieuChiId },
                        TongDiem = 0,
                        UserDanhGia_Group = new ABC_GroupDanhGia() { Id = groupDanhGiaId }
                    };
                    session.Save(obj);
                });
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("ABC_DanhGiaApi/InsertDanhGia ", ex); throw ex;
            }


            return GetDanhGiaByFK(userDuocDanhGiaId, userDanhGiaId, kyDanhGiaId, boTieuChiId, groupDanhGiaId);
        }

        [Authorize]
        [Route("")]
        public ABC_BoTieuChiDTO GetById(Guid id)
        {
            ABC_BoTieuChiDTO result = new ABC_BoTieuChiDTO();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_BoTieuChi a = session.Query<ABC_BoTieuChi>().SingleOrDefault(e => e.Id == id);
                    result = session.Query<ABC_BoTieuChi>().SingleOrDefault(e => e.Id == id)?.Map<ABC_BoTieuChiDTO>();
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DanhGiaApi/GetById ", ex); throw ex; }
            return result;
        }

        [Authorize]
        [Route("")]
        public string GetCapBacChuaDanhGia(Guid danhGiaId)
        {
            string result = "Cấp dưới";
            List<ABC_DanhGia> ListDanhGia = new List<ABC_DanhGia>();
            try
            {
                SessionManager.DoWork(session =>
                {

                    ABC_DanhGia ObjDanhGia = session.Query<ABC_DanhGia>().SingleOrDefault(e => e.Id == danhGiaId);
                    ListDanhGia = session.Query<ABC_DanhGia>().Where(e => e.Id != danhGiaId &&
                                                                        e.BoTieuChi.Id == ObjDanhGia.BoTieuChi.Id &&
                                                                        e.KyDanhGia.Id == ObjDanhGia.KyDanhGia.Id).
                                                                OrderByDescending(e => e.UserDanhGia_Group.STT).
                                                                ToList();
                    if (ListDanhGia != null)
                        foreach (ABC_DanhGia item in ListDanhGia)
                        {
                            if (item.IsLock != true)
                            {
                                result = item.UserDanhGia_Group.Name;
                                break;
                            }
                        }
                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DanhGiaApi/GetCapBacChuaDanhGia", ex); throw ex; }

            return result;
        }
        [Authorize]
        [Route("")]
        public int PutSaveOrUpdate(ABC_DanhGiaDTO obj)
        {
            int result = 0;
            DateTime TimeNow = DateTime.Now;
            try
            {
                ABC_DanhGia ObjSave = new ABC_DanhGia();
                SessionManager.DoWork(session =>
                {
                    ObjSave.Id = obj.Id == Guid.Empty ? Guid.NewGuid() : obj.Id;
                    ObjSave.BoTieuChi = new ABC_BoTieuChi() { Id = obj.BoTieuChi.Id };
                    ObjSave.KyDanhGia = new ABC_KyDanhGia() { Id = obj.KyDanhGiaId.Value };
                    ObjSave.ThoiGianDanhGia = TimeNow;
                    ObjSave.TongDiem = obj.TongDiem;
                    ObjSave.IsLock = obj.IsLock;
                    ObjSave.UserDanhGia = new ABC_User() { Id = obj.UserDanhGiaId.Value };
                    ObjSave.UserDuocDanhGia = new ABC_User() { Id = obj.UserDuocDanhGiaId.Value };
                    ObjSave.UserDanhGia_Group = new ABC_GroupDanhGia() { Id = obj.UserDanhGia_GroupId.Value };
                    session.SaveOrUpdate(ObjSave);

                    //write Log DanhGia 
                    ABC_LogDanhGia logDanhGia = new ABC_LogDanhGia()
                    {
                        Id = Guid.NewGuid(),
                        BoTieuChi = ObjSave.BoTieuChi,
                        DanhGia = new ABC_DanhGia() { Id = ObjSave.Id },
                        IsLock = ObjSave.IsLock,
                        KyDanhGia = ObjSave.KyDanhGia,
                        ThoiGianDanhGia = ObjSave.ThoiGianDanhGia,
                        TimeLog = TimeNow,
                        TongDiem = ObjSave.TongDiem,
                        UserDanhGia = ObjSave.UserDanhGia,
                        UserDanhGia_Group = ObjSave.UserDanhGia_Group,
                        UserDuocDanhGia = ObjSave.UserDuocDanhGia
                    };
                    session.Save(logDanhGia);
                    
                });
                result = 1;
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DanhGiaApi/PutSaveOrUpdate", ex); throw ex; }

            return result;
        }
        [Authorize]
        [Route("")]
        public IEnumerable<ABC_DanhGiaDTO> GetListKetQuaDanhGia(Guid danhGiaId)
        {
            Mapper.CreateMap<ABC_DanhGia, ABC_DanhGiaDTO>()
                .ForMember(dest => dest.UserDanhGia_GroupId, opt => opt.MapFrom(src => src.UserDanhGia_Group.Id))
                .ForMember(dest => dest.UserDanhGia_GroupName, opt => opt.MapFrom(src => src.UserDanhGia_Group.Name));
            List<ABC_DanhGiaDTO> result = new List<ABC_DanhGiaDTO>();
            try
            {
                SessionManager.DoWork(session =>
                {
                    ABC_DanhGia ObjDanhGia = session.Query<ABC_DanhGia>().SingleOrDefault(e => e.Id == danhGiaId);
                    List<ABC_BoTieuChi_Role> BoTieuChiRole = session.Query<ABC_BoTieuChi_Role>().
                                                                    Where(e => e.BoTieuChi.Id == ObjDanhGia.BoTieuChi.Id).ToList();

                    List<ABC_DanhGia> ListDanhGia = session.Query<ABC_DanhGia>().
                                                            Where(e => e.Id != danhGiaId &&
                                                                        e.IsLock == true &&
                                                                        e.BoTieuChi.Id == ObjDanhGia.BoTieuChi.Id &&
                                                                        e.KyDanhGia.Id == ObjDanhGia.KyDanhGia.Id &&
                                                                        e.UserDuocDanhGia.Id == ObjDanhGia.UserDuocDanhGia.Id).
                                                            OrderByDescending(e => e.UserDanhGia_Group.STT).
                                                            ToList();
                    if (BoTieuChiRole.FirstOrDefault().UserDanhGiaNgangHang != true && ObjDanhGia.IsLock != true)
                    {
                        BoTieuChiRole = BoTieuChiRole.Where(e => e.GroupDanhGia.STT > ObjDanhGia.UserDanhGia_Group.STT).
                                                                    OrderByDescending(e => e.GroupDanhGia.STT).ToList();
                        ListDanhGia = ListDanhGia.Where(e => e.UserDanhGia_Group.STT > ObjDanhGia.UserDanhGia_Group.STT).ToList();
                        if (ListDanhGia.Count == BoTieuChiRole.Count + 1)
                        {
                            foreach (ABC_DanhGia item in ListDanhGia)
                            {
                                ABC_DanhGiaDTO obj = item.Map<ABC_DanhGiaDTO>();
                                List<ABC_DanhGiaChiTiet> ListDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().
                                                                                    Where(e => e.DanhGia.Id == obj.Id).
                                                                                    OrderBy(e => e.TieuChi.STT).ThenBy(e => e.TieuChi.STTSapXep).
                                                                                    ToList();
                                for (int i = 0; i < ListDanhGiaChiTiet.Count; i++)
                                {
                                    ABC_DanhGiaChiTietDTO ObjDanhGiaChiTiet = ListDanhGiaChiTiet[i].Map<ABC_DanhGiaChiTietDTO>();
                                    ObjDanhGiaChiTiet.NoChild = ListDanhGiaChiTiet[i].TieuChi.Childrens.Count == 0;
                                    if (obj.DanhGiaChiTiet == null)
                                    {
                                        obj.DanhGiaChiTiet = new List<ABC_DanhGiaChiTietDTO>();
                                    }
                                    obj.DanhGiaChiTiet.Add(ObjDanhGiaChiTiet);
                                }
                                obj.LoaiDanhGia = obj.UserDanhGiaId != obj.UserDuocDanhGiaId ? obj.UserDanhGia_GroupName : "Tự";
                                result.Add(obj);
                            }
                        }
                    }
                    else
                    {
                        foreach (ABC_DanhGia item in ListDanhGia)
                        {
                            ABC_DanhGiaDTO obj = item.Map<ABC_DanhGiaDTO>();
                            List<ABC_DanhGiaChiTiet> ListDanhGiaChiTiet = session.Query<ABC_DanhGiaChiTiet>().
                                                                                Where(e => e.DanhGia.Id == obj.Id).
                                                                                OrderBy(e => e.TieuChi.STT).ThenBy(e => e.TieuChi.STTSapXep).
                                                                                ToList();
                            for (int i = 0; i < ListDanhGiaChiTiet.Count; i++)
                            {
                                ABC_DanhGiaChiTietDTO ObjDanhGiaChiTiet = ListDanhGiaChiTiet[i].Map<ABC_DanhGiaChiTietDTO>();
                                ObjDanhGiaChiTiet.NoChild = ListDanhGiaChiTiet[i].TieuChi.Childrens.Count == 0;
                                if (obj.DanhGiaChiTiet == null)
                                {
                                    obj.DanhGiaChiTiet = new List<ABC_DanhGiaChiTietDTO>();
                                }
                                obj.DanhGiaChiTiet.Add(ObjDanhGiaChiTiet);
                            }
                            obj.LoaiDanhGia = obj.UserDanhGiaId != obj.UserDuocDanhGiaId ? obj.UserDanhGia_GroupName : "Tự";
                            result.Add(obj);
                        }
                    }

                });
            }
            catch (Exception ex) { Helper.ErrorLog("ABC_DanhGiaApi/GetListKetQuaDanhGia", ex); throw ex; }
            return result;
        }
    }
}
