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
    public class QuanLyNgayLamBuTrongNam_Factory : BaseFactory<Entities, QuanLyNgayLamBuTrongNam>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return QuanLyNgayLamBuTrongNam_Factory.New().CreateAloneObject();
        }
        public static QuanLyNgayLamBuTrongNam_Factory New()
        {
            return new QuanLyNgayLamBuTrongNam_Factory();
        }
        public QuanLyNgayLamBuTrongNam_Factory()
            : base(Database.NewEntities())
        {

        }
        public QuanLyNgayLamBuTrongNam GetByYear(int nam)
        {
            var result = (from o in this.ObjectSet
                          where o.Nam == nam
                          && o.GCRecord == null
                          select o).SingleOrDefault();
            return result;
        }
    }
}
