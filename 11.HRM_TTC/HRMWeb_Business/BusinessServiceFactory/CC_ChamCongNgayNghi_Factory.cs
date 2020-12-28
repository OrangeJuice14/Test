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
using HRMWebApp.Helpers;

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
        public IQueryable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(DateTime tungay, DateTime denngay, Guid idNhanVien)
        {
            //
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= denngay
                                && o.DenNgay >= tungay
                                && (o.IDNhanVien == idNhanVien || idNhanVien == Guid.Empty)
                          orderby o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan.Value,
                              IDNhanVien = o.IDNhanVien.Value,
                              IDHinhThucNghi = o.IDHinhThucNghi.Value,
                              TuNgay = o.TuNgay.Value,
                              DenNgay = o.DenNgay.Value,
                              SoNgay = o.SoNgay.Value,
                              NgayTao = o.NgayTao.Value,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser.Value,
                              TrangThai_TP = o.TrangThai_TP.Value,
                              TrangThai_HDQT = o.TrangThai_HDQT.Value,
                              TrangThai_HT = o.TrangThai_HT.Value,
                              Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value ==  2 ? "Sáng" : "Chiều",
                              LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                              LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false,
                          });
            return result;
        }
        public decimal ChamCongNgayNghi_SoNgayHopLe(Guid idHinhThucNghi, Guid idNhanVien, Guid congTy, DateTime tuNgay)
        {
            try
            {
                return this.Context.spd_WebChamCong_LaySoNgayNghiHopLe(idHinhThucNghi, idNhanVien, congTy, tuNgay).FirstOrDefault().Value;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("CC_ChamCongNgayNghi_Factory/ChamCongNgayNghi_SoNgayHopLe", ex);
                throw;
            }
        }

        public decimal DangKyNghiPhep_SoNgayPhepConLai(Guid namhoc, Guid idNhanVien)
        {
            //
            var SoNgayPhepConLai = (decimal?)(from x in this.Context.CC_QuanLyNghiPhep
                                              join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                              where x.NamHoc == namhoc
                                                    && y.ThongTinNhanVien == idNhanVien
                                              select y.SoNgayPhepConLai.Value).FirstOrDefault() ?? 0;
            return SoNgayPhepConLai;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> DangKyNghiPhep_Find(int thang, int nam, Guid idNhanVien)
        {
            Guid idHinhThucNghiPhep = new Guid("00000000-0000-0000-0000-000000000002");// HinhThucNghiConst.NghiPhepId;
            DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
            DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
            //
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= ngayCuoiThang
                                && o.DenNgay >= ngayDauThang
                                && (o.IDNhanVien == idNhanVien || idNhanVien == Guid.Empty)
                                && o.IDHinhThucNghi == idHinhThucNghiPhep
                          orderby o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan.Value,
                              IDNhanVien = o.IDNhanVien.Value,
                              IDHinhThucNghi = o.IDHinhThucNghi.Value,
                              TuNgay = o.TuNgay.Value,
                              DenNgay = o.DenNgay.Value,
                              SoNgay = o.SoNgay.Value,
                              NgayTao = o.NgayTao.Value,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser.Value,
                              TrangThai_TP = o.TrangThai_TP.Value,
                              TrangThai_HDQT = o.TrangThai_HDQT.Value,
                              TrangThai_HT = o.TrangThai_HT.Value /*,
                              SoNgayPhepConLai = (decimal?)(from x in this.Context.CC_QuanLyNghiPhep
                                                            join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                                            where x.Nam == nam
                                                                  && y.ThongTinNhanVien == o.IDNhanVien
                                                            select y.SoNgayPhepConLai.Value).FirstOrDefault() ?? 0 */
                          });
            return result;
        }
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              NgaySinh = o.ThongTinNhanVien.NhanVien.HoSo.NgaySinh.Value,
                              ChucVu = o.ThongTinNhanVien.ChucVu1 != null ? o.ThongTinNhanVien.ChucVu1.TenChucVu : "",
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              DonVi = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              Khoi = "",
                              NamNghiPhep = o.TuNgay.Value.Year.ToString(),
                              SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              TuNgay = o.TuNgay.Value,
                              DenNgay = o.DenNgay.Value,
                              NguoiBanGiao = o.NguoiBanGiao,
                              DienGiai = o.DienGiai,
                              DienThoai = o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong != null ? o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong : o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiNhaRieng,
                              Email = o.ThongTinNhanVien.NhanVien.HoSo.Email,
                              DiaChiLienHe = o.DiaChiLienHe,
                              TenGiayXinPhep = o.TenGiayXinPhep.ToUpper(),
                              LoaiNghiPhep = o.LoaiNghiPhep.ToString(),
                              HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                              /*Số ngày*/
                              SoNgay = o.SoNgay.Value ,
                              /*Phép*/
                              /*
                              SoNgayPhepNam = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiPhepId ?
                                              (from x in this.Context.CC_QuanLyNghiPhep
                                               join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                               where x.NamHoc1.NgayBatDau.Value.Year == o.TuNgay.Value.Year
                                                     && y.ThongTinNhanVien == o.IDNhanVien
                                               select y.TongSoNgayPhep.Value).FirstOrDefault() : 0,

                              SoNgayPhepDaDung = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiPhepId ?
                                                 (from x in this.Context.CC_QuanLyNghiPhep
                                                  join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                                  where x.Nam == o.TuNgay.Value.Year
                                                          && y.ThongTinNhanVien == o.IDNhanVien
                                                  select y.SoNgayPhepDaNghi.Value).FirstOrDefault() : 0,
                              SoNgayPhepThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiPhepId ? o.SoNgay.Value : 0,
                              */
                              /*Chế độ*/
                              /*
                              SoNgayConKetHonToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiConKetHonId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayConKetHonThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiConKetHonId ? o.SoNgay.Value : 0,
                              SoNgayKetHonToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiKetHonId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayKetHonThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiKetHonId ? o.SoNgay.Value : 0,
                              SoNgayTangToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiTangId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayTangThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiTangId ? o.SoNgay.Value : 0,
                              */
                              /*BHXH*/
                              /*
                              SoNgayOmDNToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiOmDaiNgayId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayOmDNThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiOmDaiNgayId ? o.SoNgay.Value : 0,
                              SoNgayOmNNToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiOmNganNgayId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayOmNNThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiOmNganNgayId ? o.SoNgay.Value : 0,
                              SoNgayConOmToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiConOmId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayConOmThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiConOmId ? o.SoNgay.Value : 0,
                              SoNgayKhamThaiToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiKhamThaiId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayKhamThaiThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiKhamThaiId ? o.SoNgay.Value : 0,
                              SoNgaySayThaiToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiSayThaiId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgaySayThaiThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiSayThaiId ? o.SoNgay.Value : 0,
                              SoNgaySinhConToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiSinhConId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgaySinhConThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiSinhConId ? o.SoNgay.Value : 0,
                              SoNgayDuongSucToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiDuongSucId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayDuongSucThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiDuongSucId ? o.SoNgay.Value : 0,
                              */
                              /*Không lương*/
                              SoNgayKhongLuongToiDa = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiKhongLuongId ? o.CC_HinhThucNghi.SoNgayToiDa.Value : 0,
                              SoNgayKhongLuongThucTe = o.CC_HinhThucNghi.Oid == HinhThucNghiConst.NghiKhongLuongId ? o.SoNgay.Value : 0,

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
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen, SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien, TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan, HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan.Value, IDNhanVien = o.IDNhanVien.Value, IDHinhThucNghi = o.IDHinhThucNghi.Value, TuNgay = o.TuNgay.Value, DenNgay = o.DenNgay.Value, NgayTao = o.NgayTao.Value, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser.Value }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi(DateTime tungay, DateTime denngay, string maNhanSu, Guid idbophan, Guid idLoaiNhanSu, int? trangthai, Guid idWebUser, Guid congTy)
        {
            Guid idHinhThucNghiPhep = Guid.Empty;// HinhThucNghiConst.NghiPhepId;
            DateTime ngayHienTai = DateTime.Now.Date;
            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(idWebUser);
            if (userHienTai == null) return null;
            //
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
            Guid idHieuTruong = WebGroupConst.HieuTruongID;
            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
            Guid idTruongBoPhan = WebGroupConst.TruongBoPhanID;

            //Cấu hình chấm công
            int soNgayHieuTruongDuyet = 2;
            DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
            if (cauHinh != null)
            {
                soNgayHieuTruongDuyet = cauHinh.SoNgayHieuTruongDuyet;
            }
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(idWebUser, congTy);

            //
            if (userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                var result = (from o in this.ObjectSet
                              where o.TuNgay <= denngay
                                    && o.DenNgay >= tungay
                                    && (o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                    && o.CC_HinhThucNghi.Oid != idHinhThucNghiPhep
                                    && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                    && o.IsBanGiamHieu == true
                                    && (o.TrangThai_HDQT == trangthai || trangthai == 2)
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan.CongTy))
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            //
            else if (userHienTai.WebGroupID.Equals(idHieuTruong)
                || userHienTai.WebGroupID.Equals(idHieuTruongUQ))
            {
                var result = (from o in this.ObjectSet
                              where o.TuNgay <= denngay
                                    && o.DenNgay >= tungay
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && (o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                    && o.CC_HinhThucNghi.Oid != idHinhThucNghiPhep
                                    && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                    // 1. Nếu cá nhân thì Trưởng phòng đã duyệt và số ngày > 2
                                    // 2. Trưởng đơn vị thì khỏi
                                    && ((o.SoNgay > soNgayHieuTruongDuyet && o.IsTruongPhong == false && (o.TrangThai_TP == 1 || cauHinh.TruongDonViKhongDuyet == true))
                                        || o.IsTruongPhong == true)
                                    && o.IsBanGiamHieu == false // Không lấy Ban giám hiệu
                                    && (o.TrangThai_HT == trangthai || trangthai == 2)
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            //Trưởng phòng or Trưởng phòng Ủy quyền or Trưởng bộ phận
            else if (userHienTai.WebGroupID.Equals(idTruongPhong)
                || userHienTai.WebGroupID.Equals(idTruongPhongUyQuyen)
                || userHienTai.WebGroupID.Equals(idTruongBoPhan))
            {
                var result = (from o in this.ObjectSet
                              where o.TuNgay <= denngay
                                    && o.DenNgay >= tungay
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && o.IsTruongPhong == false // Không lấy trưởng đơn vị (Hiệu trưởng duyệt)
                                    && (o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                    && o.CC_HinhThucNghi.Oid != idHinhThucNghiPhep
                                    && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                    && (o.TrangThai_TP == trangthai || trangthai == 2)
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            else // Admin
            {
                var result = (from o in this.ObjectSet
                              where o.TuNgay <= denngay
                                    && o.DenNgay >= tungay
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && (o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                    && o.CC_HinhThucNghi.Oid != idHinhThucNghiPhep
                                    && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                                    && (o.TrangThai_Admin == trangthai || o.TrangThai_Admin == trangthai || trangthai == 2)
                                    && (o.BoPhan.CongTy == congTy || BoPhanConst.CoporationGuid == congTy)
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            //
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> QuanLyNghiPhep_Find(int thang, int nam, string maNhanSu, Guid idbophan, Guid idLoaiNhanSu)
        {
            Guid idHinhThucNghiPhep = Guid.Empty;
            DateTime ngayDauThang = Convert.ToDateTime(thang + "/01/" + nam);
            DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang, 3);
            //
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= ngayCuoiThang
                                && o.DenNgay >= ngayDauThang
                                && (o.IDBoPhan == idbophan || idbophan == Guid.Empty || o.BoPhan.BoPhanCha == idbophan)
                                && (o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien.Equals(maNhanSu) || string.IsNullOrEmpty(maNhanSu))
                                && o.CC_HinhThucNghi.Oid == idHinhThucNghiPhep
                                && (o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu || idLoaiNhanSu == Guid.Empty)
                          orderby o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan.Value,
                              IDNhanVien = o.IDNhanVien.Value,
                              IDHinhThucNghi = o.IDHinhThucNghi.Value,
                              TuNgay = o.TuNgay.Value,
                              DenNgay = o.DenNgay.Value,
                              NgayTao = o.NgayTao.Value,
                              SoNgay = o.SoNgay.Value,
                              DienGiai = o.DienGiai,
                              TrangThai_TP = o.TrangThai_TP.Value,
                              TrangThai_HDQT = o.TrangThai_HDQT.Value,
                              TrangThai_HT = o.TrangThai_HT.Value,
                              IDWebUser = o.IDWebUser.Value
                          });
            return result;
        }

        public IQueryable<CC_ChamCongNgayNghi> ChamCongNgayNghiChuaDuyet_ByUser(Guid webUserId, Guid congTy)
        {
            //
            Guid idQuanTriTruong = WebGroupConst.QuanTriTruongID;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
            Guid idHieuTruong = WebGroupConst.HieuTruongID;
            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(webUserId);
            if (userHienTai == null) return null;
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId, congTy);
            //Cấu hình chấm công
            int soNgayHieuTruongDuyet = 2;
            DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
            if (cauHinh != null)
            {
                soNgayHieuTruongDuyet = cauHinh.SoNgayHieuTruongDuyet;
            }

            //Hội đồng quản trị
            if (userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                var result = (from o in this.ObjectSet
                              where (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan.CongTy))
                                        && o.IsBanGiamHieu == true
                                        && o.TrangThai_HDQT == -1
                                        && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                              //
                              select o);
                return result;
            }
            //Hiệu trưởng
            else if (userHienTai.WebGroupID.Equals(idHieuTruong)
            || userHienTai.WebGroupID.Equals(idHieuTruongUQ))
            {
                var result = (from o in this.ObjectSet
                              where o.TrangThai_HT == -1
                                        && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                                        && !o.IsBanGiamHieu.Value
                                        && ((o.SoNgay > soNgayHieuTruongDuyet && !o.IsTruongPhong.Value && (o.TrangThai_TP == 1 || cauHinh.TruongDonViKhongDuyet == true))
                                             //Trường hợp đặc biệt dành cho hội đồng quản trị yersin 
                                             // 21/03/2018 bỏ trường hợp này
                                             //|| (o.IDBoPhan.Value.Equals(BoPhanConst.VanPhongHoiDongQuanTriYersinGuid))
                                             //
                                             || o.IsTruongPhong.Value)
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
                              where o.TrangThai_TP == -1
                                    && !o.IsTruongPhong.Value
                                    && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                              //
                              select o);
                return result;
            }
            else
            {
                var result = (from o in this.ObjectSet
                              where  ((   o.IsBanGiamHieu == false &&
                                           ((!o.IsTruongPhong.Value && o.SoNgay <= soNgayHieuTruongDuyet && o.TrangThai_TP == -1)
                                             ||
                                             (!o.IsTruongPhong.Value && o.SoNgay > soNgayHieuTruongDuyet && (o.TrangThai_HT == -1 || o.TrangThai_TP == -1))
                                             ||
                                             (o.IsTruongPhong.Value && o.TrangThai_HT == -1)
                                           )
                                        )
                                        ||
                                        (o.IsBanGiamHieu == true && o.TrangThai_HDQT == -1)
                                        )
                                        && o.ThongTinNhanVien.NhanVien.CongTy == congTy
                              //
                              select o);
                return result;
            }
            //
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChamCongNgayNghi item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion

        #region Nhắc việc

        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi_NhacViec(DateTime tungay, DateTime denngay, string maNhanSu, Guid idbophan, Guid idLoaiNhanSu, int? trangthai, Guid idWebUser, Guid congTy, bool tatCaDonChuaDuyet)
        {
            Guid idHinhThucNghiPhep = Guid.Empty;// HinhThucNghiConst.NghiPhepId;
            DateTime ngayHienTai = DateTime.Now.Date;
            //
            DTO_WebUser userHienTai = (new WebUser_Factory()).GetDTO_WebUser_ById(idWebUser);
            if (userHienTai == null) return null;
            //
            Guid idAdmin = WebGroupConst.AdminId;
            Guid idTruongPhong = WebGroupConst.TruongPhongID;
            Guid idTruongPhongUyQuyen = WebGroupConst.TruongPhongUyQuyenID;
            Guid idHieuTruong = WebGroupConst.HieuTruongID;
            Guid idHieuTruongUQ = WebGroupConst.HieuTruongUyQuyenID;
            Guid idHoiDongQuanTri = WebGroupConst.HoiDongQuanTriID;
            Guid idHoiDongQuanTriUQ = WebGroupConst.HoiDongQuanTriUyQuyenID;
            Guid idTruongBoPhan = WebGroupConst.TruongBoPhanID;

            //Cấu hình chấm công
            int soNgayHieuTruongDuyet = 2;
            DTO_CC_CauHinhChamCong cauHinh = (new CC_CauHinhChamCong_Factory()).GetCauHinhChamCongByCongTy(congTy);
            if (cauHinh != null)
            {
                soNgayHieuTruongDuyet = cauHinh.SoNgayHieuTruongDuyet;
            }
            //
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(idWebUser, congTy);

            if (userHienTai.WebGroupID.Equals(idHoiDongQuanTri)
                || userHienTai.WebGroupID.Equals(idHoiDongQuanTriUQ))
            {
                var result = (from o in this.ObjectSet
                              where (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan.CongTy))
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && o.IsBanGiamHieu == true
                                    && o.TrangThai_HDQT == -1
                                    && (tatCaDonChuaDuyet || (o.TuNgay <= denngay && o.DenNgay >= tungay))
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            //
            else if (userHienTai.WebGroupID.Equals(idHieuTruong)
                || userHienTai.WebGroupID.Equals(idHieuTruongUQ))
            {
                var result = (from o in this.ObjectSet
                              where o.TrangThai_HT == -1
                                    && !o.IsBanGiamHieu.Value
                                    && ((o.SoNgay > soNgayHieuTruongDuyet && !o.IsTruongPhong.Value && (o.TrangThai_TP == 1 || cauHinh.TruongDonViKhongDuyet == true))
                                         || o.IsTruongPhong.Value)
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                                    && (tatCaDonChuaDuyet || (o.TuNgay <= denngay && o.DenNgay >= tungay))
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            //Trưởng phòng or Trưởng phòng Ủy quyền or Trưởng bộ phận
            else if (userHienTai.WebGroupID.Equals(idTruongPhong)
                || userHienTai.WebGroupID.Equals(idTruongPhongUyQuyen)
                || userHienTai.WebGroupID.Equals(idTruongBoPhan))
            {
                var result = (from o in this.ObjectSet
                              where o.TrangThai_TP == -1
                                    && !o.IsTruongPhong.Value
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && (danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)) //Kiểm tra cho chắc
                                    && (tatCaDonChuaDuyet || (o.TuNgay <= denngay && o.DenNgay >= tungay))
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
            else // Admin
            {
                var result = (from o in this.ObjectSet
                              where ((o.IsBanGiamHieu == false &&
                                       ((!o.IsTruongPhong.Value && o.SoNgay <= soNgayHieuTruongDuyet && o.TrangThai_TP == -1)
                                         ||
                                         (!o.IsTruongPhong.Value && o.SoNgay > soNgayHieuTruongDuyet && (o.TrangThai_HT == -1 || o.TrangThai_TP == -1))
                                         ||
                                         (o.IsTruongPhong.Value && o.TrangThai_HT == -1)
                                       )
                                    )
                                    ||
                                    (o.IsBanGiamHieu == true && o.TrangThai_HDQT == -1))
                                    && (o.BoPhan.CongTy == congTy || BoPhanConst.CoporationGuid == congTy)
                                    && (idbophan == Guid.Empty || o.IDBoPhan == idbophan || o.BoPhan.BoPhanCha == idbophan)
                                    && (tatCaDonChuaDuyet || (o.TuNgay <= denngay && o.DenNgay >= tungay))
                              orderby o.TuNgay
                              select new DTO_ChamCongNgayNghi_Find()
                              {
                                  HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                  SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                                  TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                                  HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                                  Oid = o.Oid,
                                  IDBoPhan = o.IDBoPhan.Value,
                                  IDNhanVien = o.IDNhanVien.Value,
                                  IDHinhThucNghi = o.IDHinhThucNghi.Value,
                                  TuNgay = o.TuNgay.Value,
                                  DenNgay = o.DenNgay.Value,
                                  NgayTao = o.NgayTao.Value,
                                  SoNgay = o.SoNgay.Value,
                                  DienGiai = o.DienGiai,
                                  TrangThai_TP = o.TrangThai_TP.Value,
                                  TrangThai_HDQT = o.TrangThai_HDQT.Value,
                                  TrangThai_HT = o.TrangThai_HT.Value,
                                  IDWebUser = o.IDWebUser.Value,
                                  Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                                  LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                                  LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                              });
                return result;
            }
        }
        #endregion

        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi_ById(Guid chamCongNgayNghiId)
        {
            return (from o in this.ObjectSet
                    where o.Oid == chamCongNgayNghiId
                    orderby o.TuNgay
                    select new DTO_ChamCongNgayNghi_Find()
                    {
                        HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                        SoHieuCongChuc = o.ThongTinNhanVien.NhanVien.HoSo.MaNhanVien,
                        TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                        HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi,
                        Oid = o.Oid,
                        IDBoPhan = o.IDBoPhan.Value,
                        IDNhanVien = o.IDNhanVien.Value,
                        IDHinhThucNghi = o.IDHinhThucNghi.Value,
                        TuNgay = o.TuNgay.Value,
                        DenNgay = o.DenNgay.Value,
                        NgayTao = o.NgayTao.Value,
                        SoNgay = o.SoNgay.Value,
                        DienGiai = o.DienGiai,
                        TrangThai_TP = o.TrangThai_TP.Value,
                        TrangThai_HDQT = o.TrangThai_HDQT.Value,
                        TrangThai_HT = o.TrangThai_HT.Value,
                        IDWebUser = o.IDWebUser.Value,
                        Buoi = o.Buoi.Value == 1 ? "Cả ngày" : o.Buoi.Value == 2 ? "Sáng" : "Chiều",
                        LaTruongPhong = o.IsTruongPhong != null ? o.IsTruongPhong.Value : false,
                        LaBanGiamHieu = o.IsBanGiamHieu != null ? o.IsBanGiamHieu.Value : false
                    });
        }
    }//end class
}
