namespace FluentMvc.Spec.Unit.ConfigurationDsl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using ActionFilterRegistry;
    using Configuration;
    using Constraints;
    using FluentMvc;
    using FluentMvc.ActionResultFactories;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Utils;

    [TestFixture]
    public class When_no_settings_have_been_set : DslSpecBase
    {
        public override void Because()
        {
            Configuration = FluentMvcConfiguration.Create();
        }

        [Test]
        public void Should_have_default_conventions()
        {
            Configuration.Convention.ShouldBeOfType(typeof(FluentMvcConvention));
        }
    }

    [TestFixture]
    public class when_registering_an_instance_of_a_filter_with_no_contraints : DslSpecBase
    {
        private ActionFilterRegistry actionFilterRegistry;
        private IActionFilter filterInstance;

        public override void Because()
        {
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            filterInstance = CreateStub<IActionFilter>();
            FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
                .WithFilter(filterInstance).BuildControllerFactory();
        }

        [Test]
        public void should_be_registered_using_the_supplied_type()
        {
            actionFilterRegistry.Registrations.First().Create<IActionFilter>(CreateStub<IFluentMvcObjectFactory>()).ShouldEqual(filterInstance);
        }
    }

    [TestFixture]
    public class when_registering_an_instance_of_a_filter_with_a : DslSpecBase
    {
        private ActionFilterRegistry actionFilterRegistry;
        private IActionFilter filterInstance;

        public override void Because()
        {
            actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());
            filterInstance = CreateStub<IActionFilter>();
            FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
                .WithFilter(filterInstance, Apply.When<ExpectsJson>()).BuildControllerFactory();
        }

        [Test]
        public void should_be_registered_using_the_supplied_constraint()
        {
            actionFilterRegistry.Registrations.First().Constraint.ShouldBe(typeof(ExpectsJson));
        }
    }

    [TestFixture]
    public class When_creating_a_factory_with_an_inner_controllerfactory : DslSpecBase
    {
        private IControllerFactory Factory;
        private DefaultControllerFactory ExpectedControllerFactory;

        public override void Given()
        {
            ExpectedControllerFactory = new DefaultControllerFactory();
            Configuration = FluentMvcConfiguration.Create()
                .UsingControllerFactory(ExpectedControllerFactory);
        }

        public override void Because()
        {
            Factory = Configuration
                .BuildControllerFactory();
        }

        [Test]
        public void Should_set_inner_conventions_controller_factory()
        {
            Configuration.Convention.ControllerFactory.ShouldBeTheSameAs(ExpectedControllerFactory);
        }

        [Test]
        public void Should_create_the_factory()
        {
            Factory.ShouldNotBeNull();
        }
    }

    [TestFixture]
    public class When_adding_two_action_result_factories : DslSpecBase
    {
        private IActionResultFactory Child1;
        private IActionResultFactory Child2;

        public override void Given()
        {
            Configuration = FluentMvcConfiguration.Create();
            Child1 = CreateStub<IActionResultFactory>();
            Child2 = CreateStub<IActionResultFactory>();
        }

        public override void Because()
        {
            Configuration
                .WithResultFactory(Child1)
                .WithResultFactory(Child2);
        }

        [Test]
        public void Should_set_inner_conventions_factories()
        {
            Configuration.Convention.Factories.ToArray()[0].ShouldBeTheSameAs(Child1);
            Configuration.Convention.Factories.ToArray()[1].ShouldBeTheSameAs(Child2);
        }
    }

    [TestFixture]
    public class When_adding_one_action_result_factory_generically : DslSpecBase
    {

        public override void Given()
        {
            Configuration = FluentMvcConfiguration.Create();
        }

        public override void Because()
        {
            Configuration
                .WithResultFactory<TestActionResultFactory>();
        }

        [Test]
        public void Should_set_inner_conventions_factories()
        {
            Configuration.Convention.Factories.ToArray()[0].ShouldBe(typeof (TestActionResultFactory));
        }
    }

    [TestFixture]
    public class When_adding_one_action_result_factories_with_a_constraint : DslSpecBase
    {
        private IActionResultFactory resultFactory;

        public override void Given()
        {
            resultFactory = CreateStub<IActionResultFactory>();
            Configuration = FluentMvcConfiguration.Create();
        }

        public override void Because()
        {
            Configuration
                .WithResultFactory(resultFactory, Apply.When<ExpectsJson>());
        }

        [Test]
        public void should_override_factory_constraint()
        {
            resultFactory.AssertWasCalled(x => x.SetConstraint(Arg<IEnumerable<IConstraint>>.Is.Anything));
        }
    }

    [TestFixture]
    public class When_overriding_the_convention : DslSpecBase
    {
        private FluentMvcConvention ExpectedConvention;

        public override void Given()
        {
            Configuration = FluentMvcConfiguration.Create();
            ExpectedConvention = CreateStub<FluentMvcConvention>();
        }

        public override void Because()
        {
            Configuration.WithConvention(ExpectedConvention);
        }

        [Test]
        public void Should_set_the_inner_convention()
        {
            Configuration.Convention.ShouldBeTheSameAs(ExpectedConvention);
        }
    }

    [TestFixture]
    public class When_editing_convention : DslSpecBase
    {
        private bool WasCalled;

        public override void Given()
        {
            Configuration = FluentMvcConfiguration.Create();
        }

        public override void Because()
        {
            Configuration.WithConvention(convention =>
                                             {
                                                 WasCalled = true;
                                             });
        }

        [Test]
        public void Should_invoke_action()
        {
            WasCalled.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_setting_the_default_factory : DslSpecBase
    {
        private IActionResultFactory ExpectedDefaultFactory;

        public override void Given()
        {
            ExpectedDefaultFactory = CreateStub<IActionResultFactory>();
            Configuration = FluentMvcConfiguration.Create();
        }

        public override void Because()
        {
            Configuration.WithResultFactory(ExpectedDefaultFactory, true);
        }

        [Test]
        public void Should_set_the_inner_convention()
        {
            Configuration.Convention.DefaultFactory.ShouldBeTheSameAs(ExpectedDefaultFactory);
        }
    }

    [TestFixture]
    public class When_setting_the_default_factory_generically : DslSpecBase
    {
        private IActionResultFactory ExpectedDefaultFactory;

        public override void Given()
        {
            ExpectedDefaultFactory = CreateStub<IActionResultFactory>();
            Configuration = FluentMvcConfiguration.Create();
        }

        public override void Because()
        {
           Configuration.WithResultFactory<TestActionResultFactory>(true);
        }

        [Test]
        public void Should_set_the_inner_convention()
        {
            Configuration.Convention.DefaultFactory.ShouldBe(typeof(TestActionResultFactory));
        }
    }

    [TestFixture]
    public class when_building_controller_factory : DslSpecBase
    {
        private IFluentMvcResolver fluentMvcResolver;
        private IActionFilterRegistry actionFilterRegistry;
        private IActionResultRegistry actionResultRegistry;

        public override void Given()
        {
            fluentMvcResolver = CreateStub<IFluentMvcResolver>();
            actionFilterRegistry = CreateStub<IActionFilterRegistry>();
            actionResultRegistry = CreateStub<IActionResultRegistry>();

            Configuration = FluentMvcConfiguration
                .Create(fluentMvcResolver, actionFilterRegistry, actionResultRegistry);
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_set_resolvers_action_result_registry()
        {
            fluentMvcResolver
                .AssertWasCalled(arr => arr.SetActionResultRegistry(Arg<IActionResultRegistry>.Is.Anything));
        }

        [Test]
        public void should_set_resolvers_action_filter_registry()
        {
            fluentMvcResolver
                .AssertWasCalled(arr => arr.SetActionFilterRegistry(Arg<IActionFilterRegistry>.Is.Anything));
        }

        [Test]
        public void should_set_resolvers_action_result_pipeline()
        {
            fluentMvcResolver
                .AssertWasCalled(arr => arr.RegisterActionResultPipeline(Arg<IActionResultPipeline>.Is.Anything));
        }
    }

    [TestFixture]
    public class when_registering_a_custom_object_factory_with_a_result_factory : DslSpecBase
    {
        private IFluentMvcObjectFactory objectFactory;
        private IActionFilterRegistry filterRegistry;

        public override void Given()
        {
            objectFactory = CreateStub<IFluentMvcObjectFactory>();
            filterRegistry = CreateStub<IActionFilterRegistry>();
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), filterRegistry, CreateStub<IActionResultRegistry>())
                .ResolveWith(objectFactory)
                .WithResultFactory<JsonResultFactory>();
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_set_action_filter_registries_object_factory()
        {
            filterRegistry.AssertWasCalled(f => f.SetObjectFactory(Arg<IFluentMvcObjectFactory>.Is.Anything));
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
                .WithFilter<TestActionFilter>(Apply.When<TrueReturningConstraint>());
        }

        public override void Because()
        {
            Configuration.BuildControllerFactory();
        }

        [Test]
        public void should_register_with_constraint()
        {
            actionFilterRegistry.Registrations.Count().ShouldEqual(1);
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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
            Configuration = FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>())
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


}