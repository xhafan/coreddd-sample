using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using CoreDddSampleWebAppCommon.Domain;
using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace CoreDddSampleWebAppCommon
{
    public class CoreDddSampleNhibernateConfigurator : BaseNhibernateConfigurator
    {
        public CoreDddSampleNhibernateConfigurator(bool shouldMapDtos = true)
            : base(shouldMapDtos)
        {
#if DEBUG || REPOLINKS_DEBUG
            NHibernateProfiler.Initialize();
#endif
        }

        protected override Assembly[] GetAssembliesToMap()
        {
            return new[] { typeof(Ship).Assembly };
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