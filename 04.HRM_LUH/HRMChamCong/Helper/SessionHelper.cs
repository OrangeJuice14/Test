using System;

namespace HRMChamCong.Helper
{
    public enum SessionKey
    {
        UserId,
        UserName,
        ThongTinNhanVien,
        CaptchaImage,
        WebGroupId
    }
    public static class SessionHelper
    {
        public static T Data<T>(SessionKey key)
        {
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
