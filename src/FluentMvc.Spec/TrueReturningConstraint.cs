namespace FluentMvc.Spec
{
    using Constraints;

    public class TrueReturningConstraint : PredefinedConstraint
    {
        public TrueReturningConstraint() : base(true)
        {
        }
    }
}