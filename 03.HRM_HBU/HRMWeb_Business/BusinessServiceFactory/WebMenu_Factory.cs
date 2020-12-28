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
    public class WebMenu_Factory : BaseFactory<Entities, WebMenu>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebMenu_Factory.New().CreateAloneObject();
        }
        public static WebMenu_Factory New()
        {
            return new WebMenu_Factory();
        }
        public WebMenu_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public WebMenu GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public override IQueryable<WebMenu> GetAll()
        {
            var result = (from o in this.ObjectSet
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }

        public IQueryable<WebMenu> GetListBy_WebUserId(Guid webUserId)
        {
            var result = (from o in this.ObjectSet
                          where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }

        public IQueryable<String> GetURLListBy_WebUserId(Guid webUserId)
        {
            var result = (from o in this.ObjectSet
                          where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o.Url);
            return result;
        }
        public IQueryable<WebMenu> GetListTop2LevelDeepBy_WebUserId(Guid webUserId)
        {
            var result = (from o in GetListBy_WebUserId(webUserId)
                          where (o.ParentId == Guid.Empty || o.WebMenu2.ParentId == Guid.Empty)
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        public IQueryable<WebMenu> GetChildMenuList_ByWebUserId_AndMenuId(Guid webUserId, Guid menuId)
        {
            var result = (from o in GetListBy_WebUserId(webUserId)
                          where o.ParentId == menuId
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        public IQueryable<WebMenu> GetListBy_WebGroupId(Guid webGroupId)
        {
            var result = (from o in this.ObjectSet
                          where o.WebMenu_Role.Any(x => x.WebGroup.ID == webGroupId)
                          orderby o.Global_idx ascending, o.Local_idx ascending
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
