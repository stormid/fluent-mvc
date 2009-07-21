namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class InstanceBasedTransientConstraintRegistration : TransientConstraintRegistration
    {
        public InstanceBasedTransientConstraintRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(constraint.GetType(),actionDescriptor, controllerDescriptor)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException("constraint", "Constraint instance can not be null");
            }

            Constraint = constraint;
            ConstraintType = Constraint.GetType();
        }

        public override void Prepare(IFluentMvcObjectFactory factory)
        {
            if (Constraint == null)
                base.Prepare(factory);
        }
    }
}