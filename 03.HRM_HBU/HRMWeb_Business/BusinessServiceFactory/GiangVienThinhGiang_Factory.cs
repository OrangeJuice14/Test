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
    public class GiangVienThinhGiang_Factory : BaseFactory<Entities, GiangVienThinhGiang>
    {
        //static readonly Log4netCustom.ILog TracingLog_ = Log4netCustom.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ERP_Core.BaseEntityObject CreateStandAloneObject()
        {
            return GiangVienThinhGiang_Factory.New().CreateAloneObject();
        }
        public static GiangVienThinhGiang_Factory New()
        {
            return new GiangVienThinhGiang_Factory();
        }
        public GiangVienThinhGiang_Factory()
            : base(Database.NewEntities())
        {

        }

        #region Custom
        public GiangVienThinhGiang Get_GiangVienThinhGiang_ByOid(Guid oid)
        {
            var result = (from o in this.ObjectSet
                          where o.Oid == oid
                                && o.NhanVien.HoSo.GCRecord == null
                          select o).SingleOrDefault();
            return result;
        }
        public bool CheckExistsMaQuanLyOfGiangVienThinhGiang(Guid oid,string maquanly)
        {
            var result = this.Context.GiangVienThinhGiangs.Where(x => x.Oid != oid && x.NhanVien.HoSo.MaQuanLy.Equals(maquanly)).FirstOrDefault();
            if (result != null)
                return true;
            else
                return false;
        }
        public DTO_GiangVienThinhGiang Get_DTOGiangVienThinhGiang_ByOid(Guid oid)
        {
            var result = (from o in this.ObjectSet
                                          where o.Oid == oid
                                                && o.NhanVien.HoSo.GCRecord == null
                                          select new DTO_GiangVienThinhGiang {
                                              Oid = o.Oid,
                                              //Sơ yếu lý lịch
                                              MaQuanLy = o.NhanVien.HoSo.MaQuanLy,
                                              Ho = o.NhanVien.HoSo.Ho,
                                              Ten = o.NhanVien.HoSo.Ten,
                                              TenBoPhan = o.NhanVien.BoPhan1.TenBoPhan,
                                              BoPhan = o.TaiBoMon.Value,
                                              NgaySinh = o.NhanVien.HoSo.NgaySinh.Value,
                                              GioiTinh = o.NhanVien.HoSo.GioiTinh == 0 ? "Nam" : "Nữ",
                                              CMND = o.CMND_ThinhGiang,
                                              NgayCap = o.NhanVien.HoSo.NgayCap.Value,
                                              NoiCap = o.NhanVien.HoSo.NoiCap,
                                              TinhTrangHonNhan = o.NhanVien.HoSo.TinhTrangHonNhan.Value,
                                              QuocGia = o.NhanVien.HoSo.QuocTich.Value,
                                              DanToc = o.NhanVien.HoSo.DanToc.Value,
                                              TonGiao = o.NhanVien.HoSo.TonGiao.Value,
                                              Oid_NoiSinh = o.NhanVien.HoSo.NoiSinh,
                                              QuocGia_NoiSinh = o.NhanVien.HoSo.DiaChi2.QuocGia.Value,
                                              TinhThanh_NoiSinh = o.NhanVien.HoSo.DiaChi2.TinhThanh.Value,
                                              QuanHuyen_NoiSinh = o.NhanVien.HoSo.DiaChi2.QuanHuyen.Value,
                                              XaPhuong_NoiSinh = o.NhanVien.HoSo.DiaChi2.XaPhuong.Value,
                                              SoNha_NoiSinh = o.NhanVien.HoSo.DiaChi2.SoNha,
                                              NgayVaoCoQuan = o.NhanVien.NgayVaoCoQuan.Value,
                                              DonViCongTac = o.DonViCongTac,
                                              HopDongHienTai = o.NhanVien.HopDong.SoHopDong,
                                              Email = o.NhanVien.HoSo.Email,
                                              DienThoaiDiDong = o.NhanVien.HoSo.DienThoaiDiDong,
                                              DienThoaiNhaRieng = o.NhanVien.HoSo.DienThoaiNhaRieng,
                                              TinhTrang = o.NhanVien.TinhTrang,
                                              QuocGia_DCTT = o.NhanVien.HoSo.DiaChi.QuocGia.Value,
                                              TinhThanh_DCTT = o.NhanVien.HoSo.DiaChi.TinhThanh.Value,
                                              QuanHuyen_DCTT = o.NhanVien.HoSo.DiaChi.QuanHuyen.Value,
                                              XaPhuong_DCTT = o.NhanVien.HoSo.DiaChi.XaPhuong.Value,
                                              Oid_DCTT = o.NhanVien.HoSo.DiaChiThuongTru,
                                              SoNha_DCTT = o.NhanVien.HoSo.DiaChi.SoNha,
                                              QuocGia_NOHN = o.NhanVien.HoSo.DiaChi1.QuocGia.Value,
                                              TinhThanh_NOHN = o.NhanVien.HoSo.DiaChi1.TinhThanh.Value,
                                              QuanHuyen_NOHN = o.NhanVien.HoSo.DiaChi1.QuanHuyen.Value,
                                              XaPhuong_NOHN = o.NhanVien.HoSo.DiaChi1.XaPhuong.Value,
                                              Oid_NOHN = o.NhanVien.HoSo.NoiOHienNay,
                                              SoNha_NOHN = o.NhanVien.HoSo.DiaChi1.SoNha,
                                              //Trình độ chuyên môn
                                              Oid_NVTD = o.NhanVien.NhanVienTrinhDo,
                                              TrinhDoTinHoc = o.NhanVien.NhanVienTrinhDo1.TrinhDoTinHoc,
                                              TrinhDoVanHoa = o.NhanVien.NhanVienTrinhDo1.TrinhDoVanHoa,
                                              HocHam = o.NhanVien.NhanVienTrinhDo1.HocHam,
                                              TrinhDoChuyenMon = o.NhanVien.NhanVienTrinhDo1.TrinhDoChuyenMon,
                                              ChuyenNganhDaoTao = o.NhanVien.NhanVienTrinhDo1.ChuyenMonDaoTao,
                                              TruongDaoTao = o.NhanVien.NhanVienTrinhDo1.TruongDaoTao,
                                              HinhThucDaoTao = o.NhanVien.NhanVienTrinhDo1.HinhThucDaoTao,
                                              NamTotNghiep = o.NhanVien.NhanVienTrinhDo1.NamTotNghiep.Value,
                                              NgoaiNgu = o.NhanVien.NhanVienTrinhDo1.NgoaiNgu,
                                              TrinhDoNgoaiNgu = o.NhanVien.NhanVienTrinhDo1.TrinhDoNgoaiNgu,
                                              //Thông tin lương
                                              Oid_NVTTL = o.NhanVien.NhanVienThongTinLuong,
                                              MaSoThue = o.NhanVien.NhanVienThongTinLuong1.MaSoThue,
                                              CoQuanThue = o.NhanVien.NhanVienThongTinLuong1.CoQuanThue,
                                              //Tai khoản ngân hàng
                                              Oid_TKNH = (from y in this.Context.TaiKhoanNganHangs
                                                          where y.NhanVien == o.Oid
                                                          select y.Oid).FirstOrDefault(),
                                              SoTaiKhoan = (from y in this.Context.TaiKhoanNganHangs
                                                            where y.NhanVien == o.Oid
                                                            select y.SoTaiKhoan).FirstOrDefault(),
                                              NganHang = (from y in this.Context.TaiKhoanNganHangs
                                                            where y.NhanVien == o.Oid
                                                            select y.NganHang).FirstOrDefault()


                                          }).SingleOrDefault();
            return result;
        }
        public IQueryable<DTO_GiangVienThinhGiang> GetDTOGiangVienThinhGiang_Find(Guid idbophan,string manhansu,Guid iswebuser)
        {
            //
            IQueryable<DTO_GiangVienThinhGiang> result = (from o in this.ObjectSet
                                              where (o.TaiBoMon == idbophan || idbophan == Guid.Empty)
                                                    && (string.IsNullOrEmpty(manhansu) || o.NhanVien.HoSo.MaQuanLy.Contains(manhansu))
                                                    && o.NhanVien.HoSo.GCRecord == null
                                                    //&& o.BoPhan.WebUser_BoPhan.Any(x=>x.IDWebUser == iswebuser)
                                              select new DTO_GiangVienThinhGiang
                                              {
                                                  Oid = o.Oid,
                                                  MaQuanLy = o.NhanVien.HoSo.MaQuanLy,
                                                  Ho = o.NhanVien.HoSo.Ho,
                                                  Ten = o.NhanVien.HoSo.Ten,
                                                  TenBoPhan = o.BoPhan.TenBoPhan,
                                                  NgaySinh = o.NhanVien.HoSo.NgaySinh.Value ,
                                                  GioiTinh = o.NhanVien.HoSo.GioiTinh.Value == 0 ? "Nam" : "Nữ",
                                                  CMND = o.CMND_ThinhGiang
                                              });
            return result;
        }
        public List<DTO_DanToc> GetListDanTocALL()
        {
            var result = (from o in this.Context.DanTocs.Where(x => x.GCRecord == null)
                          select new DTO_DanToc { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenDanToc = o.TenDanToc }).ToList();
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
        public List<DTO_TinhTrang> GetListTinhTrangALL()
        {
            var result = (from o in this.Context.TinhTrangs.Where(x => x.GCRecord == null)
                          select new DTO_TinhTrang { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTinhTrang = o.TenTinhTrang }).ToList();
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
        public List<DTO_XaPhuong> GetListXaPhuongALL()
        {
            var result = (from o in this.Context.XaPhuongs.Where(x => x.GCRecord == null)
                          select new DTO_XaPhuong { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenXaPhuong = o.TenXaPhuong }).ToList();
            //
            return result;
        }
        public List<DTO_HocHam> GetListHocHamALL()
        {
            var result = (from o in this.Context.HocHams.Where(x => x.GCRecord == null)
                          select new DTO_HocHam { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenHocHam = o.TenHocHam }).ToList();
            //
            return result;
        }
        public List<DTO_TrinhDoTinHoc> GetListTrinhDoTinHocALL()
        {
            var result = (from o in this.Context.TrinhDoTinHocs.Where(x => x.GCRecord == null)
                          select new DTO_TrinhDoTinHoc { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenTrinhDoTinHoc = o.TenTrinhDoTinHoc }).ToList();
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
        public List<DTO_HinhThucDaoTao> GetListHinhThucDaoTaoALL()
        {
            var result = (from o in this.Context.HinhThucDaoTaos.Where(x => x.GCRecord == null)
                          select new DTO_HinhThucDaoTao { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenHinhThucDaoTao = o.TenHinhThucDaoTao }).ToList();
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
        public List<DTO_CoQuanThue> GetListCoQuanThueALL()
        {
            var result = (from o in this.Context.CoQuanThues.Where(x => x.GCRecord == null)
                          select new DTO_CoQuanThue { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenCoQuanThue = o.TenCoQuanThue }).ToList();
            //
            return result;
        }
        public List<DTO_NganHang> GetListNganHangALL()
        {
            var result = (from o in this.Context.NganHangs.Where(x => x.GCRecord == null)
                          select new DTO_NganHang { Oid = o.Oid, MaQuanLy = o.MaQuanLy, TenNganHang = o.TenNganHang }).ToList();
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
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }

        public TrinhDoTinHoc GetTrinhDoTinHocByOid(Guid oid)
        {
            var result = (from o in this.Context.TrinhDoTinHocs
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public TrinhDoVanHoa GetTrinhDoVanHoaByOid(Guid oid)
        {
            var result = (from o in this.Context.TrinhDoVanHoas
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public HocHam GetHocHamByOid(Guid oid)
        {
            var result = (from o in this.Context.HocHams
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public TrinhDoChuyenMon GetTrinhDoChuyenMonByOid(Guid oid)
        {
            var result = (from o in this.Context.TrinhDoChuyenMons
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public TruongDaoTao GetTruongDaoTaoByOid(Guid oid)
        {
            var result = (from o in this.Context.TruongDaoTaos
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public ChuyenMonDaoTao GetChuyenMonDaoTaoByOid(Guid oid)
        {
            var result = (from o in this.Context.ChuyenMonDaoTaos
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public HinhThucDaoTao GetHinhThucDaoTaoByOid(Guid oid)
        {
            var result = (from o in this.Context.HinhThucDaoTaos
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public NgoaiNgu GetNgoaiNguByOid(Guid oid)
        {
            var result = (from o in this.Context.NgoaiNgus
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public TrinhDoNgoaiNgu GetTrinhDoNgoaiNguByOid(Guid oid)
        {
            var result = (from o in this.Context.TrinhDoNgoaiNgus
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public CoQuanThue GetCoQuanThueByOid(Guid oid)
        {
            var result = (from o in this.Context.CoQuanThues
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        public NganHang GetNganHangByOid(Guid oid)
        {
            var result = (from o in this.Context.NganHangs
                          where o.GCRecord == null && o.Oid == oid
                          select o).SingleOrDefault();
            //
            return result;
        }
        #endregion
    }//end class
}
