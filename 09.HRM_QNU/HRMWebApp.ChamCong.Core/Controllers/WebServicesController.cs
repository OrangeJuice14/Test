using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth2;
using HRMWebApp.ChamCong.Core.DTO;
using HRMWebApp.Helpers;
using HRMWeb_Business.Model;
using HRMWeb_Service;
using Helper = HRMWebApp.Helpers.Helper;
using Microsoft.AspNet.Identity;
using HRMWebApp.KPI.Core.Security;
using Newtonsoft.Json;

namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class WebServicesController : Controller
    {
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();  //readonly Service1Client _service = new Service1Client();
        private static readonly GoogleClient googleClient = new GoogleClient
        {
            ClientIdentifier = ConfigurationManager.AppSettings["googleClientID"],
            ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(ConfigurationManager.AppSettings["googleClientSecret"]),
        };
        public static String DocMaDonViSuDung()
        {
            return ConfigurationUtil.ReadAppSetting("MaDonViSuDung");
        }

        [HttpPost]
        public ActionResult HelloWorld()
        {
            return new EmptyResult();
        }
        public string GetList_TinhThanh()
        {
            return _service.GetList_TinhThanh_Json(PublicKey, _token);
        }
        public string GetList_QuanLyUser()
        {
            return _service.GetList_WebUser_Json(PublicKey, _token); ;
        }

        public string GetList_QuanLyUserQuanTri()
        {
            return _service.GetList_WebUserQuanTri_Json(PublicKey, _token); ;
        }

        public string GetList_QuanLyUserKhacQuanTri()
        {
            return _service.GetList_WebUserKhacQuanTri_Json(PublicKey, _token); ;
        }


        public string GetDetail_QuanLyUser(string id)
        {
            var obj = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(id));
            //var js = new JavaScriptSerializer();
            return obj.ToJSON().Content;
        }


        public ActionResult Save_QuanLyUser(QuanLyUser obj)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = obj.ToJSON();
            _service.Save_WebUser_Json(PublicKey, _token, jsonObject.Content);
            return Helper.JsonSucess();
        }
        public ActionResult Save_KhaiBaoChamCongGiangVien(QuanLyKhaiBaoCCGV obj)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = obj.ToJSON();
            _service.Save_KhaiBaoChamCongGiangVien_Json(PublicKey, _token, jsonObject.Content);
            return Helper.JsonSucess();
        }

        public ActionResult Save_DangKyChamCongNgoaiGio(DangKyChamCongNgoaiGio obj)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = obj.ToJSON();
            _service.Save_DangKyChamCongNgoaiGio_Json(PublicKey, _token, jsonObject.Content);
            return Helper.JsonSucess();
        }


        public ActionResult WebUsers_KiemTraTrungUsername(string webUserId, string userName)
        {
            Guid? formatwebUserId = webUserId == null ? (Guid?)null : new Guid(webUserId);
            return _service.WebUsers_KiemTraTrungUsername(PublicKey, _token, formatwebUserId, userName).ToJSON();
        }


        public ActionResult ChangePassword_WebUser(string webUserId, string passWord)
        {
            _service.ChangePassword_WebUser(PublicKey, _token, new Guid(webUserId), passWord);
            return Helper.JsonSucess();
        }

        public ActionResult LogOut_WebUser()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Helper.JsonSucess();
        }

        public string WebMenu_GetListBy_WebUserId(string webUserId)
        {
            if (String.IsNullOrWhiteSpace(webUserId))
                return null;
            string json = _service.WebMenu_GetListTop2LevelDeepBy_WebUserId_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        public string WebMenu_GetURLListBy_WebUserId(string webUserId)
        {
            if (String.IsNullOrWhiteSpace(webUserId))
                return null;
            string json = _service.WebMenu_GetURLListBy_WebUserId_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        public string WebMenu_GetChildMenuListBy_WebUserId_AndMenuId(string webUserId, string menuId)
        {
            string json = _service.WebMenu_GetChildMenuListBy_WebUserId_AndMenuId_Json(PublicKey, _token, new Guid(webUserId), new Guid(menuId));
            return json;
        }

        public string WebGroup_GetList()
        {
            return _service.WebGroup_GetList_Json(PublicKey, _token);
        }

        public int CheckForLogin_WebUser(string userName, string passWord, string captchaString)
        {
            if (Session[SessionKey.CaptchaImage.ToString()].ToString().ToUpper() != captchaString.ToUpper() && captchaString != "")
                return 0;
            var user = _service.CheckForLogin_WebUser(PublicKey, _token, userName, passWord);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(userName, true, 15);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Set(cookie);
                SessionHelper.Data(SessionKey.UserId, user.Oid);
                SessionHelper.Data(SessionKey.ThongTinNhanVien, user.ThongTinNhanVien);
                SessionHelper.Data(SessionKey.UserName, user.UserName);
                SessionHelper.Data(SessionKey.HoVaTen, user.HoVaTen);

                SessionHelper.Data(SessionKey.WebGroupId, user.WebGroupID);
                return 1;
            }
            return 2;
        }

        public string QuanLyChamCong_GetDepartmentsOfUser(string userId)
        {
            return _service.GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json(PublicKey, _token, new Guid(userId));
        }

        public string QuanLyChamCong_GetUsersOfDepartment(string departmentId)
        {
            return _service.GetList_HoSoNhanVienBy_MaBoPhan_Json(PublicKey, _token, new Guid(departmentId));
        }

        public string GetList_BoPhanWebGroup_GetList()
        {
            return _service.GetList_BoPhan_Json(PublicKey, _token);
        }

        public string QuanLyChamCong_Find(int ngay, int thang, int nam, Guid? bophan, int trangthaichamcong, bool? diHoc, string maNhanSu, string webUserId, Guid? idLoaiNhanSu)
        {
            string json = _service.QuanLyChamCong_Find_Json(PublicKey, _token, ngay, thang, nam, bophan, trangthaichamcong, diHoc, maNhanSu, new Guid(webUserId), idLoaiNhanSu);
            return json;
        }
        public string QuanLyNghiPhep_Find(int nam, Guid? bophan,string maNhanSu)
        {
            string json = _service.QuanLyNghiPhep_Find_Json(PublicKey, _token, nam, bophan, maNhanSu);
            return json;
        }
        public ActionResult QuanLyNghiPhep_Create(int nam)
        {
            return _service.QuanLyNghiPhep_Create(PublicKey, _token, nam).ToJSON();
        }
        public ActionResult QuanLyNghiPhep_CheckExists(int nam)
        {
            return _service.QuanLyNghiPhep_CheckExists(PublicKey, _token, nam).ToJSON();
        }
        public string GetThongTinNghiPhep(Guid Oid)
        {
            string json = _service.GetThongTinNghiPhepDTO_Json(PublicKey, _token, Oid);
            return json;
        }
        public ActionResult ThongTinNghiPhep_Save(Guid Oid, decimal tongSoNgayPhep, decimal soNgayPhepCongThem, decimal soNgayPhepDaNghi, decimal soNgayPhepConLai, string ghiChu)
        {
            return _service.ThongTinNghiPhep_Save(PublicKey, _token, Oid, tongSoNgayPhep, soNgayPhepCongThem, soNgayPhepDaNghi, soNgayPhepConLai, ghiChu).ToJSON();
        }
        public string DangKyChamCong_Find(Guid? bophan, Guid ky, int trangthai)
        {
            string json = _service.DangKyChamCong_Find_Json(PublicKey, _token,bophan,ky,trangthai);
            return json;
        }
        public ActionResult DangKyChamCong_UpdateAll(Guid ky,Guid ca)
        {
            return _service.DangKyChamCong_UpdateAll(PublicKey, _token,ky,ca).ToJSON();
        }
        public ActionResult DangKyChamCong_DoiCa(Guid ca, Guid? bophan, byte loai, List<DTO_DangKyKhungGioLamViec> list)
        {
            return _service.DangKyChamCong_DoiCa(PublicKey, _token, ca, bophan, loai, list).ToJSON();
        }
        public ActionResult QuanLyChamCong_CheckChot(int thang, int nam, string boPhanId)
        {
            return _service.QuanLyChamCong_CheckChot(PublicKey, _token, new Guid(boPhanId), thang, nam).ToJSON();
        }
        public ActionResult QuanLyChamCong_CheckChotTheoNgay(DateTime ngay, string boPhanId)
        {
            int thang = ngay.Month;
            int nam = ngay.Year;
            return _service.QuanLyChamCong_CheckChot(PublicKey, _token, new Guid(boPhanId), thang, nam).ToJSON();
        }

        public string QuanLyChamCong_GetListHinhThucNghi()
        {
            return _service.GetList_HinhThucNghi_Json(PublicKey, _token);
        }
        public string GetList_LyDo()
        {
            return _service.GetList_LyDo_Json(PublicKey, _token);
        }

        public string QuanLyChamCong_GetListHinhThucNghiKyHieu()
        {
            return _service.GetList_HinhThucNghiKyHieu_Json(PublicKey, _token);
        }
        public string QuanLyChamCong_GetListHinhThucNghiForUpdate()
        {
            return _service.GetList_HinhThucNghiForUpdate_Json(PublicKey, _token);
        }
        public string QuanLyChamCong_GetSoNgayConLai(int nam, Guid idNhanVien)
        {
            return _service.QuanLyNghiPhep_NgayPhepConLai_Json(PublicKey, _token, nam, idNhanVien);
        }
        public string QuanLyChamCong_GetCacBuoiTrongNgay()
        {
            return _service.ChamCongNgayNghi_CacBuoiTrongNgay_Json(PublicKey, _token);
        }
        public string GetList_LoaiNhanSu()
        {
            return _service.GetList_LoaiNhanSu_Json(PublicKey, _token);
        }

        public string GetList_CaChamCong()
        {
            return _service.GetList_CaChamCong_Json(PublicKey, _token);
        }
        public string GetList_KyDangKy()
        {
            return _service.GetList_KyDangKy_Json(PublicKey, _token);
        }
        public string GetKyDangKy(Guid id)
        {
            return _service.GetKyDangKy_Json(PublicKey, _token,id);
        }
        public string GetList_ThoiGianDangKy()
        {
            return _service.GetList_ThoiGianDangKy_Json(PublicKey, _token);
        }
        public string GetDangKyByIDNV(Guid IDNV)
        {
            return _service.GetDangKyByIDNV_Json(PublicKey, _token, IDNV);
        }
        public string CaChamCong_GetByID(string id)
        {
            return _service.CaChamCong_GetByID_Json(PublicKey, _token, new Guid(id));
        }
        public ActionResult QuanLyChamCong_Save(List<UserSave> userList)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            _service.QuanLyChamCong_SaveList_Json(PublicKey, _token, jsonObject);
            return Helper.JsonSucess();
        }
        public ActionResult QuanLyChamCong_Remove(List<UserSave> userList)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            _service.QuanLyChamCong_RemoveList_Json(PublicKey, _token, jsonObject);
            return Helper.JsonSucess();
        }
        public ActionResult QuanLyChamCong_DoiCaChamCong(List<UserSave> userList, string caChamCongId)
        {
            var jsonObject = userList.ToJSON().Content;
            Guid caId = Guid.Empty;
            if (caChamCongId != "" && caChamCongId != null)
            {
                _service.QuanLyChamCong_DoiCaChamCong_Json(PublicKey, _token, jsonObject, new Guid(caChamCongId));
                return Helper.JsonSucess();
            }
            else
            {
                _service.QuanLyChamCong_DoiCaChamCong_Json(PublicKey, _token, jsonObject, caId);
                return Helper.JsonSucess();
            }
        }
        //public ActionResult CaChamCong_Save(String Oid, String TenCa, byte LoaiCa,
        //   int GioVaoSang, int PhutVaoSang, int GioRaSang, int PhutRaSang,
        //   int? GioBatDauNghi, int? PhutBatDauNghi, int? GioKetThucNghi, int? PhutKetThucNghi,
        //   int GioVaoChieu, int PhutVaoChieu, int GioRaChieu, int PhutRaChieu,
        //   int SoPhutCong, int SoPhutTru)
        //{
        //    Guid CCCID = Guid.Empty;
        //    if (Oid != "" && Oid != null)
        //    {
        //        CCCID = new Guid(Oid);
        //    }
        //    _service.CaChamCong_Save(PublicKey, _token, CCCID, TenCa, LoaiCa, GioVaoSang, PhutVaoSang, GioRaSang, PhutRaSang, GioBatDauNghi, PhutBatDauNghi, GioKetThucNghi, PhutKetThucNghi, GioVaoChieu, PhutVaoChieu, GioRaChieu, PhutRaChieu, SoPhutCong, SoPhutTru);
        //    return Helper.JsonSucess();
        //}
        public ActionResult CaChamCong_Save(String Oid, String TenCa, byte LoaiCa,
            int GioVaoSang, int PhutVaoSang,
            int? GioBatDauNghi, int? PhutBatDauNghi, int? GioKetThucNghi, int? PhutKetThucNghi,
            int GioRaChieu, int PhutRaChieu,
            int GioBatDauQuet, int PhutBatDauQuet, int GioKetThucQuet, int PhutKetThucQuet,
            int SoPhutCong, int SoPhutTru)
        {
            Guid CCCID = Guid.Empty;
            if (Oid != "" && Oid != null)
            {
                CCCID = new Guid(Oid);
            }
            int GioRaSang = GioBatDauNghi ?? default(int);
            int PhutRaSang = PhutBatDauNghi ?? default(int);
            int GioVaoChieu = GioKetThucNghi ?? default(int);
            int PhutVaoChieu = PhutKetThucNghi ?? default(int);
            _service.CaChamCong_Save(PublicKey, _token, CCCID, TenCa, LoaiCa, GioVaoSang, PhutVaoSang, GioRaSang, PhutRaSang, GioBatDauNghi, PhutBatDauNghi, GioKetThucNghi, PhutKetThucNghi, GioVaoChieu, PhutVaoChieu, GioRaChieu, PhutRaChieu, GioBatDauQuet, PhutBatDauQuet, GioKetThucQuet, PhutKetThucQuet, SoPhutCong, SoPhutTru);
            return Helper.JsonSucess();
        }
        public ActionResult ThoiGianDangKy_Save(CC_ThoiGianDangKyKhungGioLamViec obj)
        {
            _service.ThoiGianDangKy_Save(PublicKey, _token, obj);
            return Helper.JsonSucess();
        }
        public ActionResult DangKyKhungGioLamViec_Save(DTO_DangKyKhungGioLamViec obj)
        {
            var strJson = obj.ToJSON().Content;
            _service.DangKyKhungGioLamViec_Save(PublicKey, _token, obj);
            return Helper.JsonSucess();
        }
        public string DangKyChamCongNgayNghi_Find(int thang, int nam, string idNhanVien)
        {
            return _service.DangKyChamCongNgayNghi_Find_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        }
        public string QuanLyChamCong_BieuDo(int ngay, int thang, int nam, string bophanId)
        {          
            string webGroupId= new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien_Json(PublicKey, _token, ngay, thang, nam,
                    new Guid(idNhanVien));
            return _service.QuanLyChamCong_BieuDoVaoRa_Json(PublicKey, _token, ngay, thang, nam, new Guid(bophanId));
        }

        public string QuanLyChamCong_GetDepartmentOfStaff()
        {
            string result = "";
            Guid idNhanVien = Guid.Empty;
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id);
            }
             result= _service.QuanLyChamCong_GetDepartmentOfStaff(idNhanVien);
            return result;
        }
        public string QuanLyChamCong_ChamCongThang(int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu)
        {
            string webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id!=null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            Guid? formatLoaiNhanSuId = (idLoaiNhanSu == null || idLoaiNhanSu == "null") ? (Guid?)null : new Guid(idLoaiNhanSu);
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
            else
                return _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, formatLoaiNhanSuId);
        }


        public string QuanLyChamCong_ChamCongThang_Save(List<ChamCongThang> chamcongthang, int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu, string idWebUsers)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = chamcongthang.ToJSON().Content;
            _service.QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(PublicKey, _token, jsonObject, idWebUsers);
            string webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            Guid? formatLoaiNhanSuId = (idLoaiNhanSu == null || idLoaiNhanSu == "null") ? (Guid?)null : new Guid(idLoaiNhanSu);
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
            else
                return _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, formatLoaiNhanSuId);
            // return Helper.JsonSucess();
        }


        public string GetPhongBan_ById(string id)
        {
            return _service.Get_BoPhanBy_Id_Json(PublicKey, _token, new Guid(id));
        }


        public ActionResult ChotChamCongThang_CheckExists(int thang, int nam, string boPhanId)
        {
            return _service.ChotChamCongThang_CheckExists(PublicKey, _token, new Guid(boPhanId), thang, nam).ToJSON();
        }

        public ActionResult ChotChamCongThang_CheckLock(int thang, int nam)
        {
            return _service.ChotChamCongThang_CheckLock(PublicKey, _token,  thang, nam).ToJSON();
        }
        public ActionResult CaChamCong_CheckDangSuDung(String Oid)
        {
            if (Oid!="" && Oid!=null)
            {
                return _service.CaChamCong_CheckDangSuDung(PublicKey, _token, new Guid(Oid)).ToJSON();
            }
            return Helper.JsonSucess();
        }
        public ActionResult CaChamCong_Delete(string Oid)
        {
            if (Oid != "" && Oid != null)
            {
                return _service.CaChamCong_Delete(PublicKey, _token, new Guid(Oid)).ToJSON();
            }
            return Helper.JsonSucess();
        }
        public ActionResult ChotChamCongThang_Create(int thang, int nam, string userId, string boPhanId)
        {
            return _service.ChotChamCongThang_Create(PublicKey, _token, new Guid(boPhanId), thang, nam, new Guid(userId)).ToJSON();
        }


        public ActionResult ChotChamCongThang_Delete(int thang, int nam, string boPhanId)
        {
            return _service.ChotChamCongThang_Delete(PublicKey, _token, new Guid(boPhanId), thang, nam).ToJSON();
        }


        public ActionResult DoDuLieuChamCongThang(int thang, int nam, string idHinhThucNghi, string webUserId)
        {
            return _service.DoDuLieuChamCongThang(PublicKey, _token, thang, nam, new Guid(idHinhThucNghi), new Guid(webUserId)).ToJSON();
        }


        public ActionResult DoDuLieuChamCongThang_CheckExists(int thang, int nam)
        {
            return _service.DoDuLieuChamCongThang_CheckExists(PublicKey, _token, thang, nam).ToJSON();
        }

        public ActionResult DoDuLieuChamCongThang_BoSung(int thang, int nam, string idHinhThucNghi, string webUserId)
        {
            Guid id = Guid.Empty;
            if (idHinhThucNghi != null && idHinhThucNghi != "")
            {
                id = new Guid(idHinhThucNghi);
            }
            return _service.DoDuLieuChamCongThang_BoSung(PublicKey, _token, thang, nam, id, new Guid(webUserId)).ToJSON();
        }

        public ActionResult KyTinhLuong_Check(int thang, int nam)
        {
            return _service.KyTinhLuong_Check(PublicKey, _token, thang, nam).ToJSON();
        }

        public ActionResult ChamCongNhanh(int ngay, int thang, int nam, string idHinhThucNghi, string idBoPhan, string idLoaiNhanSu, string webUserId)
        {
            var formatHinhThucNghiId = idHinhThucNghi == null ? (Guid?)null : new Guid(idHinhThucNghi);
            var formatBoPhanId = idBoPhan == null ? (Guid?)null : new Guid(idBoPhan);
            var formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
            return _service.ChamCongNhanh(PublicKey, _token, ngay, thang, nam, formatHinhThucNghiId, formatBoPhanId, formatLoaiNhanSuId, new Guid(webUserId)).ToJSON();
        }

        public ActionResult CapNhatChamCongDonVi(int thang, int nam)
        {
            return _service.CapNhatChamCongDonVi(PublicKey, _token, thang, nam).ToJSON();
        }


        public string QuanLyXetABC_Find(int thang, int nam, string bophan, string idLoaiNhanSu, bool? diHoc)
        {
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            string json = _service.QuanLyXetABC_Find_Json(PublicKey, _token, thang, nam, new Guid(bophan), formatLoaiNhanSuId, diHoc);
            return json;
        }
        //public string QuanLyXetABC_Find(int thang, int nam, string bophan, string idLoaiNhanSu, bool? diHoc)
        //{
        //    Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    string json = _service.QuanLyXetABC_Find_Json(PublicKey, _token, thang, nam, new Guid(bophan), formatLoaiNhanSuId, diHoc);
        //    return json;
        //}

        public ActionResult QuanLyXetABC_Save(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> objList)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = objList.ToJSON().Content;
            return _service.QuanLyXetABC_SaveList_Json(PublicKey, _token, jsonObject).ToJSON();
        }


        public string QuanLyXetABC_BieuDo(int thang, int nam, string idNhanVien)
        {
            return _service.QuanLyXetABC_BieuDoVaoRa_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        }


        public string Get_HoSoNhanVienBy_Id(string idNhanVien)
        {
            string json=_service.Get_HoSoNhanVienBy_Id_Json(PublicKey, _token, new Guid(idNhanVien));
            return json;
        }


        public ActionResult QuanLyXetABC_KhoaVaMoKhoaList(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> userList, bool khoa)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            return _service.QuanLyXetABC_KhoaVaMoKhoaList_Json(PublicKey, _token, jsonObject, khoa).ToJSON();
        }


        public ActionResult WebUsers_XoaUsers(List<QuanLyUser> userList)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            return _service.WebUsers_XoaUsers_Json(PublicKey, _token, jsonObject).ToJSON();
        }


        public int CauHinhXetABC_GetThoiGian()
        {
            int thoigian = _service.CauHinhXetABC_GetThoiGian(PublicKey, _token, new Guid("A75E3EAE-9A56-4495-B1D2-100CC4B6B025"));
            return thoigian;
        }


        public ActionResult CauHinhXetABC_Update(int day)
        {
            return _service.CauHinhXetABC_Update(PublicKey, _token, new Guid("A75E3EAE-9A56-4495-B1D2-100CC4B6B025"), day).ToJSON();
        }


        public string QuanLyXetABC_Detail(int thang, int nam, string idNhanVien)
        {
            return _service.QuanLyXetABC_ChiTietTheoNhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        }


        public string KiemTraPhongBanXetABC_Find(int thang, int nam, bool? daXetXongAbc)
        {
            return _service.KiemTraPhongBanXetABC_Find_Json(PublicKey, _token, thang, nam, daXetXongAbc);
        }



        public string ThongKeXetABCTheoNam_Find(int nam, string bophan, string idLoaiNhanSu, string maNhanSu, string webUserId)
        {
            Guid? formatBoPhanId = bophan == "" ? (Guid?)null : new Guid(bophan);
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            string webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.ThongKeXetABCTheoNam_Cua1NhanVien_Find_Json(PublicKey, _token, nam, new Guid(idNhanVien));
            else
                return _service.ThongKeXetABCTheoNam_Find_Json(PublicKey, _token, nam, formatBoPhanId, formatLoaiNhanSuId,
                    maNhanSu, new Guid(webUserId));
        }



        public string ThongKeXetABCTheoThang_Find(int thang, int nam, string bophan, string idLoaiNhanSu, string maNhanSu, string webUserId)
        {
            Guid? formatBoPhanId = bophan == "" ? (Guid?)null : new Guid(bophan);
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            string webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.ThongKeXetABCTheoThang_Cua1NhanVien_Find_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
            else
                return _service.ThongKeXetABCTheoThang_Find_Json(PublicKey, _token, thang, nam, formatBoPhanId,
                    formatLoaiNhanSuId,
                    maNhanSu, new Guid(webUserId));
        }



        public string QuanLyKhaiBaoCongTac_Find(int thang, int nam, string bophan, int? trangthai, string maNhanSu, string webUserId)
        {
            String json = "";
            Guid? formatBoPhanId = null;
            if (bophan!=null && bophan != "")
            {
                formatBoPhanId = new Guid(bophan);
            }
            json = _service.QuanLyKhaiBaoCongTac_Find_Json(PublicKey, _token, thang, nam, formatBoPhanId, trangthai, maNhanSu, new Guid(webUserId));
            return json;

        }


        public ActionResult QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList(List<QuanLyCongTac> list, int trangthai)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = list.ToJSON().Content;
            return _service.QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList_Json(PublicKey, _token, jsonObject, trangthai).ToJSON();
        }


        public ActionResult QuanLyKhaiBaoCongTac_Delete(List<QuanLyCongTac> list)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = list.ToJSON().Content;
            return _service.QuanLyKhaiBaoCongTac_DeleteListList_Json(PublicKey, _token, jsonObject).ToJSON();
        }



        public string CaNhanKhaiBaoCongTac_Find(int thang, int nam, string webUserId)
        {
            return _service.CaNhanKhaiBaoCongTac_Find_Json(PublicKey, _token, thang, nam, new Guid(webUserId));
        }
        public string CaNhanDangKyChamCongNgoaiGio_Find(int thang, int nam, string idNhanVien)
        {
            return _service.CaNhanDangKyChamCongNgoaiGio_Find_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        }
        public string QuanLyDangKyChamCongNgoaiGio_Find(int? ngay, int thang, int nam,string IDBoPhan,byte? trangthai)
        {
            return _service.QuanLyDangKyChamCongNgoaiGio_Find_Json(PublicKey, _token,ngay, thang, nam, new Guid(IDBoPhan), trangthai);
        }
        public ActionResult CaNhanKhaiBaoCongTac_KhaiBaoMoi(string noidung,string diadiem, DateTime tungay, DateTime denngay,byte buoi, int GioBatDau,int PhutBatDau,int GioKetThuc, int PhutKetThuc, string webUserId)
        {
            _service.CaNhanKhaiBaoCongTac_KhaiBaoMoi(PublicKey, _token, noidung,diadiem, tungay, denngay,buoi, GioBatDau,PhutBatDau,GioKetThuc,PhutKetThuc, new Guid(webUserId));
            return Helper.JsonSucess();
        }
        public ActionResult KyDangKyKhungGio_New(string id, string tenky, DateTime tungay, DateTime denngay)
        {
            Guid oid = Guid.Empty;
            if (id != "")
                oid = new Guid(id);
            _service.KyDangKyKhungGio_New(PublicKey, _token, oid, tenky, tungay, denngay);
            return Helper.JsonSucess();
        }
        public ActionResult DangKyKhungGio_CheckChot()
        {
            return _service.DangKyKhungGio_CheckChot(PublicKey, _token).ToJSON();
        }
        public ActionResult DangKyKhungGio_CheckNgoaiThoiGian()
        {
            return _service.DangKyKhungGio_CheckNgoaiThoiGian(PublicKey, _token).ToJSON();
        }
        public string KyTinhLuong()
        {
            return _service.KyTinhLuong_GetAll_Json(PublicKey, _token);
        }
        public string GetSoChungTuByKyTinhLuong(Guid kyTinhLuong)
        {
            return _service.GetSoChungTuByKyTinhLuong(PublicKey, _token, kyTinhLuong);
        }
        public string TongHopTienGiangVaThuNhapKhac(Guid nhanVien, Guid kyTinhLuong, Guid soChungTu)
        {
            return _service.TongHopTienGiangVaThuNhapKhac(PublicKey, _token, nhanVien, kyTinhLuong, soChungTu);
        }
        public string TongHopThueTNCNTrongThang(Guid nhanVien, Guid kyTinhLuong, Guid soChungTu)
        {
            return _service.TongHopThueTNCNTrongThang(PublicKey, _token, nhanVien, kyTinhLuong, soChungTu);
        }


        public string KyTinhLuong_ByMonthAndYear(int month, int year)
        {
            return _service.KyTinhLuong_ByMonthAndYear_Json(PublicKey, _token, month, year);
        }


        public ActionResult KiemTraKhoaSo_KyTinhLuong(int month, int year)
        {
            return _service.KiemTraKhoaSo_KyTinhLuong(PublicKey, _token, month, year).ToJSON();
        }


        public string ThongTinNhanSu_BANGLUONG(string webUserId, string kyTinhLuong, byte? loaiLuong)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong));
            return json;

        }


        public string ThongTinNhanSu_SoYeuLyLich_Json(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_SoYeuLyLich_Json(PublicKey, _token, new Guid(webUserId));
            return json;


        }


        public string ThongTinNhanSu_THONGTINLUONGNHANVIEN(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_HOPDONGLAODONG(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_HOPDONGLAODONG_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }


        public string ModuleThongTinNhanSu_QuanHeGiaDinh(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuanHeGiaDinh_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }



        public string ModuleThongTinNhanSu_DienBienLuong(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_DienBienLuong_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_LichSuBanThan(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_LichSuBanThan_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhDaoTao(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhDaoTao_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhBoiDuong(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhBoiDuong_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhBoNhiem(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhBoNhiem_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhKhenThuong(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhKhenThuong_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }



        public string ModuleThongTinNhanSu_QuaTrinhKyLuat(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhKyLuat_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhHoatDongXaHoi(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhHoiThao(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_BAOHIEMXAHOI(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_BAOHIEMXAHOI_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_DangVien(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_DangVien_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_DoanVien(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_DoanVien_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_CongDoan(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_CongDoan_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhCongTac(string webUserId)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_QuaTrinhCongTac_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }



        public string BangLuong_LuongNganSach(string webUserId, int nam, int thang)
        {
            String json = "";
            json=_service.BangLuong_LuongNganSach_Json(PublicKey, _token, new Guid(webUserId), nam, thang);
            return json;
        }
        public string BangLuong_LuongTamUng(string webUserId, int nam, int thang)
        {
            String json = "";
            json = _service.BangLuong_LuongTamUng_Json(PublicKey, _token, new Guid(webUserId), nam, thang);
            return json;
        }
        public string NgayNghiTrongNam_Find(int nam)
        {
            return _service.NgayNghiTrongNam_Find_Json(PublicKey, _token, nam);
        }
        public ActionResult NgayNghiTrongNam_Save(String TenNgayNghi, DateTime TuNgay, DateTime DenNgay, int Nam)
        {        
            _service.NgayNghiTrongNam_Save(PublicKey, _token,TenNgayNghi, TuNgay, DenNgay, Nam);
            return Helper.JsonSucess();
        }
        public ActionResult NgayNghiTrongNam_Delete(string Oid)
        {
            if (Oid != "" && Oid != null)
            {
                return _service.NgayNghiTrongNam_Delete(PublicKey, _token, new Guid(Oid)).ToJSON();
            }
            return Helper.JsonSucess();
        }
        public string ChamCongNgayNghi_Find(int ngay, int thang, int nam, Guid? boPhanId, string maNhanSu, string webUserId, Guid? idLoaiNhanSu)
        {
            return _service.ChamCongNgayNghi_Find_Json(PublicKey, _token, thang, nam, boPhanId, maNhanSu, new Guid(webUserId), idLoaiNhanSu);
        }
        public ActionResult ChamCongNgayNghi_AcceptList(List<DTO_ChamCongNgayNghi_Find> obj, int isAdmin)
        {
            string strJson = obj.ToJSON().Content;
            _service.ChamCongNgayNghi_AcceptList_Json(PublicKey, _token, strJson, isAdmin);
            return Helper.JsonSucess();
        }

        public ActionResult ChamCongNgayNghi_CancelList(List<DTO_ChamCongNgayNghi_Find> obj, bool isAdmin)
        {
            string strJson = obj.ToJSON().Content;
            _service.ChamCongNgayNghi_CancelList_Json(PublicKey, _token, strJson, isAdmin);
            return Helper.JsonSucess();
        }
        public int ChamCongNgayNghi_LaySoDangKyNghiDangChoXet(int thang, int nam, Guid boPhanId, Guid webUserId)
        {
            return _service.ChamCongNgayNghi_LaySoDangKyNghiDangChoXet(PublicKey, _token, thang, nam, boPhanId, webUserId);
        }
        public int QuanLyKhaiBaoCongTac_LaySoKhaiBaoCongTacDangChoXet(int thang, int nam, Guid boPhanId, Guid webUserId)
        {
            return _service.QuanLyKhaiBaoCongTac_LaySoKhaiBaoCongTacDangChoXet(PublicKey, _token, thang, nam, boPhanId, webUserId);
        }
        public string KhaiBaoChamCong_Find(int? ngay, int thang, int nam, Guid boPhanId, string maNhanSu)
        {
            return _service.KhaiBaoChamCong_Find_Json(PublicKey, _token,ngay, thang, nam, boPhanId, maNhanSu);
        }
        public string QuanLyViPham_Find(int ngay, int thang, int nam, string boPhanId)
        {
            Guid boPhanGuid = Guid.Empty;
            if (boPhanId!="" && boPhanId!=null)
            {
                boPhanGuid = new Guid(boPhanId);
            }
            return _service.QuanLyViPham_Find_Json(PublicKey, _token,ngay, thang, nam, boPhanGuid);
        }

        public ActionResult QuanLyViPham_GuiMail()
        {
            _service.QuanLyViPham_GuiMail(PublicKey, _token);
            return Helper.JsonSucess();
        }

        public ActionResult ChamCongNgayNghi_TaoMoi(string nhanVienId, string noiDung, string noiNghiPhep, string tenDonXinNghi, string diaChiLienHe, string idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, string tinhThanh, bool truNgayDiDuong, int soNgayDiDuong, string webUserId, string buoiTuNgay, string buoiDenNgay, decimal? soNgayPhepConLai, bool isAdmin)
        {
            _service.ChamCongNgayNghi_TaoMoi(PublicKey, _token, new Guid(nhanVienId), noiDung, noiNghiPhep, tenDonXinNghi, diaChiLienHe, new Guid(idHinhThucNghi), loaiDonXinNghi, tuNgay,
               denNgay, new Guid(tinhThanh), truNgayDiDuong, soNgayDiDuong, new Guid(webUserId), new Guid(buoiTuNgay), new Guid(buoiDenNgay), soNgayPhepConLai, isAdmin);
            return Helper.JsonSucess();
        }
        public ActionResult KiemTraSoNgayDangKyNghi(Guid idHinhThucNghi, DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay)
        {
            return _service.KiemTraSoNgayDangKyNghi(idHinhThucNghi, tuNgay, denNgay, buoiTuNgay, buoiDenNgay).ToJSON();
        }
        public ActionResult KiemTraSoNgayDiDuong(decimal soNgayDiDuong, DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay)
        {
            return _service.KiemTraSoNgayDiDuong(soNgayDiDuong, tuNgay, denNgay, buoiTuNgay, buoiDenNgay).ToJSON();
        }
        public ActionResult KiemTraSoNgayDangKyNghiPhep(Guid idHinhThucNghi, DateTime tuNgay, DateTime denNgay, Guid buoiTuNgay, Guid buoiDenNgay, decimal soNgayToiDa, decimal soNgayDiDuong)
        {
            return _service.KiemTraSoNgayDangKyNghiPhep(idHinhThucNghi, tuNgay, denNgay, buoiTuNgay, buoiDenNgay, soNgayToiDa, soNgayDiDuong).ToJSON();
        }
        public ActionResult KiemTraTruNgayDiDuong(int nam, Guid idNhanVien)
        {
            return _service.KiemTraTruNgayDiDuong(nam, idNhanVien).ToJSON();
        }
        public string ChamCongNgayNghi_Report(string id)
        {
            return _service.ChamCongNgayNghi_Report_Json(PublicKey, _token, new Guid(id));
        }

        public ActionResult ChamCongNgayNghi_DeleteList(List<DTO_ChamCongNgayNghi_Find> obj)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strJson = obj.ToJSON().Content;
            _service.ChamCongNgayNghi_DeleteList_Json(PublicKey, _token, strJson);
            return Helper.JsonSucess();
        }
        public ActionResult KhaiBaoChamCong_DeleteList(List<DTO_KhaiBaoChamCong_Find> obj)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strJson = obj.ToJSON().Content;
            _service.KhaiBaoChamCong_DeleteList_Json(PublicKey, _token, strJson);
            return Helper.JsonSucess();
        }

        public ActionResult ChamCongNgayNghi_Save(DTO_ChamCongNgayNghi_Find obj)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strJson = obj.ToJSON().Content;
            _service.ChamCongNgayNghi_Save(PublicKey, _token, obj);
            return Helper.JsonSucess();
        }

        public string ChamCongNgayNghi_GetByID(string id)
        {
            return _service.ChamCongNgayNghi_GetByID_Json(PublicKey, _token, new Guid(id));
        }


        public string HoSoNhanVienBy_MaBoPhan(string maBoPhan)
        {
            var formatBoPhanId = maBoPhan == null ? (Guid?)null : new Guid(maBoPhan);
            return _service.GetList_HoSoNhanVienBy_MaBoPhan_Json(PublicKey, _token, formatBoPhanId);
        }


        public ActionResult ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(string chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, string nhanVienID)
        {
            var formatchamCongNgayNghiOid = chamCongNgayNghiOid == null ? (Guid?)null : new Guid(chamCongNgayNghiOid);
            return _service.ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatchamCongNgayNghiOid, tuNgay,
                denNgay, new Guid(nhanVienID)).ToJSON();
        }

        public ActionResult KhaiBaoChamCong_KiemTraTonTaiTuNgayDenNgay(DateTime tuNgay, DateTime denNgay, string nhanVienID)
        {
            return _service.KhaiBaoChamCong_KiemTraTonTaiTuNgayDenNgay(PublicKey, _token,tuNgay,
                denNgay, new Guid(nhanVienID)).ToJSON();
        }


        public ActionResult CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(string khaiBaoCongTacOid, DateTime tuNgay, DateTime denNgay, string webUserId)
        {
            var formatkhaiBaoCongTacOid = khaiBaoCongTacOid == null ? (Guid?)null : new Guid(khaiBaoCongTacOid);
            return _service.CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatkhaiBaoCongTacOid, tuNgay,
                denNgay, new Guid(webUserId)).ToJSON();
        }
        public ActionResult KyDangKy_KiemTraTuNgayDenNgayCoHopLe(string Oid, DateTime tuNgay, DateTime denNgay)
        {
            var formatkhaiBaoCongTacOid = Oid == null ? (Guid?)null : new Guid(Oid);
            return _service.KyDangKy_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatkhaiBaoCongTacOid, tuNgay,
                denNgay).ToJSON();
        }


        public ActionResult CaNhanKhaiBaoCongTac_DeleteList(List<DTO_CC_KhaiBaoCongTac> list)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strJson = list.ToJSON().Content;
            return _service.CaNhanKhaiBaoCongTac_DeleteList_Json(PublicKey, _token, strJson).ToJSON();
        }
        public ActionResult KyDangKy_DeleteList(List<CC_KyDangKyKhungGio> list)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strJson = list.ToJSON().Content;
            return _service.KyDangKy_DeleteList_Json(PublicKey, _token, strJson).ToJSON();
        }
        public ActionResult CaNhanDangKyChamCongNgoaiGio_DeleteList(List<DTO_CC_DangKyChamCongNgoaiGio> list)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strJson = list.ToJSON().Content;
            return _service.CaNhanDangKyChamCongNgoaiGio_DeleteList_Json(PublicKey, _token, strJson).ToJSON();
        }
        public ActionResult QuanLyDangKyChamCongNgoaiGio_DuyetList(List<DTO_CC_DangKyChamCongNgoaiGio> list, byte trangthai)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strJson = list.ToJSON().Content;
            return _service.QuanLyDangKyChamCongNgoaiGio_DuyetList_Json(PublicKey, _token, strJson,trangthai).ToJSON();
        }


        public string GiayToHoSo_GetList_ByNhanVienId(string nhanVienId)
        {
            String json = "";
            Guid thongTinNhanVien = nhanVienId == null ? Guid.Empty : new Guid(nhanVienId);
            json = _service.GiayToHoSo_GetList_ByNhanVienId_Json(PublicKey, _token, thongTinNhanVien);
            return json;
        }

        public ActionResult GetUserSessionInfo()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["UserId"] = Session[SessionKey.UserId.ToString()];
            result["ThongTinNhanVien"] = Session[SessionKey.ThongTinNhanVien.ToString()];
            result["UserName"] = Session[SessionKey.UserName.ToString()];
            result["HoVaTen"] = Session[SessionKey.HoVaTen.ToString()];
            result["WebGroupId"] = Session[SessionKey.WebGroupId.ToString()];
            return result.ToJSON();
        }

        public void Login_Gmail()
        {
             try
             {
                 IAuthorizationState authorization = googleClient.ProcessUserAuthorization();
                 if (authorization == null)
                 {
                     googleClient.RequestUserAuthorization(scope: new[] { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/plus.me" });
                     googleClient.RequestUserAuthorization(scope: new[] { GoogleClient.Scopes.UserInfo.Profile, GoogleClient.Scopes.UserInfo.Email });
                 }
                 else
                 {
                     IOAuth2Graph oauth2Graph = googleClient.GetGraph(authorization);
                     var user = _service.WebUser_GetByEmail(PublicKey, _token, oauth2Graph.Email);
                     if (user == null)
                         return;
                     var authTicket = new FormsAuthenticationTicket(user.UserName, true, 15);
                     string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                     var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                     System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
                     SessionHelper.Data(SessionKey.UserId, user.Oid);
                     SessionHelper.Data(SessionKey.ThongTinNhanVien, user.ThongTinNhanVien);
                     SessionHelper.Data(SessionKey.UserName, user.UserName);
                     SessionHelper.Data(SessionKey.HoVaTen, user.HoVaTen);
                     Response.Redirect("/");
                 }
             }
             catch (Exception ex) { }
           
        }

    }
}
