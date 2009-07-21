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
            var registration = new InstanceBasedTransientConstraintRegistration(new ControllerTypeConstraint<TController>(), EmptyActionDescriptor.Instance, new ReflectedControllerDescriptor(typeof(TController)));
            return CreateDsl(registration);
        }

        public static ConstraintDsl<Apply> For<TController>(Expression<Func<TController, object>> func) where TController : Controller
        {
            ActionDescriptor actionDescriptor = func.CreateActionDescriptor();
            return CreateDsl(new InstanceBasedTransientConstraintRegistration(new ControllerTypeConstraint<TController>(), actionDescriptor, actionDescriptor.ControllerDescriptor));
        }

        private static Apply CreateDsl(TransientConstraintRegistration constraintRegistration)
        {
            var dsl = new Apply();
            dsl.AddRegistration(constraintRegistration);

            return dsl;
        }
    }



    public class Is
    {
        private static bool isDefault = true;

        public static bool Default
        {
            get { return isDefault; }
        }
    }
}