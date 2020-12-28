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
    public class CC_DangKyChamCongNgoaiGio_Factory : BaseFactory<Entities, CC_DangKyChamCongNgoaiGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_DangKyChamCongNgoaiGio_Factory.New().CreateAloneObject();
        }
        public static CC_DangKyChamCongNgoaiGio_Factory New()
        {
            return new CC_DangKyChamCongNgoaiGio_Factory();
        }
        public CC_DangKyChamCongNgoaiGio_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_DangKyChamCongNgoaiGio GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_CC_DangKyChamCongNgoaiGio> CaNhanDangKyChamCongNgoaiGio_Find(int thang, int nam, Guid idNhanVien)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien.Oid==idNhanVien
                          && o.Ngay.Value.Month==thang && o.Ngay.Value.Year==nam
                          orderby o.Ngay.Value
                          select new DTO_CC_DangKyChamCongNgoaiGio() {
                              Oid =o.Oid,
                              SoPhutDangKy=o.SoPhutDangKy.ToString(),
                              SoPhutThucTe=o.SoPhutThucTe.ToString(),
                              ThoiGianBatDau=o.TuGio,
                              ThoiGianKetThuc=o.DenGio,
                              Ngay =o.Ngay,
                              LyDo =o.CC_LyDoDangKyChamCongNgoaiGio.LyDo,
                              Duyet =o.Duyet });
            return result;
        }
        public IQueryable<DTO_CC_DangKyChamCongNgoaiGio> QuanLyDangKyChamCongNgoaiGio_Find(int? ngay, int thang, int nam, Guid IDBoPhan,byte? trangthai)
        {
            string ngayString = ngay.ToString();
            Boolean theoThang = (String.IsNullOrWhiteSpace(ngayString) ? true : false);
            Boolean tatCaTrangThai = (trangthai == null);
            var result = (from o in this.ObjectSet
                          where o.IDBoPhan== IDBoPhan
                          && o.Ngay.Value.Month == thang && o.Ngay.Value.Year == nam
                          && (theoThang || o.Ngay.Value.Day==ngay)
                          && (tatCaTrangThai || o.Duyet==trangthai)
                          orderby o.Ngay.Value
                          select new DTO_CC_DangKyChamCongNgoaiGio() {
                              Oid = o.Oid,
                              SoPhutDangKy = o.SoPhutDangKy.ToString(),
                              SoPhutThucTe = o.SoPhutThucTe.ToString(),
                              ThoiGianBatDau = o.TuGio,
                              ThoiGianKetThuc = o.DenGio,
                              Ngay = o.Ngay,
                              LyDo = o.CC_LyDoDangKyChamCongNgoaiGio.LyDo,
                              Duyet = o.Duyet,
                              BoPhan =o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              BoPhanId = o.ThongTinNhanVien.NhanVien.BoPhan1.Oid,
                              HoTen=o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc= o.ThongTinNhanVien.SoHieuCongChuc
                          });
            return result;
        }
        //public IQueryable<DTO_CC_DangKyChamCongNgoaiGio> CaNhanDangKyChamCongNgoaiGio_Find(int ngay, int thang, int nam, Guid IDBoPhan)
        //{
        //    BoPhan_Factory tmpFactory = BoPhan_Factory.New();
        //    tmpFactory.Context = this.Context;
        //    var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

        //    Boolean tatCaTrangThai = (trangThai == null);
        //    Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);


        //    var result = (from o in this.ObjectSet
        //                  where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
        //                  &&
        //                  (tatCaTrangThai || o.TrangThai == trangThai)
        //                  &&
        //                  (tatCaMaNhanSu || o.HoSo.MaQuanLy == maNhanSu)
        //                  && (o.TuNgay.Value.Month >= thang && o.TuNgay.Value.Year >= nam && o.DenNgay.Value.Month <= thang && o.DenNgay.Value.Year <= nam)
        //                  select new DTO_QuanLyKhaiBaoCongTac_Find() { DenNgay = o.DenNgay, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, IDWebUser = o.IDWebUser, SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, NgayTao = o.NgayTao, Oid = o.Oid, NoiDung = o.NoiDung, TrangThai = o.TrangThai, TuNgay = o.TuNgay, Buoi = o.Buoi.ToString(), DiaDiem = o.DiaDiem });
        //    return result;

        //    return null;
        //}

        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_DangKyChamCongNgoaiGio item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
