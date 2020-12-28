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
    public class CC_NgayNghiTrongNam_Factory : BaseFactory<Entities, CC_NgayNghiTrongNam>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_NgayNghiTrongNam_Factory.New().CreateAloneObject();
        }
        public static CC_NgayNghiTrongNam_Factory New()
        {
            return new CC_NgayNghiTrongNam_Factory();
        }
        public CC_NgayNghiTrongNam_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_NgayNghiTrongNam GetNgayNghiTrongNam_ByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                                    where o.Oid == oid
                                          && o.GCRecord == null
                                    select o).SingleOrDefault();
            return result;
        }
        public IEnumerable<CC_NgayNghiTrongNam> GetNgayNghiTrongNam_ByTuNgayDenNgay(Guid congTy, DateTime tuNgay, DateTime denNgay)
        {
            var result = (from o in this.ObjectSet
                                          where o.NgayNghi >= tuNgay && o.NgayNghi <= denNgay
                                                && o.CongTy == congTy
                                                && o.GCRecord == null
                                          select o);
            return result;
        }
        #endregion
    }
}
