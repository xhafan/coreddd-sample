using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;
using Castle.MicroKernel.Lifestyle;

namespace CoreDddSampleAspNetWebApiApp
{
    // https://stackoverflow.com/a/20071808/379279
    public class IoCContainerCastleWindsorDependencyResolver : IDependencyResolver
    {
        private readonly IWindsorContainer _container;

        public IoCContainerCastleWindsorDependencyResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetService(Type t)
        {
            return _container.Kernel.HasComponent(t)
                ? _container.Resolve(t) : null;
        }

        public IEnumerable<object> GetServices(Type t)
        {
            return _container.ResolveAll(t).Cast<object>().ToArray();
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container);
        }

        public void Dispose()
        {
        }

        private class WindsorDependencyScope : IDependencyScope
        {
            private readonly IWindsorContainer _container;
            private readonly IDisposable _scope;

            public WindsorDependencyScope(IWindsorContainer container)
            {
                _container = container;
                _scope = container.BeginScope();
            }

            public object GetService(Type t)
            {
                return _container.Kernel.HasComponent(t) ? _container.Resolve(t) : null;
            }

            public IEnumerable<object> GetServices(Type t)
            {
                return _container.ResolveAll(t).Cast<object>().ToArray();
            }

            public void Dispose()
            {
                _scope.Dispose();
            }
        }
    }
}