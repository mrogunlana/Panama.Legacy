using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.WebApi;
using dev.Core.Commands;
using dev.Core.IoC;
using dev.Core.IoC.Autofac;
using dev.Core.Logger;
using dev.Core.Security;
using dev.Core.Security.Interfaces;
using dev.Core.Sql;
using dev.Core.Sql.Dapper;
using Microsoft.Owin;
using Owin;
using System;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(dev.Api.Startup))]
namespace dev.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<NLog>().As<ILog>();
            builder.RegisterType<SqlQuery>().As<IQuery>();
            builder.RegisterType<AESEncryptor>().Keyed<IStringEncryptor>(nameof(AESEncryptor));
            builder.RegisterType<Base64Encryptor>().Keyed<IStringEncryptor>(nameof(Base64Encryptor));

            //Register all encryptors -- singletons
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsAssignableTo<IStringEncryptor>())
                   .Named<IStringEncryptor>(t => t.Name)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            //Register all validators -- singletons
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsAssignableTo<IValidation>())
                   .Named<IValidation>(t => t.Name)
                   .AsImplementedInterfaces()
                   .SingleInstance();
                   

            //Register all commands -- singletons
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsAssignableTo<ICommand>())
                   .Named<ICommand>(t => t.Name)
                   .AsImplementedInterfaces()
                   .SingleInstance()
                   .WithAttributeFiltering();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            //builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            //builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            ServiceLocator.SetLocator(new AutofacServiceLocator(container));

            ConfigureAuth(app);

            var config = new HttpConfiguration();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            WebApiConfig.Register(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            //OWIN stuff goes here..
        }
    }
}