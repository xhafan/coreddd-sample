using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.AspNet.HttpModules;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Register.Castle;
using CoreDdd.UnitOfWorks;

namespace CoreDddSampleAspNetWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            var iocContainer = new WindsorContainer();

            CoreDddNhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerWebRequest);

            iocContainer.Install(
                FromAssembly.Containing<CoreDddInstaller>(),
                FromAssembly.Containing<CoreDddNhibernateInstaller>()
            );
            iocContainer.Register(
                Component
                    .For<INhibernateConfigurator>()
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifestyleSingleton()
            );

            UnitOfWorkHttpModule.Initialize(iocContainer.Resolve<IUnitOfWorkFactory>());
        }
    }
}
