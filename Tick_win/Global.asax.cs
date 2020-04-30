using BusLayer;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tick_win.Controllers;

namespace Tick_win
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule reg = new NinjectReg();
            var kernel = new StandardKernel(reg);
            kernel.Get<HomeController>();
            kernel.Get<UnitOfWork>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
