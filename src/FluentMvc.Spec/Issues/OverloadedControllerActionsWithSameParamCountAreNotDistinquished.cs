using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentMvc.Configuration;
using FluentMvc.Conventions;
using FluentMvc.Utils;
using NUnit.Framework;

namespace FluentMvc.Spec.Issues
{
    public class OverloadedControllerActionsWithSameParamCountAreNotDistinquished
    {
        public class ControllerWithOverloadedActions : Controller
        {
            public ActionResult OverloadedAction(Guid id)
            {
                return null;
            }

            public ActionResult OverloadedAction(Model model)
            {
                return null;
            }
        }

        public class Model {}

        public abstract class when_filter_is_applied_to_one_version_of_action : SpecificationBase
        {
            private ActionFilterRegistry actionFilterRegistry;
            protected int foundFilterCount;

            public override void Given()
            {
                Expression<Func<ControllerWithOverloadedActions, object>> registeredAction = c => c.OverloadedAction(default(Model));

                actionFilterRegistry = new ActionFilterRegistry(CreateStub<IFluentMvcObjectFactory>());

                FluentMvcConfiguration.Create(CreateStub<IFluentMvcResolver>(), actionFilterRegistry, CreateStub<IActionResultRegistry>(), CreateStub<IFilterConventionCollection>())
                    .WithFilter<TestActionFilter>(Apply.For(registeredAction))
                    .BuildFilterProvider();
            }

            public override void Because()
            {
                var calledAction = CalledAction;

                var actionDescriptor = calledAction.CreateActionDescriptor();

                var controllerActionFilterSelector = new ControllerActionFilterSelector(new ControllerContext(), actionDescriptor, actionDescriptor.ControllerDescriptor);

                var actionFilterRegistryItems = actionFilterRegistry.FindForSelector(controllerActionFilterSelector);
                foundFilterCount = actionFilterRegistryItems.Length;
            }

            protected abstract Expression<Func<ControllerWithOverloadedActions, object>> CalledAction { get; }
        }

        [TestFixture]
        public class selection_made_on_action_that_is_not_registered : when_filter_is_applied_to_one_version_of_action
        {
            protected override Expression<Func<ControllerWithOverloadedActions, object>> CalledAction { get { return c => c.OverloadedAction(default(Guid)); } }

            [Test]
            public void zero_filters_are_found()
            {
                Assert.AreEqual(0, foundFilterCount);
            }
        }

        [TestFixture]
        public class selection_made_on_registered_action : when_filter_is_applied_to_one_version_of_action
        {
            protected override Expression<Func<ControllerWithOverloadedActions, object>> CalledAction { get { return c => c.OverloadedAction(default(Model)); } }

            [Test]
            public void one_filters_are_found()
            {
                Assert.AreEqual(1, foundFilterCount);
            }
        }
    }
}