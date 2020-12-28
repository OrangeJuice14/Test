using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection; 
using System.Data.Linq;
using System.Data;

using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_QuanLyViPham_Factory : BaseFactory<Entities, CC_QuanLyViPham>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_QuanLyViPham_Factory.New().CreateAloneObject();
        }
        public static CC_ChamCongNgayNghi_Factory New()
        {
            return new CC_ChamCongNgayNghi_Factory();
        }
        public CC_QuanLyViPham_Factory()
            : base(Database.NewEntities())
        {

        }
        public IQueryable<DTO_QuanLyViPham_Find> FindForQuanLyViPham(int ngay, int thang, int nam, Guid boPhanId)
        {
            IQueryable<DTO_QuanLyViPham_Find> result;
            //
            result = (from o in this.ObjectSet
                      where ( o.CC_ChamCongTheoNgay1.Ngay.Day == ngay || ngay == 0)
                      && o.CC_ChamCongTheoNgay1.Ngay.Month == thang
                      && o.CC_ChamCongTheoNgay1.Ngay.Year == nam
                      && (boPhanId == Guid.Empty || o.CC_ChamCongTheoNgay1.BoPhan.Oid == boPhanId)
                      orderby o.CC_ChamCongTheoNgay1.Ngay, o.CC_ChamCongTheoNgay1.IDBoPhan, o.CC_ChamCongTheoNgay1.HoSo.HoTen
                      select new DTO_QuanLyViPham_Find()
                      {
                          HoTen = o.CC_ChamCongTheoNgay1.HoSo.HoTen,
                          SoHieuCongChuc = o.CC_ChamCongTheoNgay1.HoSo.MaNhanVien,
                          TenPhongBan = o.CC_ChamCongTheoNgay1.BoPhan.TenBoPhan,
                          TenHinhThucViPham = o.CC_HinhThucViPham1.TenHinhThucViPham,
                          Oid = o.Oid,
                          IDBoPhan = o.CC_ChamCongTheoNgay1.IDBoPhan,
                          IDNhanVien = o.CC_ChamCongTheoNgay1.IDNhanVien,
                          IDHinhThucViPham = o.cc_HinhThucViPham,
                          Ngay = o.CC_ChamCongTheoNgay1.Ngay,

                          TGVaoSangQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianVaoSang,
                          TGRaSangQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianRaSang,
                          TGVaoChieuQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianVaoChieu,
                          TGRaChieuQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianRaChieu,
                          GioVaoSang = o.CC_ChamCongTheoNgay1.GioVaoSang != null ?
                          (
                              (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second.ToString())
                          ) : "Không quét vào",
                          GioRaSang = o.CC_ChamCongTheoNgay1.GioRaSang != null ?
                          (
                              (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second.ToString())
                          ) : "Không quét ra",
                          GioVaoChieu = o.CC_ChamCongTheoNgay1.GioVaoChieu != null ?
                          (
                              (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second.ToString())
                          ) : "Không quét vào",
                          GioRaChieu = o.CC_ChamCongTheoNgay1.GioRaChieu != null ?
                          (
                              (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute.ToString()) + ":" +
                              (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second.ToString())
                          ) : "Không quét ra",

                          ThoiGianVaoTre = o.ThoiGianTre.ToString() ?? "Không có",
                          ThoiGianVeSom = o.ThoiGianSom.ToString() ?? "Không có"
                      });
            return result;
        }
        public IQueryable<DTO_QuanLyViPham_Find> FindForQuanLyViPhamXemBangChamCong(int ngay, int thang, int nam, Guid boPhanId,Guid idNhanVien)
        {
            IQueryable<DTO_QuanLyViPham_Find> result;
            bool tatcabophan = boPhanId == Guid.Empty ? true : false;
            if (idNhanVien == Guid.Empty)
            {
                result = (from o in this.ObjectSet
                          where o.CC_ChamCongTheoNgay1.Ngay.Month == thang
                          && (o.CC_ChamCongTheoNgay1.Ngay.Day == ngay || ngay == 0)
                          && o.CC_ChamCongTheoNgay1.Ngay.Year == nam
                          && (tatcabophan || o.CC_ChamCongTheoNgay1.BoPhan.Oid == boPhanId)
                          orderby o.CC_ChamCongTheoNgay1.Ngay, o.CC_ChamCongTheoNgay1.BoPhan.TenBoPhan, o.CC_ChamCongTheoNgay1.HoSo.HoTen
                          select new DTO_QuanLyViPham_Find()
                          {
                              HoTen = o.CC_ChamCongTheoNgay1.HoSo.HoTen,
                              SoHieuCongChuc = o.CC_ChamCongTheoNgay1.HoSo.MaNhanVien,
                              TenPhongBan = o.CC_ChamCongTheoNgay1.BoPhan.TenBoPhan,
                              TenHinhThucViPham = o.CC_HinhThucViPham1.TenHinhThucViPham,
                              Oid = o.Oid,
                              IDBoPhan = o.CC_ChamCongTheoNgay1.IDBoPhan,
                              IDNhanVien = o.CC_ChamCongTheoNgay1.IDNhanVien,
                              IDHinhThucViPham = o.cc_HinhThucViPham,
                              Ngay = o.CC_ChamCongTheoNgay1.Ngay,

                              TGVaoSangQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianVaoSang,
                              TGRaSangQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianRaSang,
                              TGVaoChieuQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianVaoChieu,
                              TGRaChieuQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianRaChieu,
                              GioVaoSang = o.CC_ChamCongTheoNgay1.GioVaoSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second.ToString())
                              ) : "Không quét vào",
                              GioRaSang = o.CC_ChamCongTheoNgay1.GioRaSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second.ToString())
                              ) : "Không quét ra",
                              GioVaoChieu = o.CC_ChamCongTheoNgay1.GioVaoChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second.ToString())
                              ) : "Không quét vào",
                              GioRaChieu = o.CC_ChamCongTheoNgay1.GioRaChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second.ToString())
                              ) : "Không quét ra",

                              ThoiGianVaoTre = o.ThoiGianTre.ToString() ?? "Không có",
                              ThoiGianVeSom = o.ThoiGianSom.ToString() ?? "Không có"
                          });
            }
            else
            {
                result = (from o in this.ObjectSet
                          where 
                          o.CC_ChamCongTheoNgay1.Ngay.Month == thang 
                          && o.CC_ChamCongTheoNgay1.Ngay.Year == nam
                          && (o.CC_ChamCongTheoNgay1.Ngay.Day == ngay || ngay == 0)
                          && o.CC_ChamCongTheoNgay1.IDNhanVien==idNhanVien
                          orderby o.CC_ChamCongTheoNgay1.Ngay, o.CC_ChamCongTheoNgay1.BoPhan, o.CC_ChamCongTheoNgay1.HoSo.HoTen
                          select new DTO_QuanLyViPham_Find()
                          {
                              HoTen = o.CC_ChamCongTheoNgay1.HoSo.HoTen,
                              SoHieuCongChuc = o.CC_ChamCongTheoNgay1.HoSo.MaNhanVien,
                              TenPhongBan = o.CC_ChamCongTheoNgay1.HoSo.NhanVien.BoPhan1.TenBoPhan,
                              TenHinhThucViPham = o.CC_HinhThucViPham1.TenHinhThucViPham,
                              Oid = o.Oid,
                              IDBoPhan = o.CC_ChamCongTheoNgay1.BoPhan.Oid,
                              IDNhanVien = o.CC_ChamCongTheoNgay1.IDNhanVien,
                              IDHinhThucViPham = o.cc_HinhThucViPham,
                              Ngay = o.CC_ChamCongTheoNgay1.Ngay,

                              TGVaoSangQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianVaoSang,
                              TGRaSangQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianRaSang,
                              TGVaoChieuQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianVaoChieu,
                              TGRaChieuQuyDinh = o.CC_ChamCongTheoNgay1.CC_CaChamCong1.ThoiGianRaChieu,

                              GioVaoSang = o.CC_ChamCongTheoNgay1.GioVaoSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioVaoSang.Value.Second.ToString())
                              ) : "Không quét vào",
                              GioRaSang = o.CC_ChamCongTheoNgay1.GioRaSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioRaSang.Value.Second.ToString())
                              ) : "Không quét vào",
                              GioVaoChieu = o.CC_ChamCongTheoNgay1.GioVaoChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioVaoChieu.Value.Second.ToString())
                              ) : "Không quét vào",
                              GioRaChieu = o.CC_ChamCongTheoNgay1.GioRaChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay1.GioRaChieu.Value.Second.ToString())
                              ) : "Không quét ra",

                              ThoiGianVaoTre = o.ThoiGianTre.ToString() ?? "Không có",
                              ThoiGianVeSom = o.ThoiGianSom.ToString() ?? "Không có"
                          });
            }
            return result;
        }

    }//end class
}
