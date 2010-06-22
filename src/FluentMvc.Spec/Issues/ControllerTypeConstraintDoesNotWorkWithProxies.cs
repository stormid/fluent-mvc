using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Castle.DynamicProxy;
using FluentMvc.Constraints;
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

            var actionDescriptor = new ReflectedActionDescriptor(interfaceProxyWithTarget.GetType().GetMethod("ReturnNull"), "ReturnNull",new ReflectedControllerDescriptor(interfaceProxyWithTarget.GetType()));

            controllerTypeConstraint
                .IsSatisfiedBy(new ActionFilterSelector(null, actionDescriptor, actionDescriptor.ControllerDescriptor))
                .ShouldBeTrue();
        }
    }

    public class InheritedTestController : TestController
    {
    }
}