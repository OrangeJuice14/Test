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
    public class LoaiNhanSu_Factory : BaseFactory<Entities, LoaiNhanSu>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return LoaiNhanSu_Factory.New().CreateAloneObject();
        }
        public static LoaiNhanSu_Factory New()
        {
            return new LoaiNhanSu_Factory();
        }
        public LoaiNhanSu_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<LoaiNhanSu> GetAll_GCRecordIsNull()
        {
            IOrderedQueryable<LoaiNhanSu> result = from o in this.ObjectSet
                         where o.GCRecord == null
                         orderby o.MaQuanLy, o.TenLoaiNhanSu descending 
                         select o;
            return result;
        }
        public LoaiNhanSu GetByID(Guid oid)
        {
            LoaiNhanSu result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public IQueryable<LoaiNhanSu> GetListByNotLikeName_GCRecordIsNull(string name)
        {
            IQueryable<LoaiNhanSu> result = from o in this.Context.LoaiNhanSus//cam truy van thong qua object set
                         where o.TenLoaiNhanSu.ToLower().Contains(name.ToLower()) == false
                         && o.GCRecord == null
                         select o;
            return result;
        }
        #endregion
    }//end class
}
