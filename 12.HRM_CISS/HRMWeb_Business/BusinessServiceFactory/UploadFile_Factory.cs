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
    public class UploadFile_Factory : BaseFactory<Entities, GiayToHoSo>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return UploadFile_Factory.New().CreateAloneObject();
        }
        public static UploadFile_Factory New()
        {
            return new UploadFile_Factory();
        }
        public UploadFile_Factory()
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

        public IQueryable<DTO_UploadFile> GetListDTO_UploadFileBy_NhanVienId_GCRecordIsNull(Guid nhanVienId)
        {
            IQueryable<DTO_UploadFile> result = (from o in this.ObjectSet
                                                 where o.GCRecord == null
                                                   && o.LoaiGiayTo != null
                                                   && o.HoSo == nhanVienId
                                                 select new DTO_UploadFile()
                                                 {
                                                     Oid = o.Oid,
                                                     NgayLap = o.NgayLap,
                                                     STT = o.STT.Value,
                                                     TenLoaiGiayTo = o.GiayToHoSo2.TenGiayTo,
                                                     TenGiayTo = o.TenGiayTo,
                                                     DuongDanFileWeb = o.DuongDanFileWeb
                                                 });
            return result;
        }
        #endregion
    }//end class
}
