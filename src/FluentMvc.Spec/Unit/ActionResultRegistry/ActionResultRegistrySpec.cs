namespace FluentMvc.Spec.Unit.ActionResultRegistry
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using FluentMvc;
    using NUnit.Framework;
    using Utils;

    [TestFixture]
    public class When_registering_an_item : ActionResultRegistrySpecBase
    {
        private ActionResultRegistryItem registryItem;

        public override void Given()
        {
            registryTester = new ActionResultRegistryTester();
            registry = new ActionResultRegistry();
            registryItem = new ActionResultRegistryItem(typeof(TestActionResultFactory), null, EmptyActionDescriptor.Instance, EmptyControllerDescriptor.Instance);
        }

        public override void Because()
        {
            registry.Add(registryItem);
            registryTester.Init(registry);
        }

        [Test]
        public void Should_add_item_to_registry()
        {
            registryTester.ShouldContain(registryItem);
        }
    }

    [TestFixture]
    public class when_adding_one_action_result_factory_targeting_a_certain_action : ActionResultRegistrySpecBase
    {
        private ActionDescriptor actionDescriptor;
        private Expression<Func<TestController, object>> controllerAction;

        public override void Given()
        {
            registry = new ActionResultRegistry();
            registryTester = new ActionResultRegistryTester();
            registryTester.Init(registry);
            controllerAction = controller => controller.ReturnPost();
        }

        public override void Because()
        {
            actionDescriptor = registryTester.AddItemWithActionDescriptor(controllerAction);
        }

        [Test]
        public void should_be_able_to_satisy_the_request()
        {
            registry.CanSatisfy(new ActionResultSelector(new object(), new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor))
                .ShouldBeTrue();
        }

        [Test]
        public void should_not_satisfy_a_different_controller()
        {
            Expression<Func<SecondTestController, object>> func = controller => controller.DoSomething();
            var nonMatchingDescriptor = func.CreateActionDescriptor();
            registry.CanSatisfy(new ActionResultSelector(new object(), new ControllerContext(), nonMatchingDescriptor, nonMatchingDescriptor.ControllerDescriptor))
                .ShouldBeFalse();
        }

        [Test]
        public void should_not_satisfy_same_controller_with_different_action()
        {
            Expression<Func<TestController, object>> func = controller => controller.ReturnViewResult();
            var nonMatchingDescriptor = func.CreateActionDescriptor();
            registry.CanSatisfy(new ActionResultSelector(new object(), new ControllerContext(), nonMatchingDescriptor, nonMatchingDescriptor.ControllerDescriptor))
                .ShouldBeFalse();
        }

    }

}