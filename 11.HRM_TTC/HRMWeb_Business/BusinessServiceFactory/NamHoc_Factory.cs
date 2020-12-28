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
    public class NamHoc_Factory : BaseFactory<Entities, NamHoc>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return NamHoc_Factory.New().CreateAloneObject();
        }
        public static NamHoc_Factory New()
        {
            return new NamHoc_Factory();
        }
        public NamHoc_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<DTO_NamHoc> GetList_GCRecordIsNull()
        {
            IQueryable<DTO_NamHoc> result = (from o in this.ObjectSet
                                             where o.GCRecord == null
                                             orderby o.NgayKetThuc descending
                                             select new DTO_NamHoc
                                             {
                                                 Oid = o.Oid,
                                                 NgayBatDau = o.NgayBatDau.Value,
                                                 NgayKetThuc = o.NgayKetThuc.Value,
                                                 TenNamHoc = o.TenNamHoc
                                             });
            return result;
        }

        public DTO_NamHoc GetListByID(Guid oid)
        {
            DTO_NamHoc result = (from o in this.ObjectSet
                                                 where o.GCRecord == null
                                                       && o.Oid == oid
                                                 select new DTO_NamHoc
                                                 {
                                                     Oid = o.Oid,
                                                     NgayBatDau = o.NgayBatDau.Value,
                                                     NgayKetThuc = o.NgayKetThuc.Value,
                                                     TenNamHoc = o.TenNamHoc
                                                 }).SingleOrDefault();
            return result;
        }
        public DTO_NamHoc GetListByNam(int nam)
        {
            DateTime ngayHienTai = DateTime.Now;
            //
            DateTime ngayTrongNamHoc = new DateTime(nam, ngayHienTai.Month, ngayHienTai.Day);
            //
            DTO_NamHoc result = (from o in this.ObjectSet
                                             where o.GCRecord == null
                                                   && o.NgayBatDau <= ngayTrongNamHoc
                                                   && o.NgayKetThuc >= ngayTrongNamHoc
                                             select new DTO_NamHoc
                                             {
                                                 Oid = o.Oid,
                                                 NgayBatDau = o.NgayBatDau.Value,
                                                 NgayKetThuc = o.NgayKetThuc.Value,
                                                 TenNamHoc = o.TenNamHoc
                                             }).FirstOrDefault();
            return result;
        }
        #endregion
    }//end class
}
