using System;

namespace IoCContainer.ValueObjects
{
    internal class RegisteredType
    {
        internal Type ObjectType { get; set; }
        internal LifeCycle LifeCycle { get; set; }
    }
}