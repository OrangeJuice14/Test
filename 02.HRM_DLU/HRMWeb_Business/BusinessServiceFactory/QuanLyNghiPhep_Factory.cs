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
    public class QuanLyNghiPhep_Factory : BaseFactory<Entities, CC_QuanLyNghiPhep>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return QuanLyNghiPhep_Factory.New().CreateAloneObject();
        }
        public static QuanLyNghiPhep_Factory New()
        {
            return new QuanLyNghiPhep_Factory();
        }
        public QuanLyNghiPhep_Factory()
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
            CC_QuanLyNghiPhep ql = this.ObjectSet.Where(q => q.Nam == nam).SingleOrDefault();
            if (ql != null)
            {
                result = this.Context.CC_ChiTietNghiPhep.Any(x => x.QuanLyNghiPhep == ql.Oid);
            }
            return result;
        }
        public CC_QuanLyNghiPhep GetByNam(int nam)
        {
            return this.ObjectSet.Where(x => x.Nam == nam).SingleOrDefault();
        }
        public CC_ChiTietNghiPhep GetChiTietNghiPhepByNamAndIDNV(int nam, Guid? IDNV)
        {
            var result = (from o in this.Context.CC_ChiTietNghiPhep
                          where o.ThongTinNhanVien == IDNV && o.CC_QuanLyNghiPhep.Nam == nam
                          select o
                        ).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhepNam_Find(int nam, Guid boPhanId,Guid nhanVienId)
        {
            //
            var result = (from o in this.Context.CC_ChiTietNghiPhep
                            where o.CC_QuanLyNghiPhep.Nam == nam
                                  && (boPhanId == o.BoPhan || boPhanId == Guid.Empty)
                                  && (nhanVienId == o.ThongTinNhanVien || nhanVienId == Guid.Empty)
                            orderby o.BoPhan1.TenBoPhan, o.ThongTinNhanVien1.NhanVien.HoSo.HoTen
                            select new DTO_QuanLyNghiPhep_Find()
                            {
                                Oid = o.Oid,
                                MaQuanLy = o.ThongTinNhanVien1.NhanVien.HoSo.MaQuanLy,
                                HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                                TenPhongBan = o.BoPhan1.TenBoPhan,
                                TongSoNgayPhep = o.TongSoNgayPhep.ToString(),
                                SoNgayPhepDaNghi = o.SoNgayPhepDaNghi.ToString(),
                                SoNgayPhepConLai = o.SoNgayPhepConLai.ToString(),
                                SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString(),
                                SoNgayPhepNamTruoc = o.SoNgayPhepNamTruoc.ToString(),
                                SoNgayPhepNamHienTai = o.SoNgayPhepNamHienTai.ToString()

                            });
            return result;
        }
        #endregion
    }//end class
}
