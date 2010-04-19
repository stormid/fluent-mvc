using System;
using System.Web.Routing;

namespace FluentMvc
{
    using System.Web;
    using System.Web.Mvc;
    using Utils;

    public abstract class RegistrySelector
    {
        private string[] acceptTypes;

        public ActionDescriptor ActionDescriptor { get; set; }
        public ControllerDescriptor ControllerDescriptor { get; set; }
        public ControllerContext ControllerContext { get; set; }

        public string[] AcceptTypes
        {
            get { return acceptTypes.HasItems() ? acceptTypes : new string[] {}; }
            set { acceptTypes = value; }
        }

        public RouteData RouteData { get; private set; }

        protected RegistrySelector()
        {
        }

        protected RegistrySelector(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, ControllerContext controllerContext)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
            ControllerContext = controllerContext;

            if (ControllerContextIsValid()) RegisterControllerRequestData(ControllerContext.HttpContext.Request);
            if (ControllerRouteDataIsValid()) RegisterControllerRouteData(ControllerContext.RouteData);
        }

        private void RegisterControllerRouteData(RouteData routeData)
        {
            RouteData = routeData;
        }

        private void RegisterControllerRequestData(HttpRequestBase request)
        {
            AcceptTypes = request.AcceptTypes;
        }

        private bool ControllerRouteDataIsValid()
        {
            return ControllerContext != null && ControllerContext.RouteData != null;
        }

        private bool ControllerContextIsValid()
        {
            // HACK: ControllerContext.HttpContext default value is EmptyHttpContext, which is private sealed, so we can't check for that type... :(
            return ControllerContext != null && typeof(HttpContextWrapper).IsAssignableFrom(ControllerContext.HttpContext.GetType()) && ControllerContext.HttpContext.Request != null;
        }
    }
}