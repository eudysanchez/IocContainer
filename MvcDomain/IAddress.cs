using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public interface IAddress
    {
        int HouseNumber { get; }
        string PostCode { get; }
    }
}
