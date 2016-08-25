﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcDomain
{
    public class Contact : IContact
    {
        public string TelephoneNumber { get; set; }
        public IAddress CustomerAddress { get; set; }
    }
}
