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
using ERP_Core.Common;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_BoSungCong_Factory : BaseFactory<Entities, CC_BoSungCong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_BoSungCong_Factory.New().CreateAloneObject();
        }
        public static CC_BoSungCong_Factory New()
        {
            return new CC_BoSungCong_Factory();
        }
        public CC_BoSungCong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_BoSungCong GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<CC_BoSungCong> BoSungCongChuaDuyet_ByUser(Guid webUserId, Guid congTy)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId, congTy);
            Guid idQuanTriTruong = WebGroupConst.QuanTriTruongID;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUQ = WebGroupConst.TruongPhongUyQuyenID;

            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (userHienTai == null) return null;
            //
            if (userHienTai.WebGroupID.Equals(idQuanTriTruong)
                || userHienTai.WebGroupID.Equals(idTruongPhong)
                || userHienTai.WebGroupID.Equals(idTruongPhongUQ))
            {
                var result = (from o in this.ObjectSet
                              where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
                                    && o.TrangThai == -1
                                    && o.HoSo.NhanVien.CongTy == congTy
                              //
                              select o);
                return result;
            }
            else
            {
                return this.ObjectSet.Where(x => x.Oid == Guid.Empty); // Không lấy gì cả
            }
        }

        public IQueryable<DTO_CC_BoSungCong> CaNhanBoSungCong_Find(DateTime tungay, DateTime denngay, Guid webUserId)
        {
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var result = (from o in this.ObjectSet
                          where o.NguoiTao == webUserId
                                && o.TuNgay <= denngay
                                && o.DenNgay >= tungay
                          //
                          select new DTO_CC_BoSungCong
                          {
                              Oid = o.Oid,
                              HoTen = o.HoSo.HoTen,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              NgayTao = o.NgayTao,
                              TrangThai = o.TrangThai,
                              IDNhanVien = o.IDNhanVien,
                              NguoiTao = o.NguoiTao,
                              SoNgay = o.SoNgay ?? 0,
                              Buoi = o.Buoi.ToString(),
                              LyDo = o.LyDo,
                              PhanHoi_NguoiDuyet = o.PhanHoi_NguoiDuyet
                          });
            return result;
        }
        public IQueryable<DTO_QuanLyBoSungCong_Find> QuanLyBoSungCong_Find(DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            DateTime ngayHienTai = DateTime.Now.Date;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId, congTy).Where(x => boPhanId == Guid.Empty || x.Oid == boPhanId);
            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);
            //
            var result = (from o in this.ObjectSet
                          where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan || x.Oid == o.HoSo.NhanVien.BoPhan1.BoPhanCha)
                          &&
                          (trangThai == 2 || o.TrangThai == trangThai)
                          &&
                          (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                          && (o.HoSo.NhanVien.BoPhan1.CongTy == congTy || BoPhanConst.CoporationGuid == congTy)
                          && o.TuNgay <= denngay
                          && o.DenNgay >= tungay
                          select new DTO_QuanLyBoSungCong_Find()
                          {
                              DenNgay = o.DenNgay,
                              HoTen = o.HoSo.HoTen,
                              IDNhanVien = o.IDNhanVien,
                              NguoiTao = o.NguoiTao,
                              SoHieuCongChuc = o.HoSo.MaNhanVien,
                              NgayTao = o.NgayTao,
                              Oid = o.Oid,
                              TrangThai = o.TrangThai,
                              TuNgay = o.TuNgay,
                              Buoi = o.Buoi.ToString(),
                              SoNgay = o.SoNgay ?? 0,
                              LyDo = o.LyDo,
                              PhanHoi_NguoiDuyet = o.PhanHoi_NguoiDuyet
                          });
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_BoSungCong item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion

        #region Nhắc việc
        public IQueryable<DTO_QuanLyBoSungCong_Find> QuanLyBoSungCong_Find_NhacViec(DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy, bool tatCaDonChuaDuyet)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            DateTime ngayHienTai = DateTime.Now.Date;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId, congTy);
            //
            var result = (from o in this.ObjectSet
                          where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
                                && o.TrangThai == -1
                                && (tatCaDonChuaDuyet || (o.TuNgay <= denngay && o.DenNgay >= tungay))
                          //
                          select new DTO_QuanLyBoSungCong_Find() { DenNgay = o.DenNgay, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, NguoiTao = o.NguoiTao, SoHieuCongChuc = o.HoSo.MaNhanVien, NgayTao = o.NgayTao, Oid = o.Oid, TrangThai = o.TrangThai, TuNgay = o.TuNgay, Buoi = o.Buoi.ToString(), SoNgay = o.SoNgay ?? 0, LyDo = o.LyDo, PhanHoi_NguoiDuyet = o.PhanHoi_NguoiDuyet });
            //
            return result;
        }
        #endregion
    }//end class
}
