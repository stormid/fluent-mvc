namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    // TODO: Needs another refactoring
    public class FilterInstanceInstanceRegistration : InstanceRegistration
    {
        private readonly object itemInstance;

        public FilterInstanceInstanceRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, object itemInstance, FilterScope filterScope)
            : base(constraint, actionDescriptor, controllerDescriptor, filterScope)
        {
            this.itemInstance = itemInstance;
        }

        public FilterInstanceInstanceRegistration(IConstraint constraint, object filterInstance, FilterScope filterScope)
            : this(constraint, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, filterInstance, filterScope)
        {
        }

        //public override ActionFilterRegistryItem CreateRegistryItem(Type filterType)
        //{
        //    return new ActionFilterRegistryItem(new InstanceItemActivator(itemInstance), Constraint, ActionDescriptor, ControllerDescriptor);
        //}

        protected override ItemActivator GetTypeItemActivator(Type filterType)
        {
            return new InstanceItemActivator(itemInstance);
        }
    }
}