namespace FluentMvc.Spec.Unit.ConfigurationDsl
{
    using System.Web.Mvc;
    using Configuration;
    using NUnit.Framework;

    // Spike test for Mark Nijhof's suggested solution for config
    [TestFixture] 
    public class ConfigurationDsl_Take2_Spike : TestBase
    {
        private IControllerFactory controllerFactory;

        [SetUp]
        public void Setup()
        {
            FluentMvcConfiguration
                .Configure = x =>
                                 {
                                     x.ResolveWith(CreateStub<IFluentMvcObjectFactory>());
                                     x.WithResultFactory(new TestActionResultFactory());
                                 };

            controllerFactory = FluentMvcConfiguration.ConfigureAndBuildControllerFactory();
        }

        [Test]
        public void should_build_the_controller_factory()
        {
            controllerFactory.ShouldNotBeNull();
        }
    }
}