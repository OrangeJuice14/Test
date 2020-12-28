using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection; 
using System.Data.Linq;
using System.Data;

using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class CC_ChamCongNgayNghi_Factory : BaseFactory<Entities, CC_ChamCongNgayNghi>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChamCongNgayNghi_Factory.New().CreateAloneObject();
        }
        public static CC_ChamCongNgayNghi_Factory New()
        {
            return new CC_ChamCongNgayNghi_Factory();
        }
        public CC_ChamCongNgayNghi_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public CC_ChamCongNgayNghi GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }
        public DTO_ChamCongNgayNghi_Find GetDTO_ChamCongNgayNghi_Find_ByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.HoSo.HoTen, SoHieuCongChuc=o.HoSo.MaQuanLy,MaNhanSu=o.HoSo.MaQuanLy, TenPhongBan = o.Department1.TenBoPhan, HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.IDHinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser, TinhThanh = o.TinhThanh1.TenTinhThanh,SoNgayNghiPhepNamTruoc=o.SoNgayNghiPhepNamTruoc.ToString(), TruNgayPhepDiDuong = o.TruNgayPhepDiDuong.ToString() }).SingleOrDefault();
            return result;
        }
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find() {
                              HoTen = o.HoSo.HoTen,
                              MaNhanSu = o.HoSo.MaQuanLy,
                              TenPhongBan = o.Department1.TenBoPhan,
                              ChucVu=o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1!=null? o.HoSo.NhanVien.ThongTinNhanVien.ChucVu1.TenChucVu:"",
                              Oid = o.Oid,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              NgayTao = o.NgayTao,
                              DienGiai = o.DienGiai,
                              TinhThanh = o.TinhThanh1.TenTinhThanh
                          }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_TinhThanh> GetList_TinhThanh()
        {
            var result = (from o in this.Context.TinhThanhs
                          where o.GCRecord==null
                          select new DTO_TinhThanh() {Oid=o.Oid,TenTinhThanh=o.TenTinhThanh });
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi(int thang, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);
            bool tatCaMaNhanSu = String.IsNullOrWhiteSpace(maNhanSu);
            //int xxxx= danhSachPhongBanPhanQuyen.Count();
            var result = (from o in this.ObjectSet
                         
                          let tuNgayTruncate = EntityFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = EntityFunctions.TruncateTime(o.DenNgay)
                          where (
                                        (tuNgayTruncate.Value.Month >= thang && tuNgayTruncate.Value.Year == nam)
                                        &&
                                         (denNgayTruncate.Value.Month <= thang && denNgayTruncate.Value.Year == nam)
                                    
                                )
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)

                                && (tatCaMaNhanSu == true || o.HoSo.MaQuanLy.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                          orderby o.HoSo.Ten, o.Department1.TenBoPhan, o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.HoSo.HoTen, SoHieuCongChuc = o.HoSo.MaQuanLy, MaNhanSu = o.HoSo.MaQuanLy, TenPhongBan = o.Department1.TenBoPhan, HinhThucNghi_Name = o.CC_HinhThucNghi.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.IDHinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser,TinhThanh=o.TinhThanh1.TenTinhThanh, SoNgayNghiPhepNamTruoc = o.SoNgayNghiPhepNamTruoc.ToString(), TruNgayPhepDiDuong = o.TruNgayPhepDiDuong.ToString() });
            //int xxxx = result.Count();
            return result;
        }


        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChamCongNgayNghi item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
