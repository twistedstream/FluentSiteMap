using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class ContainsStateConstraintTests
        : TestBase
    {
        [Test]
        public void Matches_should_succeed_if_the_actual_state_contains_the_expected_state()
        {
            var actual = new {Name = "Bob", Age = 42};
            var expected = new {Name = "Bob"};

            var target = new ContainsStateConstraint(expected);

            var result = target.Matches(actual);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Matches_should_fail_if_the_actual_state_contains_the_expected_state()
        {
            var actual = new { Name = "Bob", Age = 42 };
            var expected = new { Name = "Bob", Age = 24 };

            var target = new ContainsStateConstraint(expected);

            var result = target.Matches(actual);

            Assert.That(result, Is.False);
        }

        [Test]
        public void WriteMessageTo_should_write_the_fail_reason_to_the_message_if_Matches_failed()
        {
            var actual = new { Name = "Bob", Age = 42 };
            var expected = new { Name = "Bob", Age = 24 };

            var target = new ContainsStateConstraint(expected);

            target.Matches(actual);

            var writer = new TextMessageWriter();
            target.WriteMessageTo(writer);

            Assert.That(writer.ToString(), Is.EqualTo("  /Age: Actual value is not equal to expected value.\r\n  Expected: 24\r\n  But was:  42\r\n"));
        }

        [Test]
        public void WriteMessageTo_should_suppress_call_to_WriteDescriptionTo_if_result_has_a_null_actual_or_expected_value()
        {
            var actual = new[] { 10, 20 };
            var expected = new[] { 10, 20, 30 };

            var target = new ContainsStateConstraint(expected);

            target.Matches(actual);

            var writer = new TextMessageWriter();
            target.WriteMessageTo(writer);

            Assert.That(writer.ToString(), Is.EqualTo("  /: Actual collection (size = 2) is smaller than expected collection.\r\n"));
        }
    }
}