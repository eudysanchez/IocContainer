using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoCContainer.Exceptions;

namespace IoCContainer.CreationServices
{
    internal class TransientCreationService
    {
        static TransientCreationService _instance = null;

        static TransientCreationService()
        {
            _instance = new TransientCreationService();
        }

        private TransientCreationService()
        { }

        public static TransientCreationService GetInstance()
        {
            return _instance;
        }

        public object GetNewObject(Type type, object[] arguments = null)
        {
            object obj = null;

            try
            {
                obj = Activator.CreateInstance(type, arguments);
            }
            catch(Exception e)
            {
                throw new FailedToCreateInstanceException(
                    string.Format("Can't create instance of type {0} ", type.Name), e.InnerException);
            }

            return obj;
        }
    }
}
