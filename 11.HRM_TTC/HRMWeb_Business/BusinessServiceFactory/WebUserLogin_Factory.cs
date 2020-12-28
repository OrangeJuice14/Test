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
using HRMWeb_Business.Predefined;
using HRMWebApp.Helpers;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class WebUserLogin_Factory : BaseFactory<Entities, WebUserLogin>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebUserLogin_Factory.New().CreateAloneObject();
        }
        public static CC_QuanLyNghiPhep_Factory New()
        {
            return new CC_QuanLyNghiPhep_Factory();
        }
        public WebUserLogin_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public void SaveUserLogin(string userId, string ipAddress, bool isLogin)
        {
            try
            {
                DateTime today = DateTime.Today;
                if (isLogin)
                {
                    WebUserLogin item = CreateManagedObject();
                    item.Oid = Guid.NewGuid();
                    item.WebUserId = new Guid(userId);
                    item.Date = today;
                    item.LoginTime = DateTime.Now;
                    item.IpAddress = ipAddress;
                    this.SaveChanges();
                }
                else //logout
                {
                    var exists = this.ObjectSet.Where(q => q.WebUser.Oid == new Guid(userId) && q.LogoutTime == null && q.Date == today && q.LoginTime < DateTime.Now).OrderByDescending(q => q.LoginTime).FirstOrDefault();
                    if (exists != null)
                    {
                        exists.LogoutTime = DateTime.Now;
                        this.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("WebUserLogin_Factory/SaveUserLogin", ex);
                throw;
            }
        }
        #endregion
    }
}
