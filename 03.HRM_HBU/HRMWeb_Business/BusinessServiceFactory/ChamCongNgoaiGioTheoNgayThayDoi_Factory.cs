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
    public class ChamCongNgoaiGioTheoNgayThayDoi_Factory : BaseFactory<Entities, CC_ChamCongNgoaiGioTheoNgayThayDoi>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return ChamCongNgoaiGioTheoNgayThayDoi_Factory.New().CreateAloneObject();
        }
        public static ChamCongNgoaiGioTheoNgayThayDoi_Factory New()
        {
            return new ChamCongNgoaiGioTheoNgayThayDoi_Factory();
        }
        public ChamCongNgoaiGioTheoNgayThayDoi_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public bool CheckExist(Guid oid)
        {
            bool result = false;
            result = this.ObjectSet.Any(cc => cc.Oid == oid);
            return result;
        }
        public CC_ChamCongNgoaiGioTheoNgayThayDoi GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        #endregion
    }//end class
}
