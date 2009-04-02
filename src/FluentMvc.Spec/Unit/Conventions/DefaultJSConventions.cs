namespace FluentMvc.Spec.Unit.Conventions
{
    using System.Web.Mvc;
    using Configuration;
    using FluentMvc.ActionResultFactories;
    using NUnit.Framework;

    [TestFixture]
    public class Instantiating_new_instance : SpecificationBase
    {
        private MvcConvention Convention;

        public override void Because()
        {
            Convention = new MvcConvention();
        }

        [Test]
        public void ControllerFactory_will_be_DefaultControllerFactory()
        {
            Convention.ControllerFactory.ShouldBeOfType(typeof(IControllerFactory));
        }

        [Test]
        public void Should_have_correct_factories_set()
        {
            Convention.DefaultFactory.ShouldBeOfType(typeof(ViewResultFactory));
        }
    }
}

