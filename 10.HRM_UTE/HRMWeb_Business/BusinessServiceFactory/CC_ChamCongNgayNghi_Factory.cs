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
using ERP_Core.Common;

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

        public decimal DangKyNghiPhep_SoNgayPhepConLai(int nam, Guid idNhanVien)
        {
            //
            var SoNgayPhepConLai = (decimal?)(from x in this.Context.CC_QuanLyNghiPhep
                                              join y in this.Context.CC_ChiTietNghiPhep on x.Oid equals y.QuanLyNghiPhep
                                              where x.Nam == nam
                                                    && y.ThongTinNhanVien == idNhanVien
                                              select y.SoNgayPhepConLai.Value).FirstOrDefault() ?? 0;
            return SoNgayPhepConLai;
        }
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              NgaySinh = o.ThongTinNhanVien.NhanVien.HoSo.NgaySinh,
                              ChucDanh = o.ThongTinNhanVien.NhanVien.ChucDanh1 != null ? o.ThongTinNhanVien.NhanVien.ChucDanh1.TenChucDanh : "",
                              ChucVu = o.ThongTinNhanVien.ChucVu1 != null ? o.ThongTinNhanVien.ChucVu1.TenChucVu : "",
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              NamNghiPhep = o.TuNgay.Value.Year.ToString(),
                              SoNgay=o.SoNgay,
                              TuNgay=o.TuNgay,
                              DenNgay=o.DenNgay,
                              NoiNghiPhep=o.NoiNghi,
                              DienGiai=o.DienGiai,
                              DienThoai=o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong!=null ? o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong : o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiNhaRieng,
                              Email=o.ThongTinNhanVien.NhanVien.HoSo.Email,
                              DiaChiLienHe=o.DiaChiLienHe,
                              TenGiayXinPhep=o.TenGiayXinPhep.ToUpper(),
                              LoaiNghiPhep=o.LoaiNghiPhep.ToString(),
                              NgayTao = o.NgayTao
                          }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(int thang, int nam, Guid idNhanVien)
        {
            DateTime ngayDauThang = new DateTime(nam, thang, 1);
            DateTime ngayCuoiThang = HamDungChung.SetTime(ngayDauThang,3);
            //
            var result = (from o in this.ObjectSet
                          where o.TuNgay <= ngayCuoiThang 
                                && o.DenNgay >= ngayDauThang
                                && (o.IDNhanVien == idNhanVien || idNhanVien == Guid.Empty)
                          orderby o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              HinhThucNghi_Name = o.HinhThucNghi1.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan,
                              IDNhanVien = o.IDNhanVien,
                              IDHinhThucNghi = o.IDHinhThucNghi,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              SoNgay = o.SoNgay,
                              NgayTao = o.NgayTao,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser,
                              TrangThai = o.TrangThai,
                              TrangThaiAdmin = o.TrangThaiAdmin
                          });
            return result;
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
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.ThongTinNhanVien.NhanVien.HoSo.Ten, SoHieuCongChuc=o.ThongTinNhanVien.SoHieuCongChuc, TenPhongBan = o.BoPhan.TenBoPhan, HinhThucNghi_Name = o.HinhThucNghi1.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.IDHinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser,DaInNghiPhepNam = (o.DaInNghiPhepNam != null && o.DaInNghiPhepNam.Value == true ? true : false) }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi(int thang, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu, int trangThai, bool? isAdmin)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            DateTime startDate = new DateTime(nam, thang, 1);
            int temp= DateTime.DaysInMonth(nam, thang);
            DateTime endDate= new DateTime(nam, thang, temp);
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);
            bool tatCaMaNhanSu = String.IsNullOrWhiteSpace(maNhanSu);
            //int xxxx= danhSachPhongBanPhanQuyen.Count();
            var result = (from o in this.ObjectSet
                          where
                                ((o.TuNgay.Value.CompareTo(startDate)>=0 &&  o.TuNgay.Value.CompareTo(endDate)<=0)
                                ||
                                (o.DenNgay.Value.CompareTo(startDate) >= 0 && o.DenNgay.Value.CompareTo(endDate) <= 0)
                                ||
                                (o.TuNgay.Value.CompareTo(startDate) <= 0 && o.DenNgay.Value.CompareTo(endDate) >= 0))
                                &&
                                danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                                && (o.SoNgay >= 2 || isAdmin == false)
                                && ((o.TrangThaiAdmin ?? 0) == trangThai || trangThai == 2)
                                && (tatCaMaNhanSu == true || o.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == Guid.Empty || o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                          orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find() {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              HinhThucNghi_Name = o.HinhThucNghi1.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan,
                              IDNhanVien = o.IDNhanVien,
                              IDHinhThucNghi = o.IDHinhThucNghi,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              SoNgay = o.SoNgay,
                              NgayTao = o.NgayTao,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser,
                              LoaiNghiPhep = o.LoaiNghiPhep.ToString(),
                              TrangThai = o.TrangThai,
                              TrangThaiAdmin = o.TrangThaiAdmin,
                              DaInNghiPhepNam = (o.DaInNghiPhepNam != null && o.DaInNghiPhepNam == true ? true : false)
                          });
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
