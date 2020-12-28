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
    public class CC_KyDangKyKhungGio_Factory : BaseFactory<Entities, CC_KyDangKyKhungGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_KyDangKyKhungGio_Factory.New().CreateAloneObject();
        }
        public static CC_KyDangKyKhungGio_Factory New()
        {
            return new CC_KyDangKyKhungGio_Factory();
        }
        public CC_KyDangKyKhungGio_Factory()
            : base(Database.NewEntities())
        {

        }
        public IQueryable<CC_KyDangKyKhungGio> GetList_KyDangKy()
        {
            var result = (from o in this.ObjectSet
                          select o);
            return result;
        }
        public CC_KyDangKyKhungGio GetKyDangKy(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid==id
                          select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_KyDangKyKhungGio item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        public bool CheckExist(Guid kyId)
        {
            return this.Context.CC_DangKyKhungGioLamViec.Any(c => c.KyDangKy == kyId);
        }
    }//end class
}
