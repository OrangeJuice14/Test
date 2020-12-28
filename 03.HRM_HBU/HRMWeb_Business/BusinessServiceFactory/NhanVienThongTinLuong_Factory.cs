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
    public class NhanVienThongTinLuong_Factory : BaseFactory<Entities, NhanVienThongTinLuong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return NhanVienThongTinLuong_Factory.New().CreateAloneObject();
        }
        public static NhanVienThongTinLuong_Factory New()
        {
            return new NhanVienThongTinLuong_Factory();
        }
        public NhanVienThongTinLuong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public NhanVienThongTinLuong GetNhanVienThongTinLuongByOid(Guid oid)
        {
            var result = this.Context.NhanVienThongTinLuongs.Where(x => x.GCRecord == null && x.Oid == oid).SingleOrDefault();
            //
            return result;
        }
        #endregion
    }//end class
}
