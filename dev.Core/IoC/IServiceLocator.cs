using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev.Core.IoC
{
    public interface IServiceLocator
    {
        T Resolve<T>();
        T Resolve<T>(string name);
    }
}
