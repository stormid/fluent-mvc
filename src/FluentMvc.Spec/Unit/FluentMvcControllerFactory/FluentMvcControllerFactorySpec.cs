namespace FluentMvc.Spec.Unit.FluentMvcControllerFactory
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NUnit.Framework;
    using Rhino.Mocks;
    using FluentMvc;

    [TestFixture]
    public class When_creating_a_controller_and_the_controller_returns_null : FluentMvcControllerFactoryBase
    {
        private MethodThatThrows Method;
        public override void Given()
        {
            ControllerFactory = CreateStub<IControllerFactory>();
            ControllerFactory.Stub(controller => controller.CreateController(Arg<RequestContext>.Is.Anything, Arg<string>.Is.Anything))
                .Return(null);

            fluentMvcControllerFactory = new FluentMvcControllerFactory(ControllerFactory, CreateStub<IFluentMvcResolver>());
        }

        public override void Because()
        {
            Method = () => fluentMvcControllerFactory.CreateController(RequestContext, string.Empty);
        }

        [Test]
        public void Should_throw_a_HttpException()
        {
            typeof(HttpException).ShouldBeThrownBy(Method);
        }
    }

    [TestFixture]
    public class When_creating_a_controller : FluentMvcControllerFactoryBase
    {
        public override void Given()
        {
            Controller = new TestController();
            RequestContext = new ControllerContextBuilder().WithController(Controller).Build().RequestContext;

            ControllerFactory = CreateStub<IControllerFactory>();
            ControllerFactory.Stub(controllerFactory => controllerFactory.CreateController(Arg<RequestContext>.Is.Anything, Arg<string>.Is.Anything))
                .Return(Controller);

            fluentMvcControllerFactory = new FluentMvcControllerFactory(ControllerFactory, CreateStub<IFluentMvcResolver>());
        }

        public override void Because()
        {
            fluentMvcControllerFactory.CreateController(RequestContext, string.Empty);
        }

        [Test]
        public void Should_delegate_to_the_factory()
        {
            ControllerFactory.AssertWasCalled(factory => factory.CreateController(Arg<RequestContext>.Is.Anything, Arg<string>.Is.Anything));
        }

        [Test]
        public void Should_set_the_controllers_actioninvoker()
        {
            Controller.ActionInvoker.ShouldBeOfType(typeof(FluentMvcActionInvoker));
        }
    }

    [TestFixture]
    public class When_releasing_a_controller : FluentMvcControllerFactoryBase
    {
        public override void Given()
        {
            Controller = CreateStub<Controller>();
            ControllerFactory = CreateStub<IControllerFactory>();
            fluentMvcControllerFactory = new FluentMvcControllerFactory(ControllerFactory, CreateStub<IFluentMvcResolver>());
        }

        public override void Because()
        {
            ControllerFactory.ReleaseController(Controller);
        }

        [Test]
        public void Should_delegate_to_factory()
        {
            ControllerFactory.AssertWasCalled(factory => factory.ReleaseController(Arg<Controller>.Is.Anything));
        }
    }
}