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
    public class KyTinhLuong_Factory : BaseFactory<Entities, KyTinhLuong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return KyTinhLuong_Factory.New().CreateAloneObject();
        }
        public static KyTinhLuong_Factory New()
        {
            return new KyTinhLuong_Factory();
        }
        public KyTinhLuong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public KyTinhLuong GetObjByThangNam_GCRecordIsNull(int thang, int nam)
        {
            KyTinhLuong result = (from o in this.ObjectSet
                                  where (o.Thang ?? 0) == thang && (o.Nam ?? 0) == nam
                                    && o.GCRecord == null
                                  select o).SingleOrDefault();
            return result;
        }

        public KyTinhLuong GetObjById(Guid kyTinhLuong)
        {
            KyTinhLuong result = (from o in this.ObjectSet
                                  where o.Oid==kyTinhLuong
                                    && o.GCRecord == null
                                  select o).SingleOrDefault();
            return result;
        }
        public IQueryable<KyTinhLuong> GetListByNam_GCRecordIsNull(int nam)
        {
            IQueryable<KyTinhLuong> result = (from o in this.ObjectSet
                                              where (o.Nam ?? 0) == nam
                                                && o.GCRecord == null
                                              select o);
            return result;
        }
        public IQueryable<KyTinhLuong> GetAll_GCRecordIsNull()
        {
            IQueryable<KyTinhLuong> result = (from o in this.ObjectSet
                                              where o.GCRecord == null
                                              orderby o.Nam ascending , o.Thang ascending
                                              select o);
            return result;
        }
        public IQueryable<CompanyInfo> GetListThongTinTruong()
        {
            IQueryable<CompanyInfo> result = (from o in this.Context.CompanyInfoes
                                              where o.TenVietTat!=null
                                              select o);
            return result;
        }
        public KyTinhLuong GetKyTinhLuong_GCRecordIsNull_ByMonthAndYear(int month, int year)
        {
            KyTinhLuong result = (from o in this.ObjectSet
                                              where o.GCRecord == null
                                                && o.Thang == month && o.Nam == year
                                              select o).SingleOrDefault();
            return result;
        }
        //public LoaiNhanSu GetByID(Guid oid)
        //{
        //    LoaiNhanSu result = (from o in this.ObjectSet
        //                  where o.Oid == oid
        //                  select o).SingleOrDefault();
        //    return result;
        //}

        //public IQueryable<LoaiNhanSu> GetListByNotLikeName_GCRecordIsNull(string name)
        //{
        //    IQueryable<LoaiNhanSu> result = from o in this.Context.LoaiNhanSus//cam truy van thong qua object set
        //                 where o.TenLoaiNhanSu.ToLower().Contains(name.ToLower()) == false
        //                 && o.GCRecord == null
        //                 select o;
        //    return result;
        //}
        #endregion
    }//end class
}
