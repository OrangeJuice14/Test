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
        public CC_DangKyKhungGioLamViec GetDangKyByKy(Guid? oid)
        {
            CC_DangKyKhungGioLamViec result = (from o in this.ObjectSet
                                               where o.KyDangKy == oid
                                               select o).FirstOrDefault();
            return result;
        }
        public List<DTO_DangKyKhungGioLamViec> GetListDaDangKy(Guid? bophan)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            result = (from o in this.ObjectSet
                                where o.ThongTinNhanVien1.NhanVien.BoPhan == bophan
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
        public List<DTO_DangKyKhungGioLamViec> GetListChuaDangKy(Guid? bophan, List<DTO_DangKyKhungGioLamViec> result1)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            List<Guid?> list = result1.Select(r => r.ThongTinNhanVien).ToList();
            result = (from o in this.Context.HoSoes
                       where !list.Contains(o.Oid)
                       && o.GCRecord == null
                       && o.Oid==o.NhanVien.ThongTinNhanVien.Oid
                       && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                       && o.NhanVien.BoPhan == bophan
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
        public List<Guid?> GetListAllOid()
        {
            List<Guid?> result = new List<Guid?>();
            result = (from o in this.ObjectSet
                      select o.ThongTinNhanVien).Distinct().ToList();
            return result;
        }
        public IEnumerable<CC_DangKyKhungGioLamViec> GetListAll()
        {
            var result = (from o in this.ObjectSet
                          select o);
            return result;
        }
        public IEnumerable<CC_DangKyKhungGioLamViec> GetListByListOid(List<Guid> list)
        {
            var result = (from id in list
                          join cc in this.ObjectSet on id equals cc.Oid
                          select cc);
            return result;
        }
        public IEnumerable<CC_DangKyKhungGioLamViec> GetListByBoPhan(Guid bophan)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien1.NhanVien.BoPhan == bophan
                          select o);
            return result;
        }
        public List<DTO_DangKyKhungGioLamViec> GetListDaDangKyTatCa()
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            result = (from o in this.ObjectSet
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
        public List<DTO_DangKyKhungGioLamViec> GetListChuaDangKyTatCa(List<DTO_DangKyKhungGioLamViec> result1)
        {
            List<DTO_DangKyKhungGioLamViec> result = new List<DTO_DangKyKhungGioLamViec>();
            List<Guid?> list = result1.Select(r => r.ThongTinNhanVien).ToList();
            result = (from o in this.Context.HoSoes
                      where !list.Contains(o.Oid)
                      && o.GCRecord == null
                      && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                      && o.Oid == o.NhanVien.ThongTinNhanVien.Oid
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
                    result1 = GetListDaDangKy(bophan);
                }
                else if (trangthai == 0)
                {
                    result1 = GetListDaDangKy(bophan);
                    result2 = GetListChuaDangKy(bophan, result1);
                    //Làm trống result1
                    result1 = new List<DTO_DangKyKhungGioLamViec>();
                }
                else
                {
                    result1 = GetListDaDangKy(bophan);
                    result2 = GetListChuaDangKy(bophan, result1);
                }
            }
            else
            {
                if (trangthai == 1)
                {
                    result1 = GetListDaDangKyTatCa();
                }
                else if (trangthai == 0)
                {
                    result1 = GetListDaDangKyTatCa();
                    result2 = GetListChuaDangKyTatCa(result1);
                    //Làm trống result1
                    result1 = new List<DTO_DangKyKhungGioLamViec>();
                }
                else
                {
                    result1 = GetListDaDangKyTatCa();
                    result2 = GetListChuaDangKyTatCa(result1);
                }
            }
            var result = result1.Union(result2).ToList();
            return result;
        }
    }//end class
}
