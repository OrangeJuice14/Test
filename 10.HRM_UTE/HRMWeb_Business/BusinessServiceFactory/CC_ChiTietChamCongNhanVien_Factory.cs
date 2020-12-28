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
    public class CC_ChiTietChamCongNhanVien_Factory : BaseFactory<Entities, CC_ChiTietChamCongNhanVien>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return CC_ChiTietChamCongNhanVien_Factory.New().CreateAloneObject();
        }
        public static CC_ChiTietChamCongNhanVien_Factory New()
        {
            return new CC_ChiTietChamCongNhanVien_Factory();
        }
        public CC_ChiTietChamCongNhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public bool CheckKhoa(int thang, int nam, Guid bophanId)
        {
            return this.ObjectSet.Any(x => x.BoPhan == bophanId && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam);
        }
        public bool CheckChot(int thang, int nam, Guid bophanId,int loaicanbo)
        {
            return this.ObjectSet.Any(x => x.BoPhan==bophanId && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang==thang && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam==nam && ( x.LoaiCanBo == loaicanbo || loaicanbo == 0));
        }
        public IQueryable<CC_ChiTietChamCongNhanVien> GetListByIdList(List<Guid> idList)
        {
            var result = from o in this.ObjectSet
                         where idList.Any(x => x == o.Oid)
                         select o;
            return result;
        }
        public IQueryable<CC_ChiTietChamCongNhanVien> GetByHoSoNhanVienID(Guid hoSoNhanVienId)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == hoSoNhanVienId
                          select o);
            return result;
        }

        public CC_QuanLyChamCongNhanVien GetByThangNam(int thang,int nam)
        {
            CC_QuanLyChamCongNhanVien result = (from o in this.Context.CC_QuanLyChamCongNhanVien
                                                where o.KyTinhLuong1.Thang == thang
                                                    && o.KyTinhLuong1.Nam == nam  select o).SingleOrDefault();
            return result;
        }

        public IQueryable<CC_ChiTietChamCongNhanVien> GetAll(Guid hoSoNhanVienId)
        {
            IQueryable<CC_ChiTietChamCongNhanVien> result = (from o in this.ObjectSet
                                                              select o);
            return result;
        }

        public IQueryable<CC_ChiTietChamCongNhanVien> GetByBoPhanId(Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.BoPhan == boPhanId
                          select o);
            return result;
        }

        public IQueryable<CC_ChiTietChamCongNhanVien> GetListByThangNamAndBoPhan(int thang, int nam, Guid boPhanId)
        {
            var result = (from o in this.ObjectSet
                          where o.BoPhan == boPhanId
                                && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang == thang
                                && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam == nam
                          select o);
            return result;
        }

        public CC_ChiTietChamCongNhanVien GetById(Guid id)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == id
                          select o).SingleOrDefault();
            return result;
        }

        public int ThongKeXetABCTheoThang_FindCount(int thang, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId)
        {

            var result = ThongKeXetABCTheoThang_Find(thang, nam, boPhanId, maNhanSu, webUserId).Count();
            return result;
        }

        public int ThongKeXetABCTheoNam_FindCount(int nam, Guid? boPhanId, string maNhanSu, Guid webUserId)
        {

            var result = ThongKeXetABCTheoNam_Find(nam, boPhanId, maNhanSu, webUserId).Count();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="boPhanId">null la tat ca</param>
        /// <param name="maNhanSu">null la tat ca</param>
        /// <returns></returns>
        public IQueryable<DTO_ThongKeXetABCTheoThang> ThongKeXetABCTheoThang_Find(int thang, int nam, Guid? boPhanId, string maNhanSu, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);
            var result = (from o in this.ObjectSet
                          where //(boPhanId == null || o.BoPhan == boPhanId)
                              //&& 
                                    danhSachPhongBanPhanQuyen.Any(x => x.Oid == o.BoPhan)
                                    && (o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Month == thang
                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                    )
                          && (tatCaMaNhanSu || o.ThongTinNhanVien1.SoHieuCongChuc == maNhanSu)
                          orderby o.ThongTinNhanVien1.NhanVien.BoPhan1.STT
                          select new DTO_ThongKeXetABCTheoThang()
                          {
                              MaNhanSu = o.ThongTinNhanVien1.SoHieuCongChuc
                              ,
                              HoVaTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen
                              ,
                              TenPhongBan = o.BoPhan1.TenBoPhan
                              ,
                              NgayCong = (o.SoNgayCong ?? 0)
                              ,
                              NghiPhep = (o.NghiCoPhep ?? 0)
                              ,
                              NghiRo = (o.NghiRo ?? 0)
                              ,
                              NghiThaiSan = (o.NghiThaiSan ?? 0)
                              ,
                              Loai = o.DanhGia
                              ,
                              GhiChu = o.DienGiai
                          });
            return result;
        }


        public IQueryable<DTO_ThongKeXetABCTheoThang> ThongKeXetABCTheoThang_Cua1NhanVien_Find(int thang, int nam, Guid nhanVienID)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var result = (from o in this.ObjectSet
                          where
                                     (o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Month == thang
                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                    )
                          && o.ThongTinNhanVien == nhanVienID
                          select new DTO_ThongKeXetABCTheoThang()
                          {
                              MaNhanSu = o.ThongTinNhanVien1.SoHieuCongChuc
                              ,
                              HoVaTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen
                              ,
                              TenPhongBan = o.BoPhan1.TenBoPhan
                              ,
                              NgayCong = (o.SoNgayCong ?? 0)
                              ,
                              NghiPhep = (o.NghiCoPhep ?? 0)
                              ,
                              NghiRo = (o.NghiRo ?? 0)
                              ,
                              NghiThaiSan = (o.NghiThaiSan ?? 0)
                              ,
                              Loai = o.DanhGia
                              ,
                              GhiChu = o.DienGiai
                          });
            return result;
        }


        public bool ExistsByThangNamBoPhanID(int thang, int nam, Guid boPhanID,int loaicanbo)
        {
            return this.ObjectSet.Any(x => (x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Thang ?? 0) == thang && (x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.Nam ?? 0) == nam
                                    && x.BoPhan == boPhanID && (loaicanbo == 0 || x.LoaiCanBo == loaicanbo) );
        }

        public IQueryable<DTO_ThongKeXetABCTheoThang> ThongKeXetABCTheoThang_CoPhanTrang(int thang, int nam, Guid? boPhanId, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId)
        {
            int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

            var result = ThongKeXetABCTheoThang_Find(thang, nam, boPhanId, maNhanSu, webUserId).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
            return result;
        }

        public IQueryable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_CoPhanTrang(int nam, Guid? boPhanId, string maNhanSu, int trang, int soMauTinMoiTrang, Guid webUserId)
        {
            int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

            var result = ThongKeXetABCTheoNam_Find(nam, boPhanId, maNhanSu, webUserId).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
            return result;
        }


        public IQueryable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Find(int nam, Guid? boPhanId, string maNhanSu, Guid webUserId)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId_GCRecordIsNull(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);

            Boolean tatCaMaNhanSu = (String.IsNullOrWhiteSpace(maNhanSu) ? true : false);

            var danhSachHoSoNhanVien = (from obj in this.ObjectSet
                                        where danhSachPhongBanPhanQuyen.Any(x => x.Oid == obj.BoPhan)
                                        && obj.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam

                                        && (tatCaMaNhanSu || obj.ThongTinNhanVien1.SoHieuCongChuc == maNhanSu)
                                        select new
                                        {
                                            HS = obj.ThongTinNhanVien1.NhanVien.HoSo
                                            ,
                                            TenBoPhan = obj.BoPhan1.TenBoPhan
                                        }
        ).Distinct();

            var result = (from o in danhSachHoSoNhanVien
                          select new DTO_ThongKeXetABCTheoNam()
                          {
                              MaNhanSu = o.HS.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              HoVaTen = o.HS.HoTen
                              ,
                              TenPhongBan = o.TenBoPhan
                              ,
                              LoaiA = (from x in this.Context.CC_ChiTietChamCongNhanVien
                                       where x.DanhGia == "A" && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                       && x.ThongTinNhanVien == o.HS.Oid
                                       select 1
                                       ).Count()
                              ,
                              LoaiB = (from x in this.Context.CC_ChiTietChamCongNhanVien
                                       where x.DanhGia == "B" && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                       && x.ThongTinNhanVien == o.HS.Oid
                                       select 1
                                      ).Count()
                          });
            return result;
        }





        public IQueryable<DTO_ThongKeXetABCTheoNam> ThongKeXetABCTheoNam_Cua1NhanVien_Find(int nam, Guid nhanVienID)
        {
            BoPhan_Factory tmpFactory = BoPhan_Factory.New();
            tmpFactory.Context = this.Context;
            //var danhSachPhongBanPhanQuyen = tmpFactory.GetDanhSachBoPhan_DuocPhanQuyenChoWebUserId(webUserId).Where(x => boPhanId == null || x.Oid == boPhanId);


            var danhSachHoSoNhanVien = (from obj in this.ObjectSet
                                        where
                                         obj.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam

                                        && obj.ThongTinNhanVien == nhanVienID
                                        select new
                                        {
                                            HS = obj.ThongTinNhanVien1.NhanVien.HoSo
                                            ,
                                            TenBoPhan = obj.BoPhan1.TenBoPhan
                                        }
        ).Distinct();

            var result = (from o in danhSachHoSoNhanVien
                          select new DTO_ThongKeXetABCTheoNam()
                          {
                              MaNhanSu = o.HS.NhanVien.ThongTinNhanVien.SoHieuCongChuc
                              ,
                              HoVaTen = o.HS.HoTen
                              ,
                              TenPhongBan = o.TenBoPhan
                              ,
                              LoaiA = (from x in this.Context.CC_ChiTietChamCongNhanVien
                                       where x.DanhGia == "A" && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                       && x.ThongTinNhanVien == o.HS.Oid
                                       select 1
                                       ).Count()
                              ,
                              LoaiB = (from x in this.Context.CC_ChiTietChamCongNhanVien
                                       where x.DanhGia == "B" && x.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                       && x.ThongTinNhanVien == o.HS.Oid
                                       select 1
                                      ).Count()                          
                          });
            return result;
        }




        public int QuanLyXetABC_FindCount(int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc)
        {

            var result = QuanLyXetABC_Find(thang, nam, boPhanId, idLoaiNhanSu, diHoc).Count();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thang">bat buoc</param>
        /// <param name="nam">bat buoc</param>
        /// <param name="boPhanId">bat buoc</param>
        /// <param name="idLoaiNhanSu">null la tat ca</param>
        /// <param name="diHoc">null la tat ca</param>
        /// <returns></returns>
        public IQueryable<CC_ChiTietChamCongNhanVien> QuanLyXetABC_Find(int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc)
        {
            bool tatcabophan = boPhanId == Guid.Empty ? true : false;
            var result = (from o in this.ObjectSet
                          where (tatcabophan ||o.BoPhan == boPhanId)
                                && (o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Month == thang
                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                    )

                                && (idLoaiNhanSu == Guid.Empty || o.ThongTinNhanVien1.LoaiNhanSu == idLoaiNhanSu)
                                && (diHoc == null || (diHoc.Value && o.ThongTinNhanVien1.NhanVien.TinhTrang1.TenTinhTrang.ToLower().Contains("học")))
                          select o);
            return result;
        }
        public List<DTO_ChiTietChamCongNhanVien> QuanLyXetABC_XuatExcel(int thang, int nam, Guid boPhanId)
        {
            bool tatCaBoPhan = boPhanId == Guid.Empty ? true : false;
            var result = (from o in this.ObjectSet
                          where (tatCaBoPhan || o.BoPhan == boPhanId)
                                && (o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Month == thang
                                    && o.CC_QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
                                    )
                                    && o.SoNgayCong<22
                                    orderby o.BoPhan1.STT
                          select new DTO_ChiTietChamCongNhanVien()
                          {
                              HoVaTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
                              TenPhongBan=o.ThongTinNhanVien1.NhanVien.BoPhan1.TenBoPhan,
                              NghiRo=o.NghiRo.ToString(),
                              NghiThaiSan = o.NghiThaiSan.ToString(),
                              NghiCoPhep = o.NghiCoPhep.ToString(),
                              NghiDiHocCoLuong = o.NghiDiHocCoLuong.ToString(),
                              NghiDiHocKhongLuong = o.NghiDiHocKhongLuong.ToString(),
                              NghiOm = o.NghiOm.ToString(),
                              SoNgayCong=o.SoNgayCong.ToString(),
                              DanhGia=o.DanhGia
                          }).ToList();
            return result;
        }
        //public IEnumerable<DTO_ChiTietChamCongNhanVien> QuanLyXetABC_Find(int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc)
        //{
        //    var result = (from o in this.ObjectSet
        //                  where o.BoPhan == boPhanId
        //                        && (o.QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Month == thang
        //                            && o.QuanLyChamCongNhanVien1.KyTinhLuong1.TuNgay.Value.Year == nam
        //                            )

        //                        && (idLoaiNhanSu == null || o.ThongTinNhanVien1.LoaiNhanSu == idLoaiNhanSu)
        //                        && (diHoc == null || (diHoc.Value && o.ThongTinNhanVien1.NhanVien.TinhTrang1.TenTinhTrang.ToLower().Contains("học")))
        //                  select new DTO_ChiTietChamCongNhanVien()
        //                  {
        //                    Oid = o.Oid,
        //                    ThongTinNhanVien = o.ThongTinNhanVien1.Oid,
        //                    SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
        //                    HoVaTen = o.ThongTinNhanVien1.NhanVien.HoSo.HoTen,
        //                    DanhGia = o.DanhGia,
        //                    DanhGiaTruocDieuChinh = o.DanhGiaTruocDieuChinh,
        //                    SoNgayCong = o.SoNgayCong.ToString(),
        //                    DienGiai = o.DienGiai,
        //                    TrangThai = o.TrangThai,
        //                    Khoa = o.Khoa
        //                    }).ToList();
        //    return result;
        //}

        //public IQueryable<ChiTietChamCongNhanVien> QuanLyXetABCFind_CoPhanTrang(int thang, int nam, Guid boPhanId, Guid? idLoaiNhanSu, bool? diHoc, int trang, int soMauTinMoiTrang)
        //{
        //    int soMauTinBoQua = (trang - 1) * soMauTinMoiTrang;

        //    var result = QuanLyXetABC_Find(thang, nam, boPhanId, idLoaiNhanSu, diHoc).Skip(soMauTinBoQua).Take(soMauTinMoiTrang);
        //    return result;
        //}



        public static void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (CC_ChiTietChamCongNhanVien item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
