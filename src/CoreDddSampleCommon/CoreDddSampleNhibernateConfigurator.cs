using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using CoreDddSampleWebAppCommon.Domain;

namespace CoreDddSampleWebAppCommon
{
    public class CoreDddSampleNhibernateConfigurator : NhibernateConfigurator
    {
        public CoreDddSampleNhibernateConfigurator()
        {
        }

        public CoreDddSampleNhibernateConfigurator(bool shouldMapDtos)
            : base(shouldMapDtos)
        {
        }

        protected override Assembly[] GetAssembliesToMap()
        {
            return new[] { typeof(Ship).Assembly };
        }
    }
}