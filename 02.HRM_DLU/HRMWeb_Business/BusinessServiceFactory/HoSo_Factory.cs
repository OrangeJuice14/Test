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
    public class HoSo_Factory : BaseFactory<Entities, HoSo>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return HoSo_Factory.New().CreateAloneObject();
        }
        public static HoSo_Factory New()
        {
            return new HoSo_Factory();
        }
        public HoSo_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public IQueryable<HoSo> GetAll_GCRecordIsNull()
        {//su dung cho cham cong only
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong==false
                         orderby o.HoTen ascending
                         select o;
            return result;
        }

        public IQueryable<HoSo> GetAll_KhongPhaiGiangVien_GCRecordIsNull()
        {//su dung cho cham cong only
            //Guid giangVienId = Guid.Parse("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03");
            LoaiNhanSu_Factory loaiNhanSuFactory = LoaiNhanSu_Factory.New();
            loaiNhanSuFactory.Context = this.Context;
            IQueryable<LoaiNhanSu> loaiKhongPhaiGiangVienList = loaiNhanSuFactory.GetListByNotLikeName_GCRecordIsNull("giảng viên");
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                         && loaiKhongPhaiGiangVienList.Any(x => x.Oid == o.NhanVien.ThongTinNhanVien.LoaiNhanSu)
                         //&& o.NhanVien.ThongTinNhanVien.LoaiNhanSu != Guid.Parse("D8A7B32D-CCE6-4DA9-9F6D-6D28F5046D03")
                         //&& o.NhanVien.ThongTinNhanVien.LoaiNhanSu1.TenLoaiNhanSu.ToLower().Contains("giảng viên")==false
                         orderby o.HoTen ascending
                         select o;
            return result;
        }

        public IQueryable<HoSo> GetListByMaBoPhan_GCRecordIsNull(Guid? maBoPhan)
        {//su dung cho cham cong only
            bool tatCaBoPhan = (maBoPhan == null ? true : false);
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                            && (tatCaBoPhan || o.NhanVien.BoPhan == maBoPhan)
                            && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong==false
                         orderby o.HoTen ascending
                         select o;
            return result;
        }
        public IQueryable<HoSo> KhaiBaoCongTac_DanhSachTruongDonVi()
        {//su dung cho cham cong only
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                            && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                            && o.NhanVien.ThongTinNhanVien.ChucVu1.LaQuanLy==true
                         orderby o.HoTen ascending
                         select o;
            return result;
        }
        public HoSo GetByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select o).SingleOrDefault();
            return result;
        }

        public DTO_HoSoNhanVien GetHoSoNhanVienByID(Guid oid)
        {
            var result = (from o in this.ObjectSet 
                          join x in this.Context.NhanViens on o.Oid equals x.Oid
                          where o.Oid == oid
                                && o.GCRecord == null
                          select new DTO_HoSoNhanVien {
                              MaQuanLy = o.MaQuanLy,
                              TenBoPhan = x.BoPhan1.TenBoPhan,
                              MaSoThue = x.NhanVienThongTinLuong1.MaSoThue,
                              HoTen = o.HoTen

                          }).SingleOrDefault();
            return result;
        }

        public IQueryable<HoSo> GetList_HoSoNhanVienCoIDChamCong()
        {
            //
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                               && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                               && (o.IDNhanSu_ChamCong ?? 0) > 0
                         orderby o.HoTen ascending
                         select o;
            return result;
        }
        #endregion
    }//end class
}
