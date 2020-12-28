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
    public class CC_QuanLyCongNgoaiGio_Factory : BaseFactory<Entities, CC_QuanLyCongNgoaiGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_QuanLyCongNgoaiGio_Factory.New().CreateAloneObject();
        }
        public static CC_QuanLyCongNgoaiGio_Factory New()
        {
            return new CC_QuanLyCongNgoaiGio_Factory();
        }
        public CC_QuanLyCongNgoaiGio_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom

        public bool CheckKhoaQLCCNG(int thang, int nam,Guid congTy)
        {
            CC_QuanLyCongNgoaiGio qlcc = (from o in this.ObjectSet
                                            where o.CC_KyChamCong.Thang == thang && o.CC_KyChamCong.Nam == nam
                                                  && o.CongTy == congTy
                                                  && o.GCRecord == null
                                            select o).FirstOrDefault();
            bool result = false;
            if (qlcc!= null && qlcc.KhoaChamCong==true)
            {
                result = true;
            }
            return result;
        }

      
        #endregion
    }//end class
}
