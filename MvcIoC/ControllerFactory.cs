using IoCContainer.ContainerFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcIoC
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IContainer _container;

        public ControllerFactory(IContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return null;
            //resolve types for the specific controller
            return _container.Resolve(controllerType) as Controller;
        }
    }
}