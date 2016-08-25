using IoCContainer.ValueObjects;
using System;

namespace IoCContainer.ContainerFolder
{
    public interface IContainer
    {
        void Register<Tinter, Timple>() where Tinter : class where Timple : class;
        void Register<Tinter, Timple>(LifeCycle lifeCycle) where Tinter : class where Timple : class;
        T Resolve<T>();
        object Resolve(Type type);
    }
}
