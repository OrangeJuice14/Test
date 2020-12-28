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
    public class CC_KyChamCong_Factory : BaseFactory<Entities, CC_KyChamCong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_KyChamCong_Factory.New().CreateAloneObject();
        }
        public static CC_KyChamCong_Factory New()
        {
            return new CC_KyChamCong_Factory();
        }
        public CC_KyChamCong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_KyChamCong GetKyChamCong_ByID(Guid oid)
        {
            CC_KyChamCong result = (from o in this.ObjectSet
                                    where o.Oid == oid
                                          && o.GCRecord == null
                                    select o).SingleOrDefault();
            return result;
        }
        public CC_KyChamCong GetKyChamCong_ByThangNam(int thang, int nam,Guid congTy)
        {
            CC_KyChamCong result = (from o in this.ObjectSet
                                    where o.Thang == thang && o.Nam == nam
                                          && o.CongTy == congTy
                                          && o.GCRecord == null
                                    select o).SingleOrDefault();
            return result;
        }
        public CC_KyChamCong GetKyChamCong_ByNgay(DateTime ngay, Guid congTy)
        {
            CC_KyChamCong result = (from o in this.ObjectSet
                                    where o.TuNgay <= ngay && o.DenNgay >= ngay
                                          && o.CongTy == congTy
                                          && o.GCRecord == null
                                    select o).FirstOrDefault();
            return result;
        }

        public IQueryable<CC_KyChamCong> GetKyChamCongList_All()
        {
            var result = (from o in this.ObjectSet
                                    where  o.GCRecord == null
                                    select o);
            return result;
        }

        /*
        public CC_KyChamCong GetObjByThangNam_GCRecordIsNull(int thang, int nam)
        {
            CC_KyChamCong result = (from o in this.ObjectSet
                                  where (o.Thang ?? 0) == thang && (o.Nam ?? 0) == nam
                                    && o.GCRecord == null
                                  select o).SingleOrDefault();
            return result;
        }
        public IQueryable<CC_KyChamCong> GetListByNam_GCRecordIsNull(int nam)
        {
            IQueryable<CC_KyChamCong> result = (from o in this.ObjectSet
                                              where (o.Nam ?? 0) == nam
                                                && o.GCRecord == null
                                              select o);
            return result;
        }
        public IQueryable<CC_KyChamCong> GetAll_GCRecordIsNull()
        {
            IQueryable<CC_KyChamCong> result = (from o in this.ObjectSet
                                              where o.GCRecord == null
                                              orderby o.Nam ascending , o.Thang ascending
                                              select o);
            return result;
        }
        public List<DTO_CC_KyChamCong> GetYearDistinct()
        {
            List<DTO_CC_KyChamCong> result = (from o in this.ObjectSet
                                              where o.GCRecord == null
                                              orderby o.Nam
                                              select new DTO_CC_KyChamCong
                                              {
                                                  Nam=o.Nam
                                              }).Distinct().ToList();
            return result;
        }
        */ 
       
        #endregion
    }//end class
}
