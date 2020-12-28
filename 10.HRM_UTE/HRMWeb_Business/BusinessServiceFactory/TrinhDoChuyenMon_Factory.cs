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
    public class TrinhDoChuyenMon_Factory : BaseFactory<Entities, TrinhDoChuyenMon>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return TrinhDoChuyenMon_Factory.New().CreateAloneObject();
        }
        public static TrinhDoChuyenMon_Factory New()
        {
            return new TrinhDoChuyenMon_Factory();
        }
        public TrinhDoChuyenMon_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<TrinhDoChuyenMon> GetAll_GCRecordIsNull()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         orderby o.TenTrinhDoChuyenMon ascending
                         select o;
            return result;
        }
        public TrinhDoChuyenMon GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        #endregion
    }//end class
}
