using System.Collections.Generic;
using FluentMvc.Utils;

namespace FluentMvc
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Configuration;
    using Constraints;

    public abstract class RegistryItem<T>  where T : RegistrySelector
    {
        private IConstraint constraint;
        private ControllerDescriptor controllerDescriptor;
        private ActionDescriptor actionDescriptor;

        public Type Type
        {
            get { return ItemActivator.Type; }
        }

        public ActionDescriptor ActionDescriptor
        {
            get { return actionDescriptor ?? EmptyActionDescriptor.Instance; }
            protected set { actionDescriptor = value; }
        }

        public ControllerDescriptor ControllerDescriptor
        {
            get { return controllerDescriptor ?? EmptyControllerDescriptor.Instance; }
            protected set { controllerDescriptor = value; }
        }

        public IConstraint Constraint
        {
            get { return constraint ?? PredefinedConstraint.True; }
            protected set { constraint = value; }
        }

        protected ItemActivator ItemActivator { get; set; }

        protected RegistryItem(ItemActivator itemActivator, IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : this(itemActivator, actionDescriptor, controllerDescriptor)
        {
            Constraint = constraint;
            
        }

        protected RegistryItem(ItemActivator itemActivator, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            ItemActivator = itemActivator;
            ActionDescriptor = actionDescriptor;
            ControllerDescriptor = controllerDescriptor;
        }

        public bool Satisfies(T selector)
        {
            return SatisfiesCore(selector);
        }

        protected virtual bool SatisfiesCore(T selector)
        {
            return Constraint.IsSatisfiedBy(selector);
        }

        public bool AppliesToController(T selector)
        {
            if (ControllerDescriptor == EmptyControllerDescriptor.Instance)                return true;            var descriptor = selector.ControllerDescriptor;            var isCorrectType = descriptor.ControllerType.CanBeCastTo(ControllerDescriptor.ControllerType);            var isCorrentControllerName = descriptor.ControllerName.StartsWith(ControllerDescriptor.ControllerName, StringComparison.CurrentCultureIgnoreCase);
            return isCorrectType && isCorrentControllerName;        }

        public virtual bool AppliesToAction(T selector)
        {
            if (ActionDescriptor == EmptyActionDescriptor.Instance)
                return true;

            return new ActionDescriptorComparer().Compare(ActionDescriptor, selector.ActionDescriptor) > 0;
        }

        public virtual TItem Create<TItem>(IFluentMvcObjectFactory fluentMvcObjectFactory)
        {
            return ItemActivator.Activate<TItem>(fluentMvcObjectFactory);
        }

        public virtual object Create(IFluentMvcObjectFactory fluentMvcObjectFactory)
        {
            return ItemActivator.Activate(fluentMvcObjectFactory);
        }
    }

    public class ActionDescriptorComparer : IComparer<ActionDescriptor>
    {
        public int Compare(ActionDescriptor x, ActionDescriptor y)
        {
            return IsCorrectAction(x, y) && ControllerHasAction(x,y)? 1 : 0;
        }

        private bool IsCorrectAction(ActionDescriptor actionDescriptor, ActionDescriptor descriptor)
        {
            var reflectedActionDescriptor = actionDescriptor as ReflectedActionDescriptor;
            var selectorReflectedActionDescriptor = descriptor as ReflectedActionDescriptor;

            var isCorrectAction = reflectedActionDescriptor.MethodInfo.Name.Equals(selectorReflectedActionDescriptor.MethodInfo.Name, StringComparison.CurrentCultureIgnoreCase);

            var hasCorrectParamters = reflectedActionDescriptor.GetParameters().Length.Equals(selectorReflectedActionDescriptor.GetParameters().Length);

            return isCorrectAction && hasCorrectParamters;
        }

        private bool ControllerHasAction(ActionDescriptor actionDescriptor, ActionDescriptor descriptor)
        {
            return actionDescriptor.ControllerDescriptor.GetCanonicalActions().Any(x => x.ActionName.Equals(descriptor.ActionName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}