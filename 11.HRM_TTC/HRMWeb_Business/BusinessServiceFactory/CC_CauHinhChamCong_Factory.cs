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
    public class CC_CauHinhChamCong_Factory : BaseFactory<Entities, CC_CauHinhChamCong>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_CauHinhChamCong_Factory.New().CreateAloneObject();
        }
        public static CC_CauHinhChamCong_Factory New()
        {
            return new CC_CauHinhChamCong_Factory();
        }
        public CC_CauHinhChamCong_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public DTO_CC_CauHinhChamCong GetCauHinhChamCongByCongTy(Guid congTy)
        {
            var result = (from o in this.ObjectSet
                          where o.CongTy == congTy
                          select new DTO_CC_CauHinhChamCong
                          {
                              Oid = o.Oid,
                              EmailSender = o.EmailSender,
                              PassSender = o.PassSender,
                              Host = o.Host,
                              Port = o.Port,
                              SoNgayNghiPhep = o.SoNgayNghiPhep,
                              SoNgayHieuTruongDuyet = o.SoNgayHieuTruongDuyet != null ? o.SoNgayHieuTruongDuyet.Value : 0,
                              SoNgayDangKyNgoaiGio = o.SoNgayDangKyNgoaiGio != null ? o.SoNgayDangKyNgoaiGio.Value : 0,
                              NgayXoaPhepCu = o.NgayXoaPhepCu,
                              SoNgayPhepTangThem = o.SoNgayPhepTangThem,
                              ThoiGianTangSoNgayPhep = o.ThoiGianTangSoNgayPhep,
                              TruongDonViKhongDuyet = o.TruongDonViKhongDuyet,
                          }).FirstOrDefault();
            //
            return result;
        }
        public CC_CauHinhChamCong GetCauHinhChamCongByOid(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        #endregion
    }//end class
}
