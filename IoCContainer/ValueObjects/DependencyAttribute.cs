using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoCContainer.ValueObjects
{
    //tells the container that the type has other types injected, so they need to be resolved before
    [AttributeUsage(AttributeTargets.Constructor, Inherited = false)]
    public class DependencyAttribute : Attribute
    {
    }
}
