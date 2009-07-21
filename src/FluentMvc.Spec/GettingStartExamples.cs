namespace FluentMvc.Spec
{
    using System.Web.Mvc;
    using ActionResultFactories;
    using Configuration;
    using FluentMvc;
    using NUnit.Framework;
    using Utils;

    [TestFixture]
    public class GettingStartExamples
    {
        [Test]
        public void new_configuration()
        {
            FluentMvcConfiguration.Configure = config =>
                                                   {
                                                       config.UsingControllerFactory(new DefaultControllerFactory());

                                                       config.WithResultFactory<ActionResultFactory>(Is.Default);

                                                       config.WithFilter<AuthorizeAttribute>();
                                                   };

            ControllerBuilder.Current.BuildFromFluentMcv();
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

        [Test]
        public void Securing_a_specific_action_for_a_role()
        {
            FluentMvcConfiguration.Create()
                .UsingControllerFactory(new DefaultControllerFactory())
                .WithConvention(new MvcConvention())
                .WithFilter(new AuthorizeAttribute { Roles = "Role"}, Apply.For<TestController>(tc => tc.ReturnPost()))
                .BuildControllerFactory();
        }
    }
}