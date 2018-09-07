using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CoreDdd.AspNet.HttpModules;
using CoreDdd.Commands;
using CoreDdd.Domain.Events;
using CoreDdd.Nhibernate.Configurations;
using CoreDdd.Nhibernate.Register.Castle;
using CoreDdd.Queries;
using CoreDdd.Register.Castle;
using CoreDdd.UnitOfWorks;
using CoreDddSampleAspNetWebApp.Controllers;
using CoreDddSampleCommon;
using CoreDddSampleCommon.Commands;
using CoreDddSampleCommon.Domain;
using CoreDddSampleCommon.Queries;

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

            // register controllers, command handlers, query handlers and domain event handlers
            iocContainer.Register(
                Classes
                    .FromAssemblyContaining<HomeController>()
                    .BasedOn<ControllerBase>()
                    .Configure(x => x.LifestyleTransient()),
                Classes
                    .FromAssemblyContaining<CreateNewShipCommandHandler>()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Classes
                    .FromAssemblyContaining<GetShipsByNameQueryHandler>()
                    .BasedOn(typeof(IQueryHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient()),
                Classes
                    .FromAssemblyContaining<ShipUpdatedDomainEventHandler>()
                    .BasedOn(typeof(IDomainEventHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient())
            );

            UnitOfWorkHttpModule.Initialize(iocContainer.Resolve<IUnitOfWorkFactory>());

            ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory(iocContainer));

            new DatabaseCreator().CreateDatabase().Wait();

            DomainEvents.Initialize(
                iocContainer.Resolve<IDomainEventHandlerFactory>(),
                isDelayedDomainEventHandlingEnabled: true
                );
        }
    }
}
