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

//create By Vinh

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class NgayLamBuTrongNam_Factory : BaseFactory<Entities, NgayLamBuTrongNam>
    {
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return NgayLamBuTrongNam_Factory.New().CreateAloneObject();
        }
        public static NgayLamBuTrongNam_Factory New()
        {
            return new NgayLamBuTrongNam_Factory();
        }
        public NgayLamBuTrongNam_Factory()
            : base(Database.NewEntities())
        {

        }
        
        public IQueryable<NgayLamBuTrongNam> NgayLamBuTrongNam_Find(int nam)
        {
            var result = (from o in this.ObjectSet
                           where o.QuanLyNgayLamBuTrongNam1.Nam == nam
                          && o.GCRecord == null
                          orderby o.NgayLamBu ascending
                          select o);
            return result;
        }

        public NgayLamBuTrongNam NgayLamBuTrongNam_Find_ByOid(Guid Oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == Oid
                          select o).SingleOrDefault();
            return result;
        }
        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (NgayLamBuTrongNam item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
    }
}
