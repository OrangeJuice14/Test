using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using HRMWeb_Service;
using HRMWebApp.Helpers;


namespace HRMWebApp.KPI.Core.Security
{
    public class CustomUserManager:UserManager<ApplicationUser>
    {        
        public CustomUserManager() : base(new CustomUserSore<ApplicationUser>())
        {            
            //We can retrieve Old System Hash Password and can encypt or decrypt old password using custom approach.
	    //When we want to reuse old system password as it would be difficult for all users to initiate pwd change as per Idnetity Core hashing.
            this.PasswordHasher = new OldSystemPasswordHasher();           
        }
        const string PublicKey = "HRMChamCong";
        const string SecretKey = "pscvietnam@hoasua";
        readonly string _token = EncryptUtil.MakeToken(PublicKey, SecretKey);
         Service1 _service = new Service1();
        public override System.Threading.Tasks.Task<ApplicationUser> FindAsync(string userName, string password)
        {
            Task<ApplicationUser> taskInvoke = Task<ApplicationUser>.Factory.StartNew(() =>
                {
                    //First Verify Password...
                    //PasswordVerificationResult result = this.PasswordHasher.VerifyHashedPassword(userName, password);
                    HRMWeb_Business.Model.DTO_WebUser user=null;

                    user = _service.CheckForLogin_WebUser(PublicKey, _token, userName, password);

                    if (user != null)
                    {
                        //Return User Profile Object...
                        //So this data object will come from Database we can write custom ADO.net to retrieve details/
                        ApplicationUser applicationUser = new ApplicationUser();
                        applicationUser.UserName = user.UserName;
                        applicationUser.HoVaTen = user.HoVaTen;
                        applicationUser.Id = user.Oid.ToString();
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

            //Here we will place the code of password hashing that is there in our current solucion.This will take cleartext anad hash
	    //Just for demonstration purpose I always return true.	
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