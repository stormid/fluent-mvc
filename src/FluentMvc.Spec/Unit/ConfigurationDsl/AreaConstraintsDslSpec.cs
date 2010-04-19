using System;
using System.Linq;
using FluentMvc.Configuration;
using FluentMvc.Constraints;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentMvc.Spec.Unit.ConfigurationDsl
{
    [TestFixture]
    public class AreaConstraintsDslSpec : SpecificationBase
    {
        private IFluentMvcObjectFactory factory;

        public override void Because()
        {
            factory = CreateStub<IFluentMvcObjectFactory>();
            factory.Stub(x => x.CreateConstraint(Arg<Type>.Is.Anything)).Return(CreateStub<IConstraint>()).Repeat.
                Any();
        }

        [Test]
        public void can_apply()
        {
            Apply.ForArea<TestAreaRegistration>().GetConstraintRegistrations(factory).Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public void can_except()
        {
            Except.ForArea<TestAreaRegistration>().GetConstraintRegistrations(factory).Count().ShouldBeGreaterThan(0);
        }
    }
}