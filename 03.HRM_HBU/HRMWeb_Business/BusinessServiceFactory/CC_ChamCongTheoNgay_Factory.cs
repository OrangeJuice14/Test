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
using System.Data.Entity.SqlServer;

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
        public bool CheckDangSuDung(Guid Oid)
        {
            bool result = false;
            result = this.ObjectSet.Any(c => c.CC_CaChamCong == Oid);
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
        public CC_ChamCongTheoNgay GetByDate(DateTime date, Guid? idNV)
        {
            var result = (from o in this.ObjectSet
                          where o.IDNhanVien == idNV && o.Ngay == date
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
        public IQueryable<HoSo> List_NVMoi_ChuaCo_CCTheoNgay(int thang, int nam)
        {
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            DateTime tuNgay = kyChamCong.TuNgay;
            DateTime denNgay = kyChamCong.DenNgay;
            var dsCC = (from o in this.ObjectSet
                        where o.Ngay >= tuNgay && o.Ngay <= denNgay
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

        public IQueryable<CC_ChamCongNgayNghi> DSNhanVienCoTrongCCNgayNghiTheoThangNam(int thang, int nam)
        {
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            var result = (from o in this.Context.CC_ChamCongNgayNghi
                          where
                          (o.TuNgay <= kyChamCong.TuNgay && o.DenNgay >= kyChamCong.TuNgay)
                          ||
                          (o.TuNgay <= kyChamCong.TuNgay && o.DenNgay >= kyChamCong.DenNgay)
                          ||
                          (o.TuNgay >= kyChamCong.TuNgay && o.DenNgay <= kyChamCong.DenNgay)
                          ||
                          (o.TuNgay >= kyChamCong.TuNgay && o.DenNgay >= kyChamCong.DenNgay)
                          select o);
            return result;
        }

        public IQueryable<CC_KhaiBaoCongTac> DSNhanVienKhaiBaoCongTacTheoThangNam(int thang, int nam)
        {
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            var result = (from o in this.Context.CC_KhaiBaoCongTac
                          where
                          (o.TuNgay <= kyChamCong.TuNgay && o.DenNgay >= kyChamCong.TuNgay)
                          ||
                          (o.TuNgay <= kyChamCong.TuNgay && o.DenNgay >= kyChamCong.DenNgay)
                          ||
                          (o.TuNgay >= kyChamCong.TuNgay && o.DenNgay <= kyChamCong.DenNgay)
                          ||
                          (o.TuNgay >= kyChamCong.TuNgay && o.DenNgay >= kyChamCong.DenNgay)
                          && o.TrangThai == 1
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
        public IEnumerable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            //List<DTO_QuanLyChamCong_Find> result = new List<DTO_QuanLyChamCong_Find>();
            List<DTO_QuanLyChamCong_Find> result2 = new List<DTO_QuanLyChamCong_Find>();
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);

            CC_KyChamCong kyChamCong = CC_KyChamCong_Factory.New().GetByMonthAndYear(date.Month, date.Year);
            //lay danh sach nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
            List<Guid> dsIdNhanVien2 = (from o in this.Context.QuyetDinhThoiViecs
                                        where o.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && kyChamCong.TuNgay < o.NghiViecTuNgay
                                        //&& o.NghiViecTuNgay <= kyChamCong.DenNgay
                                        && o.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == boPhanId
                                        select o.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIdNhanVien3 = (from o in this.Context.QuyetDinhNghiHuus
                                        where o.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && kyChamCong.TuNgay < o.NghiViecTuNgay
                                        //&& o.NghiViecTuNgay <= kyChamCong.DenNgay
                                        && o.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == boPhanId
                                        select o.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIDNVTotal = dsIdNhanVien3.Union(dsIdNhanVien2).Union(dsIdNhanVien3).Distinct().ToList();

            //Lấy trong cc theo ngày
            List<DTO_QuanLyChamCong_Find> result1 = (from o in this.ObjectSet
                                                     where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                                                           //&& (boPhanId == null || o.IDBoPhan == boPhanId)
                                                           && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                                           && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                                           && (diHoc == null || o.DiHoc == diHoc)
                                                           && (tatCaMaNhanSu == true || o.HoSo.MaQuanLy.Contains(maNhanSu))
                                                           && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                                           && (o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong ?? false) == false
                                                     //orderby o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu != null ? o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999, o.HoSo.Ten, o.BoPhan.TenBoPhan
                                                     orderby o.HoSo.HoTen
                                                     select new DTO_QuanLyChamCong_Find()
                                                     {
                                                         DaChamCong = o.DaChamCong,
                                                         ThuTuChucVu = o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu.ToString() != "" ? o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu.ToString() : "9999",
                                                         HoTen = o.HoSo.HoTen,
                                                         MaNhanSu = o.HoSo.MaQuanLy,
                                                         IDHinhThucNghi = o.IDHinhThucNghi,
                                                         Ngay = o.Ngay,
                                                         Oid = o.Oid,
                                                         TenPhongBan = o.BoPhan.TenBoPhan,
                                                         IDNhanVien = o.IDNhanVien
                                                     }).ToList();
            //Lấy chấm công theo ngày của nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
            foreach (Guid idNv in dsIDNVTotal)
            {
                //nếu đã có trong cc theo ngày thì ko lấy nữa
                bool tontai = result1.Any(cc => cc.IDNhanVien == idNv);
                if (!tontai)
                {
                    DTO_QuanLyChamCong_Find item = (from o in this.ObjectSet
                                                    where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                                                          && o.IDNhanVien == idNv
                                                    select new DTO_QuanLyChamCong_Find()
                                                    {
                                                        DaChamCong = o.DaChamCong,
                                                        ThuTuChucVu = o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu.ToString() != "" ? o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu.ToString() : "9999",
                                                        HoTen = o.HoSo.HoTen,
                                                        MaNhanSu = o.HoSo.MaQuanLy,
                                                        IDHinhThucNghi = o.IDHinhThucNghi,
                                                        Ngay = o.Ngay,
                                                        Oid = o.Oid,
                                                        TenPhongBan = o.BoPhan.TenBoPhan,
                                                        IDNhanVien = o.IDNhanVien
                                                    }).SingleOrDefault();
                    result2.Add(item);
                }
            }
            var result = result1.Union(result2);
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
        public IEnumerable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong_CoPhanTrang(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId, Guid? idLoaiNhanSu)
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
            var result = (from o in this.ObjectSet
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                //&& (boPhanId == null || o.IDBoPhan == boPhanId)
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.MaQuanLy.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                && (o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong ?? false) == false
                          //orderby o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu != null ? o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999, o.HoSo.Ten, o.BoPhan.TenBoPhan
                          orderby o.HoSo.HoTen
                          select new DTO_QuanLyChamCong_Find() { DaChamCong = o.DaChamCong, HoTen = o.HoSo.HoTen, MaNhanSu = o.HoSo.MaQuanLy, IDHinhThucNghi = o.IDHinhThucNghi, Ngay = o.Ngay, Oid = o.Oid, TenPhongBan = o.BoPhan.TenBoPhan });
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
            Guid? idLoaiNhanSuGiangVien = new Guid("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                && (//boPhanId == Guid.Empty || 
                                o.IDBoPhan == boPhanId)
                                && o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien
                          orderby o.HoSo.Ten, o.BoPhan.TenBoPhan
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { MaNhanSu = o.HoSo.MaQuanLy, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVao, GioRa = o.GioRa });
            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(int ngay, int thang, int nam, Guid nhanVienID)
        {
            Guid? idLoaiNhanSuGiangVien = new Guid("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                &&
                                o.IDNhanVien == nhanVienID
                                && o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien
                          orderby o.HoSo.Ten, o.BoPhan.TenBoPhan
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { MaNhanSu = o.HoSo.MaQuanLy, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVao, GioRa = o.GioRa });
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
                          select new DTO_QuanLyXetABC_BieuDoVaoRa() { MaNhanSu = o.HoSo.MaQuanLy, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVao, GioRa = o.GioRa });
            return result;
        }
        public DTO_QuanLyXetABC_ChiTietTheoNhanVien XetABC_ChiTietTheoNhanVien(int thang, int nam, Guid idNhanVien)
        {

            var result = (from o in this.Context.ChiTietChamCongNhanViens
                          where o.ThongTinNhanVien == idNhanVien

                          select new DTO_QuanLyXetABC_ChiTietTheoNhanVien()
                          {
                              MaNhanSu = o.ThongTinNhanVien1.NhanVien.HoSo.MaQuanLy,
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
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu)
        {
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            var saturday = (int)DayOfWeek.Saturday + 1;
            DateTime tuNgay = kyChamCong.TuNgay;
            DateTime denNgay = kyChamCong.DenNgay;
            decimal soNgayLamViec = this.Context.CreateQuery<decimal>(
                                     "SELECT VALUE ERPModel.Store.func_TinhSoNgayLamViecTrongThang(@TuNgay,@DenNgay) FROM {1}",
                                     new ObjectParameter("TuNgay", tuNgay),
                                      new ObjectParameter("DenNgay", denNgay)
                                 ).First();
            //lay danh sach nhan vien
            List<Guid> dsIdNhanVien = (from o in this.ObjectSet
                                           //where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                       where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                       && (o.IDBoPhan == boPhanId)
                                             && (tatCaMaNhanSu || o.HoSo.MaQuanLy == maNhanSu)
                                             && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                             && (o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong ?? false) == false
                                       orderby
                                       //o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu1.MaQuanLy
                                       //, 
                                       o.HoSo.Ten
                                       select o.IDNhanVien).Distinct().ToList();
            //lay danh sach nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
            List<Guid> dsIdNhanVien2 = (from o in this.Context.QuyetDinhThoiViecs
                                        where o.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && tuNgay < o.NghiViecTuNgay
                                        //&& o.NghiViecTuNgay <= denNgay
                                        && o.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == boPhanId
                                        select o.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIdNhanVien3 = (from o in this.Context.QuyetDinhNghiHuus
                                        where o.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && tuNgay < o.NghiViecTuNgay
                                        //&& o.NghiViecTuNgay <= denNgay
                                        && o.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == boPhanId
                                        select o.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIDNVTotal = dsIdNhanVien.Union(dsIdNhanVien2).Union(dsIdNhanVien3).ToList();
            var result = (from idNv in dsIDNVTotal
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          //orderby ((hs.NhanVien.ThongTinNhanVien.ChucVu1 != null && hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu != null) ? hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999), hs.Ten
                          orderby hs.HoTen
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.MaQuanLy
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")) : false)
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                     //where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                 && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.MaQuanLy),
                                                     KyHieu = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.KyHieu),
                                                     DaThayDoi = this.Context.CC_ChamCongTheoNgayThayDoi.Any(cc => cc.Oid == o.Oid)
                                                     //,
                                                     //DaChamCong = o.DaChamCong
                                                 }
                                                              ).ToList()//.OrderBy(x => x.Ngay)
                            ,
                              //Số ngày làm việc - (Phép + không lương + BHXH)
                              HuongLuong = soNgayLamViec - (((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                        where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                          && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                                          && o.HinhThucNghi.PhanLoai == 2
                                                                        select (
                                                                       SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                        (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0)
                                                            + ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                            && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                                            && o.HinhThucNghi.PhanLoai == 0
                                                                          select (
                                                                         SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                          (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0)
                                                            + ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                            && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                                            && o.HinhThucNghi.PhanLoai == 1
                                                                          select (
                                                                         SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                          (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0)),
                              //Loại 2
                              Phep = ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                   && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                   && o.HinhThucNghi.PhanLoai == 2
                                                 select (
                                                SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                 (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0),
                              //Loại 0
                              KhongLuong = ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                         && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                         && o.HinhThucNghi.PhanLoai == 0
                                                       select (
                                                      SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                       (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0),
                              //Loại 1
                              BHXH = ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                   && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                   && o.HinhThucNghi.PhanLoai == 1
                                                 select (
                                                SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                 (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0),
                              //Số ngày làm việc - (Không lương + BHXH)
                              TongCong = soNgayLamViec- (((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                     where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                       && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                                       && o.HinhThucNghi.PhanLoai == 0
                                                                     select (
                                                                    SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                     (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0)
                                                            + ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                            && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                                            && o.HinhThucNghi.PhanLoai == 1
                                                                          select (
                                                                         SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                          (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum()??0))
                              //HuongLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                        where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                          && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                              //                                          //&& (o.IDHinhThucNghi == null || o.IDHinhThucNghi == HinhThucNghiConst.LamNuaNgayId || o.IDHinhThucNghi == HinhThucNghiConst.DiCongTacId || o.IDHinhThucNghi == HinhThucNghiConst.DiCTNuaNgayId)
                              //                                          && (o.HinhThucNghi.PhanLoai==null || o.IDHinhThucNghi == null || o.HinhThucNghi.PhanLoai == 3)
                              //                        select (o.IDHinhThucNghi == null ? 1 : o.HinhThucNghi.GiaTri)).Sum(),
                              //Phep = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                  where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                    && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                              //                                    && (o.HinhThucNghi.PhanLoai == 2 || o.HinhThucNghi.PhanLoai == 3)
                              //                  select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                              //KhongLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                        where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                          && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                              //                                          && (o.HinhThucNghi.PhanLoai == 0)
                              //                        select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                              //BHXH = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                  where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                    && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                              //                                    && (o.HinhThucNghi.PhanLoai == 1)
                              //                  select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                              //TongCong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                      where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                        && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                              //                                        //&& (o.IDHinhThucNghi == null || o.IDHinhThucNghi == HinhThucNghiConst.LamNuaNgayId || o.HinhThucNghi.PhanLoai == 2)
                              //                                        && (o.HinhThucNghi.PhanLoai==null || o.HinhThucNghi.PhanLoai==2 || o.IDHinhThucNghi == null || o.HinhThucNghi.PhanLoai == 3)
                              //                      select ((o.IDHinhThucNghi == null || o.HinhThucNghi.PhanLoai == 3 )? 1 : o.HinhThucNghi.GiaTri)).Sum()
                          });
            //.OrderByDescending(x => x.LaNhanVienToChucHanhChinh);
            return result;
        }
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThangDonVi(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu)
        {
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            DateTime tuNgay = kyChamCong.TuNgay;
            DateTime denNgay = kyChamCong.DenNgay;
            var CC_TheoNgayThayDoi = (from cc in this.Context.CC_ChamCongTheoNgay
                                      join td in this.Context.CC_ChamCongTheoNgayThayDoi on cc.Oid equals td.Oid
                                      select new
                                      {
                                          Oid = cc.Oid,
                                          Ngay = cc.Ngay,
                                          IDBoPhan = cc.IDBoPhan,
                                          IDNhanVien = cc.IDNhanVien,
                                          IDHinhThucNghi = td.IDHinhThucNghiDonViCham,
                                          IDUser = cc.IDUser,
                                          IDHinhThucNghiDonViCham = td.IDHinhThucNghiDonViCham,
                                          IDHinhThucNghiThayDoi = td.IDHinhThucNghiThayDoi
                                      });
            List<Guid> thayDoiList = new List<Guid>();
            Guid? temp = Guid.Empty;
            thayDoiList = CC_TheoNgayThayDoi.Select(c => c.Oid).ToList();
            var CC_TheoNgay = (from cc in this.Context.CC_ChamCongTheoNgay
                               where !thayDoiList.Contains(cc.Oid)
                               select new
                               {
                                   Oid = cc.Oid,
                                   Ngay = cc.Ngay,
                                   IDBoPhan = cc.IDBoPhan,
                                   IDNhanVien = cc.IDNhanVien,
                                   IDHinhThucNghi = cc.IDHinhThucNghi,
                                   IDUser = cc.IDUser,
                                   IDHinhThucNghiDonViCham = temp,
                                   IDHinhThucNghiThayDoi = temp
                               });
            var CC_ChamCongTheoNgay = CC_TheoNgayThayDoi.Union(CC_TheoNgay);
            //lay danh sach nhan vien
            var dsIdNhanVien = (from o in this.ObjectSet
                                    //where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                && (o.IDBoPhan == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.MaQuanLy == maNhanSu)
                                      && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                      && (o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong ?? false) == false
                                orderby
                                //o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu1.MaQuanLy
                                //, 
                                o.HoSo.Ten
                                select o.IDNhanVien).Distinct();
            //lay danh sach nhan vien có trong quyết định thôi việc và quyết định nghỉ hưu
            List<Guid> dsIdNhanVien2 = (from o in this.Context.QuyetDinhThoiViecs
                                        where o.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && tuNgay < o.NghiViecTuNgay
                                        //&& o.NghiViecTuNgay <= denNgay
                                        && o.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == boPhanId
                                        select o.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIdNhanVien3 = (from o in this.Context.QuyetDinhNghiHuus
                                        where o.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && tuNgay < o.NghiViecTuNgay
                                        //&& o.NghiViecTuNgay <= denNgay
                                        && o.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == boPhanId
                                        select o.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIDNVTotal = dsIdNhanVien.Union(dsIdNhanVien2).Union(dsIdNhanVien3).ToList();
            var result = (from idNv in dsIDNVTotal
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          orderby hs.NhanVien.ThongTinNhanVien.ChucVu1?.ThuTu != null ? hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999, hs.Ten
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.MaQuanLy
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")) : false)
                              ,
                              ChiTietChamCong = (from o in CC_ChamCongTheoNgay
                                                 join h in this.Context.HinhThucNghis on o.IDHinhThucNghi equals h.Oid
                                                 into a
                                                 from b in a.DefaultIfEmpty(null)
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                 && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : b.MaQuanLy),
                                                     KyHieu = (o.IDHinhThucNghi == null ? "+" : b.KyHieu),
                                                     DaThayDoi = this.Context.CC_ChamCongTheoNgayThayDoi.Any(cc => cc.Oid == o.Oid)
                                                     //,
                                                     //DaChamCong = o.DaChamCong
                                                 }
                                                              ).ToList()//.OrderBy(x => x.Ngay)
                            ,
                              HuongLuong = (decimal?)(from o in CC_ChamCongTheoNgay
                                                      join h in this.Context.HinhThucNghis on o.IDHinhThucNghi equals h.Oid
                                                      into a
                                                      from b in a.DefaultIfEmpty(null)
                                                      where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                        && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                        //&& (o.IDHinhThucNghi == null || o.IDHinhThucNghi == HinhThucNghiConst.LamNuaNgayId || o.IDHinhThucNghi == HinhThucNghiConst.DiCongTacId || o.IDHinhThucNghi == HinhThucNghiConst.DiCTNuaNgayId)
                                                                        && (b.PhanLoai == 3 || b.PhanLoai == null || o.IDHinhThucNghi == null)
                                                      select (o.IDHinhThucNghi == null ? 1 : b.GiaTri)).Sum(),
                              Phep = (decimal?)(from o in CC_ChamCongTheoNgay
                                                join h in this.Context.HinhThucNghis on o.IDHinhThucNghi equals h.Oid
                                                into a
                                                from b in a.DefaultIfEmpty(null)
                                                where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                  && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                  && (b.PhanLoai == 2 || b.PhanLoai == 3)
                                                select (b.GiaTri ?? (decimal?)0)).Sum(),
                              KhongLuong = (decimal?)(from o in CC_ChamCongTheoNgay
                                                      join h in this.Context.HinhThucNghis on o.IDHinhThucNghi equals h.Oid
                                                      into a
                                                      from b in a.DefaultIfEmpty(null)
                                                      where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                        && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                        && (b.PhanLoai == 0)
                                                      select (b.GiaTri ?? (decimal?)0)).Sum(),
                              BHXH = (decimal?)(from o in CC_ChamCongTheoNgay
                                                join h in this.Context.HinhThucNghis on o.IDHinhThucNghi equals h.Oid
                                                into a
                                                from b in a.DefaultIfEmpty(null)
                                                where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                  && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                  && (b.PhanLoai == 1)
                                                select (b.GiaTri ?? (decimal?)0)).Sum(),
                              TongCong = (decimal?)(from o in CC_ChamCongTheoNgay
                                                    join h in this.Context.HinhThucNghis on o.IDHinhThucNghi equals h.Oid
                                                    into a
                                                    from b in a.DefaultIfEmpty(null)
                                                    where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                      && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                                      //&& (o.IDHinhThucNghi == null || o.IDHinhThucNghi == HinhThucNghiConst.LamNuaNgayId || b.PhanLoai == 2)
                                                                      && (b.PhanLoai == null || b.PhanLoai == 2 || o.IDHinhThucNghi == null)
                                                    select ((o.IDHinhThucNghi == null || b.PhanLoai == 3) ? 1 : b.GiaTri)).Sum()
                          });
            //.OrderByDescending(x => x.LaNhanVienToChucHanhChinh);
            return result;
        }
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang_All> QuanLyChamCong_ThongTinChamCongThang_All(int thang, int nam, string webUserId)
        {
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            DateTime tuNgay = kyChamCong.TuNgay;
            DateTime denNgay = kyChamCong.DenNgay;
            Guid UserId = webUserId != "" ? new Guid(webUserId) : Guid.Empty;
            var dsBoPhans = (from o in this.Context.BoPhans
                             where o.WebUser_BoPhan.Any(x => x.IDWebUser == UserId)
                             && o.BoPhanCha != null
                             && o.GCRecord == null
                             orderby o.STT ascending
                             select o).ToList();
            var result = (from boPhan in dsBoPhans
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang_All()
                          {
                              TenBoPhan = boPhan.TenBoPhan,
                              STT = boPhan.STT.ToString(),
                              ThongTinChamCongThang = (from idNv in (from o in this.ObjectSet
                                                                     where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                     && (o.IDBoPhan == boPhan.Oid)
                                                                     && (o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong ?? false) == false
                                                                     orderby
                                                                     o.HoSo.Ten
                                                                     select o.IDNhanVien).Distinct()
                                                       join hs in this.Context.HoSoes on idNv equals hs.Oid
                                                       orderby hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu != null ? hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999, hs.Ten
                                                       select new DTO_QuanLyChamCong_ThongTinChamCongThang()
                                                       {

                                                           IDNhanVien = idNv
                                                           ,
                                                           HoTen = hs.HoTen
                                                           ,
                                                           Ten = hs.Ten
                                                           ,
                                                           MaNhanSu = hs.MaQuanLy
                                                           ,
                                                           LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")) : false)
                                                           ,
                                                           ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                                              where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                              && o.IDBoPhan == boPhan.Oid && o.IDNhanVien == idNv
                                                                              orderby o.Ngay ascending
                                                                              select new DTO_QuanLyChamCong_ChamCongNgay()
                                                                              {
                                                                                  CC_ChamCongTheoNgayOid = o.Oid
                                                                                  ,
                                                                                  IDNhanVien = o.IDNhanVien
                                                                                  ,
                                                                                  Ngay = o.Ngay
                                                                                  ,
                                                                                  MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.MaQuanLy),
                                                                                  KyHieu = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.KyHieu),
                                                                              }
                                                                                           ).ToList()
                                                         ,
                                                           HuongLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                   where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                                     && o.IDBoPhan == boPhan.Oid && o.IDNhanVien == idNv
                                                                                                    //&& (o.IDHinhThucNghi == null || o.IDHinhThucNghi == HinhThucNghiConst.LamNuaNgayId || o.IDHinhThucNghi == HinhThucNghiConst.DiCongTacId || o.IDHinhThucNghi == HinhThucNghiConst.DiCTNuaNgayId)
                                                                                                    && (o.HinhThucNghi.PhanLoai == 3 || o.HinhThucNghi.PhanLoai == null || o.IDHinhThucNghi == null)
                                                                                   select (o.IDHinhThucNghi == null ? 1 : o.HinhThucNghi.GiaTri)).Sum(),
                                                           Phep = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                             where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                               && o.IDBoPhan == boPhan.Oid && o.IDNhanVien == idNv
                                                                                               && (o.HinhThucNghi.PhanLoai == 3 || o.HinhThucNghi.PhanLoai == 2)
                                                                             select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                                                           KhongLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                   where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                                     && o.IDBoPhan == boPhan.Oid && o.IDNhanVien == idNv
                                                                                                     && (o.HinhThucNghi.PhanLoai == 0)
                                                                                   select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                                                           BHXH = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                             where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                               && o.IDBoPhan == boPhan.Oid && o.IDNhanVien == idNv
                                                                                               && (o.HinhThucNghi.PhanLoai == 1)
                                                                             select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                                                           TongCong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                                   && o.IDBoPhan == boPhan.Oid && o.IDNhanVien == idNv
                                                                                                    //&& (o.IDHinhThucNghi == null || o.IDHinhThucNghi == HinhThucNghiConst.LamNuaNgayId || o.HinhThucNghi.PhanLoai == 2)
                                                                                                    && (o.HinhThucNghi.PhanLoai == null || o.HinhThucNghi.PhanLoai == 2 || o.IDHinhThucNghi == null)
                                                                                 select (o.IDHinhThucNghi == null ? 1 : o.HinhThucNghi.GiaTri)).Sum()
                                                       }).ToList()
                          });
            return result;
        }
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(int thang, int nam, Guid nhanVienID)
        {
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            var saturday = (int)DayOfWeek.Saturday + 1;
            DateTime tuNgay = kyChamCong.TuNgay;
            DateTime denNgay = kyChamCong.DenNgay;
            decimal soNgayLamViec = this.Context.CreateQuery<decimal>(
                                     "SELECT VALUE ERPModel.Store.func_TinhSoNgayLamViecTrongThang(@TuNgay,@DenNgay) FROM {1}",
                                     new ObjectParameter("TuNgay", tuNgay),
                                      new ObjectParameter("DenNgay", denNgay)
                                 ).First();
            //lay danh sach nhan vien
            List<Guid> dsIdNhanVien = new List<Guid>();
            dsIdNhanVien.Add(nhanVienID);
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              MaNhanSu = hs.MaQuanLy
                              ,
                              LaNhanVienToChucHanhChinh = !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên"))
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
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
                                                 }
                                                              ).ToList()
                            ,
                              //Số ngày làm việc - (Phép + không lương + BHXH)
                              HuongLuong = soNgayLamViec - (((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                        where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                          && o.IDNhanVien == idNv
                                                                                          && o.HinhThucNghi.PhanLoai == 2
                                                                        select (
                                                                       SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                        (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0)
                                                            + ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                            && o.IDNhanVien == idNv
                                                                                            && o.HinhThucNghi.PhanLoai == 0
                                                                          select (
                                                                         SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                          (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0)
                                                            + ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                            && o.IDNhanVien == idNv
                                                                                            && o.HinhThucNghi.PhanLoai == 1
                                                                          select (
                                                                         SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                          (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0)),
                              //Loại 2
                              Phep = ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                   && o.IDNhanVien == idNv
                                                                   && o.HinhThucNghi.PhanLoai == 2
                                                 select (
                                                SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                 (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0),
                              //Loại 0
                              KhongLuong = ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                       where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                         && o.IDNhanVien == idNv
                                                                         && o.HinhThucNghi.PhanLoai == 0
                                                       select (
                                                      SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                       (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0),
                              //Loại 1
                              BHXH = ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                   && o.IDNhanVien == idNv
                                                                   && o.HinhThucNghi.PhanLoai == 1
                                                 select (
                                                SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                 (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0),
                              //Số ngày làm việc - (Không lương + BHXH)
                              TongCong = soNgayLamViec - (((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                      where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                        && o.IDNhanVien == idNv
                                                                                        && o.HinhThucNghi.PhanLoai == 0
                                                                      select (
                                                                     SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                      (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0)
                                                            + ((decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                                            && o.IDNhanVien == idNv
                                                                                            && o.HinhThucNghi.PhanLoai == 1
                                                                          select (
                                                                         SqlFunctions.DatePart("dw", o.Ngay) == saturday ?
                                                                          (o.HinhThucNghi.GiaTri ?? (decimal?)0) / 2 : o.HinhThucNghi.GiaTri ?? (decimal?)0)
                                                            ).Sum() ?? 0))
                              //HuongLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                        where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                          && o.IDNhanVien == idNv
                              //                                          && (o.HinhThucNghi.PhanLoai == null || o.IDHinhThucNghi == null || o.HinhThucNghi.PhanLoai == 3)
                              //                        select (o.IDHinhThucNghi == null ? 1 : o.HinhThucNghi.GiaTri)).Sum(),
                              //Phep = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                  where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                    && o.IDNhanVien == idNv
                              //                                    && (o.HinhThucNghi.PhanLoai == 2 || o.HinhThucNghi.PhanLoai == 3)
                              //                  select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                              //KhongLuong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                        where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                          && o.IDNhanVien == idNv
                              //                                          && (o.HinhThucNghi.PhanLoai == 0)
                              //                        select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                              //BHXH = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                  where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                    && o.IDNhanVien == idNv
                              //                                    && (o.HinhThucNghi.PhanLoai == 1)
                              //                  select (o.HinhThucNghi.GiaTri ?? (decimal?)0)).Sum(),
                              //TongCong = (decimal?)(from o in this.Context.CC_ChamCongTheoNgay
                              //                      where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                              //                                        && o.IDNhanVien == idNv
                              //                                        && (o.HinhThucNghi.PhanLoai == null || o.HinhThucNghi.PhanLoai == 2 || o.IDHinhThucNghi == null || o.HinhThucNghi.PhanLoai == 3)
                              //                      select ((o.IDHinhThucNghi == null || o.HinhThucNghi.PhanLoai == 3) ? 1 : o.HinhThucNghi.GiaTri)).Sum()

                          });

            return result;
        }
        public IEnumerable<DTO_NgayChamCong> GetList_NgayTrongKyChamCong(int thang, int nam)
        {
            List<DTO_NgayChamCong> list = new List<DTO_NgayChamCong>();
            var kyChamCong = (from o in this.Context.CC_KyChamCong where o.Thang == thang && o.Nam == nam select o).SingleOrDefault();
            DateTime tuNgay = kyChamCong.TuNgay;
            DateTime denNgay = kyChamCong.DenNgay;
            //Tao danh sach ngay trong Ky cham cong
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
        public IEnumerable<DTO_QuanLyChamCong_ThongTinChamCongThang> DangKyChamCong_DoiCaLinhDong_Find(DateTime ngay, Guid boPhanId)
        {
            //truyền vào ngày bất kì sẽ lấy ra cả một tuần, bắt đầu từ thứ 2 --> CN
            DateTime tuNgay = ngay;

            for (var dt = tuNgay; tuNgay.DayOfWeek.ToString() != "Monday"; dt = dt.AddDays(-1))
            {
                tuNgay = dt;
            }

            DateTime denNgay = tuNgay.AddDays(6);

            //lay danh sach nhan vien
            var dsIdNhanVien = (from o in this.ObjectSet
                                    //where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                && (o.IDBoPhan == boPhanId)
                                      && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                orderby
                                //o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu1.MaQuanLy
                                //, 
                                o.HoSo.Ten
                                select o.IDNhanVien).Distinct();
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          orderby hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu != null ? hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999, hs.Ten
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.MaQuanLy
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")) : false)
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                 && o.IDBoPhan == boPhanId && o.IDNhanVien == idNv
                                                 orderby o.Ngay ascending
                                                 select new DTO_QuanLyChamCong_ChamCongNgay()
                                                 {
                                                     CC_ChamCongTheoNgayOid = o.Oid
                                                     ,
                                                     IDNhanVien = o.IDNhanVien
                                                     ,
                                                     Ngay = o.Ngay
                                                     ,
                                                     CC_CaChamCong = o.CC_CaChamCong ?? Guid.Empty
                                                 }).ToList()//.OrderBy(x => x.Ngay)
                          });
            //.OrderByDescending(x => x.LaNhanVienToChucHanhChinh);
            return result;
        }
        #endregion
    }//end class
}
