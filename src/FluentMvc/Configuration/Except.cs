namespace FluentMvc.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using Utils;

    public class Except : ConstraintDsl<Except>
    {
        public static ConstraintDsl<Except> When<T>()
            where T : IConstraint
        {
            return new Except().Add<T>();
        }

        public static ConstraintDsl<Except> For<TController>() where TController : Controller
        {
            return For<TController>(EmptyActionDescriptor.Instance);
        }

        public static ConstraintDsl<Except> For<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            return For<TController>(actionDescriptor);
        }

        public static ConstraintDsl<Except> For<TController>(ActionDescriptor actionDescriptor) where TController : Controller
        {
            var dsl = new Except();
            CreateAndAddControllerTypeConstraint<TController>(dsl, actionDescriptor, new ReflectedControllerDescriptor(typeof(TController)));

            return dsl;
        }

        private static void CreateAndAddControllerTypeConstraint<TController>(Except dsl, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            var controllerTypeConstraint = new ControllerTypeConstraint<TController>();
            var actionConstraint = new ControllerActionConstraint(controllerTypeConstraint, actionDescriptor);
            var inverseConstraint = new InverseConstraint(actionConstraint);

            dsl.AddConstraint(new InstanceBasedTransientConstraintRegistration(inverseConstraint, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance));
        }

        public override ConstraintDsl<Except> AndFor<TController>()
        {
            ActionDescriptor actionDescriptor = EmptyActionDescriptor.Instance;
            CreateAndAddControllerTypeConstraint<TController>(this, actionDescriptor, new ReflectedControllerDescriptor(typeof(TController)));
            return this;
        }

        public override ConstraintDsl<Except> AndFor<TController>(Expression<Func<TController, object>> func)
        {
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            CreateAndAddControllerTypeConstraint<TController>(this, actionDescriptor, actionDescriptor.ControllerDescriptor);
            return this;
        }
    }
}