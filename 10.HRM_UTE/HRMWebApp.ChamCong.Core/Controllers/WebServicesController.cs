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
        public string DangKyNgayNghi_SoNgayConLai(string idhinhthucnghi,string nam, string iduser)
        {
            Guid idHinhThucNghi = Guid.Empty;
            if (!string.IsNullOrEmpty(idhinhthucnghi))
            {
                idHinhThucNghi = new Guid(idhinhthucnghi);
            }
            //
            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(iduser))
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(iduser), User.Identity.Name).Id);
            }
            int namdangky = Convert.ToInt32(nam);
            //
            return _service.DangKyNgayNghi_SoNgayConLai_Json(PublicKey, _token, idHinhThucNghi, namdangky, idNhanVien);
        }

        public ActionResult QuanLyNghiPhepNam_Save(List<QuanLyNghiPhepNam> objList)
        {
            var jsonObject = objList.ToJSON().Content;
            //
            if(_service.QuanLyNghiPhepNam_SaveList_Json(PublicKey, _token, jsonObject))
                return Helper.JsonSucess();
            else
                return Helper.JsonErorr();
        }

        public string QuanLyNghiPhepNam_Find(int nam, string bophan, string webGroup)
        {
            //
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophan))
            {
                idBoPhan = new Guid(bophan);
            }
            //
            string json = string.Empty;
            Guid idNhanVien = Guid.Empty;
            if (webGroup == "53D57298-1933-4E4B-B4C8-98AFED036E21")
            {
                if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
                {
                    idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id);
                    //
                    json = _service.QuanLyNghiPhepNam_Find_Json(PublicKey, _token, nam, idBoPhan, idNhanVien);
                }
            }
            else
            {
                //
                json = _service.QuanLyNghiPhepNam_Find_Json(PublicKey, _token, nam, idBoPhan, idNhanVien);
            }
            return json;
        }
        public ActionResult QuanLyNghiPhepNam_Update(int nam)
        {
            if (_service.QuanLyNghiPhepNam_Update(PublicKey, _token, nam))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }

        public ActionResult QuanLyNghiPhepNam_Remove(int nam)
        {
            if (_service.QuanLyNghiPhepNam_Remove(PublicKey, _token, nam))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }
        public ActionResult QuanLyNghiPhepNam_Create(int nam)
        {
            if (_service.QuanLyNghiPhepNam_Create(PublicKey, _token, nam))
            {
                return Helper.JsonSucess();
            }
            //
            return Helper.JsonErorr();
        }
        public string QuanLyChamCong_GetDepartmentsOfUser_All(string userId)
        {
            //Lấy groupid user hiện tại
            Guid groupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId.ToString());
            //
            return _service.GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json_All(PublicKey, _token, new Guid(userId), groupId);
        }


        public string GetList_BoPhanWebGroup_GetList()
        {
            return _service.GetList_BoPhan_Json(PublicKey, _token);
        }

        public string QuanLyChamCong_Find(int ngay, int thang, int nam, Guid? bophan, int trangthaichamcong, bool? diHoc, string maNhanSu, string webUserId, Guid? idLoaiNhanSu, int loaicanbo)
        {
            string json = _service.QuanLyChamCong_Find_Json(PublicKey, _token, ngay, thang, nam, bophan, trangthaichamcong, diHoc, maNhanSu, new Guid(webUserId), idLoaiNhanSu, loaicanbo);
            return json;
        }
        public string DangKyChamCong_Find(Guid? bophan, Guid ky, int trangthai,int trangthaidoica)
        {
            bool dadoica = Convert.ToBoolean(trangthaidoica);
            //
            string json = _service.DangKyChamCong_Find_Json(PublicKey, _token,bophan,ky,trangthai, dadoica);
            return json;
        }
        public string ThongKeKhungGioLamViec_Find(string kydangky, string bophan, string manhansu)
        {
            Guid bophanId = Guid.Empty;
            if (!string.IsNullOrEmpty(bophan))
            {
                bophanId = new Guid(bophan);
            }
            Guid kydangkyId = Guid.Empty;
            if (!string.IsNullOrEmpty(kydangky))
            {
                kydangkyId = new Guid(kydangky);
            }

            string json = _service.ThongKeKhungGioLamViec_Find_Json(PublicKey, _token, kydangkyId, bophanId, manhansu);
            return json;
        }
        public ActionResult QuanLyKhungGio_DoiKhungGio(List<DTO_DangKyKhungGioLamViec> obj)
        {
            return _service.QuanLyKhungGio_DoiKhungGio(PublicKey, _token, obj).ToJSON();
        }
        public ActionResult DangKyChamCong_UpdateAll(Guid ky)
        {
           return _service.DangKyChamCong_UpdateAll(PublicKey, _token,ky).ToJSON();
        }
        public ActionResult QuanLyChamCong_CheckChot(int thang, int nam, string boPhanId,int loaicanbo)
        {
            return _service.QuanLyChamCong_CheckChot(PublicKey, _token, new Guid(boPhanId), thang, nam, loaicanbo).ToJSON();     
        }
        public ActionResult QuanLyChamCong_CheckKhoa(int thang, int nam, string boPhanId,int loaicanbo)
        {
            return _service.QuanLyChamCong_CheckKhoa(PublicKey, _token, new Guid(boPhanId), thang, nam, loaicanbo).ToJSON();
        }
        public string QuanLyChamCong_CheckDangKyKhungGio(List<DTO_QuanLyChamCong_Find> obj, int ngay, int thang, int nam)
        {
            DTO_QuanLyChamCong_Find o = obj.FirstOrDefault();
            return _service.QuanLyChamCong_CheckDangKyKhungGio(PublicKey, _token, o.Oid, ngay, thang, nam);
            
        }
        public ActionResult QuanLyChamCong_CheckChotTheoNgay(DateTime ngay, string boPhanId,int loaicanbo)
        {
            return _service.QuanLyChamCong_CheckChot(PublicKey, _token, new Guid(boPhanId), ngay.Month, ngay.Year,loaicanbo).ToJSON();
        }
        public string QuanLyChamCong_GetListHinhThucNghiByLoaiNghi(string loaidonxinnghi)
        {
            int loai = Convert.ToInt32(loaidonxinnghi);
            //
            return _service.GetList_HinhThucNghiByLoaiNghi_Json(PublicKey, _token,loai);
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
        public string QuanLyChamCong_GetListHinhThucNghiForUpdate(int loai)
        {
            return _service.GetList_HinhThucNghiForUpdate_Json(PublicKey, _token, loai);
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
        public ActionResult QuanLyNghiPhep_CheckExists(int nam)
        {
            return _service.QuanLyNghiPhep_CheckExists(PublicKey, _token, nam).ToJSON();
        }
        public ActionResult DangKyKhungGio_GetDuLieuDaDangKy(string nhanvien, Guid ky)
        {

            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(nhanvien))
            {
                idNhanVien = new Guid(nhanvien);
            }
            //
            return _service.DangKyKhungGio_GetDuLieuDaDangKy(PublicKey, _token, idNhanVien, ky).ToJSON();
        }

        public string QuanLyChamCong_BieuDo(int ngay, int thang, int nam, string bophanId,int loaicanbo)
        {          
            string webGroupId= new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien_Json(PublicKey, _token, ngay, thang, nam, new Guid(idNhanVien), loaicanbo);
            return _service.QuanLyChamCong_BieuDoVaoRa_Json(PublicKey, _token, ngay, thang, nam, new Guid(bophanId), loaicanbo);
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
        public string QuanLyChamCong_ChamCongThang(int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu,int loaicanbo)
        {
            string webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id!=null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            //
            Guid loaiNhanSuID = Guid.Empty;
            if (!string.IsNullOrEmpty(idLoaiNhanSu))
            {
                loaiNhanSuID = new Guid(idLoaiNhanSu);
            }

            //Nếu tài khoản thường thì lấy thông tin một người
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
               return _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien), loaicanbo);
            else
                return _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, loaiNhanSuID,loaicanbo);
        }


        public string QuanLyChamCong_ChamCongThang_Save(List<ChamCongThang> chamcongthang, int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu,int loaicanbo)
        {
            //
            var jsonObject = chamcongthang.ToJSON().Content;
            //Tiến hành cập nhật lại hình thức nghỉ
            _service.QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(PublicKey, _token, jsonObject);

            //Lấy dữ liệu mới nhất lên form
            string webGroupId = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).WebGroupId).ToString();
            string idNhanVien = "";
            if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
            {
                idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
            }
            //
            Guid loaiNhanSuID = Guid.Empty;
            if (!string.IsNullOrEmpty(idLoaiNhanSu))
            {
                loaiNhanSuID = new Guid(idLoaiNhanSu);
            }

            //Nếu tài khoản thường
            if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
                return _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien), loaicanbo);
            else
                return _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, loaiNhanSuID, loaicanbo);
        }


        public string GetPhongBan_ById(string id)
        {
            return _service.Get_BoPhanBy_Id_Json(PublicKey, _token, new Guid(id));
        }


        public ActionResult ChotChamCongThang_CheckExists(int thang, int nam, string boPhanId,int loaicanbo)
        {
            return _service.ChotChamCongThang_CheckExists(PublicKey, _token, new Guid(boPhanId), thang, nam, loaicanbo).ToJSON();
        }

        public ActionResult ChotChamCongThang_CheckLock(int thang, int nam)
        {
            return _service.ChotChamCongThang_CheckLock(PublicKey, _token,  thang, nam).ToJSON();
        }
        public ActionResult ChotChamCongThang_ChamCongThangCheckLock(int thang, int nam)
        {
            return _service.ChotChamCongThang_CheckLock(PublicKey, _token, thang, nam).ToJSON();
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
        public ActionResult ChotChamCongThang_Create(int thang, int nam, string userId, string boPhanId,int loaicanbo)
        {
            return _service.ChotChamCongThang_Create(PublicKey, _token, new Guid(boPhanId), thang, nam, new Guid(userId),loaicanbo).ToJSON();
        }
        public ActionResult ChotChamCongThang_ChamCongThangCreate(int thang, int nam, string userId, string boPhanId, int loaicanbo)
        { 
            return _service.ChotChamCongThang_Create(PublicKey, _token, new Guid(boPhanId), thang, nam, new Guid(userId),loaicanbo).ToJSON();
        }
        public ActionResult QuanLyChamCong_ChamCongThang_ChamNhanhCaNgay(int thang, int nam, string boPhanId)
        {
            return _service.QuanLyChamCong_ChamCongThang_ChamNhanhCaNgay(PublicKey, _token,  thang, nam, new Guid(boPhanId)).ToJSON();
        }
        public ActionResult QuanLyChamCong_ChamCongThang_KhoaVaMoKhoa(int thang, int nam, string boPhanId,int loaicanbo,bool loai)
        {
            return _service.QuanLyChamCong_ChamCongThang_KhoaVaMoKhoa(PublicKey, _token, thang, nam, new Guid(boPhanId), loaicanbo,loai).ToJSON();
        }
        public ActionResult ChotChamCongThang_Delete(int thang, int nam, string boPhanId, int loaicanbo)
        {
            return _service.ChotChamCongThang_Delete(PublicKey, _token, new Guid(boPhanId), thang, nam, loaicanbo).ToJSON();
        }
        public ActionResult ChotChamCongThang_ChamCongThangDelete(int thang, int nam, string boPhanId, int loaicanbo)
        {
            return _service.ChotChamCongThang_Delete(PublicKey, _token, new Guid(boPhanId), thang, nam,loaicanbo).ToJSON();
        }

        public ActionResult DoDuLieuChamCongThang(int thang, int nam, string idHinhThucNghi, string webUserId)
        {
            return _service.DoDuLieuChamCongThang(PublicKey, _token, thang, nam, new Guid(idHinhThucNghi), new Guid(webUserId)).ToJSON();
        }


        public ActionResult DoDuLieuChamCongThang_CheckExists(int thang, int nam)
        {
            return _service.DoDuLieuChamCongThang_CheckExists(PublicKey, _token, thang, nam).ToJSON();
        }


        public ActionResult ChamCongNhanh(int ngay, int thang, int nam, string idHinhThucNghi, string idBoPhan, string idLoaiNhanSu, string webUserId)
        {
            var formatHinhThucNghiId = idHinhThucNghi == null ? (Guid?)null : new Guid(idHinhThucNghi);
            var formatBoPhanId = idBoPhan == null ? Guid.Empty : new Guid(idBoPhan);
            var formatLoaiNhanSuId = idLoaiNhanSu == null ? Guid.Empty: new Guid(idLoaiNhanSu);
            //
            return _service.ChamCongNhanh(PublicKey, _token, ngay, thang, nam, formatHinhThucNghiId, formatBoPhanId, formatLoaiNhanSuId, new Guid(webUserId)).ToJSON();
        }

        public ActionResult CapNhatChamCongDonVi(int thang, int nam)
        {
            return _service.CapNhatChamCongDonVi(PublicKey, _token, thang, nam).ToJSON();
        }


        public string QuanLyXetABC_Find(int thang, int nam, string bophan, string idLoaiNhanSu, bool? diHoc)
        {
            Guid? formatLoaiNhanSuId = idLoaiNhanSu == "" ? (Guid?)null : new Guid(idLoaiNhanSu);
            Guid IDBoPhan = (bophan == null || bophan == "") ? Guid.Empty : new Guid(bophan);
            string json = _service.QuanLyXetABC_Find_Json(PublicKey, _token, thang, nam, IDBoPhan, formatLoaiNhanSuId, diHoc);
            return json;
        }

        public ActionResult ThongTinHoSoNhanVien_Save(DTO_Web_UpdateHoSoNhanVien obj)
        {
            var jsonObject = obj.ToJSON().Content;
            //
            Guid userUpdate = new Guid(User.Identity.GetUserId());
            if (userUpdate != null)
            {
                if(_service.ThongTinHoSoNhanVien_Save_Json(PublicKey, _token, jsonObject, userUpdate))
                    return Helper.JsonSucess();
                else
                    return Helper.JsonErorr();

            }
            return Helper.JsonErorr();
        }

        public string UpdateHoSoNhanVien_Find(string idbophan)
        {
            Guid boPhanID = Guid.Empty;
            if (!string.IsNullOrEmpty(idbophan))
            {
                boPhanID = new Guid(idbophan);
            }
            string json = _service.GetListByBoPhan_Web_UpdateHoSoNhanVien_Json(PublicKey, _token, boPhanID);
            return json;
        }

        public string UpdateHoSoNhanVien_FindByOID(string oid)
        {
            Guid oidHoSoNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(oid))
            {
                oidHoSoNhanVien = new Guid(oid);
            }
            string json = _service.GetListByOid_Web_UpdateHoSoNhanVien_Json(PublicKey, _token, oidHoSoNhanVien);
            return json;
        }

        public ActionResult CheckUpdateThongTinVaoHRM(string oidnhanvien)
        {
            Guid oidNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(oidnhanvien))
            {
                oidNhanVien = new Guid(oidnhanvien);
            }
            if(_service.GetListByOidNhanVien_Web_UpdateHoSoNhanVien(PublicKey, _token, oidNhanVien))
                return Helper.JsonExists();
            else
                return Helper.JsonErorr();
        }

        public ActionResult UpdateHoSoNhanVien_Save(List<DTO_Web_UpdateHoSoNhanVien> obj)
        {
            var jsonObject = obj.ToJSON().Content;
            Guid userUpdate = new Guid(User.Identity.GetUserId());
            if (userUpdate != null)
            {
                if(_service.UpdateHoSoNhanSu_SaveList_Json(PublicKey, _token, jsonObject, userUpdate))
                    return Helper.JsonSucess();
                else
                    return Helper.JsonErorr();
            }
            return Helper.JsonErorr();
        }

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


        public ActionResult QuanLyXetABC_KhoaVaMoKhoaList(int thang, int nam, string bophan, bool khoa)
        {
            Guid idBoPhan = Guid.Empty;
            if (!string.IsNullOrEmpty(bophan))
                idBoPhan = new Guid(bophan);

            return _service.QuanLyXetABC_KhoaVaMoKhoaList_Json(PublicKey, _token, thang,nam,idBoPhan, khoa).ToJSON();
        }

        public ActionResult QuanLyXetABC_XetVaKhongXetList(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> userList, bool xet)
        {
            //var js = new JavaScriptSerializer();
            var jsonObject = userList.ToJSON().Content;
            return _service.QuanLyXetABC_XetVaKhongXetList_Json(PublicKey, _token, jsonObject, xet).ToJSON();
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
            String json = string.Empty;
            Guid boPhanID= Guid.Empty;
            if (!string.IsNullOrEmpty(bophan))
            {
                boPhanID = new Guid(bophan);
            }
            json = _service.QuanLyKhaiBaoCongTac_Find_Json(PublicKey, _token, thang, nam, boPhanID, trangthai, maNhanSu, new Guid(webUserId));
            //
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



        public string CaNhanKhaiBaoCongTac_Find(int thang, int nam, string idnhanvien)
        {
            Guid nhanVienID = Guid.Empty;
            if (!string.IsNullOrEmpty(idnhanvien))
            {
                nhanVienID = new Guid(idnhanvien);
            }
            return _service.CaNhanKhaiBaoCongTac_Find_Json(PublicKey, _token, thang, nam, nhanVienID);
        }
        public string CaNhanDangKyChamCongNgoaiGio_Find(int thang, int nam, string idNhanVien)
        {
            Guid nhanVienID = Guid.Empty;
            if (!string.IsNullOrEmpty(idNhanVien))
            {
                nhanVienID = new Guid(idNhanVien);
            }
            //
            return _service.CaNhanDangKyChamCongNgoaiGio_Find_Json(PublicKey, _token, thang, nam, nhanVienID);
        }
        public string QuanLyDangKyChamCongNgoaiGio_Find(int? ngay, int thang, int nam,string IDBoPhan,byte? trangthai)
        {
            Guid boPhanID = Guid.Empty;
            if (!string.IsNullOrEmpty(IDBoPhan))
            {
                boPhanID = new Guid(IDBoPhan);
            }
            return _service.QuanLyDangKyChamCongNgoaiGio_Find_Json(PublicKey, _token,ngay, thang, nam, boPhanID, trangthai);
        }
        public ActionResult CaNhanKhaiBaoCongTac_KhaiBaoMoi(string noidung,string diadiem, DateTime tungay, DateTime denngay,byte buoi, int GioBatDau,int PhutBatDau,int GioKetThuc, int PhutKetThuc, string webUserId)
        {
            _service.CaNhanKhaiBaoCongTac_KhaiBaoMoi(PublicKey, _token, noidung,diadiem, tungay, denngay,buoi, GioBatDau,PhutBatDau,GioKetThuc,PhutKetThuc, new Guid(webUserId));
            return Helper.JsonSucess();
        }
        public ActionResult KyDangKyKhungGio_New(string id, string tenky, DateTime tungay, DateTime denngay)
        {
            Guid oid = Guid.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                oid = new Guid(id);
            }
            _service.KyDangKyKhungGio_New(PublicKey, _token, oid, tenky, tungay, denngay);
            //
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
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong), loaiLuong);

            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong));
            }
            return json;

        }


        public string ThongTinNhanSu_SoYeuLyLich_Json(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_SoYeuLyLich_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_SoYeuLyLich_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_SoYeuLyLich_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;


        }


        public string ThongTinNhanSu_THONGTINLUONGNHANVIEN(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_THONGTINLUONGNHANVIEN_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }          
            return json;

        }


        public string ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_TRINHDOCHUYENMONNHANVIEN_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_HOPDONGLAODONG(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_HOPDONGLAODONG_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_HOPDONGLAODONG_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_HOPDONGLAODONG_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;
        }


        public string ModuleThongTinNhanSu_QuanHeGiaDinh(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuanHeGiaDinh_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuanHeGiaDinh_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuanHeGiaDinh_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }



        public string ModuleThongTinNhanSu_DienBienLuong(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_DienBienLuong_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_DienBienLuong_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_DienBienLuong_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_LichSuBanThan(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_LichSuBanThan_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_LichSuBanThan_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_LichSuBanThan_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhDaoTao(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhDaoTao_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhDaoTao_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhDaoTao_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhBoiDuong(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhBoiDuong_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhBoiDuong_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhBoiDuong_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhBoNhiem(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhBoNhiem_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhBoNhiem_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhBoNhiem_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhDiNuocNgoai_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhKhenThuong(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhKhenThuong_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhKhenThuong_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhKhenThuong_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }



        public string ModuleThongTinNhanSu_QuaTrinhKyLuat(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhKyLuat_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhKyLuat_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhKyLuat_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhNghienCuuKhoaHoc_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhHoatDongXaHoi(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoatDongXaHoi_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhHoiThao(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaHoiThao_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhThamGiaLucLuongVuTrang_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
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
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_DangVien_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_DangVien_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_DangVien_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_DoanVien(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_DoanVien_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_DoanVien_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_DoanVien_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_CongDoan(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_CongDoan_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_CongDoan_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_CongDoan_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_QuaTrinhCongTac(string webUserId)
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhCongTac_IUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhCongTac_LUH_Json(PublicKey, _token, new Guid(webUserId));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_QuaTrinhCongTac_UTE_Json(PublicKey, _token, new Guid(webUserId));
            }
            return json;

        }



        public string DangKyChamCongNgayNghi_Find(int thang, int nam, string idNhanVien)
        {
            Guid nhanVienID = Guid.Empty;
            if (!String.IsNullOrEmpty(idNhanVien))
            {
                nhanVienID = new Guid(idNhanVien);
            }
            return _service.DangKyChamCongNgayNghi_Find_Json(PublicKey, _token, thang, nam, nhanVienID);
        }
        public string NgayNghiTrongNam_Find(int nam)
        {
            return _service.NgayNghiTrongNam_Find_Json(PublicKey, _token, nam);
        }
        public ActionResult NgayNghiTrongNam_Save(String TenNgayNghi, DateTime NgayNghi,bool NghiBu, int Nam)
        {        
            _service.NgayNghiTrongNam_Save(PublicKey, _token,TenNgayNghi,NgayNghi,NghiBu,Nam);
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
        public string ChamCongNgayNghi_Find(int ngay, int thang, int nam, Guid? boPhanId, string maNhanSu, string webUserId, Guid idLoaiNhanSu, int trangthai, bool? isAdmin)
        {
            //
            return _service.ChamCongNgayNghi_Find_Json(PublicKey, _token, thang, nam, boPhanId, maNhanSu, new Guid(webUserId), idLoaiNhanSu, trangthai, isAdmin);
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
        public string QuanLyViPham_FindXemBangChamCong(int ngay, int thang, int nam, string boPhanId,string webGroup)
        {
            Guid boPhanGuid = Guid.Empty;
            if (boPhanId != "" && boPhanId != null)
            {
                boPhanGuid = new Guid(boPhanId);
            }
            Guid idNhanVien = Guid.Empty;
            if (webGroup== "53D57298-1933-4E4B-B4C8-98AFED036E21")
            {
                if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
                {
                    idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id);
                }
            }
            return _service.QuanLyViPham_FindXemBangChamCong_Json(PublicKey, _token, ngay, thang, nam, boPhanGuid, idNhanVien);
        }
        public ActionResult QuanLyViPham_GuiMail()
        {
            _service.QuanLyViPham_GuiMail(PublicKey, _token);
            return Helper.JsonSucess();
        }

        public ActionResult ChamCongNgayNghi_TaoMoi(string nhanVienId, string noiDung, string noiNghiPhep,  string diaChiLienHe, string idHinhThucNghi, int loaiDonXinNghi, DateTime tuNgay, DateTime denNgay, string webUserId, bool isAdmin)
        {
            _service.ChamCongNgayNghi_TaoMoi(PublicKey, _token, new Guid(nhanVienId), noiDung, noiNghiPhep, diaChiLienHe, idHinhThucNghi, loaiDonXinNghi, tuNgay, denNgay, new Guid(webUserId), isAdmin);
            return Helper.JsonSucess();
        }
        public string ChamCongNgayNghi_Report(string id)
        {
            return _service.ChamCongNgayNghi_Report_Json(PublicKey, _token, new Guid(id));
        }

        public ActionResult UpdateHoSoNhanVien_DeleteList(List<DTO_Web_UpdateHoSoNhanVien> obj)
        {
            if (_service.UpdateHoSoNhanVien_DeleteList(PublicKey, _token, obj))
            {
                return Helper.JsonSucess();
            }

            return Helper.JsonErorr();
        }

        public ActionResult ChamCongNgayNghi_DeleteList(List<DTO_ChamCongNgayNghi_Find> obj)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            string strJson = obj.ToJSON().Content;
            _service.ChamCongNgayNghi_DeleteList_Json(PublicKey, _token, strJson);
            return Helper.JsonSucess();
        }
        public ActionResult QuanLyChamCong_CapNhatKhungGioLamViec(List<DTO_QuanLyChamCong_Find> obj, int ngay, int thang,int nam,int loai,string ca)
        {
            DTO_QuanLyChamCong_Find o = obj.FirstOrDefault();
            _service.QuanLyChamCong_CapNhatKhungGioLamViec(PublicKey, _token, o.Oid,ngay,thang,nam,loai,new Guid(ca));
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

        public string GetHoSoNhanVienByOidNhanVien(string oidnhanvien)
        {
            Guid idNhanVien = Guid.Empty;
            if (!string.IsNullOrEmpty(oidnhanvien))
            {
                idNhanVien = new Guid(oidnhanvien);
            }

            return _service.GetHoSoNhanVienByOidNhanVien_Json(PublicKey, _token, idNhanVien);
        }

        public string HoSoNhanVienBy_MaBoPhan(string maBoPhan)
        {
            var formatBoPhanId = maBoPhan == null ? (Guid?)null : new Guid(maBoPhan);
            return _service.GetList_HoSoNhanVienBy_MaBoPhan_Json(PublicKey, _token, formatBoPhanId);
        }

        public string GetDanTocAll()
        {
            return _service.GetListDanTocALL_Json(PublicKey, _token);
        }

        public ActionResult GetDanhMucUpDateHoSoNhanVienAll()
        {
            Dictionary<string, string> result = new Dictionary<string, string>(); 
            
            //Tình trạng hôn nhân
            result["TinhTrangHonNhan"] = _service.GetListTinhTrangHonNhanALL_Json(PublicKey, _token);

            //Dân tộc
            result["DanToc"] = _service.GetListDanTocALL_Json(PublicKey, _token);

            //Tôn giáo
            result["TonGiao"] = _service.GetListTonGiaoALL_Json(PublicKey, _token);

            //Quốc gia
            result["QuocGia"] = _service.GetListQuocGiaALL_Json(PublicKey, _token);

            //Tỉnh thành
            result["TinhThanh"] = _service.GetListTinhThanhALL_Json(PublicKey, _token);

            //Quận huyện
            result["QuanHuyen"] = _service.GetListQuanHuyenALL_Json(PublicKey, _token);

            //Xã phường
            result["XaPhuong"] = _service.GetListXaPhuongALL_Json(PublicKey, _token);

            //Trình độ văn hóa
            result["TrinhDoVanHoa"] = _service.GetListTrinhDoVanHoaALL_Json(PublicKey, _token);

            //Trình độ chuyên môn
            result["TrinhDoChuyenMon"] = _service.GetListTrinhDoChuyenMonALL_Json(PublicKey, _token);

            //Chuyên ngành đào tạo
            result["ChuyenNganhDaoTao"] = _service.GetListChuyenNganhDaoTaoALL_Json(PublicKey, _token);

            //Trường đào tạo
            result["TruongDaoTao"] = _service.GetListTruongDaoTaoALL_Json(PublicKey, _token);

            //Ngoại ngữ
            result["NgoaiNgu"] = _service.GetListNgoaiNguALL_Json(PublicKey, _token);

            //Trình độ ngoại ngữ
            result["TrinhDoNgoaiNgu"] = _service.GetListTrinhDoNgoaiNguALL_Json(PublicKey, _token);


            return Json(result);
        }
        public string CauHinhChamCong_Find()
        {
            return _service.CauHinhChamCong_Find_Json(PublicKey, _token);
        }
        public string GetList_HoSoNhanVienCoIDChamCong()
        {
            return _service.GetList_HoSoNhanVienCoIDChamCong_Json(PublicKey, _token);
        }
        public ActionResult CauHinhChamCong_Save(string oid,  string songaynghiphep,string dongiangoaigio)
        {
            Guid oidCauHinh = Guid.Empty;
            if (!string.IsNullOrEmpty(oid))
            {
                oidCauHinh = new Guid(oid);
            }
            //
            if (_service.CauHinhChamCong_Save(PublicKey, _token, oidCauHinh, songaynghiphep, dongiangoaigio))
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

        public string GetTonGiaoAll()
        {
            return _service.GetListTonGiaoALL_Json(PublicKey, _token);
        }
        public string GetTinhTrangHonNhanAll()
        {
            return _service.GetListTinhTrangHonNhanALL_Json(PublicKey, _token);
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

        public string GetQuocGiaAll()
        {
            return _service.GetListQuocGiaALL_Json(PublicKey, _token);
        }
        public string GetTinhThanhAll()
        {
            return _service.GetListTinhThanhALL_Json(PublicKey, _token);
        }
        public string GetQuanHuyenAll()
        {
            return _service.GetListQuanHuyenALL_Json(PublicKey, _token);
        }
        public string GetTrinhDoVanHoaAll()
        {
            return _service.GetListTrinhDoVanHoaALL_Json(PublicKey, _token);
        }

        public string GetTrinhDoChuyenMonAll()
        {
            return _service.GetListTrinhDoChuyenMonALL_Json(PublicKey, _token);
        }

        public string GetChuyenNganhDaoTaoAll()
        {
            return _service.GetListChuyenNganhDaoTaoALL_Json(PublicKey, _token);
        }
        public string GetTruongDaoTaoAll()
        {
            return _service.GetListTruongDaoTaoALL_Json(PublicKey, _token);
        }
        public string GetNgoaiNguAll()
        {
            return _service.GetListNgoaiNguALL_Json(PublicKey, _token);
        }
        public string GetTrinhDoNgoaiNguAll()
        {
            return _service.GetListTrinhDoNgoaiNguALL_Json(PublicKey, _token);
        }

        public ActionResult ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(string chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, string nhanVienID)
        {
            var oidChamCongNgayNghi = Guid.Empty;
            if (!string.IsNullOrEmpty(chamCongNgayNghiOid))
            {
                oidChamCongNgayNghi = new Guid(chamCongNgayNghiOid);
            }
            return _service.ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, oidChamCongNgayNghi, tuNgay,denNgay, new Guid(nhanVienID)).ToJSON();
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
        public ActionResult QuanLyDangKyChamCongNgoaiGio_DuyetList(List<DTO_CC_DangKyChamCongNgoaiGio> list, byte trangthai,bool isadmin)
        {
            //var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strJson = list.ToJSON().Content;
            return _service.QuanLyDangKyChamCongNgoaiGio_DuyetList_Json(PublicKey, _token, strJson,trangthai, isadmin).ToJSON();
        }
        public ActionResult ChamCongNgoaiGio_XuatMauDangKy(int thang,int nam)
        {
            string result = _service.ChamCongNgoaiGio_XuatMauDangKy(PublicKey, _token,  thang,  nam);
            //
            return Helper.JsonResult(result);
        }
        public ActionResult ChamCongNgoaiGio_XuatMauTongHop(int thang, int nam,Guid bophan)
        {
            string result = _service.ChamCongNgoaiGio_XuatMauTongHop(PublicKey, _token, thang, nam, bophan);
            //
            return Helper.JsonResult(result);
        }

        public string GiayToHoSo_GetList_ByNhanVienId(string nhanVienId)
        {
            String json = "";
            Guid thongTinNhanVien = nhanVienId == null ? Guid.Empty : new Guid(nhanVienId);
            json = _service.GiayToHoSo_GetList_ByNhanVienId_Json(PublicKey, _token, thongTinNhanVien);
            return json;
        }


        public string ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang()
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                string idNhanVien = "";
                if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
                {
                    idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
                }
                json = _service.ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang_IUH_Json(PublicKey, _token, new Guid(idNhanVien));
            }
            return json;

        }


        public string ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong()
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                string idNhanVien = "";
                if (AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id != null)
                {
                    idNhanVien = new Guid(AuthenticationHelper.GetUserById(new Guid(User.Identity.GetUserId()), User.Identity.Name).Id).ToString();
                }
                json = _service.ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong_IUH_Json(PublicKey, _token, new Guid(idNhanVien));
            }
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
