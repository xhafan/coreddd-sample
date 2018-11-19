using System.Configuration;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CoreDddSampleAspNetWebApiApp.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CoreDddSampleAspNetWebApiApp.NinjectWebCommon), "Stop")]

namespace CoreDddSampleAspNetWebApiApp
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon
    {
        public static Bootstrapper Bootstrapper;
        private static bool _isNinjectUsedAsIoCContainer;

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            _isNinjectUsedAsIoCContainer = ConfigurationManager.AppSettings["IoCContainer"] == "Ninject";
            if (!_isNinjectUsedAsIoCContainer) return;

            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper = new Bootstrapper();
            Bootstrapper.Initialize(() => new StandardKernel());
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper?.ShutDown();
        }
    }
}

