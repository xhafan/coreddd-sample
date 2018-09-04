using System.Reflection;
using CoreDdd.Nhibernate.Configurations;

namespace CoreDddAspNetCoreWebApp
{
    public class CoreDddSampleNhibernateConfigurator : NhibernateConfigurator
    {
        protected override Assembly[] GetAssembliesToMap()
        {
            return new Assembly[0];
        }
    }
}