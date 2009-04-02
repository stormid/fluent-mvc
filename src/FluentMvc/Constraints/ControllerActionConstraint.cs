namespace FluentMvc.Constraints
{
    using System.Web.Mvc;

    public class ControllerActionConstraint : IConstraint
    {
        private readonly IConstraint innerConstraint;
        private readonly ActionDescriptor actionDescriptor;

        public ControllerActionConstraint(IConstraint innerConstraint, ActionDescriptor actionDescriptor)
        {
            this.innerConstraint = innerConstraint;
            this.actionDescriptor = actionDescriptor;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            if (actionDescriptor == EmptyActionDescriptor.Instance || IsCorrectAction(selector.ActionDescriptor, actionDescriptor))
            {
                return innerConstraint.IsSatisfiedBy(selector);
            }

            return false;
        }

        private bool IsCorrectAction(ActionDescriptor selectorDescriptor, ActionDescriptor descriptor)
        {
            var reflectedActionDescriptor = descriptor as ReflectedActionDescriptor;
            var selectorReflectedActionDescriptor = selectorDescriptor as ReflectedActionDescriptor;

            return reflectedActionDescriptor.MethodInfo.Equals(selectorReflectedActionDescriptor.MethodInfo);
        }
    }
}