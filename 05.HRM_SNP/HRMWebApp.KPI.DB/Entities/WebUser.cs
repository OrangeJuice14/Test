using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class WebUser
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual Guid WebGroupId { get; set; }
        public virtual StaffInfo StaffInfo { get; set; }
        public static String ChangeMd5(String password)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(password);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
    }
}
