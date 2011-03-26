using System.Linq;
using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class ContainsStateTests
        : TestBase
    {
        [Test]
        public void Null_should_return_null()
        {
            Assert.That(ContainsState.Null, Is.Null);
        }

        [Test]
        public void EmptyCollection_should_return_an_empty_collection()
        {
            var result = ContainsState.EmptyCollection.ToList();

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void With_should_return_a_NUnit_constraint_that_perform_a_state_compare()
        {
            var result = ContainsState.With("foo");

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(ContainsStateConstraint)));
        }
    }
}