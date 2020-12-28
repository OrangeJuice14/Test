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
using HRMWebApp.KPI.DB;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI;
using NHibernate.Linq;


namespace HRMWeb_Business.BusinessServiceFactory
{
    public class WebMenu_Factory : BaseFactory<Entities, HRMWeb_Business.Model.WebMenu>
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
        public HRMWeb_Business.Model.WebMenu GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public override IQueryable<HRMWeb_Business.Model.WebMenu> GetAll()
        {
            var result = (from o in this.ObjectSet
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }

        public IQueryable<HRMWeb_Business.Model.WebMenu> GetListBy_WebUserId(Guid webUserId)
        {


            var result = (from o in this.ObjectSet
                          where o.WebMenu_Role.Any(x => x.WebGroup.WebUsers.Any(y => y.Oid == webUserId))
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            if(result.Count()==0)
            {
                SessionManager.DoWork(session =>
                {
                    KPI_WebUser kpiwebUser = session.Query<KPI_WebUser>().SingleOrDefault(u => u.Id == webUserId);
                    if(kpiwebUser!=null)
                    {
                        result = (from o in this.ObjectSet
                                      where o.WebMenu_Role.Any(x => x.WebGroupID == kpiwebUser.WebGroupId)
                                      orderby o.Global_idx ascending, o.Local_idx ascending
                                      select o);
                    }
                });
            }

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
        public IQueryable<HRMWeb_Business.Model.WebMenu> GetListTop2LevelDeepBy_WebUserId(Guid webUserId)
        {
            var result = (from o in GetListBy_WebUserId(webUserId)
                          where (o.ParentId == Guid.Empty || o.WebMenu2.ParentId == Guid.Empty || o.ParentId == new Guid("00000000-0000-0000-0000-000000000001"))
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        public IQueryable<HRMWeb_Business.Model.WebMenu> GetChildMenuList_ByWebUserId_AndMenuId(Guid webUserId, Guid menuId)
        {
            var result = (from o in GetListBy_WebUserId(webUserId)
                          where o.ParentId == menuId
                          orderby o.Global_idx ascending, o.Local_idx ascending
                          select o);
            return result;
        }
        public IQueryable<HRMWeb_Business.Model.WebMenu> GetListBy_WebGroupId(Guid webGroupId)
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
