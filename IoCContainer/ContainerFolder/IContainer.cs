using IoCContainer.ValueObjects;
using System;
using System.Collections.Generic;

namespace IoCContainer.ContainerFolder
{
    public interface IContainer
    {
        void Register<Tinter, Timple>() where Tinter : class where Timple : class;
        void Register<Tinter, Timple>(LifeCycle lifeCycle) where Tinter : class where Timple : class;
        Tinter Resolve<Tinter>() where Tinter : class;
        object Resolve(Type type);
        IEnumerable<Tinter> ResolveAll<Tinter>() where Tinter : class;
        IEnumerable<object> ResolveAll(Type type);

    }
}
