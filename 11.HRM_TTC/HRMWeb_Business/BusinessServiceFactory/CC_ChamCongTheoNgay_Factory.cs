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
using System.Web.Configuration;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_ChamCongTheoNgay_Factory : BaseFactory<Entities, CC_ChamCongTheoNgay>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChamCongTheoNgay_Factory.New().CreateAloneObject();
        }
        public static CC_ChamCongTheoNgay_Factory New()
        {
            return new CC_ChamCongTheoNgay_Factory();
        }
        public CC_ChamCongTheoNgay_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public bool ExistsByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Any(x => x.Ngay.Month == thang && x.Ngay.Year == nam);
        }

        public bool ExistsByTuNgayDenNgay(DateTime tuNgay, DateTime denNgay, Guid boPhan, Guid congTy)
        {
            return this.ObjectSet.Any(x => x.Ngay >= tuNgay && x.Ngay <= denNgay && (boPhan == Guid.Empty || x.IDBoPhan == boPhan) && x.CongTy == congTy);
        }

        public CC_ChamCongTheoNgay GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public bool CheckDangKyKhungGio(DateTime date, Guid IDNV)
        {
            return this.Context.CC_DangKyKhungGioLamViec.Any(
                x => x.ThongTinNhanVien == IDNV
                && x.CC_KyDangKyKhungGio.TuNgay.Value <= date
                && x.CC_KyDangKyKhungGio.DenNgay.Value >= date);
        }
        public IQueryable<CC_ChamCongTheoNgay> GetListByIdList(List<Guid> idList)
        {
            var result = (from o in this.ObjectSet
                          where idList.Any(x => x == o.Oid)
                          select o);
            return result;
        }
        public IQueryable<CC_ChamCongTheoNgay> GetBy_HoSoId(Guid hoSoId)
        {
            var result = (from o in this.ObjectSet
                          where o.IDNhanVien == hoSoId
                          select o);
            return result;
        }
        public IQueryable<CC_ChamCongTheoNgay> GetBy_BoPhanId(Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.IDBoPhan == boPhanId
                          select o);
            return result;
        }

        public IQueryable<CC_ChamCongTheoNgay> GetBy_Date(DateTime date)
        {
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                          select o);
            return result;
        }
        public IQueryable<CC_ChamCongTheoNgay> GetBy_Date_HoSoId(DateTime date, Guid hoSoId)
        {
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                          && o.IDNhanVien == hoSoId
                          select o);
            return result;
        }
        public CC_ChamCongTheoNgay GetBy_DateNVId(DateTime date, Guid hoSoId)
        {
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                          && o.IDNhanVien == hoSoId
                          select o).FirstOrDefault();
            return result;
        }

        public IQueryable<CC_ChamCongTheoNgay> GetListBy_DateAndId(DateTime tuNgay, DateTime denNgay, Guid hoSoId)
        {
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) >= tuNgay.Date
                                && EntityFunctions.TruncateTime(o.Ngay) <= denNgay.Date
                                && o.IDNhanVien == hoSoId
                          select o);
            return result;
        }

        public IQueryable<CC_ChamCongTheoNgay> GetBy_ThangNam_HoSoId(int thang, int nam, Guid hoSoId)
        {
            var result = (from o in this.ObjectSet

                          where o.Ngay.Month == thang && o.Ngay.Year == nam
                          && o.IDNhanVien == hoSoId
                          select o).OrderBy(x => x.Ngay);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="boPhanId">Guid.Empty la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <returns></returns>
        public int FindCount_QuanlyChamCong(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu,Guid congTy)
        {
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamCong(date, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu, congTy).Count();
            return result;
        }
        public int FindCount_QuanlyChamCong_ThangVaNam(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu,Guid congTy)
        {
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamChong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu,congTy).Count();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="boPhanId">Guid.Empty la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <returns></returns>
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu,Guid congTy)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull_New(webUserId, congTy).Where(x => boPhanId == null || x.Oid == boPhanId || x.BoPhan2.BoPhanCha == boPhanId);

            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan || o.BoPhan.BoPhanCha == x.Oid)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.MaNhanVien.Contains(maNhanSu))
                                && (idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                          orderby o.HoSo.NhanVien.BoPhan1.TenBoPhan, o.HoSo.Ten
                          select new DTO_QuanLyChamCong_Find()
                          {
                              DaChamCong = o.DaChamCong,
                              HoTen = o.HoSo.HoTen,
                              SoHieuCongChuc = o.HoSo.MaNhanVien,
                              IDHinhThucNghi = o.IDHinhThucNghi,
                              TenHinhThucNghi = o.CC_HinhThucKhac != null ? o.CC_HinhThucKhac.MaQuanLy : o.CC_HinhThucNghi.MaQuanLy,
                              Ngay = o.Ngay,
                              Oid = o.Oid,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              TenCa = o.CC_CaChamCong1.TenCa
                          });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="boPhanId">Guid.Empty la tim tat ca bo phan</param>
        /// <param name="trangThaiChamCong">-1 Tat cả trạng thai, 0 chua cham cong, 1 da cham cong</param>
        /// <param name="diHoc"></param>
        /// <param name="trang"></param>
        /// <param name="soMauTinMoiTrang"></param>
        /// <returns></returns>
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong_CoPhanTrang(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu,Guid congTy)
        {
            int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamCong(date, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu,congTy).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamChong_ThangVaNam(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu,Guid congTy)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId,congTy).Where(x => boPhanId == null || x.Oid == boPhanId || x.BoPhan2.BoPhanCha == boPhanId);


            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //
            var result = (from o in this.ObjectSet
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan || o.BoPhan.BoPhanCha == x.Oid)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.MaNhanVien.Contains(maNhanSu))
                                && (idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                 && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                          orderby o.Ngay, o.HoSo.NhanVien.BoPhan1.TenBoPhan, o.HoSo.Ten
                          select new DTO_QuanLyChamCong_Find()
                          {
                              DaChamCong = o.DaChamCong,
                              HoTen = o.HoSo.HoTen,
                              SoHieuCongChuc = o.HoSo.MaNhanVien,
                              IDHinhThucNghi = o.IDHinhThucNghi,
                              TenHinhThucNghi = o.CC_HinhThucKhac != null ? o.CC_HinhThucKhac.MaQuanLy : o.CC_HinhThucNghi.MaQuanLy,
                              Ngay = o.Ngay, Oid = o.Oid,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              TenCa = o.CC_CaChamCong1.TenCa
                          });
            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong_ThangVaNam_CoPhanTrang(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu,Guid congTy)
        {
            int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamChong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu,congTy).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
            return result;
        }
        public IQueryable<CC_ChamCongTheoNgay> GetBy_Date_HoSoId_BoPhanId(DateTime date, Guid hoSoId, Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                          && o.IDNhanVien == hoSoId
                          && o.IDBoPhan == boPhanId
                          select o);
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChamCongTheoNgay item in deleteList)
            {
                context.DeleteObject(item);
            }
        }


        #endregion



        #region Bo sung
        public IQueryable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa(int ngay, int thang, int nam, Guid boPhanId)
        {
            Guid? idLoaiNhanSuGiangVien = LoaiNhanSuConst.GiangVienId;

            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                && (//boPhanId == Guid.Empty || 
                                o.IDBoPhan == boPhanId)
                                && (o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                          orderby o.HoSo.Ten, o.HoSo.NhanVien.BoPhan1.TenBoPhan
                          //Quét 4 lần
                          //select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVaoSang = o.GioVaoSang, GioRaSang = o.GioRaSang, GioVaoChieu = o.GioVaoChieu, GioRaChieu = o.GioRaChieu });
                          // Quét 2 lần
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.MaNhanVien, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVaoSang, GioRa = o.GioRaChieu });
            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(int ngay, int thang, int nam, Guid nhanVienID)
        {
            Guid? idLoaiNhanSuGiangVien = LoaiNhanSuConst.GiangVienId;

            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                && o.IDNhanVien == nhanVienID
                                && o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                          orderby o.HoSo.Ten, o.HoSo.NhanVien.BoPhan1.TenBoPhan
                          //Quét 4 lần
                          //select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVaoSang = o.GioVaoSang, GioRaSang = o.GioRaSang, GioVaoChieu = o.GioVaoChieu, GioRaChieu = o.GioRaChieu });
                          // Quét 2 lần
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.MaNhanVien, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVaoSang, GioRa = o.GioRaChieu });
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="idNhanVien">bat buoc</param>
        /// <returns></returns>
        public IQueryable<DTO_QuanLyXetABC_BieuDoVaoRa> XetABC_BieuDoVaoRa(int thang, int nam, Guid idNhanVien)
        {

            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                && o.IDNhanVien == idNhanVien
                          orderby o.HoSo.Ten, o.BoPhan.TenBoPhan
                          select new DTO_QuanLyXetABC_BieuDoVaoRa() { MaNhanSu = o.HoSo.MaNhanVien, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay });
            return result;
        }
        public DTO_QuanLyXetABC_ChiTietTheoNhanVien XetABC_ChiTietTheoNhanVien(int thang, int nam, Guid idNhanVien)
        {

            var result = (from o in this.Context.CC_ChiTietChamCong
                          where o.ThongTinNhanVien == idNhanVien

                          select new DTO_QuanLyXetABC_ChiTietTheoNhanVien()
                          {
                              MaNhanSu = o.ThongTinNhanVien1.NhanVien.HoSo.MaNhanVien,
                              HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                              IDNhanVien = o.ThongTinNhanVien.Value,
                              //lấy thêm danh sách bên trong
                              ChiTietChamCong = (from x in this.Context.CC_ChamCongTheoNgay
                                                 where (x.Ngay.Year == nam && x.Ngay.Month == thang)
                                                  && x.IDNhanVien == idNhanVien
                                                 select new DTO_QuanLyXetABC_ChiTietTheoNhanVien_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                         ,
                                                     IDNhanVien = x.IDNhanVien
                                                         ,
                                                     Ngay = x.Ngay
                                                         ,
                                                     IDHinhThucNghi = x.IDHinhThucNghi
                                                         ,
                                                     MaHinhThucNghi = (x.IDHinhThucNghi == null ? "+" : x.CC_HinhThucNghi.KyHieu)

                                                 }
                                                                  ).ToList()
                              //
                          }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_XemThongTinChamCongThang(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu,Guid congTy)
        {
            /*
            //Dữ liệu công tháng này nhưng đã chốt tháng tới thì phải lấy dữ liệu chốt tháng tới
            int thangpre = thang;
            int nampre = nam;
            if (thangpre == 1) { thangpre = 12; nampre--; }
            else { thangpre--; } */


            //Lấy kỳ chấm công theo tháng 
            CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam,congTy);
            if (kyChamCong == null) return null;

            //
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);

            //Lấy danh sách tất cả nhân viên 
            IQueryable<DTO_DanhSachChotCong> dsIdNhanVien = (from o in this.ObjectSet
                                                             where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                                                   && (o.IDBoPhan == boPhanId)
                                                                   && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                                                   && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                                                   && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                                             orderby o.HoSo.Ten
                                                             select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv.IDNhanVien equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv.IDNhanVien
                              ,
                              HoTen = hs.HoTen
                              ,
                              Ho = hs.Ho
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.MaNhanVien
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                                        && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv.IDNhanVien
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.CC_HinhThucNghi.KyHieu)
                                                     ,
                                                     QuanLyViPham = (from c in this.Context.CC_QuanLyViPham
                                                                     where c.CC_ChamCongTheoNgay == o.Oid
                                                                     select new DTO_CC_QuanLyViPham()
                                                                     {
                                                                         HinhThucViPham = c.CC_HinhThucViPham1.TenHinhThucViPham
                                                                         ,
                                                                         ThoiGianSom = c.ThoiGianSom != null ? c.ThoiGianSom.ToString() : ""
                                                                         ,
                                                                         ThoiGianTre = c.ThoiGianTre != null ? c.ThoiGianTre.ToString() : ""
                                                                     }).ToList()
                                                                     ,
                                                     QuanLyViPhamCount = (from c in this.Context.CC_QuanLyViPham
                                                                          where c.CC_ChamCongTheoNgay == o.Oid
                                                                          select 0).ToList().Count()
                                                 }).ToList()
                            ,
                              NgayHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                          select o.NgayHuongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayHuongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayHuongBHXH ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayPhep = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                     && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                     && o.BoPhan == idNv.IDBoPhan
                                                     select o.NgayPhep ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayKhongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongCong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.TongCong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayHe = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                  where o.ThongTinNhanVien1.Oid == hs.Oid
                                                        && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                        && o.BoPhan == idNv.IDBoPhan
                                                  select o.NgayHe ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayPhepBu = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                      where o.ThongTinNhanVien1.Oid == hs.Oid
                                                            && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                            && o.BoPhan == idNv.IDBoPhan
                                                      select o.NgayPhepBu ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0

                          }).OrderByDescending(x => x.LaNhanVienToChucHanhChinh).ThenBy(y => y.Ten);

            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu, Guid userId,Guid congTy)
        {
            //DateTime ngayTrongThang = Convert.ToDateTime(thang + "/01/" + nam);

            bool tatCaMaNhanSu = (String.IsNullOrEmpty(maNhanSu) == true);
            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(userId);
            if (userHienTai == null) return null;
            //
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
            Guid idHieuTruong = WebGroupConst.HieuTruongID;
            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(userId, congTy);
            
            //Lấy kỳ chấm công theo tháng 
            if (userHienTai.WebGroupID.Equals(idAdmin)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                var boPhan = tmpFactory.GetByID(boPhanId);
                if (boPhan != null)
                    congTy = boPhan.CongTy.Value;
            }
            CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam, congTy);
            if (kyChamCong == null) return null;
            //

            //Lấy danh sách rỗng
            var dsIdNhanVien = (from o in this.ObjectSet
                                where o.Oid == Guid.Empty
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();

            //1. Hội đồng quản trị
            if (userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && ( o.IDBoPhan == boPhanId || boPhanId == Guid.Empty
                                           || o.BoPhan.BoPhanCha == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null 
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false 
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay) )
                                      && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan.CongTy))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan, CapBacChucVu = o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.CapBac }).Distinct();
            }
            //2. Hiệu trưởng
            else if (userHienTai.WebGroupID.Equals(idHieuTruong)
                || userHienTai.WebGroupID.Equals(idHieuTruongUQ))
            {
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && ( o.IDBoPhan == boPhanId || boPhanId == Guid.Empty
                                           || o.BoPhan.BoPhanCha == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                      && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan, CapBacChucVu = o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.CapBac }).Distinct();
            }
            //3. Trưởng phòng or Trưởng phòng ủy quyền
            else if (userHienTai.WebGroupID.Equals(idTruongPhong)
                || userHienTai.WebGroupID.Equals(idTruongPhongUyQuyen))
            {
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                      && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan, CapBacChucVu = o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.CapBac }).Distinct();
            }
            else // Quản trị
            {
                //
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && (o.IDBoPhan == boPhanId || boPhanId == Guid.Empty
                                           || o.BoPhan.BoPhanCha == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan, CapBacChucVu = o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.CapBac }).Distinct();
            }

            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv.IDNhanVien equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {
                              CapBacChucVu = idNv.CapBacChucVu
                              ,
                              IDNhanVien = idNv.IDNhanVien
                              ,
                              HoTen = hs.HoTen
                              ,
                              TenDonVi = (from x in this.Context.BoPhans
                                          where x.GCRecord == null && x.Oid == idNv.IDBoPhan select x.TenBoPhan).FirstOrDefault()
                              ,
                              Ho = hs.Ho
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.MaNhanVien
                              ,
                              MaTapDoan = hs.NhanVien.HoSo.MaTapDoan
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              TenCa = ""  /*(from y in this.Context.CC_DangKyKhungGioLamViec
                                       join k in this.Context.CC_KyDangKyKhungGio on y.KyDangKy equals k.Oid
                                       where y.ThongTinNhanVien == idNv
                                             && k.TuNgay <= ngayTrongThang && ngayTrongThang <= k.DenNgay
                                       select y.CC_CaChamCong.TenCa).FirstOrDefault() */
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                                       && (idNv.IDBoPhan == o.IDBoPhan) && o.IDNhanVien == idNv.IDNhanVien
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     NguoiDungChinhSua = o.NguoiDungChinhSua
                                                     ,
                                                     MaHinhThucNghi = o.IDHinhThucKhac != null ? o.CC_HinhThucKhac.MaQuanLy : o.CC_HinhThucNghi.KyHieu
                                                     ,
                                                     QuanLyViPham = (from c in this.Context.CC_QuanLyViPham
                                                                     where c.CC_ChamCongTheoNgay == o.Oid
                                                                     select new DTO_CC_QuanLyViPham()
                                                                     {
                                                                         HinhThucViPham = c.CC_HinhThucViPham1.TenHinhThucViPham
                                                                         ,
                                                                         ThoiGianSom = c.ThoiGianSom != null ? c.ThoiGianSom.ToString() : ""
                                                                         ,
                                                                         ThoiGianTre = c.ThoiGianTre != null ? c.ThoiGianTre.ToString() : ""
                                                                     }).ToList()
                                                                     ,
                                                     QuanLyViPhamCount = (from c in this.Context.CC_QuanLyViPham
                                                                          where c.CC_ChamCongTheoNgay == o.Oid
                                                                          select 0).ToList().Count()
                                                 }).ToList()
                            ,
                              NgayHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                                && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                                && o.BoPhan == idNv.IDBoPhan
                                                          select o.NgayHuongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayPhep = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayPhep ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayHuongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                     select o.NgayHuongBHXH ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayKhongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongCong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                    select o.TongCong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayHe = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayHe ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayPhepBu = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                  where o.ThongTinNhanVien1.Oid == hs.Oid
                                                        && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                        && o.BoPhan == idNv.IDBoPhan
                                                  select o.NgayPhepBu ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                          }).OrderBy(q => q.TenDonVi).ThenByDescending(x => x.LaNhanVienToChucHanhChinh).ThenBy(z => z.CapBacChucVu).ThenBy(y => y.Ten);

            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThangAll(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu, Guid userId, Guid congTy)
        {
            //DateTime ngayTrongThang = Convert.ToDateTime(thang + "/01/" + nam);

            bool tatCaMaNhanSu = (String.IsNullOrEmpty(maNhanSu) == true);
            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(userId);
            if (userHienTai == null) return null;
            //
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
            Guid idHieuTruong = WebGroupConst.HieuTruongID;
            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(userId, congTy);

            //Lấy kỳ chấm công theo tháng 
            if (userHienTai.WebGroupID.Equals(idAdmin)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                var boPhan = tmpFactory.GetByID(boPhanId);
                if (boPhan != null)
                    congTy = boPhan.CongTy.Value;
            }
            CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam, congTy);
            if (kyChamCong == null) return null;
            //

            //Lấy danh sách rỗng
            var dsIdNhanVien = (from o in this.ObjectSet
                                where o.Oid == Guid.Empty
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();

            //1. Hội đồng quản trị
            if (userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && (o.IDBoPhan == boPhanId || boPhanId == Guid.Empty)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                      && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan.CongTy))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            }
            //2. Hiệu trưởng
            else if (userHienTai.WebGroupID.Equals(idHieuTruong)
                || userHienTai.WebGroupID.Equals(idHieuTruongUQ))
            {
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && (o.IDBoPhan == boPhanId || boPhanId == Guid.Empty)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                      && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            }
            //3. Trưởng phòng or Trưởng phòng ủy quyền
            else if (userHienTai.WebGroupID.Equals(idTruongPhong)
                || userHienTai.WebGroupID.Equals(idTruongPhongUyQuyen))
            {
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && (o.IDBoPhan == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                      && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            }
            else // Quản trị
            {
                //
                dsIdNhanVien = (from o in this.ObjectSet
                                where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                      && (o.IDBoPhan == boPhanId || boPhanId == Guid.Empty)
                                      && (tatCaMaNhanSu || o.HoSo.MaNhanVien == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && o.HoSo.GCRecord == null
                                      && (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                          || (o.HoSo.NhanVien.TinhTrang1.DaNghiViec == true
                                              && o.HoSo.NhanVien.NgayNghiViec >= kyChamCong.TuNgay))
                                orderby o.HoSo.Ten
                                select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            }

            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv.IDNhanVien equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv.IDNhanVien
                              ,
                              HoTen = hs.HoTen
                              ,
                              IDBoPhan = idNv.IDBoPhan
                              ,
                              TenDonVi = (from x in this.Context.BoPhans
                                          where x.GCRecord == null && x.Oid == idNv.IDBoPhan
                                          select x.TenBoPhan).FirstOrDefault()
                              ,
                              Ho = hs.Ho
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.MaNhanVien
                              ,
                              MaTapDoan = hs.NhanVien.HoSo.MaTapDoan
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              TenCa = ""  /*(from y in this.Context.CC_DangKyKhungGioLamViec
                                       join k in this.Context.CC_KyDangKyKhungGio on y.KyDangKy equals k.Oid
                                       where y.ThongTinNhanVien == idNv
                                             && k.TuNgay <= ngayTrongThang && ngayTrongThang <= k.DenNgay
                                       select y.CC_CaChamCong.TenCa).FirstOrDefault() */
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                                       && (idNv.IDBoPhan == o.IDBoPhan) && o.IDNhanVien == idNv.IDNhanVien
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     NguoiDungChinhSua = o.NguoiDungChinhSua
                                                     ,
                                                     MaHinhThucNghi = o.IDHinhThucKhac != null ? o.CC_HinhThucKhac.MaQuanLy : o.CC_HinhThucNghi.KyHieu
                                                     ,
                                                     QuanLyViPham = (from c in this.Context.CC_QuanLyViPham
                                                                     where c.CC_ChamCongTheoNgay == o.Oid
                                                                     select new DTO_CC_QuanLyViPham()
                                                                     {
                                                                         HinhThucViPham = c.CC_HinhThucViPham1.TenHinhThucViPham
                                                                         ,
                                                                         ThoiGianSom = c.ThoiGianSom != null ? c.ThoiGianSom.ToString() : ""
                                                                         ,
                                                                         ThoiGianTre = c.ThoiGianTre != null ? c.ThoiGianTre.ToString() : ""
                                                                     }).ToList()
                                                                     ,
                                                     QuanLyViPhamCount = (from c in this.Context.CC_QuanLyViPham
                                                                          where c.CC_ChamCongTheoNgay == o.Oid
                                                                          select 0).ToList().Count()
                                                 }).ToList()
                            ,
                              NgayHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                                && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                                && o.BoPhan == idNv.IDBoPhan
                                                          select o.NgayHuongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayPhep = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayPhep ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayHuongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                         where o.ThongTinNhanVien1.Oid == hs.Oid
                                                              && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                              && o.BoPhan == idNv.IDBoPhan
                                                         select o.NgayHuongBHXH ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                          select o.NgayKhongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongCong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                    select o.TongCong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayHe = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                  where o.ThongTinNhanVien1.Oid == hs.Oid
                                                        && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                        && o.BoPhan == idNv.IDBoPhan
                                                  select o.NgayHe ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayPhepBu = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                      where o.ThongTinNhanVien1.Oid == hs.Oid
                                                            && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                            && o.BoPhan == idNv.IDBoPhan
                                                      select o.NgayPhepBu ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0

                          }).OrderByDescending(x => x.LaNhanVienToChucHanhChinh).ThenBy(y => y.Ten);

            return result.OrderBy(x => x.TenDonVi);
        }

        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(int thang, int nam, Guid nhanVienID,Guid congTy)
        {
            //DateTime ngayTrongThang = Convert.ToDateTime(thang + "/01/" + nam);

            //Lấy kỳ chấm công theo tháng 
            CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam,congTy);
            if (kyChamCong == null) return null;

            //Lấy danh sách chấm công theo ngày
            IQueryable<DTO_DanhSachChotCong> dsIdNhanVien = (from o in this.ObjectSet
                                                             where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                                                   && o.IDNhanVien == nhanVienID
                                                                   && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.DaNghiViec == false
                                                             select new DTO_DanhSachChotCong { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv.IDNhanVien equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {
                              Ho = hs.Ho
                              ,
                              Ten = hs.Ten
                              ,
                              IDNhanVien = idNv.IDNhanVien
                              ,
                              HoTen = hs.HoTen
                              ,
                              TenDonVi = (from x in this.Context.BoPhans
                                          where x.GCRecord == null && x.Oid == idNv.IDBoPhan select x.TenBoPhan).FirstOrDefault()
                              ,
                              MaNhanSu = hs.NhanVien.HoSo.MaNhanVien
                              ,
                              MaTapDoan = hs.NhanVien.HoSo.MaTapDoan
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              TenCa = "" /*(from y in  this.Context.CC_DangKyKhungGioLamViec
                                            join k in this.Context.CC_KyDangKyKhungGio on y.KyDangKy equals k.Oid
                                            where y.ThongTinNhanVien == idNv 
                                                  && k.TuNgay <= ngayTrongThang && ngayTrongThang <= k.DenNgay
                                                  select y.CC_CaChamCong.TenCa).FirstOrDefault() */
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where o.Ngay >= kyChamCong.TuNgay && o.Ngay <= kyChamCong.DenNgay
                                                        && o.IDNhanVien == idNv.IDNhanVien
                                                        && (idNv.IDBoPhan == o.IDBoPhan)
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = o.IDHinhThucKhac != null ? o.CC_HinhThucKhac.MaQuanLy : o.CC_HinhThucNghi.KyHieu
                                                 }
                                                              ).ToList()
                            ,
                              NgayHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                                && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                                && o.BoPhan == idNv.IDBoPhan
                                                          select o.NgayHuongLuong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayHuongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayHuongBHXH ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayPhep = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                          && o.BoPhan == idNv.IDBoPhan
                                                     select o.NgayPhep ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              NgayKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.NgayKhongLuong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              TongCong = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                    && o.BoPhan == idNv.IDBoPhan
                                                    select o.TongCong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayHe = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                  where o.ThongTinNhanVien1.Oid == hs.Oid
                                                        && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                        && o.BoPhan == idNv.IDBoPhan
                                                  select o.NgayHe ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              NgayPhepBu = (decimal?)(from o in this.Context.CC_ChiTietChamCong
                                                      where o.ThongTinNhanVien1.Oid == hs.Oid
                                                            && o.CC_QuanLyChamCong.CC_KyChamCong.Oid == kyChamCong.Oid
                                                            && o.BoPhan == idNv.IDBoPhan
                                                      select o.NgayPhepBu ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                          });

            return result;
        }
        public IQueryable<DTO_DanhSachChotCong> ChamCongTheoNgay_DanhSachChotCong(DateTime tuNgay, DateTime denNgay, Guid idBoPhan, Guid congTy)
        {
            var result = (from o in this.ObjectSet
                          where o.Ngay >= tuNgay && o.Ngay <= denNgay
                                && (idBoPhan == Guid.Empty || o.IDBoPhan == idBoPhan || o.BoPhan.BoPhanCha == idBoPhan)
                                && o.CongTy == congTy
                          select new DTO_DanhSachChotCong
                          { IDNhanVien = o.IDNhanVien, IDBoPhan = o.IDBoPhan }).Distinct();
            return result;
        }

        public bool CheckDangSuDung(Guid Oid)
        {
            bool result = false;
            result = this.ObjectSet.Any(c => c.CC_CaChamCong == Oid);
            return result;
        }
        public IEnumerable<DTO_NgayChamCong> GetList_NgayTrongKyChamCong(int thang, int nam, Guid bophanId, Guid? webGroupId,Guid congTy)
        {
            List<DTO_NgayChamCong> list = new List<DTO_NgayChamCong>();
            //int ngaycuoi=DateTime.DaysInMonth(nam, thang);
            //DateTime tuNgay = new DateTime(nam, thang, 1);
            //DateTime denNgay = new DateTime(nam, thang, ngaycuoi);
            //
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
            //
            //Lấy kỳ chấm công theo tháng 
            if (webGroupId.Equals(idAdmin)
                || webGroupId.Equals(idHoiDongQuanTri)
                || webGroupId.Equals(idHoiDongQuanTriUQ))
            {
                var boPhan = (new BoPhan_Factory()).GetByID(bophanId);
                if (boPhan != null)
                    congTy = boPhan.CongTy.Value;
            }
            //
            CC_KyChamCong kyChamCong = new CC_KyChamCong_Factory().GetKyChamCong_ByThangNam(thang, nam,congTy);
            if (kyChamCong == null) return null;

            //
            DateTime tuNgay = kyChamCong.TuNgay.Value;
            DateTime denNgay = kyChamCong.DenNgay.Value;

            //Tao danh sach ngay
            var dates = new List<DateTime>();
            for (var dt = tuNgay; dt <= denNgay; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            //
            foreach (DateTime date in dates)
            {
                DTO_NgayChamCong ngaycc = new DTO_NgayChamCong();
                ngaycc.NgayChamCong = date;
                ngaycc.Ngay = date.Day;
                string thu = date.DayOfWeek.ToString();
                switch (thu)
                {
                    case "Sunday":
                        {
                            ngaycc.Thu = "CN";
                            ngaycc.T7CN = true;
                        }
                        break;
                    case "Monday":
                        {
                            ngaycc.Thu = "T2";
                        }
                        break;
                    case "Tuesday":
                        {
                            ngaycc.Thu = "T3";
                        }
                        break;
                    case "Wednesday":
                        {
                            ngaycc.Thu = "T4";
                        }
                        break;
                    case "Thursday":
                        {
                            ngaycc.Thu = "T5";
                        }
                        break;
                    case "Friday":
                        {
                            ngaycc.Thu = "T6";
                        }
                        break;
                    case "Saturday":
                        {
                            ngaycc.Thu = "T7";
                            ngaycc.T7CN = true;
                        }
                        break;

                }
                list.Add(ngaycc);
            }
            return list;
        }

        #endregion
    }//end class
}
