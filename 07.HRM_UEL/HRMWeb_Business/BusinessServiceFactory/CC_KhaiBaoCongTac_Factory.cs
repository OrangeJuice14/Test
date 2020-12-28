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
using System.Data.Entity.Core.Objects;

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
        public IQueryable<CC_KhaiBaoCongTac> CaNhanKhaiBaoCongTac_Find(int thang, int nam, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var result = (from o in this.ObjectSet
                          where o.IDWebUser == webUserId
                          &&
                          (
                              (thang == 0 ? true : o.TuNgay.Value.Month <= thang && o.TuNgay.Value.Year <= nam)
                              &&
                              (thang == 0 ? true : o.DenNgay.Value.Month >= thang && o.DenNgay.Value.Year >= nam)
                          )
                          select o);
            return result;

            return null;
        }
        public IQueryable<DTO_QuanLyKhaiBaoCongTac_Find> QuanLyKhaiBaoCongTac_Find(int thang, int nam, Guid? boPhanId, int? trangThai, string maNhanSu, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            Boolean tatCaTrangThai = (trangThai == null);
            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);
            Guid webGroupId = WebUser_Factory.New().GetByID(webUserId).WebGroupID ?? Guid.Empty;

            var result = (from o in this.ObjectSet
                          where danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
                          &&
                          (tatCaTrangThai || o.TrangThai == trangThai)
                          &&
                          (tatCaMaNhanSu || o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc == maNhanSu)
                          && 
                          (
                              (thang == 0 ? true : o.TuNgay.Value.Month <= thang && o.TuNgay.Value.Year <= nam) 
                              && 
                              (thang == 0 ? true : o.DenNgay.Value.Month >= thang && o.DenNgay.Value.Year >= nam)
                          )
                          //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                          //BGH lấy danh sách trưởng đơn vị
                          && ((webGroupId == new Guid("00000000-0000-0000-0000-000000000004") || webGroupId == new Guid("00000000-0000-0000-0000-000000000005")) ?
                          o.HoSo.WebUsers.FirstOrDefault().Oid != webUserId : webGroupId == new Guid("00000000-0000-0000-0000-000000000002") ?
                          o.HoSo.WebUsers.FirstOrDefault().WebGroupID == new Guid("00000000-0000-0000-0000-000000000004") : true)
                          select new DTO_QuanLyKhaiBaoCongTac_Find()
                          {
                              DenNgay = o.DenNgay,
                              HoTen = o.HoSo.HoTen,
                              IDNhanVien = o.IDNhanVien,
                              IDWebUser = o.IDWebUser,
                              SoHieuCongChuc = o.HoSo.NhanVien.ThongTinNhanVien.SoHieuCongChuc,
                              NgayTao = o.NgayTao,
                              Oid = o.Oid,
                              NoiDung = o.NoiDung,
                              TrangThai = o.TrangThai,
                              TuNgay = o.TuNgay,
                              Buoi = o.Buoi.ToString(),
                              DiaDiem = o.DiaDiem
                          });
            return result;

            return null;
        }

        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_KhaiBaoCongTac item in deleteList)
            {
                context.DeleteObject(item);
            }
        }

        public int LaySoKhaiBaoCongTacDangChoXet(int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            
            Guid webGroupId = WebUser_Factory.New().GetByID(webUserId).WebGroupID ?? Guid.Empty;
            IQueryable<CC_KhaiBaoCongTac> result = null;
            if (webGroupId == new Guid("00000000-0000-0000-0000-000000000002")) //nếu là BGH thì lấy danh sách phòng ban được phân quyền
            {
                var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId);
                result = (from o in this.ObjectSet
                          let tuNgayTruncate = EntityFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = EntityFunctions.TruncateTime(o.DenNgay)
                          where //o.HoSo.NhanVien.BoPhan == boPhanId
                          danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.HoSo.NhanVien.BoPhan)
                          //nếu truyền vào tháng == 0 thì lấy cả năm được chọn
                          && (tuNgayTruncate.Value.Month <= thang && tuNgayTruncate.Value.Year <= nam)
                          && (denNgayTruncate.Value.Month >= thang && denNgayTruncate.Value.Year >= nam)
                          && o.TrangThai == -1
                          //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                          //BGH lấy danh sách trưởng đơn vị
                          && (o.HoSo.WebUsers.FirstOrDefault().WebGroupID == new Guid("00000000-0000-0000-0000-000000000004"))
                          select o);
            }
            else //trưởng đơn vị thì lấy cả phòng ban trừ trưởng đơn vị
            {
                result = (from o in this.ObjectSet
                          let tuNgayTruncate = EntityFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = EntityFunctions.TruncateTime(o.DenNgay)
                          where o.HoSo.NhanVien.BoPhan == boPhanId
                          //nếu truyền vào tháng == 0 thì lấy cả năm được chọn
                          && (tuNgayTruncate.Value.Month <= thang && tuNgayTruncate.Value.Year <= nam)
                          && (denNgayTruncate.Value.Month >= thang && denNgayTruncate.Value.Year >= nam)
                          && o.TrangThai == -1
                          //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                          //BGH lấy danh sách trưởng đơn vị
                          //&& ((webGroupId == new Guid("00000000-0000-0000-0000-000000000004") || webGroupId == new Guid("00000000-0000-0000-0000-000000000005")) ?
                          //o.HoSo.WebUsers.FirstOrDefault().Oid != webUserId : webGroupId == new Guid("00000000-0000-0000-0000-000000000002") ?
                          //o.HoSo.WebUsers.FirstOrDefault().WebGroupID == new Guid("00000000-0000-0000-0000-000000000004") : true)
                          select o);
            }
            return result.Count();
        }
        #endregion
    }//end class
}
