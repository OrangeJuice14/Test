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
using NHibernate.Linq;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class WebMailTemplate_Factory : BaseFactory<Entities, HRMWeb_Business.Model.WebMailTemplate>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return WebMailTemplate_Factory.New().CreateAloneObject();
        }
        public static WebMailTemplate_Factory New()
        {
            return new WebMailTemplate_Factory();
        }
        public WebMailTemplate_Factory()
            : base(Database.NewEntities())
        {

        }

        public WebMailTemplate GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public IQueryable<WebMailTemplate> GetByCongTy(Guid congTyId)
        {
            var result = (from o in this.ObjectSet
                          where o.CongTy == congTyId
                          select o);
            return result;
        }

        public WebMailTemplate GetByCongTyVaLoaiGuiMail(Guid congTyId, Guid loaiGuiMail)
        {
            var result = (from o in this.ObjectSet
                          where o.CongTy == congTyId && o.LoaiGuiMail == loaiGuiMail
                          select o).FirstOrDefault();
            return result;
        }

        public IQueryable<WebMailTemplateType> GetMailType()
        {
            var result = (from o in this.Context.WebMailTemplateTypes
                          select o);
            return result;
        }
    }
}
