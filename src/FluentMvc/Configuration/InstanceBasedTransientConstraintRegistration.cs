namespace FluentMvc.Configuration
{
    using System.Web.Mvc;
    using Constraints;

    public class InstanceBasedTransientConstraintRegistration : AbstractTransientConstraintRegistration
    {
        public InstanceBasedTransientConstraintRegistration(IConstraint constraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
            : base(actionDescriptor, controllerDescriptor)
        {
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