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
    public class TinhTrang_Factory : BaseFactory<Entities, TinhTrang>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return TinhTrang_Factory.New().CreateAloneObject();
        }
        public static TinhTrang_Factory New()
        {
            return new TinhTrang_Factory();
        }
        public TinhTrang_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<TinhTrang> GetAll_GCRecordIsNull()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         orderby o.TenTinhTrang ascending
                         select o;
            return result;
        }
        public TinhTrang GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public IQueryable<TinhTrang> GetListByLikeName_GCRecordIsNull(string name)
        {
            var result = from o in this.ObjectSet
                          where o.TenTinhTrang.ToLower().Contains(name.ToLower())
                          && o.GCRecord == null
                          select o;
            return result;
        }
        #endregion
    }//end class
}
