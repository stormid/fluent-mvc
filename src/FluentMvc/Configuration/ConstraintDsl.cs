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
            return Add<T>(EmptyActionDescriptor.Instance);
        }

        public virtual ConstraintDsl<TDsl> AndFor<TController>() where TController : Controller
        {
            return Add<ControllerTypeConstraint<TController>>(EmptyActionDescriptor.Instance);
        }

        public virtual ConstraintDsl<TDsl> AndFor<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            return Add<ControllerTypeConstraint<TController>>(func.CreateActionDescriptor());
        }

        protected virtual ConstraintDsl<TDsl> Add<TConstraint>(ActionDescriptor actionDescriptor)
        {
            AddConstraint(new TransientConstraintRegistration(typeof(TConstraint), actionDescriptor, actionDescriptor.ControllerDescriptor));
            return this;
        }
    }

    public class ConstraintDsl
    {
        protected readonly HashSet<TransientConstraintRegistration> constaintRegistrations = new HashSet<TransientConstraintRegistration>();

        public TransientConstraintRegistration[] ConstraintRegistrations
        {
            get { return constaintRegistrations.ToArray(); }
        }

        protected void AddConstraint(TransientConstraintRegistration constraintRegistration)
        {
            constaintRegistrations.Add(constraintRegistration);
        }

        public virtual IEnumerable<TransientConstraintRegistration> CreateConstraintsRegistrations(IFluentMvcObjectFactory objectFactory)
        {
            foreach (var registration in constaintRegistrations)
            {
                registration.CreateConstaintInstance(objectFactory);
                yield return registration;
            }
        }
    }
}