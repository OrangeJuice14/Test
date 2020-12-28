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
    public class NienDoTaiChinh_Factory : BaseFactory<Entities, NienDoTaiChinh>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return NienDoTaiChinh_Factory.New().CreateAloneObject();
        }
        public static NienDoTaiChinh_Factory New()
        {
            return new NienDoTaiChinh_Factory();
        }
        public NienDoTaiChinh_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<DTO_NienDoTaiChinh> GetList_GCRecordIsNull(Guid congTy)
        {
            IQueryable<DTO_NienDoTaiChinh> result = (from o in this.ObjectSet
                                                     where o.GCRecord == null && o.CongTy == congTy
                                                     orderby o.DenNgay descending
                                                     select new DTO_NienDoTaiChinh
                                                     {
                                                         Oid = o.Oid,
                                                         TuNgay = o.TuNgay,
                                                         DenNgay = o.DenNgay,
                                                         TenNienDo = o.TenNienDo
                                                     });
            return result;
        }

        public DTO_NienDoTaiChinh GetListByID(Guid oid)
        {
            DTO_NienDoTaiChinh result = (from o in this.ObjectSet
                                         where o.GCRecord == null
                                               && o.Oid == oid
                                         select new DTO_NienDoTaiChinh
                                         {
                                             Oid = o.Oid,
                                             TuNgay = o.TuNgay,
                                             DenNgay = o.DenNgay,
                                             TenNienDo = o.TenNienDo
                                         }).SingleOrDefault();
            return result;
        }
        public DTO_NienDoTaiChinh GetListByNam(int nam, Guid congTy)
        {
            DateTime ngayHienTai = DateTime.Now;
            //
            DateTime ngayTrongNamHoc = new DateTime(nam, ngayHienTai.Month, ngayHienTai.Day);
            //
            DTO_NienDoTaiChinh result = (from o in this.ObjectSet
                                         where o.GCRecord == null && o.CongTy == congTy
                                               && o.TuNgay <= ngayTrongNamHoc
                                               && o.DenNgay >= ngayTrongNamHoc
                                         orderby o.DenNgay ascending
                                         select new DTO_NienDoTaiChinh
                                         {
                                             Oid = o.Oid,
                                             TuNgay = o.TuNgay,
                                             DenNgay = o.DenNgay,
                                             TenNienDo = o.TenNienDo
                                         }).FirstOrDefault();
            return result;
        }
        #endregion
    }//end class
}
