namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;

    public class TypeBasedTransientConstraintRegistration : AbstractTransientConstraintRegistration
    {
        public TypeBasedTransientConstraintRegistration(Type constraintType, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(actionDescriptor, controllerDescriptor)
        {
            ConstraintType = constraintType;
        }
    }
}