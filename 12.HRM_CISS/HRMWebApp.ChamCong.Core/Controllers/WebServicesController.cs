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
using Newtonsoft.Json;
using System.IO;
using HRMWeb_Business.BusinessServiceFactory;
using HRMWeb_Business.Predefined;

namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class WebServicesController : Controller
    {
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();  //readonly Service1Client _service = new Service1Client();

        public static Guid _congTy
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["CongTy"] == null)
                    return new Guid();
                else
                    return (Guid)System.Web.HttpContext.Current.Session["CongTy"];
            }
            set
            {
                System.Web.HttpContext.Current.Session["CongTy"] = value;
            }
        }

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
        public string GetList_QuanLyUser()
        {
            return _service.GetList_WebUser_Json(PublicKey, _token); ;
        }

        public string GetList_QuanLyUserQuanTri(string webgroupid)
        {
            return _service.GetList_WebUserQuanTri_Json(PublicKey, _token, new Guid(webgroupid), _congTy);
        }

        public string GetList_QuanLyUserKhacQuanTri(string webgroupid)
        {
            return _service.GetList_WebUserKhacQuanTri_Json(PublicKey, _token, new Guid(webgroupid), _congTy);
        }


        public string GetDetail_QuanLyUser(string id)
        {
            var obj = _service.Get_WebUser_ById(PublicKey, _token, new Guid(id));
            return obj.ToJSON().Content;
        }


        public ActionResult Save_QuanLyUser(QuanLyUser obj, string currentUserId)
        {
            //
            var jsonObject = obj.ToJSON();
            _service.Save_WebUser_Json(PublicKey, _token, jsonObject.Content, currentUserId);
            return Helper.JsonSucess();
        }
        public ActionResult Save_KhaiBaoChamCongGiangVien(QuanLyKhaiBaoCCGV obj)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = obj.ToJSON();
            //_service.Save_KhaiBaoChamCongGiangVien_Json(PublicKey, _token, jsonObject.Content);
            return Helper.JsonSucess();
        }

        public ActionResult Save_DangKyChamCongNgoaiGio(DTO_CC_ChamCongNgoaiGio obj, Guid idwebuser)
        {
            var jsonObject = obj.ToJSON();
            //
            if (_service.Save_DangKyChamCongNgoaiGio_Json(PublicKey, _token, jsonObject.Content, idwebuser))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
        public ActionResult ChamCongNgoaiGio_Save(List<DTO_CC_ChamCongNgoaiGio> objList)
        {
            var jsonObject = objList.ToJSON().Content;
            //
            if (_service.ChamCongNgoaiGio_SaveList_Json(PublicKey, _token, jsonObject))
                return Helper.JsonSucess();
            else
                return Helper.JsonErorr();
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
            string json = _service.WebMenu_GetListTop2LevelDeepBy_WebUserId_Json(PublicKey, _token, new Guid(webUserId), _congTy);
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
            string json = _service.WebMenu_GetChildMenuListBy_WebUserId_AndMenuId_Json(PublicKey, _token, new Guid(webUserId), new Guid(menuId), _congTy);
            return json;
        }

        public string WebGroup_GetList()
        {
            return _service.WebGroup_GetList_Json(PublicKey, _token, _congTy);
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
            //
            return _service.GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json(PublicKey, _token, new Guid(userId), _congTy);
        }

        public string QuanLyChamCong_GetDepartmentsOfUserAndCompany(string userId, string company)
        {
            //
            return _service.GetList_BoPhan_DuocPhanQuyenChoWebUserIdAndCompany_Json(PublicKey, _token, new Guid(userId), new Guid(company));
        }

        public string BoPhan_GetLoaiBoPhanByWebGroup(string webgroupid, string company)
        {
            return _service.GetBoPhan_GetLoaiBoPhanByWebGroup_Json(PublicKey, _token, new Guid(webgroupid), new Guid(company));
        }
        public string BoPhan_LayTatCaBoPhan()
        {
            return _service.GetBoPhan_LayTatCaBoPhan_Json(PublicKey, _token);
        }

        public string KhaiBaoCongTac_DanhSachTruongDonVi()
        {
            return _service.KhaiBaoCongTac_DanhSachTruongDonVi_Json(PublicKey, _token);
        }
        public string GetList_BoPhanWebGroup_GetList()
        {
            return _service.GetList_BoPhan_Json(PublicKey, _token);
        }
        public string GetList_NgayTrongKyChamCong(int thang, int nam, Guid bophanId, Guid webGroupId)
        {
            return _service.GetList_NgayTrongKyChamCong_Json(PublicKey, _token, thang, nam, bophanId, webGroupId, _congTy);
        }
        public string QuanLyChamCong_Find(int ngay, int thang, int nam, Guid? bophan, int trangthaichamcong, bool? diHoc, string maNhanSu, string webUserId, Guid? idLoaiNhanSu)
        {
            string json = _service.QuanLyChamCong_Find_Json(PublicKey, _token, ngay, thang, nam, bophan, trangthaichamcong, diHoc, maNhanSu, new Guid(webUserId), idLoaiNhanSu, _congTy);
            return json;
        }
        public ActionResult ChotChamCongThang_ChamCongThangDelete(int thang, int nam, string boPhanId)
        {
            //
            return _service.ChotChamCongThang_Delete(PublicKey, _token, new Guid(boPhanId), thang, nam, _congTy).ToJSON();
        }
        public ActionResult ChotChamCongThang_ChamCongThangCreate(int thang, int nam, string userId, string boPhanId)
        {
            if (_service.ChotChamCongThang_Create(PublicKey, _token, new Guid(boPhanId), thang, nam, new Guid(userId), _congTy))
            {
                return Helper.JsonSucess();
            }
            else
            {
                return Helper.JsonErorr(); ;
            }
            //
        }
        public string QuanLyChamCong_CheckDangKyKhungGio(List<DTO_QuanLyChamCong_Find> obj, int ngay, int thang, int nam)
        {
            DTO_QuanLyChamCong_Find o = obj.FirstOrDefault();
            return _service.QuanLyChamCong_CheckDangKyKhungGio(PublicKey, _token, o.Oid, ngay, thang, nam);

        }
        public ActionResult QuanLyChamCong_ChamCongThang_ChamNhanhCaNgay(int thang, int nam, string boPhanId)
        {
            return _service.QuanLyChamCong_ChamCongThang_ChamNhanhCaNgay(PublicKey, _token, thang, nam, new Guid(boPhanId)).ToJSON();
        }
        public ActionResult QuanLyChamCong_CheckChot(int thang, int nam, string boPhanId)
        {
            return _service.QuanLyChamCong_CheckChot(PublicKey, _token, new Guid(boPhanId), thang, nam).ToJSON();
        }
        public ActionResult ChotChamCongThang_ChamCongThangCheckLock(int thang, int nam)
        {
            return _service.ChotChamCongThang_ChamCongThangCheckLock(PublicKey, _token, thang, nam, _congTy).ToJSON();
        }
        public ActionResult QuanLyNghiPhep_CheckExists(Guid nienDoTaiChinh, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.QuanLyNghiPhep_CheckExists(PublicKey, _token, nienDoTaiChinh, idCongTy).ToJSON();
        }
        public ActionResult QuanLyChamCong_CheckChotTheoNgay(DateTime ngay, string boPhanId)
        {
            int thang = ngay.Month;
            int nam = ngay.Year;
            return _service.QuanLyChamCong_CheckChot(PublicKey, _token, new Guid(boPhanId), thang, nam).ToJSON();
        }
        public string ChamCongNgayNghi_GetListHinhThucNghi()
        {
            return _service.ChamCongNgayNghi_GetList_HinhThucNghi_Json(PublicKey, _token, _congTy);
        }
        public string QuanLyKhaiBaoCongTac_DanhSachFile(string oid)
        {
            //
            Guid oidKhaiBaoCongTac = Guid.Empty;
            if (!string.IsNullOrEmpty(oid))
                oidKhaiBaoCongTac = new Guid(oid);
            //
            return _service.QuanLyKhaiBaoCongTac_DanhSachFile_Json(PublicKey, _token, oidKhaiBaoCongTac);
        }
        public string CauHinhChamCong_Find(string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.CauHinhChamCong_Find_Json(PublicKey, _token, idCongTy);
        }
        public string QuanLyNhacViec_Find(string userId)
        {
            Guid userID = Guid.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                userID = new Guid(userId);
            }
            //
            return _service.QuanLyNhacViec_Find_Json(PublicKey, _token, userID, _congTy);
        }
        public string GetList_HoSoNhanVienCoIDChamCong()
        {
            return _service.GetList_HoSoNhanVienCoIDChamCong_Json(PublicKey, _token);
        }
        public ActionResult CauHinhChamCong_Save(string oid, string emailsender, string passsender, string songaynghiphep, string sogdkhoiduyet, string sodkngoaigio)
        {
            Guid oidCauHinh = Guid.Empty;
            if (!string.IsNullOrEmpty(oid))
            {
                oidCauHinh = new Guid(oid);
            }
            //
            if (_service.CauHinhChamCong_Save(PublicKey, _token, oidCauHinh, emailsender, passsender, songaynghiphep, sogdkhoiduyet, sodkngoaigio))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public ActionResult QuanLyNghiPhep_LuuTraPhep(string oid, DateTime ngaytraphep)
        {
            Guid oidNghiPhep = Guid.Empty;
            if (!string.IsNullOrEmpty(oid))
            {
                oidNghiPhep = new Guid(oid);
            }
            //
            if (_service.QuanLyNghiPhep_LuuTraPhep(PublicKey, _token, oidNghiPhep, ngaytraphep))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public ActionResult QuanLyNghiPhep_HuyTraPhep(string oid)
        {
            Guid oidNghiPhep = Guid.Empty;
            if (!string.IsNullOrEmpty(oid))
            {
                oidNghiPhep = new Guid(oid);
            }
            //
            if (_service.QuanLyNghiPhep_HuyTraPhep(PublicKey, _token, oidNghiPhep))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public ActionResult GetDuLieuTuMayChamCong(string tungay, string denngay)
        {
            try
            {
                DateTime tuNgay = DateTime.MinValue;
                DateTime denNgay = DateTime.MinValue;
                if (!string.IsNullOrEmpty(tungay))
                {
                    tuNgay = Convert.ToDateTime(tungay);
                }
                if (!string.IsNullOrEmpty(denngay))
                {
                    denNgay = Convert.ToDateTime(denngay);
                }
                //
                if (_service.GetDuLieuTuMayChamCong_Json(PublicKey, _token, tuNgay, denNgay))
                {
                    return Helper.JsonSucess();
                }
            }
            catch { }
            return Helper.JsonErorr();
        }
        public ActionResult ChotDuLieuTuMayChamCongTuNgay_DenNgay(string nhanvien, string tungay, string denngay, bool type)
        {
            try
            {
                DateTime tuNgay = DateTime.MinValue;
                DateTime denNgay = DateTime.MinValue;
                Guid idNhanVien = Guid.Empty;

                if (!string.IsNullOrEmpty(nhanvien))
                {
                    idNhanVien = new Guid(nhanvien);
                }
                else
                {
                    return Helper.JsonErorr();
                }
                if (!string.IsNullOrEmpty(tungay))
                {
                    tuNgay = Convert.ToDateTime(tungay);
                }
                if (!string.IsNullOrEmpty(denngay))
                {
                    denNgay = Convert.ToDateTime(denngay);
                }
                //
                if (_service.ChotDuLieuTuMayChamCongTuNgay_DenNgay_Json(PublicKey, _token, idNhanVien, tuNgay, denNgay, type))
                {
                    return Helper.JsonSucess();
                }
            }
            catch { }

            return Helper.JsonErorr();
        }
        public string QuanLyChamCong_GetListHinhThucNghi()
        {
            return _service.GetList_HinhThucNghi_Json(PublicKey, _token);
        }
        public string GetList_LyDo()
        {
            return "";//_service.GetList_LyDo_Json(PublicKey, _token);
        }

        public string QuanLyChamCong_GetListHinhThucNghiKyHieu()
        {
            return _service.GetList_HinhThucNghiKyHieu_Json(PublicKey, _token);
        }
        public string QuanLyChamCong_GetListHinhThucNghiForUpdate()
        {
            return _service.GetList_HinhThucNghiForUpdate_Json(PublicKey, _token);
        }

        public string GetList_LoaiNhanSu()
        {
            return _service.GetList_LoaiNhanSu_Json(PublicKey, _token);
        }

        public string GetNamHocList()
        {
            return _service.GetNamHocList_Json(PublicKey, _token);
        }
        public string GetNienDoTaiChinhList(string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.GetNienDoTaiChinhList_Json(PublicKey, _token, idCongTy);
        }
        public string GetNamHocHienTai()
        {
            return _service.GetNamHocHienTai_Json(PublicKey, _token);
        }


        public string GetList_CaChamCong()
        {
            return _service.GetList_CaChamCong_Json(PublicKey, _token);
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
        public ActionResult DangKyKhungGio_CheckChot()
        {
            return _service.DangKyKhungGio_CheckChot(PublicKey, _token).ToJSON();
        }
        public string GetKyDangKy(Guid id)
        {
            return _service.GetKyDangKy_Json(PublicKey, _token, id);
        }
        public string GetList_ThoiGianDangKy()
        {
            return _service.GetList_ThoiGianDangKy_Json(PublicKey, _token);
        }
        public ActionResult DangKyChamCong_UpdateAll(Guid ky, Guid ca)
        {
            if (_service.DangKyChamCong_UpdateAll(PublicKey, _token, ky, ca))
            {
                return Helper.JsonSucess();
            }

            return Helper.JsonErorr();
        }
        public ActionResult KyDangKy_DeleteList(List<CC_KyDangKyKhungGio> list)
        {
            var strJson = list.ToJSON().Content;
            if (_service.KyDangKy_DeleteList_Json(PublicKey, _token, strJson))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
        public ActionResult ThoiGianDangKy_Save(CC_ThoiGianDangKyKhungGioLamViec obj)
        {
            _service.ThoiGianDangKy_Save(PublicKey, _token, obj);
            return Helper.JsonSucess();
        }
        public ActionResult KyDangKyKhungGio_New(string id, string tenky, DateTime tungay, DateTime denngay)
        {
            Guid oid = Guid.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                oid = new Guid(id);
            }
            if (_service.KyDangKyKhungGio_New(PublicKey, _token, oid, tenky, tungay, denngay))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }
        public ActionResult DangKyKhungGio_CheckNgoaiThoiGian()
        {
            return _service.DangKyKhungGio_CheckNgoaiThoiGian(PublicKey, _token).ToJSON();
        }
        public string DangKyChamCong_Find(Guid? bophan, Guid ky, int trangthai)
        {
            string json = _service.DangKyChamCong_Find_Json(PublicKey, _token, bophan, ky, trangthai);
            return json;
        }
        public ActionResult DangKyKhungGio_GetDuLieuDaDangKy(string nhanvien)
        {

            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(nhanvien))
            {
                idNhanVien = new Guid(nhanvien);
            }
            //
            return _service.DangKyKhungGio_GetDuLieuDaDangKy(PublicKey, _token, idNhanVien).ToJSON();
        }
        public ActionResult KyDangKy_KiemTraTuNgayDenNgayCoHopLe(string Oid, DateTime tuNgay, DateTime denNgay)
        {
            var formatkhaiBaoCongTacOid = Oid == null ? (Guid?)null : new Guid(Oid);
            return _service.KyDangKy_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatkhaiBaoCongTacOid, tuNgay,
                denNgay).ToJSON();
        }
        public ActionResult DangKyKhungGioLamViec_Save(DTO_DangKyKhungGioLamViec obj)
        {
            var strJson = obj.ToJSON().Content;
            _service.DangKyKhungGioLamViec_Save(PublicKey, _token, obj);
            return Helper.JsonSucess();
        }
        public string GetList_KyDangKy()
        {
            return _service.GetList_KyDangKy_Json(PublicKey, _token);
        }
        public string QuanLyChamCong_GetDepartmentsOfUser_All(string userId)
        {
            //
            string result = string.Empty;
            string webGroupId = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId)).WebGroupID.ToString();
            if (!string.IsNullOrEmpty(webGroupId))
            {
                //Lấy groupid user hiện tại
                Guid groupId = new Guid(webGroupId);
                //
                result = _service.GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json_All(PublicKey, _token, new Guid(userId), groupId, _congTy);
            }
            //
            return result;
        }

        public string QuanLyChamCong_GetCompanyListOfUser(string userId)
        {
            //
            string result = string.Empty;
            string webGroupId = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId)).WebGroupID.ToString();
            if (!string.IsNullOrEmpty(webGroupId))
            {

                //
                result = _service.GetList_Truong_DuocPhanQuyenChoWebUserId_Json(PublicKey, _token, new Guid(userId));
            }
            //
            return result;
        }
        public string QuanLyChamCong_GetCompanyListOfUser_New(string userId)
        {
            //
            string result = string.Empty;
            string webGroupId = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId)).WebGroupID.ToString();
            if (!string.IsNullOrEmpty(webGroupId))
            {

                //
                result = _service.GetList_Truong_DuocPhanQuyenChoWebUserId_New_Json(PublicKey, _token, new Guid(userId));
            }
            //
            return result;
        }

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

        public string QuanLyChamCong_BieuDo(int ngay, int thang, int nam, string bophanId)
        {
            string result = string.Empty;
            string userId = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null) return "";
                //
                string webGroupId = webUser.WebGroupID.ToString();
                string idNhanVien = webUser.ThongTinNhanVien.ToString();

                //
                string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                if (webGroupId.ToUpper() == idTaiKhoanCaNhan)
                    result = _service.QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien_Json(PublicKey, _token, ngay, thang, nam, new Guid(idNhanVien));
                else
                    result = _service.QuanLyChamCong_BieuDoVaoRa_Json(PublicKey, _token, ngay, thang, nam, new Guid(bophanId));
            }
            //
            return result;
        }

        public string QuanLyChamCong_GetDepartmentOfStaff()
        {
            string result = "";
            Guid idNhanVien = Guid.Empty;
            //
            if (User.Identity.GetUserId() != null)
            {
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(User.Identity.GetUserId()));
                if (webUser == null)
                    return "";
                //
                if (webUser.ThongTinNhanVien != null)
                    idNhanVien = webUser.ThongTinNhanVien.Value;
            }
            //
            if (idNhanVien != Guid.Empty)
                result = _service.QuanLyChamCong_GetDepartmentOfStaff(idNhanVien);
            //
            return result;
        }
        public ActionResult QuanLyChamCong_CapNhatKhungGioLamViec(List<DTO_QuanLyChamCong_Find> obj, int ngay, int thang, int nam, int loai, string ca)
        {
            DTO_QuanLyChamCong_Find o = obj.FirstOrDefault();
            _service.QuanLyChamCong_CapNhatKhungGioLamViec(PublicKey, _token, o.Oid, ngay, thang, nam, loai, new Guid(ca));
            return Helper.JsonSucess();
        }
        public string QuanLyChamCong_ChamCongThang(int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu)
        {
            string result = string.Empty;
            string userId = User.Identity.GetUserId();
            //
            if (!string.IsNullOrEmpty(userId))
            {
                //
                Guid loaiNhanSuID = Guid.Empty;
                if (!string.IsNullOrEmpty(idLoaiNhanSu))
                {
                    loaiNhanSuID = new Guid(idLoaiNhanSu);
                }
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null)
                    return "";
                //
                string idNhanVien = webUser.ThongTinNhanVien.ToString();
                string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                //
                if (webUser.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan))
                    result = _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien), _congTy);
                else
                    result = _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, loaiNhanSuID, webUser.Oid, _congTy);
            }
            //
            return result;
        }


        public string QuanLyChamCong_ChamCongThang_Save(List<ChamCongThang> chamcongthang, int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu)
        {
            //
            var jsonObject = chamcongthang.ToJSON().Content;

            //Tiến hành cập nhật lại hình thức nghỉ
            _service.QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(PublicKey, _token, jsonObject);

            //Lấy dữ liệu mới nhất lên form
            string result = string.Empty;
            string userId = User.Identity.GetUserId();
            //
            if (!string.IsNullOrEmpty(userId))
            {
                //
                Guid loaiNhanSuID = Guid.Empty;
                if (!string.IsNullOrEmpty(idLoaiNhanSu))
                {
                    loaiNhanSuID = new Guid(idLoaiNhanSu);
                }
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null) return "";
                //
                Guid webGroupId = webUser.WebGroupID.Value;
                string idNhanVien = webUser.ThongTinNhanVien.ToString();
                //
                if (webGroupId == WebGroupConst.TaiKhoanCaNhanID)
                    result = _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien), _congTy);
                else
                    result = _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, loaiNhanSuID, webUser.Oid, _congTy);
            }
            //
            return result;
        }


        public string GetPhongBan_ById(string id)
        {
            return _service.Get_BoPhanBy_Id_Json(PublicKey, _token, new Guid(id));
        }
        public string GetDanhSachBoPhanTheoCongTy(string congTy)
        {
            return _service.GetDanhSachBoPhanTheoCongTy(PublicKey, _token, new Guid(congTy));
        }


        public ActionResult ChotChamCongThang_CheckExists(int thang, int nam, string boPhanId, string congTy)
        {
            //
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.ChotChamCongThang_CheckExists(PublicKey, _token, new Guid(boPhanId), thang, nam, idCongTy).ToJSON();
        }

        public ActionResult ChotChamCongThang_CheckLock(int thang, int nam, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.ChotChamCongThang_CheckLock(PublicKey, _token, thang, nam, idCongTy).ToJSON();
        }
        public ActionResult CaChamCong_CheckDangSuDung(String Oid)
        {
            if (Oid != "" && Oid != null)
            {
                return _service.CaChamCong_CheckDangSuDung(PublicKey, _token, new Guid(Oid)).ToJSON();
            }
            return Helper.JsonSucess();
        }
        public ActionResult CaChamCong_Delete(string Oid)
        {
            if (!string.IsNullOrEmpty(Oid))
            {
                if (_service.CaChamCong_Delete(PublicKey, _token, new Guid(Oid)))
                {
                    return Helper.JsonSucess();
                }
            }
            return Helper.JsonErorr();
        }
        public ActionResult ChotChamCongThang_Create(int thang, int nam, string userId, string boPhanId, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(boPhanId))
            {
                idBoPhan = new Guid(boPhanId);
            }
            //
            if (_service.ChotChamCongThang_Create(PublicKey, _token, idBoPhan, thang, nam, new Guid(userId), idCongTy))
            {
                return Helper.JsonSucess();
            }
            else {
                return Helper.JsonErorr();
            }
        }
        public ActionResult QuanLyNghiPhepNam_Create(Guid nienDoTaiChinh, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            if (_service.QuanLyNghiPhepNam_Create(PublicKey, _token, nienDoTaiChinh, idCongTy))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }
        public ActionResult QuanLyNghiPhepNam_Remove(Guid nienDoTaiChinh, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            if (_service.QuanLyNghiPhepNam_Remove(PublicKey, _token, nienDoTaiChinh, idCongTy))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }
        public ActionResult QuanLyNghiPhepNam_Update(Guid nienDoTaiChinh, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            if (_service.QuanLyNghiPhepNam_Update(PublicKey, _token, nienDoTaiChinh, idCongTy))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }

        public string QuanLyPhepHe_Find(string nienDoTaiChinh, string bophan, string webGroup, string congTy)
        {
            //
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophan))
            {
                idBoPhan = new Guid(bophan);
            }
            Guid idNienDoTaiChinh = Guid.Empty;
            if (!string.IsNullOrEmpty(nienDoTaiChinh))
            {
                idNienDoTaiChinh = new Guid(nienDoTaiChinh);
            }
            //
            string json = string.Empty;
            string userId = User.Identity.GetUserId();
            //
            if (!string.IsNullOrEmpty(userId))
            {
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null) return "";
                //
                string idNhanVien = webUser.ThongTinNhanVien.ToString();
                string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                //
                if (webUser.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan))
                {
                    //
                    json = _service.QuanLyPhepHe_Find_Json(PublicKey, _token, idNienDoTaiChinh, idBoPhan, new Guid(idNhanVien), webUser.Oid, webUser.WebGroupID.Value, idCongTy);
                }
                else
                {
                    //
                    json = _service.QuanLyPhepHe_Find_Json(PublicKey, _token, idNienDoTaiChinh, idBoPhan, Guid.Empty, webUser.Oid, webUser.WebGroupID.Value, idCongTy);
                }
            }
            //
            return json;
        }
        public ActionResult QuanLyPhepHe_ChotPhepHe(Guid nienDoTaiChinh, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            if (_service.QuanLyPhepHe_ChotPhepHe(PublicKey, _token, nienDoTaiChinh, idCongTy))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }

        public ActionResult ChotChamCongThang_Delete(int thang, int nam, string boPhanId, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(boPhanId))
            {
                idBoPhan = new Guid(boPhanId);
            }
            //
            if (_service.ChotChamCongThang_Delete(PublicKey, _token, idBoPhan, thang, nam, idCongTy))
            {
                return Helper.JsonSucess();
            }
            else
            {
                return Helper.JsonErorr(); ;
            }
        }
        public ActionResult DoDuLieuChamCongThang(int thang, int nam, string idHinhThucNghi, string webUserId, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.DoDuLieuChamCongThang(PublicKey, _token, thang, nam, new Guid(idHinhThucNghi), new Guid(webUserId), idCongTy).ToJSON();
        }
        public ActionResult XoaDuLieuChamCongThang(int thang, int nam, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.XoaDuLieuChamCongThang(PublicKey, _token, thang, nam, idCongTy).ToJSON();
        }

        public ActionResult CaChamCong_CheckExists(int thang, int nam, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.CaChamCong_CheckExists(PublicKey, _token, thang, nam, idCongTy).ToJSON();
        }

        public ActionResult CheckDoDuLieuChamCongByThangNam(int thang, int nam, string boPhan, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid boPhanId = boPhan == "" ? Guid.Empty : new Guid(boPhan);
            return _service.ChotChamCongThang_CheckDoDuLieuChamCongByThangNam(PublicKey, _token, thang, nam, boPhanId, idCongTy).ToJSON();
        }

        public ActionResult ChamCongNhanh(int ngay, int thang, int nam, string idHinhThucNghi, string idBoPhan, string idLoaiNhanSu, string webUserId)
        {
            Guid hinhThucNghiID = Guid.Empty;
            if (!string.IsNullOrEmpty(idHinhThucNghi))
                hinhThucNghiID = new Guid(idHinhThucNghi);
            //
            Guid boPhanId = Guid.Empty;
            if (!string.IsNullOrEmpty(idBoPhan))
            {
                boPhanId = new Guid(idBoPhan);
            }
            //
            Guid loaiNhanSuId = Guid.Empty;
            if (!string.IsNullOrEmpty(idLoaiNhanSu))
            {
                loaiNhanSuId = new Guid(idLoaiNhanSu);
            }

            return _service.ChamCongNhanh(PublicKey, _token, ngay, thang, nam, hinhThucNghiID, boPhanId, loaiNhanSuId, new Guid(webUserId)).ToJSON();
        }

        public ActionResult CapNhatChamCongDonVi(int thang, int nam)
        {
            return _service.CapNhatChamCongDonVi(PublicKey, _token, thang, nam).ToJSON();
        }

        public ActionResult QuanLyXetABC_XetVaKhongXetList(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> userList, bool xet)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            return null;// _service.QuanLyXetABC_XetVaKhongXetList_Json(PublicKey, _token, jsonObject, xet).ToJSON();
        }
        public string QuanLyXetABC_Find(int thang, int nam, string bophan, string idLoaiNhanSu, bool? diHoc)
        {
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            string json = "";// _service.QuanLyXetABC_Find_Json(PublicKey, _token, thang, nam, new Guid(bophan), formatLoaiNhanSuId, diHoc);
            return json;
        }

        public ActionResult QuanLyXetABC_Save(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> objList)
        {
            var jsonObject = objList.ToJSON().Content;
            return null;// _service.QuanLyXetABC_SaveList_Json(PublicKey, _token, jsonObject).ToJSON();
        }


        public string QuanLyXetABC_BieuDo(int thang, int nam, string idNhanVien)
        {
            return ""; //_service.QuanLyXetABC_BieuDoVaoRa_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        }


        public string Get_HoSoNhanVienBy_Id(string idNhanVien)
        {
            string json = _service.Get_HoSoNhanVienBy_Id_Json(PublicKey, _token, new Guid(idNhanVien));
            return json;
        }


        public ActionResult QuanLyXetABC_KhoaVaMoKhoaList(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> userList, bool khoa)
        {
            var jsonObject = userList.ToJSON().Content;
            return null; // _service.QuanLyXetABC_KhoaVaMoKhoaList_Json(PublicKey, _token, jsonObject, khoa).ToJSON();
        }


        public ActionResult WebUsers_XoaUsers(List<QuanLyUser> userList)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            return _service.WebUsers_XoaUsers_Json(PublicKey, _token, jsonObject).ToJSON();
        }


        public int CauHinhXetABC_GetThoiGian()
        {
            int thoigian = 0;// _service.CauHinhXetABC_GetThoiGian(PublicKey, _token, new Guid("A75E3EAE-9A56-4495-B1D2-100CC4B6B025"));
            return thoigian;
        }


        public ActionResult CauHinhXetABC_Update(int day)
        {
            return null; // _service.CauHinhXetABC_Update(PublicKey, _token, new Guid("A75E3EAE-9A56-4495-B1D2-100CC4B6B025"), day).ToJSON();
        }


        public string QuanLyXetABC_Detail(int thang, int nam, string idNhanVien)
        {
            return "";// _service.QuanLyXetABC_ChiTietTheoNhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        }


        public string KiemTraPhongBanXetABC_Find(int thang, int nam, bool? daXetXongAbc, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.KiemTraPhongBanXetABC_Find_Json(PublicKey, _token, thang, nam, daXetXongAbc, idCongTy);
        }

        public string ThongKeXetABCTheoNam_Find(int nam, string bophan, string idLoaiNhanSu, string maNhanSu, string webUserId)
        {
            Guid? formatBoPhanId = bophan == "" ? (Guid?)null : new Guid(bophan);
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            string result = string.Empty;
            string userId = User.Identity.GetUserId();
            //
            if (!string.IsNullOrEmpty(userId))
            {
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null) return "";
                //
                string webGroupId = webUser.WebGroupID.ToString();
                string idNhanVien = webUser.ThongTinNhanVien.ToString();
                //
                if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                    result = ""; // _service.ThongKeXetABCTheoNam_Cua1NhanVien_Find_Json(PublicKey, _token, nam, new Guid(idNhanVien));
                else
                    result = ""; // _service.ThongKeXetABCTheoNam_Find_Json(PublicKey, _token, nam, formatBoPhanId, formatLoaiNhanSuId, maNhanSu, new Guid(webUserId));
            }
            //
            return result;
        }

        public string ThongKeXetABCTheoThang_Find(int thang, int nam, string bophan, string idLoaiNhanSu, string maNhanSu, string webUserId)
        {
            Guid? formatBoPhanId = bophan == "" ? (Guid?)null : new Guid(bophan);
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            string userId = User.Identity.GetUserId();
            string result = string.Empty;
            //
            if (!string.IsNullOrEmpty(userId))
            {
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null) return "";
                //
                string webGroupId = webUser.WebGroupID.ToString();
                string idNhanVien = webUser.ThongTinNhanVien.ToString();
                //
                if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                    result = ""; // _service.ThongKeXetABCTheoThang_Cua1NhanVien_Find_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
                else
                    result = ""; // _service.ThongKeXetABCTheoThang_Find_Json(PublicKey, _token, thang, nam, formatBoPhanId, formatLoaiNhanSuId, maNhanSu, new Guid(webUserId));

            }
            //
            return result;
        }
        public ActionResult QuanLyGuiEmail_Delete(List<CC_MailManager> list)
        {
            var jsonObject = list.ToJSON().Content;
            if (_service.QuanLyGuiEmail_DeleteList_Json(PublicKey, _token, jsonObject))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public string QuanLyGuiEmail_Find(DateTime tungay, DateTime denngay, string userid)
        {
            String json = "";
            Guid idWebUser = Guid.Empty;
            if (!string.IsNullOrEmpty(userid))
                idWebUser = new Guid(userid);
            //
            json = _service.QuanLyGuiEmail_Find_Json(PublicKey, _token, tungay.Date, denngay.Date, idWebUser);
            return json;
            //
        }
        public ActionResult QuanLyGuiEmail_SendMail(string emailgui, string passgui, string emailnhan, string tieude, string noidung, string filename, string idwebuser)
        {
            Guid idWebUser = Guid.Empty;
            if (!string.IsNullOrEmpty(idwebuser))
            {
                idWebUser = new Guid(idwebuser);
                //
                if (_service.QuanLyGuiEmail_SendMail(PublicKey, _token, emailgui, passgui, emailnhan, tieude, noidung, filename, idWebUser, _congTy))
                {
                    return Helper.JsonSucess();
                }
            }
            return Helper.JsonErorr();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            //  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }

                    return Helper.JsonSucess();
                }
                catch (Exception ex)
                {
                    return Helper.JsonErorr();
                }
            }
            //
            return Helper.JsonErorr();
        }

        [HttpPost]
        public ActionResult QuanLyKhaiBaoCongTac_DeleteFile(string oidkhaibaocongtac, string fileName)
        {
            if (_service.QuanLyKhaiBaoCongTac_DeleteFile(PublicKey, _token, Server, oidkhaibaocongtac, fileName))
                return Helper.JsonSucess();
            //
            return Helper.JsonErorr();
        }

        [HttpPost]
        public ActionResult QuanLyKhaiBaoCongTac_DownLoadFile(string oidkhaibaocongtac, string fileName)
        {
            if (_service.QuanLyKhaiBaoCongTac_DownLoadFile(PublicKey, _token, Server, oidkhaibaocongtac, fileName))
                return Helper.JsonSucess();
            //
            return Helper.JsonErorr();
        }

        [HttpPost]
        public ActionResult QuanLyKhaiBaoCongTac_UploadFiles(string oidkhaibaocaongtac)
        {
            Guid oidCongTac = Guid.Empty;
            if (!string.IsNullOrEmpty(oidkhaibaocaongtac))
            {
                oidCongTac = new Guid(oidkhaibaocaongtac);
            }
            //  
            if (Request.Files.Count > 0)
            {
                try
                {
                    CC_Attachments_Factory factory = new CC_Attachments_Factory();

                    //
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        //
                        HttpPostedFileBase file = files[i];
                        //
                        CC_Attachments attachment = factory.CreateManagedObject();
                        attachment.Oid = Guid.NewGuid();
                        attachment.Date = DateTime.Now.Date;
                        attachment.FileName = file.FileName;
                        attachment.KhaiBaoCongTac = oidCongTac;
                        //
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();
                        //
                        attachment.Data = data;
                        //
                        factory.SaveChanges();
                    }

                    return Helper.JsonSucess();
                }
                catch (Exception ex)
                {
                    return Helper.JsonErorr();
                }
            }
            //
            return Helper.JsonErorr();
        }
        public string QuanLyKhaiBaoCongTac_Find(DateTime tungay, DateTime denngay, string bophan, int? trangthai, string maNhanSu, string webUserId, string congTy)
        {
            String json = "";
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid? formatBoPhanId = null;
            if (!string.IsNullOrEmpty(bophan))
            {
                formatBoPhanId = new Guid(bophan);
            }
            //
            json = _service.QuanLyKhaiBaoCongTac_Find_Json(PublicKey, _token, tungay, denngay, formatBoPhanId, trangthai, maNhanSu, new Guid(webUserId), idCongTy);
            return json;
        }

        public ActionResult QuanLyKhaiBaoCongTac_Find_NhacViec(DateTime tungay, DateTime denngay, string bophan, int? trangthai, string maNhanSu, string webUserId, string congTy, bool tatCaDonChuaDuyet)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid? formatBoPhanId = null;
            if (!string.IsNullOrEmpty(bophan))
            {
                formatBoPhanId = new Guid(bophan);
            }
            //
            return _service.QuanLyKhaiBaoCongTac_Find_NhacViec(PublicKey, _token, tungay, denngay, formatBoPhanId, trangthai, maNhanSu, new Guid(webUserId), idCongTy, tatCaDonChuaDuyet).ToJSON();
        }


        public ActionResult QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList(List<DTO_QuanLyKhaiBaoCongTac_Find> list, int trangthai, string userId)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = list.ToJSON().Content;
            return _service.QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList_Json(PublicKey, _token, jsonObject, trangthai, userId).ToJSON();
        }


        public ActionResult QuanLyKhaiBaoCongTac_Delete(List<QuanLyCongTac> list)
        {
            var jsonObject = list.ToJSON().Content;
            if (_service.QuanLyKhaiBaoCongTac_DeleteListList_Json(PublicKey, _token, jsonObject))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }



        public string CaNhanKhaiBaoCongTac_Find(DateTime tungay, DateTime denngay, string webUserId)
        {
            return _service.CaNhanKhaiBaoCongTac_Find_Json(PublicKey, _token, tungay, denngay, new Guid(webUserId));
        }
        public string CaNhanKhaiBaoCongTac_Report(string id)
        {
            return _service.CaNhanKhaiBaoCongTac_Report_Json(PublicKey, _token, new Guid(id));
        }
        public ActionResult CaNhanKhaiBaoCongTac_KhaiBaoMoi(string noidung, string diadiem, DateTime tungay, DateTime denngay, byte buoi, int GioBatDau, int PhutBatDau, int GioKetThuc, int PhutKetThuc, string webUserId, string nguoiKy, string IDNhanVien, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            if (_service.CaNhanKhaiBaoCongTac_KhaiBaoMoi(PublicKey, _token, noidung, diadiem, tungay, denngay, buoi, GioBatDau, PhutBatDau, GioKetThuc, PhutKetThuc, new Guid(webUserId), nguoiKy != null ? new Guid(nguoiKy) : Guid.Empty, IDNhanVien, idCongTy))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
        public string KyTinhLuongOfBangLuong()
        {
            return _service.KyTinhLuong_GetAll_Json(PublicKey, _token, _congTy);
        }

        public string KyTinhLuong()
        {
            return _service.KyChamCong_GetAll_Json(PublicKey, _token);
        }
        public string GetYearKyTinhLuong()
        {
            return _service.GetYearKyChamCong_Json(PublicKey, _token);
        }

        public string KyTinhLuong_ByMonthAndYear(string kyChamCong)
        {
            Guid oidKyChamCong = Guid.Empty;
            if (!string.IsNullOrEmpty(kyChamCong))
                oidKyChamCong = new Guid(kyChamCong);
            //
            return _service.KyChamCong_ByID_Json(PublicKey, _token, oidKyChamCong);
        }


        public ActionResult KiemTraKhoaSo_KyTinhLuong(string kyChamCong)
        {
            Guid oidKyChamCong = Guid.Empty;
            if (!string.IsNullOrEmpty(kyChamCong))
                oidKyChamCong = new Guid(kyChamCong);
            //
            return _service.KiemTraKhoaSo_KyChamCong(PublicKey, _token, oidKyChamCong).ToJSON();
        }


        public string ThongTinNhanSu_BANGLUONG(string webUserId, string kyTinhLuong, byte? loaiLuong)
        {
            String json = json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong), _congTy);
            //
            return json;

        }

        public string ThongTinNhanSu_SoYeuLyLich_Json(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_SoYeuLyLich_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ThongTinNhanSu_THONGTINLUONGNHANVIEN(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_HOPDONGLAODONG(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_HOPDONGLAODONG_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_QuanHeGiaDinh(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuanHeGiaDinh_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_DienBienLuong(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_DienBienLuong_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }


        public string ModuleThongTinNhanSu_LichSuBanThan(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_LichSuBanThan_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhDaoTao(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhDaoTao_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhDieuDong(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhDieuDong_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;

        }
        public string ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem(string webUserId)
        {
            String json = json = _service.ModuleThongTinNhanSu_QuaTrinhBoNhiemKiemNhiem_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhBoNhiem(string webUserId)
        {
            String json = json = _service.ModuleThongTinNhanSu_QuaTrinhBoNhiem_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhKhenThuong(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhKhenThuong_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhKyLuat(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhKyLuat_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhHoatDongXaHoi(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;

        }

        public string ModuleThongTinNhanSu_QuaTrinhHoiThao(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_DangVien(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_DangVien_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }


        public string ModuleThongTinNhanSu_DoanVien(string webUserId)
        {
            String json = _service.ModuleThongTinNhanSu_DoanVien_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;
        }

        public string ModuleThongTinNhanSu_CongDoan(string webUserId)
        {
            String json = json = _service.ModuleThongTinNhanSu_CongDoan_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        public string ModuleThongTinNhanSu_QuaTrinhCongTac(string webUserId)
        {
            String json = json = _service.ModuleThongTinNhanSu_QuaTrinhCongTac_Json(PublicKey, _token, new Guid(webUserId));
            //
            return json;

        }
        public string DangKyChamCongNgayNghi_Find(DateTime tungay, DateTime denngay, string idNhanVien)
        {
            Guid nhanVienID = Guid.Empty;
            if (!String.IsNullOrEmpty(idNhanVien))
            {
                nhanVienID = new Guid(idNhanVien);
            }
            return _service.DangKyChamCongNgayNghi_Find_Json(PublicKey, _token, tungay, denngay, nhanVienID);
        }
        public string DangKyNghiPhep_Find(int thang, int nam, string idnhanvien)
        {
            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(idnhanvien))
            {
                idNhanVien = new Guid(idnhanvien);
            }
            return _service.DangKyNghiPhep_Find_Json(PublicKey, _token, thang, nam, idNhanVien);
        }

        public string ChamCongNgayNghi_SoNgayHopLe(string hinhThucNghi, string nhanVien, string congTy, DateTime tuNgay)
        {
            //
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(nhanVien))
            {
                idNhanVien = new Guid(nhanVien);
            } //
            Guid idHinhThucNghi = Guid.Empty;
            if (!string.IsNullOrEmpty(hinhThucNghi))
            {
                idHinhThucNghi = new Guid(hinhThucNghi);
            }

            return _service.ChamCongNgayNghi_SoNgayHopLe_Json(PublicKey, _token, idHinhThucNghi, idNhanVien, idCongTy, tuNgay);
        }

        public string DangKyNghiPhep_SoNgayPhepConLai(int nam, string idnhanvien)
        {
            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(idnhanvien))
            {
                idNhanVien = new Guid(idnhanvien);
            }
            return _service.DangKyNghiPhep_SoNgayPhepConLai_Json(PublicKey, _token, nam, idNhanVien);
        }

        public string ChamCongNgayNghi_Find(DateTime tungay, DateTime denngay, string manhansu, string bophanid, string loainhansuid, int? trangthai, string webUserId, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophanid))
            {
                idBoPhan = new Guid(bophanid);
            }
            Guid idLoaiNhanSu = Guid.Empty;
            if (!string.IsNullOrEmpty(loainhansuid))
            {
                idLoaiNhanSu = new Guid(loainhansuid);
            }
            Guid idWebUser = Guid.Empty;
            if (!string.IsNullOrEmpty(webUserId))
            {
                idWebUser = new Guid(webUserId);
            }
            //
            return _service.ChamCongNgayNghi_Find_Json(PublicKey, _token, tungay, denngay, manhansu, idBoPhan, idLoaiNhanSu, trangthai, idWebUser, idCongTy);
        }
        public ActionResult ChamCongNgayNghi_Find_NhacViec(DateTime tungay, DateTime denngay, string manhansu, string bophanid, string loainhansuid, int? trangthai, string webUserId, string congTy, bool tatCaDonChuaDuyet)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophanid))
            {
                idBoPhan = new Guid(bophanid);
            }
            Guid idLoaiNhanSu = Guid.Empty;
            if (!string.IsNullOrEmpty(loainhansuid))
            {
                idLoaiNhanSu = new Guid(loainhansuid);
            }
            Guid idWebUser = Guid.Empty;
            if (!string.IsNullOrEmpty(webUserId))
            {
                idWebUser = new Guid(webUserId);
            }
            //
            return _service.ChamCongNgayNghi_Find_NhacViec(PublicKey, _token, tungay, denngay, manhansu, idBoPhan, idLoaiNhanSu, trangthai, idWebUser, idCongTy, tatCaDonChuaDuyet).ToJSON();
        }
        public string QuanLyNghiPhep_Find(int thang, int nam, string manhansu, string bophanid, string loainhansuid)
        {
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophanid))
            {
                idBoPhan = new Guid(bophanid);
            }
            Guid idLoaiNhanSu = Guid.Empty;
            if (!string.IsNullOrEmpty(loainhansuid))
            {
                idLoaiNhanSu = new Guid(loainhansuid);
            }
            return _service.QuanLyNghiPhep_Find_Json(PublicKey, _token, thang, nam, manhansu, idBoPhan, idLoaiNhanSu);
        }
        public string ChamCongNgayNghi_Report(string id)
        {
            return _service.ChamCongNgayNghi_Report_Json(PublicKey, _token, new Guid(id));
        }
        public string QuanLyNghiPhepNam_Find(string nienDoTaiChinh, string bophan, string webGroup, string congTy)
        {
            //
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophan))
            {
                idBoPhan = new Guid(bophan);
            }
            Guid idNienDoTaiChinh = Guid.Empty;
            if (!string.IsNullOrEmpty(nienDoTaiChinh))
            {
                idNienDoTaiChinh = new Guid(nienDoTaiChinh);
            }
            //
            string json = string.Empty;
            string userId = User.Identity.GetUserId();
            //
            if (!string.IsNullOrEmpty(userId))
            {
                //
                DTO_WebUser webUser = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(userId));
                if (webUser == null) return "";
                //
                string idNhanVien = webUser.ThongTinNhanVien.ToString();
                string idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                //
                if (webUser.WebGroupID.ToString().ToUpper().Equals(idTaiKhoanCaNhan))
                {
                    //
                    json = _service.QuanLyNghiPhepNam_Find_Json(PublicKey, _token, idNienDoTaiChinh, idBoPhan, new Guid(idNhanVien), webUser.Oid, webUser.WebGroupID.Value, idCongTy);
                }
                else
                {
                    //
                    json = _service.QuanLyNghiPhepNam_Find_Json(PublicKey, _token, idNienDoTaiChinh, idBoPhan, Guid.Empty, webUser.Oid, webUser.WebGroupID.Value, idCongTy);
                }
            }
            //
            return json;
        }
        public string QuanLyViPham_Find(int ngay, int thang, int nam, string boPhanId)
        {
            Guid boPhanGuid = Guid.Empty;
            if (!string.IsNullOrEmpty(boPhanId))
            {
                boPhanGuid = new Guid(boPhanId);
            }
            return _service.QuanLyViPham_Find_Json(PublicKey, _token, ngay, thang, nam, boPhanGuid);
        }
        public string QuanLyViPham_FindXemBangChamCong(int ngay, int thang, int nam, string boPhanId, string webGroup)
        {
            Guid boPhanGuid = Guid.Empty;
            if (boPhanId != "" && boPhanId != null)
            {
                boPhanGuid = new Guid(boPhanId);
            }
            Guid idNhanVien = Guid.Empty;
            if (webGroup == "53D57298-1933-4E4B-B4C8-98AFED036E21")
            {
                idNhanVien = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(User.Identity.GetUserId())).ThongTinNhanVien.Value;
            }
            return _service.QuanLyViPham_FindXemBangChamCong_Json(PublicKey, _token, ngay, thang, nam, boPhanGuid, idNhanVien);
        }
        public ActionResult ChamCongNgayNghi_TaoMoi(string nhanVienId, string noiDung, string nguoibangiao, string tenDonXinNghi, string diaChiLienHe, string idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, string webUserId, int buoi)
        {
            if (_service.ChamCongNgayNghi_TaoMoi(PublicKey, _token, new Guid(nhanVienId), noiDung, nguoibangiao, tenDonXinNghi, diaChiLienHe, new Guid(idHinhThucNghi), loaiDonXinNghi, tuNgay, denNgay, new Guid(webUserId), buoi, _congTy))
                return Helper.JsonSucess();
            else
                return Helper.JsonErorr();

        }
        public ActionResult DangKyNghiPhep_TaoMoi(string nhanVienId, string noiDung, string noiNghiPhep, string tenDonXinNghi, string diaChiLienHe, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, string webUserId)
        {
            if (_service.DangKyNghiPhep_TaoMoi(PublicKey, _token, new Guid(nhanVienId), noiDung, noiNghiPhep, tenDonXinNghi, diaChiLienHe, loaiDonXinNghi, tuNgay, denNgay, new Guid(webUserId)))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
        public ActionResult ChamCongNgayNghi_AcceptList(List<DTO_ChamCongNgayNghi_Find> obj, string userId)
        {
            //
            string strJson = obj.ToJSON().Content;
            //
            if (_service.ChamCongNgayNghi_AcceptList_Json(PublicKey, _token, strJson, new Guid(userId), _congTy))
            {
                return Helper.JsonSucess();
            }
            else
            {
                return Helper.JsonErorr();
            }
        }

        public ActionResult ChamCongNgayNghi_CancelList(List<DTO_ChamCongNgayNghi_Find> obj, string userId)
        {
            string strJson = obj.ToJSON().Content;
            _service.ChamCongNgayNghi_CancelList_Json(PublicKey, _token, strJson, new Guid(userId), _congTy);
            return Helper.JsonSucess();
        }
        public ActionResult DangKyNghiPhep_AcceptList(List<DTO_ChamCongNgayNghi_Find> obj, int isAdmin)
        {
            string strJson = obj.ToJSON().Content;
            _service.DangKyNghiPhep_AcceptList_Json(PublicKey, _token, strJson, isAdmin);
            return Helper.JsonSucess();
        }

        public ActionResult DangKyNghiPhep_CancelList(List<DTO_ChamCongNgayNghi_Find> obj, int isAdmin)
        {
            string strJson = obj.ToJSON().Content;
            _service.DangKyNghiPhep_CancelList_Json(PublicKey, _token, strJson, isAdmin);
            return Helper.JsonSucess();
        }

        public ActionResult DangKyChamCongNgayNghi_DeleteList(List<DTO_ChamCongNgayNghi_Find> obj)
        {
            string strJson = obj.ToJSON().Content;
            if (_service.DangKyChamCongNgayNghi_DeleteList_Json(PublicKey, _token, strJson))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public ActionResult ChamCongNgayNghi_DeleteList(List<DTO_ChamCongNgayNghi_Find> obj)
        {
            string strJson = obj.ToJSON().Content;
            if (_service.ChamCongNgayNghi_DeleteList_Json(PublicKey, _token, strJson))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
        public ActionResult ChamCongNgayNghi_Save(DTO_ChamCongNgayNghi_Find obj)
        {
            var strJson = obj.ToJSON().Content;
            if (_service.ChamCongNgayNghi_Save(PublicKey, _token, obj))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public ActionResult QuanLyNghiPhepNam_Save(string Oid, string TongSoNgayPhep, string SoNgayPhepCongThem, string SoNgayPhepNamTruoc, string SoNgayTamUngHienTai, string SoNgayBuNamHienTai)
        {
            Guid idChiTietNghiPhep = Guid.Empty;
            if (!string.IsNullOrEmpty(Oid))
            {
                idChiTietNghiPhep = new Guid(Oid);
            }
            //
            if (_service.QuanLyNghiPhepNam_SaveList(PublicKey, _token, idChiTietNghiPhep, TongSoNgayPhep, SoNgayPhepCongThem, SoNgayPhepNamTruoc, SoNgayTamUngHienTai, SoNgayBuNamHienTai))
                return Helper.JsonSucess();
            else
                return Helper.JsonErorr();
        }

        public string ChamCongNgayNghi_GetByID(string id)
        {
            return _service.ChamCongNgayNghi_GetByID_Json(PublicKey, _token, new Guid(id));
        }

        public string QuanLyNghiPhepNam_GetByID(string id)
        {
            return _service.QuanLyNghiPhepNam_GetByID_Json(PublicKey, _token, new Guid(id));
        }

        public string HoSoNhanVienBy_MaBoPhan(string maBoPhan, string company)
        {
            var formatBoPhanId = maBoPhan == null ? (Guid?)null : new Guid(maBoPhan);
            //Trường hợp nếu trên form không truyền công ty thì lấy công ty của tài khoản đã lưu trữ
            if (string.IsNullOrEmpty(company))
                company = _congTy.ToString();
            return _service.GetList_HoSoNhanVienBy_MaBoPhan_Json(PublicKey, _token, formatBoPhanId, new Guid(company));
        }


        public ActionResult ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(string chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, string nhanVienID, int buoi)
        {
            var formatchamCongNgayNghiOid = chamCongNgayNghiOid == null ? (Guid?)null : new Guid(chamCongNgayNghiOid);
            return _service.ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatchamCongNgayNghiOid, tuNgay,
                denNgay, new Guid(nhanVienID), buoi).ToJSON();
        }

        public ActionResult CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(string khaiBaoCongTacOid, DateTime tuNgay, DateTime denNgay, string webUserId, string IDNhanVien, int buoi)
        {
            var formatkhaiBaoCongTacOid = khaiBaoCongTacOid == null ? (Guid?)null : new Guid(khaiBaoCongTacOid);
            return _service.CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatkhaiBaoCongTacOid, tuNgay,
                denNgay, new Guid(webUserId), IDNhanVien, buoi).ToJSON();
        }


        public ActionResult CaNhanKhaiBaoCongTac_DeleteList(List<DTO_CC_KhaiBaoCongTac> list)
        {
            //
            var strJson = list.ToJSON().Content;
            if (_service.CaNhanKhaiBaoCongTac_DeleteList_Json(PublicKey, _token, strJson))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public string GiayToHoSo_GetList_ByNhanVienId(string nhanVienId)
        {
            String json = "";
            Guid thongTinNhanVien = nhanVienId == null ? Guid.Empty : new Guid(nhanVienId);
            json = _service.GiayToHoSo_GetList_ByNhanVienId_Json(PublicKey, _token, thongTinNhanVien);
            return json;
        }

        public string UploadFile_GetList_ByNhanVienId(string nhanVienId)
        {
            String json = "";
            Guid thongTinNhanVien = nhanVienId == null ? Guid.Empty : new Guid(nhanVienId);
            json = _service.UploadFile_GetList_ByNhanVienId_Json(PublicKey, _token, thongTinNhanVien);
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
            result["CongTyId"] = Session[SessionKey.CongTyId.ToString()];
            //
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
        public string DangKyChamCongNgoaiGio_Find(int thang, int nam, string idNhanVien)
        {
            Guid nhanVienID = Guid.Empty;
            if (!string.IsNullOrEmpty(idNhanVien))
            {
                nhanVienID = new Guid(idNhanVien);
            }
            //
            return _service.DangKyChamCongNgoaiGio_Find_Json(PublicKey, _token, thang, nam, nhanVienID);
        }
        public ActionResult DangKyChamCongNgoaiGio_DeleteList(List<DTO_CC_ChamCongNgoaiGio> list)
        {
            var strJson = list.ToJSON().Content;
            ///
            return _service.DangKyChamCongNgoaiGio_DeleteList_Json(PublicKey, _token, strJson).ToJSON();
        }
        public string QuanLyChamCongNgoaiGio_Find(int ngay, int thang, int nam, string IDBoPhan, int trangthai, string idwebuser, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid boPhanID = Guid.Empty;
            if (!string.IsNullOrEmpty(IDBoPhan))
            {
                boPhanID = new Guid(IDBoPhan);
            }
            Guid userID = Guid.Empty;
            if (!string.IsNullOrEmpty(idwebuser))
            {
                userID = new Guid(idwebuser);
            }
            return _service.QuanLyChamCongNgoaiGio_Find_Json(PublicKey, _token, ngay, thang, nam, boPhanID, trangthai, userID, idCongTy);
        }
        public ActionResult QuanLyChamCongNgoaiGio_Find_NhacViec(int ngay, int thang, int nam, string IDBoPhan, int trangthai, string idwebuser, string congTy, bool tatCaDonChuaDuyet)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid boPhanID = Guid.Empty;
            if (!string.IsNullOrEmpty(IDBoPhan))
            {
                boPhanID = new Guid(IDBoPhan);
            }
            Guid userID = Guid.Empty;
            if (!string.IsNullOrEmpty(idwebuser))
            {
                userID = new Guid(idwebuser);
            }
            return _service.QuanLyChamCongNgoaiGio_Find_NhacViec(PublicKey, _token, ngay, thang, nam, boPhanID, trangthai, userID, idCongTy, tatCaDonChuaDuyet).ToJSON();
        }
        public ActionResult QuanLyChamCongNgoaiGio_DuyetList(List<DTO_CC_ChamCongNgoaiGio> list, byte trangthai, string idwebuser)
        {
            var strJson = list.ToJSON().Content;
            Guid userID = Guid.Empty;
            if (!string.IsNullOrEmpty(idwebuser))
            {
                userID = new Guid(idwebuser);
            }
            return _service.QuanLyDangKyChamCongNgoaiGio_DuyetList_Json(PublicKey, _token, strJson, trangthai, userID).ToJSON();
        }
        public ActionResult ChotChamCongNgoaiGio_CheckLock(int thang, int nam, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            return _service.ChotChamCongNgoaiGio_CheckLock(PublicKey, _token, thang, nam, idCongTy).ToJSON();
        }
        public ActionResult ChotCongNgoaiGioCCuoiThang(int thang, int nam, string userId, string boPhanId, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            //
            return _service.ChotCongNgoaiGioCCuoiThang(PublicKey, _token, thang, nam, idCongTy).ToJSON();
        }
        public ActionResult XoaChotCongNgoaiGioCCuoiThang(int thang, int nam, string boPhanId, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            //
            return _service.XoaChotCongNgoaiGioCCuoiThang(PublicKey, _token, thang, nam, idCongTy).ToJSON();
        }
        public string ChamCongNgoaiGio_NgayHopLe()
        {
            //
            return _service.ChamCongNgoaiGio_NgayHopLe_Json(PublicKey, _token, _congTy);
        }

        public ActionResult KyChamCong_GetTuNgayDenNgay_ByNgay(DateTime ngay)
        {
            return _service.KyChamCong_GetTuNgayDenNgay_ByNgay(PublicKey, _token, ngay, _congTy).ToJSON();
        }

        public string CaNhanBoSungCong_Find(DateTime tungay, DateTime denngay, string webUserId)
        {
            return _service.CaNhanBoSungCong_Find_Json(PublicKey, _token, tungay, denngay, new Guid(webUserId));
        }

        public ActionResult CaNhanBoSungCong_KiemTraTuNgayDenNgayCoHopLe(string boSungCongOid, DateTime tuNgay, DateTime denNgay, string webUserId, string IDNhanVien, int buoi)
        {
            var formatboSungCongOid = boSungCongOid == null ? (Guid?)null : new Guid(boSungCongOid);
            return _service.CaNhanBoSungCong_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatboSungCongOid, tuNgay,
                denNgay, new Guid(webUserId), IDNhanVien, buoi).ToJSON();
        }

        public ActionResult CaNhanBoSungCong_KhaiBaoMoi(DateTime tungay, DateTime denngay, byte buoi, string lyDo, string webUserId, string IDNhanVien, string congTy)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            if (_service.CaNhanBoSungCong_KhaiBaoMoi(PublicKey, _token, tungay, denngay, buoi, lyDo, new Guid(webUserId), IDNhanVien, idCongTy))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }

        public ActionResult CaNhanBoSungCong_DeleteList(List<DTO_CC_BoSungCong> list)
        {
            //
            var strJson = list.ToJSON().Content;
            if (_service.CaNhanBoSungCong_DeleteList_Json(PublicKey, _token, strJson))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
        public string QuanLyBoSungCong_Find(DateTime tungay, DateTime denngay, string bophan, int? trangthai, string maNhanSu, string webUserId, string congTy)
        {
            String json = "";
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid? formatBoPhanId = null;
            if (!string.IsNullOrEmpty(bophan))
            {
                formatBoPhanId = new Guid(bophan);
            }
            //
            json = _service.QuanLyBoSungCong_Find_Json(PublicKey, _token, tungay, denngay, formatBoPhanId, trangthai, maNhanSu, new Guid(webUserId), idCongTy);
            return json;
        }

        public ActionResult QuanLyBoSungCong_Find_NhacViec(DateTime tungay, DateTime denngay, string bophan, int? trangthai, string maNhanSu, string webUserId, string congTy, bool tatCaDonChuaDuyet)
        {
            Guid idCongTy = _congTy;
            if (!string.IsNullOrEmpty(congTy))
            {
                idCongTy = new Guid(congTy);
            }
            Guid? formatBoPhanId = null;
            if (!string.IsNullOrEmpty(bophan))
            {
                formatBoPhanId = new Guid(bophan);
            }
            //
            return _service.QuanLyBoSungCong_Find_NhacViec(PublicKey, _token, tungay, denngay, formatBoPhanId, trangthai, maNhanSu, new Guid(webUserId), idCongTy, tatCaDonChuaDuyet).ToJSON();
        }
        public ActionResult QuanLyBoSungCong_ThayDoiTrangThaiList(List<DTO_QuanLyBoSungCong_Find> list, int trangthai, string userId)
        {
            var jsonObject = list.ToJSON().Content;
            return _service.QuanLyBoSungCong_ThayDoiTrangThaiList_Json(PublicKey, _token, jsonObject, trangthai, userId).ToJSON();
        }
        public ActionResult QuanLyBoSungCong_Delete(List<QuanLyCongTac> list)
        {
            var jsonObject = list.ToJSON().Content;
            if (_service.QuanLyBoSungCong_DeleteListList_Json(PublicKey, _token, jsonObject))
            {
                return Helper.JsonSucess();
            }
            return Helper.JsonErorr();
        }
    }
}
