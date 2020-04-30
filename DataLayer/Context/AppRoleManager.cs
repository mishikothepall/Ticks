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

    
    public class AppRoleManager : RoleManager<UserRoles>
    {
        public AppRoleManager(RoleStore<UserRoles> store)
      : base(store)
        { }

        public static AppRoleManager Create(
            IdentityFactoryOptions<AppRoleManager> options,
            IOwinContext context)
        {
            return new AppRoleManager(new
                RoleStore<UserRoles>(context.Get<AppDbContext>()));
        }
    }
}
