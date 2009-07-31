namespace FluentMvc.Constraints
{
    public class ContinuationConstraint : IConstraint
    {
        private readonly IConstraint firstConstraint;
        private readonly IConstraint secondConstraint;

        public ContinuationConstraint(IConstraint firstConstraint, IConstraint secondConstraint)
        {
            this.firstConstraint = firstConstraint;
            this.secondConstraint = secondConstraint;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return firstConstraint.IsSatisfiedBy(selector) && secondConstraint.IsSatisfiedBy(selector);
        }
    }
}