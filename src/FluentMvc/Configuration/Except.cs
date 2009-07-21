namespace FluentMvc.Configuration
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using Registrations;
    using Utils;

    public class Except : ConstraintDsl<Except>
    {
        public static WhenDsl<Except> When<T>()
            where T : IConstraint
        {
            return When<T>(new Except());
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
            dsl.AddRegistration(dsl.CreateInstanceRegistration(CreateConstraint<TController>(actionDescriptor), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance));

            return dsl;
        }

        private static InverseConstraint CreateConstraint<TController>(ActionDescriptor actionDescriptor)
        {
            var controllerTypeConstraint = new ControllerTypeConstraint<TController>();
            var actionConstraint = new ControllerActionConstraint(controllerTypeConstraint, actionDescriptor);
            return new InverseConstraint(actionConstraint);
        }

        public override ConstraintDsl<Except> AndFor<TController>()
        {
            ActionDescriptor actionDescriptor = EmptyActionDescriptor.Instance;
            this.AddRegistration(new InstanceBasedTransientConstraintRegistration(CreateConstraint<TController>(actionDescriptor), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance));
            return this;
        }

        public override ConstraintDsl<Except> AndFor<TController>(Expression<Func<TController, object>> func)
        {
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            this.AddRegistration(new InstanceBasedTransientConstraintRegistration(CreateConstraint<TController>(actionDescriptor), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance));
            return this;
        }
    }
}