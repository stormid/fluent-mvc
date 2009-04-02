namespace FluentMvc.Configuration
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class ConstraintRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        public Type Type { get; private set; }
        public IConstraint Constraint { get; private set; }
        public IConstraintExecutor Executor { get; private set; }

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

        public ConstraintRegistration(Type type, ActionDescriptor descriptor)
        {
            Type = type;
            ActionDescriptor = descriptor;
            ControllerDescriptor = descriptor.ControllerDescriptor;
        }

        public ConstraintRegistration(Type type, ControllerDescriptor descriptor)
        {
            Type = type;
            ControllerDescriptor = descriptor;
        }

        public ConstraintRegistration(Type type, ActionDescriptor descriptor, IConstraintExecutor constraintExecutor) : this(type, descriptor)
        {
            Executor = constraintExecutor;
        }

        public ConstraintRegistration(IConstraint constraint, ActionDescriptor descriptor, IConstraintExecutor constraintExecutor)
        {
            Constraint = constraint;
            ActionDescriptor = descriptor;
            Executor = constraintExecutor;
        }

        public ConstraintRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
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
    }
}