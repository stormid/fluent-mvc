namespace FluentMvc.Spec
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;
    using FluentMvc;
    using System.Linq;

    internal class TestActionResultFactory : IActionResultFactory
    {
        public ActionResult Create(ActionResultSelector selector)
        {
            return new TestActionResult();
        }

        public bool ShouldBeReturnedFor(ActionResultSelector selector)
        {
            return selector.AcceptTypes.Contains("accept");
        }

        public IActionResultFactory Child
        {
            get { throw new System.NotImplementedException(); }
        }

        public void SetChild(IActionResultFactory newChild)
        {
            throw new System.NotImplementedException();
        }

        public void OverrideConstraint(IEnumerable<TransientConstraintRegistration> constraintRegistrations)
        {
            throw new NotImplementedException();
        }

        public void AddFiltersTo(FilterInfo baseBilterInfo, ActionFilterSelector context)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterFilter(IActionFilter actionFilter)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterFilter(IAuthorizationFilter authorizefilter)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterFilter(IResultFilter resultFilter)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class TestActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            
        }
    }
}