using Autofac;
using dev.Business.Commands;
using dev.Business.Validators;
using dev.Core.Commands;
using dev.Core.IoC;
using dev.Core.Logger;
using dev.Core.Security.Interfaces;
using dev.Core.Sql;
using dev.Entities.Models;
using System;
using System.Linq;

namespace dev.Console
{
    class Program
    {
        static void Register()
        {
            var builder = new ContainerBuilder();

            //INFRASTRUCTURE
            builder.RegisterType<NLog>().As<ILog>();
            builder.RegisterType<SqlQuery>().As<IQuery>();
            builder.RegisterType<Handler>().As<IHandler>();

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
                   .SingleInstance();

            var container = builder.Build();

            CompositionRoot.Wire(container);
        }
        static void Main(string[] args)
        {
            Register();

            var result = CompositionRoot.Resolve<IHandler>()
                .Add(new User(){
                    FirstName = "User with role id",
                    LastName = "Last",
                    Email = "First.role@test.com",
                    Password = "abc1234",
                    ConfirmPassword = "abc1234"
                })
                .Validate<FirstNameNotNullOrEmpty>()
                .Validate<EmailNotNullOrEmpty>()
                .Validate<PasswordNotNullOrEmpty>()
                .Validate<ConfirmPasswordNotNullOrEmpty>()
                .Validate<PasswordAndConfirmPasswordMustMatch>()
                .Validate<EmailNotExist>()
                .Command<GenerateUserId>()
                .Command<HashUserPassword>()
                .Command<SaveUser>()
                .Invoke();

            System.Console.WriteLine("\nPress any key to exit.");
            System.Console.ReadKey();
        }
    }
}
