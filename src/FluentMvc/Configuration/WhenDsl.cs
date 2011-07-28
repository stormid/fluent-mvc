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
        public FilterScope Scope { get; set; }
        private readonly TransientRegistration guardContraintRegistration;
        private readonly TDsl innerDsl;

        public override TransientRegistration[] ConstraintRegistrations
        {
            get { return innerDsl.ConstraintRegistrations.Concat(new[] { guardContraintRegistration }).ToArray(); }
        }

        internal WhenDsl(TDsl innerDsl, Type guardConstraintType, FilterScope scope)
        {
            Scope = scope;
            this.innerDsl = innerDsl;
            guardContraintRegistration = innerDsl.CreateTypeRegistration(guardConstraintType, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, scope);
            //guardContraintRegistration = CreateTypeRegistration(guardConstraintType, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance);
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

        public override ConstraintDsl<WhenDsl<TDsl>> ExceptFor<TController>()
        {
            innerDsl.ExceptFor<TController>();
            return this;
        }

        public override ConstraintDsl<WhenDsl<TDsl>> ExceptFor<TController>(Expression<Func<TController, object>> func)
        {
            // record action/controller details here
            innerDsl.ExceptFor(func);
            return this;
        }

        public override IEnumerable<TransientRegistration> GetConstraintRegistrations(IFluentMvcObjectFactory objectFactory)
        {
            IConstraint guardConstraint = CreateGuardConstraint(objectFactory);

            if (!innerDsl.ConstraintRegistrations.Any())
            {
                // HACK: This is here to support no following constraint being defined
                yield return CreateInstanceRegistration(guardConstraint, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, FilterScope.Global);
            } 

            foreach (var registration in innerDsl.ConstraintRegistrations)
            {
                registration.Prepare(objectFactory);
                yield return CreateInstanceRegistration(new AndConstraint(guardConstraint, registration.Constraint), registration.ActionDescriptor, registration.ControllerDescriptor, registration.Scope);
            }
        }

        private IConstraint CreateGuardConstraint(IFluentMvcObjectFactory objectFactory)
        {
            guardContraintRegistration.Prepare(objectFactory);
            return guardContraintRegistration.Constraint;
        }
    }
}
