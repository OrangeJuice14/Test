using HRMWebApp.ChamCong.Core.WebAPI.Models;

namespace HRMWebApp.ChamCong.Core.WebAPI
{
    public class User
    {
        public static URMUser _currentUser
        {
            get
            {
                var user = System.Web.HttpContext.Current.Session["URMUser"] as URMUser;
                if (user == null)
                {
                    user = new URMUser();
                    System.Web.HttpContext.Current.Session["URMUser"] = user;
                }
                return user;
            }
            set
            {
                System.Web.HttpContext.Current.Session["URMUser"] = value;
            }
        }
    }
}
