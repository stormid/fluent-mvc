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
            return new WhenDsl<TDsl>(innerDsl, typeof(T), FilterScope.Global);
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
            AddRegistration(CreateTypeRegistration(typeof(TConstraint), actionDescriptor, controllerDescriptor, FilterScope.Global));
            return this;
        }

        public virtual ConstraintDsl<TDsl> ExceptFor<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {   
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            var controllerTypeConstraint = new ControllerTypeConstraint<TController>();
            var actionConstraint = new ControllerActionConstraint(actionDescriptor);
            var contContraint = new AndConstraint(controllerTypeConstraint, actionConstraint);
            AddRegistration(CreateInstanceRegistration(new NotConstraint(contContraint), actionDescriptor, actionDescriptor.ControllerDescriptor, FilterScope.Action));
            return this;
        }
    }

    public class ConstraintDsl
    {
        protected readonly HashSet<TransientRegistration> constaintRegistrations = new HashSet<TransientRegistration>();

        public virtual TransientRegistration[] ConstraintRegistrations
        {
            get { return constaintRegistrations.ToArray(); }
        }

        protected void AddRegistration(TransientRegistration constraintRegistration)
        {
            constaintRegistrations.Add(constraintRegistration);
        }

        public virtual IEnumerable<TransientRegistration> GetConstraintRegistrations(IFluentMvcObjectFactory objectFactory)
        {
            foreach (var registration in constaintRegistrations)
            {
                registration.Prepare(objectFactory);
                yield return registration;
            }
        }

        protected virtual InstanceRegistration CreateInstanceRegistration(IConstraint guardConstraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope filterScope)
        {
            return new InstanceRegistration(guardConstraint, actionDescriptor, controllerDescriptor, filterScope);
        }

        public virtual TransientRegistration CreateTypeRegistration(Type guardConstraintType, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor, FilterScope filterScope)
        {
            return new TransientRegistration(guardConstraintType, actionDescriptor, controllerDescriptor, filterScope);
        }
    }
}