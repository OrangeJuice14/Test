using System;
using System.Collections.Generic;
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

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_KhaiBaoChamCongGiangVien_Factory : BaseFactory<Entities, CC_KhaiBaoChamCongGiangVien>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_KhaiBaoChamCongGiangVien_Factory.New().CreateAloneObject();
        }
        public static CC_KhaiBaoChamCongGiangVien_Factory New()
        {
            return new CC_KhaiBaoChamCongGiangVien_Factory();
        }
        public CC_KhaiBaoChamCongGiangVien_Factory()
            : base(Database.NewEntities())
        {

        }
        public IQueryable<DTO_KhaiBaoChamCong_Find> FindForKhaiBaoChamCong(int? ngay, int thang, int nam, Guid boPhanId, string maNhanSu)
        {
            bool tatCaMaNhanSu = String.IsNullOrWhiteSpace(maNhanSu);
            IQueryable<DTO_KhaiBaoChamCong_Find> result = null;
            if (ngay == 0 || ngay == null)
            {
                result = (from o in this.ObjectSet
                          where o.Ngay.Value.Month == thang && o.Ngay.Value.Year == nam
                                && (tatCaMaNhanSu == true || o.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && o.ThongTinNhanVien.NhanVien.BoPhan == boPhanId
                          orderby o.ThongTinNhanVien.NhanVien.HoSo.HoTen, o.Ngay
                          select new DTO_KhaiBaoChamCong_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              Oid = o.Oid,
                              Ngay = o.Ngay,
                              Buoi = o.Buoi==0? "Cả ngày" : o.Buoi == 1? "Buổi sáng" : "Buổi chiều"
                          });
            }
            else
            {
                result = (from o in this.ObjectSet
                          where o.Ngay.Value.Day == ngay && o.Ngay.Value.Month == thang && o.Ngay.Value.Year == nam
                                && (tatCaMaNhanSu == true || o.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && o.ThongTinNhanVien.NhanVien.BoPhan == boPhanId
                          orderby o.ThongTinNhanVien.NhanVien.HoSo.HoTen, o.Ngay
                          select new DTO_KhaiBaoChamCong_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              Oid = o.Oid,
                              Ngay = o.Ngay,
                              Buoi = o.Buoi == 0 ? "Chủ nhật" : o.Buoi == 1 ? "Buổi sáng" : "Buổi chiều"
                          });
            }
            return result;
        }

        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_KhaiBaoChamCongGiangVien item in deleteList)
            {
                context.DeleteObject(item);
            }
        }

    }//end class
}
