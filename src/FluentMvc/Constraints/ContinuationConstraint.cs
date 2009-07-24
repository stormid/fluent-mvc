namespace FluentMvc.Constraints
{
    public class ContinuationConstraint : IConstraint
    {
        private readonly IConstraint constraint;
        private readonly IConstraint constraints;

        public ContinuationConstraint(IConstraint constraint, IConstraint constraints)
        {
            this.constraint = constraint;
            this.constraints = constraints;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return constraint.IsSatisfiedBy(selector) && constraints.IsSatisfiedBy(selector);
        }
    }
}