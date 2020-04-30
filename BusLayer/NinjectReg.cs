using DataLayer.Context;
using DataLayer.Models;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer
{
    public class NinjectReg : NinjectModule
    {
        public override void Load()
        {
            Bind<UnitOfWork>().ToSelf().InTransientScope();
            Bind<AppDbContext>().ToSelf().InTransientScope();
        }
    }
}
