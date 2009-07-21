namespace FluentMvc.Spec.Unit.ConfigurationDsl
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Configuration;
    using Configuration.Registrations;
    using Constraints;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Utils;

    // TODO: Tidy these up
    [TestFixture]
    public class ApplySpec : SpecificationBase
    {
        private IFluentMvcObjectFactory factory;

        public override void Because()
        {
            factory = CreateStub<IFluentMvcObjectFactory>();
            factory.Stub(x => x.CreateConstraint(Arg<Type>.Is.Anything)).Return(CreateStub<IConstraint>()).Repeat.
                Any();
        }

        [Test]
        public void When()
        {
            Apply.When<FalseReturningConstraint>().GetConstraintRegistrations(factory).Count().ShouldEqual(1);
        }

        [Test]
        public void When_for()
        {
            Apply.When<ExpectsJson>().For<TestController>().GetConstraintRegistrations(factory).Count().ShouldEqual(1);
        }

        [Test]
        public void For()
        {
            Apply.For<TestController>().GetConstraintRegistrations(factory).Count().ShouldEqual(1);
        }

        [Test]
        public void For_AndFor()
        {
            Apply.For<TestController>().AndFor<SecondTestController>().GetConstraintRegistrations(factory).Count()
                .ShouldEqual(2);
        }

        [Test]
        public void For_with_action()
        {
            Expression<Func<TestController, object>> expression = x => x.ReturnPost();
            AbstractTransientConstraintRegistration[] registrations = Apply.For(expression).ConstraintRegistrations;
            registrations.Length.ShouldEqual(1);
            registrations[0].ActionDescriptor.ActionName.ShouldEqual(expression.CreateActionDescriptor().ActionName);
        }
    }
}