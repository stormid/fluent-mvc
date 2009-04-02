namespace FluentMvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Constraints;

    public class RegistryItem
    {
        private IConstraint constraint;
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        public Type Type { get; protected set; }

        public ActionDescriptor ActionDescriptor
        {
            get { return actionDescriptor ?? EmptyActionDescriptor.Instance; }
            protected set { actionDescriptor = value; }
        }

        public ControllerDescriptor ControllerDescriptor
        {
            get { return controllerDescriptor ?? EmptyControllerDescriptor.Instance; }
            set { controllerDescriptor = value; }
        }

        public IConstraint Constraint
        {
            get { return constraint ?? new PredefinedConstraint(true); }
            protected set { constraint = value; }
        }

        public RegistryItem(Type actionFilterType, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            Constraint = constraint;
            Type = actionFilterType;
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public bool Satisfies<T>(T selector) where T : RegistrySelector
        {
            return Constraint.IsSatisfiedBy(selector);
        }

        public bool AppliesToController<T>(T selector) where T : RegistrySelector
        {

            if (ControllerDescriptor == EmptyControllerDescriptor.Instance)
                return true;

            return ControllerDescriptor.ControllerType.Equals(selector.ControllerDescriptor.ControllerType) && ControllerDescriptor.ControllerName.Equals(selector.ControllerDescriptor.ControllerName, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool AppliesToAction<T>(T selector)  where T : RegistrySelector
        {
            if (ActionDescriptor == EmptyActionDescriptor.Instance)
                return true;

            return IsCorrectAction(selector.ActionDescriptor) && ControllerHasAction(selector.ActionDescriptor);
        }

        private bool ControllerHasAction(ActionDescriptor descriptor)
        {
            return actionDescriptor.ControllerDescriptor.GetCanonicalActions().Any(x => x.ActionName == descriptor.ActionName);
        }

        private bool IsCorrectAction(ActionDescriptor descriptor)
        {
            var reflectedActionDescriptor = ActionDescriptor as ReflectedActionDescriptor;
            var selectorReflectedActionDescriptor = descriptor as ReflectedActionDescriptor;

            return reflectedActionDescriptor.MethodInfo.Equals(selectorReflectedActionDescriptor.MethodInfo);
        }

    }
}