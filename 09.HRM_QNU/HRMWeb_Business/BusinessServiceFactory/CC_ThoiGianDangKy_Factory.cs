using System;
using System.Collections.Generic;
//using System.Data.Common.CommandTrees;
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
using HRMWeb_Business.Predefined;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_ThoiGianDangKy_Factory : BaseFactory<Entities, CC_ThoiGianDangKyKhungGioLamViec>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ThoiGianDangKy_Factory.New().CreateAloneObject();
        }
        public static CC_ThoiGianDangKy_Factory New()
        {
            return new CC_ThoiGianDangKy_Factory();
        }
        public CC_ThoiGianDangKy_Factory()
            : base(Database.NewEntities())
        {

        }
        public CC_ThoiGianDangKyKhungGioLamViec GetList_ThoiGianDangKy()
        {
            CC_ThoiGianDangKyKhungGioLamViec result = (from o in this.ObjectSet
                                                       select o).FirstOrDefault();
            return result;
        }
        public CC_ThoiGianDangKyKhungGioLamViec GetThoiGianDangKy(Guid oid)
        {
            CC_ThoiGianDangKyKhungGioLamViec result = (from o in this.ObjectSet
                                                       where o.Oid==oid
                                                       select o).SingleOrDefault();
            return result;
        }
        public bool DangKyKhungGio_CheckChot()
        {
            bool result = false;
            DateTime date = DateTime.Now;
            CC_ThoiGianDangKyKhungGioLamViec temp = (from o in this.ObjectSet
                                                     where o.TuNgay<=date && o.DenNgay>=date
                                                     select o).FirstOrDefault();
            if (temp!=null)
            {
                result = true;
            }           
            return result;
        }
        public bool DangKyKhungGio_CheckNgoaiThoiGian()
        {
            DateTime date = DateTime.Now;
            bool result = this.ObjectSet.Any(c => c.DenNgay < date);
            return result;
        }
    }//end class
}
