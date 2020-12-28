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
    public class CauHinhXetABC_Factory : BaseFactory<Entities, CauHinhXetABC>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CauHinhXetABC_Factory.New().CreateAloneObject();
        }
        public static CauHinhXetABC_Factory New()
        {
            return new CauHinhXetABC_Factory();
        }
        public CauHinhXetABC_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CauHinhXetABC GetCauHinhXetABC(Guid Oid)
        {
            CauHinhXetABC result = (from o in this.ObjectSet
                                  where o.Oid==Oid
                                  select o).SingleOrDefault();
            return result;
        }

        #endregion
    }//end class
}
