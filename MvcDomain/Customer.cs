using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public class Customer : ICustomer
    {
        public string FirstName { get; set; }
        public IContact ContactDetails { get; set; }
    }
}
