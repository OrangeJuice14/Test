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
                x => x.ThongTinNhanVien==IDNV
                && x.CC_KyDangKyKhungGio.TuNgay.Value<date
                && x.CC_KyDangKyKhungGio.DenNgay.Value > date);
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
        public IQueryable<CC_ChamCongTheoNgay> GetBy_BoPhanIdThangNam(Guid boPhanId,int thang,int nam)
        {
            DateTime Saturday = new DateTime(2000, 1, 1);
            DateTime Sunday = new DateTime(2000, 1, 2);
            Guid KhongXacDinh = new Guid(WebConfigurationManager.AppSettings["KhongXacDinh"]);
            var result = (from o in this.ObjectSet
                          where o.IDBoPhan == boPhanId 
                          && o.Ngay.Month==thang 
                          && o.Ngay.Year==nam
                          && o.IDHinhThucNghi== KhongXacDinh
                          && EntityFunctions.DiffDays(Saturday, o.Ngay) % 7 != 6
                          && EntityFunctions.DiffDays(Sunday, o.Ngay) % 7 != 1
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
                          select o).OrderBy(x=>x.Ngay);
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
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong_New(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, int loaicanbo)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                && ((loaicanbo == 1 && (o.LoaiCanBo == 1 || o.LoaiCanBo == 2)) || o.LoaiCanBo == loaicanbo)
                                // sửa ngày 12/09/2018 gộp loại cán bộ lao động hợp đồng vào biên chế thành 1 bảng công
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
        /// <returns></returns>
        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamCong(DateTime date, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //
            var result = (from o in this.ObjectSet
                          where EntityFunctions.TruncateTime(o.Ngay) == date.Date
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
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

        public IQueryable<DTO_QuanLyChamCong_Find> FindForQuanlyChamChong_ThangVaNam_New(int thang, int nam, Guid? boPhanId, int trangThaiChamCong, bool? diHoc, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu,int loaicanbo)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);


            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatCaTrangThai = (trangThaiChamCong == -1);
            bool daChamCong = (trangThaiChamCong == 1);
            //
            var result = (from o in this.ObjectSet
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                && ((loaicanbo == 1 && (o.LoaiCanBo == 1 || o.LoaiCanBo == 2)) || o.LoaiCanBo == loaicanbo)
                          orderby o.Ngay, o.BoPhan.STT, o.HoSo.Ten
                          select new DTO_QuanLyChamCong_Find() { DaChamCong = o.DaChamCong, HoTen = o.HoSo.HoTen, SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, IDHinhThucNghi = o.IDHinhThucNghi, Ngay = o.Ngay, Oid = o.Oid, TenPhongBan = o.BoPhan.TenBoPhan, TenCa = o.CC_CaChamCong1.TenCa });
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
            //
            var result = (from o in this.ObjectSet
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (tatCaTrangThai || o.DaChamCong == daChamCong)
                                && (diHoc == null || o.DiHoc == diHoc)
                                && (tatCaMaNhanSu == true || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
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
        public IQueryable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa(int ngay, int thang, int nam, Guid boPhanId,int loaicanbo)
        {
            Guid? idLoaiNhanSuGiangVien = new Guid("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                && (//boPhanId == Guid.Empty || 
                                o.IDBoPhan == boPhanId)
                                && (o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien)
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                && ((loaicanbo == 1 && (o.LoaiCanBo == 1 || o.LoaiCanBo == 2)) || o.LoaiCanBo == loaicanbo || loaicanbo == 0)
                          orderby o.HoSo.Ten, o.BoPhan.TenBoPhan
                          //Quét 4 lần
                          //select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVaoSang = o.GioVaoSang, GioRaSang = o.GioRaSang, GioVaoChieu = o.GioVaoChieu, GioRaChieu = o.GioRaChieu });
                          // Quét 2 lần
                          select new DTO_QuanLyChamCong_BieuDoVaoRa() { SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, Ngay = o.Ngay, GioVao = o.GioVaoSang, GioRa = o.GioRaChieu });
            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_BieuDoVaoRa> QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien(int ngay, int thang, int nam, Guid nhanVienID,int loaicanbo)
        {
            Guid? idLoaiNhanSuGiangVien = new Guid("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            //luu y chi lay nhan vien hanh chinh (khong phai giang vien)
            var result = (from o in this.Context.CC_ChamCongTheoNgay
                          where (o.Ngay.Year == nam && o.Ngay.Month == thang && o.Ngay.Day == ngay)
                                && o.IDNhanVien == nhanVienID
                                && o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu != idLoaiNhanSuGiangVien
                                && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                && ((loaicanbo == 1 && (o.LoaiCanBo == 1 || o.LoaiCanBo == 2)) || o.LoaiCanBo == loaicanbo || loaicanbo == 0)
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

            var result = (from o in this.Context.CC_ChiTietChamCongNhanVien
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
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_XemThongTinChamCongThang(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu)
        {
            //Dữ liệu công tháng này nhưng đã chốt tháng tới thì phải lấy dữ liệu chốt tháng tới
            int thangpre = thang;
            int nampre = nam;
            if (thangpre == 1) { thangpre = 12; nampre--; }
            else { thangpre--; }

            //
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);

            //Lấy danh sách tất cả nhân viên 
            var dsIdNhanVien = (from o in this.ObjectSet
                                where (o.Ngay.Year == nampre && o.Ngay.Month == thangpre)
                                      && (o.IDBoPhan == boPhanId)
                                      && (tatCaMaNhanSu || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc == maNhanSu)
                                      && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                       && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                orderby o.HoSo.Ten select o.IDNhanVien).Distinct();
            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              Ho = hs.Ho
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where  o.Ngay.Year == nampre && o.Ngay.Month == thangpre
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
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.KyHieu)
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
                                                 }).ToList()                                                             
                            ,
                            NgayCong = (decimal?)(  from o in this.Context.CC_ChiTietChamCongNhanVien
                                                    where o.ThongTinNhanVien1.Oid==hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang== thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                    select o.SoNgayCong ?? (decimal?) 0 ).FirstOrDefault() ?? 0
                            ,
                            TongHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                        where o.ThongTinNhanVien1.Oid == hs.Oid
                                                        && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                        && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                        select o.NghiCoPhep ?? (decimal?)0 ).FirstOrDefault() ?? 0
                            ,
                            TongDiHoc = (decimal?)( from o in this.Context.CC_ChiTietChamCongNhanVien
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                    select o.NghiDiHocCoLuong ?? (decimal?)0 ).FirstOrDefault() ?? 0 
                                        + (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                     select o.NghiDiHocKhongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                            TongKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                        where o.ThongTinNhanVien1.Oid == hs.Oid
                                                        && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                        && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                        select o.NghiRo ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                            TongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                  where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                  select o.NghiThaiSan ?? (decimal?)0 ).FirstOrDefault() ?? 0
                                         
                          }).OrderByDescending(x => x.LaNhanVienToChucHanhChinh).ThenBy(y=>y.Ten);

            return result;
        }
        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang(int thang, int nam, Guid boPhanId, string maNhanSu, Guid? idLoaiNhanSu,int loaicanbo)
        {
            DateTime ngayTrongThang = new DateTime(nam, thang, 1);
            //
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);

            //Lấy danh sách tất cả nhân viên 
            var dsIdNhanVien = (from o in this.ObjectSet
                                where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                       && (o.IDBoPhan == boPhanId)
                                       && (tatCaMaNhanSu || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc == maNhanSu)
                                       && (idLoaiNhanSu == null || idLoaiNhanSu == Guid.Empty || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                       && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                       && ((loaicanbo == 1 && (o.LoaiCanBo == 1 || o.LoaiCanBo == 2)) || o.LoaiCanBo == loaicanbo ||  loaicanbo == 0)
                                orderby o.HoSo.Ten
                                select o.IDNhanVien).Distinct();
            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {

                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              Ho = hs.Ho
                              ,
                              Ten = hs.Ten
                              ,
                              MaNhanSu = hs.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              TenCa = (from y in this.Context.CC_DangKyKhungGioLamViec
                                       join k in this.Context.CC_KyDangKyKhungGio on y.KyDangKy equals k.Oid
                                       where y.ThongTinNhanVien == idNv
                                             && k.TuNgay <= ngayTrongThang && ngayTrongThang <= k.DenNgay
                                       select y.CC_CaChamCong.TenCa).FirstOrDefault()
                              ,
                              ChiTietChamCong = (from o in this.Context.CC_ChamCongTheoNgay
                                                 where o.Ngay.Year == nam && o.Ngay.Month == thang
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
                                                     NguoiDungChinhSua = o.NguoiDungChinhSua
                                                     ,
                                                     MaHinhThucNghi = (o.IDHinhThucNghi == null ? "+" : o.HinhThucNghi.KyHieu)
                                                     ,
                                                     QuanLyViPham = (from c in this.Context.CC_QuanLyViPham
                                                                     where c.CC_ChamCongTheoNgay.Oid == o.Oid
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
                                                                          where c.CC_ChamCongTheoNgay.Oid == o.Oid
                                                                          select 0).ToList().Count()
                                                 }).ToList()
                            ,
                              NgayCong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                    select o.SoNgayCong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                          select o.NghiCoPhep ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongDiHoc = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                     select o.NghiDiHocCoLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                                        + (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                     select o.NghiDiHocKhongLuong ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                          select o.NghiRo ?? (decimal?)0).FirstOrDefault() ?? 0
                            ,
                              TongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                    select o.NghiThaiSan ?? (decimal?)0).FirstOrDefault() ?? 0
                             ,
                              DanhGia = (string)(from x in this.Context.CC_QuanLyChamCongNhanVien
                                                 join y in this.Context.CC_ChiTietChamCongNhanVien on x.Oid equals y.CC_QuanLyChamCongNhanVien
                                                 where x.KyTinhLuong1.Nam == nam && x.KyTinhLuong1.Thang == thang
                                                       && y.ThongTinNhanVien == hs.Oid
                                                 select y.DanhGia).FirstOrDefault() ?? (string)""
                             ,
                              CC_ChiTietChamCongNhanVienOid = (from x in this.Context.CC_QuanLyChamCongNhanVien
                                                               join y in this.Context.CC_ChiTietChamCongNhanVien on x.Oid equals y.CC_QuanLyChamCongNhanVien
                                                               where x.KyTinhLuong1.Nam == nam && x.KyTinhLuong1.Thang == thang
                                                                     && y.ThongTinNhanVien == hs.Oid
                                                               select y.Oid).FirstOrDefault()
                          }).OrderByDescending(x => x.LaNhanVienToChucHanhChinh).ThenBy(y => y.Ten);

            return result;
        }

        public IQueryable<DTO_QuanLyChamCong_ThongTinChamCongThang> QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien(int thang, int nam, Guid nhanVienID,int loaicanbo)
        {
            DateTime ngayTrongThang = new DateTime(nam, thang, 1);

            //Lấy danh sách chấm công theo ngày
            var dsIdNhanVien = (from o in this.ObjectSet
                                where (o.Ngay.Year == nam && o.Ngay.Month == thang)
                                       && o.IDNhanVien == nhanVienID
                                       && o.HoSo.GCRecord == null && o.HoSo.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                                       && ((loaicanbo == 1 && (o.LoaiCanBo == 1 || o.LoaiCanBo == 2)) || o.LoaiCanBo == loaicanbo || loaicanbo == 0)
                                select o.IDNhanVien).Distinct();
            //Lấy kết quả
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          select new DTO_QuanLyChamCong_ThongTinChamCongThang
                          {
                              Ho = hs.Ho,
                              Ten = hs.Ten,
                              IDNhanVien = idNv
                              ,
                              HoTen = hs.HoTen
                              ,
                              MaNhanSu = hs.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              LaNhanVienToChucHanhChinh = (hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1 != null ? !(hs.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng")) : false)
                              ,
                              TenCa = (from y in this.Context.CC_DangKyKhungGioLamViec
                                       join k in this.Context.CC_KyDangKyKhungGio on y.KyDangKy equals k.Oid
                                       where y.ThongTinNhanVien == idNv
                                             && k.TuNgay <= ngayTrongThang && ngayTrongThang <= k.DenNgay
                                       select y.CC_CaChamCong.TenCa).FirstOrDefault()
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
                                                 }
                                                              ).ToList()
                            ,
                              NgayCong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                    select o.SoNgayCong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              TongHuongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                          select o.NghiCoPhep ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              TongDiHoc = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                     where o.ThongTinNhanVien1.Oid == hs.Oid
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                     && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                     select o.NghiDiHocCoLuong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0 + (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                                             where o.ThongTinNhanVien1.Oid == hs.Oid
                                                                             && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                                             && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                                             select o.NghiDiHocKhongLuong ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              TongKhongLuong = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                          where o.ThongTinNhanVien1.Oid == hs.Oid
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                          && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                          select o.NghiRo ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                            ,
                              TongBHXH = (decimal?)(from o in this.Context.CC_ChiTietChamCongNhanVien
                                                    where o.ThongTinNhanVien1.Oid == hs.Oid
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                                                    select o.NghiThaiSan ?? (decimal?)0
                                        ).FirstOrDefault() ?? 0
                             ,
                              DanhGia = (string)(from x in this.Context.CC_QuanLyChamCongNhanVien
                                                 join y in this.Context.CC_ChiTietChamCongNhanVien on x.Oid equals y.CC_QuanLyChamCongNhanVien
                                                 where x.KyTinhLuong1.Nam == nam && x.KyTinhLuong1.Thang == thang
                                                       && y.ThongTinNhanVien == hs.Oid && y.TrangThai == true
                                                 select y.DanhGia).FirstOrDefault() ?? (string)""
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

        public bool QuanLyChamCong_CheckKhoa(Guid boPhanID, int thang, int nam, int loaicanbo)
        {
            //
            var result = (from x in this.ObjectSet
                       where x.Ngay.Month == thang
                             && x.Ngay.Year == nam
                             && x.IDBoPhan == boPhanID
                             && (x.Khoa ?? false ) == true
                             && ((loaicanbo == 1 && (x.LoaiCanBo == 1 || x.LoaiCanBo == 2)) || (x.LoaiCanBo ?? 0) == loaicanbo)
                          select true).FirstOrDefault();
            return result;
        }
        #endregion
    }//end class
}
