using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class StaffInfo
    {
        public virtual Guid Id { get; set; }

        public virtual Position Position { get; set; }
        //public virtual WebUser WebUser { get; set; }
        public virtual IList<WebUser> WebUsers { get; set; }
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
        public virtual StaffType StaffType { get; set; }
    }
}
