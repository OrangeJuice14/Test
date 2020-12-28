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
    public class CC_DangKyKhungGioLamViec_Factory : BaseFactory<Entities, CC_DangKyKhungGioLamViec>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_DangKyKhungGioLamViec_Factory.New().CreateAloneObject();
        }
        public static CC_DangKyKhungGioLamViec_Factory New()
        {
            return new CC_DangKyKhungGioLamViec_Factory();
        }
        public CC_DangKyKhungGioLamViec_Factory()
            : base(Database.NewEntities())
        {

        }
        public string DangKyKhungGio_GetDuLieuDaDangKy(Guid idNhanVien)
        {
            var result = (from x in this.ObjectSet
                          join y in this.Context.CC_KyDangKyKhungGio on x.KyDangKy equals y.Oid
                          where x.ThongTinNhanVien == idNhanVien
                          orderby y.DenNgay descending
                          select "Cán bộ [" + x.ThongTinNhanVien1.NhanVien.HoSo.HoTen + "] đã đăng ký " + y.TenKy + " với khung giờ là: " + x.CC_CaChamCong.TenCa).Take(1).FirstOrDefault();
            //
            return result;
        }

        public CC_DangKyKhungGioLamViec GetDangKyByIDNV(Guid IDNV)
        {
            CC_DangKyKhungGioLamViec result = (from o in this.ObjectSet
                                               where o.ThongTinNhanVien == IDNV
                                               select o).FirstOrDefault();
            return result;
        }
        public CC_DangKyKhungGioLamViec GetDangKyByOid(Guid oid)
        {
            CC_DangKyKhungGioLamViec result = (from o in this.ObjectSet
                                               where o.Oid == oid
                                               select o).FirstOrDefault();
            return result;
        }
        public CC_DangKyKhungGioLamViec GetDangKyByKy(Guid oid,Guid idnhanvien)
        {
            CC_DangKyKhungGioLamViec result = (from o in this.ObjectSet
                                               where o.KyDangKy == oid && o.ThongTinNhanVien == idnhanvien
                                               select o).FirstOrDefault();
            return result;
        }
        public List<DTO_DangKyKhungGioLamViec> GetListDaDangKy(Guid? bophan, Guid ky)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            result = (from o in this.ObjectSet
                                where o.ThongTinNhanVien1.NhanVien.BoPhan == bophan
                                && o.KyDangKy == ky
                                select new DTO_DangKyKhungGioLamViec()
                                {
                                    Oid = o.Oid,
                                    TenCa = o.CC_CaChamCong.TenCa,
                                    TrangThai = "Đã đăng ký",
                                    TuNgay = o.CC_KyDangKyKhungGio.TuNgay,
                                    DenNgay = o.CC_KyDangKyKhungGio.DenNgay,
                                    ThongTinNhanVien = o.ThongTinNhanVien,
                                    HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                    SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
                                    TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan
                                }).ToList();
            return result;
        }
        public List<DTO_DangKyKhungGioLamViec> GetListChuaDangKy(Guid? bophan,List<DTO_DangKyKhungGioLamViec> result1)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            List<Guid?> list = result1.Select(r => r.ThongTinNhanVien).ToList();
            result = (from o in this.Context.HoSoes
                       where !list.Contains(o.Oid)
                       && o.GCRecord == null
                       && o.Oid==o.NhanVien.ThongTinNhanVien.Oid
                       && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                       && o.NhanVien.BoPhan == bophan
                       && o.IDNhanSu_ChamCong> 0
                      select new DTO_DangKyKhungGioLamViec()
                       {
                           Oid = o.Oid,
                           TenCa = null,
                           TuNgay = null,
                           DenNgay = null,
                           TrangThai = "Chưa đăng ký",
                           ThongTinNhanVien = o.Oid,
                           HoTen = o.HoTen,
                           SoHieuCongChuc = o.NhanVien.ThongTinNhanVien.SoHieuCongChuc,
                           TenPhongBan = o.NhanVien.BoPhan1.TenBoPhan
                       }
                                 ).ToList();
            return result;
        }
        public List<DTO_DangKyKhungGioLamViec> GetListDaDangKyTatCa(Guid ky)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            result = (from o in this.ObjectSet
                      where o.KyDangKy == ky
                      select new DTO_DangKyKhungGioLamViec()
                      {
                          Oid = o.Oid,
                          TenCa = o.CC_CaChamCong.TenCa,
                          TrangThai = "Đã đăng ký",
                          TuNgay = o.CC_KyDangKyKhungGio.TuNgay,
                          DenNgay = o.CC_KyDangKyKhungGio.DenNgay,
                          ThongTinNhanVien = o.ThongTinNhanVien,
                          HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                          SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
                          TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan
                      }).ToList();
            return result;
        }

        public List<DTO_DangKyKhungGioLamViec> GetListDaDangKyTatCa_ThongKe(Guid ky, Guid bophan, string manhansu)
        {
            bool tatcanhanvien = string.IsNullOrEmpty(manhansu) ? true : false;
            //
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            result = (from o in this.ObjectSet
                      where (o.ThongTinNhanVien1.NhanVien.BoPhan == bophan || bophan == Guid.Empty)
                      && (o.ThongTinNhanVien1.SoHieuCongChuc == manhansu || tatcanhanvien)
                      && (o.ThongTinNhanVien1.NhanVien.HoSo.IDNhanSu_ChamCong > 0)
                      && o.KyDangKy == ky
                      select new DTO_DangKyKhungGioLamViec()
                      {
                          Oid = o.Oid,
                          TenCa = o.CC_CaChamCong.TenCa,
                          TrangThai = "Đã đăng ký",
                          TuNgay = o.CC_KyDangKyKhungGio.TuNgay,
                          DenNgay = o.CC_KyDangKyKhungGio.DenNgay,
                          ThongTinNhanVien = o.ThongTinNhanVien,
                          HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                          SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
                          TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                          CaChamCongApDung = (
                                                    from x in this.Context.CC_ChamCongTheoNgay
                                                    join y in this.Context.CC_DangKyKhungGioLamViec on x.IDNhanVien equals y.ThongTinNhanVien
                                                    join z in this.Context.CC_KyDangKyKhungGio on y.CC_KyDangKyKhungGio.Oid equals z.Oid
                                                    join h in this.Context.CC_CaChamCong on x.CC_CaChamCong equals h.Oid
                                                    where x.IDNhanVien == o.ThongTinNhanVien
                                                          && y.KyDangKy == ky
                                                          && x.Ngay >= z.TuNgay && x.Ngay <= z.DenNgay
                                                    group new { h.TenCa, x.NgayDoiCa, x.Ngay } by new { h.TenCa, x.NgayDoiCa } into g
                                                    select new DTO_CC_CaChamCongApDung
                                                    {
                                                        TenCa = g.Select(j => j.TenCa).FirstOrDefault(),
                                                        TuNgay = g.Min(j => j.Ngay),
                                                        DenNgay = g.Max(j => j.Ngay)
                                                    }
                                             ).Distinct().OrderBy(x => x.TuNgay)
                      }).ToList();
            return result;
        }

        public List<DTO_DangKyKhungGioLamViec> GetListChuaDangKyTatCa(List<DTO_DangKyKhungGioLamViec> result1)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            List<Guid?> list = result1.Select(r => r.ThongTinNhanVien).ToList();
            result = (from o in this.Context.HoSoes
                      where !list.Contains(o.Oid)
                      && o.GCRecord == null
                      && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                      && o.Oid == o.NhanVien.ThongTinNhanVien.Oid
                      && o.IDNhanSu_ChamCong > 0
                      select new DTO_DangKyKhungGioLamViec()
                      {
                          Oid = o.Oid,
                          TenCa = null,
                          TuNgay = null,
                          DenNgay = null,
                          TrangThai = "Chưa đăng ký",
                          ThongTinNhanVien = o.Oid,
                          HoTen = o.HoTen,
                          SoHieuCongChuc = o.NhanVien.ThongTinNhanVien.SoHieuCongChuc,
                          TenPhongBan = o.NhanVien.BoPhan1.TenBoPhan
                      }
                                 ).ToList();
            return result;
        }
        public List<DTO_DangKyKhungGioLamViec> DangKyChamCong_Find(Guid? bophan, Guid ky, int trangthai)
        {
            List<DTO_DangKyKhungGioLamViec> result1 = new List<DTO_DangKyKhungGioLamViec>();
            List<DTO_DangKyKhungGioLamViec> result2 = new List<DTO_DangKyKhungGioLamViec>();
            //1: Đã đăng ký chỉ lấy result1
            //0: Chưa đăng ký chỉ lấy result2
            //-1: Tất cả
            if (bophan != null && bophan != Guid.Empty)
            {
                if (trangthai == 1)
                {
                    result1 = GetListDaDangKy(bophan,ky);
                }
                else if (trangthai == 0)
                {
                    result1 = GetListDaDangKy(bophan,ky);
                    result2 = GetListChuaDangKy(bophan, result1);
                    //Làm trống result1
                    result1 = new List<DTO_DangKyKhungGioLamViec>();
                }
                else
                {
                    result1 = GetListDaDangKy(bophan,ky);
                    result2 = GetListChuaDangKy(bophan, result1);
                }
            }
            else
            {
                if (trangthai == 1)
                {
                    result1 = GetListDaDangKyTatCa(ky);
                }
                else if (trangthai == 0)
                {
                    result1 = GetListDaDangKyTatCa(ky);
                    result2 = GetListChuaDangKyTatCa(result1);
                    //Làm trống result1
                    result1 = new List<DTO_DangKyKhungGioLamViec>();
                }
                else
                {
                    result1 = GetListDaDangKyTatCa(ky);
                    result2 = GetListChuaDangKyTatCa(result1);
                }
            }
            var result = result1.Union(result2).ToList();
            return result;
        }

        public List<DTO_DangKyKhungGioLamViec> ThongKeKhungGioLamViec_Find(Guid ky, Guid bophan,  string manhansu)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            //
            result = GetListDaDangKyTatCa_ThongKe(ky,bophan, manhansu);
        
            //Lấy tên ca chấm công áp dụng theo kiểu string
            foreach (DTO_DangKyKhungGioLamViec item in result)
            {
                if (item.TuNgay != null && item.DenNgay != null)
                {
                    // Tên ca đăng ký
                    item.TenCa = item.TenCa + " </br> (" + item.TuNgay.Value.ToString("dd/MM/yyyy") + "-" + item.DenNgay.Value.ToString("dd/MM/yyyy") + ")";

                    //Tên ca áp dụng
                    string tenCa = string.Empty;
                    foreach (DTO_CC_CaChamCongApDung itemCaChamCong in item.CaChamCongApDung)
                    {
                        tenCa += itemCaChamCong.TenCa + " </br> (" + itemCaChamCong.TuNgay.ToString("dd/MM/yyyy") + "-" + itemCaChamCong.DenNgay.ToString("dd/MM/yyyy") + ") </br>";
                    }
                    //
                    item.TenCaChamCongApDung = tenCa;
                }
            }
            return result;
        }
    }//end class
}
