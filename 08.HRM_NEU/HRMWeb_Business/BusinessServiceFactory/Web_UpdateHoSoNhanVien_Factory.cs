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
            var result = (from o in this.Context.QuocGias.Where(x => x.GCRecord == null).OrderBy(x => x.TenQuocGia)
                          select new DTO_QuocGia { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuocGia = o.TenQuocGia }).ToList();
            //
            return result;
        }
        public List<DTO_TinhThanh> GetListTinhThanhALL()
        {
            var result = (from o in this.Context.TinhThanhs.Where(x => x.GCRecord == null).OrderBy(x => x.TenTinhThanh)
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
        public List<DTO_ThanhPhanXuatThan> GetListThanhPhanXuatThanALL()
        {
            var result = (from o in this.Context.ThanhPhanXuatThans.Where(x => x.GCRecord == null)
                          select new DTO_ThanhPhanXuatThan { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenThanhPhanXuatThan = o.TenThanhPhanXuatThan }).ToList();
            //
            return result;
        }
        public List<DTO_DanToc> GetListDanTocALL()
        {
            var result = (from o in this.Context.DanTocs.Where(x => x.GCRecord == null)
                          select new DTO_DanToc { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenDanToc = o.TenDanToc, TenGoiKhac = o.TenGoiKhac }).ToList();
            //
            return result;
        }
        public List<DTO_QuanHuyen> GetListQuanHuyenALL()
        {
            var result = (from o in this.Context.QuanHuyens.Where(x => x.GCRecord == null)
                          select new DTO_QuanHuyen { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuanHuyen = o.TenQuanHuyen}).ToList();
            //
            return result;
        }
        public List<DTO_QuanHuyen> GetListQuanHuyenByTinhThanhId(Guid tinhThanhId)
        {
            var result = (from o in this.Context.QuanHuyens.Where(x => x.GCRecord == null && x.TinhThanh == tinhThanhId)
                          select new DTO_QuanHuyen { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuanHuyen = o.TenQuanHuyen}).ToList();
            //
            return result;
        }
        public List<DTO_SucKhoe> GetListSucKhoeALL()
        {
            var result = (from o in this.Context.SucKhoes.Where(x => x.GCRecord == null)
                          select new DTO_SucKhoe { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenSucKhoe = o.TenSucKhoe }).ToList();
            //
            return result;
        }
        public List<DTO_UuTienBanThan> GetListUuTienBanThanALL()
        {
            var result = (from o in this.Context.UuTienBanThans.Where(x => x.GCRecord == null)
                          select new DTO_UuTienBanThan { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenUuTienBanThan = o.TenUuTienBanThan }).ToList();
            //
            return result;
        }
        public List<DTO_UuTienGiaDinh> GetListUuTienGiaDinhALL()
        {
            var result = (from o in this.Context.UuTienGiaDinhs.Where(x => x.GCRecord == null)
                          select new DTO_UuTienGiaDinh { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenUuTienGiaDinh = o.TenUuTienGiaDinh }).ToList();
            //
            return result;
        }
        public List<DTO_XaPhuong> GetListXaPhuongALL()
        {
            var result = (from o in this.Context.XaPhuongs.Where(x => x.GCRecord == null)
                          select new DTO_XaPhuong { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenXaPhuong = o.TenXaPhuong }).ToList();
            //
            return result;
        }
        public List<DTO_XaPhuong> GetListXaPhuongByQuanHuyenId(Guid quanHuyenId)
        {
            var result = (from o in this.Context.XaPhuongs.Where(x => x.GCRecord == null && x.QuanHuyen == quanHuyenId)
                          select new DTO_XaPhuong { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenXaPhuong = o.TenXaPhuong }).ToList();
            //
            return result;
        }
        public List<DTO_NhomMau> GetListNhomMauALL()
        {
            var result = (from o in this.Context.NhomMaus.Where(x => x.GCRecord == null)
                          select new DTO_NhomMau { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenNhomMau = o.TenNhomMau}).ToList();
            //
            return result;
        }
        #endregion
    }//end class
}
