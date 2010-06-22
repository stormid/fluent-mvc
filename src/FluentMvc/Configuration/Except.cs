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
            dsl.AddRegistration(dsl.CreateInstanceRegistration(CreateActionConstraint<TController>(actionDescriptor), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance));

            return dsl;
        }

        public static ConstraintDsl<Except> ForArea<TArea>() where TArea : AreaRegistration, new()
        {
            var dsl = new Except();
            dsl.AddRegistration(dsl.CreateInstanceRegistration(new AreaConstraint(new TArea()), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance));

            return dsl;
        }

        private static IConstraint CreateActionConstraint<TController>(ActionDescriptor actionDescriptor)
        {
            var controllerTypeConstraint = new ControllerTypeConstraint<TController>();
            var actionConstraint = new ControllerActionConstraint(actionDescriptor);
            var contContraint = new AndConstraint(controllerTypeConstraint, actionConstraint);
            return contContraint;
        }

        protected override InstanceRegistration CreateInstanceRegistration(IConstraint guardConstraint, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            return new ExceptInstanceRegistration(guardConstraint, actionDescriptor, controllerDescriptor);
        }

        public override TransientRegistration CreateTypeRegistration(Type guardConstraintType, ActionDescriptor actionDescriptor, ControllerDescriptor controllerDescriptor)
        {
            return new ExceptTransientRegistration(guardConstraintType, actionDescriptor, controllerDescriptor);
        }
    }
}