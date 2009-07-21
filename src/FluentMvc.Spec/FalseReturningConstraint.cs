namespace FluentMvc.Spec
{
    using Constraints;

    public class FalseReturningConstraint : PredefinedConstraint
    {
        public FalseReturningConstraint()
            : base(false)
        {
        }
    }
}