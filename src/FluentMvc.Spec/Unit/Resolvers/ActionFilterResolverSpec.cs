using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentMvc.Configuration;
using FluentMvc.Constraints;
using FluentMvc.Spec.Issues;
using FluentMvc.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentMvc.Spec.Unit.Resolvers
{
    public class ActionFilterResolverSpec
    {
        [TestFixture]
        public class when_getting_all_filters : SpecificationBase
        {
            private ActionFilterResolver resolver;
            private IEnumerable<Filter> filters;
            private IActionFilterRegistry actionFilterRegistry;
            private ActionFilterRegistryItem globalFilter;
            private ActionDescriptor actionDescriptor;
            private ControllerRegistryItem controllerFilterItem;
            private ActionFilterRegistryItem controllerActionFilterItem;

            public override void Given()
            {
                var fluentMvcObjectFactory = CreateStub<IFluentMvcObjectFactory>();
                fluentMvcObjectFactory.Stub(x => x.Resolve<object>(Arg<Type>.Is.Anything)).Return(new TestFilter());

                actionFilterRegistry = new ActionFilterRegistry(fluentMvcObjectFactory);
                globalFilter = new GlobalActionFilterRegistryItem(typeof(TestActionFilter), PredefinedConstraint.True);

                Expression<Func<TestController, object>> func = c => c.ReturnPost();
                actionDescriptor = func.CreateActionDescriptor();

                controllerFilterItem = new ControllerRegistryItem(typeof(TestActionFilter3), PredefinedConstraint.True, actionDescriptor.ControllerDescriptor);
                controllerActionFilterItem = new ControllerActionRegistryItem(typeof(TestActionFilter2),
                                                                          PredefinedConstraint.True,
                                                                          actionDescriptor,
                                                                          actionDescriptor.ControllerDescriptor);

                actionFilterRegistry.Add(globalFilter);
                actionFilterRegistry.Add(controllerFilterItem);
                actionFilterRegistry.Add(controllerActionFilterItem);

                resolver = new ActionFilterResolver(actionFilterRegistry, fluentMvcObjectFactory);
            }

            public override void Because()
            {
                filters = resolver.GetFilters(null, actionDescriptor, actionDescriptor.ControllerDescriptor);
            }

            [Test]
            public void should_return_correct_global_filters()
            {
                filters.Count(x => x.Scope.Equals(FilterScope.Global)).ShouldEqual(1);
            }

            [Test]
            public void should_return_correct_action_filters()
            {
                filters.Count(x => x.Scope.Equals(FilterScope.Action)).ShouldEqual(1);
            }

            [Test]
            public void should_return_correct_controller_filters()
            {
                filters.Count(x => x.Scope.Equals(FilterScope.Controller)).ShouldEqual(1);
            }
        }
    }
}