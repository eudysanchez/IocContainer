﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using IoCContainer.CreationServices;
using IoCContainer.Exceptions;

namespace IoCContainer.Container
{
    public class Container : IContainer
    {
        Dictionary<Type, RegisteredType> instanceRegistry = new Dictionary<Type, RegisteredType>();

        public void Register<Tinter, Timple>()
            where Tinter : class
            where Timple : class
        {
            RegisterTransientType<Tinter, Timple>(LifeCycle.Transient);
        }

        public void Register<Tinter, Timple>(LifeCycle lifeCycle)
            where Tinter : class
            where Timple : class
        {
            RegisterTransientType<Tinter, Timple>(lifeCycle);

        }
        public void RegisterTransientType<Tinter, Timple>(LifeCycle lifeCycle)
        {
            RegisterType<Tinter, Timple>(lifeCycle);
        }

        public void RegisterSingletonType<Tinter, Timple>(LifeCycle lifeCycle)
        {
            RegisterType<Tinter, Timple>(lifeCycle);
        }

        private void RegisterType<Tinter, Timple>(LifeCycle lifeCycle)
        {
            if (instanceRegistry.ContainsKey(typeof(Tinter)) == true)
            {
                instanceRegistry.Remove(typeof(Tinter));
            }

            instanceRegistry.Add(
                typeof(Tinter),
                    new RegisteredType
                    {
                        LifeCycle = lifeCycle,
                        ObjectType = typeof(Timple)
                    }
                );
        }

        public Tinter Resolve<Tinter>()
        {
            return (Tinter)Resolve(typeof(Tinter));
        }

        private object Resolve(Type type)
        {
            object obj = null;

            if (instanceRegistry.ContainsKey(type) == true)
            {
                RegisteredType registered = instanceRegistry[type];

                if (registered != null)
                {
                    Type typeToCreate = registered.ObjectType;

                    ConstructorInfo[] ctroInfo = typeToCreate.GetConstructors();

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
                                arguments.Add(Resolve(paramType));
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

        private object CreateInstance(RegisteredType registered, object[] arguments = null)
        {
            object returnedObj = null;
            Type typeToCreate = registered.ObjectType;

            if (registered.LifeCycle == LifeCycle.Transient)
            {
                returnedObj = TransientCreationService.GetInstance().GetNewObject(typeToCreate, arguments);
            }
            else if (registered.LifeCycle == LifeCycle.Singleton)
            {
                returnedObj = SingletonCreationService.GetInstance().GetSingleton(typeToCreate, arguments);
            }

            return returnedObj;
        }

    }
}