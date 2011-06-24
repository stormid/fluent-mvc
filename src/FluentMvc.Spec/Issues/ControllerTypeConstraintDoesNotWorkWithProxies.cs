using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Castle.DynamicProxy;
using FluentMvc.Configuration;
using FluentMvc.Constraints;
using FluentMvc.Conventions;
using FluentMvc.Spec.Unit.ConfigurationDsl;
using NUnit.Framework;
using FluentMvc.Utils;

namespace FluentMvc.Spec.Issues
{
    [TestFixture]
    public class ControllerTypeConstraintDoesNotWorkWithProxiesOrInhertiedControllers
    {
        [Test]
        public void can_match_inhertied_controller()
        {
            Expression<Func<InheritedTestController, object>> func = c => c.ReturnNull();

            var controllerTypeConstraint = new ControllerTypeConstraint<TestController>();
            var actionDescriptor = func.CreateActionDescriptor();

            controllerTypeConstraint
                .IsSatisfiedBy(new ActionFilterSelector(null, actionDescriptor, actionDescriptor.ControllerDescriptor))
                .ShouldBeTrue();
        }

        [Test]
        public void can_match_castle_proxied_controller()
        {
            var controllerTypeConstraint = new ControllerTypeConstraint<TestController>();

            var proxyGenerator = new ProxyGenerator();
            var interfaceProxyWithTarget = proxyGenerator.CreateClassProxy<TestController>();

            var controllerType = interfaceProxyWithTarget.GetType();
            var actionDescriptor = new ReflectedActionDescriptor(controllerType.GetMethod("ReturnViewResult"), "ReturnViewResult", new ReflectedControllerDescriptor(controllerType));

            controllerTypeConstraint
                .IsSatisfiedBy(new ActionFilterSelector(null, actionDescriptor, actionDescriptor.ControllerDescriptor))
                .ShouldBeTrue();
        }

        [Test]
        public void can_resolve_through_registry()
        {
            var controllerTypeConstraint = new ControllerTypeConstraint<TestController>();

            var method = typeof(TestController).GetMethods().First(x => x.Name.Equals("ReturnNull") && x.GetParameters().Count() == 0);
            var actionDescriptor = new ReflectedActionDescriptor(method, "ReturnNull", new ReflectedControllerDescriptor(typeof(TestController)));

            var registry = new ActionFilterRegistry(new FluentMvcObjectFactory());
            registry.Add(new ActionFilterRegistryItem(typeof(TestFilter), controllerTypeConstraint, actionDescriptor, actionDescriptor.ControllerDescriptor));

            var proxyGenerator = new ProxyGenerator();
            var interfaceProxyWithTarget = proxyGenerator.CreateClassProxy<TestController>();

            var controllerType = interfaceProxyWithTarget.GetType();

            var methodInfos = controllerType.GetMethods();
            var proxyActionDescriptor = new ReflectedActionDescriptor(methodInfos.First(x => x.Name.Equals("ReturnNull") && x.GetParameters().Count() == 0), "ReturnNull", new ReflectedControllerDescriptor(controllerType));

            registry
                .CanSatisfy(new ActionFilterSelector(null, proxyActionDescriptor, proxyActionDescriptor.ControllerDescriptor))
                .ShouldBeTrue();
        }

        [TestFixture]
        public class when_add_a_filter_to_two_overloads_of_an_action_and_request_for_a_proxied_version_of_the_controller : DslSpecBase
        {
            private ActionDescriptor actionDescriptor;
            private IActionFilterRegistry actionFilterRegistry;

            public override void Given()
            {
                actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
                Expression<Func<TestController, object>> func = controller => controller.ReturnNull();
                Expression<Func<TestController, object>> otherFunc = controller => controller.ReturnNull(null);

                var proxyGenerator = new ProxyGenerator();
                var interfaceProxyWithTarget = proxyGenerator.CreateClassProxy<TestController>();

                var controllerType = interfaceProxyWithTarget.GetType();

                var methodInfos = controllerType.GetMethods();
                actionDescriptor = new ReflectedActionDescriptor(methodInfos.First(x => x.Name.Equals("ReturnNull") && x.GetParameters().Count() == 0), "ReturnNull", new ReflectedControllerDescriptor(controllerType));

                Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                    .WithFilter<TestActionFilter>(Apply.For(func).AndFor(otherFunc));
            }

            public override void Because()
            {
                Configuration.BuildFilterProvider();
            }

            [Test]
            public void should_return_the_attribute_for_any_none_ignored_action()
            {
                actionFilterRegistry
                    .FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor))
                    .Length.ShouldEqual(1);
            }
        }
    }

    public class TestFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }

    public class InheritedTestController : TestController
    {
    }
}