namespace FluentMvc.Configuration
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public abstract class AbstractTransientConstraintRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        protected AbstractTransientConstraintRegistration(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
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