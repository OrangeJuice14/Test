using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HRMWebApp.Helpers;
using HRMWeb_Business.Model;
using HRMWeb_Service;
using Helper = HRMWebApp.Helpers.Helper;
using Microsoft.AspNet.Identity;
using NHibernate.Linq;
using Microsoft.Owin.Security;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth2;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using HRMWeb_Business.Predefined;
using System.Web.Script.Serialization;
using HRMWebApp.ChamCong.Core.WebAPI;
using HRMWebApp.ChamCong.Core.WebAPI.Models;
using HRMWeb_Business.BusinessServiceFactory;
using System.Net;

using System.Security.Claims;

namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class AuthenticationController : Controller
    {
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
        readonly Service1 _service = new Service1();  //readonly Service1Client _service = new Service1Client();
        CustomUserManager PCustomUserManager = new CustomUserManager();

        private static readonly GoogleClient googleClient = new GoogleClient
        {
            ClientIdentifier = ConfigurationManager.AppSettings["googleClientID"],
            ClientCredentialApplicator = ClientCredentialApplicator.PostParameter(ConfigurationManager.AppSettings["googleClientSecret"]),
        };
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult LogOff()
        {
            try
            {
                var cookie = Request.Cookies["User"];
                cookie.Values.Remove("User");
                Response.Cookies.Add(cookie);
            }
            catch (Exception ex) { }
            //

            WebUserLogin_Factory factory = new WebUserLogin_Factory();
            factory.SaveUserLogin(System.Web.HttpContext.Current.User.Identity.GetUserId(), GetVisitorIPAddress(), false);
            AuthenticationManager.SignOut();
            return Helper.JsonSucess();
        }

        bool LoginDomain(string username, string password)
        {
            //
            string domainLogin = ConfigurationUtil.ReadAppSetting("DomainLogin");
            //
            if (CheckDomain(username, password, domainLogin))
            {
                //
                return true;
            }
            //
            return false;
        }
        bool CheckDomain(string username, string pass, string doamin)
        {
            bool result = false;
            //
            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, doamin))
                {
                    //
                    result = pc.ValidateCredentials(username, pass);
                }
            }
            catch (Exception ex)
            {

            }
            //
            return result;
        }

        public async Task<int> Login_New(string userName, string password, bool isKPIs, string captchaString)
        {
            try
            {
                //Dictionary<string, object> postData = new Dictionary<string, object>();
                //postData.Add("type", "password");
                //postData.Add("username", userName);
                //postData.Add("password", password);
                //postData.Add("appid", "HETHONGERP");
                //postData.Add("hours", 24);
                //var json = new JavaScriptSerializer().Serialize(postData);
                //var result = ApiHelper.Post_NotAsync<URMUser>(ApiHelper.APIURL + "api/User/Login", postData).Result;
                var result = _service.CheckForLogin_WebUser_URM(PublicKey, _token, userName, password);
                if (result == "INVALID_USERNAME")
                {
                    return 6;
                }
                else if (result == "SUCCESS")
                {
                    //WebAPI.User._currentUser = result;

                    SessionHelper.Data(SessionKey.IsKPIs, false);

                    var user = _service.CheckForLogin_WebUser_LDap(PublicKey, _token, userName);

                    if (user != null)
                    {
                        if (user.HoatDong != false)
                        {
                            ApplicationUser applicationUser = new ApplicationUser();
                            applicationUser.UserName = user.UserName;
                            applicationUser.HoVaTen = user.HoVaTen;
                            applicationUser.Id = user.Oid.ToString();
                            applicationUser.WebGroupId = user.WebGroupID.ToString();

                            // đổi password hrm nếu khác với password urm
                            if (password != user.Password)
                            {
                                _service.ChangePassword_WebUser(PublicKey, _token, user.Oid, password);
                            }

                            String idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                            await SignInAsync(applicationUser, false);

                            WebUserLogin_Factory factory = new WebUserLogin_Factory();
                            factory.SaveUserLogin(applicationUser.Id, GetVisitorIPAddress(), true);
                            // 
                            #region 1. Tạo cookie sử dụng UIS và HRM (Quan trọng)

                            HttpCookie cookie = new HttpCookie("User");
                            cookie["User"] = user.UserName;
                            cookie["Domain"] = "psctelecom.com.vn";
                            cookie.Expires = DateTime.Now.AddDays(1);
                            //
                            Response.Cookies.Add(cookie);
                            /*
                            Response.Cookies["User"].Value = user.UserName;
                            Response.Cookies["User"].Domain = "psctelecom.com.vn";*/
                            #endregion
                            //
                            #region 2. Trả dữ liệu cho client
                            if (applicationUser.WebGroupId.ToUpper().Equals(idTaiKhoanCaNhan))
                                return 1;
                            else
                                return 2;
                            #endregion
                            //
                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản đã bị khóa ở HRM.");
                            //
                            return 5;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kiểm tra tài khoản HRM.");
                        //
                        return 0;
                    }
                }
                else if (result == "WRONG_PASSWORD")
                {
                    ModelState.AddModelError("", "Kiểm tra tên và mật khẩu trên Phân quyền tổng.");
                    //
                    return 0;
                }
                else if (result == "LOCKED")
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa trên Phân quyền tổng.");
                    //
                    return 4;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Helper.ErrorLog("AuthenticationController/Login_New", ex);
                throw ex;
            }
        }

        public async Task<int> Login(string userName, string password, bool isKPIs, string captchaString)
        {
            string maDonViSuDung = ConfigurationUtil.ReadAppSetting("MaDonViSuDung");
            //
            //Kiểm tra tài khoản đăng nhập có đúng với Ldap hay không
            if (maDonViSuDung.Equals(""))
            {
                if (!LoginDomain(userName, password))
                {
                    return 3;
                }
            }
            //
            ApplicationUser model = new ApplicationUser() { UserName = userName, Password = password };
            CustomUserManager PCustomUserManager = new CustomUserManager();

            ApplicationUser user = new ApplicationUser();
            //
            user = await PCustomUserManager.FindAsync(model.UserName, model.Password);
            SessionHelper.Data(SessionKey.IsKPIs, false);
            //
            if (user != null)
            {
                String idTaiKhoanCaNhan = WebGroupConst.TaiKhoanCaNhanID.ToString().ToUpper();
                await SignInAsync(user, false);

                WebUserLogin_Factory factory = new WebUserLogin_Factory();
                factory.SaveUserLogin(user.Id, GetVisitorIPAddress(), true);

                #region 1. Tạo cookie sử dụng UIS và HRM (Quan trọng)
                HttpCookie cookie = new HttpCookie("User");
                cookie["User"] = user.UserName;
                cookie["Domain"] = "psctelecom.com.vn";
                cookie.Expires = DateTime.Now.AddDays(1);
                //
                Response.Cookies.Add(cookie);
               /*
                Response.Cookies["User"].Value = user.UserName;
                Response.Cookies["User"].Domain = "psctelecom.com.vn";*/
                #endregion
                //
                #region 2. Trả dữ liệu cho client
                if (user.WebGroupId.ToUpper().Equals(idTaiKhoanCaNhan))
                    return 1;
                else
                    return 2;
                #endregion

            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                //
                return 0;
            }

        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //
            var identity = await PCustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private async Task RequestToGetInputReport()
        {
            // lots of code prior to this
            Response.Redirect("/");
        }
        public async Task<ActionResult> Login_Gmail()
        {
            ActionResult action = new EmptyResult();

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
                string domainName = ConfigurationManager.AppSettings["mainDomain"];

                if (user == null)
                {
                    string html = "<h3>Email của bạn không tồn tại trong hệ thống! Vui lòng <a style='color: blue' href='https://www.google.com/accounts/Logout?continue=https://appengine.google.com/_ah/logout?continue=http://kpis.dlu.edu.vn/Authentication/Login_Gmail'>đăng xuất tài khoản email</a> hiện tại rồi đăng nhập bằng tài khoản email do trường cung cấp.</h3>";
                    return Content(html, "text/html");
                }

                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = user.UserName;
                applicationUser.HoVaTen = user.HoVaTen;
                applicationUser.Id = user.Oid.ToString();
                applicationUser.CongTyId = user.CongTyId.ToString();
                applicationUser.WebGroupId = user.WebGroupID.ToString();
                applicationUser.AgentObjectTypeId = user.AgentObjectTypeId.ToString();
                applicationUser.DepartmentId = user.BoPhanId == null ? Guid.Empty.ToString() : user.BoPhanId.ToString();
                //
                await SignInAsync(applicationUser, false);
                //
                return RedirectToLocal(domainName);
            }
            //
            return action;
        }
        
        public void ExternalLogin(string provider, string returnUrl = null)
        {
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = Url.Action("ExternalLoginCallback", "Authentication", new { returnUrl = returnUrl }) },
                provider);
        }
        
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToLocal("/");
            }
            string email = null;
            string urlLogout = null;
            string domainName = Helper.GetCurrentDomainName();

            if (info.Login.LoginProvider == "Microsoft")
            {
                email = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                //logout lúc được lúc không
                urlLogout = "https://login.microsoftonline.com/common/oauth2/logout?post_logout_redirect_uri=" + domainName;
            }

            DTO_WebUser user = null;
            if (email != null)
            {
                user = _service.WebUser_GetByEmail(PublicKey, _token, email);
            }

            if (user == null)
            {
                string html = "<h3>Email của bạn không tồn tại trong hệ thống! Vui lòng <a style='color: blue' href='" + urlLogout + "'>đăng xuất tài khoản email</a> hiện tại rồi đăng nhập bằng tài khoản email khác.</h3>";
                return Content(html, "text/html");
            }

            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = user.UserName;
            applicationUser.HoVaTen = user.HoVaTen;
            applicationUser.Id = user.Oid.ToString();
            applicationUser.CongTyId = user.CongTyId.ToString();
            applicationUser.WebGroupId = user.WebGroupID.ToString();
            applicationUser.AgentObjectTypeId = user.AgentObjectTypeId.ToString();
            applicationUser.DepartmentId = user.BoPhanId == null ? Guid.Empty.ToString() : user.BoPhanId.ToString();

            await SignInAsync(applicationUser, false);

            #region Trả dữ liệu cho client
            if (applicationUser.WebGroupId.ToUpper().Equals(WebGroupConst.TaiKhoanCaNhanID.ToString()))
                return RedirectToLocal("/kpi/HoSoNhanSu");
            else
                return RedirectToLocal("/kpi/quanlynhacviec");
            #endregion
        }

        public ActionResult GetUserSessionInfo()
        {
           
            #region Kiểm tra thông tin tài khoản
            ApplicationUser result = new ApplicationUser();
            //
            string currentUserId = User.Identity.GetUserId();
            //
            if (!String.IsNullOrEmpty(currentUserId))
            {
                DTO_WebUser user = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(currentUserId));
                //
                if (user != null)
                {
                    result.UserId = user.Oid.ToString();
                    result.Id = user.ThongTinNhanVien == null ? Guid.Empty.ToString() : user.ThongTinNhanVien.ToString();
                    result.UserName = user.UserName;
                    result.HoVaTen = user.HoVaTen;
                    result.WebGroupId = user.WebGroupID.ToString();
                    result.AgentObjectTypeId = user.AgentObjectTypeId.ToString();
                    result.DepartmentId = user.BoPhanId == null ? Guid.Empty.ToString() : user.BoPhanId.ToString();
                    result.Email = user.Email;
                    result.CongTyId = user.CongTyId.ToString();

                    //Lưu ý Quan trọng: Lấy công ty hiện tại của user
                    WebServicesController._congTy = user.CongTyId.Value;
                    //
                }
            }
            #endregion

            //
            return result.ToJSON();
        }

        public ActionResult GetUserCookieInfo()
        {
            bool isExist = false;
            //
            try
            {
                var cookieId = Request.Cookies["User"];
                //
                if (cookieId != null && !string.IsNullOrEmpty(cookieId.Value))
                {
                    //
                    isExist = true;
                }
            }
            catch (Exception ex) { }
            //
            if (isExist)
            {
                return Helper.JsonExists();
            }
            else
            {
                return Helper.JsonErorr();
            }
        }

        //public ActionResult ChangePassword_WebUser(string webUserId, string passWord)
        //{
        //    try
        //    {
        //        bool changePasswordHRM = _service.ChangePassword_WebUser(PublicKey, _token, new Guid(webUserId), passWord);
        //        if (changePasswordHRM)
        //        {
        //            DTO_WebUser user = _service.Get_WebUserBy_Id(PublicKey, _token, new Guid(webUserId));
        //            if (user != null)
        //            {
        //                var userFromApi = ApiHelper.Get<URMUser>(ApiHelper.APIURL + "api/User/GetByUsername?username=" + user.UserName);
        //                if (userFromApi.Success)
        //                {
        //                    Dictionary<string, object> postData = new Dictionary<string, object>();
        //                    postData.Add("username", user.UserName);
        //                    postData.Add("password", Helper.getMd5Hash("UisStaffID=" + user.UserName.ToUpper() + ";UisPassword=" + passWord ?? ""));
        //                    var result = ApiHelper.Post_NotAsync<string>(ApiHelper.APIURL + "api/User/UpdatePass", postData).Result;
        //                    if (result == "OK")
        //                    {
        //                        //thành công
        //                    }
        //                }
        //            }
        //        }
        //        return Helper.JsonSucess();
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.ErrorLog("AuthenticationController/ChangePassword_WebUser", ex);
        //        throw ex;
        //    }
        //}

        public string GetVisitorIPAddress()
        {
            try
            {
                //string visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                ////HTTP_X_FORWARDED_FOR có thể có nhiều địa chỉ ip. Chỉ lấy cái đầu tiên.
                ////kèm port
                //if (!string.IsNullOrEmpty(visitorIPAddress))
                //{
                //    string[] addresses = visitorIPAddress.Split(',');
                //    if (addresses.Length != 0)
                //    {
                //        visitorIPAddress = addresses[0];
                //    }
                //}

                string visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (string.IsNullOrEmpty(visitorIPAddress))
                    visitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

                return visitorIPAddress;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
