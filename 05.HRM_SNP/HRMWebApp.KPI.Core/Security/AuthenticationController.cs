using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HRMWebApp.KPI.Core.Security;
using HRMWebApp.Helpers;
using HRMWeb_Business.Model;
using HRMWeb_Service;
using Helper = HRMWebApp.Helpers.Helper;
using Microsoft.AspNet.Identity;
using NHibernate.Linq;
using Microsoft.Owin.Security;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.DB;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.OAuth2;
using System.Configuration;

namespace HRMWebApp.KPI.Core.Security
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
            AuthenticationManager.SignOut();
            return Helper.JsonSucess();
        }


        public async Task<int> Login(string userName, string passWord, string captchaString)
        {

            ApplicationUser model = new ApplicationUser() { UserName = userName, Password = passWord };
            CustomUserManager PCustomUserManager = new CustomUserManager();
            var user = await PCustomUserManager.FindAsync(model.UserName, model.Password);
            if (user != null)
            {
                await SignInAsync(user, false);
                if (user.HoVaTen != null)
                    return 1;
                else
                    return 2;
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
                // If we got this far, something failed, redisplay form
                return 0;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);



            var identity = await PCustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            //var identity = await UserManager1.CreateAsync(user);//, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }


        public ActionResult GetUserSessionInfo()
        {
            ApplicationUser result = new ApplicationUser();
            string userName = User.Identity.Name;
            string currentUserId = User.Identity.GetUserId();
            if (currentUserId != null)
                result = AuthenticationHelper.GetUserById(new Guid(currentUserId), userName);


            //Dictionary<string, object> result = new Dictionary<string, object>();


            //result["UserId"] = Session[SessionKey.UserId.ToString()];
            //result["ThongTinNhanVien"] = Session[SessionKey.ThongTinNhanVien.ToString()];
            //result["UserName"] = Session[SessionKey.UserName.ToString()];
            //result["HoVaTen"] = Session[SessionKey.HoVaTen.ToString()];
            //result["WebGroupId"] = Session[SessionKey.WebGroupId.ToString()];
            return result.ToJSON();
        }

        private async Task RequestToGetInputReport()
        {
            // lots of code prior to this
            Response.Redirect("/");
        }
        public async Task<ActionResult> Login_Gmail()
        {
            ActionResult action = new EmptyResult();
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
                        return new EmptyResult();
                    ApplicationUser applicationUser = new ApplicationUser();
                    applicationUser.UserName = user.UserName;
                    applicationUser.HoVaTen = user.HoVaTen;
                    applicationUser.Id = user.Oid.ToString();



                    await SignInAsync(applicationUser, false);
                    return RedirectToLocal("/");


                }


            }
            catch (Exception ex) { }
            return action;
        }

    }
}
