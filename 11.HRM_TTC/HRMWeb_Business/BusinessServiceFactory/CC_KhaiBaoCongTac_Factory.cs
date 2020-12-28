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
    public class CC_KhaiBaoCongTac_Factory : BaseFactory<Entities, CC_KhaiBaoCongTac>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_KhaiBaoCongTac_Factory.New().CreateAloneObject();
        }
        public static CC_KhaiBaoCongTac_Factory New()
        {
            return new CC_KhaiBaoCongTac_Factory();
        }
        public CC_KhaiBaoCongTac_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_KhaiBaoCongTac GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public int LaySoLonNhat()
        {
            int nam = DateTime.Now.Year;
            var maxValue = this.ObjectSet.Where(c => c.NgayTao.Value.Year == nam).Max(x => x.So) ?? 0;
            if (maxValue == 0)
            {
                CC_CauHinhChamCong_Factory fac = new CC_CauHinhChamCong_Factory();
                maxValue = 10;//fac.GetCauHinhCauCong().SoGiayDiDuong;
            }
            return maxValue;
        }
        public IQueryable<CC_KhaiBaoCongTac> KhaiBaoCongTacChuaDuyet_ByUser(Guid webUserId, Guid congTy)
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
                                    && o.HoSo1.NhanVien.CongTy == congTy
                              //
                              select o);
                return result;
            }
            else
            {
                return this.ObjectSet.Where(x => x.Oid == Guid.Empty); // Không lấy gì cả
            }
        }

        public IQueryable<DTO_CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(DateTime tungay, DateTime denngay, Guid webUserId)
        {
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var result = (from o in this.ObjectSet
                          where o.IDWebUser == webUserId
                                && o.TuNgay <= denngay
                                && o.DenNgay >= tungay
                          //
                          select new DTO_CC_KhaiBaoCongTac
                          {
                              Oid = o.Oid,
                              HoTen = o.HoSo.HoTen,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              NoiDung = o.NoiDung,
                              DiaDiem = o.DiaDiem,
                              NgayTao = o.NgayTao,
                              TrangThai = o.TrangThai,
                              IDNhanVien = o.IDNhanVien,
                              SoNgay = o.SoNgay.Value,
                              Buoi = o.Buoi.ToString()
                          });
            return result;
        }
        public DTO_CC_KhaiBaoCongTac CaNhanKhaiBaoCongTac_Report(Guid id)
        {

            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select new DTO_CC_KhaiBaoCongTac()
                          {
                              Oid = o.Oid,
                              So = o.So ?? 0,
                              DiaDiem = o.DiaDiem,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              IDNhanVien = o.IDNhanVien,
                              IDNguoiKy = o.NguoiKy ?? Guid.Empty,
                              NoiDung = o.NoiDung
                          }).SingleOrDefault();
            //
            var hoSo = (from o in this.Context.HoSoes
                        where o.Oid == result.IDNhanVien
                        select o).SingleOrDefault();
            if (hoSo != null)
            {
                result.OngBa = hoSo.GioiTinh == 0 ? "Ông" : "Bà";
                result.HoTen = StringHelpers.ToTitleCase(hoSo.HoTen.ToLower());
                result.MaNhanVien = hoSo.MaNhanVien;
                result.TenKhoi = StringHelpers.ToTitleCase("");
                result.ChucVu = hoSo.NhanVien.ThongTinNhanVien.ChucVu1 != null ? hoSo.NhanVien.ThongTinNhanVien.ChucVu1.TenChucVu : "";
                result.TenPhongBan = StringHelpers.ToTitleCase(hoSo.NhanVien.BoPhan1.TenBoPhan.ToLower());
            }
            //
            var nguoiky = (from o in this.Context.HoSoes
                           where o.Oid == result.IDNguoiKy
                           select o).SingleOrDefault();
            if (nguoiky != null)
            {
                result.HoTenNguoiKy = nguoiky.HoTen;
                result.ChucVuNguoiKy = nguoiky.NhanVien.ThongTinNhanVien.ChucVu1 != null ? nguoiky.NhanVien.ThongTinNhanVien.ChucVu1.TenChucVu : "";
                result.DonViNguoiKy = nguoiky.NhanVien.BoPhan1.TenBoPhan;
            }
            result.TuNgayString = String.Format("{0:dd/MM/yyyy}", result.TuNgay);
            result.DenNgayString = String.Format("{0:dd/MM/yyyy}", result.DenNgay);
            return result;
        }
        public IQueryable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find(DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            DateTime ngayHienTai = DateTime.Now.Date;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull_New(webUserId, congTy).Where(x => boPhanId == Guid.Empty || x.Oid == boPhanId);
            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);
            //
            var result = (from o in this.ObjectSet
                          where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan || x.Oid == o.HoSo.NhanVien.BoPhan1.BoPhanCha)
                          &&
                          (trangThai == 2 || o.TrangThai == trangThai)
                          &&
                          (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                          && o.TuNgay <= denngay
                          && o.DenNgay >= tungay
                          select new DTO_QuanLyKhaiBaoCongTac_Find() { DenNgay = o.DenNgay, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, IDWebUser = o.IDWebUser, SoHieuCongChuc = o.HoSo.MaNhanVien, NgayTao = o.NgayTao, Oid = o.Oid, NoiDung = o.NoiDung, TrangThai = o.TrangThai, TuNgay = o.TuNgay, Buoi = o.Buoi.ToString(), DiaDiem = o.DiaDiem, SoNgay = o.SoNgay.Value });
            return result;
        }

        public IQueryable<DTO_DanhSachFile> GetDanhSachFile_ByOidKhaiBaoCongTac(Guid id)
        {
            var list = (from o in this.Context.CC_Attachments
                        where o.KhaiBaoCongTac == id
                        orderby o.Date
                        select new DTO_DanhSachFile
                        {
                            Oid = o.Oid,
                            FileName = o.FileName,
                            Date = o.Date.Value,
                            Data = o.Data
                        });

            return list;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_KhaiBaoCongTac item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion

        #region Nhắc việc
        public IQueryable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find_NhacViec(DateTime tungay, DateTime denngay, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId, Guid congTy, bool tatCaDonChuaDuyet)
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
                          select new DTO_QuanLyKhaiBaoCongTac_Find() { DenNgay = o.DenNgay, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, IDWebUser = o.IDWebUser, SoHieuCongChuc = o.HoSo.MaNhanVien, NgayTao = o.NgayTao, Oid = o.Oid, NoiDung = o.NoiDung, TrangThai = o.TrangThai, TuNgay = o.TuNgay, Buoi = o.Buoi.ToString(), DiaDiem = o.DiaDiem, SoNgay = o.SoNgay.Value });
            //
            return result;
        }
        #endregion
    }//end class
}
