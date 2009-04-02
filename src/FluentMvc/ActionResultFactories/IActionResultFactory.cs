namespace FluentMvc.ActionResultFactories
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Configuration;

    public interface IActionResultFactory
    {
        ActionResult Create(ActionResultSelector selector);
        bool ShouldBeReturnedFor(ActionResultSelector selector);
        void OverrideConstraint(IEnumerable<TransientConstraintRegistration> constraintRegistrations);
    }
}