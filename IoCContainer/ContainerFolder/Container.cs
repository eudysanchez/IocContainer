using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IoCContainer.Exceptions;
using IoCContainer.IoCCreationServices;
using IoCContainer.ValueObjects;

namespace IoCContainer.ContainerFolder
{
    public class Container : IContainer
    {
        Dictionary<Type, HashSet<RegisteredType>> _instanceRegistry;//store all containers and all the implementation types

        public Container()
        {
            _instanceRegistry = new Dictionary<Type, HashSet<RegisteredType>>();
        }

        //register type with Transient lifeCycle by default
        public void Register<Tinter, Timple>()
            where Tinter : class
            where Timple : class
        {
            Register(typeof(Tinter), typeof(Timple), LifeCycle.Transient);
        }

        //take the lifecyle choosen by the user
        public void Register<Tinter, Timple>(LifeCycle lifeCycle)
            where Tinter : class
            where Timple : class
        {
            Register(typeof(Tinter), typeof(Timple), lifeCycle);
        }

        //register type with Transient lifeCycle by default
        private void Register(Type interfaceType, Type implType)
        {
            Register(interfaceType, implType, LifeCycle.Transient);
        }

        //Using the container(interface) as the key, adds type to register and container to the dictionary
        private void Register(Type interfaceType, Type implType, LifeCycle lifeCycle)
        {
            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType Cant be null");
            if (implType == null)
                throw new ArgumentNullException("implType cant be null");
            if (!interfaceType.IsAssignableFrom(implType))
                throw new ArgumentException("implType");

            if (_instanceRegistry.ContainsKey(interfaceType))
            {
                _instanceRegistry[interfaceType].Add(new RegisteredType
                {
                    LifeCycle = lifeCycle,
                    ObjectType = implType
                });
            }
            else
            {
                var registeredType = new RegisteredType
                {
                    LifeCycle = lifeCycle,
                    ObjectType = implType
                };

                _instanceRegistry.Add(interfaceType, new HashSet<RegisteredType> { registeredType });
            }
        }

        //resolve from type param
        public Tinter Resolve<Tinter>() where Tinter : class
        {
            return (Tinter)Resolve(typeof(Tinter));
        }

        //resolve from type
        public object Resolve(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            //verify the container is registered
            if (_instanceRegistry.ContainsKey(type))
            {
                //make sure there is just one value for the key: one interface for the implementation
                if (_instanceRegistry[type].Count > 1)
                {
                    throw new MultipleImplementationsException(string.Format(
                        "The type {0} has Miltuple implementations, Please use ResolveAll ", type.Name));
                }

                return ResolveAndCreate(_instanceRegistry[type].Single());
            }

            //Type is not registered, throw exception
            throw new TypeNotRegisteredException(string.Format(
                "The type {0} has not been registered", type.Name));
        }

        //resolve all from type param
        public IEnumerable<Tinter> ResolveAll<Tinter>() where Tinter : class
        {
            var objects = ResolveAll(typeof(Tinter));
            foreach (var obj in objects)
            {
                yield return (Tinter)obj;
            }
        }

        //resolve all from type
        public IEnumerable<object> ResolveAll(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            //verify the container is registered
            if (_instanceRegistry.ContainsKey(type))
            {
                var implementations = _instanceRegistry[type];
                foreach (var implementation in implementations)
                {
                    yield return ResolveAndCreate(implementation);
                }
            }

            //Type is not registered, throw exception
            throw new TypeNotRegisteredException(string.Format(
                "The type {0} has not been registered", type.Name));
        }

        private object ResolveAndCreate(RegisteredType registered)
        {
            ConstructorInfo dependentCtor = GetConstructorInfo(registered.ObjectType);
            if (dependentCtor == null)
            {
                // use the default constructor to create
                return CreationService.Instance.GetInstance(registered);
            }

            //we got some parameter(types)s that need to be created
            return CreateCtor(registered, dependentCtor);
        }

        private ConstructorInfo GetConstructorInfo(Type typeToCreate)
        {
            //get ctor information so we can create the injected types later
            ConstructorInfo[] ctorInfo = typeToCreate.GetConstructors();

            //verify if the ctor has any types that need to be instantiated
            return ctorInfo.FirstOrDefault(
                item => item.CustomAttributes.FirstOrDefault(
                    attr => attr.AttributeType == typeof(DependencyAttribute)) != null);
        }

        //creates ctor and it dendencies if needed.
        private object CreateCtor(RegisteredType registered, ConstructorInfo dependentCtor)
        {
            // We found a constructor with dependency attribute
            ParameterInfo[] parameters = dependentCtor.GetParameters();
            if (parameters.Length == 0)
            {
                // Futile dependency attribute, use the default constructor only
                return CreationService.Instance.GetInstance(registered);
            }

            // valid dependency attribute, create the dependencies first and pass them to the constructor
            object[] arguments = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = Resolve(parameters[i].ParameterType);
            }

            return CreationService.Instance.GetInstance(registered, arguments);
        }
    }
}
