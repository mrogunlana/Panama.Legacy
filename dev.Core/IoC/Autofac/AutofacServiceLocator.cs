using Autofac;

namespace dev.Core.IoC
{
    public class AutofacServiceLocator : IServiceLocator
    {
        private static Autofac.IContainer _kernel = null;

        public AutofacServiceLocator(Autofac.IContainer kernel)
        {
            _kernel = kernel;
        }

        public T Resolve<T>()
        {
            return _kernel.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return _kernel.ResolveNamed<T>(name);
        }
    }
}
