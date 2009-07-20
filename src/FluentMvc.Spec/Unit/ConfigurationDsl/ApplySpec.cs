namespace FluentMvc.Spec.Unit.ConfigurationDsl
{
    using System;
    using System.Linq.Expressions;
    using Configuration;
    using Constraints;
    using NUnit.Framework;
    using Utils;

    // TODO: Tidy these up

    [TestFixture]
    public class ApplySpec : TestBase
    {
        [Test]
        public void When()
        {
            Apply.When<FalseReturningConstraint>().ConstraintRegistrations.Length.ShouldEqual(1);
        }

        [Test]
        public void For()
        {
            Apply.For<TestController>().ConstraintRegistrations.Length.ShouldEqual(1);
        }

        [Test]
        public void For_AndFor()
        {
            TransientConstraintRegistration[] registrations = Apply.For<TestController>().AndFor<SecondTestController>().ConstraintRegistrations;
            registrations.Length.ShouldEqual(2);
        }

        [Test]
        public void For_with_action()
        {
            Expression<Func<TestController, object>> expression = x => x.ReturnPost();
            TransientConstraintRegistration[] registrations = Apply.For(expression).ConstraintRegistrations;
            registrations.Length.ShouldEqual(1);
            registrations[0].ActionDescriptor.ActionName.ShouldEqual(expression.CreateActionDescriptor().ActionName);
        }
    }
}