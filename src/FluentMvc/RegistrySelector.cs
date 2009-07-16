namespace FluentMvc
{
    using System.Web;
    using System.Web.Mvc;
    using Utils;

    public abstract class RegistrySelector
    {
        public ActionDescriptor ActionDescriptor { get; set; }
        public ControllerDescriptor ControllerDescriptor { get; set; }
        public ControllerContext ControllerContext { get; set; }
        private string[] acceptTypes;
        public string[] AcceptTypes
        {
            get { return acceptTypes.HasItems() ? acceptTypes : new string[] {}; }
            set { acceptTypes = value; }
        }

        public bool IsAjaxRequest { get; set; }

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
            {
                AcceptTypes = ControllerContext.HttpContext.Request.AcceptTypes;
                IsAjaxRequest = ControllerContext.HttpContext.Request.IsAjaxRequest();
            }
        }
    }
}