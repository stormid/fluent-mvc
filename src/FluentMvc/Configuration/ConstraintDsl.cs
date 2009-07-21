namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using Registrations;
    using Utils;

    public class ConstraintDsl<TDsl> : ConstraintDsl
        where TDsl : ConstraintDsl<TDsl>
    {
        protected static WhenDsl<TDsl> When<T>(TDsl innerDsl)
            where T : IConstraint
        {
            return new WhenDsl<TDsl>(innerDsl, typeof(T));
        }

        public virtual ConstraintDsl<TDsl> AndFor<TController>() where TController : Controller
        {
            return Add<ControllerTypeConstraint<TController>>(EmptyActionDescriptor.Instance, new ReflectedControllerDescriptor(typeof(TController)));
        }

        public virtual ConstraintDsl<TDsl> AndFor<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            return Add<ControllerTypeConstraint<TController>>(actionDescriptor, actionDescriptor.ControllerDescriptor);
        }

        protected virtual ConstraintDsl<TDsl> Add<TConstraint>(ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            AddRegistration(new TypeBasedTransientConstraintRegistration(typeof(TConstraint), actionDescriptor, controllerDescriptor));
            return this;
        }
    }

    public class WhenConstraint : IConstraint
    {
        private readonly IConstraint constraint;
        private readonly IConstraint constraints;

        public WhenConstraint(IConstraint constraint, IConstraint constraints)
        {
            this.constraint = constraint;
            this.constraints = constraints;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return constraint.IsSatisfiedBy(selector) && constraints.IsSatisfiedBy(selector);
        }
    }

    public class ConstraintDsl
    {
        protected readonly HashSet<AbstractTransientConstraintRegistration> constaintRegistrations = new HashSet<AbstractTransientConstraintRegistration>();

        public virtual AbstractTransientConstraintRegistration[] ConstraintRegistrations
        {
            get { return constaintRegistrations.ToArray(); }
        }

        protected void AddRegistration(AbstractTransientConstraintRegistration constraintRegistration)
        {
            constaintRegistrations.Add(constraintRegistration);
        }

        public virtual IEnumerable<AbstractTransientConstraintRegistration> GetConstraintRegistrations(IFluentMvcObjectFactory objectFactory)
        {
            foreach (var registration in constaintRegistrations)
            {
                registration.Prepare(objectFactory);
                yield return registration;
            }
        }

        protected InstanceBasedTransientConstraintRegistration CreateInstanceRegistration(IConstraint guardConstraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            return new InstanceBasedTransientConstraintRegistration(guardConstraint, actionDescriptor, controllerDescriptor);
        }

        protected TypeBasedTransientConstraintRegistration CreateTypeRegistration(Type guardConstraintType)
        {
            return new TypeBasedTransientConstraintRegistration(guardConstraintType, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance);
        }
    }
}