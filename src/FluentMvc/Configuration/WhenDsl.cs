namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using Registrations;

    public class WhenDsl<TDsl> : ConstraintDsl<WhenDsl<TDsl>> where TDsl : ConstraintDsl<TDsl>
    {
        private readonly AbstractTransientConstraintRegistration guardContraintRegistration;
        private readonly TDsl innerDsl;

        public override AbstractTransientConstraintRegistration[] ConstraintRegistrations
        {
            get { return innerDsl.ConstraintRegistrations.Concat(new[] { guardContraintRegistration }).ToArray(); }
        }

        internal WhenDsl(TDsl innerDsl, Type guardConstraintType)
        {
            guardContraintRegistration = CreateTypeRegistration(guardConstraintType);
            this.innerDsl = innerDsl;
        }

        public override ConstraintDsl<WhenDsl<TDsl>> AndFor<TController>()
        {
            innerDsl.AndFor<TController>();
            return this;
        }

        public override ConstraintDsl<WhenDsl<TDsl>> AndFor<TController>(Expression<Func<TController, object>> func)
        {
            innerDsl.AndFor(func);
            return this;
        }

        public ConstraintDsl<WhenDsl<TDsl>> For<TController>() where TController : Controller
        {
            innerDsl.AndFor<TController>();
            return this;
        }

        public ConstraintDsl<WhenDsl<TDsl>> For<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            innerDsl.AndFor(func);
            return this;
        }

        public override IEnumerable<AbstractTransientConstraintRegistration> GetConstraintRegistrations(IFluentMvcObjectFactory objectFactory)
        {
            IConstraint guardConstraint = CreateGuardConstraint(objectFactory);

            if (!innerDsl.ConstraintRegistrations.Any())
            {
                // HACK: This is here to support no following constraint being defined
                yield return CreateInstanceRegistration(guardConstraint, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance);
            } 

            foreach (var registration in innerDsl.ConstraintRegistrations)
            {
                registration.Prepare(objectFactory);
                yield return CreateInstanceRegistration(new WhenConstraint(guardConstraint, registration.Constraint), registration.ActionDescriptor, registration.ControllerDescriptor);
            }
        }

        private IConstraint CreateGuardConstraint(IFluentMvcObjectFactory objectFactory)
        {
            guardContraintRegistration.Prepare(objectFactory);
            return guardContraintRegistration.Constraint;
        }
    }
}
