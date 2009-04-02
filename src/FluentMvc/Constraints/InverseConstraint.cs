namespace FluentMvc.Constraints
{
    public class InverseConstraint : IConstraint
    {
        private readonly IConstraint constraint;

        public InverseConstraint(IConstraint constraint)
        {
            this.constraint = constraint;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return !constraint.IsSatisfiedBy(selector);
        }
    }
}