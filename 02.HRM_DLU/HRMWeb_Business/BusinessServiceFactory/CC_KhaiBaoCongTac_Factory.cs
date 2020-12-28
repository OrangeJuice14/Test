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
        public int LaySoLonNhat()
        {
            int nam = DateTime.Now.Year;
            var maxValue = this.ObjectSet.Where(c=>c.NgayTao.Value.Year==nam).Max(x => x.So)??0;
            if (maxValue==0)
            {
                CC_CauHinhChamCong_Factory fac = new CC_CauHinhChamCong_Factory();
                maxValue = fac.GetCauHinhCauCong().SoGiayDiDuong;
            }
            return maxValue;
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
        }
        public DTO_CC_KhaiBaoCongTac CaNhanKhaiBaoCongTac_Report(Guid id)
        {

            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select new DTO_CC_KhaiBaoCongTac()
                          {
                              Oid=o.Oid,
                              So=o.So??0,
                              DiaDiem=o.DiaDiem,
                              TuNgay=o.TuNgay,
                              DenNgay=o.DenNgay,
                              IDNhanVien=o.IDNhanVien,
                              IDNguoiKy =o.NguoiKy??Guid.Empty                           
                          }).SingleOrDefault();
            var nhanvien = (from o in this.Context.HoSoes
                            where o.Oid == result.IDNhanVien
                            select o).SingleOrDefault();
            result.OngBa = nhanvien.GioiTinh == 0 ? "Ông" : "Bà";
            result.HoTen = nhanvien.HoTen;
            result.ChucVu = nhanvien.NhanVien.ThongTinNhanVien.ChucVu1!=null? nhanvien.NhanVien.ThongTinNhanVien.ChucVu1.TenChucVu:nhanvien.NhanVien.NhanVienThongTinLuong1.NgachLuong1.TenNgachLuong;
            result.TenPhongBan = nhanvien.NhanVien.BoPhan1.TenBoPhan;
            var nguoiky = (from o in this.Context.HoSoes
                           where o.Oid == result.IDNguoiKy
                           select o).SingleOrDefault();
            result.HoTenNguoiKy = nguoiky.HoTen;
            result.ChucVuNguoiKy = nguoiky.NhanVien.ThongTinNhanVien.ChucVu1!=null ? nguoiky.NhanVien.ThongTinNhanVien.ChucVu1.TenChucVu:"";
            result.DonViNguoiKy = nguoiky.NhanVien.BoPhan1.TenBoPhan;
            result.TuNgayString = String.Format("{0:dd/MM/yyyy}", result.TuNgay);
            result.DenNgayString = String.Format("{0:dd/MM/yyyy}", result.DenNgay);
            return result;
        }
        public IQueryable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find(int thang, int nam, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == Guid.Empty || x.Oid == boPhanId);
            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);
            //
            var result = (from o in this.ObjectSet
                          where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
                          &&
                          (trangThai == 2 || o.TrangThai == trangThai)
                          &&
                          (tatCaMaNhanSu || o.HoSo.MaQuanLy == maNhanSu)
                          && (o.TuNgay.Value.Month >= thang && o.TuNgay.Value.Year >= nam && o.DenNgay.Value.Month <= thang && o.DenNgay.Value.Year <= nam)
                          select new DTO_QuanLyKhaiBaoCongTac_Find() { DenNgay = o.DenNgay, HoTen = o.HoSo.HoTen, IDNhanVien = o.IDNhanVien, IDWebUser = o.IDWebUser, SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc, NgayTao = o.NgayTao, Oid = o.Oid, NoiDung = o.NoiDung, TrangThai = o.TrangThai, TuNgay = o.TuNgay,Buoi=o.Buoi.ToString(),DiaDiem=o.DiaDiem });
            return result;
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
