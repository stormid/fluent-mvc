namespace FluentMvc.Configuration
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Constraints;
    using Registrations;
    using Utils;

    public class Apply : ConstraintDsl<Apply>
    {
        public static WhenDsl<Apply> When<T>()
            where T : IConstraint
        {
            return When<T>(new Apply());
        }

        public static ConstraintDsl<Apply> For<TController>() where TController : Controller
        {
            var registration = new InstanceRegistration(new ControllerTypeConstraint<TController>(), EmptyActionDescriptor.Instance, new ReflectedControllerDescriptor(typeof(TController)), FilterScope.Controller);
            return CreateDsl(registration);
        }

        public static ConstraintDsl<Apply> For<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            return CreateDsl(new InstanceRegistration(new ControllerActionConstraint(actionDescriptor), actionDescriptor, actionDescriptor.ControllerDescriptor, FilterScope.Action));
        }

        private static Apply CreateDsl(TransientRegistration constraintRegistration)
        {
            var dsl = new Apply();
            dsl.AddRegistration(constraintRegistration);

            return dsl;
        }

        public static ConstraintDsl<Apply> ForArea<TArea>()
            where TArea : AreaRegistration, new()
        {
            var dsl = new Apply();
            dsl.AddRegistration(dsl.CreateInstanceRegistration(new AreaConstraint(new TArea()), EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance, FilterScope.Global));

            return dsl;
        }
    }
}