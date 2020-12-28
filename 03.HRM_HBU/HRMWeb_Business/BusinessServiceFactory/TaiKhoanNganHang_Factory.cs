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
    public class TaiKhoanNganHang_Factory : BaseFactory<Entities, TaiKhoanNganHang>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return TaiKhoanNganHang_Factory.New().CreateAloneObject();
        }
        public static TaiKhoanNganHang_Factory New()
        {
            return new TaiKhoanNganHang_Factory();
        }
        public TaiKhoanNganHang_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public TaiKhoanNganHang GetTaiKhoanNganHangByOid(Guid oid)
        {
            var result = this.Context.TaiKhoanNganHangs.Where(x => x.GCRecord == null && x.Oid == oid).SingleOrDefault();
            //
            return result;
        }
        #endregion
    }//end class
}
