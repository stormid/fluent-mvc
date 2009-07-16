namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ActionResultFactories;
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
            bool shouldApplyFactory = constaints.HasItems() && constaints.Any(x => x.IsSatisfiedBy(selector));

            if (shouldApplyFactory)
                return true;

            shouldApplyFactory = ShouldApplyFactory(selector);

            return shouldApplyFactory;
        }

        protected virtual bool ShouldApplyFactory(ActionResultSelector selector)
        {
            return false;
        }

        public virtual ActionResult Create(ActionResultSelector selector)
        {
            return CreateResult(selector);
        }

        protected abstract ActionResult CreateResult(ActionResultSelector selector);
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