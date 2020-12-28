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
    public class CC_ChamCongTheoNgayThayDoi_Factory : BaseFactory<Entities, CC_ChamCongTheoNgayThayDoi>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChamCongTheoNgayThayDoi_Factory.New().CreateAloneObject();
        }
        public static CC_ChamCongTheoNgayThayDoi_Factory New()
        {
            return new CC_ChamCongTheoNgayThayDoi_Factory();
        }
        public CC_ChamCongTheoNgayThayDoi_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_ChamCongTheoNgayThayDoi GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public bool CheckExist(Guid oid)
        {
            bool result = false;
            result = this.ObjectSet.Any(cc => cc.Oid == oid);
            return result;
        }
        public static void Delete(Entities context, CC_ChamCongNgayNghi item)
        {
                context.DeleteObject(item);
        }
        #endregion
    }//end class
}
