namespace FluentMvc
{
    using System.Web.Mvc;

    public class ActionFilterSelector : RegistrySelector
    {
        public ActionFilterSelector()
        {
        }

        public ActionFilterSelector(ControllerContext controllerContext, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(actionDescriptor, controllerDescriptor, controllerContext)
        {
        }
    }
}