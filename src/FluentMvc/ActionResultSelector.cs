namespace FluentMvc
{
    using System.Web.Mvc;

    public class ActionResultSelector : RegistrySelector
    {
        public object ReturnValue { get; set; }

        public ViewDataDictionary ViewData { get; set; }

        public ActionResultSelector()
        {
        }

        public ActionResultSelector(object returnValue, ControllerContext controllerContext, ActionDescriptor descriptor, ControllerDescriptor controllerDescriptor)
            : base(descriptor, controllerDescriptor, controllerContext)
        {
            ReturnValue = returnValue;
            SetViewData(controllerContext.Controller);
        }

        private void SetViewData(ControllerBase controllerContext)
        {
            if (controllerContext != null)
                ViewData = controllerContext.ViewData;
        }
    }
}