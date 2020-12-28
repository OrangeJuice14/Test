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
        public CC_KyChamCong GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public CC_KyChamCong GetByMonthAndYear(int thang, int nam)
        {
            var result = (from o in this.ObjectSet
                          where o.Thang == thang && o.Nam==nam
                          select o).SingleOrDefault();
            return result;
        }
        public CC_KyChamCong GetByDate(DateTime date)
        {
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= date && o.DenNgay >= date
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<CC_KyChamCong> GetListByYear(int nam)
        {
            IQueryable<CC_KyChamCong> result = (from o in this.ObjectSet
                                              where o.Nam==nam
                                              orderby o.TuNgay
                                              select o);
            return result;
        }
        public IQueryable<CC_KyChamCong> GetAll_Order()
        {
            IQueryable<CC_KyChamCong> result = (from o in this.ObjectSet
                                                orderby o.TuNgay
                                                select o);
            return result;
        }
        public CC_KyChamCong GetByDate(int ngay, int thang, int nam)
        {
            DateTime date = new DateTime(nam, thang, ngay);
            CC_KyChamCong result = (from o in this.ObjectSet
                                                where o.TuNgay<=date && o.DenNgay>=date
                                                select o).FirstOrDefault();
            return result;
        }
        public CC_KyChamCong GetByKyTinhLuong(Guid KyTinhLuong)
        {
            KyTinhLuong ky = (from o in this.Context.KyTinhLuongs
                              where o.Oid == KyTinhLuong
                              select o).SingleOrDefault();
            CC_KyChamCong result = (from o in this.ObjectSet
                                    where o.Thang==ky.Thang && o.Nam==ky.Nam
                                    select o).FirstOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_KyChamCong item in deleteList)
            {
                context.DeleteObject(item);
            }
        }

        public bool ExistsByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Any(x => x.Thang == thang && x.Nam == nam);
        }
        #endregion
    }//end class
}
