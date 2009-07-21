namespace FluentMvc.Spec.Unit.DefaultActionResultResolver
{
    using System.Web.Mvc;
    using Configuration;
    using FluentMvc;
    using FluentMvc.ActionResultFactories;
    using NUnit.Framework;
    using Rhino.Mocks;

    [TestFixture]
    public class when_registering_action_result_pipeline : FluentMvcResolverSpecBase
    {
        private IActionResultPipeline pipeline;

        public override void Given()
        {
            pipeline = CreateStub<IActionResultPipeline>();
            actionResultResolver = new FluentMvcResolver(actionResultRegistry, actionFilterRegistry, CreateStub<IFluentMvcObjectFactory>());
        }

        public override void Because()
        {
            actionResultResolver.RegisterActionResultPipeline(pipeline);
        }

        [Test]
        public void should_set_pipeline()
        {
            actionResultResolver.ActionResultPipeLine.ShouldBeTheSameAs(pipeline);
        }
    }

    [TestFixture]
    public class When_creating_an_action_result_with_no_factories : FluentMvcResolverSpecBase
    {
        private ActionResultSelector Selector;
        private IActionResultFactory CatchAllFactory;

        public override void Given()
        {
            Selector = CreateStub<ActionResultSelector>();
            actionResultResolver = new FluentMvcResolver(actionResultRegistry, actionFilterRegistry, CreateStub<IFluentMvcObjectFactory>());
            CatchAllFactory = CreateStub<IActionResultFactory>();
            actionResultResolver.SetDefaultFactory(CatchAllFactory);
        }

        public override void Because()
        {
            actionResultResolver.CreateActionResult(Selector);
        }

        [Test]
        public void should_check_registry()
        {
            actionResultRegistry.AssertWasCalled(ar => ar.CanSatisfy(Arg<ActionResultSelector>.Is.Anything));
        }

        [Test]
        public void Should_call_catchall_factory()
        {
            CatchAllFactory.AssertWasCalled(x => x.Create(Arg<ActionResultSelector>.Is.Anything));
        }
    }

    [TestFixture]
    public class When_creating_an_action_result_with_a_factory_that_is_applicable : FluentMvcResolverSpecBase
    {
        private IActionResultPipeline pipeline;
        private ActionResultSelector FactoryOptions;
        private ActionResult ExpectedActionResult;
        private ActionResult Result;

        public override void Given()
        {
            pipeline = CreateStub<IActionResultPipeline>();

            ExpectedActionResult = CreateStub<ActionResult>();
            pipeline.Stub(factory => factory.Create(Arg<ActionResultSelector>.Is.Anything))
                .Return(ExpectedActionResult);

            FactoryOptions = CreateStub<ActionResultSelector>();
            actionResultResolver = new FluentMvcResolver(actionResultRegistry, actionFilterRegistry, CreateStub<IFluentMvcObjectFactory>());
            actionResultResolver.RegisterActionResultPipeline(pipeline);
        }

        public override void Because()
        {
            Result = actionResultResolver.CreateActionResult(FactoryOptions);
        }

        [Test]
        public void Should_return_the_correct_result()
        {
            Result.ShouldBeTheSameAs(ExpectedActionResult);
        }
    }

    [TestFixture]
    public class when_creating_an_action_result_that_is_present_in_the_registry : FluentMvcResolverSpecBase
    {
        private IActionResultPipeline pipeline;
        private ActionResultSelector FactoryOptions;
        private ActionResult ExpectedActionResult;
        private ActionResult Result;

        public override void Given()
        {
            pipeline = CreateStub<IActionResultPipeline>();

            ExpectedActionResult = CreateStub<ActionResult>();
            pipeline.Stub(factory => factory.Create(Arg<ActionResultSelector>.Is.Anything))
                .Return(ExpectedActionResult);

            FactoryOptions = CreateStub<ActionResultSelector>();
            actionResultResolver = new FluentMvcResolver(actionResultRegistry, actionFilterRegistry, CreateStub<IFluentMvcObjectFactory>());
            actionResultResolver.RegisterActionResultPipeline(pipeline);
        }

        public override void Because()
        {
            Result = actionResultResolver.CreateActionResult(FactoryOptions);
        }

        [Test]
        public void Should_return_the_correct_result()
        {
            Result.ShouldBeTheSameAs(ExpectedActionResult);
        }
    }
}