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
        public List<DTO_KyTinhLuong> GetYearDistinct()
        {
            List<DTO_KyTinhLuong> result = (from o in this.ObjectSet
                                              where o.GCRecord == null
                                              orderby o.Nam
                                              select new DTO_KyTinhLuong
                                              {
                                                  Nam=o.Nam
                                              }).Distinct().ToList();
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
        #endregion
    }//end class
}
