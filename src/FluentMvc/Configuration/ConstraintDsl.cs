namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using Utils;

    public class ConstraintDsl<TDsl> : ConstraintDsl
        where TDsl : ConstraintDsl<TDsl>, new()
    {
        public ConstraintDsl<TDsl> Add<T>()
            where T : IConstraint
        {
            ActionDescriptor actionDescriptor = EmptyActionDescriptor.Instance;
            return Add<T>(actionDescriptor, actionDescriptor.ControllerDescriptor);
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
            AddConstraint(new TypeBasedTransientConstraintRegistration(typeof(TConstraint), actionDescriptor, controllerDescriptor));
            return this;
        }
    }

    public class ConstraintDsl
    {
        protected readonly HashSet<AbstractTransientConstraintRegistration> constaintRegistrations = new HashSet<AbstractTransientConstraintRegistration>();

        public AbstractTransientConstraintRegistration[] ConstraintRegistrations
        {
            get { return constaintRegistrations.ToArray(); }
        }

        protected void AddConstraint(AbstractTransientConstraintRegistration constraintRegistration)
        {
            constaintRegistrations.Add(constraintRegistration);
        }

        public virtual IEnumerable<AbstractTransientConstraintRegistration> CreateConstraintsRegistrations(IFluentMvcObjectFactory objectFactory)
        {
            foreach (var registration in constaintRegistrations)
            {
                registration.Prepare(objectFactory);
                yield return registration;
            }
        }
    }
}