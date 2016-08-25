using System;

namespace IoCContainer.ValueObjects
{
    //wrap the type to be registered and its life cycle togethers
    internal class RegisteredType
    {
        internal Type ObjectType { get; set; }
        internal LifeCycle LifeCycle { get; set; }
    }
}