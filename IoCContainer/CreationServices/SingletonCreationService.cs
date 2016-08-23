using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoCContainer.CreationService;
using IoCContainer.Exceptions;

namespace IoCContainer.CreationServices
{
    internal class SingletonCreationService
    {
        static SingletonCreationService instance = null;
        static Dictionary<string, object> registerList = new Dictionary<string, object>();

        static SingletonCreationService()
        {
            instance = new SingletonCreationService();
        }

        private SingletonCreationService()
        { }

        public static SingletonCreationService GetInstance()
        {
            return instance;
        }

        public object GetSingleton(Type type, object[] arguments = null)
        {
            object obj = null;

            try
            {
                if (registerList.ContainsKey(type.Name) == false)
                {
                    obj = TransientCreationService.GetInstance().GetNewObject(type, arguments);
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
    }
}
