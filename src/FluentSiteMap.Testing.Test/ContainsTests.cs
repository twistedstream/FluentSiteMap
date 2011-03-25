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
    }
}