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
        Dictionary<Type, RegisteredType> _instanceRegistry;

        public Container()
        {
            _instanceRegistry = new Dictionary<Type, RegisteredType>();
        }
        //register type with Transient lifeCycle by default
        public void Register<Tinter, Timple>()
            where Tinter : class
            where Timple : class
        {
            RegisterType<Tinter, Timple>(LifeCycle.Transient);
        }
        //Let take the lifecyle choosen by the user
        public void Register<Tinter, Timple>(LifeCycle lifeCycle)
            where Tinter : class
            where Timple : class
        {
            RegisterType<Tinter, Timple>(lifeCycle);

        }
        /*Using the container as the key, adds type to register and container to the dictionary
         * deletes entry if type is already in the dictionary
         * Note: this behavior needs to be updated if we want to allow various implementations to one type  
         * */
        private void RegisterType<Tinter, Timple>(LifeCycle lifeCycle)
        {
            if (_instanceRegistry.ContainsKey(typeof(Tinter)) == true)
            {
                _instanceRegistry.Remove(typeof(Tinter));
            }

            _instanceRegistry.Add(
                typeof(Tinter),
                    new RegisteredType
                    {
                        LifeCycle = lifeCycle,
                        ObjectType = typeof(Timple)
                    }
                );
        }
        //resolve from type
        public Tinter Resolve<Tinter>()
        {
            return (Tinter)ResolveAndCreate(typeof(Tinter));
        }
        //resolve from parameter
        public object Resolve(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

           return ResolveAndCreate(type);
        }

        private object ResolveAndCreate(Type type)
        {
            object obj = null;

            if (_instanceRegistry.ContainsKey(type) == true)
            {
                RegisteredType registered = _instanceRegistry[type];

                if (registered != null)
                {
                    Type typeToCreate = registered.ObjectType;
                    //get ctro information so we can create the injected types later
                    ConstructorInfo[] ctroInfo = typeToCreate.GetConstructors();
                    //verify if the ctro has any types that need to be instantiated 
                    var dependentCtor = ctroInfo.FirstOrDefault(item => item.CustomAttributes.FirstOrDefault(attr => attr.AttributeType == typeof(DependencyAttribute)) != null);

                    if (dependentCtor == null)
                    {
                        // use the default constructor to create
                        obj = CreateInstance(registered);
                    }
                    else
                    {
                        // We found a constructor with dependency attribute
                        ParameterInfo[] parameters = dependentCtor.GetParameters();
                        if (parameters.Count() == 0)
                        {
                            // Futile dependency attribute, use the default constructor only
                            obj = CreateInstance(registered);
                        }
                        else
                        {
                            // valid dependency attribute, create the dependencies first and pass them to the constructor
                            List<object> arguments = new List<object>();
                            foreach (var param in parameters)
                            {
                                Type paramType = param.ParameterType;
                                arguments.Add(ResolveAndCreate(paramType));
                            }

                            obj = CreateInstance(registered, arguments.ToArray());
                        }
                    }
                }
            }
            //Type is not registered, throw exception
            else
            {
                throw new TypeNotRegisteredException(string.Format(
                    "The type {0} has not been registered", type.Name));
            }

            return obj;
        }

        //create the instance dending on the lifecycle type
        private object CreateInstance(RegisteredType registered, object[] arguments = null)
        {
            object returnedObj = null;
            Type typeToCreate = registered.ObjectType;

            if (registered.LifeCycle == LifeCycle.Transient)
            {
                returnedObj = CreationService.GetInstance().GetNewInstance(typeToCreate, arguments);
            }
            else if (registered.LifeCycle == LifeCycle.Singleton)
            {
                returnedObj = CreationService.GetInstance().GetSingleton(typeToCreate, arguments);
            }

            return returnedObj;
        }

    }
}
