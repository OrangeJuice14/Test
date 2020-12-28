using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
using System.Web.Configuration;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class BoPhan_Factory : BaseFactory<Entities, BoPhan>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return BoPhan_Factory.New().CreateAloneObject();
        }
        public static BoPhan_Factory New()
        {
            return new BoPhan_Factory();
        }
        public BoPhan_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public IQueryable<BoPhan> GetAll_GCRecordIsNull()
        {
            //Danh sách phòng ban
            var resultList = (from o in this.Context.BoPhans
                              where o.GCRecord == null
                                    && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                    && (o.NgungHoatDong ?? false ) == false
                                    && (o.KhongTinhLuong ?? false) == false
                              orderby o.STT ascending, o.TenBoPhan ascending
                              select o);
            //
            return resultList.OrderBy(x => x.STT);
        }

        public IQueryable<BoPhan> GetBoPhanAll_GCRecordIsNull()
        {
            //
            var result = from o in this.ObjectSet
                         where o.GCRecord == null 
                               && o.BoPhanCha != null
                               && (o.NgungHoatDong ?? false) == false
                               && (o.KhongTinhLuong ?? false) == false
                         orderby o.STT ascending, o.TenBoPhan ascending
                         select o;
            return result;
        }
        public List<BoPhan> GetChildByParentID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.BoPhanCha == oid
                                && (o.NgungHoatDong ?? false) == false
                                && (o.KhongTinhLuong ?? false) == false
                                && o.GCRecord == null
                          select o).ToList();
            return result;
        }
        public List<BoPhan> GetDanhSachBoPhanTheoCongTy(Guid congTy)
        {
            var result = (from o in this.ObjectSet
                          where o.BoPhanCha == congTy
                                && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                && (o.NgungHoatDong ?? false) == false
                                && (o.KhongTinhLuong ?? false) == false
                                && o.GCRecord == null
                          select o).ToList();
            return result;
        }
        public BoPhan GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                                && (o.NgungHoatDong ?? false) == false
                                && (o.KhongTinhLuong ?? false) == false
                                && o.GCRecord == null
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<BoPhan> GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(Guid webUserId,Guid congTy)
        {
            string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
            string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
            string idHoiDongQT = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
            string idHoiDongQTUQ = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            string idTruongBoPhan = WebGroupConst.TruongBoPhanID.ToString().ToUpper();
            //
            DTO_WebUser user = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (user.WebGroupID.ToString().ToUpper().Equals(idAdminToanQuyen)) // Admin thì lấy hết
            {
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.PhongBan && o.GCRecord == null
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                return resultList;
            }
            else if (user.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan)) //Cá nhân
            {
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.PhongBan && o.GCRecord == null
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && user.BoPhanId.ToString().ToUpper().Equals(o.Oid.ToString().ToUpper())
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                return resultList;
            }
            else //Các nhóm còn lại
            {
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTy
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                                        && o.GCRecord == null
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                return resultList;
            }
        }

        /// <summary>
        /// lấy danh sách bộ phận theo trường để lọc các đơn vị trong trường ở các menu chấm công ngày nghỉ, công tác,...
        /// </summary>
        /// <param name="webUserId"></param>
        /// <param name="congTyId"></param>
        /// <returns></returns>
        public IQueryable<BoPhan> GetDanhSachBoPhan_DuocPhanQuyenChoWebUserIdAndCompany_GCRecordIsNull(Guid webUserId, Guid congTyId)
        {
            //
            string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
            string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
            string idHoiDongQT = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
            string idHoiDongQTUQ = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            string idTruongBoPhan = WebGroupConst.TruongBoPhanID.ToString().ToUpper();
            //
            DTO_WebUser user = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (user.WebGroupID.ToString().ToUpper().Equals(idAdminToanQuyen)) // Admin thì lấy hết
            {
                var resultList = (from o in this.Context.BoPhans
                                  where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTyId
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                return resultList;
            }
            else if (user.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan)) //Cá nhân
            {
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTyId
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && user.BoPhanId == o.Oid
                                        && o.GCRecord == null
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                return resultList;
            }
            else // Tất cả các nhóm còn lại đều phải phân quyền tới bộ phận
            {
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTyId
                                        && o.GCRecord == null
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                if (resultList.Count() == 0)
                {
                    resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTyId
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && user.BoPhanId == o.Oid
                                        && o.GCRecord == null
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                }
                return resultList;
            }
        }


        public IQueryable<BoPhan> GetDanhSachTruong_DuocPhanQuyenChoWebUserId_GCRecordIsNull(Guid webUserId)
        {
            //
            string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
            string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
            string idHoiDongQT = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
            string idHoiDongQTUQ = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            //
            DTO_WebUser user = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (user.WebGroupID.ToString().ToUpper().Equals(idAdminToanQuyen)) // Admin thì lấy hết
            {
                //
                var resultList = (from o in this.Context.BoPhans
                                  where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                return resultList.OrderBy(x => x.STT);
            }
            else
            {
                //Cá nhân
                if (user.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan))
                {
                    //
                    var resultList = (from o in this.Context.BoPhans
                                      where o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                            && (o.NgungHoatDong ?? false) == false
                                            && (o.KhongTinhLuong ?? false) == false
                                            && user.BoPhanId.ToString().ToUpper().Equals(o.Oid.ToString().ToUpper())
                                            && o.GCRecord == null
                                      orderby o.STT ascending, o.TenBoPhan ascending
                                      select o);
                    //
                    return resultList.OrderBy(x => x.STT);
                }
                // Hội đồng quản trị
                else if (user.WebGroupID.ToString().ToUpper().Equals(idHoiDongQT)
                         || user.WebGroupID.ToString().ToUpper().Equals(idHoiDongQTUQ))
                {
                    //
                    var resultList = (from o in this.Context.BoPhans
                                      where o.GCRecord == null
                                            && o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                            && (o.NgungHoatDong ?? false) == false
                                            && (o.KhongTinhLuong ?? false) == false
                                            && o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                                      orderby o.STT ascending, o.TenBoPhan ascending
                                      select o);
                    //
                    return resultList.OrderBy(x => x.STT);
                }
                // Hiệu trưởng
                else if (user.WebGroupID.ToString().ToUpper().Equals(idHieuTruong)
                         || user.WebGroupID.ToString().ToUpper().Equals(idHieuTruongUQ))
                {
                    //
                    var resultList = (from o in this.Context.BoPhans
                                      where o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                            && o.GCRecord == null
                                            && (o.NgungHoatDong ?? false) == false
                                            && (o.KhongTinhLuong ?? false) == false
                                            && o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                                      orderby o.STT ascending, o.TenBoPhan ascending
                                      select o);
                    //
                    return resultList.OrderBy(x => x.STT);
                }
                // Trưởng phòng
                else if (user.WebGroupID.ToString().ToUpper().Equals(idTruongPhong)
                         || user.WebGroupID.ToString().ToUpper().Equals(idTruongPhongUQ))
                {
                    //
                    var resultList = (from o in this.Context.BoPhans
                                      where o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                            && o.GCRecord == null
                                            && (o.NgungHoatDong ?? false) == false
                                            && (o.KhongTinhLuong ?? false) == false
                                            && o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                                      orderby o.STT ascending, o.TenBoPhan ascending
                                      select o);
                    //
                    return resultList.OrderBy(x => x.STT);
                }
                //Quản trị Trường
                else
                {
                    //
                    var resultList = (from o in this.Context.BoPhans
                                      where o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                            && o.GCRecord == null
                                            && (o.NgungHoatDong ?? false) == false
                                            && (o.KhongTinhLuong ?? false) == false
                                      orderby o.STT ascending, o.TenBoPhan ascending
                                      select o);
                    //
                    return resultList.OrderBy(x => x.STT);
                }
                //              
            }
        }

        /// <summary>
        /// lấy danh sách trường để lọc các đơn vị trong trường ở các menu chấm công ngày nghỉ, công tác,...
        /// </summary>
        /// <param name="webUserId"></param>
        /// <returns></returns>
        public IQueryable<BoPhan> GetDanhSachTruong_DuocPhanQuyenChoWebUserId_New(Guid webUserId)
        {
            //
            string idAdminToanQuyen = WebGroupConst.AdminId.ToString().ToUpper();
            string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
            string idHoiDongQT = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
            string idHoiDongQTUQ = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
            //
            DTO_WebUser user = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (user.WebGroupID.ToString().ToUpper().Equals(idAdminToanQuyen)) // Admin thì lấy hết
            {
                //
                var resultList = (from o in this.Context.BoPhans
                                  where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                return resultList.OrderBy(x => x.STT);
            }
            else if (user.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan))  //Cá nhân
            {
                //
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && user.CongTyId == o.Oid
                                        && o.GCRecord == null
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                return resultList.OrderBy(x => x.STT);
            }
            else // Các nhóm còn lại (trưởng phòng, hiệu trưởng, hội đồng quản trị, quản trị trường - có thể quản lý 2 trường)
            {
                //
                var resultList = (from o in this.Context.BoPhans
                                  where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && o.WebUser_BoPhan.Any(x => x.IDWebUser == webUserId)
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                if (resultList.Count() == 0)
                {
                    resultList = from o in this.Context.BoPhans
                                 where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                        && user.CongTyId == o.Oid
                                 orderby o.STT ascending, o.TenBoPhan ascending
                                 select o;
                }
                return resultList.OrderBy(x => x.STT);
            }
            //
        }

        public IQueryable<BoPhan> BoPhan_GetLoaiBoPhanByWebGroup(Guid webgroupid, Guid congTy)
        {
            //
            string idQuanTriKhoi = WebGroupConst.QuanTriKhoiID.ToString().ToUpper();
            string idHoiDongQT = WebGroupConst.HoiDongQuanTriID.ToString().ToUpper();
            string idHoiDongQTUQ = WebGroupConst.HoiDongQuanTriUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
            string idTruongBoPhan = WebGroupConst.TruongBoPhanID.ToString().ToUpper();

            //Quản trị trường,Hiệu trưởng,Trương phòng
            if (webgroupid.ToString().ToUpper().Equals(idQuanTriTruong)
                || webgroupid.ToString().ToUpper().Equals(idHieuTruong)
                || webgroupid.ToString().ToUpper().Equals(idHieuTruongUQ)
                || webgroupid.ToString().ToUpper().Equals(idTruongPhong)
                || webgroupid.ToString().ToUpper().Equals(idTruongPhongUQ)
                || webgroupid.ToString().ToUpper().Equals(idTruongBoPhan))
            {
                //
                var resultList = (from o in this.Context.BoPhans
                                  where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTy
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                return resultList.OrderBy(x => x.STT);
            }
            //
            else if (webgroupid.ToString().ToUpper().Equals(idHoiDongQT)
                || webgroupid.ToString().ToUpper().Equals(idHoiDongQTUQ)
                || webgroupid.ToString().ToUpper().Equals(idQuanTriKhoi))
            {
                //Danh sách phòng ban
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == LoaiBoPhanConst.Truong
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                return resultList.OrderBy(x => x.STT);
            }
            else
            {
                //Danh sách phòng ban trống
                var resultList = (from o in this.Context.BoPhans
                                  where o.LoaiBoPhan == 10 ///Chủ yếu trả về trống
                                        && (o.NgungHoatDong ?? false) == false
                                        && (o.KhongTinhLuong ?? false) == false
                                  orderby o.STT ascending, o.TenBoPhan ascending
                                  select o);
                //
                return resultList.OrderBy(x => x.STT);
            }
        }

        public IQueryable<BoPhan> BoPhan_LayTatCaBoPhan()
        {
            var resultList = (from o in this.ObjectSet
                              where o.GCRecord == null
                                    && (o.NgungHoatDong ?? false) == false
                                    && (o.KhongTinhLuong ?? false) == false
                              orderby o.STT ascending, o.TenBoPhan ascending
                              select o);
            //
            return resultList;
        }

        private bool GetBoPhanInCC_ChiTietChamCongNhanVien(int thang, int nam, Guid idBoPhan)
        {
            var result = (from x in this.Context.CC_ChiTietChamCong
                          where x.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang
                                && x.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                && x.BoPhan.Value == idBoPhan
                          select true).FirstOrDefault();
            return result;
        }

        public IEnumerable<DTO_KiemTraPhongBanXetABC> KiemTraPhongBanXetABC_Find(int thang, int nam, Boolean? daXetXongABC,Guid congTy)
        {
            Boolean tatCaTrangThaiXetABC = daXetXongABC == null ? true : false;
            //
            IEnumerable<DTO_KiemTraPhongBanXetABC> query = null;
            //
            List<Guid> boPhandaXet_List = (from o in this.Context.BoPhans
                                           join x in this.Context.CC_ChiTietChamCong on o.Oid equals x.BoPhan
                                           where o.GCRecord == null
                                                  && (o.NgungHoatDong ?? false) == false
                                                  && x.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang
                                                  && x.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                                  && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                                  && o.CongTy == congTy
                                           select o.Oid).Distinct().ToList();
            //
            List<Guid> boPhanChuaXet_List = (from o in this.Context.BoPhans
                                             where o.GCRecord == null
                                                     && !boPhandaXet_List.Contains(o.Oid)
                                                     && (o.NgungHoatDong ?? false) == false
                                                     && (o.KhongTinhLuong ?? false) == false
                                                     && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                                     && o.CongTy == congTy
                                             select o.Oid).Distinct().ToList();
            //
            if (!tatCaTrangThaiXetABC)
            {
                if (daXetXongABC.Value)
                {
                    query = (from o in this.Context.BoPhans
                             let cCount =
                             (
                                from c in Context.CC_ChiTietChamCong
                                where c.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang
                                      && c.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                      && c.BoPhan == o.Oid
                                select c
                             ).Count()
                             where o.GCRecord == null
                                   && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                   && o.CongTy == congTy
                                   && boPhandaXet_List.Contains(o.Oid)
                             select new DTO_KiemTraPhongBanXetABC() { STT = o.STT.Value, TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Hoàn thành" });
                }
                //
                else if (!daXetXongABC.Value)
                {
                    query = (from o in this.Context.BoPhans
                             let cCount =
                             (
                                from c in Context.CC_ChiTietChamCong
                                where c.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang
                                      && c.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                      && c.BoPhan == o.Oid
                                select c
                             ).Count()
                             where o.GCRecord == null
                                   && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                   && o.CongTy == congTy
                                   && !boPhandaXet_List.Contains(o.Oid)
                             select new DTO_KiemTraPhongBanXetABC() { STT = o.STT.Value, TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Chưa hoàn thành" });
                }
            }
            else
            {
                var queryDaXet = (from o in this.Context.BoPhans
                                  let cCount =
                                  (
                                     from c in Context.CC_ChiTietChamCong
                                     where c.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang
                                           && c.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                           && c.BoPhan == o.Oid
                                     select c
                                  ).Count()
                                  where o.GCRecord == null
                                        && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                        && o.CongTy == congTy
                                        && boPhandaXet_List.Contains(o.Oid)
                                  select new DTO_KiemTraPhongBanXetABC() { STT = o.STT.Value, TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Hoàn thành" });

                var queryChuaXet = (from o in this.Context.BoPhans
                                    let cCount =
                                    (
                                       from c in Context.CC_ChiTietChamCong
                                       where c.CC_QuanLyChamCong.CC_KyChamCong.Thang == thang
                                             && c.CC_QuanLyChamCong.CC_KyChamCong.Nam == nam
                                             && c.BoPhan == o.Oid
                                       select c
                                    ).Count()
                                    where o.GCRecord == null
                                          && !boPhandaXet_List.Contains(o.Oid)
                                          && o.LoaiBoPhan == LoaiBoPhanConst.PhongBan
                                          && o.CongTy == congTy
                                    select new DTO_KiemTraPhongBanXetABC() { STT = o.STT.Value, TenPhongBan = o.TenBoPhan, ThangNam = thang + "/" + nam, TrangThai = "Chưa hoàn thành" });
                //
                query = queryDaXet.Union(queryChuaXet);
            }
            //
            return query.OrderBy(x => x.STT);
        }

        #endregion
    }//end class
}
