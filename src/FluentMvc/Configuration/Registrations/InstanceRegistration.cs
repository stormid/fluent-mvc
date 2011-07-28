namespace FluentMvc.Configuration.Registrations
{
    using System;
    using System.Web.Mvc;
    using Constraints;

    public class InstanceRegistration : TransientRegistration
    {
        public InstanceRegistration(IConstraint constraint, FilterScope scope1)
            : this(constraint, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, scope1)
        {
        }

        public InstanceRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope scope1)
            : base(actionDescriptor, controllerDescriptor, scope1)
        {
            if (constraint == null)
            {
                throw new ArgumentNullException("constraint", "Constraint instance can not be null.");
            }

            Constraint = constraint;
            ConstraintType = Constraint.GetType();
        }

        public override void Prepare(IFluentMvcObjectFactory factory) { }
    }
}