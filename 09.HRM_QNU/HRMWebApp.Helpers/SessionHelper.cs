using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.Helpers
{
    public enum SessionKey
    {
        UserId,
        UserName,
        HoVaTen,
        ThongTinNhanVien,
        CaptchaImage,
        WebGroupId,
        IsKPIs
    }
    public static class SessionHelper
    {
        public static T Data<T>(SessionKey key)
        {
            if (System.Web.HttpContext.Current.Session[key.ToString()] == null)
                return default(T);
            return (T)System.Web.HttpContext.Current.Session[key.ToString()];
        }

        public static void Data(SessionKey key, object value)
        {
            System.Web.HttpContext.Current.Session[key.ToString()] = value;
        }

        public static void Remove(SessionKey key)
        {
            System.Web.HttpContext.Current.Session.Remove(key.ToString());
        }

        public static bool ContainKey(SessionKey key)
        {
            return System.Web.HttpContext.Current.Session[key.ToString()] != null;
        }
    }
}
