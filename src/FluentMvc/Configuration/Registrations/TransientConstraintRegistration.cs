namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class TransientConstraintRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        public TransientConstraintRegistration(Type constraintType, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            ConstraintType = constraintType;
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