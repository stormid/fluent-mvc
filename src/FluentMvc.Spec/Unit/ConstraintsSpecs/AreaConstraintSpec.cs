using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentMvc.Constraints;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentMvc.Spec.Unit.ConstraintsSpecs
{
    [TestFixture]
    public class when_the_area_is_correct : SpecificationBase
    {
        private AreaConstraint constraint;
        private bool isSatisfiedBy;
        private ControllerContext controllerContext;

        public override void Given()
        {
            constraint = new AreaConstraint(new TestAreaRegistration());

            var routeData = new RouteData();
            routeData.DataTokens.Add("area", "Test");
            routeData.Values.Add("area", "Test");

            var httpContextBase = CreateStub<HttpContextBase>();
            httpContextBase.Stub(x => x.Request).Return(CreateStub<HttpRequestBase>());

            controllerContext = new ControllerContext
                                    {
                                        RequestContext = new RequestContext(httpContextBase, routeData),
                                    };
        }

        public override void Because()
        {
            isSatisfiedBy = constraint.IsSatisfiedBy(new ActionFilterSelector(controllerContext, null, null));
        }

        [Test]
        public void should_be_satisfied()
        {
            isSatisfiedBy.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class when_the_area_is_not_correct : SpecificationBase
    {
        private AreaConstraint constraint;
        private bool isSatisfiedBy;
        private ControllerContext controllerContext;

        public override void Given()
        {
            constraint = new AreaConstraint(new TestAreaRegistration());

            var routeData = new RouteData();
            routeData.DataTokens.Add("area", "Test2");
            routeData.Values.Add("area", "Test2");

            var httpContextBase = CreateStub<HttpContextBase>();
            httpContextBase.Stub(x => x.Request).Return(CreateStub<HttpRequestBase>());

            controllerContext = new ControllerContext
            {
                RequestContext = new RequestContext(httpContextBase, routeData),
            };
        }

        public override void Because()
        {
            isSatisfiedBy = constraint.IsSatisfiedBy(new ActionFilterSelector(controllerContext, null, null));
        }

        [Test]
        public void should_be_satisfied()
        {
            isSatisfiedBy.ShouldBeFalse();
        }
    }

    public class TestAreaRegistration : AreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            throw new NotImplementedException();
        }

        public override string AreaName
        {
            get { return "Test"; }
        }
    }

    public class AreaConstraint : IConstraint
    {
        private readonly TestAreaRegistration areaRegistration;

        public AreaConstraint(TestAreaRegistration areaRegistration)
        {
            this.areaRegistration = areaRegistration;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            var areaName = selector.RouteData.GetRequiredString("area");

            return areaRegistration.AreaName == areaName;
        }
    }
}