using System;
using System.Configuration;
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
using CoreDdd.Nhibernate.Register.Ninject;
using CoreDdd.Queries;
using CoreDdd.Register.Castle;
using CoreDdd.Register.Ninject;
using CoreDdd.UnitOfWorks;
using CoreDddSampleAspNetMvcWebApp.Controllers;
using CoreDddSampleAspNetWebApiApp;
using CoreDddSampleWebAppCommon;
using CoreDddSampleWebAppCommon.Commands;
using CoreDddSampleWebAppCommon.Domain;
using CoreDddSampleWebAppCommon.Queries;
using Ninject;
using Ninject.Web.Common;
using Ninject.Extensions.Conventions;

namespace CoreDddSampleAspNetMvcWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private WindsorContainer _castleWindsorIoCContainer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var iocContainer = ConfigurationManager.AppSettings["IoCContainer"];
            switch (iocContainer)
            {
                case "Castle":
                    _RegisterServicesIntoCastleWindsorIoCContainer();
                    break;
                case "Ninject":
                    _RegisterServicesIntoNinjectIoCContainer();
                    break;
                default:
                    throw new Exception($"Unknown IoC container: {iocContainer}");
            }

            new DatabaseCreator().CreateDatabase();
        }

        protected void Application_End()
        {
            _castleWindsorIoCContainer?.Dispose();
        }

        private void _RegisterServicesIntoCastleWindsorIoCContainer()
        {
            _castleWindsorIoCContainer = new WindsorContainer();

            CoreDddNhibernateInstaller.SetUnitOfWorkLifeStyle(x => x.PerWebRequest);

            _castleWindsorIoCContainer.Install(
                FromAssembly.Containing<CoreDddInstaller>(),
                FromAssembly.Containing<CoreDddNhibernateInstaller>()
            );
            _castleWindsorIoCContainer.Register(
                Component
                    .For<INhibernateConfigurator>()
                    .ImplementedBy<CoreDddSampleNhibernateConfigurator>()
                    .LifestyleSingleton()
            );

            // register MVC controllers
            _castleWindsorIoCContainer.Register(
                Classes
                    .FromAssemblyContaining<HomeController>()
                    .BasedOn<ControllerBase>()
                    .Configure(x => x.LifestyleTransient())
            );

            // register command handlers
            _castleWindsorIoCContainer.Register(
                Classes
                    .FromAssemblyContaining<CreateNewShipCommandHandler>()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient())
            );
            // register query handlers
            _castleWindsorIoCContainer.Register(
                Classes
                    .FromAssemblyContaining<GetShipsByNameQueryHandler>()
                    .BasedOn(typeof(IQueryHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient())
            );
            // register domain event handlers
            _castleWindsorIoCContainer.Register(
                Classes 
                    .FromAssemblyContaining<ShipUpdatedDomainEventHandler>()
                    .BasedOn(typeof(IDomainEventHandler<>))
                    .WithService.FirstInterface()
                    .Configure(x => x.LifestyleTransient())
            );

            UnitOfWorkHttpModule.Initialize(
                _castleWindsorIoCContainer.Resolve<IUnitOfWorkFactory>(),
                isolationLevel: System.Data.IsolationLevel.ReadCommitted
                );

            DomainEvents.Initialize(
                _castleWindsorIoCContainer.Resolve<IDomainEventHandlerFactory>(),
                isDelayedDomainEventHandlingEnabled: true
            );

            ControllerBuilder.Current.SetControllerFactory(new IoCContainerCastleWindsorControllerFactory(_castleWindsorIoCContainer));
        }

        private void _RegisterServicesIntoNinjectIoCContainer()
        {
            var ninjectIoCContainer = NinjectWebCommon.Bootstrapper.Kernel;

            CoreDddNhibernateBindings.SetUnitOfWorkLifeStyle(x => x.InRequestScope());

            ninjectIoCContainer.Load(
                typeof(CoreDddBindings).Assembly,
                typeof(CoreDddNhibernateBindings).Assembly
            );
            ninjectIoCContainer
                .Bind<INhibernateConfigurator>()
                .To<CoreDddSampleNhibernateConfigurator>()
                .InSingletonScope();

            // register controllers
            ninjectIoCContainer.Bind(x => x
                .FromAssemblyContaining<HomeController>()
                .SelectAllClasses()
                .InheritedFrom<ControllerBase>()
                .BindAllInterfaces()
                .Configure(y => y.InTransientScope()));
            // register command handlers
            ninjectIoCContainer.Bind(x => x
                .FromAssemblyContaining<CreateNewShipCommandHandler>()
                .SelectAllClasses()
                .InheritedFrom(typeof(ICommandHandler<>))
                .BindAllInterfaces()
                .Configure(y => y.InTransientScope()));
            // register query handlers
            ninjectIoCContainer.Bind(x => x
                .FromAssemblyContaining<GetShipsByNameQueryHandler>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IQueryHandler<>))
                .BindAllInterfaces()
                .Configure(y => y.InTransientScope()));
            // register domain event handlers
            ninjectIoCContainer.Bind(x => x
                .FromAssemblyContaining<ShipUpdatedDomainEventHandler>()
                .SelectAllClasses()
                .InheritedFrom(typeof(IDomainEventHandler<>))
                .BindAllInterfaces()
                .Configure(y => y.InTransientScope()));

            UnitOfWorkHttpModule.Initialize(
                ninjectIoCContainer.Get<IUnitOfWorkFactory>(),
                isolationLevel: System.Data.IsolationLevel.ReadCommitted
            );

            DomainEvents.Initialize(
                ninjectIoCContainer.Get<IDomainEventHandlerFactory>(),
                isDelayedDomainEventHandlingEnabled: true
            );
        }
    }
}
