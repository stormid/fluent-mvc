namespace FluentMvc.Constraints
{
    public class AndConstraint : IConstraint
    {
        private readonly IConstraint left;
        private readonly IConstraint right;

        public AndConstraint(IConstraint left, IConstraint right)
        {
            this.left = left;
            this.right = right;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return left.IsSatisfiedBy(selector) && right.IsSatisfiedBy(selector);
        }
    }
}