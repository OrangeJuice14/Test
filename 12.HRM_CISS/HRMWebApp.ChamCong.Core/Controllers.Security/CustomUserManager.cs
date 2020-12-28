using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using HRMWeb_Service;
using HRMWebApp.Helpers;

namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class CustomUserManager:UserManager<ApplicationUser>
    {        
        public CustomUserManager() : base(new CustomUserSore<ApplicationUser>())
        {            
            this.PasswordHasher = new OldSystemPasswordHasher();           
        }
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
         Service1 _service = new Service1();
        public override System.Threading.Tasks.Task<ApplicationUser> FindAsync(string userName, string password)
        {
            //
            Task<ApplicationUser> taskInvoke = Task<ApplicationUser>.Factory.StartNew(() =>
                {
                    HRMWeb_Business.Model.DTO_WebUser user=null;
                    //
                    string maDonViSuDung = ConfigurationUtil.ReadAppSetting("MaDonViSuDung");
                    if (maDonViSuDung.Equals("NH"))
                    {
                        user = _service.CheckForLogin_WebUser_LDap(PublicKey, _token, userName);
                    }
                    else
                    {
                        user = _service.CheckForLogin_WebUser(PublicKey, _token, userName, password);
                    }
                    //
                    if (user != null)
                    {   ApplicationUser applicationUser = new ApplicationUser();
                        applicationUser.UserName = user.UserName;
                        applicationUser.HoVaTen = user.HoVaTen;
                        applicationUser.Id = user.Oid.ToString();
                        applicationUser.WebGroupId = user.WebGroupID.ToString();
                        return applicationUser;
                    }
                    return null;
                });
            return taskInvoke;
        }      
    }

    /// <summary>
    /// Use Custom approach to verify password
    /// </summary>
    public class OldSystemPasswordHasher : PasswordHasher
    {
        public override string HashPassword(string password)
        {
            return base.HashPassword(password);
        }

        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {	
            if (true)
            {
                return PasswordVerificationResult.SuccessRehashNeeded;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}