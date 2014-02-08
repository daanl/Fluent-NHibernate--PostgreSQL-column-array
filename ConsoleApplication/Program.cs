using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;
using System.Linq;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            throw new Exception("Adjust the connectionString in App.config -> appSettings -> connectionString");

            var sessionFactory = _sessionFactory.Value;

            CreateDatabase();

            using (var session = sessionFactory.OpenSession())
            {
                var project = new Project
                {
                    Tags = new List<string>
                    {
                        "TagOne",
                        "TagTwo",
                        "TagThree"
                    }
                };

                session.Save(project);
                session.Flush();
                session.Evict(project);

                var projectFromDatabase = session.Load<Project>(project.Id);

                Console.WriteLine("Project: {0} with {1} tags: {2}", projectFromDatabase.Id, projectFromDatabase.Tags.Count(), string.Join(",", projectFromDatabase.Tags));
            }

            Console.ReadLine();
        }

        #region DatabaseStuff

        private static void CreateDatabase()
        {
            var schemaExport = new SchemaExport(_configuration.Value);
            schemaExport.Drop(false, true);
            schemaExport.Create(false, true);
        }

        private static Lazy<ISessionFactory> _sessionFactory = new Lazy<ISessionFactory>(() =>
        {
            var configuration = _configuration.Value;

            var sessionFactory = Fluently.Configure(configuration)
                .Mappings(m =>
                    m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();

            return sessionFactory;
        });

        private static Lazy<Configuration> _configuration = new Lazy<Configuration>(() =>
        {
            var configuration = new Configuration();

            configuration.SetProperty(
                NHibernate.Cfg.Environment.ConnectionString,
                ConfigurationManager.AppSettings["connectionString"]
            );

            configuration.Configure();

            return configuration;
        });

        #endregion
    }
}
