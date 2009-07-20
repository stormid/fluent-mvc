namespace FluentMvc.Spec.Bugs
{
    using NUnit.Framework;
    using Utils;

    [TestFixture]
    public class HasItemsExtensionMethodBug
    {
        [Test]
        public void should_be_true_when_there_are_items()
        {
            string[] items = new[] {"test", "string"};
            items.HasItems().ShouldBeTrue();
        }

        [Test]
        public void should_be_false_when_there_are_no_items()
        {
            string[] items = new string[] {};
            items.HasItems().ShouldBeFalse();
        }

        [Test]
        public void should_be_false_when_collection_is_null()
        {
            string[] items = null;
            items.HasItems().ShouldBeFalse();
        }
    }
}