namespace FluentMvc
{
    using System;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Constraints;

    public class ActionResultRegistryItem
        : RegistryItem
    {
        public ActionResultRegistryItem(Type actionFilterType, IConstraint constraints, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(new TypeItemActivator(actionFilterType), constraints, actionDescriptor, controllerDescriptor)
        {
        }

        public ActionResultRegistryItem(IActionResultFactory factory) : base(new InstanceItemActivator(factory), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance)
        {
            Constraint = new ListConstraint(factory.Constraints);
        }
    }
}