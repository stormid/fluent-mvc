namespace FluentMvc
{
    using System.Web;
    using System.Web.Mvc;

    public abstract class RegistrySelector
    {
        public ActionDescriptor ActionDescriptor { get; set; }
        public ControllerDescriptor ControllerDescriptor { get; set; }
        public ControllerContext ControllerContext { get; set; }
        public string[] AcceptTypes { get; set; }

        protected RegistrySelector()
        {
        }

        protected RegistrySelector(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, ControllerContext controllerContext)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
            ControllerContext = controllerContext;

            // HACK: ControllerContext.HttpContext default value is EmptyHttpContext, which is private sealed, so we can't check for that type... :(
            if (ControllerContext != null && typeof(HttpContextWrapper).IsAssignableFrom(ControllerContext.HttpContext.GetType()) && ControllerContext.HttpContext.Request != null)
                AcceptTypes = ControllerContext.HttpContext.Request.AcceptTypes;
        }
    }
}