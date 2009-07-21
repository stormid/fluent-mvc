namespace FluentMvc.ActionResultFactories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constraints;
    using Utils;

    public abstract class AbstractActionResultFactory : IActionResultFactory
    {
        private IEnumerable<IConstraint> constaints;

        public void SetConstraint(IEnumerable<IConstraint> constraintRegistrations)
        {
            constaints = constraintRegistrations;
        }

        public virtual bool ShouldBeReturnedFor(ActionResultSelector selector)
        {
            return IsConstraintSatisfied(selector) || ShouldBeReturnedForCore(selector);
        }

        private bool IsConstraintSatisfied(ActionResultSelector selector)
        {
            return constaints.HasItems() && constaints.Any(x => x.IsSatisfiedBy(selector));
        }

        protected virtual bool ShouldBeReturnedForCore(ActionResultSelector selector)
        {
            return false;
        }

        public virtual ActionResult Create(ActionResultSelector selector)
        {
            return CreateCore(selector);
        }

        protected abstract ActionResult CreateCore(ActionResultSelector selector);
    }

    internal class EmptyActionResultFactory : IActionResultFactory
    {
        public static readonly IActionResultFactory Instance = new EmptyActionResultFactory();

        private EmptyActionResultFactory()
        {
        }

        public ActionResult Create(ActionResultSelector selector)
        {
            return new EmptyResult();
        }

        public void SetConstraint(IEnumerable<IConstraint> constraintRegistrations)
        {
            
        }

        public bool ShouldBeReturnedFor(ActionResultSelector selector)
        {
            return false;
        }
    }
}