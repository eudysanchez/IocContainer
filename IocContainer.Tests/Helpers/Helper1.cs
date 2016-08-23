using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer.Tests.Helpers
{
    public class Helper1 : IHelper1
    {
        public void Print()
        {
            Console.WriteLine("Coming from Helper1 - ClassName: {0}", GetType().Name);
        }
    }
}
