using FluentMvc.Utils;

namespace FluentMvc
{
    using System;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Constraints;

    public class ActionResultRegistryItem
        : RegistryItem<ActionResultSelector>
    {
        public ActionResultRegistryItem(Type actionFilterType, IConstraint constraints, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(new TypeItemActivator(actionFilterType), constraints, actionDescriptor, controllerDescriptor)
        {
        }

        public ActionResultRegistryItem(IActionResultFactory factory) : base(new InstanceItemActivator(factory), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance)
        {
            Constraint = new ListConstraint(factory.Constraints);
        }

        //public override bool AppliesToController(ActionResultSelector selector)
        //{
        //    var descriptor = selector.ControllerDescriptor;        //    var isCorrectType = descriptor.ControllerType.CanBeCastTo(ControllerDescriptor.ControllerType);        //    var isCorrentControllerName = descriptor.ControllerName.StartsWith(ControllerDescriptor.ControllerName, StringComparison.CurrentCultureIgnoreCase);
        //    return isCorrectType && isCorrentControllerName;
        //}
    }
}