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
    public class CC_QuanLyNghiPhep_Factory : BaseFactory<Entities, QuanLyNghiPhep>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_QuanLyNghiPhep_Factory.New().CreateAloneObject();
        }
        public static CC_QuanLyNghiPhep_Factory New()
        {
            return new CC_QuanLyNghiPhep_Factory();
        }
        public CC_QuanLyNghiPhep_Factory()
            : base(Database.NewEntities())
        {

        }
        #region Custom
        public bool ExistsByNam(int nam)
        {
            return this.ObjectSet.Any(x => x.Nam == nam);
        }
        public bool CheckExistChiTietNghiPhep(int nam)
        {
            bool result = false;
            QuanLyNghiPhep ql = this.ObjectSet.Where(q => q.Nam == nam).SingleOrDefault();
            if (ql != null)
            {
                result = this.Context.ThongTinNghiPheps.Any(x => x.QuanLyNghiPhep == ql.Oid);
            }
            return result;
        }
        public QuanLyNghiPhep GetByNam(int nam)
        {
            return this.ObjectSet.Where(x => x.Nam == nam && x.GCRecord == null).SingleOrDefault();
        }
        public int GetSoNgayCongThem(DateTime? NgayVaoLam)
        {
            int result = 0;
            int sonam = DateTime.Now.Year - NgayVaoLam.Value.Year;
            if (sonam < 5)
            {
            }
            else if (sonam > 10)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }
            return result;
        }
        public IQueryable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhep_Find(int nam, Guid? bophan, string maNhanSu)
        {
            bool tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) == true);
            bool tatPhongBan = (bophan == null || bophan == Guid.Empty) ? true : false;
            IQueryable<DTO_QuanLyNghiPhep_Find> result;
            Guid thongTinNhanVien = Guid.Empty;
            try
            {
                thongTinNhanVien = new Guid(maNhanSu);
            }
            catch { }
            result = (from o in this.Context.ThongTinNghiPheps
                      where (tatCaMaNhanSu || o.ThongTinNhanVien == thongTinNhanVien || o.ThongTinNhanVien1.SoHieuCongChuc.Contains(maNhanSu))
                      && (tatPhongBan || o.BoPhan == bophan)
                      && o.QuanLyNghiPhep1.Nam == nam
                      orderby o.BoPhan, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                      select new DTO_QuanLyNghiPhep_Find()
                      {
                          Oid = o.Oid,
                          SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
                          HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                          TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                          TongSoNgayPhep = o.SoNgayPhepCoBan + o.SoNgayPhepCongThem ?? 0,
                          SoNgayPhepDaNghi = o.SoNgayPhepDaNghi ?? 0,
                          SoNgayPhepConLai = o.SoNgayPhepConLai ?? 0,
                          SoNgayPhepCongThem = o.SoNgayPhepCongThem ?? 0,
                          GhiChu = o.GhiChu,
                          TruNgayDiDuong = o.TruNgayDiDuong ?? false
                      });
            return result;
        }
        public DTO_QuanLyNghiPhep_Find GetThongTinNghiPhepDTO(Guid Oid)
        {
            DTO_QuanLyNghiPhep_Find result;
            result = (from o in this.Context.ThongTinNghiPheps
                      where o.Oid == Oid
                      select new DTO_QuanLyNghiPhep_Find()
                      {
                          Oid = o.Oid,
                          SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
                          HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                          TenPhongBan = o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                          TongSoNgayPhep = o.SoNgayPhepCoBan ?? 0,
                          SoNgayPhepDaNghi = o.SoNgayPhepDaNghi ?? 0,
                          SoNgayPhepConLai = o.SoNgayPhepConLai ?? 0,
                          SoNgayPhepCongThem = o.SoNgayPhepCongThem ?? 0,
                          GhiChu = o.GhiChu
                      }).SingleOrDefault();
            return result;
        }
        public void ThongTinNghiPhep_Save(Guid Oid, decimal tongSoNgayPhep, decimal soNgayPhepCongThem, decimal daNghi, decimal conLai, string ghiChu)
        {
            var result = (from o in this.Context.ThongTinNghiPheps
                          where o.Oid == Oid
                          select o).SingleOrDefault();
            result.SoNgayPhepCoBan = tongSoNgayPhep;
            result.SoNgayPhepCongThem = soNgayPhepCongThem;
            result.SoNgayPhepDaNghi = daNghi;
            result.SoNgayPhepConLai = conLai;
            result.GhiChu = ghiChu;
            result.GetContext().SaveChanges();
        }
        public decimal QuanLyNghiPhep_NgayPhepConLai(int nam, Guid idNhanVien)
        {
            decimal result;
            result = (from o in this.Context.ThongTinNghiPheps
                     where (o.ThongTinNhanVien1.Oid == idNhanVien)
                     && o.QuanLyNghiPhep1.Nam == nam
                     select o.SoNgayPhepConLai).FirstOrDefault() ?? 0;
            return result;
        }
        #endregion
    }//end class
}
