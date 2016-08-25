using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public interface ICustomer
    {
        string FirstName { get; set; }
        IContact ContactDetails { get; set; }
    }
}
