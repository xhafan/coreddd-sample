using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Syntax;

namespace CoreDddSampleAspNetMvcWebApp
{
    public class IoCContainerNinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IResolutionRoot _iocContainer;

        public IoCContainerNinjectControllerFactory(IResolutionRoot iocContainer)
        {
            _iocContainer = iocContainer;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, null);
            }

            var controller = (IController)_iocContainer.Get(controllerType);
            return controller;
        }

        public override void ReleaseController(IController controller)
        {
            _iocContainer.Release(controller);
            base.ReleaseController(controller);
        }
    }
}