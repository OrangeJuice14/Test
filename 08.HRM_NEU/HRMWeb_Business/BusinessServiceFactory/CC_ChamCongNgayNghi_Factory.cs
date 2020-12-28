using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection; 
using System.Data.Linq;
using System.Data;
using System.Data.Entity;

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
        public DTO_ChamCongNgayNghi_Find ChamCongNgayNghi_Report(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              NgaySinh = o.ThongTinNhanVien.NhanVien.HoSo.NgaySinh,
                              ChucDanh = o.ThongTinNhanVien.NhanVien.ChucDanh1 != null ? o.ThongTinNhanVien.NhanVien.ChucDanh1.TenChucDanh : "",
                              TenPhongBan = o.ThongTinNhanVien.NhanVien.BoPhan1.TenBoPhan,
                              NamNghiPhep = o.TuNgay.Value.Year.ToString(),
                              SoNgay = o.SoNgay,
                              TuNgay = o.TuNgay,
                              DenNgay = o.DenNgay,
                              NoiNghiPhep = o.NoiNghi,
                              DienGiai = o.DienGiai,
                              DienThoai = o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong != null ? o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiDiDong : o.ThongTinNhanVien.NhanVien.HoSo.DienThoaiNhaRieng,
                              Email = o.ThongTinNhanVien.NhanVien.HoSo.Email,
                              DiaChiLienHe = o.DiaChiLienHe,
                              TenGiayXinPhep = o.TenGiayXinPhep.ToUpper(),
                              LoaiNghiPhep = o.LoaiNghiPhep.ToString(),
                              TruNgayDiDuong = o.TruNgayDiDuong,
                              SoNgayDiDuong = o.SoNgayDiDuong
                          }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_ChamCongNgayNghi_Find> DangKyChamCongNgayNghi_Find(int thang, int nam, Guid idNhanVien)
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
            var result = (from o in this.ObjectSet

                          let tuNgayTruncate = DbFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = DbFunctions.TruncateTime(o.DenNgay)
                          where (
                                    ((denNgayTruncate >= dauThang && denNgayTruncate <= cuoiThang) || (tuNgayTruncate <= cuoiThang && tuNgayTruncate >= dauThang) || (tuNgayTruncate <= dauThang && denNgayTruncate >= cuoiThang))
                                        //((thang == 0 ? true : tuNgayTruncate.Value.Month <= thang) && tuNgayTruncate.Value.Year <= nam)
                                        //&&
                                        // ((thang == 0 ? true : denNgayTruncate.Value.Month >= thang) && denNgayTruncate.Value.Year >= nam)
                                )
                                && o.IDNhanVien == idNhanVien
                                && o.GCRecord == null
                          orderby o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find()
                          {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              HinhThucNghi_Name = o.HinhThucNghi2.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan,
                              IDNhanVien = o.IDNhanVien,
                              IDHinhThucNghi = o.IDHinhThucNghi,
                              TuNgay = o.TuNgay,
                              DenNgay = (o.TuNgay == o.DenNgay) ? null : o.DenNgay,
                              SoNgay = o.SoNgay,
                              SoNgayPhepConLai = o.SoNgayPhepConLai,
                              NgayTao = o.NgayTao,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser,
                              TrangThai = o.TrangThai ?? -1, //nếu null thì hiện chờ xét
                              TrangThaiAdmin = o.TrangThaiAdmin ?? -1,
                              TinhThanh = o.TinhThanh,
                              TruNgayDiDuong = o.TruNgayDiDuong,
                              SoNgayDiDuong = o.TruNgayDiDuong.HasValue && o.TruNgayDiDuong.Value == true ? o.SoNgayDiDuong : null,
                              CacBuoiTrongNgay_TuNgay = o.CacBuoiTrongNgay.TenBuoi,
                              CacBuoiTrongNgay_DenNgay = (o.TuNgay == o.DenNgay) ? "" : o.CacBuoiTrongNgay1.TenBuoi
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
        public int LaySoDangKyNghiDangChoXet(int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, Guid webUserId)
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
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;

            Guid webGroupId = WebUser_Factory.New().GetByID(webUserId).WebGroupID ?? Guid.Empty;
            IQueryable<CC_ChamCongNgayNghi> result = null;
            if (webGroupId == new Guid("00000000-0000-0000-0000-000000000002")) //nếu là BGH thì lấy danh sách phòng ban được phân quyền
            {
                var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId);
                result = (from o in this.ObjectSet
                          let tuNgayTruncate = DbFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = DbFunctions.TruncateTime(o.DenNgay)
                          where //o.IDBoPhan == boPhanId
                          danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)
                          &&
                          //nếu truyền vào tháng == 0 thì lấy cả năm được chọn
                          ((denNgayTruncate >= dauThang && denNgayTruncate <= cuoiThang) || (tuNgayTruncate <= cuoiThang && tuNgayTruncate >= dauThang) || (tuNgayTruncate <= dauThang && denNgayTruncate >= cuoiThang))
                          //&& (tuNgayTruncate.Value.Month <= thang && tuNgayTruncate.Value.Year <= nam)
                          //&& (denNgayTruncate.Value.Month >= thang && denNgayTruncate.Value.Year >= nam)
                          && o.TrangThai == -1
                          //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                          //BGH lấy danh sách trưởng đơn vị
                          && ((webGroupId == new Guid("00000000-0000-0000-0000-000000000004") || webGroupId == new Guid("00000000-0000-0000-0000-000000000005")) ?
                          o.ThongTinNhanVien.WebUsers.FirstOrDefault().Oid != webUserId : webGroupId == new Guid("00000000-0000-0000-0000-000000000002") ?
                          o.ThongTinNhanVien.WebUsers.FirstOrDefault().WebGroupID == new Guid("00000000-0000-0000-0000-000000000004") : true)
                          select o);
            }
            else //trưởng đơn vị thì lấy cả phòng ban trừ trưởng đơn vị
            {
                result = (from o in this.ObjectSet
                          let tuNgayTruncate = DbFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = DbFunctions.TruncateTime(o.DenNgay)
                          where o.IDBoPhan == boPhanId
                          &&
                          //nếu truyền vào tháng == 0 thì lấy cả năm được chọn
                          ((denNgayTruncate >= dauThang && denNgayTruncate <= cuoiThang) || (tuNgayTruncate <= cuoiThang && tuNgayTruncate >= dauThang) || (tuNgayTruncate <= dauThang && denNgayTruncate >= cuoiThang))
                          //&& (tuNgayTruncate.Value.Month <= thang && tuNgayTruncate.Value.Year <= nam)
                          //&& (denNgayTruncate.Value.Month >= thang && denNgayTruncate.Value.Year >= nam)
                          && o.TrangThai == -1
                          //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                          //BGH lấy danh sách trưởng đơn vị
                          && ((webGroupId == new Guid("00000000-0000-0000-0000-000000000004") || webGroupId == new Guid("00000000-0000-0000-0000-000000000005")) ?
                          o.ThongTinNhanVien.WebUsers.FirstOrDefault().Oid != webUserId : webGroupId == new Guid("00000000-0000-0000-0000-000000000002") ?
                          o.ThongTinNhanVien.WebUsers.FirstOrDefault().WebGroupID == new Guid("00000000-0000-0000-0000-000000000004") : true)
                          select o);
            }
            return result.Count();
        }
        public IQueryable<CacBuoiTrongNgay> CacBuoiTrongNgay()
        {
            var result = from q in this.Context.CacBuoiTrongNgays
                         select q;
            return result;
        }
        public decimal LaySoNgayDangKyNghi(DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay)
        {
            decimal result = 0;
            decimal soNgayTru = 0;
            if (tuNgay == denNgay)
            { 
                soNgayTru = (from q in this.Context.CacBuoiTrongNgays
                            where q.Oid == buoiTuNgay
                            select q.GiaTri).FirstOrDefault() ?? 0;
            }
            else
            {
                soNgayTru = ((from q in this.Context.CacBuoiTrongNgays
                             where q.Oid == buoiTuNgay
                             select q.GiaTri).FirstOrDefault() + (from q in this.Context.CacBuoiTrongNgays
                                                                      where q.Oid == buoiDenNgay
                                                                      select q.GiaTri).FirstOrDefault()) ?? 0;
            }
            for (var date = tuNgay; date <= denNgay; date = date.AddDays(1))
            {
                var kiemTraTrungNgayNghiTrongNam = (from q in this.Context.NgayNghiTrongNams
                                                    where q.NgayNghi == date && q.GCRecord == null
                                                    select q).Count() > 0;
                if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday && !kiemTraTrungNgayNghiTrongNam)
                    result++;
            }
            return result - soNgayTru;
        }
        public bool KiemTraTruNgayDiDuong(int nam, Guid idNhanVien)
        {
            return (from q in this.Context.ThongTinNghiPheps
                     where q.ThongTinNhanVien == idNhanVien && q.QuanLyNghiPhep1.Nam == nam
                     select q.TruNgayDiDuong).FirstOrDefault() ?? false;
        }
        public DTO_ChamCongNgayNghi_Find GetDTO_ChamCongNgayNghi_Find_ByID(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                          select new DTO_ChamCongNgayNghi_Find() { HoTen = o.ThongTinNhanVien.NhanVien.HoSo.Ten, SoHieuCongChuc=o.ThongTinNhanVien.SoHieuCongChuc, TenPhongBan = o.BoPhan.TenBoPhan, HinhThucNghi_Name = o.HinhThucNghi2.TenHinhThucNghi, Oid = o.Oid, IDBoPhan = o.IDBoPhan, IDNhanVien = o.IDNhanVien, IDHinhThucNghi = o.IDHinhThucNghi, TuNgay = o.TuNgay, DenNgay = o.DenNgay, NgayTao = o.NgayTao, DienGiai = o.DienGiai, IDWebUser = o.IDWebUser, TinhThanh = o.TinhThanh, TruNgayDiDuong = o.TruNgayDiDuong, SoNgayDiDuong = o.SoNgayDiDuong }).SingleOrDefault();
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
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);
            bool tatCaMaNhanSu = String.IsNullOrWhiteSpace(maNhanSu);
            //int xxxx= danhSachPhongBanPhanQuyen.Count();
            var result = (from o in this.ObjectSet

                          let tuNgayTruncate = DbFunctions.TruncateTime(o.TuNgay)
                          let denNgayTruncate = DbFunctions.TruncateTime(o.DenNgay)
                          where (
                                    ((denNgayTruncate >= dauThang && denNgayTruncate <= cuoiThang) || (tuNgayTruncate <= cuoiThang && tuNgayTruncate >= dauThang) || (tuNgayTruncate <= dauThang && denNgayTruncate >= cuoiThang))
                                        //(thang == 0 ? true : tuNgayTruncate.Value.Month <= thang && tuNgayTruncate.Value.Year <= nam)
                                        //&&
                                        // (thang == 0 ? true : denNgayTruncate.Value.Month >= thang && denNgayTruncate.Value.Year >= nam)
                                //o.TuNgay.Value.Month>=thang && o.TuNgay.Value.Year==nam
                                //&& o.DenNgay.Value.Month<=thang && o.DenNgay.Value.Year==nam

                                )
                                && danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.IDBoPhan)

                                && (tatCaMaNhanSu == true || o.ThongTinNhanVien.SoHieuCongChuc.Contains(maNhanSu))
                                && (idLoaiNhanSu == null || o.ThongTinNhanVien.LoaiNhanSu == idLoaiNhanSu)
                                //trưởng đơn vị chỉ lấy danh sách nhân viên (loại trưởng đơn vị)
                                //BGH lấy danh sách trưởng đơn vị
                                && ((webGroupId == new Guid("00000000-0000-0000-0000-000000000004") || webGroupId == new Guid("00000000-0000-0000-0000-000000000005")) ? 
                                o.ThongTinNhanVien.WebUsers.FirstOrDefault().Oid != webUserId : webGroupId == new Guid("00000000-0000-0000-0000-000000000002") ? 
                                o.ThongTinNhanVien.WebUsers.FirstOrDefault().WebGroupID == new Guid("00000000-0000-0000-0000-000000000004") : true)
                          orderby o.ThongTinNhanVien.NhanVien.HoSo.Ten, o.BoPhan.TenBoPhan, o.TuNgay
                          select new DTO_ChamCongNgayNghi_Find() {
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              SoHieuCongChuc = o.ThongTinNhanVien.SoHieuCongChuc,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              HinhThucNghi_Name = o.HinhThucNghi2.TenHinhThucNghi,
                              Oid = o.Oid,
                              IDBoPhan = o.IDBoPhan,
                              IDNhanVien = o.IDNhanVien,
                              IDHinhThucNghi = o.IDHinhThucNghi,
                              TuNgay = o.TuNgay,
                              DenNgay = (DbFunctions.TruncateTime(o.TuNgay) == DbFunctions.TruncateTime(o.DenNgay)) ? null : DbFunctions.TruncateTime(o.DenNgay),
                              CacBuoiTrongNgay_TuNgay = o.CacBuoiTrongNgay.TenBuoi,
                              CacBuoiTrongNgay_DenNgay = (DbFunctions.TruncateTime(o.TuNgay) == DbFunctions.TruncateTime(o.DenNgay)) ? "" : o.CacBuoiTrongNgay1.TenBuoi,
                              SoNgayDiDuong = o.TruNgayDiDuong.HasValue && o.TruNgayDiDuong.Value == true ? o.SoNgayDiDuong : null,
                              SoNgay = o.SoNgay,
                              SoNgayPhepConLai = o.SoNgayPhepConLai,
                              NgayTao = o.NgayTao,
                              DienGiai = o.DienGiai,
                              IDWebUser = o.IDWebUser,
                              LoaiNghiPhep = o.LoaiNghiPhep.ToString(),
                              TrangThai = o.TrangThai ?? -1, //nếu null thì hiện chờ xét
                              TrangThaiAdmin = o.TrangThaiAdmin ?? -1
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
