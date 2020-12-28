using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HRMWebApp.Helpers;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var name = SessionHelper.Data<string>(SessionKey.UserName);

            return View();
        }
    }
}
