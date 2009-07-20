namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public class ControllerTypeConstraint<TControllerType> : IConstraint
    {
        private readonly ControllerDescriptor controllerDescriptor;

        public ControllerTypeConstraint()
        {
            controllerDescriptor = new ReflectedControllerDescriptor(typeof(TControllerType));
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return selector.ControllerDescriptor.ControllerType.Equals(controllerDescriptor.ControllerType);
        }
    }
}