namespace FluentMvc
{
    using System.Web.Mvc;

    public abstract class ActionFilterSelector : RegistrySelector
    {
        public FilterScope Scope { get; protected set; }

        public ActionFilterSelector()
        {
        }

        public ActionFilterSelector(ControllerContext controllerContext, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(actionDescriptor, controllerDescriptor, controllerContext)
        {
        }
    }

    public class EmptyActionFilterSelector : ActionFilterSelector
    {
    }
}