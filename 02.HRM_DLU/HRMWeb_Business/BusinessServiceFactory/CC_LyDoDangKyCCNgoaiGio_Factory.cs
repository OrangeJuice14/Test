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
    public class CC_LyDoDangKyCCNgoaiGio_Factory : BaseFactory<Entities, CC_LyDoDangKyChamCongNgoaiGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_LyDoDangKyCCNgoaiGio_Factory.New().CreateAloneObject();
        }
        public static CC_LyDoDangKyCCNgoaiGio_Factory New()
        {
            return new CC_LyDoDangKyCCNgoaiGio_Factory();
        }
        public CC_LyDoDangKyCCNgoaiGio_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_LyDoDangKyChamCongNgoaiGio GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        #endregion
    }//end class
}
