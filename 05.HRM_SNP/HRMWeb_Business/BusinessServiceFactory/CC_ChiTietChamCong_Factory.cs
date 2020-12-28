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
    public class CC_ChiTietChamCong_Factory : BaseFactory<Entities, CC_ChiTietChamCong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChiTietChamCong_Factory.New().CreateAloneObject();
        }
        public static CC_ChiTietChamCong_Factory New()
        {
            return new CC_ChiTietChamCong_Factory();
        }
        public CC_ChiTietChamCong_Factory()
            : base(Database.NewEntities())
        {

        }

        public IQueryable<DTO_ChiTietChamCong_Find> ChiTietChamCong_Thang(int thang, int nam, Guid? boPhanId)
        {
            bool tatCaBoPhan = boPhanId == null ? true : false;
            var result = (from o in this.ObjectSet
                          where o.CC_QuanLyChamCong.KyTinhLuong1.Thang==thang && o.CC_QuanLyChamCong.KyTinhLuong1.Nam==nam
                           && (tatCaBoPhan || o.ThongTinNhanVien1.NhanVien.Department1.Oid==boPhanId)
                           && o.ThongTinNhanVien1.NhanVien.HoSo.GCRecord == null && o.ThongTinNhanVien1.NhanVien.TinhTrang1.DaNghiViec == false
                           orderby o.ThongTinNhanVien1.NhanVien.Department1.STT, o.ThongTinNhanVien1.NhanVien.Department1.TenBoPhan,o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                          select new DTO_ChiTietChamCong_Find()
                          {
                              Oid=o.Oid,
                              MaNhanSu=o.ThongTinNhanVien1.NhanVien.HoSo.MaQuanLy,
                              HoTen= o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                              TenPhongBan =o.ThongTinNhanVien1.NhanVien.Department1.TenBoPhan,
                              NgayCongChuan=o.NgayCongChuan,
                              NgayCongThucTe = o.NgayCongThucTe,
                              NgayCongCangTra = o.NgayCongCangTra,
                              NgayCongNghi = o.NgayCongNghi,
                              NgayCongSuaChua = o.NgayCongSuaChua,
                              NgayCongLamLe = o.NgayCongLamLe,
                              NgayCongNghiLe = o.NgayCongNghiLe,
                              NgayCongAnCa = o.NgayCongAnCa,
                              NgayCongDocHai = o.NgayCongDocHai,
                              NgayCongLamDem = o.NgayCongLamDem,
                              NgayNghiPhep = o.NgayNghiPhep,
                              NgayNghiKhongPhep = o.NgayNghiKhongPhep,
                              NgayNghiThaiSan = o.NgayNghiThaiSan,
                              XepLoaiCanBo=o.CC_XepLoaiDanhGia.TenXepLoai,
                              HeSoXepLoai = o.HeSoXepLoai,
                              HeSoNgayCong = o.HeSoNgayCong,
                          });
            return result;
        }
        #region Custom

        public IQueryable<CC_ChiTietChamCong> GetAll_GCRecordIsNull()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         select o;
            return result;
        }
        public bool CheckChot(int thang, int nam, Guid bophanId)
        {
            return this.ObjectSet.Any(x => x.BoPhan == bophanId && x.CC_QuanLyChamCong.KyTinhLuong1.Thang == thang && x.CC_QuanLyChamCong.KyTinhLuong1.Nam == nam);
        }
        public bool ExistByQuanLyChamCong(Guid idQLCC)
        {
            return this.ObjectSet.Any(x => x.CC_QuanLyChamCong.Oid == idQLCC);
        }
        public IQueryable<CC_ChiTietChamCong> GetListByIdList(List<Guid> idList)
        {
            var result = from o in this.ObjectSet
                         where idList.Any(x => x == o.Oid)
                         select o;
            return result;
        }
        public IQueryable<CC_ChiTietChamCong> GetByHoSoNhanVienID(Guid hoSoNhanVienId)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == hoSoNhanVienId
                          select o);
            return result;
        }
        public IQueryable<CC_XepLoaiDanhGia> GetList_XepLoai()
        {
            var result = (from o in this.Context.CC_XepLoaiDanhGia
                          where o.GCRecord == null
                          select o);
            return result;
        }
        public IQueryable<CC_ChiTietChamCong> GetByBoPhanId(Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.BoPhan == boPhanId
                          select o);
            return result;
        }
        public CC_ChiTietChamCong GetById(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChiTietChamCong item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }
}
