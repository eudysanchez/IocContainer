using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using IoCContainer.ContainerFolder;
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

            //Create new container
            IContainer iocContainer = new Container();
            //Configure the container
            BootStrapper.Configure(iocContainer);
            //set the controller factory and pass the container to the factory so it can resolve the types.
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(iocContainer));


        }
    }
}
