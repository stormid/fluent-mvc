namespace FluentMvc.Spec.Bugs
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Configuration;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Utils;

    [TestFixture]
    public class RegisteringFilterTypeForMultipileControllersRemovesItFromSelectedFilters : SpecificationBase
    {
        private TestActionFilter actionFilter;
        private IFluentMvcObjectFactory objectFactory;
        private ActionFilterRegistry actionFilterRegistry;
        private Expression<Func<TestController, object>> func;

        public override void Because()
        {
            actionFilter = new TestActionFilter();
            objectFactory = CreateStub<IFluentMvcObjectFactory>();
            objectFactory.Stub(of => of.CreateFilter<IActionFilter>(Arg<Type>.Is.Anything))
                .Return(actionFilter).Repeat.Any();

            actionFilterRegistry = new ActionFilterRegistry(objectFactory);

            func = c => c.ReturnPost();

            var config = FluentMvcConfiguration.Create(CreateStub<IActionResultResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>());

            config.ExposeConfiguration(x =>
                                           {
                                               x.WithFilter<TestActionFilter>(Except.For<ThirdTestController>());

                                               x.WithFilter<TestActionFilter>(Apply.For<TestController>().AndFor<SecondTestController>());
                                           });

            config.BuildControllerFactory();
        }

        [Test]
        public void should_return_the_correct_filters()
        {
            var actionDesc = func.CreateActionDescriptor();
            ActionFilterRegistryItem[] selector = actionFilterRegistry.FindForSelector(new ActionFilterSelector(null, actionDesc, actionDesc.ControllerDescriptor));

            selector.Length.ShouldEqual(1);
        }

    }
}
