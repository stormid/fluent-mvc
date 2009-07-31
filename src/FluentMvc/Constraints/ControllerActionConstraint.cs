namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public class ControllerActionConstraint : IConstraint
    {
        private readonly ActionDescriptor actionDescriptor;

        public ControllerActionConstraint(ActionDescriptor actionDescriptor)
        {
            this.actionDescriptor = actionDescriptor;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return (actionDescriptor == EmptyActionDescriptor.Instance || IsCorrectAction(selector.ActionDescriptor, actionDescriptor));
        }

        private bool IsCorrectAction(ActionDescriptor selectorDescriptor, ActionDescriptor descriptor)
        {
            if (selectorDescriptor == EmptyActionDescriptor.Instance)
                return true;

            var reflectedActionDescriptor = descriptor as ReflectedActionDescriptor;
            var selectorReflectedActionDescriptor = selectorDescriptor as ReflectedActionDescriptor;

            return reflectedActionDescriptor.MethodInfo.Equals(selectorReflectedActionDescriptor.MethodInfo);
        }
    }
}