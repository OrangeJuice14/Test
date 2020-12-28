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
    public class CC_HinhThucNghi_Factory : BaseFactory<Entities, CC_HinhThucNghi>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_HinhThucNghi_Factory.New().CreateAloneObject();
        }
        public static CC_HinhThucNghi_Factory New()
        {
            return new CC_HinhThucNghi_Factory();
        }
        public CC_HinhThucNghi_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<CC_HinhThucNghi> GetAll_GCRecordIsNull()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         orderby o.TenHinhThucNghi ascending
                         select o;
            return result;
        }
        public IQueryable<CC_HinhThucNghi> ChamCongNgayNghi_GetAll_GCRecordIsNull()
        {
            Guid idHinhThucNghiPhep = HinhThucNghiConst.NghiPhepId;
            //
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                               && o.Oid != idHinhThucNghiPhep
                         orderby o.TenHinhThucNghi ascending
                         select o;
            return result;
        }
        public IQueryable<CC_HinhThucNghi> GetListForUpdate()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         && o.KyHieu!=""
                         orderby o.TenHinhThucNghi ascending
                         select o;
            return result;
        }
        public CC_HinhThucNghi GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        #endregion
    }//end class
}
