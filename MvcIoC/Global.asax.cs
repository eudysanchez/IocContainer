using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IoCContainer.Container;
using MvcIoC.Controllers;
using MvcDomain;

namespace MvcIoC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            IContainer iocContainer = new Container();
            iocContainer.Register<CompanyController, CompanyController>();
            iocContainer.Register<ICompanyRepository, CompanyRepository>();
            iocContainer.Register<HomeController, HomeController>();
            //iocContainer.Register<IContact, Contact>();
            //iocContainer.Register<IAddress, Address>();

            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(iocContainer));


        }
    }
}
