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
    public class Web_GiayChungNhan_AutoNumber_Factory : BaseFactory<Entities, Web_GiayChungNhan_AutoNumber>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return Web_GiayChungNhan_AutoNumber_Factory.New().CreateAloneObject();
        }
        public static Web_GiayChungNhan_AutoNumber_Factory New()
        {
            return new Web_GiayChungNhan_AutoNumber_Factory();
        }
        public Web_GiayChungNhan_AutoNumber_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public Web_GiayChungNhan_AutoNumber GetByID(Guid id)
        {
            Web_GiayChungNhan_AutoNumber result = (from o in this.ObjectSet
                          where o.Id == id
                          select o).SingleOrDefault();
            return result;
        }

     
        #endregion
    }//end class
}
