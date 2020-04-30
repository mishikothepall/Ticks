using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class UserRoles : IdentityRole
    {
        public UserRoles() : base() { }

        public UserRoles(string name) : base(name) { }
    }
}
