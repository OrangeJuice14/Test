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
    public class ChamCongNgoaiGioTheoNgay_Factory : BaseFactory<Entities, CC_ChamCongNgoaiGioTheoNgay>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return ChamCongNgoaiGioTheoNgay_Factory.New().CreateAloneObject();
        }
        public static ChamCongNgoaiGioTheoNgay_Factory New()
        {
            return new ChamCongNgoaiGioTheoNgay_Factory();
        }
        public ChamCongNgoaiGioTheoNgay_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_ChamCongNgoaiGioTheoNgay GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public bool CheckExist(DateTime ngay, Guid nhanvienid)
        {
            return this.ObjectSet.Any(o=>o.Ngay==ngay && o.IDNhanVien==nhanvienid);
        }
        public bool CheckNgay(Guid kytinhluong,DateTime ngay)
        {
            return this.Context.KyTinhLuongs.Any(o => o.TuNgay <= ngay && o.DenNgay >= ngay && o.Oid==kytinhluong);
        }
        #endregion
    }//end class
}
