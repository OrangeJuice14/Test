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
    public class Web_UpdateHoSoNhanVien_Factory : BaseFactory<Entities, Web_UpdateHoSoNhanVien>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return Web_UpdateHoSoNhanVien_Factory.New().CreateAloneObject();
        }
        public static Web_UpdateHoSoNhanVien_Factory New()
        {
            return new Web_UpdateHoSoNhanVien_Factory();
        }
        public Web_UpdateHoSoNhanVien_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public DTO_Web_UpdateHoSoNhanVien GetHoSoNhanVien_InHRM_ByOidNhanVien(Guid idnhanvien)
        {

            var result = (from o in this.Context.ThongTinNhanViens
                          join x in this.Context.HoSoes on o.Oid equals x.Oid
                          join y in this.Context.NhanViens on o.Oid equals y.Oid
                          where o.Oid == idnhanvien
                                && x.GCRecord == null
                          select new DTO_Web_UpdateHoSoNhanVien()
                          {
                              ThongTinNhanVien = o.Oid,
                              SoHieuCongChuc = o.SoHieuCongChuc,
                              Ho = x.Ho,
                              Ten = x.Ten,
                              TenGoiKhac = x.TenGoiKhac,
                              TenBoPhan = y.BoPhan1.TenBoPhan,
                              NgaySinh = x.NgaySinh,
                              NoiSinh = (from d in this.Context.DiaChis
                                         where x.DiaChi2.Oid == d.Oid
                                               && d.GCRecord == null
                                         select new DTO_DiaChi
                                         {
                                             Oid = d.Oid,
                                             QuocGia = d.QuocGia.Value,
                                             TinhThanh = d.TinhThanh,
                                             QuanHuyen = d.QuanHuyen,
                                             XaPhuong = d.XaPhuong,
                                             SoNha = d.SoNha
                                         }).FirstOrDefault(),
                              GioiTinh = x.GioiTinh == 0 ? "Nam" : "Nữ",
                              CMND = x.CMND,
                              NgayCap = x.NgayCap,
                              NoiCap = x.NoiCap,
                              QueQuan = (from d in this.Context.DiaChis
                                         where x.DiaChi3.Oid == d.Oid
                                               && d.GCRecord == null
                                         select new DTO_DiaChi
                                         {
                                             Oid = d.Oid,
                                             QuocGia = d.QuocGia.Value,
                                             TinhThanh = d.TinhThanh,
                                             QuanHuyen = d.QuanHuyen,
                                             XaPhuong = d.XaPhuong,
                                             SoNha = d.SoNha
                                         }).FirstOrDefault(),
                              DiaChiThuongTru = (from d in this.Context.DiaChis
                                                 where x.DiaChi.Oid == d.Oid
                                                       && d.GCRecord == null
                                                 select new DTO_DiaChi
                                                 {
                                                     Oid = d.Oid,
                                                     QuocGia = d.QuocGia.Value,
                                                     TinhThanh = d.TinhThanh,
                                                     QuanHuyen = d.QuanHuyen,
                                                     XaPhuong = d.XaPhuong,
                                                     SoNha = d.SoNha
                                                 }).FirstOrDefault(),
                              NoiOHienNay = (from d in this.Context.DiaChis
                                             where x.DiaChi1.Oid == d.Oid
                                                    && d.GCRecord == null
                                             select new DTO_DiaChi
                                             {
                                                 Oid = d.Oid,
                                                 QuocGia = d.QuocGia.Value,
                                                 TinhThanh = d.TinhThanh,
                                                 QuanHuyen = d.QuanHuyen,
                                                 XaPhuong = d.XaPhuong,
                                                 SoNha = d.SoNha
                                             }).FirstOrDefault(),
                              Email = x.Email,
                              DienThoaiDiDong = x.DienThoaiDiDong,
                              TinhTrangHonNhan = x.TinhTrangHonNhan,
                              DanToc = x.DanToc,
                              TonGiao = x.TonGiao,
                              TrinhDoVanHoa = y.NhanVienTrinhDo1.TrinhDoVanHoa,
                              TrinhDoChuyenMon = y.NhanVienTrinhDo1.TrinhDoChuyenMon,
                              ChuyenNganhDaoTao = y.NhanVienTrinhDo1.ChuyenMonDaoTao,
                              TruongDaoTao = y.NhanVienTrinhDo1.TruongDaoTao,
                              NamTotNghiep = y.NhanVienTrinhDo1.NamTotNghiep.Value,
                              NgoaiNgu = y.NhanVienTrinhDo1.NgoaiNgu,
                              TrinhDoNgoaiNgu = y.NhanVienTrinhDo1.TrinhDoNgoaiNgu,
                              NgayCapNhat = DateTime.MinValue
                          }).OrderByDescending(x => x.NgayCapNhat).SingleOrDefault();
            return result;
        }
        public DTO_Web_UpdateHoSoNhanVien GetHoSoNhanVienByOidNhanVien(Guid idnhanvien)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == idnhanvien
                                && (o.DaCapNhatHRM ?? false) == false
                          select new DTO_Web_UpdateHoSoNhanVien()
                          {
                              ThongTinNhanVien = o.ThongTinNhanVien.Value,
                              SoHieuCongChuc = o.SoHieuCongChuc,
                              Ho = o.Ho,
                              Ten = o.Ten,
                              TenGoiKhac = o.TenGoiKhac,
                              TenBoPhan = o.ThongTinNhanVien1.BoPhan.TenBoPhan,
                              NgaySinh = o.NgaySinh,
                              NoiSinh= (from d in this.Context.DiaChis
                                        where d.Oid == o.DiaChi2.Oid
                                               && d.GCRecord == null
                                        select new DTO_DiaChi {
                                                                  Oid = d.Oid,
                                                                  QuocGia = d.QuocGia.Value,
                                                                  TinhThanh = d.TinhThanh,
                                                                  QuanHuyen = d.QuanHuyen,
                                                                  XaPhuong = d.XaPhuong,
                                                                  SoNha = d.SoNha}).FirstOrDefault(),
                              GioiTinh = o.GioiTinh == false ? "Nam" : "Nữ",
                              CMND = o.CMND,
                              NgayCap = o.NgayCap,
                              NoiCap = o.NoiCap,
                              NoiCapText = o.TinhThanh.TenTinhThanh,
                              QueQuan = (from d in this.Context.DiaChis
                                         where d.Oid == o.DiaChi3.Oid
                                               && d.GCRecord == null
                                         select new DTO_DiaChi
                                         {
                                             Oid = d.Oid,
                                             QuocGia = d.QuocGia.Value,
                                             TinhThanh = d.TinhThanh,
                                             QuanHuyen = d.QuanHuyen,
                                             XaPhuong = d.XaPhuong,
                                             SoNha = d.SoNha
                                         }).FirstOrDefault(),
                              DiaChiThuongTru= (from d in this.Context.DiaChis
                                                where d.Oid == o.DiaChi.Oid
                                                      && d.GCRecord == null
                                                select new DTO_DiaChi
                                                {
                                                    Oid = d.Oid,
                                                    QuocGia = d.QuocGia.Value,
                                                    TinhThanh = d.TinhThanh,
                                                    QuanHuyen = d.QuanHuyen,
                                                    XaPhuong = d.XaPhuong,
                                                    SoNha = d.SoNha
                                                }).FirstOrDefault(),
                              NoiOHienNay = (from d in this.Context.DiaChis
                                             where d.Oid == o.DiaChi1.Oid
                                                   && d.GCRecord == null
                                             select new DTO_DiaChi
                                             {
                                                 Oid = d.Oid,
                                                 QuocGia = d.QuocGia.Value,
                                                 TinhThanh = d.TinhThanh,
                                                 QuanHuyen = d.QuanHuyen,
                                                 XaPhuong = d.XaPhuong,
                                                 SoNha = d.SoNha
                                             }).FirstOrDefault(),
                              Email = o.Email,
                              DienThoaiDiDong = o.DienThoaiDiDong,
                              TinhTrangHonNhan = o.TinhTrangHonNhan,
                              TenTinhTrangHonNhan = o.TinhTrangHonNhan1.TenTinhTrangHonNhan,
                              DanToc = o.DanToc,
                              TenDanToc = o.DanToc1.TenDanToc,
                              TonGiao = o.TonGiao,
                              TenTonGiao = o.TonGiao1.TenTonGiao,
                              TrinhDoVanHoa = o.TrinhDoVanHoa,
                              TenTrinhDoVanHoa = o.TrinhDoVanHoa1.TenTrinhDoVanHoa,
                              TrinhDoChuyenMon = o.TrinhDoChuyenMon,
                              TenTrinhDoChuyenMon = o.TrinhDoChuyenMon1.TenTrinhDoChuyenMon,
                              ChuyenNganhDaoTao = o.ChuyenNganhDaoTao,
                              TenChuyenNganhDaoTao = o.ChuyenMonDaoTao.TenChuyenMonDaoTao,
                              TruongDaoTao = o.TruongDaoTao,
                              TenTruongDaoTao = o.TruongDaoTao1.TenTruongDaoTao,
                              NamTotNghiep = o.NamTotNghiep,
                              NgoaiNgu = o.NgoaiNgu,
                              TenNgoaiNgu = o.NgoaiNgu1.TenNgoaiNgu,
                              TrinhDoNgoaiNgu = o.TrinhDoNgoaiNgu,
                              TenTrinhDoNgoaiNgu = o.TrinhDoNgoaiNgu1.TenTrinhDoNgoaiNgu,
                              NgayCapNhat = o.NgayCapNhat
                          }).OrderByDescending(x => x.NgayCapNhat).SingleOrDefault();
            //
            if (result == null)
            {
                result = GetHoSoNhanVien_InHRM_ByOidNhanVien(idnhanvien);
            }
            else
            {
                var item = GetHoSoNhanVien_InHRM_ByOidNhanVien(idnhanvien);
                if (item != null)
                {
                    if (string.IsNullOrEmpty(result.SoHieuCongChuc))
                        result.SoHieuCongChuc = item.SoHieuCongChuc.Trim();
                    if (string.IsNullOrEmpty(result.Ho))
                        result.Ho = item.Ho.Trim();
                    if (string.IsNullOrEmpty(result.Ten))
                        result.Ten = item.Ten.Trim();
                    if (string.IsNullOrEmpty(result.TenGoiKhac))
                        result.TenGoiKhac = item.TenGoiKhac.Trim();
                    if (result.NgaySinh == null || result.NgaySinh == DateTime.MinValue)
                        result.NgaySinh = item.NgaySinh;
                    if (string.IsNullOrEmpty(result.GioiTinh))
                        result.GioiTinh = item.GioiTinh.Trim();
                    if (string.IsNullOrEmpty(result.Email))
                        result.Email = item.Email.Trim();
                    if (string.IsNullOrEmpty(result.DienThoaiDiDong))
                        result.DienThoaiDiDong = item.DienThoaiDiDong.Trim();
                    if (result.DanToc == null)
                        result.DanToc = item.DanToc;
                    if (result.TonGiao == null)
                        result.TonGiao = item.TonGiao;
                    if (result.NoiSinh == null)
                        result.NoiSinh = item.NoiSinh;
                    if (result.DiaChiThuongTru == null)
                        result.DiaChiThuongTru = item.DiaChiThuongTru;
                    if (result.NoiOHienNay == null)
                        result.NoiOHienNay = item.NoiOHienNay;
                    if (result.QueQuan == null)
                        result.QueQuan = item.QueQuan;
                    if (string.IsNullOrEmpty(result.CMND))
                        result.CMND = item.CMND.Trim();
                    if (result.NgayCap == null || result.NgayCap == DateTime.MinValue)
                        result.NgayCap = item.NgayCap;
                    if (result.NoiCap == null)
                        result.NoiCap = item.NoiCap;
                    if (result.TinhTrangHonNhan == null)
                        result.TinhTrangHonNhan = item.TinhTrangHonNhan;
                    if (result.TrinhDoVanHoa == null)
                        result.TrinhDoVanHoa = item.TrinhDoVanHoa;
                    if (result.TrinhDoChuyenMon == null)
                        result.TrinhDoChuyenMon = item.TrinhDoChuyenMon;
                    if (result.ChuyenNganhDaoTao == null)
                        result.ChuyenNganhDaoTao = item.ChuyenNganhDaoTao;
                    if (result.TruongDaoTao == null)
                        result.TruongDaoTao = item.TruongDaoTao;
                    if (result.NamTotNghiep == 0)
                        result.NamTotNghiep = item.NamTotNghiep;
                    if (result.NgoaiNgu == null)
                        result.NgoaiNgu = item.NgoaiNgu;
                    if (result.TrinhDoNgoaiNgu == null)
                        result.TrinhDoNgoaiNgu = item.TrinhDoNgoaiNgu;
                }
            }

            return result;
        }

        public Web_UpdateHoSoNhanVien GetHoSoNhanVienByOidNV(Guid idnhanvien)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == idnhanvien
                                && (o.DaCapNhatHRM ?? false) == false
                          select o).OrderByDescending(x => x.NgayCapNhat).SingleOrDefault();

            return result;
        }
        public IQueryable<DTO_Web_UpdateHoSoNhanVien> GetListByBoPhan(Guid idBoPhan)
        {
            var result = (from o in this.ObjectSet
                         where o.ThongTinNhanVien1.BoPhan.Oid == idBoPhan || idBoPhan == Guid.Empty
                               && o.DaCapNhatHRM == false
                         select new DTO_Web_UpdateHoSoNhanVien()
                         {
                             Oid = o.Oid,
                             SoHieuCongChuc = o.ThongTinNhanVien1.SoHieuCongChuc,
                             Ho = o.Ho,
                             Ten = o.Ten,
                             TenGoiKhac = o.TenGoiKhac,
                             TenBoPhan = o.ThongTinNhanVien1.BoPhan.TenBoPhan,
                             NgaySinh = o.NgaySinh,
                             NoiSinhFull = o.DiaChi2.FullDiaChi,
                             GioiTinh = o.GioiTinh == false ? "Nam" : "Nữ",
                             CMND = o.CMND,
                             NgayCap = o.NgayCap,
                             NoiCap = o.NoiCap,
                             NoiCapText = o.TinhThanh.TenTinhThanh,
                             QueQuanFull = o.DiaChi3.FullDiaChi,
                             DiaChiThuongTruFull = o.DiaChi.FullDiaChi,
                             NoiOHienNayFull = o.DiaChi1.FullDiaChi,
                             Email = o.Email,
                             DienThoaiDiDong = o.DienThoaiDiDong,
                             TinhTrangHonNhan = o.TinhTrangHonNhan,
                             TenTinhTrangHonNhan = o.TinhTrangHonNhan1.TenTinhTrangHonNhan,
                             DanToc = o.DanToc,
                             TenDanToc = o.DanToc1.TenDanToc,
                             TonGiao = o.TonGiao,
                             TenTonGiao = o.TonGiao1.TenTonGiao,
                             TrinhDoVanHoa = o.TrinhDoVanHoa,
                             TenTrinhDoVanHoa = o.TrinhDoVanHoa1.TenTrinhDoVanHoa,
                             TrinhDoChuyenMon = o.TrinhDoChuyenMon,
                             TenTrinhDoChuyenMon = o.TrinhDoChuyenMon1.TenTrinhDoChuyenMon,
                             ChuyenNganhDaoTao = o.ChuyenNganhDaoTao,
                             TenChuyenNganhDaoTao = o.ChuyenMonDaoTao.TenChuyenMonDaoTao,
                             TruongDaoTao = o.TruongDaoTao,
                             TenTruongDaoTao = o.TruongDaoTao1.TenTruongDaoTao,
                             NamTotNghiep = o.NamTotNghiep,
                             NgoaiNgu = o.NgoaiNgu,
                             TenNgoaiNgu = o.NgoaiNgu1.TenNgoaiNgu,
                             TrinhDoNgoaiNgu = o.TrinhDoNgoaiNgu,
                             TenTrinhDoNgoaiNgu = o.TrinhDoNgoaiNgu1.TenTrinhDoNgoaiNgu,
                             NgayCapNhat = o.NgayCapNhat
                         }).OrderByDescending(x=>x.NgayCapNhat);
            return result;
        }
        public bool GetWeb_UpdateHoSoNhanVienByOIdNhanVien(Guid oidNhanVien)
        {
            var result = (from o in this.ObjectSet
                          where o.ThongTinNhanVien == oidNhanVien
                                && (o.DaCapNhatHRM ?? false) == false
                          select true).SingleOrDefault();
            return result;
        }
        public DTO_Web_UpdateHoSoNhanVien GetListByOId(Guid oidHoSoNhanVien)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oidHoSoNhanVien
                          select new DTO_Web_UpdateHoSoNhanVien()
                          {
                              Oid = o.Oid,
                              SoHieuCongChuc = o.SoHieuCongChuc,
                              Ho = o.Ho,
                              Ten = o.Ten,
                              TenGoiKhac = o.TenGoiKhac,
                              TenBoPhan = o.ThongTinNhanVien1.BoPhan.TenBoPhan,
                              NgaySinh = o.NgaySinh,
                              NoiSinhFull = o.DiaChi2.FullDiaChi,
                              GioiTinh = o.GioiTinh == false ? "Nam" : "Nữ",
                              CMND = o.CMND,
                              NgayCap = o.NgayCap,
                              NoiCap = o.NoiCap,
                              NoiCapText = o.TinhThanh.TenTinhThanh,
                              QueQuanFull = o.DiaChi3.FullDiaChi,
                              DiaChiThuongTruFull = o.DiaChi.FullDiaChi,
                              NoiOHienNayFull = o.DiaChi1.FullDiaChi,
                              Email = o.Email,
                              DienThoaiDiDong = o.DienThoaiDiDong,
                              TinhTrangHonNhan = o.TinhTrangHonNhan,
                              TenTinhTrangHonNhan = o.TinhTrangHonNhan1.TenTinhTrangHonNhan,
                              DanToc = o.DanToc,
                              TenDanToc = o.DanToc1.TenDanToc,
                              TonGiao = o.TonGiao,
                              TenTonGiao = o.TonGiao1.TenTonGiao,
                              TrinhDoVanHoa = o.TrinhDoVanHoa,
                              TenTrinhDoVanHoa = o.TrinhDoVanHoa1.TenTrinhDoVanHoa,
                              TrinhDoChuyenMon = o.TrinhDoChuyenMon,
                              TenTrinhDoChuyenMon = o.TrinhDoChuyenMon1.TenTrinhDoChuyenMon,
                              ChuyenNganhDaoTao = o.ChuyenNganhDaoTao,
                              TenChuyenNganhDaoTao = o.ChuyenMonDaoTao.TenChuyenMonDaoTao,
                              TruongDaoTao = o.TruongDaoTao,
                              TenTruongDaoTao = o.TruongDaoTao1.TenTruongDaoTao,
                              NamTotNghiep = o.NamTotNghiep,
                              NgoaiNgu = o.NgoaiNgu,
                              TenNgoaiNgu = o.NgoaiNgu1.TenNgoaiNgu,
                              TrinhDoNgoaiNgu = o.TrinhDoNgoaiNgu,
                              TenTrinhDoNgoaiNgu = o.TrinhDoNgoaiNgu1.TenTrinhDoNgoaiNgu,
                              NgayCapNhat = o.NgayCapNhat
                          }).OrderByDescending(x => x.NgayCapNhat).SingleOrDefault();
            return result;
        }

        public List<DTO_DanToc> GetListDanTocALL()
        {
            var result = (from o in this.Context.DanTocs.Where(x=>x.GCRecord == null)
                         select new DTO_DanToc { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenDanToc = o.TenDanToc}).ToList();
            //
            return result;
        }
        public List<DTO_TonGiao> GetListTonGiaoALL()
        {
            var result = (from o in this.Context.TonGiaos.Where(x => x.GCRecord == null)
                         select new DTO_TonGiao { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTonGiao = o.TenTonGiao }).ToList();
            //
            return result;
        }
        public List<DTO_TinhTrangHonNhan> GetListTinhTrangHonNhanALL()
        {
            var result = (from o in this.Context.TinhTrangHonNhans.Where(x => x.GCRecord == null)
                          select new DTO_TinhTrangHonNhan { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTinhTrangHonNhan = o.TenTinhTrangHonNhan }).ToList();
            //
            return result;
        }
        public List<DTO_QuocGia> GetListQuocGiaALL()
        {
            var result = (from o in this.Context.QuocGias.Where(x => x.GCRecord == null)
                          select new DTO_QuocGia { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuocGia = o.TenQuocGia }).ToList();
            //
            return result;
        }
        public List<DTO_TinhThanh> GetListTinhThanhALL()
        {
            var result = (from o in this.Context.TinhThanhs.Where(x => x.GCRecord == null)
                          select new DTO_TinhThanh { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTinhThanh = o.TenTinhThanh }).ToList();
            //
            return result;
        }
        public List<DTO_QuanHuyen> GetListQuanHuyenALL()
        {
            var result = (from o in this.Context.QuanHuyens.Where(x => x.GCRecord == null)
                          select new DTO_QuanHuyen { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenQuanHuyen = o.TenQuanHuyen }).ToList();
            //
            return result;
        }

        public QuocGia GetQuocGiaByOid(Guid oid)
        {
            var result = (from o in this.Context.QuocGias
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }

        public TinhThanh GetTinhThanhByOid(Guid oid)
        {
            var result = (from o in this.Context.TinhThanhs
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }

        public QuanHuyen GetQuanHuyenByOid(Guid oid)
        {
            var result = (from o in this.Context.QuanHuyens
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }

        public XaPhuong GetXaPhuongByOid(Guid oid)
        {
            var result = (from o in this.Context.XaPhuongs
                          where o.GCRecord == null && o.Oid == oid select o).SingleOrDefault();
            //
            return result;
        }
        public List<DTO_XaPhuong> GetListXaPhuongALL()
        {
            var result = (from o in this.Context.XaPhuongs.Where(x => x.GCRecord == null)
                          select new DTO_XaPhuong { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenXaPhuong = o.TenXaPhuong }).ToList();
            //
            return result;
        }

        public List<DTO_TrinhDoVanHoa> GetListTrinhDoVanHoaALL()
        {
            var result = (from o in this.Context.TrinhDoVanHoas.Where(x => x.GCRecord == null)
                          select new DTO_TrinhDoVanHoa { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTrinhDoVanHoa = o.TenTrinhDoVanHoa }).ToList();
            //
            return result;
        }
        public List<DTO_TrinhDoChuyenMon> GetListTrinhDoChuyenMonALL()
        {
            var result = (from o in this.Context.TrinhDoChuyenMons.Where(x => x.GCRecord == null)
                          select new DTO_TrinhDoChuyenMon { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTrinhDoChuyenMon = o.TenTrinhDoChuyenMon }).ToList();
            //
            return result;
        }
        public List<DTO_ChuyenNganhDaoTao> GetListChuyenNganhDaoTaoALL()
        {
            var result = (from o in this.Context.ChuyenMonDaoTaos.Where(x => x.GCRecord == null)
                          select new DTO_ChuyenNganhDaoTao { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenChuyenNganhDaoTao = o.TenChuyenMonDaoTao }).ToList();
            //
            return result;
        }
        public List<DTO_TruongDaoTao> GetListTruongDaoTaoALL()
        {
            var result = (from o in this.Context.TruongDaoTaos.Where(x => x.GCRecord == null)
                          select new DTO_TruongDaoTao { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTruongDaoTao = o.TenTruongDaoTao }).ToList();
            //
            return result;
        }
        public List<DTO_NgoaiNgu> GetListNgoaiNguALL()
        {
            var result = (from o in this.Context.NgoaiNgus.Where(x => x.GCRecord == null)
                          select new DTO_NgoaiNgu { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenNgoaiNgu = o.TenNgoaiNgu }).ToList();
            //
            return result;
        }
        public List<DTO_TrinhDoNgoaiNgu> GetListTrinhDoNgoaiNguALL()
        {
            var result = (from o in this.Context.TrinhDoNgoaiNgus.Where(x => x.GCRecord == null)
                          select new DTO_TrinhDoNgoaiNgu { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTrinhDoNgoaiNgu = o.TenTrinhDoNgoaiNgu }).ToList();
            //
            return result;
        }

        public void FullDelete(Entities context, params Object[] deleteList)
        {
            foreach (Web_UpdateHoSoNhanVien item in deleteList)
            {
                context.DeleteObject(item);
            }
        }
        #endregion
    }//end class
}
