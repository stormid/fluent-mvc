namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class TransientRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;
        private FilterScope scope;

        protected TransientRegistration(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope scope)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
            this.scope = scope;
        }

        public TransientRegistration(Type type, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope scope)
            : this(actionDescriptor, controllerDescriptor, scope)
        {
            ConstraintType = type;
        }

        public ActionDescriptor ActionDescriptor
        {
            get { return actionDescriptor ?? EmptyActionDescriptor.Instance; }
            private set { actionDescriptor = value; }
        }

        public ControllerDescriptor ControllerDescriptor
        {
            get { return controllerDescriptor ?? EmptyControllerDescriptor.Instance; }
            private set { controllerDescriptor = value; }
        }

        public IConstraint Constraint { get; protected set; }

        protected Type ConstraintType { get; set; }

        public FilterScope Scope { get { return scope; } }

        public virtual void Prepare(IFluentMvcObjectFactory factory)
        {
            Constraint = factory.CreateConstraint(ConstraintType);
        }

        public ActionFilterRegistryItem CreateRegistryItem(Type filterType)
        {
            if (ControllerDescriptor == EmptyControllerDescriptor.Instance && ActionDescriptor == EmptyActionDescriptor.Instance)
                return new GlobalActionFilterRegistryItem(GetTypeItemActivator(filterType), Constraint);

            if (ControllerDescriptor != EmptyControllerDescriptor.Instance && ActionDescriptor == EmptyActionDescriptor.Instance)
                return new ControllerRegistryItem(GetTypeItemActivator(filterType), Constraint, ControllerDescriptor);

            if (ControllerDescriptor != EmptyControllerDescriptor.Instance && ActionDescriptor != EmptyActionDescriptor.Instance)
                return new ControllerActionRegistryItem(GetTypeItemActivator(filterType), Constraint, ActionDescriptor, ControllerDescriptor);

            throw new InvalidOperationException();
        }

        protected virtual ItemActivator GetTypeItemActivator(Type filterType)
        {
            return new TypeItemActivator(filterType);
        }
    }

}