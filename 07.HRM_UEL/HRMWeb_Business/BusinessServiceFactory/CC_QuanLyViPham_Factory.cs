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
        public static CC_QuanLyViPham_Factory New()
        {
            return new CC_QuanLyViPham_Factory();
        }
        public CC_QuanLyViPham_Factory()
            : base(Database.NewEntities())
        {

        }
        public CC_QuanLyViPham GetById(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }
        public CC_QuanLyViPham_File GetFileById(Guid id)
        {
            var result = (from o in this.Context.CC_QuanLyViPham_File
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_QuanLyViPham_Find> FindForQuanLyViPham(int ngay, int thang, int nam, Guid boPhanId, Guid webUserId, Guid? nhanVienId)
        {
            IQueryable<DTO_QuanLyViPham_Find> result;
            Guid webGroupId = WebUser_Factory.New().GetByID(webUserId).WebGroupID ?? Guid.Empty;
            result = (from o in this.ObjectSet
                      where o.CC_ChamCongTheoNgay.Ngay.Day == ngay && o.CC_ChamCongTheoNgay.Ngay.Month == thang && o.CC_ChamCongTheoNgay.Ngay.Year == nam
                      && (boPhanId == Guid.Empty || o.CC_ChamCongTheoNgay.BoPhan.Oid == boPhanId)
                      // Nếu tài khoản bình thường thì chỉ lấy 1
                      && (webGroupId == new Guid("53D57298-1933-4E4B-B4C8-98AFED036E21") ? o.CC_ChamCongTheoNgay.IDNhanVien == nhanVienId : true)
                      orderby o.CC_ChamCongTheoNgay.Ngay, o.CC_ChamCongTheoNgay.IDBoPhan, o.CC_ChamCongTheoNgay.HoSo.HoTen
                      select new DTO_QuanLyViPham_Find()
                      {
                          HoTen = o.CC_ChamCongTheoNgay.HoSo.HoTen,
                          SoHieuCongChuc = o.CC_ChamCongTheoNgay.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc,
                          TenPhongBan = o.CC_ChamCongTheoNgay.BoPhan.TenBoPhan,
                          TenHinhThucViPham = o.CC_HinhThucViPham1.TenHinhThucViPham,
                          Oid = o.Oid,
                          IDBoPhan = o.CC_ChamCongTheoNgay.IDBoPhan,
                          IDNhanVien = o.CC_ChamCongTheoNgay.IDNhanVien,
                          IDHinhThucViPham = o.cc_HinhThucViPham,
                          Ngay = o.CC_ChamCongTheoNgay.Ngay,

                          TGVaoSangQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianVaoSang,
                          TGRaSangQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianRaSang,
                          TGVaoChieuQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianVaoChieu,
                          TGRaChieuQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianRaChieu,

                          GioVaoSang = o.CC_ChamCongTheoNgay.GioVaoSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioVaoSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioVaoSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioVaoSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioVaoSang.Value.Second.ToString())
                              ) : "Không quét vào",
                          GioRaSang = o.CC_ChamCongTheoNgay.GioRaSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioRaSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioRaSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioRaSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioRaSang.Value.Second.ToString())
                              ) : "Không quét ra",
                          GioVaoChieu = o.CC_ChamCongTheoNgay.GioVaoChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Second.ToString())
                              ) : "Không quét vào",
                          GioRaChieu = o.CC_ChamCongTheoNgay.GioRaChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioRaChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioRaChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioRaChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioRaChieu.Value.Second.ToString())
                              ) : "Không quét ra",

                          ThoiGianVaoTre = o.ThoiGianTre.ToString() ?? "Không có",
                          ThoiGianVeSom = o.ThoiGianSom.ToString() ?? "Không có",
                          GiaiTrinh = o.GiaiTrinh,
                          TrangThai_TP = o.TrangThai_TP ?? -1,
                          TrangThai_HD = o.TrangThai_HD ?? -1,
                          CC_QuanLyViPham_File = (from q in this.Context.CC_QuanLyViPham_File
                                                  where q.CC_QuanLyViPham == o.Oid && q.GCRecord == null
                                                  orderby q.DateCreated ascending
                                                  select new DTO_CC_QuanLyViPham_File()
                                                  {
                                                      Oid = q.Oid,
                                                      CC_QuanLyViPham = q.CC_QuanLyViPham.Value,
                                                      FileName = q.FileName + q.FileExtension,
                                                      FilePath = q.FilePath
                                                  }).ToList()
                      });
            return result;         
        }
        public IQueryable<DTO_QuanLyViPham_Find> FindForQuanLyViPham_ThongKe(DateTime? tuNgay, DateTime? denNgay, Guid boPhanId, Guid webUserId, Guid? nhanVienId)
        {
            IQueryable<DTO_QuanLyViPham_Find> result;
            Guid webGroupId = WebUser_Factory.New().GetByID(webUserId).WebGroupID ?? Guid.Empty;
            result = (from o in this.ObjectSet
                      where o.CC_ChamCongTheoNgay.Ngay >= tuNgay && o.CC_ChamCongTheoNgay.Ngay <= denNgay
                      && (boPhanId == Guid.Empty || o.CC_ChamCongTheoNgay.BoPhan.Oid == boPhanId)
                      && (nhanVienId == null || o.CC_ChamCongTheoNgay.IDNhanVien == nhanVienId)
                      orderby o.CC_ChamCongTheoNgay.BoPhan.TenBoPhan, o.CC_ChamCongTheoNgay.HoSo.Ten, o.CC_ChamCongTheoNgay.Ngay
                      select new DTO_QuanLyViPham_Find()
                      {
                          HoTen = o.CC_ChamCongTheoNgay.HoSo.HoTen,
                          SoHieuCongChuc = o.CC_ChamCongTheoNgay.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc,
                          TenPhongBan = o.CC_ChamCongTheoNgay.BoPhan.TenBoPhan,
                          TenHinhThucViPham = o.CC_HinhThucViPham1.TenHinhThucViPham,
                          Oid = o.Oid,
                          IDBoPhan = o.CC_ChamCongTheoNgay.IDBoPhan,
                          IDNhanVien = o.CC_ChamCongTheoNgay.IDNhanVien,
                          IDHinhThucViPham = o.cc_HinhThucViPham,
                          Ngay = o.CC_ChamCongTheoNgay.Ngay,

                          TGVaoSangQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianVaoSang,
                          TGRaSangQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianRaSang,
                          TGVaoChieuQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianVaoChieu,
                          TGRaChieuQuyDinh = o.CC_ChamCongTheoNgay.CC_CaChamCong1.ThoiGianRaChieu,

                          GioVaoSang = o.CC_ChamCongTheoNgay.GioVaoSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioVaoSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioVaoSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioVaoSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioVaoSang.Value.Second.ToString())
                              ) : "Không quét vào",
                          GioRaSang = o.CC_ChamCongTheoNgay.GioRaSang != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioRaSang.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaSang.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioRaSang.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaSang.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaSang.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioRaSang.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaSang.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaSang.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioRaSang.Value.Second.ToString())
                              ) : "Không quét ra",
                          GioVaoChieu = o.CC_ChamCongTheoNgay.GioVaoChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioVaoChieu.Value.Second.ToString())
                              ) : "Không quét vào",
                          GioRaChieu = o.CC_ChamCongTheoNgay.GioRaChieu != null ?
                              (
                                  (o.CC_ChamCongTheoNgay.GioRaChieu.Value.Hour < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaChieu.Value.Hour.ToString() : o.CC_ChamCongTheoNgay.GioRaChieu.Value.Hour.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaChieu.Value.Minute < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaChieu.Value.Minute.ToString() : o.CC_ChamCongTheoNgay.GioRaChieu.Value.Minute.ToString()) + ":" +
                                  (o.CC_ChamCongTheoNgay.GioRaChieu.Value.Second < 10 ? "0" + o.CC_ChamCongTheoNgay.GioRaChieu.Value.Second.ToString() : o.CC_ChamCongTheoNgay.GioRaChieu.Value.Second.ToString())
                              ) : "Không quét ra",

                          ThoiGianVaoTre = o.ThoiGianTre.ToString() ?? "Không có",
                          ThoiGianVeSom = o.ThoiGianSom.ToString() ?? "Không có",
                          GiaiTrinh = o.GiaiTrinh,
                          TrangThai_TP = o.TrangThai_TP ?? -1,
                          TrangThai_HD = o.TrangThai_HD ?? -1,
                          CC_QuanLyViPham_File = (from q in this.Context.CC_QuanLyViPham_File
                                                  where q.CC_QuanLyViPham == o.Oid && q.GCRecord == null
                                                  orderby q.DateCreated ascending
                                                  select new DTO_CC_QuanLyViPham_File()
                                                  {
                                                      Oid = q.Oid,
                                                      CC_QuanLyViPham = q.CC_QuanLyViPham.Value,
                                                      FileName = q.FileName + q.FileExtension,
                                                      FilePath = q.FilePath
                                                  }).ToList()
                      });
            return result;         
        }

    }//end class
}
