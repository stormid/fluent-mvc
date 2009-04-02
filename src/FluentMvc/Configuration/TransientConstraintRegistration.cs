namespace FluentMvc.Configuration
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class TransientConstraintRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        public Type Type { get; private set; }
        public IConstraint Constraint { get; set; }

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

        public TransientConstraintRegistration(Type type, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            Type = type;
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public TransientConstraintRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            Constraint = constraint;
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public void CreateConstaintInstance(IFluentMvcObjectFactory factory)
        {
            if (Constraint == null)
                Constraint = factory.CreateConstraint(Type);
        }

        public ActionFilterRegistryItem CreateRegistryItem(Type filterType)
        {
            return new ActionFilterRegistryItem(filterType, Constraint, ActionDescriptor, ControllerDescriptor);
        }
    }
}