using Autofac;

namespace dev.Core.IoC
{
    public static class CompositionRoot
    {
        private static IContainer _kernel = null;

        public static void Wire(IContainer kernel)
        {
            _kernel = kernel;
        }

        public static T Resolve<T>()
        {
            return _kernel.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return _kernel.ResolveNamed<T>(name);
        }
    }
}
