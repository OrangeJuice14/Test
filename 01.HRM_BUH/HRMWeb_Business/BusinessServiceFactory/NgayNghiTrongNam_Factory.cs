using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection; 
using System.Data.Linq;
using System.Data;

using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class NgayNghiTrongNam_Factory : BaseFactory<Entities, NgayNghiTrongNam>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return NgayNghiTrongNam_Factory.New().CreateAloneObject();
        }
        public static NgayNghiTrongNam_Factory New()
        {
            return new NgayNghiTrongNam_Factory();
        }
        public NgayNghiTrongNam_Factory()
            : base(Database.NewEntities())
        {

        }
        public IQueryable<NgayNghiTrongNam> NgayNghiTrongNam_Find(int nam)
        {
            var result = (from o in this.ObjectSet
                          where o.QuanLyNgayNghiTrongNam1.Nam == nam
                          && o.GCRecord == null
                          select o);
            return result;
        }

        
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (NgayNghiTrongNam item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
    }//end class
}
