﻿using System.Linq;
using TS.FluentSiteMap.Filters;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest
{
    [TestFixture]
    public class DefaultFilterProviderTests
        : FluentSiteMapTestBase
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