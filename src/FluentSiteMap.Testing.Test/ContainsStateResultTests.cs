using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class ContainsStateResultTests
        : TestBase
    {
        [Test]
        public void Instances_should_be_successful_with_an_empty_fail_message_if_created_with_the_default_constructor()
        {
            var target = new ContainsStateResult();
            
            Assert.That(target.Success, Is.True);
            Assert.That(target.FailReason, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Instances_should_be_failure_with_the_expected_fail_message_if_created_with_the_message_constructor()
        {
            var target = new ContainsStateResult("foo", "bar");

            Assert.That(target.Success, Is.False);
            Assert.That(target.FailReason, Is.EqualTo("foo: bar"));
        }

        [Test]
        public void Instances_should_be_failure_with_the_expected_fail_message_if_created_with_the_format_constructor()
        {
            var target = new ContainsStateResult("foo", "{0}!", "bar");

            Assert.That(target.Success, Is.False);
            Assert.That(target.FailReason, Is.EqualTo("foo: bar!"));
        }
    }
}