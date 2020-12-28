using System;
using System.Collections.Generic;
//using System.Data.Common.CommandTrees;
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
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_ChamCongNgoaiGio_Factory : BaseFactory<Entities, CC_ChamCongNgoaiGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChamCongNgoaiGio_Factory.New().CreateAloneObject();
        }
        public static CC_ChamCongNgoaiGio_Factory New()
        {
            return new CC_ChamCongNgoaiGio_Factory();
        }
        public CC_ChamCongNgoaiGio_Factory()
            : base(Database.NewEntities())
        {

        }
        public IQueryable<CC_ChamCongNgoaiGio> GetAll_GCRecordIsNull()
        {
            IOrderedQueryable<CC_ChamCongNgoaiGio> result = from o in this.ObjectSet
                                                            orderby o.Ngay
                                                            select o;
            return result;
        }
        public IEnumerable<DTO_CC_ChamCongNgoaiGio> ChamCongNgoaiGio_Find(DateTime tuNgay, DateTime denNgay, Guid idnhanvien)
        {
            var result = from o in this.ObjectSet
                                    where tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay
                                            && o.IDNhanVien == idnhanvien
                                     orderby o.Ngay
                                     select new DTO_CC_ChamCongNgoaiGio()
                                     {
                                         Oid = o.Oid,
                                         MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                         HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                         Ngay = o.Ngay,
                                         TuGio = o.TuGio,
                                         DenGio = o.DenGio,
                                         TuGioThucTe = o.TuGioThucTe,
                                         DenGioThucTe = o.DenGioThucTe,
                                         SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                                         SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                                         SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                                         SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                                         LyDo = o.LyDo,
                                         TrangThai_Admin = o.TrangThai_Admin,
                                         TrangThai_TP = o.TrangThai_TP,
                                         TrangThai_BGH = o.TrangThai_BGH,
                                         IdBoPhan = o.IDBoPhan.Value,
                                         TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                         TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                                     };

            return result;
        }
        public IQueryable<DTO_CC_ChamCongNgoaiGio> QuanLyChamCongNgoaiGio_Find(DateTime tuNgay, DateTime denNgay, Guid idbophan, int trangthai, Guid userID,Guid congTy)
        {
            WebUser user = (new WebUser_Factory()).GetByID(userID);
            if (user == null) return null;
            string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
            string idQuanTriKhoi = WebGroupConst.QuanTriKhoiID.ToString().ToUpper();
            string idAdmin = WebGroupConst.AdminId.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            string idHieuTruongUyQuyen = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idTruongBoPhan = WebGroupConst.TruongBoPhanID.ToString().ToUpper();

            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(user.Oid, congTy);

            //Lấy danh sách rỗng
            var result = (from o in this.ObjectSet
                          where o.Ngay.Value.Month == 100
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            //
            if ((idHieuTruong.Equals(user.WebGroupID.ToString().ToUpper())
               || idHieuTruongUyQuyen.Equals(user.WebGroupID.ToString().ToUpper())))
            {
                result = (from o in this.ObjectSet
                          where tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay
                                  && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                  && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                  && (o.TrangThai_BGH == trangthai || trangthai == 3)
                                  && o.NgoaiKeHoach.Value
                                  && o.TrangThai_TP == 1
                                  && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                          orderby o.Ngay
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            }
            if ((idTruongPhong.Equals(user.WebGroupID.ToString().ToUpper())
                || idTruongPhongUyQuyen.Equals(user.WebGroupID.ToString().ToUpper())
                || idTruongBoPhan.Equals(user.WebGroupID.ToString().ToUpper())))
            {
                result = (from o in this.ObjectSet
                          where tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay
                                  && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                  && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                  && (o.TrangThai_TP == trangthai || trangthai == 3)
                                  && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                          orderby o.Ngay
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            }
            if (idQuanTriTruong.Equals(user.WebGroupID.ToString().ToUpper())
                || idQuanTriKhoi.Equals(user.WebGroupID.ToString().ToUpper())
                || idAdmin.Equals(user.WebGroupID.ToString().ToUpper()))
            {
                result = (from o in this.ObjectSet
                          where tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay
                                  && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                  && (o.TrangThai_Admin == trangthai || trangthai == 3)
                                  && (o.ThongTinNhanVien.NhanVien.CongTy == congTy || BoPhanConst.CoporationGuid == congTy)
                          orderby o.Ngay
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            }
            return result;
        }
        public IQueryable<DTO_CC_ChamCongNgoaiGio> QuanLyChamCongNgoaiGio_Find_NhacViec(DateTime tuNgay, DateTime denNgay, Guid idbophan, int trangthai, Guid userID,Guid congTy, bool tatCaDonChuaDuyet)
        {
            WebUser user = (new WebUser_Factory()).GetByID(userID);
            if (user == null) return null;
            string idQuanTriTruong = WebGroupConst.QuanTriTruongID.ToString().ToUpper();
            string idQuanTriKhoi = WebGroupConst.QuanTriKhoiID.ToString().ToUpper();
            string idAdmin = WebGroupConst.AdminId.ToString().ToUpper();
            string idTruongPhong = WebGroupConst.TruongPhongID.ToString().ToUpper();
            string idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID.ToString().ToUpper();
            string idHieuTruongUyQuyen = WebGroupConst.HieuTruongUyQuyenID.ToString().ToUpper();
            string idHieuTruong = WebGroupConst.HieuTruongID.ToString().ToUpper();
            string idTruongBoPhan = WebGroupConst.TruongBoPhanID.ToString().ToUpper();

            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(user.Oid, congTy);

            //Lấy danh sách rỗng
            var result = (from o in this.ObjectSet
                          where o.Ngay.Value.Month == 100
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            //
            if ((idHieuTruong.Equals(user.WebGroupID.ToString().ToUpper())
               || idHieuTruongUyQuyen.Equals(user.WebGroupID.ToString().ToUpper())))
            {
                result = (from o in this.ObjectSet
                          where (tatCaDonChuaDuyet || (tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay))
                                  && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                  && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                  && (o.TrangThai_BGH == 0)
                                  &&  o.NgoaiKeHoach.Value
                                  && o.TrangThai_TP == 1
                                  && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                          orderby o.Ngay
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            }
            if ((idTruongPhong.Equals(user.WebGroupID.ToString().ToUpper())
                || idTruongPhongUyQuyen.Equals(user.WebGroupID.ToString().ToUpper())
                || idTruongBoPhan.Equals(user.WebGroupID.ToString().ToUpper())))
            {
                result = (from o in this.ObjectSet
                          where (tatCaDonChuaDuyet || (tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay))
                                  && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                  && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                  && (o.TrangThai_TP == 0)
                                  && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                          orderby o.Ngay
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            }
            if (idQuanTriTruong.Equals(user.WebGroupID.ToString().ToUpper())
                || idQuanTriKhoi.Equals(user.WebGroupID.ToString().ToUpper())
                || idAdmin.Equals(user.WebGroupID.ToString().ToUpper()))
            {
                result = (from o in this.ObjectSet
                          where (tatCaDonChuaDuyet || (tuNgay <= o.Ngay.Value && o.Ngay.Value <= denNgay))
                                  && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                  && (o.TrangThai_Admin == 0)
                                  && (o.ThongTinNhanVien.NhanVien.CongTy == congTy || BoPhanConst.CoporationGuid == congTy)
                          orderby o.Ngay
                          select new DTO_CC_ChamCongNgoaiGio()
                          {
                              Oid = o.Oid,
                              MaNhanVien = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              Ngay = o.Ngay,
                              TuGio = o.TuGio,
                              DenGio = o.DenGio,
                              TuGioThucTe = o.TuGioThucTe,
                              DenGioThucTe = o.DenGioThucTe,
                              SoPhutDangKy = o.SoPhutDangKy != null ? o.SoPhutDangKy.Value : 0,
                              SoPhutThucTe = o.SoPhutThucTe != null ? o.SoPhutThucTe.Value : 0,
                              SoGioDangKy = o.SoGioDangKy != null ? o.SoGioDangKy.Value : 0,
                              SoGioThucTe = o.SoGioThucTe != null ? o.SoGioThucTe.Value : 0,
                              LyDo = o.LyDo,
                              TrangThai_Admin = o.TrangThai_Admin,
                              TrangThai_TP = o.TrangThai_TP,
                              TrangThai_BGH = o.TrangThai_BGH,
                              IdBoPhan = o.IDBoPhan.Value,
                              TenBoPhan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              TenLoaiNgay = o.LoaiNgayNgoaiGio1.TenNgayNgoaiGio
                          });
            }
            return result;
        }

        public CC_ChamCongNgoaiGio GetByID(Guid oid)
        {
            CC_ChamCongNgoaiGio result = (from o in this.ObjectSet
                                          where o.Oid == oid
                                          select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChamCongNgoaiGio item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        public IQueryable<CC_ChamCongNgoaiGio> ChamCongNgoaiGioChuaDuyet_ByUser(Guid webUserId, Guid congTy)
        {
            //
            Guid idQuanTriTruong = WebGroupConst.QuanTriTruongID;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
            Guid idHieuTruong = WebGroupConst.HieuTruongID;
            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (userHienTai == null) return null;
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId, congTy);

            //Hiệu trưởng
            if (userHienTai.WebGroupID.Equals(idHieuTruong)
            ||  userHienTai.WebGroupID.Equals(idHieuTruongUQ))
            {
                var result = (from o in this.ObjectSet
                              where     o.TrangThai_BGH == 0
                                        && o.TrangThai_TP == 1
                                        && o.NgoaiKeHoach.Value
                                        && o.CongTy == congTy
                                        && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                              //
                              select o);
                return result;
            }
            //Trưởng phòng or Trưởng phòng Ủy quyền
            else if (userHienTai.WebGroupID.Equals(idTruongPhong)
            || userHienTai.WebGroupID.Equals(idTruongPhongUyQuyen))
            {
                var result = (from o in this.ObjectSet
                              where o.TrangThai_TP == 0
                                    && o.TrangThai_Admin != 1
                                    && o.NgoaiKeHoach.Value
                                    && o.CongTy == congTy
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                              //
                              select o);
                return result;
            }
            else
            {
                var result = (from o in this.ObjectSet
                              where o.TrangThai_Admin == 0
                                    && o.CongTy == congTy
                              //
                              select o);
                return result;
            }
            //
        }

    }//end class
}
