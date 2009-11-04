namespace FluentMvc.Constraints
{
    public class AndConstraint : IConstraint
    {
        private readonly IConstraint first;
        private readonly IConstraint second;

        public AndConstraint(IConstraint first, IConstraint second)
        {
            this.first = first;
            this.second = second;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return first.IsSatisfiedBy(selector) && second.IsSatisfiedBy(selector);
        }
    }
}