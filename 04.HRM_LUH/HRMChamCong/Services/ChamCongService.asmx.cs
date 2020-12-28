using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services.Protocols;
using HRMChamCong.DTO;
using HRMChamCong.Helper;

using HRMWeb_Service.Utils;
using System;
using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using Microsoft.SqlServer.Server;
using HRMWeb_Service;
using HRMWeb_Business.Model;
using HRMChamCong.Utility;

namespace HRMChamCong.Services
{
    /// <summary>
    /// Summary description for ChamCongService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ChamCongService : System.Web.Services.WebService
    {
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();  //readonly Service1Client _service = new Service1Client();

        public static String DocMaDonViSuDung()
        {
            return ConfigurationUtil.ReadAppSetting("MaDonViSuDung");
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string GetList_QuanLyUser()
        {
            return _service.GetList_WebUser_Json(PublicKey, _token); ;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string GetList_QuanLyUserQuanTri()
        {
            return _service.GetList_WebUserQuanTri_Json(PublicKey, _token); ;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string GetList_QuanLyUserKhacQuanTri()
        {
            return _service.GetList_WebUserKhacQuanTri_Json(PublicKey, _token); ;
        }


        [WebMethod]
        public string GetDetail_QuanLyUser(string id)
        {
            var obj = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(id));
            var js = new JavaScriptSerializer();
            return js.Serialize(obj);
        }

        [WebMethod]
        public void Save_QuanLyUser(QuanLyUser obj)
        {
            var js = new JavaScriptSerializer();
            var jsonObject = js.Serialize(obj);
            _service.Save_WebUser_Json(PublicKey, _token, jsonObject);
        }

        [WebMethod]
        public bool WebUsers_KiemTraTrungUsername(string webUserId, string userName)
        {
            Guid? formatwebUserId = webUserId == null ? (Guid?)null : new Guid(webUserId);
            return _service.WebUsers_KiemTraTrungUsername(PublicKey, _token, formatwebUserId, userName);
        }

        [WebMethod]
        public void ChangePassword_WebUser(string webUserId, string passWord)
        {
            _service.ChangePassword_WebUser(PublicKey, _token, new Guid(webUserId), passWord);
        }

        [WebMethod(EnableSession = true)]
        public void LogOut_WebUser()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
        }

        [WebMethod]
        public string WebMenu_GetListBy_WebUserId(string webUserId)
        {
            if (String.IsNullOrWhiteSpace(webUserId))
                return null;
            string json=_service.WebMenu_GetListTop2LevelDeepBy_WebUserId_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        [WebMethod]
        public string WebMenu_GetURLListBy_WebUserId(string webUserId)
        {
            if (String.IsNullOrWhiteSpace(webUserId))
                return null;
            string json = _service.WebMenu_GetURLListBy_WebUserId_Json(PublicKey, _token, new Guid(webUserId));
            return json;
        }

        [WebMethod]
        public string WebMenu_GetChildMenuListBy_WebUserId_AndMenuId(string webUserId, string menuId)
        {
            return _service.WebMenu_GetChildMenuListBy_WebUserId_AndMenuId_Json(PublicKey, _token, new Guid(webUserId), new Guid(menuId));
        }

        [WebMethod]
        public string WebGroup_GetList()
        {
            return _service.WebGroup_GetList_Json(PublicKey, _token);
        }

        [WebMethod(EnableSession = true)]
        public int CheckForLogin_WebUser(string userName, string passWord, string captchaString)
        {
            if (HttpContext.Current.Session[SessionKey.CaptchaImage.ToString()].ToString().ToUpper() != captchaString.ToUpper() && captchaString != "")
                return 0;
            var user = _service.CheckForLogin_WebUser(PublicKey, _token, userName, passWord);
            if (user != null)
            {
                var authTicket = new FormsAuthenticationTicket(userName, true, 15);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
                SessionHelper.Data(SessionKey.UserId, user.Oid);
                SessionHelper.Data(SessionKey.ThongTinNhanVien, user.ThongTinNhanVien);
                SessionHelper.Data(SessionKey.UserName, user.UserName);
                SessionHelper.Data(SessionKey.WebGroupId, user.WebGroupID);
                return 1;
            }
            return 2;
        }

        [WebMethod]
        public string QuanLyChamCong_GetDepartmentsOfUser(string userId)
        {
            return _service.GetList_BoPhan_DuocPhanQuyenChoWebUserId_Json(PublicKey, _token, new Guid(userId));
        }

        [WebMethod]
        public string GetList_BoPhanWebGroup_GetList()
        {
            return _service.GetList_BoPhan_Json(PublicKey, _token);
        }

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string QuanLyChamCong_Find(int ngay, int thang, int nam, string bophan, int trangthaichamcong, bool? diHoc, string maNhanSu, string webUserId, string idLoaiNhanSu)
        //{
        //    Guid? formatBoPhanId = bophan == null ? (Guid?)null : new Guid(bophan);
        //    Guid? formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    string json=_service.QuanLyChamCong_Find_Json(PublicKey, _token, ngay, thang, nam, formatBoPhanId, trangthaichamcong, diHoc, maNhanSu, new Guid(webUserId), formatLoaiNhanSuId);
        //    return json;
        //}

        //[WebMethod]
        //public string QuanLyChamCong_GetListHinhThucNghi()
        //{
        //    return _service.GetList_HinhThucNghi_Json(PublicKey, _token);
        //}

        [WebMethod]
        public string GetList_LoaiNhanSu()
        {
            return _service.GetList_LoaiNhanSu_Json(PublicKey, _token);
        }

        //[WebMethod]
        //public void QuanLyChamCong_Save(List<UserSave> userList)
        //{
        //    var js = new JavaScriptSerializer();
        //    var jsonObject = js.Serialize(userList);
        //    _service.QuanLyChamCong_SaveList_Json(PublicKey, _token, jsonObject);
        //}

        //[WebMethod(EnableSession = true)]
        //public string QuanLyChamCong_BieuDo(int ngay, int thang, int nam, string bophanId)
        //{
        //    string webGroupId = HttpContext.Current.Session[SessionKey.WebGroupId.ToString()].ToString();
        //    string idNhanVien = HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()].ToString();
        //    if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
        //        return _service.QuanLyChamCong_BieuDoVaoRa_Cua1NhanVien_Json(PublicKey, _token, ngay, thang, nam,
        //            new Guid(idNhanVien));
        //    return _service.QuanLyChamCong_BieuDoVaoRa_Json(PublicKey, _token, ngay, thang, nam, new Guid(bophanId));
        //}

        //[WebMethod(EnableSession = true)]
        //public string QuanLyChamCong_ChamCongThang(int thang, int nam, string bophanId, string maNhanSu, string idLoaiNhanSu)
        //{
        //    string webGroupId = HttpContext.Current.Session[SessionKey.WebGroupId.ToString()].ToString();
        //    string idNhanVien = HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()].ToString();
        //    Guid? formatLoaiNhanSuId = (idLoaiNhanSu == null || idLoaiNhanSu == "null") ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
        //        return _service.QuanLyChamCong_ThongTinChamCongThang_Cua1NhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        //    else
        //        return _service.QuanLyChamCong_ThongTinChamCongThang_Json(PublicKey, _token, thang, nam, new Guid(bophanId), maNhanSu, formatLoaiNhanSuId);
        //}

        //[WebMethod]
        //public void QuanLyChamCong_ChamCongThang_Save(List<ChamCongThang> chamcongthang)
        //{
        //    var js = new JavaScriptSerializer();
        //    var jsonObject = js.Serialize(chamcongthang);
        //    _service.QuanLyChamCong_ThongTinChamCongThang_SaveList_Json(PublicKey, _token, jsonObject);
        //}

        [WebMethod]
        public string GetPhongBan_ById(string id)
        {
            return _service.Get_BoPhanBy_Id_Json(PublicKey, _token, new Guid(id));
        }

        //[WebMethod]
        //public bool ChotChamCongThang_CheckExists(int thang, int nam, string boPhanId)
        //{
        //    return _service.ChotChamCongThang_CheckExists(PublicKey, _token, new Guid(boPhanId), thang, nam);
        //}

        //[WebMethod]
        //public bool ChotChamCongThang_Create(int thang, int nam, string userId, string boPhanId)
        //{
        //    return _service.ChotChamCongThang_Create(PublicKey, _token, new Guid(boPhanId), thang, nam, new Guid(userId));
        //}

        //[WebMethod]
        //public bool ChotChamCongThang_Delete(int thang, int nam, string boPhanId)
        //{
        //    return _service.ChotChamCongThang_Delete(PublicKey, _token, new Guid(boPhanId), thang, nam);
        //}

        //[WebMethod]
        //public bool DoDuLieuChamCongThang(int thang, int nam, string idHinhThucNghi, string webUserId)
        //{
        //    return _service.DoDuLieuChamCongThang(PublicKey, _token, thang, nam, new Guid(idHinhThucNghi), new Guid(webUserId));
        //}

        //[WebMethod]
        //public bool DoDuLieuChamCongThang_CheckExists(int thang, int nam)
        //{
        //    return _service.DoDuLieuChamCongThang_CheckExists(PublicKey, _token, thang, nam);
        //}

        //[WebMethod]
        //public bool ChamCongNhanh(int ngay, int thang, int nam, string idHinhThucNghi, string idBoPhan, string idLoaiNhanSu, string webUserId)
        //{
        //    var formatHinhThucNghiId = idHinhThucNghi == null ? (Guid?)null : new Guid(idHinhThucNghi);
        //    var formatBoPhanId = idBoPhan == null ? (Guid?)null : new Guid(idBoPhan);
        //    var formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    return _service.ChamCongNhanh(PublicKey, _token, ngay, thang, nam, formatHinhThucNghiId, formatBoPhanId, formatLoaiNhanSuId, new Guid(webUserId));
        //}

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string QuanLyXetABC_Find(int thang, int nam, string bophan, string idLoaiNhanSu, bool? diHoc)
        //{
        //    string ngayNghiFormat = "<div  class='jqx-grid-cell-wrap'>";
        //    ngayNghiFormat += "1/2 : <input class='jqx-widget-content jqx-input jqx-widget jqx-rc-all' type='text' value='{0}'  style='width:15px;height:15px;text-align:center;' />&nbsp";
        //    ngayNghiFormat += "P: <input class='jqx-widget-content jqx-input jqx-widget jqx-rc-all' type='text' value='{1}'  style='width:15px;height:15px;text-align:center;' />&nbsp";
        //    ngayNghiFormat += "Ro: <input class='jqx-widget-content jqx-input jqx-widget jqx-rc-all' type='text' value='{2}'  style='width:15px;height:15px;text-align:center;' /><br>";
        //    ngayNghiFormat += "TS: <input class='jqx-widget-content jqx-input jqx-widget jqx-rc-all' type='text' value='{3}'  style='width:15px;height:15px;text-align:center;' />&nbsp";
        //    ngayNghiFormat += "H: <input class='jqx-widget-content jqx-input jqx-widget jqx-rc-all' type='text' value='{4}'  style='width:15px;height:15px;text-align:center;' />";
        //    ngayNghiFormat += "</div>";
            
        //    Guid? formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    string json = _service.QuanLyXetABC_Find_Json(PublicKey, _token, thang, nam, new Guid(bophan), formatLoaiNhanSuId, diHoc, ngayNghiFormat);
        //    return json;
        //}

        //[WebMethod]
        //public bool QuanLyXetABC_Save(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> objList)
        //{
        //    var js = new JavaScriptSerializer();
        //    var jsonObject = js.Serialize(objList);
        //    return _service.QuanLyXetABC_SaveList_Json(PublicKey, _token, jsonObject);
        //}

        //[WebMethod]
        //public string QuanLyXetABC_BieuDo(int thang, int nam, string idNhanVien)
        //{
        //    return _service.QuanLyXetABC_BieuDoVaoRa_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        //}

        [WebMethod]
        public string Get_HoSoNhanVienBy_Id(string idNhanVien)
        {
            return _service.Get_HoSoNhanVienBy_Id_Json(PublicKey, _token, new Guid(idNhanVien));
        }

        //[WebMethod]
        //public bool QuanLyXetABC_KhoaVaMoKhoaList(List<QuanLyXetABC.QuanLyXetABCFieldsForSave> userList, bool khoa)
        //{
        //    var js = new JavaScriptSerializer();
        //    var jsonObject = js.Serialize(userList);
        //    return _service.QuanLyXetABC_KhoaVaMoKhoaList_Json(PublicKey, _token, jsonObject, khoa);
        //}

        [WebMethod]
        public bool WebUsers_XoaUsers(List<QuanLyUser> userList)
        {
            var js = new JavaScriptSerializer();
            var jsonObject = js.Serialize(userList);
            return _service.WebUsers_XoaUsers_Json(PublicKey, _token, jsonObject);
        }

        //[WebMethod]
        //public int CauHinhXetABC_GetThoiGian()
        //{
        //    int thoigian = _service.CauHinhXetABC_GetThoiGian(PublicKey, _token, new Guid("A75E3EAE-9A56-4495-B1D2-100CC4B6B025"));
        //    return thoigian;
        //}

        //[WebMethod]
        //public bool CauHinhXetABC_Update(int day)
        //{
        //    return _service.CauHinhXetABC_Update(PublicKey, _token, new Guid("A75E3EAE-9A56-4495-B1D2-100CC4B6B025"),day);
        //}

        //[WebMethod]
        //public string QuanLyXetABC_Detail(int thang, int nam, string idNhanVien)
        //{
        //    return _service.QuanLyXetABC_ChiTietTheoNhanVien_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        //}

        //[WebMethod]
        //public string KiemTraPhongBanXetABC_Find(int thang, int nam, bool? daXetXongAbc)
        //{
        //    return _service.KiemTraPhongBanXetABC_Find_Json(PublicKey, _token, thang, nam, daXetXongAbc);
        //}

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string ThongKeXetABCTheoNam_Find(int nam, string bophan, string idLoaiNhanSu, string maNhanSu, string webUserId)
        //{
        //    Guid? formatBoPhanId = bophan == null ? (Guid?)null : new Guid(bophan);
        //    Guid? formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    string webGroupId = HttpContext.Current.Session[SessionKey.WebGroupId.ToString()].ToString();
        //    string idNhanVien = HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()].ToString();
        //    if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
        //        return _service.ThongKeXetABCTheoNam_Cua1NhanVien_Find_Json(PublicKey, _token, nam, new Guid(idNhanVien));
        //    else
        //        return _service.ThongKeXetABCTheoNam_Find_Json(PublicKey, _token, nam, formatBoPhanId, formatLoaiNhanSuId,
        //            maNhanSu, new Guid(webUserId));
        //}

        //[WebMethod(EnableSession = true)]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string ThongKeXetABCTheoThang_Find(int thang, int nam, string bophan, string idLoaiNhanSu, string maNhanSu, string webUserId)
        //{
        //    Guid? formatBoPhanId = bophan == null ? (Guid?)null : new Guid(bophan);
        //    Guid? formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    string webGroupId = HttpContext.Current.Session[SessionKey.WebGroupId.ToString()].ToString();
        //    string idNhanVien = HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()].ToString();
        //    if (webGroupId.ToUpper() == "53D57298-1933-4E4B-B4C8-98AFED036E21")
        //        return _service.ThongKeXetABCTheoThang_Cua1NhanVien_Find_Json(PublicKey, _token, thang, nam, new Guid(idNhanVien));
        //    else
        //        return _service.ThongKeXetABCTheoThang_Find_Json(PublicKey, _token, thang, nam, formatBoPhanId,
        //            formatLoaiNhanSuId,
        //            maNhanSu, new Guid(webUserId));
        //}

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string QuanLyKhaiBaoCongTac_Find(int thang, int nam, string bophan, int? trangthai, string maNhanSu, string webUserId)
        //{
        //    String json = "";
        //        Guid? formatBoPhanId = bophan == null ? (Guid?)null : new Guid(bophan);
        //        json= _service.QuanLyKhaiBaoCongTac_Find_Json(PublicKey, _token, thang, nam, formatBoPhanId, trangthai, maNhanSu, new Guid(webUserId));              
        //    return json;
            
        //}

        //[WebMethod]
        //public bool QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList(List<QuanLyCongTac> list, int trangthai)
        //{
        //    var js = new JavaScriptSerializer();
        //    var jsonObject = js.Serialize(list);
        //    return _service.QuanLyKhaiBaoCongTac_ThayDoiTrangThaiList_Json(PublicKey, _token, jsonObject, trangthai);
        //}

        //[WebMethod]
        //public bool QuanLyKhaiBaoCongTac_Delete(List<QuanLyCongTac> list)
        //{
        //    var js = new JavaScriptSerializer();
        //    var jsonObject = js.Serialize(list);
        //    return _service.QuanLyKhaiBaoCongTac_DeleteListList_Json(PublicKey, _token, jsonObject);
        //}

        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string CaNhanKhaiBaoCongTac_Find(int thang, int nam, string webUserId)
        //{
        //    return _service.CaNhanKhaiBaoCongTac_Find_Json(PublicKey, _token, thang, nam, new Guid(webUserId));
        //}

        //[WebMethod]
        //public void CaNhanKhaiBaoCongTac_KhaiBaoMoi(string noidung, DateTime tungay, DateTime denngay, string webUserId)
        //{
        //    _service.CaNhanKhaiBaoCongTac_KhaiBaoMoi(PublicKey, _token, noidung, tungay, denngay, new Guid(webUserId));
        //}


        [WebMethod]
        public string KyTinhLuong()
        {
            return _service.KyTinhLuong_GetAll_Json(PublicKey, _token);
        }
        [WebMethod]
        public string KyTinhLuong_ByYear(int year)
        {
            return _service.KyTinhLuong_ByYear_Json(PublicKey, _token, year);
        }

        [WebMethod]
        public string KyTinhLuong_ByMonthAndYear(int month, int year)
        {
            return _service.KyTinhLuong_ByMonthAndYear_Json(PublicKey, _token,month,year);
        }

        [WebMethod]
        public bool KiemTraKhoaSo_KyTinhLuong(int month, int year)
        {
            return _service.KiemTraKhoaSo_KyTinhLuong(PublicKey, _token, month, year);
        } 

        [WebMethod]
        public string ThongTinNhanSu_BANGLUONG(string webUserId, string kyTinhLuong, byte? loaiLuong)
        {
            String json="";
            if (DocMaDonViSuDung() == "IUH")
            {
                json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_IUH_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong), loaiLuong);

            }
            else if (DocMaDonViSuDung() == "LUH")
            {
                json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_LUH_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong));
            }
            else if (DocMaDonViSuDung() == "UTE")
            {
                json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong));
            }
            return json;
            
        }
        [WebMethod]
        public string ThongTinNhanSu_BANGLUONG_Report(string webUserId, string kyTinhLuong)
        {
            String json = "";
            json = _service.ModuleThongTinNhanSu_BANGLUONGNHANVIEN_Report_Json(PublicKey, _token, new Guid(webUserId), new Guid(kyTinhLuong));            
            return json;
        }

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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


        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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


        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
        public string ModuleThongTinNhanSu_BAOHIEMXAHOI(string webUserId)
        {
            String json = "";
                json = _service.ModuleThongTinNhanSu_BAOHIEMXAHOI_Json(PublicKey, _token, new Guid(webUserId));
            return json;

        }

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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

        [WebMethod]
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


        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public string ChamCongNgayNghi_Find(int ngay, int thang, int nam, string boPhanId, string maNhanSu, string webUserId, string idLoaiNhanSu)
        //{
        //    var formatBoPhanId = boPhanId == null ? (Guid?)null : new Guid(boPhanId);
        //    var formatLoaiNhanSuId = idLoaiNhanSu == null ? (Guid?)null : new Guid(idLoaiNhanSu);
        //    return _service.ChamCongNgayNghi_Find_Json(PublicKey, _token, thang, nam, formatBoPhanId, maNhanSu, new Guid(webUserId), formatLoaiNhanSuId);
        //}

        //[WebMethod]
        //public void ChamCongNgayNghi_TaoMoi(string nhanVienId, string noiDung, string idHinhThucNghi, DateTime tuNgay, DateTime denNgay, string webUserId)
        //{
        //    _service.ChamCongNgayNghi_TaoMoi(PublicKey, _token, new Guid(nhanVienId), noiDung, new Guid(idHinhThucNghi), tuNgay,
        //       denNgay, new Guid(webUserId));
        //}

        //[WebMethod]
        //public void ChamCongNgayNghi_DeleteList(List<DTO_ChamCongNgayNghi_Find> obj)
        //{
        //    var js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    var strJson = js.Serialize(obj);
        //    _service.ChamCongNgayNghi_DeleteList_Json(PublicKey, _token, strJson);
        //}

        //[WebMethod]
        //public void ChamCongNgayNghi_Save(DTO_ChamCongNgayNghi_Find obj)
        //{
        //    var js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    var strJson = js.Serialize(obj);
        //    _service.ChamCongNgayNghi_Save(PublicKey, _token, obj);
        //}

        //[WebMethod]
        //public string ChamCongNgayNghi_GetByID(string id)
        //{
        //    return _service.ChamCongNgayNghi_GetByID_Json(PublicKey, _token, new Guid(id));
        //}

        //[WebMethod]
        //public string HoSoNhanVienBy_MaBoPhan(string maBoPhan)
        //{
        //    var formatBoPhanId = maBoPhan == null ? (Guid?)null : new Guid(maBoPhan);
        //    return _service.GetList_HoSoNhanVienBy_MaBoPhan_Json(PublicKey, _token, formatBoPhanId);
        //}

        //[WebMethod]
        //public bool ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(string chamCongNgayNghiOid, DateTime tuNgay, DateTime denNgay, string nhanVienID)
        //{
        //    var formatchamCongNgayNghiOid = chamCongNgayNghiOid == null ? (Guid?)null : new Guid(chamCongNgayNghiOid);
        //    return _service.ChamCongNgayNghi_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatchamCongNgayNghiOid, tuNgay,
        //        denNgay, new Guid(nhanVienID));
        //}

        //[WebMethod]
        //public bool CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(string khaiBaoCongTacOid, DateTime tuNgay, DateTime denNgay, string webUserId)
        //{
        //    var formatkhaiBaoCongTacOid = khaiBaoCongTacOid == null ? (Guid?)null : new Guid(khaiBaoCongTacOid);
        //    return _service.CaNhanKhaiBaoCongTac_KiemTraTuNgayDenNgayCoHopLe(PublicKey, _token, formatkhaiBaoCongTacOid, tuNgay,
        //        denNgay, new Guid(webUserId));
        //}

        //[WebMethod]
        //public bool CaNhanKhaiBaoCongTac_DeleteList(List<DTO_CC_KhaiBaoCongTac> list)
        //{
        //    var js = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    var strJson = js.Serialize(list);
        //    return _service.CaNhanKhaiBaoCongTac_DeleteList_Json(PublicKey, _token, strJson);
        //}

        [WebMethod]
        public string GiayToHoSo_GetList_ByNhanVienId(string nhanVienId)
        {
            String json = "";
            Guid thongTinNhanVien = nhanVienId == null ? Guid.Empty : new Guid(nhanVienId);
            json= _service.GiayToHoSo_GetList_ByNhanVienId_Json(PublicKey, _token, thongTinNhanVien);   
            return json;
        }

        [WebMethod(EnableSession = true)]
        public string ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang()
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                string idNhanVien = HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()].ToString();
                json = _service.ModuleThongTinNhanSu_GiayXacNhanVayVonNganHang_IUH_Json(PublicKey, _token, new Guid(idNhanVien));
            }
            return json;

        }

        [WebMethod(EnableSession = true)]
        public string ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong()
        {
            String json = "";
            if (DocMaDonViSuDung() == "IUH")
            {
                string idNhanVien = HttpContext.Current.Session[SessionKey.ThongTinNhanVien.ToString()].ToString();
                json= _service.ModuleThongTinNhanSu_GiayXacNhanCanBoVienChucTruong_IUH_Json(PublicKey, _token, new Guid(idNhanVien));
            }
            return json;
            
        }
    }
}
