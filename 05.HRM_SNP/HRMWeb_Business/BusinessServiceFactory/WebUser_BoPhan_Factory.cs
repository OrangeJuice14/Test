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
    public class WebUser_BoPhan_Factory : BaseFactory<Entities, WebUser_Department>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebUser_BoPhan_Factory.New().CreateAloneObject();
        }
        public static WebUser_BoPhan_Factory New()
        {
            return new WebUser_BoPhan_Factory();
        }
        public WebUser_BoPhan_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public WebUser_Department GetBy_WebUserId_And_BoPhanId(Guid webUserId, Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.IDWebUser == webUserId && o.DepartmentID == boPhanId
                          select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (WebUser_Department item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
