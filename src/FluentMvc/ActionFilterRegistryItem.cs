using FluentMvc.Utils;

namespace FluentMvc
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public abstract class ActionFilterRegistryItem : RegistryItem<ActionFilterSelector>
    {
        private readonly FilterScope scope;

        public FilterScope Scope { get { return scope; } }

        protected ActionFilterRegistryItem(Type actionFilterType, IConstraint constraints, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope scope)
            : this(new TypeItemActivator(actionFilterType), constraints, actionDescriptor, controllerDescriptor, scope)
        {
        }

        protected ActionFilterRegistryItem(ItemActivator itemActivator, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope scope)
            : base(itemActivator, constraint, actionDescriptor, controllerDescriptor)
        {
            this.scope = scope;
        }
    }

    public class ControllerActionRegistryItem : ActionFilterRegistryItem
    {
        public ControllerActionRegistryItem(Type type, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(type, constraint, actionDescriptor, controllerDescriptor, FilterScope.Action)
        {

        }

        public ControllerActionRegistryItem(ItemActivator type, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(type, constraint, actionDescriptor, controllerDescriptor, FilterScope.Action)
        {

        }

        //public override bool AppliesToController(ActionFilterSelector selector)
        //{
        //    if (selector.ControllerDescriptor == EmptyControllerDescriptor.Instance || selector.ActionDescriptor == EmptyActionDescriptor.Instance)
        //        return false;

        //    return selector.Scope == FilterScope.Action || base.AppliesToAction(selector);
        //}
    }

    public class ControllerRegistryItem : ActionFilterRegistryItem
    {
        public ControllerRegistryItem(Type type, IConstraint constraints, ControllerDescriptor controllerDescriptor)
            : this(new TypeItemActivator(type), constraints, controllerDescriptor)
        {
        }
        public ControllerRegistryItem(ItemActivator type, IConstraint constraints, ControllerDescriptor controllerDescriptor)
            : base(type, constraints, EmptyActionDescriptor.Instance, controllerDescriptor, FilterScope.Controller)
        {
        }

        //public override bool AppliesToController(ActionFilterSelector selector)
        //{
        //    if (selector.ControllerDescriptor == EmptyControllerDescriptor.Instance)
        //        return false;

        //    return selector.Scope == FilterScope.Controller || CorrectController(selector.ControllerDescriptor);
        //}

        private bool CorrectController(ControllerDescriptor selectorDescriptor)
        {
            var isCorrectType = selectorDescriptor.ControllerType.CanBeCastTo(ControllerDescriptor.ControllerType);
            var isCorrentControllerName = selectorDescriptor.ControllerName.StartsWith(ControllerDescriptor.ControllerName, StringComparison.CurrentCultureIgnoreCase);

            return isCorrectType && isCorrentControllerName;
        }
    }

    public class GlobalActionFilterRegistryItem : ActionFilterRegistryItem
    {
        public GlobalActionFilterRegistryItem(Type type, IConstraint constraints)
            : this(new TypeItemActivator(type), constraints)
        {

        }

        public GlobalActionFilterRegistryItem(ItemActivator itemActivator, IConstraint constraint)
            : base(itemActivator, constraint, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, FilterScope.Global)
        {
        }


        //public override bool AppliesToController(ActionFilterSelector selector)
        //{
        //    return selector.Scope == FilterScope.Global;
        //}
    }
}