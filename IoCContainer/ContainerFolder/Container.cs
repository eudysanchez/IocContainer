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
        Dictionary<Type, RegisteredType> _processingRegistry;//store a specific container and all its implementations temporally
        public Container()
        {
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
            //verify the container is registered 
            if (_instanceRegistry.ContainsKey(typeof(Tinter)))
            {
                //make sure there is just one value for the key: one interface for the implementation
                if (_instanceRegistry[typeof(Tinter)].Count() == 1)
                {
                    _processingRegistry = new Dictionary<Type, RegisteredType>();
                    _processingRegistry[typeof(Tinter)] = _instanceRegistry[typeof(Tinter)].FirstOrDefault();
                    return (Tinter)ResolveAndCreate(typeof(Tinter));
                }
                //Type has multiple implementations, throw exception
                else
                {
                    throw new MultipleImplementationsException(string.Format(
                        "The type {0} has Miltuple implementations, Please use ResolveAll ", typeof(Tinter).Name));
                }

            }
            //Type is not registered, throw exception
            else
            {
                throw new TypeNotRegisteredException(string.Format(
                    "The type {0} has not been registered", typeof(Tinter).Name));
            }
        }

        //resolve from parameter
        public object Resolve(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            //verify the container is registered 
            if (_instanceRegistry.ContainsKey(type))
            {
                //make sure there is just one value for the key: one interface for the implementation
                if (_instanceRegistry[type].Count() == 1)
                {
                    _processingRegistry = new Dictionary<Type, RegisteredType>();
                    _processingRegistry[type] = _instanceRegistry[type].FirstOrDefault();
                    return ResolveAndCreate(type);
                }
                //Type has multiple implementations, throw exception
                else
                {
                    throw new MultipleImplementationsException(string.Format(
                        "The type {0} has Miltuple implementations, Please use ResolveAll ", type.Name));
                }
            }
            //Type is not registered, throw exception
            else
            {
                throw new TypeNotRegisteredException(string.Format(
                    "The type {0} has not been registered", type.Name));
            }
        }

        //resolve for all implementations
        public IEnumerable<Type> ResolveAll<Tinter>()
        {
            int i = 0;
            List<Type> impleList = new List<Type>();
            //verify the container is registered 
            if (_instanceRegistry.ContainsKey(typeof(Tinter)))
            {
                var implementations = _instanceRegistry[typeof(Tinter)];
                
                foreach (var implementation in implementations)
                {
                    _processingRegistry = new Dictionary<Type, RegisteredType>();
                    var temp = _instanceRegistry[typeof(Tinter)].ToArray();
                    _processingRegistry[typeof(Tinter)] = temp[i];
                    impleList.Add(ResolveAndCreate(typeof(Tinter)).GetType());

                    i++;
                }
            }
            //Type is not registered, throw exception
            else
            {
                throw new TypeNotRegisteredException(string.Format(
                    "The type {0} has not been registered", typeof(Tinter).Name));
            }
            return impleList;
        }

        private object ResolveAndCreate(Type type)
        {
            object obj = null;

            var registered = _processingRegistry[type];
            Type typeToCreate = registered.ObjectType;

            ConstructorInfo dependentCtor = GetConstructorInfo(typeToCreate); 
            if (dependentCtor == null)
            {
                // use the default constructor to create
                obj = CreateInstance(registered);
            }
            else
            {
                //we got some parameter(types)s that need to be created
                CreateCtro(registered, dependentCtor, out obj);
            }

            return obj;
        }

        private ConstructorInfo GetConstructorInfo(Type typeToCreate)
        {
            //get ctro information so we can create the injected types later
            ConstructorInfo[] ctroInfo = typeToCreate.GetConstructors();

            //verify if the ctro has any types that need to be instantiated
            return ctroInfo.FirstOrDefault(
                item => item.CustomAttributes.FirstOrDefault(
                    attr => attr.AttributeType == typeof(DependencyAttribute)) != null);

        }

        //creates ctro and it dendencies if needed.
        private void CreateCtro(RegisteredType registered, ConstructorInfo dependentCtor, out object obj)
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
                    _processingRegistry[paramType] = _instanceRegistry[paramType].FirstOrDefault();
                    arguments.Add(ResolveAndCreate(paramType));
                }

                obj = CreateInstance(registered, arguments.ToArray());
            }
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
