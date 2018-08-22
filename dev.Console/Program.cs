using Autofac;
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
            builder.RegisterType<Handler>().As<IHandler>();

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

            var scheduler = CompositionRoot.Resolve<Core.Jobs.IScheduler>();

            //TODO: Enter test logic here if needed...

            scheduler.Start();

            System.Console.WriteLine($"\nRunning ({scheduler.Count()}) Task(s) Every Minute.");
            System.Console.WriteLine("\nPress any key to exit.");
            System.Console.ReadKey();

            scheduler.Stop();
        }
    }
}
