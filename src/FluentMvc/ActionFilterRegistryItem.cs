namespace FluentMvc
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class ActionFilterRegistryItem : RegistryItem
    {
        public ActionFilterRegistryItem(Type actionFilterType, IConstraint constraints, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(actionFilterType, constraints, actionDescriptor, controllerDescriptor)
        {
        }
    }
}