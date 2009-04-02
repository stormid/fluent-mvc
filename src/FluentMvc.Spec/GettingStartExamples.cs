namespace FluentMvc.Spec
{
    using System.Web.Mvc;
    using Configuration;
    using FluentMvc;
    using NUnit.Framework;

    [TestFixture]
    public class GettingStartExamples
    {
        private IActionResultRegistry actionResultRegistry;

        [SetUp]
        public void Setup()
        {
            actionResultRegistry = new ActionResultRegistry();
        }

        [Test]
        public void Standard_Mvc()
        {
            FluentMvcConfiguration.Create()
                .WithConvention(new MvcConvention())
                .BuildControllerFactory();
        }

        [Test]
        public void Standard_Mvc_Custom_ControllerFactory()
        {
            // Change this to your factory of choice
            IControllerFactory customControllerFactory = new DefaultControllerFactory();

            FluentMvcConfiguration.Create()
                .WithConvention(new MvcConvention())
                .WithControllerFactory(customControllerFactory)
                .BuildControllerFactory();
        }

        [Test]
        public void Securing_a_speicfic_action()
        {
            FluentMvcConfiguration.Create()
                .WithControllerFactory(new DefaultControllerFactory())
                .WithConvention(new MvcConvention())
                .AddFilter<AuthorizeAttribute>(Apply.For<TestController>(tc => tc.ReturnPost()))
                .BuildControllerFactory();
        }

    }
}