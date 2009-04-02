namespace FluentMvc.Spec.Unit.ActionResultFactories
{
    using System.Web.Mvc;
    using FluentMvc.ActionResultFactories;
    using NUnit.Framework;

    [TestFixture]
    public class When_ViewResult_checking_if_the_factory_should_be_used_with_correct_accept_type : SpecificationBase
    {
        private bool Result;
        private ActionResultFactoryTester<ViewResultFactory> actionResultFactoryTester;

        public override void Given()
        {
            actionResultFactoryTester = new ActionResultFactoryTester<ViewResultFactory>(new[] { "text/html" }, null);
        }

        public override void Because()
        {
            Result = actionResultFactoryTester.GetShouldbeReturned();
        }

        [Test]
        public void Result_should_be_true()
        {
            Result.ShouldBeTrue();
        }
    }

    [TestFixture]
    public class When_ViewResult_checking_if_the_factory_should_be_used_with_incorrect_accept_type : SpecificationBase
    {
        private bool Result;
        private ActionResultFactoryTester<ViewResultFactory> actionResultFactoryTester;

        public override void Given()
        {
            actionResultFactoryTester = new ActionResultFactoryTester<ViewResultFactory>(new[] { string.Empty }, null);
        }

        public override void Because()
        {
            Result = actionResultFactoryTester.GetShouldbeReturned();
        }

        [Test]
        public void Result_should_be_false()
        {
            Result.ShouldBeFalse();
        }
    }

    [TestFixture]
    public class When_AcceptType_Is_text_html : SpecificationBase
    {
        private ActionResultFactoryTester<ViewResultFactory> actionResultFactoryTester;
        private ViewResult Result;

        public override void Given()
        {
            actionResultFactoryTester = new ActionResultFactoryTester<ViewResultFactory>(new[] { "text/html" }, new ViewDataDictionary { { "Test", new object() } });
        }

        public override void Because()
        {
            Result = actionResultFactoryTester.CreateResult() as ViewResult;
        }

        [Test]
        public void Should_return_a_ViewResult()
        {
            Result.ShouldBeOfType(typeof(ViewResult));
        }

        [Test]
        public void Should_Set_The_Correct_ViewData()
        {
            Result.ViewData.ShouldBeOfType(actionResultFactoryTester.ReturnValue.GetType());
        }
    }
}