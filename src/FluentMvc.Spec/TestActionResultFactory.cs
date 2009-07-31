namespace FluentMvc.Spec
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Constraints;
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

        public void SetConstraints(IEnumerable<IConstraint> constraintRegistrations)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConstraint> Constraints
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal class TestActionResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            
        }
    }
}