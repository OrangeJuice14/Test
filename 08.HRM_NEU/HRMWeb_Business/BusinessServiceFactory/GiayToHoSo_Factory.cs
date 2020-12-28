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
    public class GiayToHoSo_Factory : BaseFactory<Entities, GiayToHoSo>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return GiayToHoSo_Factory.New().CreateAloneObject();
        }
        public static GiayToHoSo_Factory New()
        {
            return new GiayToHoSo_Factory();
        }
        public GiayToHoSo_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<GiayToHoSo> GetListBy_NhanVienId_GCRecordIsNull(Guid nhanVienId)
        {
            IQueryable<GiayToHoSo> result = (from o in this.ObjectSet
                                             where o.GCRecord == null
                                               && o.HoSo == nhanVienId
                                             select o);
            return result;
        }

        public IQueryable<DTO_GiayToHoSo> GetListDTO_GiayToHoSoBy_NhanVienId_GCRecordIsNull(Guid nhanVienId)
        {
            IQueryable<DTO_GiayToHoSo> result = (from o in this.ObjectSet
                                                 where o.GCRecord == null
                                                   && o.HoSo == nhanVienId
                                                   orderby o.NgayBanHanh ascending ,o.NgayLap ascending
                                                 select new DTO_GiayToHoSo()
                                                 {
                                                     Oid = o.Oid,
                                                     LuuTru = o.LuuTru,
                                                     NgayBanHanh = o.NgayBanHanh,
                                                     SoBan = o.SoBan,
                                                     SoGiayTo = o.SoGiayTo,
                                                     TenDangLuuTru = o.DangLuuTru1.TenDangLuuTru,
                                                     Giay_To = o.GiayTo1.TenGiayTo,
                                                     TrichYeu = o.TrichYeu
                                                 });
            return result;
        }
        #endregion
    }//end class
}
