using System.Reflection;
using CoreDdd.Nhibernate.Configurations;

namespace CoreDddSampleAspNetWebApp
{
    public class CoreDddSampleNhibernateConfigurator : NhibernateConfigurator
    {
        protected override Assembly[] GetAssembliesToMap()
        {
            return new Assembly[0];
        }
    }
}