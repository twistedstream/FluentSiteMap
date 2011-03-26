using System.Linq;
using FluentSiteMap.Filters;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class DefaultFilterProviderTests
        : TestBase
    {
        [Test]
        public void GetFilters_should_return_the_expected_filters()
        {
            IDefaultFilterProvider target = new DefaultFilterProvider();

            var results = target.GetFilters().ToList();

            Assert.That(results.Count, Is.EqualTo(1));
            Assert.That(results[0], Is.TypeOf(typeof (CurrentNodeFilter)));
        }
    }
}