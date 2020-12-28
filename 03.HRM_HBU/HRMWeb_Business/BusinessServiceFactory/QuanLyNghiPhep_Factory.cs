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
            if(ql!=null)
            {
                result= this.Context.CC_ChiTietNghiPhep.Any(x => x.QuanLyNghiPhep == ql.Oid);
            }
            return result;
        }
        public CC_QuanLyNghiPhep GetByNam(int nam)
        {
            return this.ObjectSet.Where(x => x.Nam == nam).SingleOrDefault();
        }
        public DTO_QuanLyNghiPhep_InGiayNghiPhep QuanLyNghiPhep_InGiayNghiPhepNam(Guid NhanVien, int Nam)
        {
            DTO_QuanLyNghiPhep_InGiayNghiPhep result = new DTO_QuanLyNghiPhep_InGiayNghiPhep();
            var hoso = (from o in this.Context.HoSoes
                        where o.Oid == NhanVien
                        select o).SingleOrDefault();
            var ql = (from o in this.Context.CC_ChiTietNghiPhep
                      where o.CC_QuanLyNghiPhep.Nam == Nam
                      && o.ThongTinNhanVien == NhanVien
                      select o).FirstOrDefault();
            result.ChucDanh = hoso.NhanVien.ChucDanh1.TenChucDanh;
            result.HoTen = hoso.HoTen;
            result.TenDonVi = hoso.NhanVien.BoPhan1.TenBoPhan.ToUpper();
            decimal SoNgayCongThem = ql.SoNgayPhepCongThem??0;
            if (SoNgayCongThem==1)
                result.LamViec5Den10Nam = 1;
            else if (SoNgayCongThem == 2)
                result.LamViecTren10Nam = 2;                 
            result.MaQuanLy = hoso.MaQuanLy;
            result.Nam = Nam;
            result.NgayVaoCoQuan= String.Format("{0:dd/MM/yyyy}", hoso.NhanVien.NgayVaoCoQuan.Value);
            result.PhepConLai = 0;
            result.PhepNamNay = 12 + ql.SoNgayPhepCongThem??0;
            result.TongSoNgayPhepDuocNghi = result.PhepNamNay + result.PhepConLai;
            result.TongSoPhepDaNghiTrongNam = ql.SoNgayPhepDaNghi ?? 0;
            result.SoPhepChuyenSangNamSau = result.TongSoNgayPhepDuocNghi - result.TongSoPhepDaNghiTrongNam;
            result.ChiTietNghiPhep = new List<DTO_QuanLyNghiPhep_InGiayNghiPhep_ChiTiet>();
            CC_ChamCongNgayNghi_Factory ccfac = CC_ChamCongNgayNghi_Factory.New();
            List<CC_ChamCongNgayNghi> listNghi = ccfac.ListByNhanVienAndYear(NhanVien, Nam).ToList();
            decimal SoNgayDuocNghi = result.TongSoNgayPhepDuocNghi;
            foreach (CC_ChamCongNgayNghi nghi in listNghi)
            {
                DTO_QuanLyNghiPhep_InGiayNghiPhep_ChiTiet dto = new DTO_QuanLyNghiPhep_InGiayNghiPhep_ChiTiet();
                dto.TuNgay= String.Format("{0:dd/MM/yy}", nghi.TuNgay.Value);
                dto.DenNgay = String.Format("{0:dd/MM/yy}", nghi.DenNgay.Value);
                dto.SoNgayXinNghi = Convert.ToDecimal((nghi.DenNgay.Value - nghi.TuNgay.Value).TotalDays+1);
                dto.SoNgayConLai = SoNgayDuocNghi - dto.SoNgayXinNghi;
                SoNgayDuocNghi = dto.SoNgayConLai??0;
                dto.LyDo = nghi.DienGiai;
                result.ChiTietNghiPhep.Add(dto);
            }          
            return result;
        }
        public int GetSoNgayCongThem(DateTime? NgayVaoLam)
        {
            int result =0;
            int sonam = DateTime.Now.Year - NgayVaoLam.Value.Year;
            if (sonam<5)
            {
            }
            else if (sonam>10)
            {
                result = 2;
            }
            else
            {
                result = 1;
            }
            return result;
        }
        public IQueryable<DTO_QuanLyNghiPhep_Find> QuanLyNghiPhep_Find(Guid QuanLyNghiPhepOid, string boPhanId)
        {
            bool tatCaDonVi = (String.IsNullOrWhiteSpace(boPhanId) == true);
            if (tatCaDonVi)
            {
                var result = (from o in this.Context.CC_ChiTietNghiPhep
                              where o.QuanLyNghiPhep == QuanLyNghiPhepOid
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
                                  SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString()
                              });
                return result;
            }
            else
            {
                var result = (from o in this.Context.CC_ChiTietNghiPhep
                              where o.QuanLyNghiPhep == QuanLyNghiPhepOid
                              && o.BoPhan1.Oid == new Guid(boPhanId)
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
                                  SoNgayPhepCongThem = o.SoNgayPhepCongThem.ToString()
                              });
                return result;
            }
        }
        #endregion
    }//end class
}
