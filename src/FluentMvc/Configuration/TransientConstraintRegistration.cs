namespace FluentMvc.Configuration
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class TransientConstraintRegistration
    {
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;
        private readonly object itemInstance;

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

        private TransientConstraintRegistration(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public TransientConstraintRegistration(Type type, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(actionDescriptor, controllerDescriptor)
        {
            Type = type;
        }

        public TransientConstraintRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(actionDescriptor, controllerDescriptor)
        {
            Constraint = constraint;
        }

        public TransientConstraintRegistration(object itemInstance, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(constraint, actionDescriptor, controllerDescriptor)
        {
            this.itemInstance = itemInstance;
        }

        public void CreateConstaintInstance(IFluentMvcObjectFactory factory)
        {
            if (Constraint == null)
                Constraint = factory.CreateConstraint(Type);
        }

        public ActionFilterRegistryItem CreateRegistryItem(Type filterType)
        {
            if ( itemInstance == null)
                return new ActionFilterRegistryItem(new TypeBasedItemActivator(filterType), Constraint, ActionDescriptor, ControllerDescriptor);

            return new ActionFilterRegistryItem(new InstanceItemActivator(itemInstance), Constraint, ActionDescriptor, ControllerDescriptor);
        }
    }
}