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
    public class QuanLyNgayNghiTrongNam_Factory : BaseFactory<Entities, QuanLyNgayNghiTrongNam>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return QuanLyNgayNghiTrongNam_Factory.New().CreateAloneObject();
        }
        public static QuanLyNgayNghiTrongNam_Factory New()
        {
            return new QuanLyNgayNghiTrongNam_Factory();
        }
        public QuanLyNgayNghiTrongNam_Factory()
            : base(Database.NewEntities())
        {

        }
        public QuanLyNgayNghiTrongNam GetByYear(int nam)
        {
            var result = (from o in this.ObjectSet
                          where o.Nam == nam
                          && o.GCRecord==null
                          select o).SingleOrDefault();
            return result;
        }
    }//end class
}
