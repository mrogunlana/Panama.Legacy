using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Panama.IoC;
using Panama.IoC.Autofac;
using Panama.Logger;
using Panama.Sql;
using Panama.Sql.Dapper;
using Panama.Tests.Models;

namespace Panama.Tests
{
    [TestClass]
    public class BatchTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SqlGeneratorImpl>()
               .As<ISqlGenerator>()
               .WithParameter("configuration", new DapperExtensionsConfiguration(typeof(ClassMapper<>), AppDomain.CurrentDomain.GetAssemblies(), new SqlServerDialect()))
               .SingleInstance();

            builder.RegisterType<NLog>().As<ILog>();
            builder.RegisterType<SqlQuery>().As<IQuery>();

            ServiceLocator.SetLocator(new AutofacServiceLocator(builder.Build()));
        }

        [TestCleanup]
        public void Cleanup()
        {
            var sql = ServiceLocator.Current.Resolve<IQuery>();

            sql.Execute("delete from [User]", null);
        }

        [TestMethod]
        public void CanBatchInsert10Users()
        {
            var sql = ServiceLocator.Current.Resolve<IQuery>();
            var users = new List<User>();

            for (int i = 0; i < 1000000; i++)
                users.Add(new User() {
                    ID = Guid.NewGuid(),
                    UserName = $"User{i}"
                });

            sql.InsertBatch(users);

            var results = sql.ExecuteScalar<int>($"select count(*) from [User]", null);

            Assert.AreEqual(users.Count, results);
        }
    }
}
