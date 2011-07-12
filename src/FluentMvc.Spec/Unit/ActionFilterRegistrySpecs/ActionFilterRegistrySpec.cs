using System.Linq.Expressions;
using FluentMvc.Utils;

namespace FluentMvc.Spec.Unit.ActionFilterRegistrySpecs
{
    using System;
    using System.Web.Mvc;
    using Configuration;
    using Constraints;
    using NUnit.Framework;
    using Rhino.Mocks;
    using ActionFilterRegistry=FluentMvc.ActionFilterRegistry;

    [TestFixture, Ignore("Covered due to global filters")]
    public class When_registering_an_action_filter_not_associated_with_a_controller_with_no_constraints : SpecificationBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private IActionFilter actionFilter;
        private ActionFilterRegistryTester actionFilterRegistryTester;
        private IFluentMvcObjectFactory objectFactory;

        public override void Given()
        {
            actionFilter = new TestActionFilter();
            objectFactory = CreateStub<IFluentMvcObjectFactory>();
            objectFactory.Stub(of => of.Resolve<IActionFilter>(Arg<Type>.Is.Anything))
                .Return(actionFilter);

            actionFilterRegistry = new ActionFilterRegistry(objectFactory);
            actionFilterRegistryTester = new ActionFilterRegistryTester(actionFilterRegistry);
        }

        public override void Because()
        {
            actionFilterRegistryTester.RegisterFilter(new GlobalActionFilterRegistryItem(typeof(TestActionFilter), PredefinedConstraint.True));
        }

        [Test]
        public void should_be_returned_for_any_controller()
        {
            actionFilterRegistryTester.CountReturnedForControllerAndAction<TestController>(controller => controller.ReturnPost())
                .ShouldEqual(1);
        }
    }

    [TestFixture]
    public class When_registering_an_action_filter_not_associated_with_a_controller_with_a_constraint_that_is_not_statisfied : SpecificationBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private IActionFilter actionFilter;
        private ActionFilterRegistryTester actionFilterRegistryTester;
        private IFluentMvcObjectFactory objectFactory;

        public override void Given()
        {
            actionFilter = new TestActionFilter();
            objectFactory = CreateStub<IFluentMvcObjectFactory>();
            objectFactory.Stub(of => of.Resolve<IActionFilter>(Arg<Type>.Is.Anything))
                .Return(actionFilter);

            actionFilterRegistry = new ActionFilterRegistry(objectFactory);
            actionFilterRegistryTester = new ActionFilterRegistryTester(actionFilterRegistry);
        }

        public override void Because()
        {
            actionFilterRegistryTester.RegisterFilter(new GlobalActionFilterRegistryItem(typeof(TestActionFilter), PredefinedConstraint.False));
        }

        [Test]
        public void should_not_be_returned()
        {
            actionFilterRegistryTester.AssertFilterIsNotReturned(actionFilter);
        }
    }

    [TestFixture]
    public class When_registering_an_action_filter_associated_with_a_controller_with_a_constraint_that_is_not_statisfied : SpecificationBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private IActionFilter actionFilter;
        private ActionFilterRegistryTester actionFilterRegistryTester;
        private IFluentMvcObjectFactory objectFactory;

        public override void Given()
        {
            actionFilter = new TestActionFilter();
            objectFactory = CreateStub<IFluentMvcObjectFactory>();
            objectFactory.Stub(of => of.Resolve<IActionFilter>(Arg<Type>.Is.Anything))
                .Return(actionFilter);

            actionFilterRegistry = new ActionFilterRegistry(objectFactory);
            actionFilterRegistryTester = new ActionFilterRegistryTester(actionFilterRegistry);
        }

        public override void Because()
        {
            Expression<Func<SecondTestController, object>> func = c => c.ReturnPost();
            var actionDescriptor = func.CreateActionDescriptor();
            actionFilterRegistryTester.RegisterFilter(new ControllerRegistryItem(typeof(TestActionFilter), PredefinedConstraint.True, actionDescriptor.ControllerDescriptor));
        }

        [Test]
        public void should_not_be_returned_for_incorrect_controller()
        {
            actionFilterRegistryTester.CountReturnedForControllerAndAction<TestController>(controller => controller.ReturnPost()).ShouldEqual(0);
        }

        [Test]
        public void should_be_returned_for_correct_controller()
        {
            actionFilterRegistryTester.CountReturnedForControllerAndAction<SecondTestController>(controller => controller.ReturnPost()).ShouldEqual(1);
        }
    }
}