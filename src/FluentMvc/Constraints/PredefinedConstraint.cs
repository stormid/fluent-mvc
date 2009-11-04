namespace FluentMvc.Constraints
{
    public class PredefinedConstraint : IConstraint
    {
        private readonly bool value;

        public static IConstraint True = new PredefinedConstraint(true);
        public static IConstraint False = new PredefinedConstraint(false);

        protected PredefinedConstraint(bool value)
        {
            this.value = value;
        }

        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return value;
        }
    }
}