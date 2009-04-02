namespace FluentMvc
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;

    public abstract class AbstractActionResultFactory : IActionResultFactory
    {
        private IEnumerable<TransientConstraintRegistration> constaints;

        public void OverrideConstraint(IEnumerable<TransientConstraintRegistration> constraintRegistrations)
        {
            constaints = constraintRegistrations;
        }

        public virtual bool ShouldBeReturnedFor(ActionResultSelector selector)
        {
            bool shouldApplyFactory = constaints != null && constaints.Any(x => x.Constraint.IsSatisfiedBy(selector));

            if (shouldApplyFactory)
                return true;

            shouldApplyFactory = ShouldApplyFactory(selector);

            return shouldApplyFactory;
        }

        protected abstract bool ShouldApplyFactory(ActionResultSelector selector);

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

        public void OverrideConstraint(IEnumerable<TransientConstraintRegistration> constraintRegistrations)
        {
            
        }

        public bool ShouldBeReturnedFor(ActionResultSelector selector)
        {
            return false;
        }
    }
}