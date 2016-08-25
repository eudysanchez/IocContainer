using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public class Address : IAddress
    {
        public int HouseNumber { get; private set; }
        public string PostCode { get; private set; }

        public Address() { }

        public Address(int houseNumber, string postCode)
        {
            this.HouseNumber = houseNumber;
            this.PostCode = postCode;
        }
    }
}
