using System;
using System.Web.Mvc;
using FluentMvc.Constraints;

namespace FluentMvc.Configuration.Registrations
{
    public class ExceptTransientRegistration : TransientRegistration
    {
        public ExceptTransientRegistration(Type guardConstraintType, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(guardConstraintType, actionDescriptor, controllerDescriptor)
        {
        }

        public override void Prepare(IFluentMvcObjectFactory factory)
        {
            var constraint = factory.CreateConstraint(ConstraintType);
            Constraint = new NotConstraint(constraint);
        }
    }
}