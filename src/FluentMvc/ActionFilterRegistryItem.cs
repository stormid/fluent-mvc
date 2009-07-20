namespace FluentMvc
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class ActionFilterRegistryItem : RegistryItem
    {
        public ActionFilterRegistryItem(Type actionFilterType, IConstraint constraints, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(new TypeBasedItemActivator(actionFilterType), constraints, actionDescriptor, controllerDescriptor)
        {
        }

        public ActionFilterRegistryItem(ItemActivator itemActivator, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(itemActivator, constraint, actionDescriptor, controllerDescriptor)
        {
        }
    }
}