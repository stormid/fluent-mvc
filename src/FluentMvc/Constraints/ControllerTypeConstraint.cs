namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public class ControllerTypeConstraint : IConstraint
    {
        private readonly ControllerDescriptor controllerDescriptor;

        public ControllerTypeConstraint(ControllerDescriptor controllerDescriptor)
        {
            this.controllerDescriptor = controllerDescriptor;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return selector.ControllerDescriptor.ControllerType.Equals(controllerDescriptor.ControllerType);
        }
    }
}