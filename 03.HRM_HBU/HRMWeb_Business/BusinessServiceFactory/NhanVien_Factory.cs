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
    public class NhanVien_Factory : BaseFactory<Entities, NhanVien>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return NhanVien_Factory.New().CreateAloneObject();
        }
        public static NhanVien_Factory New()
        {
            return new NhanVien_Factory();
        }
        public NhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public NhanVien GetNhanVienByOid(Guid oid)
        {
            var result = this.Context.NhanViens.Where(x => x.HoSo.GCRecord == null && x.Oid == oid).SingleOrDefault();
            //
            return result;
        }
        #endregion
    }//end class
}
