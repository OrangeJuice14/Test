using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection;
using System.ComponentModel;
using System.Data.Linq;
using System.Data;

using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;
using HRMWeb_Business.Predefined;
using HRMWebApp.Helpers;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_QuanLyPhepHe_Factory : BaseFactory<Entities, CC_QuanLyPhepHe>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_QuanLyPhepHe_Factory.New().CreateAloneObject();
        }
        public static CC_QuanLyPhepHe_Factory New()
        {
            return new CC_QuanLyPhepHe_Factory();
        }
        public CC_QuanLyPhepHe_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public bool CheckExistChiTietPhepHe(Guid nienDoTaiChinh, Guid congTy)
        {
            try
            {
                bool result = false;
                CC_QuanLyPhepHe ql = this.ObjectSet.Where(q => q.NienDoTaiChinh == nienDoTaiChinh && q.CongTy == congTy).SingleOrDefault();
                if (ql != null)
                {
                    result = this.Context.CC_ChiTietPhepHe.Any(x => x.QuanLyPhepHe == ql.Oid);
                }
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyPhepHe_Factory/CheckExistChiTietPhepHe", ex);
                throw;
            }
        }
        public CC_QuanLyPhepHe GetByNienDoTaiChinh(Guid nienDoTaiChinh, Guid congTy)
        {
            try
            {
                return this.ObjectSet.Where(x => x.NienDoTaiChinh == nienDoTaiChinh && x.CongTy == congTy).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyPhepHe_Factory/GetByNienDoTaiChinh", ex);
                throw;
            }
        }
        public IQueryable<DTO_QuanLyPhepHe_Find> QuanLyPhepHe_Find(Guid nienDoTaiChinh, Guid boPhanId,Guid nhanVienId, Guid idWebUser, Guid idWebGroup,Guid congTy)
        {
            try
            {
                //Lấy nhóm quyền
                Guid idQuanTriTruong = WebGroupConst.QuanTriTruongID;
                Guid idAdminToanQuyen = WebGroupConst.AdminId;
                Guid idHieuTruong = WebGroupConst.HieuTruongID;
                Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
                Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
                Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
                Guid idTruongPhong = WebGroupConst.TruongPhongID;
                Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
                //
                BoPhan_Factory tmpFactory = BoPhan_Factory.New();
                tmpFactory.Context = this.Context;
                var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(idWebUser, congTy);

                //1. Admin
                if (idQuanTriTruong.Equals(idWebGroup) || idAdminToanQuyen.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietPhepHe
                                  join y in this.Context.ChucDanhs on o.ChucDanh equals y.Oid into ys
                                  from y in ys.DefaultIfEmpty()
                                  where o.CC_QuanLyPhepHe.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && congTy == o.CC_QuanLyPhepHe.CongTy
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyPhepHe_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      NgayVaoCoQuan = o.NgayVaoCoQuan,
                                      SoPhepHe = o.SoPhepHe.ToString(),
                                      ChucDanh = y == null ? "Không có" : y.TenChucDanh
                                  });
                    return result;
                }
                else if (idHoiDongQuanTri.Equals(idWebGroup)
                         || idHoiDongQuanTriUQ.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietPhepHe
                                  join y in this.Context.ChucDanhs on o.ChucDanh equals y.Oid into ys
                                  from y in ys.DefaultIfEmpty()
                                  where o.CC_QuanLyPhepHe.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.CC_QuanLyPhepHe.CongTy)
                                        && congTy == o.CC_QuanLyPhepHe.CongTy
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyPhepHe_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      NgayVaoCoQuan = o.NgayVaoCoQuan,
                                      SoPhepHe = o.SoPhepHe.ToString(),
                                      ChucDanh = y == null ? "Không có" : y.TenChucDanh
                                  });
                    return result;


                }
                else if (idHieuTruong.Equals(idWebGroup)
                         || idHieuTruongUQ.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietPhepHe
                                  join y in this.Context.ChucDanhs on o.ChucDanh equals y.Oid into ys
                                  from y in ys.DefaultIfEmpty()
                                  where o.CC_QuanLyPhepHe.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan)
                                        && congTy == o.CC_QuanLyPhepHe.CongTy
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyPhepHe_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      NgayVaoCoQuan = o.NgayVaoCoQuan,
                                      SoPhepHe = o.SoPhepHe.ToString(),
                                      ChucDanh = y == null ? "Không có" : y.TenChucDanh
                                  });
                    return result;
                }
                else if (idTruongPhong.Equals(idWebGroup)
                         || idTruongPhongUyQuyen.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietPhepHe
                                  join y in this.Context.ChucDanhs on o.ChucDanh equals y.Oid into ys
                                  from y in ys.DefaultIfEmpty()
                                  where o.CC_QuanLyPhepHe.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan)
                                        && congTy == o.CC_QuanLyPhepHe.CongTy
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyPhepHe_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      NgayVaoCoQuan = o.NgayVaoCoQuan,
                                      SoPhepHe = o.SoPhepHe.ToString(),
                                      ChucDanh = y == null ? "Không có" : y.TenChucDanh
                                  });
                    return result;
                }
                else// Cá nhân
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietPhepHe
                                  join y in this.Context.ChucDanhs on o.ChucDanh equals y.Oid into ys
                                  from y in ys.DefaultIfEmpty()
                                  where o.CC_QuanLyPhepHe.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == o.BoPhan)
                                        && (o.ThongTinNhanVien == nhanVienId)
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyPhepHe_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      NgayVaoCoQuan = o.NgayVaoCoQuan,
                                      SoPhepHe = o.SoPhepHe.ToString(),
                                      ChucDanh = y == null ? "Không có" : y.TenChucDanh
                                  });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyPhepHe_Factory/QuanLyPhepHe_Find", ex);
                throw;
            }
        }
        public CC_ChiTietPhepHe GetChiTietPhepHeByOid(Guid oid)
        {
            try
            {
                return this.Context.CC_ChiTietPhepHe.Where(x => x.Oid == oid).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyPhepHe_Factory/GetChiTietPhepHeByOid", ex);
                throw;
            }
        }

        public DTO_QuanLyPhepHe_Find QuanLyPhepHe_ByID(Guid oid)
        {
            try
            {
                //
                var result = (from o in this.Context.CC_ChiTietPhepHe
                              where o.Oid == oid
                              orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan, o.ThongTinNhanVien1.NhanVien.HoSo.HoTen
                              select new DTO_QuanLyPhepHe_Find()
                              {
                                  Oid = o.Oid,
                                  MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                  HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                  TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                  NgayVaoCoQuan = o.NgayVaoCoQuan,
                                  SoPhepHe = o.SoPhepHe.ToString()

                              }).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyPhepHe_Factory/QuanLyPhepHe_ByID", ex);
                throw;
            }
        }

        #endregion
    }//end class
}
