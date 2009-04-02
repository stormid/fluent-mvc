namespace FluentMvc.Spec
{
    using NUnit.Framework;

    public abstract class SpecificationBase : TestBase
    {
        [SetUp]
        public void Setup()
        {
            ForEach();
            Given();
            Because();
        }

        [TearDown]
        public void TearDown()
        {
            AfterEach();
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            ForAll();
        }

        [TestFixtureTearDown]
        public void FixtureTeardown()
        {
            AfterAll();
        }

        public virtual void ForAll() { }
        public virtual void ForEach() { }
        public virtual void Given() { }
        public abstract void Because();
        public virtual void AfterEach() { }
        public virtual void AfterAll() { }
    }
}