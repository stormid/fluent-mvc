namespace FluentMvc.Spec.Unit.ActionInvoker
{
    using System.Web.Mvc;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Utils;

    [TestFixture]
    public class when_an_action_is_invoked_and_the_action_return_value_is_not_null_or_an_action_result : SpecificationBase
    {
        private IFluentMvcResolver fluentMvcFactory;
        private FluentMvcActionInvoker ActionInvocker;
        private ControllerContext ControllerContext;

        public override void Given()
        {
            fluentMvcFactory = CreateStub<IFluentMvcResolver>();
            ControllerContext = new ControllerContextBuilder().WithController(new TestController()).Build();

            ActionInvocker = FluentMvcActionInvoker.Create(fluentMvcFactory) as FluentMvcActionInvoker;
        }

        public override void Because()
        {
            ActionInvocker.InvokeAction(ControllerContext, ReflectionHelper.GetMethod<TestController>(x => x.ReturnPost()).Name);
        }

        [Test]
        public void should_assign_return_value_to_controller_viewdata()
        {
            ControllerContext.Controller.ViewData.Model.ShouldBe(typeof(Post));
        }

        [Test]
        public void Should_Delegate_To_The_ActionResultResolver()
        {
            fluentMvcFactory.AssertWasCalled(x => x.CreateActionResult(Arg<ActionResultSelector>.Is.Anything));
        }
    }

    [TestFixture]
    public class when_an_action_is_invoked_and_the_action_return_value_is_null : SpecificationBase
    {
        private IFluentMvcResolver fluentMvcFactory;
        private FluentMvcActionInvoker ActionInvocker;
        private ControllerContext ControllerContext;

        public override void Given()
        {
            fluentMvcFactory = CreateStub<IFluentMvcResolver>();
            ControllerContext = new ControllerContextBuilder().WithController(new TestController()).Build();

            ActionInvocker = FluentMvcActionInvoker.Create(fluentMvcFactory) as FluentMvcActionInvoker;
        }

        public override void Because()
        {
            ActionInvocker.InvokeAction(ControllerContext, ReflectionHelper.GetMethod<TestController>(x => x.ReturnNull()).Name);
        }

        [Test]
        public void Should_Delegate_To_The_ActionResultResolver()
        {
            fluentMvcFactory.AssertWasCalled(x => x.CreateActionResult(Arg<ActionResultSelector>.Is.Anything));
        }
    }

    [TestFixture]
    public class when_an_action_is_invoked_and_the_action_return_value_an_action_result : SpecificationBase
    {
        private IFluentMvcResolver fluentMvcFactory;
        private FluentMvcActionInvoker ActionInvocker;
        private ControllerContext ControllerContext;

        public override void Given()
        {
            fluentMvcFactory = CreateStub<IFluentMvcResolver>();
            ControllerContext = new ControllerContextBuilder().WithController(new TestController()).Build();

            ActionInvocker = FluentMvcActionInvoker.Create(fluentMvcFactory) as FluentMvcActionInvoker;
        }

        public override void Because()
        {
            ActionInvocker.InvokeAction(ControllerContext, ReflectionHelper.GetMethod<TestController>(x => x.ReturnViewResult()).Name);
        }

        [Test]
        public void should_assign_return_value_to_controller_viewdata()
        {
            ControllerContext.Controller.ViewData.Model.ShouldBeNull();
        }

        [Test]
        public void Should_Delegate_To_The_ActionResultResolver()
        {
            fluentMvcFactory.AssertWasCalled(x => x.CreateActionResult(Arg<ActionResultSelector>.Is.Anything));
        }
    }
}