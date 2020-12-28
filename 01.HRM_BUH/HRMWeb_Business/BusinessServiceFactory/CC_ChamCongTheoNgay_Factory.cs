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
        public IQueryable<HoSo> List_NVMoi_ChuaCo_CCTheoNgay(int thang, int nam)
        {
            var dsCC = (from o in this.ObjectSet
                        where o.Ngay.Month == thang
                        && o.Ngay.Year == nam
                        select o.IDNhanVien).Distinct();
            int s = dsCC.Count();
            var result = (from o in this.Context.HoSoes
                          where o.GCRecord == null
                          && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                          && o.NhanVien.NgayVaoCoQuan != null
                          && !dsCC.Contains(o.Oid)
                          select o);
            int d = result.Count();
            return result;
        }

        public bool ExistsByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Any(x => x.Ngay.Month == thang && x.Ngay.Year == nam);
        }

        public CC_ChamCongTheoNgay GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
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
        public IQueryable<CC_ChamCongTheoNgay> GetBy_ThangNam_HoSoId(int thang, int nam, Guid hoSoId)
        {
            var result = (from o in this.ObjectSet

                          where o.Ngay.Month == thang && o.Ngay.Year == nam
                          && o.IDNhanVien == hoSoId
                          select o);
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
        public int FindCount_QuanlyChamCong(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamCong(date, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu).Count();
            return result;
        }
        public int FindCount_QuanlyChamCong_ThangVaNam(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamChong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu).Count();
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
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //var result = (from o in this.ObjectSet
            //              where EntityFunctions.TruncateTime(o.Ngay) == date.Date
            //                    //&& (boPhanId == null || o.IDBoPhan == boPhanId)
            //                    && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
            //                    && (tatCaTrangThai || o.DaChamCong == daChamCong)
            //                    && (diHoc == null || o.DiHoc == diHoc)
            //                    && (tatCaMaNhanSu == true || o.HoSo.MaQuanLy.Contains(maNhanSu))
            //                    && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
            //              orderby o.BoPhan.TenBoPhan, o.HoSo.Ten
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                                //&& (boPhanId == null || o.IDBoPhan == boPhanId)
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                          orderby o.BoPhan.TenBoPhan, o.HoSo.Ten
                          select new DTO_QuanLyChamCong_Find() { DaChamCong = o.DaChamCong, HoTen = o.HoSo.HoTen, SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, IDHinhThucNghi = o.IDHinhThucNghi, Ngay = o.Ngay, Oid = o.Oid, TenPhongBan = o.BoPhan.TenBoPhan, TenCa = o.CC_CaChamCong1.TenCa });
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
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong_CoPhanTrang(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu)
        {
            int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamCong(date, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamChong_ThangVaNam(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);


            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //var result = (from o in this.ObjectSet
            //              where (o.Ngay.Year == nam && o.Ngay.Month == thang)
            //                  //&& (boPhanId == null || o.IDBoPhan == boPhanId)
            //                    && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
            //                    && (tatCaTrangThai || o.DaChamCong == daChamCong)
            //                    && (diHoc == null || o.DiHoc == diHoc)
            //                    && (tatCaMaNhanSu == true || o.HoSo.MaQuanLy.Contains(maNhanSu))
            //                    && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
            //              orderby o.Ngay, o.BoPhan.STT,o.HoSo.Ten
            var result = (from o in this.ObjectSet
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                //&& (boPhanId == null || o.IDBoPhan == boPhanId)
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                 && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                          orderby o.Ngay, o.BoPhan.STT, o.HoSo.Ten
                          select new DTO_QuanLyChamCong_Find() { DaChamCong = o.DaChamCong, HoTen = o.HoSo.HoTen, SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, IDHinhThucNghi = o.IDHinhThucNghi, Ngay = o.Ngay, Oid = o.Oid, TenPhongBan = o.BoPhan.TenBoPhan, TenCa = o.CC_CaChamCong1.TenCa });
            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong_ThangVaNam_CoPhanTrang(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu)
        {
            int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            var result = FindForQuanlyChamChong_ThangVaNam(thang, nam, boPhanId, trangThaiChamCong, diHoc, maNhanSu, webUserId, idLoaiNhanSu).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
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
            //Guid? idLoaiNhanSuGiangVien = new Guid("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                && (//boPhanId == Guid.Empty || 
                                o.IDBoPhan == boPhanId)
                                //&& (o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                          orderby o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.HSPCChucVu descending
                          ,
                          o.HoSo.NhanVien.ChucDanh1.MaQuanLy
                          ,
                          o.HoSo.Ten
                          , 
                          o.BoPhan.TenBoPhan
                          //Quét 4 lần
                          //select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVaoSang = o.GioVaoSang, GioRaSang = o.GioRaSang, GioVaoChieu = o.GioVaoChieu, GioRaChieu = o.GioRaChieu });
                          // Quét 2 lần
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVaoSang, GioRa = o.GioRaChieu });
            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(int ngay, int thang, int nam, Guid nhanVienID)
        {
            //Guid? idLoaiNhanSuGiangVien = new Guid("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                &&
                                o.IDNhanVien == nhanVienID
                                //&& o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                          orderby o.HoSo.Ten, o.BoPhan.TenBoPhan
                          //Quét 4 lần
                          //select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVaoSang = o.GioVaoSang, GioRaSang = o.GioRaSang, GioVaoChieu = o.GioVaoChieu, GioRaChieu = o.GioRaChieu });
                          // Quét 2 lần
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVaoSang, GioRa = o.GioRaChieu });
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
                          //select new DTO_QuanLyXetABC_BieuDoVaoRa() { MaNhanSu = o.HoSo.MaQuanLy, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVao, GioRa = o.GioRa });
                          select new DTO_QuanLyXetABC_BieuDoVaoRa() { MaNhanSu = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay });
            return result;
        }
        public DTO_QuanLyXetABC_ChiTietTheoNhanVien XetABC_ChiTietTheoNhanVien(int thang, int nam, Guid idNhanVien)
        {

            var result = (from o in this.Context.ChiTietChamCongNhanViens
                          where o.ThongTinNhanVien == idNhanVien

                          select new DTO_QuanLyXetABC_ChiTietTheoNhanVien()
                          {
                              MaNhanSu = o.ThongTinNhanVien1.SoHieuCongChuc,
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
                                                     MaHinhThucNghi = (x.IDHinhThucNghi == null ? "+" : x.HinhThucNghi.KyHieu)

                                                 }
                                                                  ).ToList()
                              //
                          }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu)
        {
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            //lay danh sach nhan vien
            var dsIdNhanVien = (from o in this.ObjectSet
                                where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                      && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                       && o.HoSo.GCRecord == null 
                                       //&& o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                orderby
                                //o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu1.MaQuanLy
                                //, 
                                o.HoSo.Ten
                                select o.IDNhanVien).Distinct();
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          orderby !hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên") descending //LaNhanVienToChucHanhChinh xếp trước
                          , 
                          hs.NhanVien.ThongTinNhanVien.ChucVu1.HSPCChucVu descending
                          , 
                          hs.NhanVien.ChucDanh1.MaQuanLy
                          , 
                          hs.Ten
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              MaNhanSu = hs.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")) : false)
                              ,
                              LaNhanVienGiangDay = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? ((hs.NhanVien.BoPhan1.MaQuanLy.ToLower().StartsWith("G")) && !hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.MaQuanLy.ToLower().Contains("NV")) : false)
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                 &&( o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId)&& o.IDNhanVien == idNv
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.KyHieu)
                                                     ,
                                                     GioVaoSang = o.GioVaoSang
                                                     ,
                                                     GioRaChieu = o.GioRaChieu
                                                     ,
                                                     QuanLyViPham = (from c in this.Context.CC_QuanLyViPham
                                                                     where c.CC_ChamCongTheoNgay.Oid == o.Oid
                                                                     select new DTO_CC_QuanLyViPham()
                                                                     {
                                                                         HinhThucViPham = c.CC_HinhThucViPham1.TenHinhThucViPham
                                                                         ,
                                                                         ThoiGianSom = c.ThoiGianSom!=null ? c.ThoiGianSom.ToString() : ""
                                                                         ,
                                                                         ThoiGianTre = c.ThoiGianTre != null? c.ThoiGianTre.ToString() : ""
                                                                     }).ToList()
                                                                     ,
                                                     QuanLyViPhamCount = (from c in this.Context.CC_QuanLyViPham
                                                                          where c.CC_ChamCongTheoNgay.Oid == o.Oid
                                                                          select 0).ToList().Count()
                                                                          ,
                                                     GhiChu = o.GhiChu
                                                 }
                                                              ).ToList()                                                             
                            ,
                              //  NgayCong = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //            where o.ThongTinNhanVien1.Oid==hs.Oid
                              //            && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang==thang
                              //            && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //           select o.SoNgayCong ?? (decimal?) 0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongHuongLuong = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                        where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                        select o.NghiCoPhep ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongDiHoc = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                        where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                        select o.NghiDiHocCoLuong ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0 + (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                                  where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                                  && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                                  && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                                  select o.NghiDiHocKhongLuong ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongKhongLuong = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                        where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                        select o.NghiRo ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongBHXH = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                        where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                        select o.NghiThaiSan ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              NgayCong = (22 - (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                            && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                            && o.IDHinhThucNghi != null && o.HinhThucNghi.PhanLoai != 2
                                                          select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()) < 0 ? 0 : 22 - (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                            && ( o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                                                                                                            && o.IDHinhThucNghi != null && o.HinhThucNghi.PhanLoai != 2
                                                                                                                                          select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiNuaNgay = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                         && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                          && o.IDHinhThucNghi == HinhThucNghiConst.NghiNuaNgayId
                                                       select 0).Count()
                              ,
                              NghiKhongPhepRo = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                           where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                             && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId )&& o.IDNhanVien == idNv
                                                                              && o.HinhThucNghi.PhanLoai == 0 && o.IDHinhThucNghi != HinhThucNghiConst.NghiNuaNgayId
                                                           select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                         where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                           && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                                                                                                            && o.HinhThucNghi.PhanLoai == 0 && o.IDHinhThucNghi != HinhThucNghiConst.NghiNuaNgayId
                                                                                                                                         select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              //Nghỉ không phép + nghỉ nửa ngày
                              NghiTruLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                           where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                             && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                              && o.HinhThucNghi.PhanLoai == 0
                                                           select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                         where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                           && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId )&& o.IDNhanVien == idNv
                                                                                                                                                            && o.HinhThucNghi.PhanLoai == 0
                                                                                                                                         select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiPhep = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                    where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                      && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId )&& o.IDNhanVien == idNv
                                                                       && o.HinhThucNghi.PhanLoai == 2
                                                    select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                  where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                    && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                                                                                                     && o.HinhThucNghi.PhanLoai == 2
                                                                                                                                  select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiThaiSan = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                         && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId )&& o.IDNhanVien == idNv
                                                                         && o.HinhThucNghi.PhanLoai == 1
                                                       select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                     where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                       && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId )&& o.IDNhanVien == idNv
                                                                                                                                                       && o.HinhThucNghi.PhanLoai == 1
                                                                                                                                     select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiOmDau = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                         && ( o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                         && o.HinhThucNghi.PhanLoai == 5
                                                       select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                     where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                       && (o.IDBoPhan == boPhanId || o.BoPhan.BoPhanCha == boPhanId) && o.IDNhanVien == idNv
                                                                                                                                                       && o.HinhThucNghi.PhanLoai == 5
                                                                                                                                     select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()

                          });

            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(int thang, int nam, Guid nhanVienID)
        {

            //lay danh sach nhan vien
            var dsIdNhanVien = (from o in this.ObjectSet
                                where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                      && o.IDNhanVien == nhanVienID
                                       && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                select o.IDNhanVien).Distinct();
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              MaNhanSu = hs.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")) : false)
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                 && o.IDNhanVien == idNv
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.KyHieu)
                                                     ,
                                                     GioVaoSang = o.GioVaoSang
                                                     ,
                                                     GioRaChieu = o.GioRaChieu
                                                     ,
                                                 }
                                                              ).ToList()
                            ,
                              //  NgayCong = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                        where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                        select o.SoNgayCong ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongHuongLuong = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                              where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                              && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                              && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                              select o.NghiCoPhep ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongDiHoc = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                         where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                         && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                         && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                         select o.NghiDiHocCoLuong ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0 + (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                                                 where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                                                 && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                                                 && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                                                 select o.NghiDiHocKhongLuong ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongKhongLuong = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                              where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                              && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                              && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                              select o.NghiRo ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              //,
                              //  TongBHXH = (decimal?)(from o in this.Context.ChiTietChamCongNhanViens
                              //                        where o.ThongTinNhanVien1.Oid == hs.Oid
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                              //                        && o.QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                              //                        select o.NghiThaiSan ?? (decimal?)0
                              //            ).FirstOrDefault() ?? 0
                              NgayCong = 22 - ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                            && o.IDNhanVien == idNv
                                                                            && o.IDHinhThucNghi != null && o.HinhThucNghi.PhanLoai != 2
                                                          select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()) < 0 ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                     where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                       && o.IDNhanVien == idNv
                                                                                                                                                       && o.IDHinhThucNghi != null && o.HinhThucNghi.PhanLoai != 2
                                                                                                                                     select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiNuaNgay = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                         && o.IDNhanVien == idNv
                                                                          && o.IDHinhThucNghi == HinhThucNghiConst.NghiNuaNgayId
                                                       select 0).Count()
                              ,
                              NghiKhongPhepRo = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                           where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                             && o.IDNhanVien == idNv
                                                                              && o.HinhThucNghi.PhanLoai == 0 && o.IDHinhThucNghi != HinhThucNghiConst.NghiNuaNgayId
                                                           select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                         where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                           && o.IDNhanVien == idNv
                                                                                                                                                            && o.HinhThucNghi.PhanLoai == 0 && o.IDHinhThucNghi != HinhThucNghiConst.NghiNuaNgayId
                                                                                                                                         select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiTruLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                        where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                          && o.IDNhanVien == idNv
                                                                           && o.HinhThucNghi.PhanLoai == 0
                                                        select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                      where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                        && o.IDNhanVien == idNv
                                                                                                                                                         && o.HinhThucNghi.PhanLoai == 0
                                                                                                                                      select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiPhep = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                    where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                      && o.IDNhanVien == idNv
                                                                       && o.HinhThucNghi.PhanLoai == 2
                                                    select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                  where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                    && o.IDNhanVien == idNv
                                                                                                                                                     && o.HinhThucNghi.PhanLoai == 2
                                                                                                                                  select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiThaiSan = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                         && o.IDNhanVien == idNv
                                                                         && o.HinhThucNghi.PhanLoai == 1
                                                       select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                     where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                       && o.IDNhanVien == idNv
                                                                                                                                                       && o.HinhThucNghi.PhanLoai == 1
                                                                                                                                     select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                              ,
                              NghiOmDau = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                     where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                       && o.IDNhanVien == idNv
                                                                       && o.HinhThucNghi.PhanLoai == 5
                                                     select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum() == null ? 0 : (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                                                                   where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                                                                                                                     && o.IDNhanVien == idNv
                                                                                                                                                     && o.HinhThucNghi.PhanLoai == 5
                                                                                                                                   select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum()
                          });

            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_VanTayVaoRa(int thang, int nam, Guid boPhanID, Guid nhanVienID)
        {
            bool tatCaMaNhanSu = nhanVienID == Guid.Empty ? true : false;
            //lay danh sach nhan vien
            //orderby và distinct viết đồng thời thì order không có tác dụng
            var dsIdNhanVien = (from o in this.ObjectSet
                                where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                      && (o.IDBoPhan == boPhanID)
                                      && (tatCaMaNhanSu || o.IDNhanVien == nhanVienID)
                                       && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                orderby o.HoSo.Ten
                                select o.IDNhanVien).Distinct();

            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          orderby hs.NhanVien.ThongTinNhanVien.ChucVu1.HSPCChucVu descending, hs.NhanVien.ChucDanh1.MaQuanLy, hs.Ten
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              MaNhanSu = hs.IDNhanSu_ChamCong.HasValue ? hs.IDNhanSu_ChamCong.Value.ToString() : null
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 join vipham in this.Context.CC_QuanLyViPham_New on o.Oid equals vipham.CC_ChamCongTheoNgay.HasValue ? vipham.CC_ChamCongTheoNgay.Value : Guid.Empty
                                                 where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                 && o.IDNhanVien == idNv
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = (vipham != null && vipham.ChuoiHinhThucViPham != null) ? vipham.ChuoiHinhThucViPham : ""
                                                     ,
                                                     GioVaoSang = o.GioVaoSang
                                                     ,
                                                     GioRaChieu = o.GioRaChieu
                                                     ,
                                                 })
                          });
            return result;
        }
        public bool CheckDangSuDung(Guid Oid)
        {
            bool result = false;
            result = this.ObjectSet.Any(c => c.CC_CaChamCong == Oid);
            return result;
        }
        public IEnumerable<DTO_NgayChamCong> GetList_NgayTrongKyChamCong(int thang, int nam)
        {
            List<DTO_NgayChamCong> list = new List<DTO_NgayChamCong>();
            int ngaycuoi=DateTime.DaysInMonth(nam, thang);
            DateTime tuNgay = new DateTime(nam, thang, 1);
            DateTime denNgay = new DateTime(nam, thang, ngaycuoi);
            //Tao danh sach ngay
            var dates = new List<DateTime>();
            for (var dt = tuNgay; dt <= denNgay; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

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
