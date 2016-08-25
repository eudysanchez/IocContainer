using IoCContainer.ValueObjects;
using MvcDomain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcIoC.Controllers
{
    public class CompanyController : Controller
    {
        readonly ICompanyRepository repo;

        [Dependency]
        public CompanyController(ICompanyRepository tempProduct)
        {
            this.repo = tempProduct;
        }
        public string Index()
        {
            var data = repo.GetAll();
            return JsonConvert.SerializeObject(data);
        }
    }
}