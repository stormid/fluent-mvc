using FluentMvc.Conventions;

namespace FluentMvc.Spec.Unit.ConfigurationDsl
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using ActionFilterRegistrySpecs;
    using Configuration;
    using FluentMvc.ActionResultFactories;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Utils;

    [TestFixture]
    public class when_registering_a_custom_object_factory_with_a_result_factory : DslSpecBase
    {
        private IFluentMvcObjectFactory objectFactory;
        private IActionFilterRegistry actionFilterRegistry;

        public override void Given()
        {
            objectFactory = CreateStub<IFluentMvcObjectFactory>();
            actionFilterRegistry = CreateStub<IActionFilterRegistry>();
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .ResolveWith(objectFactory)
                .WithResultFactory<JsonResultFactory>();
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_set_the_object_factory()
        {
            actionFilterRegistry.AssertWasCalled(f => f.SetObjectFactory(Arg<IFluentMvcObjectFactory>.Is.Anything));
        }

        [Test]
        public void should_use_object_factory()
        {
            objectFactory.AssertWasCalled(o => o.CreateFactory<JsonResultFactory>());
        }

    }

    [TestFixture]
    public class when_registering_an_action_filter_with_no_constraint : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;

        public override void Given()
        {
            Expression<Func<TestController, object>> func = c => c.ReturnPost();
            actionDescriptor = func.CreateActionDescriptor();
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>();
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_register_action_filter()
        {
            actionFilterRegistry.Registrations.Length.ShouldEqual(1);
        }

        [Test]
        public void should_be_able_to_retrieve_item()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
        }
    }

    [TestFixture]
    public class when_registering_an_action_filter_with_no_constraint_and_one_with_a_constraint_for_another_controller : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;

        public override void Given()
        {
            Expression<Func<TestController, object>> func = c => c.ReturnPost();
            actionDescriptor = func.CreateActionDescriptor();
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>()
                .WithFilter<AuthorizeAttribute>(Apply.For<SecondTestController>());
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_register_action_filters()
        {
            actionFilterRegistry.Registrations.Length.ShouldEqual(2);
        }

        [Test]
        public void should_be_able_to_retrieve_item_that_does_not_have_a_constaint()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
        }
    }

    [TestFixture]
    public class when_registering_an_action_filter_with_a_constraint : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;

        public override void Given()
        {
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>(Apply.When<TrueReturningConstraint>());
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_register_with_constraint()
        {
            actionFilterRegistry.Registrations.Length.ShouldEqual(1);
        }
    }

    [TestFixture]
    public class when_registering_an_action_filter_with_an_except_constraint_on_controller_type : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;
        private ActionDescriptor anotherActionDescriptor;

        public override void Given()
        {
            Expression<Func<TestController, object>> func = c => c.ReturnPost();
            Expression<Func<SecondTestController, object>> func2 = c => c.ReturnPost();
            actionDescriptor = func.CreateActionDescriptor();
            anotherActionDescriptor = func2.CreateActionDescriptor();
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>(Except.For<TestController>());
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_not_return_for_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor)).Length.ShouldEqual(0);
        }

        [Test]
        public void should_return_for_none_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), anotherActionDescriptor, anotherActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
        }
    }

    [TestFixture]
    public class when_registering_an_action_filter_with_an_except_constraint_on_two_controller_types : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;
        private ActionDescriptor secondActionDescriptor;
        private ActionDescriptor thirdActionDescriptor;

        public override void Given()
        {
            Expression<Func<TestController, object>> func = c => c.ReturnPost();
            Expression<Func<SecondTestController, object>> func2 = c => c.ReturnPost();
            Expression<Func<ThirdTestController, object>> func3 = c => c.ReturnPost();
            actionDescriptor = func.CreateActionDescriptor();
            secondActionDescriptor = func2.CreateActionDescriptor();
            thirdActionDescriptor = func3.CreateActionDescriptor();
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>(Except.For<TestController>().AndFor<ThirdTestController>());
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_not_return_for_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor)).Length.ShouldEqual(0);
        }

        [Test]
        public void should_return_for_none_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), secondActionDescriptor, secondActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
        }

        [Test]
        public void should_not_return_for_none_matching_second_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), thirdActionDescriptor, thirdActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(0);
        }
    }

    [TestFixture]
    public class when_registering_an_authorize_filter_with_an_except_constraint_on_two_controller_types : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;
        private ActionDescriptor secondActionDescriptor;
        private ActionDescriptor thirdActionDescriptor;

        public override void Given()
        {
            Expression<Func<TestController, object>> func = c => c.ReturnPost();
            Expression<Func<SecondTestController, object>> func2 = c => c.ReturnPost();
            Expression<Func<ThirdTestController, object>> func3 = c => c.ReturnPost();
            actionDescriptor = func.CreateActionDescriptor();
            secondActionDescriptor = func2.CreateActionDescriptor();
            thirdActionDescriptor = func3.CreateActionDescriptor();
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<AuthorizeAttribute>(Except.For<TestController>().AndFor<ThirdTestController>());
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_not_return_for_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor)).Length.ShouldEqual(0);
        }

        [Test]
        public void should_return_for_none_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), secondActionDescriptor, secondActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
        }

        [Test]
        public void should_not_return_for_none_matching_second_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), thirdActionDescriptor, thirdActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(0);
        }
    }

    [TestFixture]
    public class when_registering_an_authorize_filter_with_an_except_constraint_on_two_controller_types_and_a_global_filter : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;
        private ActionDescriptor secondActionDescriptor;
        private ActionDescriptor excludedActionDescriptor;

        public override void Given()
        {
            Expression<Func<TestController, object>> func = c => c.ReturnPost();
            Expression<Func<SecondTestController, object>> func2 = c => c.ReturnPost();
            Expression<Func<ThirdTestController, object>> func3 = c => c.ReturnPost();
            actionDescriptor = func.CreateActionDescriptor();
            secondActionDescriptor = func2.CreateActionDescriptor();
            excludedActionDescriptor = func3.CreateActionDescriptor();
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<AcceptVerbsAttribute>()
                .WithFilter<AuthorizeAttribute>(Except.For<TestController>().AndFor<ThirdTestController>(func3));
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_return_for_only_global_filter_for_matching_controller_type_of_authorize()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor))
                .Length.ShouldEqual(1);
        }

        [Test]
        public void should_return_for_matching_controller_and_different_action()
        {
            Expression<Func<ThirdTestController, object>> func = c => c.ReturnNull();
            var descriptor = func.CreateActionDescriptor();
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), descriptor, descriptor.ControllerDescriptor)).Length
                .ShouldEqual(2);
        }

        [Test]
        public void should_return_all_for_none_matching_controller_type()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), secondActionDescriptor, secondActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(2);
        }

        [Test]
        public void should_return_global_filter_only_for_excluded_controller()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), excludedActionDescriptor, excludedActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), excludedActionDescriptor, excludedActionDescriptor.ControllerDescriptor))[0].Type.ShouldEqual(typeof(AcceptVerbsAttribute));
        }
    }

    [TestFixture]
    public class when_registering_an_action_filter_with_a_constraint_targeting_a_specific_action : DslSpecBase
    {
        private IActionFilterRegistry actionFilterRegistry;
        private ActionDescriptor actionDescriptor;
        private ActionDescriptor incorrectActionDescriptor;
        private ActionFilterRegistryTester registryTester;

        public override void Given()
        {
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Expression<Func<TestController, object>> func = controller => controller.ReturnPost();
            Expression<Func<TestController, object>> otherFunc = controller => controller.ReturnViewResult();
            actionDescriptor = func.CreateActionDescriptor();
            incorrectActionDescriptor = otherFunc.CreateActionDescriptor();
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>(Except.For<SecondTestController>().AndFor<TestController>(func).AndFor<TestController>(t => t.ReturnNull()));

            registryTester = new ActionFilterRegistryTester(actionFilterRegistry);
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_register_constraints()
        {
            registryTester.RegistryCount.ShouldEqual(3);
        }

        [Test]
        public void should_not_return_for_matching_controller_not_registered_with_any_actions()
        {
            registryTester.CountReturnedForControllerAndAction<SecondTestController>(x => x.DoSomething()).ShouldEqual(0);
        }

        [Test]
        public void should_not_be_able_to_retrieve_item_for_matching_controller_and_action()
        {
            registryTester.CountReturnedForControllerAndAction(actionDescriptor).ShouldEqual(0);
        }

        [Test]
        public void should_return_for_incorrect_action_on_a_matching_controller()
        {
            registryTester.CountReturnedForControllerAndAction(incorrectActionDescriptor).ShouldEqual(1);
        }

        [Test]
        public void should_return_for_correct_action_and_incorrect_controller()
        {
            Expression<Func<ThirdTestController, object>> otherFunc = controller => controller.ReturnPost();
            ActionDescriptor descriptior = otherFunc.CreateActionDescriptor();
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), descriptior, descriptior.ControllerDescriptor)).Length.ShouldEqual(1);
        }

        [Test]
        public void should_return_for_incorrect_action_and_incorrect_controller()
        {
            Expression<Func<ThirdTestController, object>> otherFunc = controller => controller.ReturnNull();
            ActionDescriptor descriptior = otherFunc.CreateActionDescriptor();
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), descriptior, descriptior.ControllerDescriptor)).Length.ShouldEqual(1);
        }

    }

    [TestFixture]
    public class when_adding_a_filter_globally_for_a_controller_except_for_a_specific_action : DslSpecBase
    {
        private ActionDescriptor actionDescriptor;
        private ActionDescriptor incorrectActionDescriptor;
        private IActionFilterRegistry actionFilterRegistry;

        public override void Given()
        {
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            Expression<Func<TestController, object>> func = controller => controller.ReturnPost();
            Expression<Func<TestController, object>> otherFunc = controller => controller.ReturnViewResult();
            actionDescriptor = func.CreateActionDescriptor();
            incorrectActionDescriptor = otherFunc.CreateActionDescriptor();
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                .WithFilter<TestActionFilter>(Apply.For<TestController>().ExceptFor(otherFunc));
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_not_return_the_attribute_for_the_ignored_action()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), incorrectActionDescriptor, incorrectActionDescriptor.ControllerDescriptor)).Length.ShouldEqual(0);
        }

        [Test]
        public void should_return_the_attribute_for_any_none_ignored_action()
        {
            actionFilterRegistry.FindForSelector(new ActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor)).Length.ShouldEqual(1);
        }
    }
}