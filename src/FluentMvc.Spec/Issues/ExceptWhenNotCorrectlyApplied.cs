using FluentMvc.Configuration;
using FluentMvc.Constraints;
using NUnit.Framework;

namespace FluentMvc.Spec.Issues
{
    public class ExceptWhenNotCorrectlyApplied
    {
        [Test]
        public void should_create_not_constraint()
        {
            var whenDsl = Except.When<CustomContraint>();
            whenDsl.ConstraintRegistrations.Length.ShouldBeGreaterThan(0);
            whenDsl.ConstraintRegistrations[0].Prepare(new FluentMvcObjectFactory());
            whenDsl.ConstraintRegistrations[0].Constraint.ShouldBeOfType(typeof(NotConstraint));
        }
    }

    public class CustomContraint : IConstraint
    {
        public bool IsSatisfiedBy<T>(T selector) where T : RegistrySelector
        {
            return false;
        }
    }
}