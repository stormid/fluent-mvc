namespace FluentMvc.Spec.Unit.ActionResultFactories
{
    using System.Web.Mvc;
    using FluentMvc.ActionResultFactories;
    using NUnit.Framework;

    [TestFixture]
    public class When_checking_to_be_used_with_return_not_an_actionresult : SpecificationBase
    {
        private bool Result;
        private ActionResultFactoryTester<ActionResultFactory> actionResultFactoryTester;

        public override void Given()
        {
            actionResultFactoryTester = new ActionResultFactoryTester<ActionResultFactory>(new[] { "text/html" }, null);
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
    public class When_checking_to_be_used_with_return_an_actionresult : SpecificationBase
    {
        private bool Result;
        private ActionResultFactoryTester<ActionResultFactory> actionResultFactoryTester;

        public override void Given()
        {
            actionResultFactoryTester = new ActionResultFactoryTester<ActionResultFactory>(new[] { "text/html" }, new EmptyResult());
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
    public class When_creating_actionresult : SpecificationBase
    {
        private ActionResult Result;
        private ActionResultFactoryTester<ActionResultFactory> actionResultFactoryTester;
        private EmptyResult ExpectedResult;

        public override void Given()
        {
            ExpectedResult = new EmptyResult();
            actionResultFactoryTester = new ActionResultFactoryTester<ActionResultFactory>(new[] { "text/html" }, ExpectedResult);
        }

        public override void Because()
        {
            Result = actionResultFactoryTester.CreateResult();
        }

        [Test]
        public void Result_should_be_true()
        {
            Result.ShouldBeTheSameAs(ExpectedResult);
        }
    }
}