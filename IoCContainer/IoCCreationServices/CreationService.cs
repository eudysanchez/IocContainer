using IoCContainer.Exceptions;
using IoCContainer.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Creare intance type 
namespace IoCContainer.IoCCreationServices
{
    internal class CreationService
    {
        //private static readonly CreationService instance = new CreationService();

        // Lazy init
        private static CreationService instance = null;

        private readonly Dictionary<Type, object> registerList = new Dictionary<Type, object>();

        private CreationService() { }

        //returns the instance Property
        public static CreationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CreationService();
                }
                return instance;
            }
        }

        public object GetInstance(RegisteredType registered, object[] arguments = null)
        {
            if (registered.LifeCycle == LifeCycle.Transient)
            {
                return GetNewInstance(registered.ObjectType, arguments);
            }

            return GetSingleton(registered.ObjectType, arguments);
        }

        //returns the intance of the object
        private object GetSingleton(Type type, object[] arguments = null)
        {
            //if type is a new type, then create it and add it to the dictionary
            if (!registerList.ContainsKey(type))
            {
                registerList.Add(type, GetNewInstance(type, arguments));
            }

            return registerList[type];
        }

        //returns a new instance of the object
        private object GetNewInstance(Type type, object[] arguments = null)
        {
            try
            {
                //create new instance
                return Activator.CreateInstance(type, arguments);
            }
            catch (Exception e)
            {
                throw new FailedToCreateInstanceException(
                    string.Format("Can't create instance of type {0}", type.Name), e.InnerException);
            }
        }
    }
}