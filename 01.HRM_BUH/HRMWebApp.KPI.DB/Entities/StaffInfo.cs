using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMWebApp.KPI.DB.Entities
{
    public class StaffInfo
    {
        public StaffInfo()
        {
            AgentObjects = new List<AgentObject>();
            SubPositions = new List<SubPosition>();
            //WebUsers = new List<WebUser>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Password { get; set; }
        public virtual string ManageCode { get; set; }
        public virtual IList<AgentObject> AgentObjects { get; set; }

        public virtual Position Position { get; set; }
        public virtual IList<WebUser> WebUsers { get; set; }
        public virtual WebUser WebUser { get; set; } // NOT OK
        //public virtual Department Department { get; set; }
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
        public virtual Department Subject { get; set; }

        public virtual IList<SubPosition> SubPositions { get; set; }
    }
}
