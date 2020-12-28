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
    public class KyTinhLuong_Factory : BaseFactory<Entities, KyTinhLuong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return KyTinhLuong_Factory.New().CreateAloneObject();
        }
        public static KyTinhLuong_Factory New()
        {
            return new KyTinhLuong_Factory();
        }
        public KyTinhLuong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public KyTinhLuong GetKyTinhLuong_ByID(Guid oid)
        {
            KyTinhLuong result = (from o in this.ObjectSet
                                    where o.Oid == oid
                                          && o.GCRecord == null
                                    select o).SingleOrDefault();
            return result;
        }
        public KyTinhLuong GetKyTinhLuong_ByThangNam(int thang, int nam)
        {
            KyTinhLuong result = (from o in this.ObjectSet
                                    where o.Thang == thang && o.Nam == nam
                                          && o.GCRecord == null
                                    select o).SingleOrDefault();
            return result;
        }

        public IQueryable<KyTinhLuong> GetKyTinhLuongList_All(Guid congTy)
        {
            var result = (from o in this.ObjectSet
                                    where  o.GCRecord == null
                                           && o.CongTy == congTy
                                    select o ).OrderBy(x => x.Thang);
            return result;
        }
        #endregion
    }//end class
}
