using MvcDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcIoC.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// This is the prpoerty we're going to inject via the Ioc Container.
        /// </summary>
        /// <value>The customer.</value>
        public ICustomer Customer { get; set; }

        public ActionResult Index()
        {
            this.ViewData.Model = this.Customer;
            return View();
        }
    }
}