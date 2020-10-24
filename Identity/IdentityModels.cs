using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace HealthcareCompanion
{
    /*
    public class AppUser : IdentityUser
    {
        //public AppUser() : base() { }
        //public AppUser(string name) : base(name) { }
    }
    */
    /*
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {

        }

        public AppRole(string name) : base(name)
        {

        }
    }*/

    public class HealthcareDbContext : IdentityDbContext<IdentityUser>
    {
        public HealthcareDbContext() :base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static HealthcareDbContext Create()
        {
            return new HealthcareDbContext();
        }

    }

   
    public class AppUserManager : UserManager<IdentityUser>
    {
        public AppUserManager(IUserStore<IdentityUser> store)
            : base(store)
        {
        }

        // this method is called by Owin therefore best place to configure your User Manager
        public static AppUserManager Create(
            IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager = new AppUserManager(
                new UserStore<IdentityUser>(context.Get<HealthcareDbContext>()));

            // optionally configure your manager
            // ...

            return manager;
        }
    }

   

}