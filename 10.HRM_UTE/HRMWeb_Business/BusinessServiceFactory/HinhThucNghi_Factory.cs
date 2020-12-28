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
    public class HinhThucNghi_Factory : BaseFactory<Entities, HinhThucNghi>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return HinhThucNghi_Factory.New().CreateAloneObject();
        }
        public static HinhThucNghi_Factory New()
        {
            return new HinhThucNghi_Factory();
        }
        public HinhThucNghi_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<HinhThucNghi> GetHinhThucNghiByLoaiNghi(int loaihinhthucnghi)
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null 
                         && ( o.LoaiDonXinNghi == loaihinhthucnghi
                              || (loaihinhthucnghi == 1 && o.Oid == new Guid("E04E2C42-2EB1-4007-89C7-AD8A52BA4E74")))
                         orderby o.TenHinhThucNghi ascending
                         select o;
            return result;
        }
        public IQueryable<HinhThucNghi> GetAll_GCRecordIsNull()
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         orderby o.TenHinhThucNghi ascending
                         select o;
            return result;
        }
        public IQueryable<HinhThucNghi> GetListForUpdate(int loai)
        {
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         && o.KyHieu!=""
                         //&& ( (o.PhanLoai != 6 && loai != 3) || (o.PhanLoai == 6 && loai == 3))
                         orderby o.TenHinhThucNghi ascending
                         select o;
            return result;
        }
        public HinhThucNghi GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        #endregion
    }//end class
}
