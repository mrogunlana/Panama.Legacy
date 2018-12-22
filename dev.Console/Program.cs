using Autofac;
using dev.Business.Validators;
using dev.Core.Commands;
using dev.Core.IoC;
using dev.Core.Logger;
using dev.Core.Sql;
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

            //Register all commands -- singletons
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsAssignableTo<ICommand>())
                   .Named<ICommand>(t => t.Name)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            //Register all validators -- singletons
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                   .Where(t => t.IsAssignableTo<IValidation>())
                   .Named<IValidation>(t => t.Name)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            IContainer container = builder.Build();
            ServiceLocator.SetLocator(new AutofacServiceLocator(container));

        }
        static void Main(string[] args)
        {
            Register();

            var result = new Handler(ServiceLocator.Current)
                .Add(new Entities.Models.User(){
                    LastName = "Smith"
                })
                .Validate<FirstNameNotNullOrEmpty>()
                .Validate<EmailNotNullOrEmpty>()
                .Invoke();

            System.Console.WriteLine(string.Join(",", result.Messages.ToArray()));
            System.Console.ReadKey();

            /*
            var scheduler = CompositionRoot.Resolve<Core.Jobs.IScheduler>();

            //TODO: Enter test logic here if needed...

            scheduler.Start();

            System.Console.WriteLine($"\nRunning ({scheduler.Count()}) Task(s) Every Minute.");
            System.Console.WriteLine("\nPress any key to exit.");
            System.Console.ReadKey();

            scheduler.Stop();
            */

        }
    }
}