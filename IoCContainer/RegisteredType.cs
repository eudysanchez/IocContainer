using System;

namespace IoCContainer
{
    internal class RegisteredType
    {
        internal Type ObjectType { get; set; }
        internal LifeCycle LifeCycle { get; set; }
    }
}