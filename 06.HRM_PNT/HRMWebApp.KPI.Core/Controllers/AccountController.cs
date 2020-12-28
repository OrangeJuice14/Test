using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HRMWebApp.KPI.Core.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return new FilePathResult("~/app/shell.html", "text/html");
        }
    }
}
