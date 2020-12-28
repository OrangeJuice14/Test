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
    public class WebMenu_Role_Factory : BaseFactory<Entities, WebMenu_Role>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebMenu_Role_Factory.New().CreateAloneObject();
        }
        public static WebMenu_Role_Factory New()
        {
            return new WebMenu_Role_Factory();
        }
        public WebMenu_Role_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom


        public IQueryable<WebMenu_Role> GetAll()
        {
            var result = (from o in this.ObjectSet       
                          select o);
            return result;
        }

       
        //public static void FullDelete(Entities context, params Object[] deleteList)
        //{
        //    //foreach (AppL item in deleteList)
        //    //{
        //    //    context.DeleteObject(item);
        //    //}
        //}
        #endregion
    }//end class
}
