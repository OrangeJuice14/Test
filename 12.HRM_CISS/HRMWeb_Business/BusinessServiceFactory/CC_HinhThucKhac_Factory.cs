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
using System.Web.Configuration;
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_HinhThucKhac_Factory : BaseFactory<Entities, CC_HinhThucKhac>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_HinhThucKhac_Factory.New().CreateAloneObject();
        }
        public static CC_HinhThucKhac_Factory New()
        {
            return new CC_HinhThucKhac_Factory();
        }
        public CC_HinhThucKhac_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
       
        #endregion
    }//end class
}
