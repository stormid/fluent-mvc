namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class TransientRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        protected TransientRegistration(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public TransientRegistration(Type type, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(actionDescriptor, controllerDescriptor)
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

        public virtual void Prepare(IFluentMvcObjectFactory factory)
        {
            Constraint = factory.CreateConstraint(ConstraintType);
        }

        public virtual ActionFilterRegistryItem CreateRegistryItem(Type filterType)
        {
            return new ActionFilterRegistryItem(new TypeItemActivator(filterType), Constraint, ActionDescriptor, ControllerDescriptor);
        }
    }
}