namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public class ControllerTypeConstraint<TController> : IConstraint
        where TController : Controller
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return selector.ControllerDescriptor.ControllerType.Equals(typeof(TController));
        }
    }
}