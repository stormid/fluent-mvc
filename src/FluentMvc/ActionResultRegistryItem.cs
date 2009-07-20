namespace FluentMvc
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class ActionResultRegistryItem
        : RegistryItem
    {
        public ActionResultRegistryItem(Type actionFilterType, IConstraint constraints, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(new TypeBasedItemActivator(actionFilterType), constraints, actionDescriptor, controllerDescriptor)
        {
        }
    }
}