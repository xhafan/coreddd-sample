using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using CoreDddSampleConsoleApp.Domain;
using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace CoreDddSampleConsoleApp
{
    public class CoreDddSampleNhibernateConfigurator : BaseNhibernateConfigurator
    {
        public CoreDddSampleNhibernateConfigurator(
            bool shouldMapDtos = true,
            string configurationFileName = null,
            string connectionString = null
            )
            : base(shouldMapDtos, configurationFileName, connectionString)
        {
#if DEBUG || REPOLINKS_DEBUG
            NHibernateProfiler.Initialize();
#endif
        }

        protected override Assembly[] GetAssembliesToMap()
        {
            return new[] {typeof(Ship).Assembly};
        }

        protected override IEnumerable<Type> GetIncludeBaseTypes()
        {
            return base.GetIncludeBaseTypes().Union(new[]
            {
                typeof(PolicyItem),
                typeof(CargoPolicyItem)
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
#if DEBUG || REPOLINKS_DEBUG
                NHibernateProfiler.Shutdown();
#endif
            }
            base.Dispose(disposing);
        }
    }
}