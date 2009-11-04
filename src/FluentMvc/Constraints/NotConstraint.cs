namespace FluentMvc.Constraints
{
    public class NotConstraint : IConstraint
    {
        private readonly IConstraint constraint;

        public NotConstraint(IConstraint constraint)
        {
            this.constraint = constraint;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return !constraint.IsSatisfiedBy(selector);
        }
    }
}