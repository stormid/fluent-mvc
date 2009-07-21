namespace FluentMvc
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class FluentMvcControllerFactory : IControllerFactory
    {
        private readonly IControllerFactory innerControllerFactory;
        private readonly IFluentMvcResolver resolver;

        public FluentMvcControllerFactory(IControllerFactory innerControllerFactory, IFluentMvcResolver resolver)
        {
            this.innerControllerFactory = innerControllerFactory;
            this.resolver = resolver;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controller = innerControllerFactory.CreateController(requestContext, controllerName) as Controller;

            if (controller == null)
                throw new HttpException(404, string.Format("The controller for path '{0}' could not be found or it does not implement IController.", "Arrgh!"));

            controller.ActionInvoker = FluentMvcActionInvoker.Create(resolver);
            
            return controller;
        }

        public void ReleaseController(IController controller)
        {
            innerControllerFactory.ReleaseController(controller);
        }
    }
}