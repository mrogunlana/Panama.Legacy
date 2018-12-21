using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev.Core.IoC
{
    public static class ServiceLocator
    {
        private static IServiceLocator _serviceLocator;
        private static object _syncLock = new Object();

        public static IServiceLocator Current
        {
            get
            {
                IServiceLocator tempLocator = null;
                lock (_syncLock)
                {
                    tempLocator = _serviceLocator;
                }
                return tempLocator;
            }
        }

        public static void SetLocator(IServiceLocator locator)
        {
            lock(_syncLock)
            {
                _serviceLocator = locator;
            }
        }

    }
}
