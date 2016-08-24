using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocContainer.Tests.Helpers
{
    class CompositeItemWrong : ICompositeItemWrong
    {
        private IHelper1 _helper1;
        private IHelper2 _helper2;

        public CompositeItemWrong(IHelper1 helper1, IHelper2 helper2)
        {
            _helper1 = helper1;
            _helper2 = helper2;
        }

        public void Print()
        {
            Console.WriteLine("Coming from CompositeItem -ClassName: {0}", GetType().Name);
        }
    }
}
