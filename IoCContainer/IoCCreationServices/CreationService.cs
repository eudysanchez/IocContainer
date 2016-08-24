using IoCContainer.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public object GetSingleton(Type type, object[] arguments = null)
        {
            object obj = null;

            try
            {
                if (!registerList.ContainsKey(type.Name))
                {
                    obj = GetInstance().GetNewObject(type, arguments);
                    registerList.Add(type.Name, obj);
                }
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

        public object GetNewObject(Type type, object[] arguments = null)
        {
            object obj = null;

            try
            {
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
