using System;
using System.Data.SQLite;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Repositories;
using CoreDdd.Nhibernate.UnitOfWorks;
using CoreDddSampleConsoleApp.Domain;
using NHibernate.Tool.hbm2ddl;

namespace CoreDddSampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var nhibernateConfigurator = new CoreDddSampleNhibernateConfigurator();
            _CreateDatabase(nhibernateConfigurator);

            using (var unitOfWork = new NhibernateUnitOfWork(nhibernateConfigurator))
            {
                unitOfWork.BeginTransaction();

                var productRepository = new NhibernateRepository<Product>(unitOfWork);

                try
                {
                    var product = new Product();

                    productRepository.Save(product);

                    unitOfWork.Commit();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private static void _CreateDatabase(INhibernateConfigurator nhibernateConfigurator)
        {
            var configuration = nhibernateConfigurator.GetConfiguration();
            var connectionString = configuration.Properties["connection.connection_string"];

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                new SchemaExport(configuration).Execute(
                        useStdOut: true,
                        execute: true,
                        justDrop: false,
                        connection: connection,
                        exportOutput: Console.Out)
                    ;
            }
        }
    }
}
