using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
using System.Globalization;

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
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.HoSo.HoTen, MaNhanSu = o.HoSo.MaQuanLy, TenPhongBan = o.BoPhan.TenBoPhan, HinhThucNghi_Name = o.HinhThucNghi.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.IDHinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> FindForChamCongNgayNghi(int thang, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId, Guid? idLoaiNhanSu)
        {
            DateTime dauThang;
            DateTime cuoiThang;
            if (thang == 0)
            {
                dauThang = new DateTime(nam, 1, 1);
                cuoiThang = dauThang.AddYears(1).AddDays(-1);
            }
            else
            {
                dauThang = new DateTime(nam, thang, 1);
                cuoiThang = dauThang.AddMonths(1).AddDays(-1);
            }
            Guid webGroupId = WebUser_Factory.New().GetByID(webUserId).WebGroupID ?? Guid.Empty;
            //DateTime ngayhientai = new DateTime(nam, thang, 1);
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);
            bool tatCaMaNhanSu = String.IsNullOrWhiteSpace(maNhanSu);
            //int xxxx= danhSachPhongBanPhanQuyen.Count();
            var result = (from o in this.ObjectSet
                         
                          let tuNgayTruncate = EntityFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = EntityFunctions.TruncateTime(o.DenNgay)
                          where 
                          (
                                        (denNgayTruncate >= dauThang && denNgayTruncate <= cuoiThang) 
                                        || 
                                        (tuNgayTruncate <= cuoiThang && tuNgayTruncate >= dauThang) 
                                        || 
                                        (tuNgayTruncate <= dauThang && denNgayTruncate >= cuoiThang)

                                )
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)

                                && (tatCaMaNhanSu == true || o.HoSo.MaQuanLy.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || o.HoSo.NhanVien.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                                && (webGroupId == new Guid("00000000-0000-0000-0000-000000000002") ? o.ThongTinNhanVien.WebUsers.FirstOrDefault().Oid != webUserId : true)
                          orderby o.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.HoSo.HoTen, MaNhanSu = o.HoSo.MaQuanLy, TenPhongBan = o.BoPhan.TenBoPhan, HinhThucNghi_Name = o.HinhThucNghi.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.IDHinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser,TrangThai=o.TrangThai });
            //int xxxx = result.Count();
            return result;
        }
        public IQueryable<CC_ChamCongNgayNghi> ListByNhanVienAndYear(Guid IDNV,int Nam)
        {
            var result = (from o in this.ObjectSet
                          where o.IDNhanVien == IDNV && o.TuNgay.Value.Year == Nam
                          orderby o.TuNgay
                          select o);
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
