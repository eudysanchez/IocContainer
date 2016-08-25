using IoCContainer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Creare type intance 
namespace IoCContainer.IoCCreationServices 
{
    public class CreationService
    {
        static CreationService instance = null;
        static Dictionary<string, object> registerList = new Dictionary<string, object>();

        static CreationService()
        {
            instance = new CreationService();
        }

        private CreationService()
        { }

        public static CreationService GetInstance()
        {
            return instance;
        }
        //returns the intance of the object
        public object GetSingleton(Type type, object[] arguments = null)
        {
            object obj = null;

            try
            {   //if type is a new type, then create it and ad it to the dictionary
                if (!registerList.ContainsKey(type.Name))
                {
                    obj = GetInstance().GetNewInstance(type, arguments);
                    registerList.Add(type.Name, obj);
                }
                //else, just get type from the list 
                else
                {
                    obj = registerList[type.Name];
                }
            }
            catch (Exception e)
            {
                throw new FailedToCreateInstanceException(
                    string.Format("Can't create instance of type {0} ", type.Name), e.InnerException);
            }

            return obj;
        }

        //returns a new instance of the object
        public object GetNewInstance(Type type, object[] arguments = null)
        {
            object obj = null;
            
            try
            {//create new instance
                obj = Activator.CreateInstance(type, arguments);
            }
            catch (Exception e)
            {
                throw new FailedToCreateInstanceException(
                    string.Format("Can't create instance of type {0}", type.Name), e.InnerException);
            }

            return obj;
        }
    }
}
