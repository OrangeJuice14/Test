using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
    public class BangChotThongTinTinhLuong_Factory : BaseFactory<Entities, BangChotThongTinTinhLuong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return BangChotThongTinTinhLuong_Factory.New().CreateAloneObject();
        }
        public static BangChotThongTinTinhLuong_Factory New()
        {
            return new BangChotThongTinTinhLuong_Factory();
        }
        public BangChotThongTinTinhLuong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

     

        #endregion
    }//end class
}
