namespace FluentMvc.Spec
{
    using System.Web.Mvc;
    using Constraints;

    public class TrueReturningConstraint : IConstraint
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return true;
        }

        public bool AppliesToController(ControllerDescriptor descriptor)
        {
            return true;
        }

        public void SetTargetController(ControllerDescriptor controllerDescriptor)
        {
            
        }
    }
}