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

        public bool IsAjaxRequest { get; private set; }

        protected RegistrySelector()
        {
        }

        protected RegistrySelector(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, ControllerContext controllerContext)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
            ControllerContext = controllerContext;

            if (ControllerContextIsValid())
            {
                RegisterControllerRequestData(ControllerContext.HttpContext.Request);
            }
        }

        private void RegisterControllerRequestData(HttpRequestBase request)
        {
            AcceptTypes = request.AcceptTypes;
            IsAjaxRequest = request.IsAjaxRequest();
        }

        private bool ControllerContextIsValid()
        {
            // HACK: ControllerContext.HttpContext default value is EmptyHttpContext, which is private sealed, so we can't check for that type... :(
            return ControllerContext != null && typeof(HttpContextWrapper).IsAssignableFrom(ControllerContext.HttpContext.GetType()) && ControllerContext.HttpContext.Request != null;
        }
    }
}