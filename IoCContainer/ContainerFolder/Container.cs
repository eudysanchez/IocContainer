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
        Dictionary<Type, HashSet<RegisteredType>> _instanceRegistry;
        Dictionary<Type, RegisteredType> _processingRegistry;
        HashSet<RegisteredType> _registeredType;
        public Container()
        {
            _registeredType = new HashSet<RegisteredType>();
            _processingRegistry = new Dictionary<Type, RegisteredType>();
            _instanceRegistry = new Dictionary<Type, HashSet<RegisteredType>>();
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
            var key = typeof(Tinter);
            if (_instanceRegistry.ContainsKey(key) == true)
            {

                _instanceRegistry[key].Add(new RegisteredType
                {
                    LifeCycle = lifeCycle,
                    ObjectType = typeof(Timple)
                });
            }

            else
            {
                var registeredType = new RegisteredType
                {
                    LifeCycle = lifeCycle,
                    ObjectType = typeof(Timple)
                };

                _instanceRegistry.Add(
                    typeof(Tinter), new HashSet<RegisteredType> { registeredType });
            }


        }
        //resolve from type
        public Tinter Resolve<Tinter>()
        {
            if (_instanceRegistry.ContainsKey(typeof(Tinter)) == true)
            {
                //var registeredHash = _instanceRegistry[type];

                if (_instanceRegistry[typeof(Tinter)].Count() == 1)
                {
                    return (Tinter)ResolveAndCreate(typeof(Tinter));
                }
                //Type is not registered, throw exception
                else
                {
                    throw new TypeNotRegisteredException(string.Format(
                        "The type {0} has not been registered", typeof(Tinter)));
                }

            }
            //Type has multiple implementations, throw exception
            else
            {
                throw new MultipleImplementationsException(string.Format(
                    "The type {0} has Miltuple implementations ", typeof(Tinter)));
            }

        }
        //resolve from parameter
        public object Resolve(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return ResolveAndCreate(type);
        }

        //resolve for all implementations
        public IEnumerable<object> ResolveAll<Tinter>()
        {
            var implementations = _instanceRegistry[typeof(Tinter)];
            List<object> impleList = new List<object>();
            foreach (var implementation in implementations)
            {
                impleList.Add((Tinter)ResolveAndCreate(typeof(Tinter)));
            }
            return impleList;
        }

        //private void VerifyType(Type type)
        //{
        //    if (_instanceRegistry.ContainsKey(type) == true)
        //    {
        //        //var registeredHash = _instanceRegistry[type];

        //        if (_instanceRegistry[type].Count() == 1)
        //        {
        //            return (type)ResolveAndCreate(typeof(Tinter));
        //        }
        //        //Type is not registered, throw exception
        //        else
        //        {
        //            throw new TypeNotRegisteredException(string.Format(
        //                "The type {0} has not been registered", typeof(Tinter)));
        //        }

        //    }
        //    //Type has multiple implementations, throw exception
        //    else
        //    {
        //        throw new MultipleImplementationsException(string.Format(
        //            "The type {0} has Miltuple implementations ", typeof(Tinter)));
        //    }
        //}
        private object ResolveAndCreate(Type type)
        {
            object obj = null;

            if (_instanceRegistry.ContainsKey(type) == true)
            {
                var registeredHash = _instanceRegistry[type];

                if (registeredHash.Count() == 1)
                {
                    foreach (var registered in registeredHash)
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
                //Type has multiple implementations, throw exception
                else
                {
                    throw new MultipleImplementationsException(string.Format(
                        "The type {0} has Miltuple implementations ", type.Name));
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
