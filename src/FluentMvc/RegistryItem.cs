namespace FluentMvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;
    using Constraints;

    public class RegistryItem
    {
        private IConstraint constraint;
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        public Type Type
        {
            get { return ItemActivator.Type; }
        }

        public ActionDescriptor ActionDescriptor
        {
            get { return actionDescriptor ?? EmptyActionDescriptor.Instance; }
            protected set { actionDescriptor = value; }
        }

        public ControllerDescriptor ControllerDescriptor
        {
            get { return controllerDescriptor ?? EmptyControllerDescriptor.Instance; }
            protected set { controllerDescriptor = value; }
        }

        public IConstraint Constraint
        {
            get { return constraint ?? PredefinedConstraint.True; }
            protected set { constraint = value; }
        }

        protected ItemActivator ItemActivator { get; set; }

        protected RegistryItem(ItemActivator itemActivator, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(itemActivator, actionDescriptor, controllerDescriptor)
        {
            Constraint = constraint;
            
        }

        protected RegistryItem(ItemActivator itemActivator, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            ItemActivator = itemActivator;
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public bool Satisfies<T>(T selector) where T : RegistrySelector
        {
            return SatisfiesCore(selector);
        }

        protected virtual bool SatisfiesCore<T>(T selector) where T : RegistrySelector
        {
            return Constraint.IsSatisfiedBy(selector);
        }

        public bool AppliesToController<T>(T selector) where T : RegistrySelector
        {
            if (ControllerDescriptor == EmptyControllerDescriptor.Instance)
                return true;

            // HACK: Fugly and makes kittens cry, sort it!
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

        public TItem Create<TItem>(IFluentMvcObjectFactory fluentMvcObjectFactory)
        {
            return ItemActivator.Activate<TItem>(fluentMvcObjectFactory);
        }
    }
}