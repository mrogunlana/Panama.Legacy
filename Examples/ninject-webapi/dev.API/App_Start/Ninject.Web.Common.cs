[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(dev.API.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(dev.API.App_Start.NinjectWebCommon), "Stop")]

namespace dev.API.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using Ninject.Web.WebApi;
    using Panama.Commands;
    using Panama.Logger;
    using Panama.Security.Interfaces;
    using Panama.Sql;
    using Panama.Sql.Dapper;
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ILog>().To<NLog>();
            kernel.Bind<IQuery>().To<SqlQuery>();

            //Register all encryptors, validators, commands -- singletons
            //pattern: common interface by key/name
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IStringEncryptor).IsAssignableFrom(p))
                .ToList()
                .ForEach(x => 
                    kernel.Bind<IStringEncryptor>()
                        .To(x)
                        .InSingletonScope()
                        .Named(x.Name));

            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IValidation).IsAssignableFrom(p))
                .ToList()
                .ForEach(x => 
                    kernel.Bind<IValidation>()
                        .To(x)
                        .InSingletonScope()
                        .Named(x.Name));

            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ICommand).IsAssignableFrom(p))
                .ToList()
                .ForEach(x => 
                    kernel.Bind<ICommand>()
                        .To(x)
                        .InSingletonScope()
                        .Named(x.Name));
        }
    }
}