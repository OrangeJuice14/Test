using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection;
using System.Data.Linq;
using System.Data;
using System.Web.Configuration;
using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;
using ERP_Core.Common;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_ChamCongNgayNghi_Factory : BaseFactory<Entities, CC_ChamCongNgayNghi>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChamCongNgayNghi_Factory.New().CreateAloneObject();
        }
        public static CC_ChamCongNgayNghi_Factory New()
        {
            return new CC_ChamCongNgayNghi_Factory();
        }
        public CC_ChamCongNgayNghi_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(int thang, int nam, Guid idNhanVien)
        {
            DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
            DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
            Guid idHinhThucNghiPhep = HinhThucNghiConst.NghiPhepId;
            //
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= ngayCuoiThang
                                && o.DenNgay >= ngayDauThang
                                && (o.IDNhanVien == idNhanVien || idNhanVien == Guid.Empty)
                                && o.CC_HinhThucNghi != idHinhThucNghiPhep
                          orderby o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan,
                              IDNhanVien = o.IDNhanVien,
                              IDHinhThucNghi = o.CC_HinhThucNghi,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              SoNgay = o.SoNgay,
                              NgayTao = o.NgayTao,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser,
                              TrangThai = o.TrangThai,
                              TrangThaiAdmin = o.TrangThaiAdmin
                          });
            return result;
        }

        public decimal DangKyNghiPhep_SoNgayPhepConLai(int nam, Guid idNhanVien)
        {
            //
            var SoNgayPhepConLai = (decimal?)(from x in this.Context.CC_QuanLyNghiPhep
                                              join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                              where x.Nam == nam
                                                    && y.ThongTinNhanVien == idNhanVien
                                              select y.SoNgayPhepConLai.Value).FirstOrDefault() ?? 0;
            return SoNgayPhepConLai;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> DangKyNghiPhep_Find(int thang, int nam, Guid idNhanVien)
        {
            Guid idHinhThucNghiPhep = HinhThucNghiConst.NghiPhepId;

            if (thang > 0)
            {
                DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
                DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
                //
                var result = (from o in this.ObjectSet
                              where o.TuNgay <= ngayCuoiThang
                                    && o.DenNgay >= ngayDauThang
                                    && (o.IDNhanVien == idNhanVien || idNhanVien == Guid.Empty)
                                    && o.CC_HinhThucNghi == idHinhThucNghiPhep
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                                  TenPhongBan = o.BoPhan.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan,
                                  IDNhanVien = o.IDNhanVien,
                                  IDHinhThucNghi = o.CC_HinhThucNghi,
                                  TuNgay = o.TuNgay,
                                  DenNgay = o.DenNgay,
                                  SoNgay = o.SoNgay,
                                  NgayTao = o.NgayTao,
                                  DienGiai = o.DienGiai,
                                  IDWebUser = o.IDWebUser,
                                  TrangThai = o.TrangThai,
                                  TrangThaiAdmin = o.TrangThaiAdmin,
                                  TrangThaiBGH = o.TrangThaiBGH,
                                  SoNgayPhepConLai = (decimal?)(from x in this.Context.CC_QuanLyNghiPhep
                                                                join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                                                where x.Nam == nam
                                                                      && y.ThongTinNhanVien == o.IDNhanVien
                                                                select y.SoNgayPhepConLai.Value).FirstOrDefault() ?? 0
                              });
                return result;
            }
            else
            {
                //
                var result = (from o in this.ObjectSet
                              where (o.TuNgay.Value.Year == nam ||  o.DenNgay.Value.Year == nam)
                                    && (o.IDNhanVien == idNhanVien || idNhanVien == Guid.Empty)
                                    && o.CC_HinhThucNghi == idHinhThucNghiPhep
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                                  TenPhongBan = o.BoPhan.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan,
                                  IDNhanVien = o.IDNhanVien,
                                  IDHinhThucNghi = o.CC_HinhThucNghi,
                                  TuNgay = o.TuNgay,
                                  DenNgay = o.DenNgay,
                                  SoNgay = o.SoNgay,
                                  NgayTao = o.NgayTao,
                                  DienGiai = o.DienGiai,
                                  IDWebUser = o.IDWebUser,
                                  TrangThai = o.TrangThai,
                                  TrangThaiAdmin = o.TrangThaiAdmin,
                                  TrangThaiBGH = o.TrangThaiBGH,
                                  SoNgayPhepConLai = (decimal?)(from x in this.Context.CC_QuanLyNghiPhep
                                                                join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                                                where x.Nam == nam
                                                                      && y.ThongTinNhanVien == o.IDNhanVien
                                                                select y.SoNgayPhepConLai.Value).FirstOrDefault() ?? 0
                              });
                return result;
            }
        }
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              NgaySinh = o.ThongTinNhanVien.NhanVien.HoSo.NgaySinh,
                              ChucVu = o.ThongTinNhanVien.ChucVu1 != null ? o.ThongTinNhanVien.ChucVu1.TenChucVu: o.ThongTinNhanVien.NhanVien.NhanVienThongTinLuong1.NgachLuong1.TenNgachLuong,
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              NamNghiPhep = o.TuNgay.Value.Year.ToString(),
                              SoNgay = o.SoNgay,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              NoiNghiPhep = o.NoiNghi,
                              DienGiai = o.DienGiai,
                              DienThoai = o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong != null ? o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong : o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiNhaRieng,
                              Email = o.ThongTinNhanVien.NhanVien.HoSo.Email,
                              DiaChiLienHe = o.DiaChiLienHe,
                              TenGiayXinPhep = o.TenGiayXinPhep.ToUpper(),
                              LoaiNghiPhep = o.LoaiNghiPhep.ToString(),
                              DanhXung = o.ThongTinNhanVien.NhanVien.HoSo.GioiTinh == 0 ? "Ông" : "Bà"
                          }).SingleOrDefault();
            return result;
        }
        public CC_ChamCongNgayNghi GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public DTO_ChamCongNgayNghi_Find GetDTO_ChamCongNgayNghi_Find_ByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen, SoHieuCongChuc=o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy, TenPhongBan = o.BoPhan.TenBoPhan, HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.CC_HinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi(int thang, int nam, string maNhanSu, Guid idbophan, Guid idLoaiNhanSu)
        {
            Guid idHinhThucNghiPhep = HinhThucNghiConst.NghiPhepId;
            DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
            DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
            //
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= ngayCuoiThang
                                && o.DenNgay >= ngayDauThang
                                && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                && (o.ThongTinNhanVien.SoHieuCongChuc.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                && o.CC_HinhThucNghi != idHinhThucNghiPhep
                                && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                          orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan,
                              IDNhanVien = o.IDNhanVien,
                              IDHinhThucNghi = o.CC_HinhThucNghi,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              NgayTao = o.NgayTao,
                              SoNgay = o.SoNgay,
                              DienGiai = o.DienGiai,
                              TrangThai = o.TrangThai,
                              TrangThaiAdmin = o.TrangThaiAdmin,
                              TrangThaiBGH = o.TrangThaiBGH,
                              IDWebUser = o.IDWebUser,
                          });
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> QuanLyNghiPhep_Find(int thang, int nam, int trangthai, string maNhanSu, Guid idbophan, Guid idLoaiNhanSu, string idwebgroup)
        {
            Guid idHinhThucNghiPhep= HinhThucNghiConst.NghiPhepId;

            // Admin or Phòng tổ chức
            if (idwebgroup.ToUpper().Equals("00000000-0000-0000-0000-000000000001")
                || idwebgroup.ToUpper().Equals("00000000-0000-0000-0000-000000000008")
                || idwebgroup.ToUpper().Equals("05A1BF24-BD1C-455F-96F6-7C4237F4659E"))
            {
                if (thang > 0)
                {

                    #region Theo tháng
                    DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
                    DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
                    //
                    var result = (from o in this.ObjectSet
                                  where o.TuNgay <= ngayCuoiThang
                                        && o.DenNgay >= ngayDauThang
                                        && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                        && (o.ThongTinNhanVien.SoHieuCongChuc.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                        && o.CC_HinhThucNghi == idHinhThucNghiPhep
                                        && (o.TrangThaiAdmin == trangthai || trangthai == 2)
                                        && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                  orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                                  select new DTO_ChamCongNgayNghi_Find()
                                  {
                                      HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                      SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                      TenPhongBan = o.BoPhan.TenBoPhan,
                                      HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                      Oid = o.Oid,
                                      IDBoPhan = o.IDBoPhan,
                                      IDNhanVien = o.IDNhanVien,
                                      IDHinhThucNghi = o.CC_HinhThucNghi,
                                      TuNgay = o.TuNgay,
                                      DenNgay = o.DenNgay,
                                      NgayTao = o.NgayTao,
                                      SoNgay = o.SoNgay,
                                      DienGiai = o.DienGiai,
                                      TrangThai = o.TrangThai,
                                      TrangThaiAdmin = o.TrangThaiAdmin,
                                      TrangThaiBGH = o.TrangThaiBGH,
                                      IDWebUser = o.IDWebUser,
                                      NgayTraPhep = o.NgayTraPhep
                                  });
                    return result;
                    #endregion
                }
                else
                {
                    #region Theo năm
                    //
                    var result = (from o in this.ObjectSet
                                  where (o.TuNgay.Value.Year == nam || o.DenNgay.Value.Year == nam)
                                        && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                        && (o.ThongTinNhanVien.SoHieuCongChuc.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                        && o.CC_HinhThucNghi == idHinhThucNghiPhep
                                        && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                        && (o.TrangThaiAdmin == trangthai || trangthai == 2)
                                  orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                                  select new DTO_ChamCongNgayNghi_Find()
                                  {
                                      HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                      SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                      TenPhongBan = o.BoPhan.TenBoPhan,
                                      HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                      Oid = o.Oid,
                                      IDBoPhan = o.IDBoPhan,
                                      IDNhanVien = o.IDNhanVien,
                                      IDHinhThucNghi = o.CC_HinhThucNghi,
                                      TuNgay = o.TuNgay,
                                      DenNgay = o.DenNgay,
                                      NgayTao = o.NgayTao,
                                      SoNgay = o.SoNgay,
                                      DienGiai = o.DienGiai,
                                      TrangThai = o.TrangThai,
                                      TrangThaiAdmin = o.TrangThaiAdmin,
                                      TrangThaiBGH = o.TrangThaiBGH,
                                      IDWebUser = o.IDWebUser,
                                      NgayTraPhep = o.NgayTraPhep
                                  });
                    return result;
                    #endregion
                }
            }
            else // Ban giám hiệu
            {

                if (thang > 0)
                {
                    #region Theo tháng

                    DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
                    DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
                    //
                    var result = (from o in this.ObjectSet
                                  where o.TuNgay <= ngayCuoiThang
                                        && o.DenNgay >= ngayDauThang
                                        && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                        && (o.ThongTinNhanVien.SoHieuCongChuc.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                        && o.CC_HinhThucNghi == idHinhThucNghiPhep
                                        && (o.TrangThaiBGH == trangthai || trangthai == 2)
                                        && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                  orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                                  select new DTO_ChamCongNgayNghi_Find()
                                  {
                                      HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                      SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                      TenPhongBan = o.BoPhan.TenBoPhan,
                                      HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                      Oid = o.Oid,
                                      IDBoPhan = o.IDBoPhan,
                                      IDNhanVien = o.IDNhanVien,
                                      IDHinhThucNghi = o.CC_HinhThucNghi,
                                      TuNgay = o.TuNgay,
                                      DenNgay = o.DenNgay,
                                      NgayTao = o.NgayTao,
                                      SoNgay = o.SoNgay,
                                      DienGiai = o.DienGiai,
                                      TrangThai = o.TrangThai,
                                      TrangThaiAdmin = o.TrangThaiAdmin,
                                      TrangThaiBGH = o.TrangThaiBGH,
                                      IDWebUser = o.IDWebUser,
                                      NgayTraPhep = o.NgayTraPhep
                                  });
                    return result;
                    #endregion
                }
                else
                {
                    #region Theo năm
                    //
                    var result = (from o in this.ObjectSet
                                  where (o.TuNgay.Value.Year == nam || o.DenNgay.Value.Year == nam)
                                        && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                        && (o.ThongTinNhanVien.SoHieuCongChuc.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                        && o.CC_HinhThucNghi == idHinhThucNghiPhep
                                        && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                        && (o.TrangThaiBGH == trangthai || trangthai == 2)
                                  orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                                  select new DTO_ChamCongNgayNghi_Find()
                                  {
                                      HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                      SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                      TenPhongBan = o.BoPhan.TenBoPhan,
                                      HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                      Oid = o.Oid,
                                      IDBoPhan = o.IDBoPhan,
                                      IDNhanVien = o.IDNhanVien,
                                      IDHinhThucNghi = o.CC_HinhThucNghi,
                                      TuNgay = o.TuNgay,
                                      DenNgay = o.DenNgay,
                                      NgayTao = o.NgayTao,
                                      SoNgay = o.SoNgay,
                                      DienGiai = o.DienGiai,
                                      TrangThai = o.TrangThai,
                                      TrangThaiAdmin = o.TrangThaiAdmin,
                                      TrangThaiBGH = o.TrangThaiBGH,
                                      IDWebUser = o.IDWebUser,
                                      NgayTraPhep = o.NgayTraPhep
                                  });
                    return result;
                    #endregion
                }
            }
        }

        public IQueryable<DTO_ChamCongNgayNghi_Find> QuanLyNghiPhep_DanhSachDaDuyet(int nam, Guid idbophan, string webGroupId)
        {
            Guid idHinhThucNghiPhep = HinhThucNghiConst.NghiPhepId;
            
            //1. Tổ chức duyệt
            if (webGroupId.ToUpper().Equals("00000000-0000-0000-0000-000000000008")
                || webGroupId.ToUpper().Equals("05A1BF24-BD1C-455F-96F6-7C4237F4659E"))
            {
                //
                var result = (from o in this.ObjectSet
                              where (o.TuNgay.Value.Year == nam || o.DenNgay.Value.Year == nam)
                                    && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                    && o.CC_HinhThucNghi == idHinhThucNghiPhep
                                    && o.TrangThaiAdmin == 1
                              orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                  TenPhongBan = o.BoPhan.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan,
                                  IDNhanVien = o.IDNhanVien,
                                  IDHinhThucNghi = o.CC_HinhThucNghi,
                                  TuNgay = o.TuNgay,
                                  DenNgay = o.DenNgay,
                                  NgayTao = o.NgayTao,
                                  SoNgay = o.SoNgay,
                                  DienGiai = o.DienGiai,
                                  TrangThai = o.TrangThai,
                                  TrangThaiAdmin = o.TrangThaiAdmin,
                                  TrangThaiBGH = o.TrangThaiBGH,
                                  IDWebUser = o.IDWebUser,
                                  NgayTraPhep = o.NgayTraPhep,
                                  Ho = o.ThongTinNhanVien.NhanVien.HoSo.Ho,
                                  Ten = o.ThongTinNhanVien.NhanVien.HoSo.Ten,
                                  ChucDanh = o.ThongTinNhanVien.ChucVu1 != null ? o.ThongTinNhanVien.ChucVu1.TenChucVu : o.ThongTinNhanVien.NhanVien.NhanVienThongTinLuong1.NgachLuong1.TenNgachLuong,
                                  NoiNghiPhep = o.NoiNghi,
                                  STT = o.ThongTinNhanVien.NhanVien.BoPhan1.STT.Value
                              });
                return result.OrderBy(m => new { m.STT, m.Ten,m.Ho });
            }
            //2. Hiệu trưởng duyệt
            if (webGroupId.ToUpper().Equals("00000000-0000-0000-0000-000000000003"))
            {
                //
                var result = (from o in this.ObjectSet
                              where (o.TuNgay.Value.Year == nam || o.DenNgay.Value.Year == nam)
                                    && (o.IDBoPhan == idbophan || idbophan == Guid.Empty)
                                    && o.CC_HinhThucNghi == idHinhThucNghiPhep
                                    && o.TrangThaiBGH == 1
                              orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                  TenPhongBan = o.BoPhan.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi1.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan,
                                  IDNhanVien = o.IDNhanVien,
                                  IDHinhThucNghi = o.CC_HinhThucNghi,
                                  TuNgay = o.TuNgay,
                                  DenNgay = o.DenNgay,
                                  NgayTao = o.NgayTao,
                                  SoNgay = o.SoNgay,
                                  DienGiai = o.DienGiai,
                                  TrangThai = o.TrangThai,
                                  TrangThaiAdmin = o.TrangThaiAdmin,
                                  TrangThaiBGH = o.TrangThaiBGH,
                                  IDWebUser = o.IDWebUser,
                                  NgayTraPhep = o.NgayTraPhep,
                                  Ho = o.ThongTinNhanVien.NhanVien.HoSo.Ho,
                                  Ten = o.ThongTinNhanVien.NhanVien.HoSo.Ten,
                                  ChucDanh = o.ThongTinNhanVien.ChucVu1 != null ? o.ThongTinNhanVien.ChucVu1.TenChucVu : o.ThongTinNhanVien.NhanVien.NhanVienThongTinLuong1.NgachLuong1.TenNgachLuong,
                                  NoiNghiPhep = o.NoiNghi,
                                  STT = o.ThongTinNhanVien.NhanVien.BoPhan1.STT.Value
                              });
                return result.OrderBy(m => new { m.STT, m.Ten, m.Ho });
            }
            //
            return null;
        }


        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChamCongNgayNghi item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
