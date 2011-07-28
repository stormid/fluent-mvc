using System;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentMvc.Spec.Unit
{
    public class FluentMvcFilterProviderSpec
    {
        [TestFixture]
        public class when_getting_all_filters : SpecificationBase
        {
            private FluentMvcFilterProvider provider;
            private IFluentMvcResolver fluentMvcResolver;

            public override void Given()
            {
                fluentMvcResolver = CreateStub<IFluentMvcResolver>();
                provider = new FluentMvcFilterProvider(fluentMvcResolver);
            }

            public override void Because()
            {
                provider.GetFilters(new ControllerContext(), EmptyActionDescriptor.Instance);
            }

            [Test]
            public void should_return_correct_action_level_filters()
            {
                fluentMvcResolver.AssertWasCalled(x => x.GetFilters(Arg<ControllerContext>.Is.Anything, Arg<ActionDescriptor>.Is.Anything, Arg<ControllerDescriptor>.Is.Anything));
            }
        }
    }
}