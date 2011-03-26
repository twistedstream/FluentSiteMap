using System.Linq;
using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class ContainsTests
        : TestBase
    {
        [Test]
        public void Null_should_return_null()
        {
            Assert.That(Contains.Null, Is.Null);
        }

        [Test]
        public void EmptyCollection_should_return_an_empty_collection()
        {
            var result = Contains.EmptyCollection.ToList();

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void State_should_return_a_NUnit_constraint_that_perform_a_state_compare()
        {
            var result = Contains.State("foo");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(ContainsStateConstraint)));
        }
    }
}