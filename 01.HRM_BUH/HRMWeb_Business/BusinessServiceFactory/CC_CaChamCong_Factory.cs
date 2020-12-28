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
    public class CC_CaChamCong_Factory : BaseFactory<Entities, CC_CaChamCong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_CaChamCong_Factory.New().CreateAloneObject();
        }
        public static CC_CaChamCong_Factory New()
        {
            return new CC_CaChamCong_Factory();
        }
        public CC_CaChamCong_Factory()
            : base(Database.NewEntities())
        {

        }
        public IQueryable<CC_CaChamCong> GetAll_GCRecordIsNull()
        {
            IOrderedQueryable<CC_CaChamCong> result = from o in this.ObjectSet
                                                   where o.GCRecord == null
                                                   orderby o.TenCa
                                                   select o;
            return result;
        }
        public CC_ThoiGianDangKyKhungGioLamViec GetList_ThoiGianDangKy()
        {
            CC_ThoiGianDangKyKhungGioLamViec result = (from o in this.Context.CC_ThoiGianDangKyKhungGioLamViec
                                                      select o).FirstOrDefault();
            return result;
        }
        public CC_ThoiGianDangKyKhungGioLamViec GetThoiGianDangKy(Guid oid)
        {
            CC_ThoiGianDangKyKhungGioLamViec result = (from o in this.Context.CC_ThoiGianDangKyKhungGioLamViec
                                                       where o.Oid==oid
                                                       select o).SingleOrDefault();
            return result;
        }
        public CC_CaChamCong GetByID(Guid oid)
        {
            CC_CaChamCong result = (from o in this.ObjectSet
                                 where o.Oid == oid
                                 select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_CaChamCong item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
    }//end class
}
