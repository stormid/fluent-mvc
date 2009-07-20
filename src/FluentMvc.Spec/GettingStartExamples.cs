namespace FluentMvc.Spec
{
    using System.Web.Mvc;
    using Configuration;
    using FluentMvc;
    using NUnit.Framework;

    [TestFixture]
    public class GettingStartExamples
    {
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
                .UsingControllerFactory(customControllerFactory)
                .BuildControllerFactory();
        }

        [Test]
        public void Securing_a_specific_action()
        {
            FluentMvcConfiguration.Create()
                .UsingControllerFactory(new DefaultControllerFactory())
                .WithConvention(new MvcConvention())
                .WithFilter<AuthorizeAttribute>(Apply.For<TestController>(tc => tc.ReturnPost()))
                .BuildControllerFactory();
        }

    }
}