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
    public class CC_KhaiBaoCongTac_Factory : BaseFactory<Entities, CC_KhaiBaoCongTac>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_KhaiBaoCongTac_Factory.New().CreateAloneObject();
        }
        public static CC_KhaiBaoCongTac_Factory New()
        {
            return new CC_KhaiBaoCongTac_Factory();
        }
        public CC_KhaiBaoCongTac_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_KhaiBaoCongTac GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(int thang, int nam, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var result = (from o in this.ObjectSet
                          where o.IDWebUser == webUserId
                          && (o.TuNgay.Value.Month >= thang && o.TuNgay.Value.Year >= nam && o.DenNgay.Value.Month <= thang && o.DenNgay.Value.Year <= nam)

                          select o);
            return result;

            return null;
        }
        public IQueryable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find(int thang, int nam, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            Boolean tatCaTrangThai = (trangThai == null);
            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);


            var result = (from o in this.ObjectSet
                          where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
                          &&
                          (tatCaTrangThai || o.TrangThai == trangThai)
                          &&
                          (tatCaMaNhanSu || o.HoSo.MaQuanLy == maNhanSu)
                          && (o.TuNgay.Value.Month >= thang && o.TuNgay.Value.Year >= nam && o.DenNgay.Value.Month <= thang && o.DenNgay.Value.Year <= nam)
                          select new DTO_QuanLyKhaiBaoCongTac_Find() { DenNgay = o.DenNgay, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, IDWebUser = o.IDWebUser, MaNhanSu = o.HoSo.MaQuanLy, NgayTao = o.NgayTao, Oid = o.Oid, NoiDung = o.NoiDung, TrangThai = o.TrangThai, TuNgay = o.TuNgay });
            return result;

            return null;
        }

        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_KhaiBaoCongTac item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
