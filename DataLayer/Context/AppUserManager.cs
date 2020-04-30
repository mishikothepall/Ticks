using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
            UserValidator = new UserValidator<AppUser>(this) { AllowOnlyAlphanumericUserNames = false };
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,
          IOwinContext context)
        {
            AppDbContext db = context.Get<AppDbContext>();
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireDigit = false,
                RequireNonLetterOrDigit = false,
                RequireLowercase = true,
                RequireUppercase = true
            };

            return manager;
        }
    }
}
