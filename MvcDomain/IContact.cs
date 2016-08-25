using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public interface IContact
    {
        string TelephoneNumber { get; set; }
        IAddress CustomerAddress { get; set; }
    }
}
