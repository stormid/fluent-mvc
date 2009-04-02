namespace FluentMvc.Spec
{
    using System.Web.Mvc;
    using Constraints;

    public class FalseReturningConstraint : IConstraint
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return false;
        }

        public bool AppliesToController(ControllerDescriptor descriptor)
        {
            return false;
        }

        public void SetTargetController(ControllerDescriptor controllerDescriptor)
        {
            
        }
    }
}