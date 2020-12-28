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
            var factory = HoSo_Factory.New();
            //
            bool tatCaBoPhan = (maBoPhan == null ? true : false);
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                            && (tatCaBoPhan || o.NhanVien.BoPhan == maBoPhan)
                            && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong==false
                         orderby o.HoTen ascending
                         select o;
            return result;
        }
        public List<Guid> GetListOidAll_GCRecordIsNull()
        {//su dung cho cham cong only
            var result = (from o in this.ObjectSet
                         where o.GCRecord == null
                            && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                            && o.NhanVien.ThongTinNhanVien!=null
                         select o.Oid).Distinct().ToList();
            return result;
        }
        public IQueryable<DTO_HoSoNhanVien> GetListByIdNV_GCRecordIsNull(Guid idNhanVien)
        {//su dung cho cham cong only
            var result = from o in this.ObjectSet
                         where o.GCRecord == null
                            && o.NhanVien.TinhTrang1.KhongConCongTacTaiTruong == false
                            && o.NhanVien.BoPhan==(from a in this.ObjectSet
                                                   where a.Oid==idNhanVien
                                                   select a.NhanVien.BoPhan
                                                   ).FirstOrDefault()
                         orderby o.HoTen ascending
                         select new DTO_HoSoNhanVien()
                         {
                             Oid=o.Oid,
                             MaQuanLy=o.MaQuanLy,
                             HoTen=o.HoTen,
                             TenPhongBan=o.NhanVien.BoPhan1.TenBoPhan
                             
                         };
            return result;
        }
        public IQueryable<HoSo> GetListByMaBoPhan_GCRecordIsNull(Guid? maBoPhan, DateTime ngayChot)
        {//su dung cho cham cong only
            var factory = HoSo_Factory.New(); 
            //
            bool tatCaBoPhan = (maBoPhan == null ? true : false);
            var result = from o in this.ObjectSet
                         join a in this.Context.NhanViens on o.Oid equals a.Oid
                         where o.GCRecord == null
                            && (tatCaBoPhan || o.NhanVien.BoPhan == maBoPhan)
                            && a.NgayVaoCoQuan.Value <= ngayChot
                            && a.ThongTinNhanVien.NhanVien.TinhTrang1.KhongConCongTacTaiTruong==false
                         orderby o.HoTen ascending
                         select o;
            return result;
        }
        public IQueryable<HoSo> GetListByMaBoPhan_NhanVienThoiViec(Guid? maBoPhan,DateTime tuNgay, DateTime ngayChot)
        {//su dung cho cham cong only
            var factory = HoSo_Factory.New();
            //
            List<Guid> dsIdNhanVien2 = (from a in this.Context.QuyetDinhThoiViecs
                                        where a.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && tuNgay <= a.NghiViecTuNgay
                                        //&& a.NghiViecTuNgay <= ngayChot
                                        && a.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == maBoPhan
                                        select a.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIdNhanVien3 = (from a in this.Context.QuyetDinhNghiHuus
                                        where a.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                        && tuNgay <= a.NghiViecTuNgay
                                        //&& a.NghiViecTuNgay <= ngayChot
                                        && a.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == maBoPhan
                                        select a.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            List<Guid> dsIDNVTotal = dsIdNhanVien2.Union(dsIdNhanVien3).ToList();
            var result = from o in this.ObjectSet
                         where dsIDNVTotal.Contains(o.Oid)
                         select o;
            return result;
        }
        public IQueryable<HoSo> GetListByMaBoPhan_NhanVienThoiViec_ChamCongNgoaiGio(Guid? maBoPhan)
        {//su dung cho cham cong only
            var factory = HoSo_Factory.New();
            CC_KyChamCong_Factory cc = CC_KyChamCong_Factory.New();
            //
            DateTime date = DateTime.Now;
            List<Guid> dsIdNhanVien2 = new List<Guid>();
            List<Guid> dsIdNhanVien3 = new List<Guid>();
            CC_KyChamCong kyChamCong = cc.GetByMonthAndYear(date.Month, date.Year);
            if (kyChamCong != null)
            {
                dsIdNhanVien2 = (from a in this.Context.QuyetDinhThoiViecs
                                 where a.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                 && kyChamCong.TuNgay <= a.NghiViecTuNgay
                                 //&& a.NghiViecTuNgay <= kyChamCong.DenNgay
                                 && a.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == maBoPhan
                                 select a.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
                dsIdNhanVien3 = (from a in this.Context.QuyetDinhNghiHuus
                                 where a.QuyetDinhCaNhan.QuyetDinh.GCRecord == null
                                 && kyChamCong.TuNgay <= a.NghiViecTuNgay
                                 //&& a.NghiViecTuNgay <= kyChamCong.DenNgay
                                 && a.QuyetDinhCaNhan.ThongTinNhanVien1.NhanVien.BoPhan == maBoPhan
                                 select a.QuyetDinhCaNhan.ThongTinNhanVien ?? Guid.Empty).Distinct().ToList();
            }
            List<Guid> dsIDNVTotal = dsIdNhanVien2.Union(dsIdNhanVien3).ToList();
            var result = from o in this.ObjectSet
                         where dsIDNVTotal.Contains(o.Oid)
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
        //public static void FullDelete(Entities context, params Object[] deleteList)
        //{
        //    //foreach (AppL item in deleteList)
        //    //{
        //    //    context.DeleteObject(item);
        //    //}
        //}
        #endregion
    }//end class
}
