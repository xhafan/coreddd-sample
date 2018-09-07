using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using CoreDddSampleCommon.Domain;

namespace CoreDddSampleCommon
{
    public class CoreDddSampleNhibernateConfigurator : NhibernateConfigurator
    {
        public CoreDddSampleNhibernateConfigurator(bool shouldMapDtos = true)
            : base(shouldMapDtos)
        {
        }

        protected override Assembly[] GetAssembliesToMap()
        {
            return new[] { typeof(Ship).Assembly };
        }
    }
}