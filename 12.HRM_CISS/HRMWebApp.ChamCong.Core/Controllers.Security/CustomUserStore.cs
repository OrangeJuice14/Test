using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Threading;

namespace HRMWebApp.ChamCong.Core.Controllers
{
    public class CustomUserSore<T> : IUserStore<T> where T : ApplicationUser
    {
        public CustomUserSore() { }
        //public UserStore(ExampleStorage database) { ... }
        public System.Threading.Tasks.Task CreateAsync(T user) { throw new NotImplementedException(); }
        public System.Threading.Tasks.Task DeleteAsync(T user) { throw new NotImplementedException(); }
        public System.Threading.Tasks.Task<T> FindByIdAsync(string userId) { throw new NotImplementedException(); }
        public System.Threading.Tasks.Task<T> FindByNameAsync(string userName) { throw new NotImplementedException(); }
        public System.Threading.Tasks.Task UpdateAsync(T user) { throw new NotImplementedException(); }
        public void Dispose() {  }
    }


    //public class CustomUserSore<T> : IUserStore<T> where T : ApplicationUser
    //{

    //    System.Threading.Tasks.Task IUserStore<T>.CreateAsync(T user)
    //    {
    //        //Create /Register New User
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task IUserStore<T>.DeleteAsync(T user)
    //    {
    //        //Delete User
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task<T> IUserStore<T>.FindByIdAsync(string userId)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task<T> IUserStore<T>.FindByNameAsync(string userName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    System.Threading.Tasks.Task IUserStore<T>.UpdateAsync(T user)
    //    {
    //        //Update User Profile
    //        throw new NotImplementedException();
    //    }

    //    void IDisposable.Dispose()
    //    {
    //        // throw new NotImplementedException();

    //    }
    //}
}