using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP_Core;
using System.Reflection;
using System.ComponentModel;
using System.Data.Linq;
using System.Data;
using System.Data.Entity.Core.Objects;
using ERP_Business;
using HRMWeb_Business.Model;
using HRMWeb_Business.Model.Context;
using System.Data.Entity;

namespace HRMWeb_Business.BusinessServiceFactory
{
    public class QuanLyChamCongNgoaiGio_Factory : BaseFactory<Entities, BangChamCongNgoaiGio>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return QuanLyChamCongNgoaiGio_Factory.New().CreateAloneObject();
        }
        public static QuanLyChamCongNgoaiGio_Factory New()
        {
            return new QuanLyChamCongNgoaiGio_Factory();
        }
        public QuanLyChamCongNgoaiGio_Factory()
            : base(Database.NewEntities())
        {

        }
        public bool ExistsByThangNamBoPhanID(int thang, int nam, Guid boPhanID)
        {
            return this.ObjectSet.Any(x => (x.KyTinhLuong1.Thang ?? 0) == thang && (x.KyTinhLuong1.Nam ?? 0) == nam
                                    && x.ChiTietChamCongNgoaiGios.Any(y => y.BoPhan == boPhanID)
                            );
        }
        public bool ExistsByKyTinhLuongBoPhanID(Guid kyTinhLuong)
        {
            return this.ObjectSet.Any(x => x.KyTinhLuong==kyTinhLuong);
        }
        #region Custom
        public IQueryable<DTO_QuanLyChamCongNgoaiGio_Find> QuanLyChamCongNgoaiGio_Find(Guid? boPhanId,Guid kytinhluong)
        {
            bool allDept = boPhanId == null ? true : false;
            var result = (from o in this.Context.ChiTietChamCongNgoaiGios
                          where (allDept || o.BoPhan == boPhanId)
                                && o.BangChamCongNgoaiGio1.KyTinhLuong == kytinhluong
                          orderby o.BoPhan1.TenBoPhan, o.ThongTinNhanVien1.NhanVien.HoSo.Ten
                          select new DTO_QuanLyChamCongNgoaiGio_Find() {
                              Oid = o.Oid,
                              KyTinhLuong=o.BangChamCongNgoaiGio1.KyTinhLuong??Guid.Empty,
                              HoTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                              MaNhanSu = o.ThongTinNhanVien1.NhanVien.HoSo.MaQuanLy,
                              TenPhongBan = o.BoPhan1.TenBoPhan,
                              SoCongNgoaiGio = o.SoCongNgoaiGio.ToString(),
                              SoCongNgoaiGioSau23Gio = o.SoCongNgoaiGioSau23Gio.ToString(),
                              SoCongNgoaiGioT7CN = o.SoCongNgoaiGio1.ToString(),
                              SoCongNgoaiGioT7CNSau23Gio = o.SoCongNgoaiGio1Sau23Gio.ToString(),
                              SoCongNgoaiGioLe = o.SoCongNgoaiGio2.ToString(),
                              SoCongNgoaiGioLeSau23Gio = o.SoCongNgoaiGio2Sau23Gio.ToString()
                          });
            return result;
        }
        public IQueryable<DTO_QuanLyChamCongNgoaiGio_Find> ChamCongNgoaiGioTheoNgay_Find(Guid? boPhanId, DateTime fromDate, DateTime toDate)
        {
            bool allDept = boPhanId == null ? true : false;
            var result = (from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                          where (allDept || o.IDBoPhan == boPhanId)
                                && (DbFunctions.TruncateTime(o.Ngay) >= fromDate.Date && DbFunctions.TruncateTime(o.Ngay) <= toDate.Date)
                          orderby o.BoPhan.TenBoPhan, o.ThongTinNhanVien.NhanVien.HoSo.Ten
                          select new DTO_QuanLyChamCongNgoaiGio_Find()
                          {
                              Oid = o.Oid,
                              Ngay = o.Ngay,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              MaNhanSu = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              SoCongNgoaiGio = o.SoCongNgoaiGio.ToString(),
                              SoCongNgoaiGioSau23Gio = o.SoCongNgoaiGioSau23Gio.ToString(),
                              SoCongNgoaiGioT7CN = o.SoCongNgoaiGioT7CN.ToString(),
                              SoCongNgoaiGioT7CNSau23Gio = o.SoCongNgoaiGioT7CNSau23Gio.ToString(),
                              SoCongNgoaiGioLe = o.SoCongNgoaiGioLe.ToString(),
                              SoCongNgoaiGioLeSau23Gio = o.SoCongNgoaiGioLeSau23Gio.ToString(),
                              TuGio = o.TuGio.ToString(),
                              DenGio = o.DenGio.ToString(),
                              DaChinhSua = 0
                          });
            return result;
        }
        public IQueryable<DTO_QuanLyChamCongNgoaiGio_Find> ChamCongNgoaiGioTheoNgayDonVi_Find(Guid? boPhanId, DateTime? fromDate, DateTime? toDate)
        {
            //Lấy giá trị ban đầu của đơn vị chấm đã bị thay đổi
            var CC_TheoNgayNgoaiGioThayDoi = (from cc in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                      join td in this.Context.CC_ChamCongNgoaiGioTheoNgayThayDoi on cc.Oid equals td.Oid
                                              where cc.IDBoPhan == boPhanId
                                         && (EntityFunctions.TruncateTime(cc.Ngay) >= fromDate.Value.Date && EntityFunctions.TruncateTime(cc.Ngay) <= toDate.Value.Date)
                                              select new DTO_QuanLyChamCongNgoaiGio_Find()
                                      {
                                          Oid = cc.Oid,
                                          Ngay = cc.Ngay,
                                          HoTen = cc.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                                          MaNhanSu = cc.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                                          TenPhongBan = cc.BoPhan.TenBoPhan,
                                          SoCongNgoaiGio = td.SoCongNgoaiGioDonVi.ToString(),
                                          SoCongNgoaiGioSau23Gio = td.SoCongNgoaiGioSau23GioDonVi.ToString(),
                                          SoCongNgoaiGioT7CN = td.SoCongNgoaiGioT7CNDonVi.ToString(),
                                          SoCongNgoaiGioT7CNSau23Gio = td.SoCongNgoaiGioT7CNSau23GioDonVi.ToString(),
                                          SoCongNgoaiGioLe = td.SoCongNgoaiGioLeDonVi.ToString(),
                                          SoCongNgoaiGioLeSau23Gio = td.SoCongNgoaiGioLeSau23GioDonVi.ToString(),
                                          DaChinhSua = 0
                                      });
            List<Guid> thayDoiList = new List<Guid>();
            thayDoiList = CC_TheoNgayNgoaiGioThayDoi.Select(c => c.Oid).ToList();
            //Lấy hiện tại chưa bị thay đổi
            var CC_ChamCongTheoNgayNgoaiGio = (from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                          where o.IDBoPhan == boPhanId
                          && !thayDoiList.Contains(o.Oid)
                                && (EntityFunctions.TruncateTime(o.Ngay) >= fromDate.Value.Date && EntityFunctions.TruncateTime(o.Ngay) <= toDate.Value.Date)
                          orderby o.BoPhan.TenBoPhan, o.ThongTinNhanVien.NhanVien.HoSo.Ten
                          select new DTO_QuanLyChamCongNgoaiGio_Find()
                          {
                              Oid = o.Oid,
                              Ngay = o.Ngay,
                              HoTen = o.ThongTinNhanVien.NhanVien.HoSo.HoTen,
                              MaNhanSu = o.ThongTinNhanVien.NhanVien.HoSo.MaQuanLy,
                              TenPhongBan = o.BoPhan.TenBoPhan,
                              SoCongNgoaiGio = o.SoCongNgoaiGio.ToString(),
                              SoCongNgoaiGioSau23Gio = o.SoCongNgoaiGioSau23Gio.ToString(),
                              SoCongNgoaiGioT7CN = o.SoCongNgoaiGioT7CN.ToString(),
                              SoCongNgoaiGioT7CNSau23Gio = o.SoCongNgoaiGioT7CNSau23Gio.ToString(),
                              SoCongNgoaiGioLe = o.SoCongNgoaiGioLe.ToString(),
                              SoCongNgoaiGioLeSau23Gio = o.SoCongNgoaiGioLeSau23Gio.ToString(),
                              TuGio = o.TuGio.ToString(),
                              DenGio = o.DenGio.ToString(),
                              DaChinhSua = 0
                          });
            var result = CC_ChamCongTheoNgayNgoaiGio.Union(CC_TheoNgayNgoaiGioThayDoi);
            return result;
        }
        public bool CheckExist(Guid kytinhluong)
        {
            return this.ObjectSet.Any(x => x.KyTinhLuong==kytinhluong);
        }
        public BangChamCongNgoaiGio GetByKyTinhLuong(Guid kytinhluong)
        {
            return this.ObjectSet.Where(b => b.KyTinhLuong == kytinhluong).SingleOrDefault();
        }
        public BangChamCongNgoaiGio GetByThangNam(int thang, int nam)
        {
            return this.ObjectSet.Where(x => (x.KyTinhLuong1.Thang ?? 0) == thang && (x.KyTinhLuong1.Nam ?? 0) == nam).SingleOrDefault();
        }
        public IQueryable<DTO_QuanLyChamCongNgoaiGio_ChamCongThang> QuanLyChamCongNgoaiGio_ChamCongThang(Guid PhongBan, Guid KyTinhLuong)
        {
            var kyTinhLuong = (from o in this.Context.KyTinhLuongs where o.Oid== KyTinhLuong select o).SingleOrDefault();
            DateTime? tuNgay = kyTinhLuong.TuNgay;
            DateTime? denNgay = kyTinhLuong.DenNgay;
            //lay danh sach nhan vien
            var dsIdNhanVien = (from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                && (o.IDBoPhan == PhongBan || PhongBan == Guid.Empty)
                                && (o.ThongTinNhanVien.NhanVien.TinhTrang1.KhongConCongTacTaiTruong ?? false) == false
                                select o.IDNhanVien).Distinct();
            var result = (from idNv in dsIdNhanVien
                          join hs in this.Context.HoSoes on idNv equals hs.Oid
                          orderby hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu != null ? hs.NhanVien.ThongTinNhanVien.ChucVu1.ThuTu : 9999, hs.Ten
                          select new DTO_QuanLyChamCongNgoaiGio_ChamCongThang
                          {

                              HoTen = hs.HoTen
                              ,
                              MaNhanSu = hs.MaQuanLy
                              ,
                              TenPhongBan = hs.NhanVien.BoPhan1.TenBoPhan
                              ,
                              SoCongNgoaiGio = (decimal?)(from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                                where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                  && o.IDNhanVien == idNv
                                                select (o.SoCongNgoaiGio ?? (decimal?)0)).Sum(),
                              SoCongNgoaiGioSau23Gio = (decimal?)(from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                            && o.IDNhanVien == idNv
                                                          select (o.SoCongNgoaiGioSau23Gio ?? (decimal?)0)).Sum(),
                              SoCongNgoaiGioT7CN = (decimal?)(from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                            && o.IDNhanVien == idNv
                                                          select (o.SoCongNgoaiGioT7CN ?? (decimal?)0)).Sum(),
                              SoCongNgoaiGioT7CNSau23Gio = (decimal?)(from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                            && o.IDNhanVien == idNv
                                                          select (o.SoCongNgoaiGioT7CNSau23Gio ?? (decimal?)0)).Sum(),
                              SoCongNgoaiGioLe = (decimal?)(from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                            && o.IDNhanVien == idNv
                                                          select (o.SoCongNgoaiGioLe ?? (decimal?)0)).Sum(),
                              SoCongNgoaiGioLeSau23Gio = (decimal?)(from o in this.Context.CC_ChamCongNgoaiGioTheoNgay
                                                          where (o.Ngay >= tuNgay && o.Ngay <= denNgay)
                                                                            && o.IDNhanVien == idNv
                                                          select (o.SoCongNgoaiGioLeSau23Gio ?? (decimal?)0)).Sum()
                          });         
            return result;
        }
        #endregion
    }//end class
}
