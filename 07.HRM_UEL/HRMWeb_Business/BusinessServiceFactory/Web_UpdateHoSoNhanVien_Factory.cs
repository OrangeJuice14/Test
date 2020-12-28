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
    public class Web_UpdateHoSoNhanVien_Factory : BaseFactory<Entities, HoSo>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return Web_UpdateHoSoNhanVien_Factory.New().CreateAloneObject();
        }
        public static Web_UpdateHoSoNhanVien_Factory New()
        {
            return new Web_UpdateHoSoNhanVien_Factory();
        }
        public Web_UpdateHoSoNhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public List<DTO_QuocGia> GetListQuocGiaALL()
        {
            var result = (from o in this.Context.QuocGias.Where(x => x.GCRecord == null)
                          select new DTO_QuocGia { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuocGia = o.TenQuocGia }).ToList();
            //
            return result;
        }
        public List<DTO_TinhThanh> GetListTinhThanhALL()
        {
            var result = (from o in this.Context.TinhThanhs.Where(x => x.GCRecord == null)
                          select new DTO_TinhThanh { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTinhThanh = o.TenTinhThanh }).ToList();
            //
            return result;
        }
        public List<DTO_TonGiao> GetListTonGiaoALL()
        {
            var result = (from o in this.Context.TonGiaos.Where(x => x.GCRecord == null)
                          select new DTO_TonGiao { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTonGiao = o.TenTonGiao }).ToList();
            //
            return result;
        }
        public List<DTO_TinhTrangHonNhan> GetListTinhTrangHonNhanALL()
        {
            var result = (from o in this.Context.TinhTrangHonNhans.Where(x => x.GCRecord == null)
                          select new DTO_TinhTrangHonNhan { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTinhTrangHonNhan = o.TenTinhTrangHonNhan }).ToList();
            //
            return result;
        }

        public List<DTO_QuanHe> GetListQuanHeALL()
        {
            var result = (from o in this.Context.QuanHes.Where(x => x.GCRecord == null)
                          select new DTO_QuanHe { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuanHe = o.TenQuanHe }).ToList();
            //
            return result;
        }
        public List<DTO_LoaiGiamTruGiaCanh> GetListLoaiGiamTruGiaCanhALL()
        {
            var result = (from o in this.Context.LoaiGiamTruGiaCanhs.Where(x => x.GCRecord == null)
                          select new DTO_LoaiGiamTruGiaCanh { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenLoaiGiamTruGiaCanh = o.TenLoaiGiamTruGiaCanh }).ToList();
            //
            return result;
        }
        #endregion
    }//end class
}
