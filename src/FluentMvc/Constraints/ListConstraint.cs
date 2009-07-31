namespace FluentMvc.Constraints
{
    using System.Collections.Generic;
    using System.Linq;

    public class ListConstraint : IConstraint
    {
        private readonly IEnumerable<IConstraint> constraints;

        public ListConstraint(IEnumerable<IConstraint> constraints)
        {
            this.constraints = constraints;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return constraints.Any(constraint => constraint.IsSatisfiedBy(selector));
        }
    }
}
