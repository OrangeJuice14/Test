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
    public class CC_QuanLyNghiPhep_Factory : BaseFactory<Entities, CC_QuanLyNghiPhep>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_QuanLyNghiPhep_Factory.New().CreateAloneObject();
        }
        public static CC_QuanLyNghiPhep_Factory New()
        {
            return new CC_QuanLyNghiPhep_Factory();
        }
        public CC_QuanLyNghiPhep_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        //public bool ExistsByNamHoc(Guid namhoc)
        //{
        //    return this.ObjectSet.Any(x => x.NamHoc == namhoc);
        //}
        //public CC_QuanLyNghiPhep GetByNamHoc(Guid namhoc,Guid congTy)
        //{
        //    return this.ObjectSet.Where(x => x.NamHoc == namhoc && x.CongTy== congTy).SingleOrDefault();
        //}
        //public CC_ChiTietNghiPhep GetChiTietNghiPhepByNamAndIDNV(Guid namhoc, Guid? IDNV)
        //{
        //    var result = (from o in this.Context.CC_ChiTietNghiPhep
        //                  where o.ThongTinNhanVien == IDNV && o.CC_QuanLyNghiPhep.NamHoc == namhoc
        //                  select o
        //                ).SingleOrDefault();
        //    return result;
        //}

        public bool CheckExistChiTietNghiPhep(Guid nienDoTaiChinh, Guid congTy)
        {
            try
            {
                bool result = false;
                CC_QuanLyNghiPhep ql = this.ObjectSet.Where(q => q.NienDoTaiChinh == nienDoTaiChinh && q.CongTy == congTy).SingleOrDefault();
                if (ql != null)
                {
                    result = this.Context.CC_ChiTietNghiPhep.Any(x => x.QuanLyNghiPhep == ql.Oid);
                }
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyNghiPhep_Factory/CheckExistChiTietNghiPhep", ex);
                throw;
            }
        }
        public CC_QuanLyNghiPhep GetByNienDoTaiChinh(Guid nienDoTaiChinh, Guid congTy)
        {
            try
            {
                return this.ObjectSet.Where(x => x.NienDoTaiChinh == nienDoTaiChinh && x.CongTy == congTy).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyNghiPhep_Factory/GetByNienDoTaiChinh", ex);
                throw;
            }
        }
        public IQueryable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhepNam_Find(Guid nienDoTaiChinh, Guid boPhanId, Guid nhanVienId, Guid idWebUser, Guid idWebGroup, Guid congTy)
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
                    var result = (from o in this.Context.CC_ChiTietNghiPhep
                                  where o.CC_QuanLyNghiPhep.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && congTy == o.CC_QuanLyNghiPhep.CongTy
                                        && o.ThongTinNhanVien1.NhanVien.HoSo.GCRecord == null
                                        && o.ThongTinNhanVien1.NhanVien.TinhTrang1.DaNghiViec == false
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyNghiPhep_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      TongSoNgayPhep = o.TongSoNgayPhep.ToString(),
                                      SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString(),
                                      SoNgayPhepConLai = o.SoNgayPhepConLai.ToString(),
                                      SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString(),
                                      SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString(),
                                      SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString(),
                                      SoNgayPhepDaNghi_QuiI = o.SoNgayPhepDaNghi_QuiI.ToString(),
                                      SoNgayTamUngHienTai = o.SoNgayTamUngHienTai.ToString(),
                                      SoNgayTamUngNamTruoc = o.SoNgayTamUngNamTruoc.ToString(),
                                      TongSoNgayBu = o.TongSoNgayBu.ToString(),
                                      SoNgayBuDaNghi = o.SoNgayBuDaNghi.ToString(),
                                      SoNgayBuConLai = o.SoNgayBuConLai.ToString(),
                                      SoNgayBuNamHienTai = o.SoNgayBuNamHienTai.ToString(),
                                      SoNgayBuNamTruoc = o.SoNgayBuNamTruoc.ToString()

                                  });
                    return result;
                }
                else if (idHoiDongQuanTri.Equals(idWebGroup)
                         || idHoiDongQuanTriUQ.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietNghiPhep
                                  where o.CC_QuanLyNghiPhep.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.CC_QuanLyNghiPhep.CongTy)
                                        && congTy == o.CC_QuanLyNghiPhep.CongTy
                                        && o.ThongTinNhanVien1.NhanVien.HoSo.GCRecord == null
                                        && o.ThongTinNhanVien1.NhanVien.TinhTrang1.DaNghiViec == false
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyNghiPhep_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      TongSoNgayPhep = o.TongSoNgayPhep.ToString(),
                                      SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString(),
                                      SoNgayPhepConLai = o.SoNgayPhepConLai.ToString(),
                                      SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString(),
                                      SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString(),
                                      SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString(),
                                      SoNgayPhepDaNghi_QuiI = o.SoNgayPhepDaNghi_QuiI.ToString(),
                                      SoNgayTamUngHienTai = o.SoNgayTamUngHienTai.ToString(),
                                      SoNgayTamUngNamTruoc = o.SoNgayTamUngNamTruoc.ToString(),
                                      TongSoNgayBu = o.TongSoNgayBu.ToString(),
                                      SoNgayBuDaNghi = o.SoNgayBuDaNghi.ToString(),
                                      SoNgayBuConLai = o.SoNgayBuConLai.ToString(),
                                      SoNgayBuNamHienTai = o.SoNgayBuNamHienTai.ToString(),
                                      SoNgayBuNamTruoc = o.SoNgayBuNamTruoc.ToString()

                                  });
                    return result;


                }
                else if (idHieuTruong.Equals(idWebGroup)
                         || idHieuTruongUQ.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietNghiPhep
                                  where o.CC_QuanLyNghiPhep.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan)
                                        && congTy == o.CC_QuanLyNghiPhep.CongTy
                                        && o.ThongTinNhanVien1.NhanVien.HoSo.GCRecord == null
                                        && o.ThongTinNhanVien1.NhanVien.TinhTrang1.DaNghiViec == false
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyNghiPhep_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      TongSoNgayPhep = o.TongSoNgayPhep.ToString(),
                                      SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString(),
                                      SoNgayPhepConLai = o.SoNgayPhepConLai.ToString(),
                                      SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString(),
                                      SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString(),
                                      SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString(),
                                      SoNgayPhepDaNghi_QuiI = o.SoNgayPhepDaNghi_QuiI.ToString(),
                                      SoNgayTamUngHienTai = o.SoNgayTamUngHienTai.ToString(),
                                      SoNgayTamUngNamTruoc = o.SoNgayTamUngNamTruoc.ToString(),
                                      TongSoNgayBu = o.TongSoNgayBu.ToString(),
                                      SoNgayBuDaNghi = o.SoNgayBuDaNghi.ToString(),
                                      SoNgayBuConLai = o.SoNgayBuConLai.ToString(),
                                      SoNgayBuNamHienTai = o.SoNgayBuNamHienTai.ToString(),
                                      SoNgayBuNamTruoc = o.SoNgayBuNamTruoc.ToString()

                                  });
                    return result;
                }
                else if (idTruongPhong.Equals(idWebGroup)
                         || idTruongPhongUyQuyen.Equals(idWebGroup))
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietNghiPhep
                                  where o.CC_QuanLyNghiPhep.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == Guid.Empty || o.BoPhan == boPhanId || o.BoPhan1.BoPhanCha == boPhanId)
                                        && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan)
                                        && congTy == o.CC_QuanLyNghiPhep.CongTy
                                        && o.ThongTinNhanVien1.NhanVien.HoSo.GCRecord == null
                                        && o.ThongTinNhanVien1.NhanVien.TinhTrang1.DaNghiViec == false
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyNghiPhep_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      TongSoNgayPhep = o.TongSoNgayPhep.ToString(),
                                      SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString(),
                                      SoNgayPhepConLai = o.SoNgayPhepConLai.ToString(),
                                      SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString(),
                                      SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString(),
                                      SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString(),
                                      SoNgayPhepDaNghi_QuiI = o.SoNgayPhepDaNghi_QuiI.ToString(),
                                      SoNgayTamUngHienTai = o.SoNgayTamUngHienTai.ToString(),
                                      SoNgayTamUngNamTruoc = o.SoNgayTamUngNamTruoc.ToString(),
                                      TongSoNgayBu = o.TongSoNgayBu.ToString(),
                                      SoNgayBuDaNghi = o.SoNgayBuDaNghi.ToString(),
                                      SoNgayBuConLai = o.SoNgayBuConLai.ToString(),
                                      SoNgayBuNamHienTai = o.SoNgayBuNamHienTai.ToString(),
                                      SoNgayBuNamTruoc = o.SoNgayBuNamTruoc.ToString()

                                  });
                    return result;
                }
                else// Cá nhân
                {
                    //
                    var result = (from o in this.Context.CC_ChiTietNghiPhep
                                  where o.CC_QuanLyNghiPhep.NienDoTaiChinh == nienDoTaiChinh
                                        && (boPhanId == o.BoPhan)
                                        && (o.ThongTinNhanVien == nhanVienId)
                                  orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                                  select new DTO_QuanLyNghiPhep_Find()
                                  {
                                      Oid = o.Oid,
                                      MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                      HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                      TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                      TongSoNgayPhep = o.TongSoNgayPhep.ToString(),
                                      SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString(),
                                      SoNgayPhepConLai = o.SoNgayPhepConLai.ToString(),
                                      SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString(),
                                      SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString(),
                                      SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString(),
                                      SoNgayPhepDaNghi_QuiI = o.SoNgayPhepDaNghi_QuiI.ToString(),
                                      SoNgayTamUngHienTai = o.SoNgayTamUngHienTai.ToString(),
                                      SoNgayTamUngNamTruoc = o.SoNgayTamUngNamTruoc.ToString(),
                                      TongSoNgayBu = o.TongSoNgayBu.ToString(),
                                      SoNgayBuDaNghi = o.SoNgayBuDaNghi.ToString(),
                                      SoNgayBuConLai = o.SoNgayBuConLai.ToString(),
                                      SoNgayBuNamHienTai = o.SoNgayBuNamHienTai.ToString(),
                                      SoNgayBuNamTruoc = o.SoNgayBuNamTruoc.ToString()

                                  });
                    return result;
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyNghiPhep_Factory/QuanLyNghiPhepNam_Find", ex);
                throw;
            }
        }
        public CC_ChiTietNghiPhep GetChiTietNghiPhepByOid(Guid oid)
        {
            try
            {
                return this.Context.CC_ChiTietNghiPhep.Where(x => x.Oid == oid).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyNghiPhep_Factory/GetChiTietNghiPhepByOid", ex);
                throw;
            }
        }

        public DTO_QuanLyNghiPhep_Find QuanLyNghiPhepNam_ByID(Guid oid)
        {
            try
            {
                //
                var result = (from o in this.Context.CC_ChiTietNghiPhep
                              where o.Oid == oid
                              orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan, o.ThongTinNhanVien1.NhanVien.HoSo.HoTen
                              select new DTO_QuanLyNghiPhep_Find()
                              {
                                  Oid = o.Oid,
                                  MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                                  HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                  TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                                  TongSoNgayPhep = o.TongSoNgayPhep.ToString().Trim(),
                                  SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString().Trim(),
                                  SoNgayPhepConLai = o.SoNgayPhepConLai.ToString().Trim(),
                                  SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString().Trim(),
                                  SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString().Trim(),
                                  SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString().Trim(),
                                  SoNgayPhepDaNghi_QuiI = o.SoNgayPhepDaNghi_QuiI.ToString().Trim(),
                                  SoNgayTamUngHienTai = o.SoNgayTamUngHienTai.ToString(),
                                  SoNgayTamUngNamTruoc = o.SoNgayTamUngNamTruoc.ToString(),
                                  TongSoNgayBu = o.TongSoNgayBu.ToString(),
                                  SoNgayBuDaNghi = o.SoNgayBuDaNghi.ToString(),
                                  SoNgayBuConLai = o.SoNgayBuConLai.ToString(),
                                  SoNgayBuNamHienTai = o.SoNgayBuNamHienTai.ToString(),
                                  SoNgayBuNamTruoc = o.SoNgayBuNamTruoc.ToString()

                              }).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_QuanLyNghiPhep_Factory/QuanLyNghiPhepNam_ByID", ex);
                throw;
            }
        }

        #endregion
    }//end class
}
