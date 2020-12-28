using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate.Linq;
using HRMWebApp.Helpers;
using System.Web.Http;

namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return new FilePathResult("~/app/shell.html", "text/html");
        }

        public ActionResult SignOut()
        {
            //
            SessionHelper.Data(SessionKey.UserId, null);
            SessionHelper.Data(SessionKey.UserName, null);
            //
            return new EmptyResult();
        }

        public ActionResult Login(string redirect)
        {
            //
            Response.Status = "301 Moved Permanently";
            Response.StatusCode = 301;
            Response.AddHeader("Location", "/login.html");
            Response.End();
            //
            return View();
        }
    }
}
