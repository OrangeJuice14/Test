using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRMWebApp.KPI.DB.Entities;
using HRMWebApp.KPI.Core.DTO;
using HRMWebApp.KPI.DB;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;
using HRMWebApp.Helpers;


namespace HRMWebApp.KPI.Core.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return new FilePathResult("~/app/shell.html", "text/html");
        }


        public ActionResult ValidateUser(string userName,string password)
        {
            WebUser webUser = null;
            SessionManager.DoWork(session =>
            {
                webUser = session.Query<WebUser>().SingleOrDefault(s => s.UserName.Trim().Equals(userName.Trim()));
                if (webUser != null && password != WebUser.ChangeMd5(password.Trim()))
                    webUser = null;
                else if (webUser != null && webUser.Password == WebUser.ChangeMd5(password.Trim()))
                {
                    SessionHelper.Data(SessionKey.UserId, webUser.Id);
                    SessionHelper.Data(SessionKey.UserName, webUser.UserName);
                }
            });
            return webUser.ToJSON();
        }

        public ActionResult SignOut()
        {

            SessionHelper.Data(SessionKey.UserId, null);
            SessionHelper.Data(SessionKey.UserName, null);


            return new EmptyResult();
        }

        public ActionResult GetCurrentUser()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["Id"] = SessionHelper.Data<Guid>(SessionKey.UserId);
            result["Name"] = SessionHelper.Data<string>(SessionKey.UserName);
            return result.ToJSON();
        }

        public ActionResult Login(string redirect)
        {
            //var loginData = new LoginData { Redirect = redirect };

            //save the default login details for now
            //loginData.EmailAddress = "admin@rentthatbike.com";
            //loginData.Password = "admin";
           


            ViewBag.Redirect = redirect;
            return View();
        }
    }
}
