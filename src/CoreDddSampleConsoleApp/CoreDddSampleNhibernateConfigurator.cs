using System.Reflection;
using CoreDdd.Nhibernate.Configurations;
using CoreDddSampleConsoleApp.Domain;

namespace CoreDddSampleConsoleApp
{
    public class CoreDddSampleNhibernateConfigurator : NhibernateConfigurator
    {
        public CoreDddSampleNhibernateConfigurator()
            : base(mapDtoAssembly: false)
        {
        }

        protected override Assembly[] GetAssembliesToMap(bool mapDtoAssembly)
        {
            return new[] { typeof(Product).Assembly };
        }
    }
}