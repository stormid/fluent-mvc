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
            var provider = FluentMvcConfiguration.ConfigureFilterProvider(config =>
                                                   {
                                                       config.WithResultFactory<ActionResultFactory>(isDefault:true);
                                                       config.WithFilter<AuthorizeAttribute>();
                                                   });
        }

        [Test]
        public void Standard_Mvc()
        {
            FluentMvcConfiguration.Create()
                .WithConvention(new MvcConvention())
                .BuildFilterProvider();
        }

        [Test]
        public void Securing_a_specific_action()
        {
            FluentMvcConfiguration.Create()
                .WithConvention(new MvcConvention())
                .WithFilter<AuthorizeAttribute>(Apply.For<TestController>(tc => tc.ReturnPost()))
                .BuildFilterProvider();
        }

        [Test]
        public void Securing_a_specific_action_for_a_role()
        {
            FluentMvcConfiguration.Create()
                .WithConvention(new MvcConvention())
                .WithFilter(new AuthorizeAttribute { Roles = "Role"}, Apply.For<TestController>(tc => tc.ReturnPost()))
                .BuildFilterProvider();
        }

        [Test]
        public void define_where_to_find_where_to_find_FilterConventions()
        {
            FluentMvcConfiguration.Configure = config =>
                                                   {
                                                       config.
                                                           FilterConventions.
                                                           LoadFromAssemblyContaining<GettingStartExamples>();
                                                   };
        }
    }
}