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
    public class DangLuuTru_Factory : BaseFactory<Entities, DangLuuTru>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return DangLuuTru_Factory.New().CreateAloneObject();
        }
        public static DangLuuTru_Factory New()
        {
            return new DangLuuTru_Factory();
        }
        public DangLuuTru_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<DangLuuTru>  GetAll_GCRecordIsNull()
        {
            IQueryable<DangLuuTru> result = (from o in this.ObjectSet
                                  where o.GCRecord == null
                                  select o);
            return result;
        }
    
     
        #endregion
    }//end class
}
