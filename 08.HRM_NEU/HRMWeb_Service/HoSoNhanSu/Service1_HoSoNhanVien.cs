using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ERP_Core;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Model;
using Newtonsoft.Json;

namespace HRMWeb_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class Service1 : IService1
    {
        private static void GetSetChild(HoSo hoSo)
        {
            if (hoSo.NhanVien.BoPhan != null)
            {
                hoSo.MaBoPhan = hoSo.NhanVien.BoPhan;
                //hoSo.DTOBoPhan = hoSo.NhanVien.BoPhan1.Map<DTO_BoPhan>();
            }
        }
        // ////////////////////////////////////////////
        public DTO_HoSoNhanVien Get_HoSoNhanVienBy_Id(String publicKey, String token, Guid id)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                HoSo_Factory factory = HoSo_Factory.New();
                var tmpObj = factory.GetByID(id);
                GetSetChild(tmpObj);
                DTO_HoSoNhanVien obj = tmpObj.Map<DTO_HoSoNhanVien>();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public String Get_HoSoNhanVienBy_Id_Json(String publicKey, String token, Guid id)
        {//DANG SD
            DTO_HoSoNhanVien obj = Get_HoSoNhanVienBy_Id(publicKey, token, id);
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }
        // ///////////////////////////////////////
        public IEnumerable<DTO_HoSoNhanVien> GetList_HoSoNhanVien(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                HoSo_Factory factory = HoSo_Factory.New();
                IEnumerable<HoSo> tmpList = factory.GetAll_GCRecordIsNull().ToList();
                foreach (HoSo hoSo in tmpList)
                {
                    GetSetChild(hoSo);
                }
                IEnumerable<DTO_HoSoNhanVien> list = tmpList.Map<DTO_HoSoNhanVien>();
                return list;
            }
            else
            {
                return null;
            }
        }



        public String GetList_HoSoNhanVien_Json(String publicKey, String token)
        {
            IEnumerable<DTO_HoSoNhanVien> list = GetList_HoSoNhanVien(publicKey, token);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }
        // ///////////////////////////////////////
        public IEnumerable<DTO_HoSoNhanVien> GetList_HoSoNhanVienBy_MaBoPhan(String publicKey, String token, Guid? maBoPhan)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                HoSo_Factory factory = HoSo_Factory.New();
                IEnumerable<HoSo> tmpList = factory.GetListByMaBoPhan_GCRecordIsNull(maBoPhan).ToList();
                foreach (HoSo hoSo in tmpList)
                {
                    GetSetChild(hoSo);
                }
                IEnumerable<DTO_HoSoNhanVien> list = tmpList.Map<DTO_HoSoNhanVien>();
                return list;
            }
            else
            {
                return null;
            }
        }
        public String GetList_HoSoNhanVienBy_MaBoPhan_Json(String publicKey, String token, Guid? maBoPhan)
        {//DANG SD
            IEnumerable<DTO_HoSoNhanVien> list = GetList_HoSoNhanVienBy_MaBoPhan(publicKey, token, maBoPhan);
            String json = JsonConvert.SerializeObject(list);
            return json;
        }


        ///////////////// Cập nhật thông tin Hồ Sơ /////////////////
        public String HoSoNhanVien_GetListTinhThanhALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListTinhThanhALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/HoSoNhanVien_GetListTinhThanhALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String HoSoNhanVien_GetListTinhTrangHonNhanALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListTinhTrangHonNhanALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/HoSoNhanVien_GetListTinhTrangHonNhanALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String HoSoNhanVien_GetListTonGiaoALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListTonGiaoALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/HoSoNhanVien_GetListTonGiaoALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String QuanHeGiaDinh_GetListQuocGiaALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListQuocGiaALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_GetListQuocGiaALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String QuanHeGiaDinh_GetListQuanHeALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListQuanHeALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_GetListQuanHeALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String QuanHeGiaDinh_GetListLoaiGiamTruGiaCanhALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListLoaiGiamTruGiaCanhALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_GetListLoaiGiamTruGiaCanhALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public String NhomMauALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListNhomMauALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/NhomMauALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }

        public String HoSoNhanVien_GetByNhanVien(String publicKey, String token, Guid thongTinNhanVien)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    HoSo_Factory factory = HoSo_Factory.New();
                    var hoso = factory.Context.spd_WebChamCong_CapNhatThongTinHoSo_Select(thongTinNhanVien).FirstOrDefault().Map<spd_WebChamCong_CapNhatThongTinHoSo_Select_Result>();
                    return JsonConvert.SerializeObject(hoso);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/HoSoNhanVien_GetByNhanVien", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public bool HoSoNhanVien_CapNhat(String publicKey, String token, string tenGoiKhac, bool? gioiTinh, DateTime? ngaySinh,
            Guid? noiSinhQuocGia, Guid? noiSinhTinhThanh, Guid? noiSinhQuanHuyen, Guid? noiSinhXaPhuong, string noiSinhSoNha, Guid? queQuanQuocGia,
            Guid? queQuanTinhThanh, Guid? queQuanQuanHuyen, Guid? queQuanXaPhuong, string queQuanSoNha, Guid? danToc, Guid? tinhTrangSucKhoe, Guid? nhomMau,
            string donViTuyenDung, DateTime? ngayTuyenDung, string congViecTuyenDung, Guid? diaChiThuongTruQuocGia, Guid? diaChiThuongTruTinhThanh,
            Guid? diaChiThuongTruQuanHuyen, Guid? diaChiThuongTruXaPhuong, string diaChiThuongTruSoNha, Guid? noiOHienNayQuocGia, Guid? noiOHienNayTinhThanh,
            Guid? noiOHienNayQuanHuyen, Guid? noiOHienNayXaPhuong, string noiOHienNaySoNha, Guid? quocTich, Guid? thanhPhanXuatThan, Guid? uuTienGiaDinh,
            Guid? uuTienBanThan, Guid? oid, Guid? tonGiao, int? chieuCao, int? canNang, Guid? tinhTrangHonNhan, string cMND, DateTime? ngayCap, Guid? noiCap, string dienThoaiDiDong)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    HoSo_Factory factory = HoSo_Factory.New();
                    factory.Context.spd_WebChamCong_CapNhatThongTinHoSo(tenGoiKhac, gioiTinh, ngaySinh, noiSinhQuocGia, noiSinhTinhThanh, noiSinhQuanHuyen,
                        noiSinhXaPhuong, noiSinhSoNha, queQuanQuocGia, queQuanTinhThanh, queQuanQuanHuyen, queQuanXaPhuong, queQuanSoNha,
                        danToc, tinhTrangSucKhoe, nhomMau, donViTuyenDung, ngayTuyenDung, congViecTuyenDung, diaChiThuongTruQuocGia, diaChiThuongTruTinhThanh,
             diaChiThuongTruQuanHuyen, diaChiThuongTruXaPhuong, diaChiThuongTruSoNha, noiOHienNayQuocGia, noiOHienNayTinhThanh, noiOHienNayQuanHuyen,
                         noiOHienNayXaPhuong, noiOHienNaySoNha, quocTich, thanhPhanXuatThan, uuTienGiaDinh, uuTienBanThan,
                        oid, tonGiao, chieuCao, canNang, tinhTrangHonNhan, cMND, ngayCap, noiCap, dienThoaiDiDong);
                    return true;
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/HoSoNhanVien_CapNhat", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public String QuanHeGiaDinh_GetByOid(String publicKey, String token, Guid oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    HoSo_Factory factory = HoSo_Factory.New();
                    var quanhe = factory.Context.spd_WebChamCong_QuanHeGiaDinh_Select(oid).FirstOrDefault().Map<spd_WebChamCong_QuanHeGiaDinh_Select_Result>();
                    return JsonConvert.SerializeObject(quanhe);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_GetByOid", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public bool QuanHeGiaDinh_ThemMoi(String publicKey, String token, Guid? oid, Guid thongTinNhanVien, string hoTenNguoiThan, DateTime? ngaySinhFull, int? namSinh, string noiSinh, Guid? quocTich, string cMND, string soHoChieu, Guid? quanHe, Guid? loaiGiamTruGiaCanh, bool? phuThuoc, bool? lienHeKhanCap, string dienThoaiDiDong, byte? gioiTinh, Guid? queQuan, string ngheNghiepHienTai, string noiLamViec, byte? tinhTrang, string noiCuTru, Guid? nuocCuTru)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    HoSo_Factory factory = HoSo_Factory.New();
                    factory.Context.spd_WebChamCong_QuanHeGiaDinh_ThemMoi(oid, thongTinNhanVien, hoTenNguoiThan, ngaySinhFull, namSinh, noiSinh, quocTich, cMND, soHoChieu, quanHe, loaiGiamTruGiaCanh, phuThuoc, lienHeKhanCap, dienThoaiDiDong, gioiTinh, queQuan, ngheNghiepHienTai, noiLamViec, tinhTrang, noiCuTru, nuocCuTru);
                    return true;
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_ThemMoi", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool QuanHeGiaDinh_CapNhat(String publicKey, String token, Guid oid, Guid thongTinNhanVien, string hoTenNguoiThan, DateTime? ngaySinhFull, int? namSinh, string noiSinh, Guid? quocTich, string cMND, string soHoChieu, Guid? quanHe, Guid? loaiGiamTruGiaCanh, bool? phuThuoc, bool? lienHeKhanCap, string dienThoaiDiDong, byte? gioiTinh, Guid? queQuan, string ngheNghiepHienTai, string noiLamViec, byte? tinhTrang, string noiCuTru, Guid? nuocCuTru)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    HoSo_Factory factory = HoSo_Factory.New();
                    factory.Context.spd_WebChamCong_QuanHeGiaDinh_CapNhat(oid, thongTinNhanVien, hoTenNguoiThan, ngaySinhFull, namSinh, noiSinh, quocTich, cMND, soHoChieu, quanHe, loaiGiamTruGiaCanh, phuThuoc, lienHeKhanCap, dienThoaiDiDong, gioiTinh, queQuan, ngheNghiepHienTai, noiLamViec, tinhTrang, noiCuTru, nuocCuTru);
                    return true;
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_CapNhat", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool QuanHeGiaDinh_Xoa(String publicKey, String token, Guid oid)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    HoSo_Factory factory = HoSo_Factory.New();
                    factory.Context.spd_WebChamCong_QuanHeGiaDinh_Xoa(oid);
                    return true;
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/QuanHeGiaDinh_Xoa", ex);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public String DanToc_GetListDanTocALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListDanTocALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListDanTocALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String QuanHuyen_GetListQuanHuyenALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListQuanHuyenALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListQuanHuyenALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String QuanHuyen_GetListQuanHuyenByTinhThanhId(String publicKey, String token, Guid tinhThanh)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListQuanHuyenByTinhThanhId(tinhThanh).ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListQuanHuyenByTinhThanhId", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String XaPhuong_GetListXaPhuongALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListXaPhuongALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListXaPhuongALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String XaPhuong_GetListXaPhuongByQuanHuyen(String publicKey, String token, Guid quanHuyen)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListXaPhuongByQuanHuyenId(quanHuyen).ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListXaPhuongByQuanHuyenId", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String SucKhoe_GetListSucKhoeALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListSucKhoeALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListSucKhoeALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String ThanhPhanXuatThan_GetListThanhPhanXuatThanALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListThanhPhanXuatThanALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListThanhPhanXuatThanALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String UuTienBanThan_GetListUuTienBanThanALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListUuTienBanThanALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListUuTienBanThanALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
        public String UuTienGiaDinh_GetListUuTienGiaDinhALL(String publicKey, String token)
        {
            if (Helper.TrustTest(publicKey, token))
            {
                try
                {
                    Web_UpdateHoSoNhanVien_Factory factory = Web_UpdateHoSoNhanVien_Factory.New();
                    var result = factory.GetListUuTienGiaDinhALL().ToList();
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    HRMWebApp.Helpers.Helper.ErrorLog("Service1_HoSoNhanVien/DanToc_GetListUuTienGiaDinhALL", ex);
                    throw;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
