using IoCContainer.ValueObjects;

namespace IoCContainer.Container
{
    public interface IContainer
    {
        void Register<Tinter, Timple>() where Tinter : class where Timple : class;

        void Register<Tinter, Timple>(LifeCycle lifeCycle) where Tinter : class where Timple : class;

        T Resolve<T>();
    }
}
