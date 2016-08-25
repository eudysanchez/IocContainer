using IoCContainer.ContainerFolder;
using MvcDomain;
using MvcIoC.Controllers;
using System;

namespace MvcIoC
{
    internal class BootStrapper
    {
        internal static void Configure(IContainer container)
        {
            container.Register<CompanyController, CompanyController>();
            container.Register<ICompanyRepository, CompanyRepository>();
            container.Register<HomeController, HomeController>();
        }
    }
}